using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace df
{

    public class ExceptionFFmpeg : Exception
    {
        public bool userAbort { get; set; } = false;
        public ExceptionFFmpeg(string msg, bool abort = false) : base(msg)
        {
            userAbort = abort;
        }
    }

    public class FFmpeg
    {

        public MediaFile info { get; set; } = new MediaFile();
        public MediaFileObj infoObj { get; set; } = new MediaFileObj();

        public FileConvertParameter parameters { get; set; } = new FileConvertParameter();

        public string fileName { get; set; } = "";

        static string ffmp = @"ffmpeg\ffmpeg.exe";
        static string ffprobe = @"ffmpeg\ffprobe.exe";


        public bool isPic()
        {
            return info.streams.Count == 1 && info.streams[0].isPic();
        }

        public static string[] streamLangs = new string[] { "und", "eng", "chi", "jpn", "mul", "ger", "ita", "fre", "fin", "rus", "swe" };
        static Dictionary<string, string> codeExt = new Dictionary<string, string>(){
            //video
            { "hevc", "mp4" },
            { "h263", "3gp" },
            { "h264", "mp4" },
            { "h265", "mp4" },
            { "avc", "mp4" },
            { "mpeg1" , "mpg" },
            { "mpeg1video" , "mpg" },
            { "mpeg2" , "mpg" },
            { "mpeg2video" , "mpg" },
            { "mpeg4", "mp4" },
            { "msmpeg4v3", "avi" },
            { "vp6f", "webm" },
            { "vp6", "webm" },
            { "vp7", "webm" },
            { "vp8", "webm" },
            { "vp9", "webm" },
            { "wmv2", "wmv" },
            { "wmv3", "wmv" },
            { "rv10", "rm" },
            { "rv20", "rm" },
            { "rv30", "rm" },
            { "rv40", "rm" },
            { "rv60", "rm" },
            { "flv1" , "flv" },
            { "flv2" , "flv" },
            { "flv3" , "flv" },
            { "flv4" , "flv" },
            { "rawvideo" , "avi" },
            
           

            ///audio
            { "aac", "m4a" },
            { "ac3", "m4a" },
            { "vorbis", "ogg" },
            { "opus", "ogg" },
            { "cook", "ra" },
            { "wmav2", "wma" },
            { "wmav", "wma" },
            { "pcm_u8", "wav" },

            { "pcm_s16be", "wav" },
            { "pcm_s32be", "wav" },
            { "pcm_s16le", "wav" },
            { "pcm_s32le", "wav" },
            { "pcm_bluray", "flac" },
            { "amr_nb", "amr" },
            { "amr_wb", "amr" },

            //subtitle
            { "subrip", "srt" },
            { "hdmv_pgs_subtitle", "sup" },
            { "dvd_subtitle", "sub" },
            { "dvb_subtitle", "sub" },
            { "webvtt", "vtt" },
            { "sami", "smi" },
            { "mov_text", "srt" },

            { "text", "txt" },
            { "ansi", "txt" },
            { "mjpeg", "jpg" },
        };




        public Action<int> onProc { get; set; } = null;


        public static MediaStream getAttachment(string file)
        {
            var ext = dfv.getFileExt(file);
            var name = Path.GetFileName(file);
            var s = new MediaStream();
            s.codec_long_name = file;
            s.codec_name = MimeType.GetMimeType(ext);
            s.codec_type = "attachment";
            s.tagsEdit["filename"] = name;
            s.tagsEdit["mimetype"] = s.codec_name;
            s.isAttachmentFile = true;
            s.fileIndex = 0;
            return s;
        }

        public static MediaStream getAvs(string file)
        {
            var ext = dfv.getFileExt(file);
            var name = Path.GetFileName(file);
            var s = new MediaStream();
            s.codec_long_name = "avs";
            s.codec_name = "avs";
            s.codec_type = "video";
            s.fileIndex = 0;
            return s;
        }


        public async Task getInfo(string file)
        {
            if (file == "")
                throw new ExceptionFFmpeg(dfv.lang.dat.Please_add_file_name);

            fileName = file;
            var ext = dfv.getFileExt(file);
            if (ext == "avs")
            {
                info.format.format_name = "avs";
                info.streams.Add(getAvs(file));
                return;
            }

            CommandTask command = new CommandTask();
            var json = "";

            command.onRes = (dat) =>
            {
                json += dat;
            };
            await command.exec(ffprobe, " -print_format json -v quiet -show_error -show_format -show_streams -show_chapters  \"" + file + "\" ");

            infoObj = JsonConvert.DeserializeObject<MediaFileObj>(json);
            info = infoObj.toMediaFile();

            if (infoObj.error.message != "")
                throw new ExceptionFFmpeg(infoObj.error.message);

            info.format.durationMilli = dfv.timeStrToLong(info.format.duration);
        }


        string hasAllMap(string file)
        {
            //-map 0 for all stream
            var map = " -map 0 ";
            if (dfv.getFileExt(file) == "mp4")
                map = "";

            return map;
        }

        public string cutVideoCommand(string file, string time, string toFile, int mode)
        {
            if (toFile == "")
                throw new ExceptionFFmpeg(dfv.lang.dat.Please_add_file_name);

            if (time == "")
            {
                throw new ExceptionFFmpeg(dfv.lang.dat.Please_input_time);
            }

            var fileName = dfv.removeFileExt(toFile);
            var ext = dfv.getFileExt(toFile);

            var newFile = fileName + "_%d" + "." + ext;
            var cmd = "";
            if (mode == 1)
            {
                var times = time.Replace("\r", "").Split('\n');
                if (times.Length < 1)
                {
                    throw new ExceptionFFmpeg(dfv.lang.dat.Please_input_time);
                }

                var points = times.JoinStr(",", it => it);

                cmd = " -i  \"" + file + "\" " + hasAllMap(toFile) + " -codec copy -f segment -segment_times " + points + " -reset_timestamps 1 -y \"" + newFile + "\" ";
                return cmd;
            }


            cmd = " -i  \"" + file + "\" " + hasAllMap(toFile) + " -codec copy -f segment -segment_time " + time + " -reset_timestamps 1 -y \"" + newFile + "\" ";

            return cmd;
        }


        string getConcatFile()
        {
            return AppDomain.CurrentDomain.BaseDirectory + "\\concat.txt";
        }


        public Task concatVideo(IEnumerable<string> files, string toFile)
        {
            if (toFile == "")
                throw new ExceptionFFmpeg(dfv.lang.dat.Please_add_file_name);

            var str = "";
            foreach (var s in files)
            {
                str += "file '" + s + "'\r\n";
            }
            File.WriteAllText(getConcatFile(), str);

            //-safe 0
            var cmd = "-f concat -safe 0 -i \"" + getConcatFile() + "\" " + hasAllMap(toFile) + "  -c copy \"" + toFile + "\" ";

            return exec(cmd);
        }


        public static string getStreamExt(MediaStream st)
        {
            var ext = st.codec_name;
            if (codeExt.ContainsKey(ext))
            {
                ext = codeExt[ext];
            }
            return ext;
        }

        public static string getStreamFileName(string file, MediaStream st)
        {
            var toFile = "";
            if (st.codec_type == "attachment")
            {
                toFile = dfv.getFile2(dfv.removeFileExt(file) + "_" + st.getName());
            }
            else
            {
                toFile = dfv.getFile2(file, getStreamExt(st));
            }
            return toFile;
        }

        public Task extractFile(string file, MediaStream st, string toFile = "")
        {
            if (toFile == "")
            {
                toFile = getStreamFileName(file, st);
            }

            var cmd = "-i \"" + file + "\"  -map 0:" + st.index + " -codec ";

            if (st.isMovText())
                cmd += "srt";
            else if (st.codec_name == "pcm_bluray")
            {
                cmd += "flac";
            }
            else
                cmd += "copy";

            cmd += " -y \"" + toFile + "\"";
            if (st.codec_type == "attachment")
            {
                cmd = " -y -dump_attachment:" + st.index + " \"" + toFile + "\" -i \"" + file + "\" ";
            }

            //else if (st.type == "audio")
            //{
            //    cmd = "-i \"" + file + "\"  -map 0:" + stream + " -vn -acodec copy \"" + dfv.getFile2(file, ext) + "\"";
            //}

            return exec(cmd);
        }




        string getMetadata(MediaStream s, int streamI)
        {
            var res = "";
            s.tagsEdit.ForEach(v =>
            {
                if (v.Key != "" && v.Key != null && v.Value != s.tags.GetStrVal(v.Key))
                    res += " -metadata:s:" + streamI + " " + v.Key + "=\"" + v.Value + "\" ";
            });

            return res;
        }

        public static async Task<string> getVersion()
        {
            CommandTask command = new CommandTask();
            command.enableMultiTask = true;
            var res = "";
            var count = 0;
            command.onRes = (dat) =>
            {
                //if (count < 2)
                res += dat + "\r\n";

                count++;
            };
            await command.exec(ffmp, " -version ");

            return res;
        }

        public static List<MediaStream> getAllStreams(FFmpeg files)
        {
            return getAllStreams(new FFmpeg[] { files });
        }
        public static List<MediaStream> getAllStreams(IEnumerable<FFmpeg> files)
        {
            List<MediaStream> list = new List<MediaStream>();
            files.ForEach((ff, index) => ff.info.streams.ForEach(it =>
            {
                it.fileIndex = index;
                list.Add(it);
            }));
            return list;
        }



        readonly string mainAudioStr = "[maina]";
        readonly string subAudioStr = "[suba]";
        string getAudioFilter(string filterMain, IList<FFmpeg> files, AudioFilter extFilters)
        {
            var prevFilter = filterMain;

            var filterAll = "";
            var filter = "";
            Func<string> checkFilter = () =>
            {
                if (filterAll.Length + filter.Length > 0)
                {
                    if (filter.Length > 0)
                        return prevFilter + ";";
                    else
                        return filterMain + ";";
                }
                else
                {
                    return "";
                }
            };
            for (int index = 0; index < files.Count; index++)
            {
                var f = files[index];
                var filterSub = "[" + index + ":a]";

                filter = "";

                if (filterAll != "")
                    filterMain = mainAudioStr;

                var filters = f.parameters.audio_filters.getCMD();
                if (filters != "")
                {
                    filter += checkFilter() + filterSub + filters;
                    //filterMain = mainAudioStr;
                    filterSub = subAudioStr;
                    prevFilter = subAudioStr;
                }


                if (f.parameters.mix_audio != "")
                {
                    filter += checkFilter() + filterMain + filterSub + "amix=duration=";
                    if (f.parameters.mix_shortest != "")
                        filter += "shortest";
                    else
                        filter += "longest";
                    filterMain = mainAudioStr;
                    //filterSub = subStr;
                    prevFilter = mainAudioStr;
                }
                else if (f.parameters.concat != "")
                {
                    filter += checkFilter() + filterMain + filterSub + "concat=v=0:a=1";
                    filterMain = mainAudioStr;
                    //filterSub = subStr;
                    prevFilter = mainAudioStr;
                }
                filterAll += filter;
            }

            filter = "";
            var filterCfg = extFilters.getCMD();
            if (filterCfg != "")
            {
                filterAll += checkFilter() + filterMain + filterCfg;
                filterMain = mainAudioStr;
                //filterSub = subStr;
                prevFilter = mainAudioStr;
            }

            return filterAll;
        }

        readonly string mainStr = "[main]";

        readonly string subStr = "[sub]";



        string getFilter(string filterMain, IList<FFmpeg> files, IEnumerable<string> ext, VideoFilter extFilters)
        {

            //var streamI = 0;

            var prevFilter = filterMain;


            var filterAll = "";
            var filter = "";
            Func<string> checkFilter = () =>
            {
                if (filterAll.Length + filter.Length > 0)
                {
                    if (filter.Length > 0)
                        return prevFilter + ";";
                    else
                        return filterMain + ";";
                }
                else
                {
                    return "";
                    //filter_complex += "[" + st.fileIndex + ":" + st.index + "]";
                }
            };

            for (int index = 0; index < files.Count; index++)
            {
                var f = files[index];

                filter = "";

                if (filterAll != "")
                    filterMain = mainStr;

                var filterSub = "[" + index + ":v]";

                if (f.info.streams.Sum(it => it.isSubtitle() ? 0 : 1) == 0)
                    filterSub = "[" + index + ":s]";

                var filters = f.parameters.filters.getCMD();

                if (filters != "")
                {
                    filter += checkFilter() + filterSub + filters;
                    //filterMain = mainStr;
                    filterSub = subStr;
                    prevFilter = subStr;
                }

                if (f.parameters.fade_in != "" || f.parameters.fade_out != "")
                {
                    var fade = "";
                    fade += "format=yuva420p";
                    var sTime = f.parameters.overlay.start_time != "" ? f.parameters.overlay.start_time : "0";
                    float outTime = 0;
                    if (f.parameters.overlay.end_time != "" && f.parameters.fade_out != "")
                        outTime = (float)(dfv.timeStrToLong(f.parameters.overlay.end_time) - dfv.timeStrToLong(f.parameters.fade_out)) / 1000;

                    float startTime = (float)dfv.timeStrToLong(sTime) / 1000;
                    fade += f.parameters.fade_in != "" ? ",fade=in:st=" + startTime + ":d=" + dfv.strEscape(f.parameters.fade_in) + ":alpha=1" : "";
                    fade += f.parameters.fade_out != "" ? ",fade=out:st=" + outTime + ":d=" + dfv.strEscape(f.parameters.fade_out) + ":alpha=1" : "";
                    filter += checkFilter() + filterSub + fade;
                    //filterMain = mainStr;
                    filterSub = subStr;
                    prevFilter = subStr;
                }


                if (f.parameters.concat != "")
                {
                    filter += checkFilter() + filterMain + filterSub + "concat";
                    filterMain = mainStr;
                    //filterSub = subStr;
                    prevFilter = mainStr;
                }
                else if (f.parameters.overlay.position != "" || f.parameters.overlay.x != "")
                {
                    var x = "0";
                    var y = "0";
                    if (f.parameters.overlay.position == "1")
                    {
                        x = "main_w-overlay_w";
                    }
                    else if (f.parameters.overlay.position == "2")
                    {
                        x = "main_w-overlay_w";
                        y = "main_h-overlay_h";
                    }
                    else if (f.parameters.overlay.position == "3")
                    {
                        y = "main_h-overlay_h";
                    }
                    if (f.parameters.overlay.x != "")
                        x += "+" + f.parameters.overlay.x;
                    if (f.parameters.overlay.y != "")
                        y += "+" + f.parameters.overlay.y;

                    var overlay = "overlay=" + x + ":" + y;

                    if (f.parameters.overlay.start_time != "")
                    {
                        if (f.parameters.overlay.end_time != "")
                            overlay += ":enable='between(t," + dfv.timeStrToLong(f.parameters.overlay.start_time) / 1000 + "," + dfv.timeStrToLong(f.parameters.overlay.end_time) / 1000 + ")'";
                        else
                            overlay += ":enable='gte(t," + dfv.timeStrToLong(f.parameters.overlay.start_time) / 1000 + ")'";
                    }
                    else if (f.parameters.overlay.start_time == "" && f.parameters.overlay.end_time != "")
                    {
                        overlay += ":enable='lte(t," + dfv.timeStrToLong(f.parameters.overlay.end_time) / 1000 + ")'";
                    }
                    if (f.parameters.overlay.shortest != "")
                        overlay += ":shortest=" + f.parameters.overlay.shortest;

                    filter += checkFilter() + filterMain + filterSub + overlay;
                    filterMain = mainStr;
                    //filterSub = subStr;
                    prevFilter = mainStr;
                }

                filterAll += filter;

            }

            filter = "";



            var filterCfg = extFilters.getCMD();
            if (filterCfg != "")
            {
                filterAll += checkFilter() + filterMain + filterCfg;
                filterMain = mainStr;
                //filterSub = subStr;
                prevFilter = mainStr;
            }


            ext.ForEach(str =>
            {
                if (str != "")
                {
                    filterAll += checkFilter() + filterMain + str;
                    filterMain = mainStr;
                    //filterSub = subStr;
                    prevFilter = mainStr;
                }
            });

            return filterAll;
        }

        public string muxerCommand(FFmpeg files, IEnumerable<MediaStream> streams, string toFile, ConvertMedia convertAll = null)
        {
            return muxerCommand(new FFmpeg[] { files }.ToList(), streams, toFile, convertAll);
        }

        public static Func<string, MediaStream, string> getSubtitle = (subName, stream) =>
         {
             var res = "subtitles='" + dfv.strEscape(subName) + "'";
             return res;
         };


        public static string getFilesSubtitle(IList<FFmpeg> files)
        {
            foreach (var ff in files)
            {
                if (ff.info.streams.Count != 1)
                    continue;

                var st = ff.info.streams[0];
                if (st.selected)
                {
                    var fi = files[st.fileIndex].fileName;
                    if (st.isSubtitle() && !st.isImgSubtitle())
                    {
                        return getSubtitle(fi, st);
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="files"></param>
        /// <param name="streams">must assign fileIndex field, reference to getAllStreams()</param>
        /// <param name="toFile"></param>
        /// <returns></returns>
        public string muxerCommand(IList<FFmpeg> files, IEnumerable<MediaStream> streams, string toFile, ConvertMedia convertAll = null)
        {
            if (toFile == "")
                throw new ExceptionFFmpeg(dfv.lang.dat.Please_add_file_name);

            var ext = dfv.getFileExt(toFile);
            if (ext == "")
                throw new ExceptionFFmpeg(dfv.lang.dat.Please_add_extension);

            var videoCount = 0;
            var count = 0;
            List<string> filters = new List<string>();


            var subtitleBurnIn = "";
            streams.ForEach(st =>
            {
                var convert = convertAll;
                if (convert == null)
                    convert = st.convert;
                if (st.selected)
                {
                    count++;
                    if (st.isPureVideo())
                    {
                        videoCount++;
                    }

                    var fi = files[st.fileIndex].fileName;
                    if (st.isSubtitle() && ext != "mkv" && !ConvertMedia.extAudios.Contains(ext) && convert.video_code != "")
                    {
                        if (subtitleBurnIn == "")
                        {
                            if (st.isImgSubtitle())
                                subtitleBurnIn = "[" + st.fileIndex + ":s]overlay";
                            else
                                subtitleBurnIn = getSubtitle(fi, st);
                        }
                    }
                }

            });

            if (count < 1)
                throw new ExceptionFFmpeg(dfv.lang.dat.At_least_one_stream);

            if (subtitleBurnIn != "")
                filters.Add(subtitleBurnIn);

            var cmd = "";

            var inputCmd = "";
            for (int index = 0; index < files.Count; index++)
            {
                var f = files[index];
                if (f.fileName == "")
                {
                    throw new ExceptionFFmpeg(dfv.lang.dat.Could_not_found_FFmpeg_fileName);
                }

                var paras = getFieldAttr(f.parameters, 0);

                inputCmd += " " + paras + " -i \"" + f.fileName + "\" \n";

            }


            var streamI = 0;

            var shortest = "";
            foreach (var st in streams)
            {
                if (!st.selected)
                    continue;

                var convert = convertAll;
                if (convert == null)
                    convert = st.convert;


                var filterMap = "";
                var filterOut = "[out" + streamI + "]";

                if (ConvertMedia.extAudios.Contains(ext) && !st.isAudio())
                {
                    continue;
                }

                if (st.isPureVideo())
                {
                    filterMap = getFilter("[" + st.fileIndex + ":" + st.index + "]", files, filters, convert.video_filters);
                    if (filterMap != "")
                        filterMap += filterOut;
                }

                if (ext != "mkv")
                {
                    if (st.codec_type == "subtitle" && !(st.hasMovText(ext) && convert.video_code == "" && !st.isImgSubtitle()))
                    {
                        continue;
                    }
                    if (st.codec_type == "attachment")
                    {
                        continue;
                    }
                }

                if (st.isPic())
                {
                    if (videoCount == 0)
                    {
                        var fil = getFilter(mainStr, files, filters, convert.video_filters);

                        if (fil != "")
                        {
                            filterMap = "color=c=black,scale=" + getScale(st.width, st.height) + ",setsar=1" + mainStr + ";";
                            filterMap += fil + filterOut;
                        }

                        shortest = " -shortest ";
                        if (convert.video_code == "")
                            convert.video_codec = 1;
                        videoCount++;
                    }
                    else
                    {
                        continue;
                    }

                }

                if (st.isAudio())
                {
                    filterMap = getAudioFilter("[" + st.fileIndex + ":a]", files, convert.audio_filters);
                    if (filterMap != "")
                        filterMap += filterOut;
                }

                var metaData = "";

                if (st.disposition.defaul != st.disposition2.defaul)
                {
                    metaData += " -disposition:" + streamI;
                    metaData += st.disposition2.defaul == 0 ? " 0 " : " default ";
                }

                if (st.isAttachmentFile)
                {
                    cmd += "\n -attach \"" + st.codec_long_name + "\" ";

                    cmd += getMetadata(st, streamI);
                }
                else
                {
                    if (filterMap != "")
                    {
                        cmd += "\n -filter_complex \"" + filterMap + "\" ";
                        cmd += "\n -map " + filterOut;
                    }
                    else
                    {
                        cmd += "\n -map " + st.fileIndex + ":" + st.index;
                    }



                    metaData += getMetadata(st, streamI);

                    string extraParas = "";

                    if (convertAll != null && convertAll.video_2pass == "1")
                    {
                        extraParas = "pass=1";
                    }

                    cmd += getCodec(st, streamI, convertAll, ext, extraParas) + " " + metaData;

                }
                streamI++;
            }




            if (convertAll != null && convertAll.command_line != "")
            {
                cmd += "\n" + convertAll.command_line + " ";
            }

            cmd += "\n " + shortest;

            if (convertAll != null && convertAll.video_2pass == "1")
            {
                var oldCmd = cmd;
                var passFormat = convertAll.video_code.IndexOf("vp9") >= 0 ? "webm" : "mp4";

                if (convertAll.video_code == "hevc")
                {
                    oldCmd = oldCmd.Replace("pass=1", "pass=2");
                }

                cmd += " -pass 1 -an  -f " + passFormat + " -y NUL \n\n&&\n\n" + inputCmd + oldCmd + " \n -pass 2";
            }

            //var pass1 = cmd + " -pass 1 -f " + ext + " -y NUL";
            //var psss2 = cmd + " -pass 2  \"" + toFile + "\" ";
            //await exec(inputCmd + pass1);
            //await exec(inputCmd + psss2);



            cmd += "\n\n -strict -2 -y \"" + toFile + "\" ";

            return inputCmd + cmd;
        }




        string getCodec(MediaStream st, int streamI, ConvertMedia convertAll, string ext, string extra265Paras = "")
        {
            var cmd = " -codec:" + streamI + " ";

            var convert = st.convert;
            if (convertAll != null)
                convert = convertAll;

            if (convert.audio_code != "" && st.isAudio())
            {
                cmd += convert.audio_code;


                if (convert.audio_bit_rate != "")
                    cmd += " -ab:" + streamI + " " + convert.audio_bit_rate + "k ";
                else if (convert.audio_quality != "")
                    cmd += " -aq:" + streamI + " " + convert.audio_quality + " ";
                else
                    cmd += " -aq:" + streamI + " 2 ";


                if (convert.audio_channels != "")
                {
                    cmd += " -ac:" + streamI + " " + convert.audio_channels + " ";
                }

                if (convert.audio_sample_rate != "")
                {
                    cmd += " -ar:" + streamI + " " + convert.audio_sample_rate + " ";
                }

            }
            else if (convert.video_code != "" && st.isVideo())
            {
                cmd += convert.video_code;
            }
            else
            {
                if (st.isSubtitle() && !st.isImgSubtitle() && st.hasMovText(ext))
                    cmd += " mov_text ";
                else if (ext == "mkv" && st.isMovText())
                    cmd += " srt ";
                else
                    cmd += " copy ";
            }

            if (st.isVideo())
            {
                if (convert.video_crfs > 0)
                {
                    var crf = convert.video_crfs - 1;
                    if (convert.video_code.IndexOf("nvenc") > 0)
                        cmd += " -qp:" + streamI + " " + crf + " ";
                    else if (convert.video_code.IndexOf("qsv") > 0)
                        cmd += "  -global_quality:" + streamI + " " + crf + " ";
                    else if (convert.video_code.IndexOf("vp9") >= 0)
                        cmd += "  -crf:" + streamI + " " + crf + " -b:v:" + streamI + " 0 ";
                    else
                        cmd += "  -crf:" + streamI + " " + crf + " ";

                }
                else if (convert.video_bit_rate != "")
                {
                    var rate = int.Parse(convert.video_bit_rate);

                    cmd += "  -b:v:" + streamI + " " + rate + "k ";
                }

                if (convert.video_max_bit_rate != "")
                {
                    cmd += " -maxrate:v:" + streamI + " " + convert.video_max_bit_rate + "k ";

                    var buf_size = convert.video_buffer_size;
                    if (buf_size == "")
                        buf_size = convert.video_max_bit_rate + "";


                    cmd += " -bufsize " + buf_size + "k ";

                }

                if (convert.preset != "")
                {
                    if(convert.video_code == "vp9")
                    {
                        cmd += "  -deadline:" + streamI + " " + convert.preset + " ";
                    }
                    else
                    {
                        cmd += "  -preset:" + streamI + " " + convert.preset + " ";
                    }
                }


                if (convert.video_display_size != "")
                {
                    var size = convert.video_display_size;
                    if (convert.video_display_size.Contains("-1"))
                    {
                        var whs = convert.video_display_size.Replace(" ", "").Split(':');
                        if (whs.Length == 2)
                        {
                            int w = int.Parse(whs[0]);
                            int h = int.Parse(whs[1]);
                            if (whs[0] == "-1")
                            {
                                if (st.height != 0)
                                    w = h * st.width / st.height;
                            }
                            else if (whs[1] == "-1")
                            {
                                if (st.width != 0)
                                    h = w * st.height / st.width;
                            }
                            size = getScale(w, h);
                        }
                    }
                    cmd += " -s:" + streamI + " " + size + " ";
                }

                cmd += getFieldAttr(convert, streamI);


                if (convert.video_code == "h264")
                {
                    var paras = convert.x26x_params.GetType().GetProperties().JoinStr(":", it => FileConvertParameter.getFieldStr(it, convert.x26x_params));
                    if (paras != "")
                        cmd += " -x264-params:" + streamI + " \"" + paras + "\" ";
                }
                else if (convert.video_code == "hevc")
                {
                    var vals = convert.x26x_params.GetType().GetProperties().Select(it => FileConvertParameter.getFieldStr(it, convert.x26x_params, true)).
                        Concat(convert.x265_params.GetType().GetProperties().Select(it => FileConvertParameter.getFieldStr(it, convert.x265_params, true)));

                    if (extra265Paras != "")
                    {
                        vals = new string[] { extra265Paras }.Concat(vals);
                    }

                    var paras = vals.JoinStr(":", it => it);
                    if (paras != "")
                    {
                        cmd += " -x265-params:" + streamI + " \"" + paras + "\" ";

                    }
                }

            }

            if (convert.command_line != "" && convertAll == null)
            {
                cmd += " " + convert.command_line + " ";
            }

            return cmd;
        }

        static string getScale(int w, int h)
        {
            w = w & (~1);
            h = h & (~1);
            return w + "x" + h;
        }




        string getFieldAttr(object convert, int index)
        {
            var cmd = "";
            var fields = convert.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < fields.Length; i++)
            {
                var field = fields[i];
                var att = field.GetCustomAttribute<ConvertAttribute>();
                var val = field.GetValue(convert);
                if (att != null && val != null && val.ToString() != "")
                {
                    if (att.hasIndex)
                        cmd += " " + att.name + ":" + index + " " + val + " ";
                    else if (att.name != "")
                        cmd += " " + att.name + " " + val + " ";
                }
            }
            return cmd;
        }


        string getBracketStr(string s)
        {
            var l = s.IndexOf('(');
            var r = s.IndexOf(')');
            if (l >= 0 && r >= 0 && r > l)
            {
                return s.Substring(l + 1, r - l - 1);
            }
            return "";
        }

        private volatile int processPercent = 0;


        private void parseInfo(string s)
        {
            if (s.StartsWith("  Duration:"))
            {
                var strs = s.Split(',');
                if (strs.Length == 3)
                {
                    var du = strs[0].Substring(12);
                    if (du != "N/A")
                    {
                        var dl = dfv.timeStrToLong(du);
                        if (dl > info.format.durationMilli)
                        {
                            info.format.durationMilli = dl;
                            info.format.duration = du;
                        }

                    }
                    //bitrate = strs[2].Substring(10);
                }
            }
        }

        public bool isProcessing(string dat)
        {
            return dat.StartsWith("size=") || dat.StartsWith("frame=");
        }

        public volatile string processSpeed = "";
        public volatile string processFPS = "";
        public volatile string processTime = "";
        private void onProcess(string dat, int nPass)
        {
            var duration = info.format.durationMilli;
            if (isProcessing(dat) && duration > 0)
            {
                var speedI = dat.IndexOf("speed=");
                if (speedI >= 0)
                {
                    processSpeed = dat.Substring(speedI + 6).Replace(" ", "");
                    if (nPass > 0)
                    {
                        processSpeed += " " + (nPass + 1) + "-pass";
                    }
                }

                var fpsI = dat.IndexOf("fps=");
                if (fpsI >= 0)
                {
                    processFPS = dat.Substring(fpsI + 4);
                    if (processFPS[0] == ' ')
                        processFPS = processFPS.Substring(1);
                    var space = processFPS.IndexOf(" ");
                    if (space > 0)
                    {
                        processFPS = processFPS.Substring(0, space);
                    }
                }

                var timeI = dat.IndexOf("time=");
                if (timeI < 0)
                    return;

                var time = dat.Substring(timeI + 5, 11);
                processTime = time;
                var mill = dfv.timeStrToLong(time);
                if (duration == 0)
                    processPercent = 0;
                else
                    processPercent = (int)(mill * 100 / duration);

                if (processPercent > 100)
                    processPercent = 100;

                if (onProc != null)
                    onProc(processPercent);
            }


        }


        public Action<string> onResLine = null;

        private async Task execOne(string cmd, int nPass)
        {
#if DEBUG
            dfv.log(cmd);
#endif
            cmd = cmd.Replace("\r", "").Replace("\n", " ");

            cmd += " -hide_banner";


            processPercent = 0;
            processSpeed = "";
            processFPS = "";
            processTime = "";
            string err = "";

            string errPrev = "";
            CommandTask command = new CommandTask();
            command.onErr = (dat) =>
            {

                if (onResLine != null)
                    onResLine(dat);

                parseInfo(dat);

                onProcess(dat, nPass);


                if (dat.StartsWith("[") ||
                dat.StartsWith("Incompatible ") ||
                dat.StartsWith("Error ") ||
                dat.StartsWith("Invalid ") ||
                dat.Contains("[error]") ||
                dat.StartsWith("Unknown ")
                )
                {
                    errPrev += dat + "\r\n";
                }
                else
                {
                    err = dat;
                }
            };
            await command.exec(ffmp, cmd);

            if (command.isUserKill)
            {
                throw new ExceptionFFmpeg(dfv.lang.dat.Task_aborted, true);
            }

            if (command.exitCode != 0)
            {
                var allErr = errPrev + err;
                if (allErr != "At least one output file must be specified")
                    throw new ExceptionFFmpeg(errPrev + err);
            }


            processPercent = 100;
        }

        public async Task exec(string cmd)
        {
            if (cmd == "")
                throw new ExceptionFFmpeg(dfv.lang.dat.Command_cant_empty);


            var ss = Regex.Split(cmd, "&&\n", RegexOptions.IgnoreCase);
            for (int i = 0; i < ss.Length; i++)
            {
                await execOne(ss[i], i);
            }
        }

        public static void kill()
        {
            if (CommandTask.inst_ != null)
            {
                CommandTask.inst_.StartInfo.UserName = "user kill";
                CommandTask.inst_.StandardInput.WriteLine("q");
                for (int i = 0; i < 10; i++)
                {
                    if (CommandTask.inst_ != null)
                        Thread.Sleep(100);
                    else
                        break;
                }

                if (CommandTask.inst_ != null)
                    CommandTask.inst_.Kill();
            }
        }
        ///////////////////////////////////////////
    }
}
