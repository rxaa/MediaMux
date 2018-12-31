using df;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaMux
{
    public partial class FormMain : Form
    {

        List<string> files = new List<string>();
        List<FFmpeg> ffs = new List<FFmpeg>();
        List<MediaStream> streamsCurrent = new List<MediaStream>();

        int currentStreamIndex = 0;

        ConvertMedia convert = new ConvertMedia();


        void initFont()
        {
            com.initFont(menuStrip1);
            com.initFont(contextMenuStripFile);
            com.initFont(contextMenuStripStream);
            com.initFont(richTextBoxLog);
        }
        void init()
        {
            this.SizeChanged -= FormMain_SizeChanged;
            com.init(this);
            AppLanguage.InitLanguage(contextMenuStripFile);
            AppLanguage.InitLanguage(contextMenuStripStream);
            initFont();
            if (com.cfg.dat.winHeight != 0 && com.cfg.dat.winWidth != 0)
            {
                this.Size = new Size(com.cfg.dat.winWidth, com.cfg.dat.winHeight);

            }
            this.SizeChanged += FormMain_SizeChanged;
        }


        public FormMain()
        {
            InitializeComponent();

            init();

            this.Text += " " + com.lang.dat.version + " " + com.getVer();
        }



        async private void buttonAdd_Click(object sender, EventArgs e)
        {
            var res = dfv.openFile(r => r.Multiselect = true);
            if (res.Length < 1)
                return;

            foreach (var it in res)
            {
                await addFileItem(it);
            };

        }

        void clear()
        {
            flowLayoutPanelProp.Enabled = false;
            listViewFile.Items.Clear();
            files.Clear();
            ffs.Clear();
            listViewStream.Items.Clear();
            streamsCurrent.Clear();
            currentStreamIndex = 0;
            textBoxDestination.Text = "";
            convertEachRes.Clear();
        }


        string getFileName(string file, string ext)
        {
            return dfv.getFile2(file, ext);
        }

        async Task addFileItem(string name)
        {
            try
            {
                var ff = new FFmpeg();
                await ff.getInfo(name);
                if (textBoxDestination.Text == "")
                {
                    textBoxDestination.Text = getFileName(name, comboBoxPack.Text);
                }

                ff.parameters.fileName = name;
                if (ff.isPic())
                {
                    ff.parameters.loop = "1";
                    ff.parameters.overlay.position = "0";
                    ff.parameters.overlay.shortest = "1";
                }

                ListViewItem lvi = new ListViewItem();

                //lvi.ImageIndex = i;   

                var fileName = Path.GetFileName(name);
                lvi.Text = fileName;
                lvi.SubItems.Add(name);

                listViewFile.Items.Add(lvi);
                ffs.Add(ff);
                files.Add(name);

                addStreams(name, ff.info.streams, ffs.Count - 1);
            }
            catch (Exception er)
            {
                dfv.msgERR(er.Message);
            }
        }

        string getFileCon()
        {
            var sel = "";
            if (comboBoxCon.SelectedIndex > 0)
            {
                sel = comboBoxCon.Items[comboBoxCon.SelectedIndex] + "";
            }
            return sel;

        }

        async private void buttonStart_Click(object sender, EventArgs e)
        {
            if (files.Count == 0)
            {
                dfv.msgERR(com.lang.dat.HaveToAddFile);
                return;
            }


            foreach (var it in files)
            {
                var ff = new FFmpeg();
                progressStart(ff, Path.GetFileName(it));
                var cmd = ff.cutVideoCommand(it, textBoxSplit.Text, dfv.getFile2(it, getFileCon()), comboBoxSplit.SelectedIndex);
                setLog(cmd, ff);
                await ff.exec(cmd);
                progressEnd();
            }
        }


        private void buttonClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        void progressError(string err)
        {
            labelProc.Text = err;
        }
        void progressStart(FFmpeg ff, string name, string extString = "")
        {
            progressBarProc.Value = 0;
            ff.onProc = per =>
            {
                long remain = ff.info.format.durationMilli - dfv.timeStrToLong(ff.processTime);
                float speed = 0;
                try
                {
                    speed = float.Parse(ff.processSpeed.Replace("x", ""));
                }
                catch (Exception)
                {

                }
                if (remain > 0 && speed > 0)
                    remain = (long)(remain / speed);

                this.BeginInvoke(new Action(() =>
                {
                    progressBarProc.Value = per;
                    var info = per + "% ";
                    if (ff.processFPS != "")
                    {
                        info += ff.processFPS + "FPS ";
                    }

                    info += ff.processSpeed;

                    if (remain > 0)
                    {
                        info += " " + dfv.timeToStr(remain);
                    }

                    info += " " + extString + com.lang.dat.Progressing + "..." + name;
                    labelProc.Text = info;
                }));

            };
            labelProc.Text = extString + com.lang.dat.Progressing + "..." + name;
        }

        void progressEnd()
        {
            progressBarProc.Value = 100;
            labelProc.Text = com.lang.dat.Complete;
        }

        async private void ExtractM4aToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (files.Count == 0)
            {
                dfv.msgERR(com.lang.dat.HaveToAddFile);
                return;
            }

            if (!dfv.msgOK(com.lang.dat.AllAudioExtractTo))
            {
                return;
            }


            foreach (var file in ffs)
            {
                try
                {
                    foreach (var stream in file.info.streams)
                    {
                        if (!stream.isAudio())
                            continue;
                        var ff = new FFmpeg();
                        progressStart(ff, Path.GetFileName(file.fileName));
                        await ff.extractFile(file.fileName, stream);
                        progressEnd();
                    };

                }
                catch (Exception err)
                {
                    var str = com.lang.dat.Error + ":" + Path.GetFileName(file.fileName);
                    progressError(str);
                    dfv.msgERR(str + "\r\n" + err.Message);
                }
            };
        }




        HashSet<Control> bindEv = new HashSet<Control>();
        void bindTags(Control con, string tagName, string defa = "")
        {
            if (currentStreamIndex >= streamsCurrent.Count)
                return;

            if (!streamsCurrent[currentStreamIndex].tags2.GetVal(tagName, val => con.Text = val))
            {
                con.Text = defa;
            }

            if (bindEv.Contains(con))
                return;

            con.TextChanged += (object sender, EventArgs ee) =>
            {
                if (currentStreamIndex >= streamsCurrent.Count)
                    return;
                streamsCurrent[currentStreamIndex].tags2[tagName] = con.Text;
            };
            bindEv.Add(con);
        }


        void addStreams(string file, IEnumerable<MediaStream> streams, int fileIndex)
        {
            listViewStream.ItemChecked -= listViewStream_ItemChecked;
            try
            {
                foreach (var s in streams)
                {
                    ListViewItem lvi = new ListViewItem();
                    s.fileIndex = fileIndex;
                    //lvi.ImageIndex = i;    
                    lvi.Checked = s.selected;
                    lvi.Text = s.getText();
                    lvi.SubItems.Add(s.getDurationStr());
                    lvi.SubItems.Add(Path.GetFileName(file));
                    listViewStream.Items.Add(lvi);
                    streamsCurrent.Add(s);
                }
            }
            finally
            {
                listViewStream.ItemChecked += listViewStream_ItemChecked;
            }
        }

        void bindConvert(ConvertMedia ca)
        {

            if (ca.ext != "")
                comboBoxPack.Text = ca.ext;
            comboBoxAudioCode.bindText(() => ca.audio_code);
            comboBoxAudioBitRate.bindText(() => ca.audio_bit_rate);
            comboBoxVideoCode.bindIndex(() => ca.video_codec);
            comboBoxSpeed.bindText(() => ca.preset);
            comboBoxCrf.bindIndex(() => ca.video_crfs);
            comboBoxSize.bindText(() => ca.video_display_size);
            comboBoxPixelFormat.bindText(() => ca.video_pixel_format);
            comboBoxFPS.bindText(() => ca.video_fps);
        }


        private void Form1_Load(object sender, EventArgs e)
        {

            flowLayoutPanelProp.Enabled = false;

            var dd = new Dictionary<string, string>();

            comboBoxCon.Items.AddRange(new string[] { com.lang.dat.Dont_change, "mkv", "mp4" });
            comboBoxCon.SelectedIndex = 0;

            comboBoxSplit.Items.AddRange(new string[] { com.lang.dat.After_duration, com.lang.dat.Specific_time });
            comboBoxSplit.SelectedIndex = 0;

            comboBoxPack.Items.AddRange(ConvertMedia.extVideos);
            comboBoxPack.Items.AddRange(ConvertMedia.extAudios);

            comboBoxPack.SelectedIndex = 0;
            comboBoxIsDefault.Items.AddRange(new string[] { com.lang.dat.No, com.lang.dat.Yes });
            comboBoxIsDefault.SelectedIndex = 0;


            comboBoxAudioCode.Items.Add("");
            comboBoxAudioCode.Items.AddRange(ConvertMedia.codeAudios);
            comboBoxAudioCode.SelectedIndex = 0;

            comboBoxVideoCode.Items.AddRange(ConvertMedia.codecVideosStr);
            comboBoxVideoCode.SelectedIndex = 0;

            initPreset();

            comboBoxAudioBitRate.Items.Add("");
            comboBoxAudioBitRate.Items.AddRange(ConvertMedia.bitRates);

            comboBoxStreamLang.Items.Add("");
            comboBoxStreamLang.Items.AddRange(FFmpeg.streamLangs);

            comboBoxFPS.Items.Add("");
            comboBoxFPS.Items.AddRange(ConvertMedia.FPSlist);

            comboBoxCrf.Items.AddRange(ConvertMedia.getCRF());

            comboBoxSize.Items.Add("");
            comboBoxSize.Items.AddRange(ConvertMedia.codeSizes);

            comboBoxPixelFormat.Items.Add("");
            comboBoxPixelFormat.Items.AddRange(ConvertMedia.codePixelFormats);

            bindConvert(convert);

            ListViewExt lvExt = new ListViewExt(listViewStream);
            lvExt.onSortStart = () =>
            {
                listViewStream.ItemChecked -= listViewStream_ItemChecked;
            };
            lvExt.onSortEnd = (oldI, newI, realI) =>
            {
                if (streamsCurrent.Count > 0)
                {
                    var old = streamsCurrent[oldI];
                    streamsCurrent[oldI] = null;
                    streamsCurrent.Insert(newI, old);
                    streamsCurrent.Remove(null);


                    currentStreamIndex = realI;
                    listViewStream.Items[realI].Selected = true;

                }

                listViewStream.ItemChecked += listViewStream_ItemChecked;

            };


            lvExt.setDrag();


#if DEBUG
            var oldLang = com.lang.dat;
            var oldMenu = com.lang.fileMenu;
            com.lang.dat = new LanguageFile();
            com.lang.fileMenu = AppLanguage.getLangFile(AppLanguage.engLang);
            com.lang.save();
            com.lang.dat = oldLang;
            com.lang.fileMenu = oldMenu;
            com.lang.save();

            genCode();

#endif

        }

        Dictionary<string, string> langProperty = new Dictionary<string, string>();
        void genCode()
        {
            recurObj(new ConvertMedia());
            recurObj(new FileConvertParameter());
            recurObj(new ConfigFile());
            var res = langProperty.JoinStr("\r\n", it =>
              {
                  return "public string " + it.Key + " = \"" + it.Value + "\";";
              });
            File.WriteAllText("lang.txt", res);
        }

        void recurObj(object obj)
        {
            obj.GetType().GetProperties().ForEach(p =>
           {
               var name = p.GetCustomAttribute<DisplayNameDf>();
               var descr = p.GetCustomAttribute<DescriptionDf>();

               if (p.CanWrite == false)
                   return;
               if (name != null)
               {
                   if (!name.DisplayName.Contains(" "))
                       langProperty[name.DisplayName] = name.DisplayName.Replace("_", " ");
               }

               if (descr != null)
               {
                   if (!descr.Description.Contains(" "))
                       langProperty[descr.Description] = "";
               }


               if (p.PropertyType == typeof(string) || p.PropertyType == typeof(int) || p.PropertyType == typeof(Font) || p.PropertyType == typeof(Color))
               {

               }
               else
               {
                   recurObj(p.GetValue(obj));
               }
           });
        }


        async private void buttonMerge_Click(object sender, EventArgs e)
        {
            if (files.Count == 0)
            {
                dfv.msgERR(com.lang.dat.HaveToAddFile);
                return;
            }

            var ff = new FFmpeg();
            foreach (var f in ffs)
            {
                ff.info.format.durationMilli += f.info.format.durationMilli;
            }
            var fileName = dfv.getFile2(files[0], getFileCon());
            try
            {
                progressStart(ff, Path.GetFileName(fileName));
                setLog("", ff);
                await ff.concatVideo(files, fileName);
                progressEnd();
            }
            catch (Exception err)
            {
                var str = com.lang.dat.Error + ":" + Path.GetFileName(fileName);
                progressError(str);
                dfv.msgERR(str + "\r\n" + err.Message);
            }
        }

        private void AddFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.buttonAdd_Click(sender, e);
        }



        async private void ExtractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (streamsCurrent.Count < 1)
            {
                return;
            }
            if (!dfv.msgOK(com.lang.dat.Extract_select_file))
            {
                return;
            }
            for (int i = 0; i < listViewStream.SelectedIndices.Count; i++)
            {

                var index = listViewStream.SelectedIndices[i];
                var stre = streamsCurrent[index];
                if (stre.isAttachmentFile)
                    continue;

                var ffm = ffs[stre.fileIndex];
                var fileName = files[stre.fileIndex];
                try
                {
                    var ff = new FFmpeg();
                    progressStart(ff, stre.getText());
                    await ff.extractFile(fileName, stre);
                    progressEnd();
                }
                catch (Exception err)
                {
                    var str = com.lang.dat.Error + ":" + fileName;
                    progressError(str);
                    dfv.msgERR(str + "\r\n" + err.Message);
                }
            }
        }

        private void ClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clear();
        }



        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            com.cfg.save();
            if (CommandTask.inst_ != null)
            {
                if (dfv.msgOK(com.lang.dat.Force_close))
                {
                    FFmpeg.kill();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void listViewStream_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            var itemI = e.Item.Index;
            var stre = streamsCurrent[itemI];

            stre.selected = e.Item.Checked;
        }

        void setLog(string str, FFmpeg ff)
        {
            convertEachRes.Clear();
            richTextBoxLog.Text = str;

            var procStr = "";
            ff.onResLine = line =>
            {
                if (ff.isProcessing(line))
                {
                    procStr = line;
                    return;
                }
                if (procStr != "")
                {
                    line = procStr + "\r\n" + line;
                    procStr = "";
                }
                this.BeginInvoke(new Action(() =>
                {
                    richTextBoxLog.Text += "\r\n" + line;
                    //将光标位置设置到当前内容的末尾
                    richTextBoxLog.SelectionStart = richTextBoxLog.Text.Length;
                    //滚动到光标位置
                    richTextBoxLog.ScrollToCaret();
                }));

            };
        }

        async private void buttonStartPack_Click(object sender, EventArgs e)
        {
            if (streamsCurrent.Count < 1 || files.Count < 1)
            {
                dfv.msgERR(com.lang.dat.HaveToAddFile);
                return;
            }

            var convertAll = checkBoxConvertAllAudio.Checked ? convert : null;
            var fileName = textBoxDestination.Text;

            try
            {
                var ff = new FFmpeg();
                progressStart(ff, Path.GetFileName(fileName));
                var cmd = ff.muxerCommand(ffs, streamsCurrent, fileName, convertAll);
                setLog(cmd, ff);
                await ff.exec(cmd);
                progressEnd();
            }
            catch (Exception err)
            {
                removeEmptyFile(fileName);
                var str = com.lang.dat.Error + ":" + Path.GetFileName(fileName);
                progressError(str);
                dfv.msgERR(err.Message);
            }
        }




        private void selectallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (streamsCurrent.Count < 1)
            {
                return;
            }

            for (int i = 0; i < listViewStream.Items.Count; i++)
            {
                listViewStream.Items[i].Checked = true;
            }
        }

        private void disableallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (streamsCurrent.Count < 1)
            {
                return;
            }

            for (int i = 0; i < listViewStream.Items.Count; i++)
            {
                listViewStream.Items[i].Checked = false;
            }
        }





        private void addattatchmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var res = dfv.openFile(r => r.Multiselect = true);
            if (res.Length < 1)
                return;

            foreach (var name in res)
            {
                var ff = FFmpeg.getAttachment(name);
                addStreams(name, new MediaStream[] { ff }, 0);
            };
        }

        private void viewdetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentStreamIndex >= streamsCurrent.Count)
                return;

            var steam = streamsCurrent[currentStreamIndex];


            var str = steam.streamObj.ToString(Formatting.Indented);
            var formD = new FormDetails();
            formD.setText(str);
            formD.Show();

        }

        private void removeattachmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentStreamIndex >= streamsCurrent.Count)
                return;

            var st = streamsCurrent[currentStreamIndex];
            if (st.isAttachmentFile)
            {
                streamsCurrent.RemoveAt(currentStreamIndex);
                listViewStream.Items.RemoveAt(currentStreamIndex);
                currentStreamIndex = 0;
            }
        }

        private void edittagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentStreamIndex >= streamsCurrent.Count)
                return;

            var st = streamsCurrent[currentStreamIndex];
            var json = JsonConvert.SerializeObject(st.tags2, Formatting.Indented);
            var edit = new FormEdit();
            edit.verifyJson = true;
            edit.EditText = json;
            if (edit.StartEdit())
            {
                st.tags2 = JsonConvert.DeserializeAnonymousType(edit.EditText, st.tags2);
            }
        }

        private void edittagsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listViewFile.SelectedIndices.Count < 1)
                return;

            var index = listViewFile.SelectedIndices[0];

            var ff = ffs[index];


            var edit = new FormEdit();
            edit.setJSON(ff.info.format.tags);
            if (edit.StartEdit())
            {
                ff.info.format.tags = JsonConvert.DeserializeAnonymousType(edit.EditText, ff.info.format.tags);
            }

        }


        private void checkBoxConvertAllAudio_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxConvertAllAudio.Checked)
            {
                bindConvert(convert);
            }
        }

        private void listViewStream_Click(object sender, EventArgs e)
        {
            if (listViewStream.SelectedIndices.Count < 1)
                return;

            flowLayoutPanelProp.Enabled = true;
            currentStreamIndex = listViewStream.SelectedIndices[0];
            var stream = streamsCurrent[currentStreamIndex];

            bindTags(textBoxStreamName, "title");
            bindTags(comboBoxStreamLang, "language");

            comboBoxIsDefault.bindIndex(() => stream.disposition2.defaul);


            if (!checkBoxConvertAllAudio.Checked)
            {
                bindConvert(stream.convert);
            }
        }

        private void listViewStream_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void FormMain_SizeChanged(object sender, EventArgs e)
        {
            var res = Size;
            com.cfg.dat.winWidth = res.Width;
            com.cfg.dat.winHeight = res.Height;

        }



        private void viewdetailsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listViewFile.SelectedIndices.Count < 1)
                return;

            var index = listViewFile.SelectedIndices[0];

            var ff = ffs[index];

            var str = JsonConvert.SerializeObject(ff.infoObj, Formatting.Indented);
            var formD = new FormDetails();
            formD.setText(str);
            formD.Show();
        }

        void changeExt(string ext)
        {
            convert.ext = ext;

            if (textBoxDestination.Text != "")
            {
                textBoxDestination.Text = getFileName(textBoxDestination.Text, ext);
                return;
            }

            if (files.Count < 1)
                return;

            textBoxDestination.Text = getFileName(files[0], ext);
        }

        private void comboBoxPack_SelectedIndexChanged(object sender, EventArgs e)
        {
            changeExt(comboBoxPack.Text);
        }

        private void comboBoxCon_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void buttonCfgList_Click(object sender, EventArgs e)
        {


            var codeList = new FormEditTitle();
            codeList.setJSON(convert);
            if (!codeList.startSelect())
                return;


            if (checkBoxConvertAllAudio.Checked)
            {
                this.convert = codeList.convert;
                bindConvert(convert);
                return;
            }


            if (currentStreamIndex >= streamsCurrent.Count)
                return;

            var stream = streamsCurrent[currentStreamIndex];
            stream.convert = codeList.convert;
            bindConvert(stream.convert);
        }

        private void RemoveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewFile.SelectedIndices.Count < 1)
                return;

            var index = listViewFile.SelectedIndices[0];

            var ff = ffs[index];
            ffs.RemoveAt(index);
            files.RemoveAt(index);
            listViewFile.Items.RemoveAt(index);
            currentStreamIndex = 0;
            for (int i = streamsCurrent.Count - 1; i >= 0; i--)
            {
                var st = streamsCurrent[i];

                if (st.fileIndex == index)
                {
                    streamsCurrent.RemoveAt(i);
                    listViewStream.Items.RemoveAt(i);
                    continue;
                }

                if (st.fileIndex > index)
                {
                    st.fileIndex--;
                    listViewStream.Items[i].Text = st.getText();
                }
            }

        }

        private void listViewFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (convertEachRes.Count < 1)
                return;
            if (listViewFile.SelectedIndices.Count < 1)
                return;

            var index = listViewFile.SelectedIndices[0];

            if (index >= convertEachRes.Count)
                return;
            richTextBoxLog.Text = convertEachRes[index];

        }


        string oldCmd = "";
        string newCmd = "";

        async private void commandlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (streamsCurrent.Count < 1 || files.Count < 1)
            {
                dfv.msgERR(com.lang.dat.HaveToAddFile);
                return;
            }

            var convertAll = checkBoxConvertAllAudio.Checked ? convert : null;
            var fileName = textBoxDestination.Text;

            try
            {

                var ff = new FFmpeg();
                var str = ff.muxerCommand(ffs, streamsCurrent, fileName, convertAll);

                var cl = new FormCommand();
                if (str == oldCmd && newCmd != "")
                    cl.EditText = newCmd;
                else
                    cl.EditText = str;


                cl.ShowDialog();

                if (cl.start)
                {
                    oldCmd = str;
                    newCmd = cl.EditText;
                    progressStart(ff, Path.GetFileName(fileName));
                    setLog(cl.EditText, ff);
                    await ff.exec(cl.EditText);
                    progressEnd();
                }
            }
            catch (Exception err)
            {
                var str = com.lang.dat.Error + ":" + Path.GetFileName(fileName);
                progressError(str);
                dfv.msgERR(err.Message);
            }

        }

        private void editparamsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewFile.SelectedIndices.Count < 1)
                return;

            var index = listViewFile.SelectedIndices[0];

            var ff = ffs[index];

            //var edit = new FormEdit();
            //edit.setJSON(ff.parameters);
            //if (edit.StartEdit())
            //{
            //    ff.parameters = JsonConvert.DeserializeAnonymousType(edit.EditText, ff.parameters);
            //}

            var edit = new FormEditProperty();
            edit.setObj(ff.parameters, ff);
            if (edit.StartEdit())
            {

                //ff.parameters = JsonConvert.DeserializeAnonymousType(edit.EditText, ff.parameters);
            }

            if (ff.parameters.concat != "" || ff.parameters.overlay.position != "" || ff.parameters.mix_audio != "")
            {
                if (!ff.isPic())
                    disableFile(index);
            }
        }
        void disableFile(int index)
        {
            for (var i = 0; i < streamsCurrent.Count; i++)
            {
                var st = streamsCurrent[i];
                if (st.fileIndex == index)
                {
                    st.selected = false;
                    listViewStream.Items[i].Checked = false;
                }
            }
        }

        private void openlocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewFile.SelectedIndices.Count < 1)
                return;

            var index = listViewFile.SelectedIndices[0];

            var ff = ffs[index];

            var path = Path.GetDirectoryName(ff.fileName);
            System.Diagnostics.Process.Start("Explorer", "/select," + Path.GetDirectoryName(ff.fileName) + "\\" + Path.GetFileName(ff.fileName));
        }

        async private void saveasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (streamsCurrent.Count < 1 || listViewStream.SelectedIndices.Count < 1)
            {
                return;
            }
            var indx = listViewStream.SelectedIndices[0];
            var st = streamsCurrent[indx];
            var file = files[st.fileIndex];
            var toFile = dfv.saveFile(pa =>
              {
                  pa.FileName = FFmpeg.getStreamFileName(file, st);
              });
            if (toFile != "")
            {
                try
                {
                    var ff = new FFmpeg();
                    progressStart(ff, Path.GetFileName(toFile));
                    setLog("", ff);
                    await ff.extractFile(file, st, toFile);
                    progressEnd();
                }
                catch (Exception err)
                {
                    var str = com.lang.dat.Error + ":" + Path.GetFileName(toFile);
                    progressError(str);
                    dfv.msgERR(str + "\r\n" + err.Message);
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var toFile = dfv.saveFile(pa =>
            {
                pa.FileName = textBoxDestination.Text;
            });

            if (toFile != "")
            {
                textBoxDestination.Text = toFile;
            }
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fp = new FormPreference();
            fp.ShowDialog();
            com.initFont(this);
            initFont();
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonStartPack_Click(sender, e);
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (CommandTask.inst_ != null)
            {
                if (dfv.msgOK(com.lang.dat.Abort_current_task))
                {
                    FFmpeg.kill();
                }
            }
        }

        void removeEmptyFile(string file)
        {
            try
            {
                System.IO.FileInfo info = new System.IO.FileInfo(file);
                if (info.Length == 0)
                {
                    File.Delete(file);
                }
            }
            catch (Exception)
            {
            }
        }

        List<string> convertEachRes = new List<string>();
        async private void converteachfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (files.Count < 1)
            {
                dfv.msgERR(com.lang.dat.HaveToAddFile);
                return;
            }

            convertEachRes.Clear();

            var successCount = 0;
            for (var ind = 0; ind < ffs.Count; ind++)
            {
                var f = ffs[ind];
                var fileName = dfv.getFile2(f.fileName, convert.ext);
                var execRes = "";
                try
                {
                    var ff = new FFmpeg();
                    progressStart(ff, Path.GetFileName(fileName), (ind + 1) + "/" + ffs.Count + " ");
                    var cmd = ff.muxerCommand(f, FFmpeg.getAllStreams(f), fileName, convert);
                    execRes = cmd + "\r\n";
                    ff.onResLine = line =>
                    {
                        if (ff.isProcessing(line))
                            return;
                        this.BeginInvoke(new Action(() =>
                        {
                            execRes += "\r\n" + line;
                        }));

                    };
                    await ff.exec(cmd);
                    successCount++;
                    listViewFile.Items[ind].ForeColor = Color.Black;
                }
                catch (Exception err)
                {
                    removeEmptyFile(fileName);
                    var str = com.lang.dat.Error + ":" + Path.GetFileName(fileName);
                    listViewFile.Items[ind].ForeColor = Color.Red;
                    if (err is ExceptionFFmpeg)
                    {
                        if ((err as ExceptionFFmpeg).userAbort)
                        {
                            break;
                        }
                    }
                }
                convertEachRes.Add(execRes);
            }
            progressBarProc.Value = 100;
            labelProc.Text = successCount + " " + com.lang.dat.Complete + " "
                + (ffs.Count - successCount) + " " + com.lang.dat.Failed + "!";
            FFmpeg.getAllStreams(ffs);

        }


        void initPreset()
        {
            if (comboBoxSpeed.Items.Count > 0)
                comboBoxSpeed.SelectedIndex = 0;

            comboBoxSpeed.Items.Clear();
            comboBoxSpeed.Items.AddRange(ConvertMedia.getPreset(comboBoxVideoCode.Text));
        }
        private void comboBoxVideoCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            initPreset();
        }

        private async void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var fd = new FormDetails();
            fd.Text = com.lang.dat.About;
            var f = new Font(this.Font.FontFamily, this.Font.Size + 1, FontStyle.Bold);
            fd.addText("MediaMux " + com.lang.dat.version + " " + com.getVer() + "\r\n", Color.Black, f);

            fd.addText("Email:\r\n", Color.Black, f);
            fd.addText("zyxdde@gmail.com\r\n\r\n", Color.Blue);

            fd.addText(await FFmpeg.getVersion(), Color.Black);

            fd.ShowDialog();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fw = new FormWeb();
            fw.Text = com.lang.dat.Help;
            fw.setUrl(Application.StartupPath + "\\help\\help_zh-CN.html");
            fw.Show();
        }

        private void listViewFile_DragEnter(object sender, DragEventArgs e)
        {
            if (e.AllowedEffect == DragDropEffects.Move)
                return;
            e.Effect = DragDropEffects.Copy;

        }

        async private void listViewFile_DragDrop(object sender, DragEventArgs e)
        {
            if (e.AllowedEffect == DragDropEffects.Move)
                return;
            e.Effect = DragDropEffects.Copy;
            String[] str_Drop = (String[])e.Data.GetData(DataFormats.FileDrop, true);
            foreach (var s in str_Drop)
            {
                await addFileItem(s);
            }
        }

        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fp = new FormPlayer();
            fp.Show();
        }

        private void previewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var index = listViewFile.SelectedIndices[0];


            if (index < 0)
                return;

            var ff = ffs[index];

            var fp = new FormPlayer();
           

            fp.setFilters(ff.parameters.filters.getCMD(), ff.parameters.audio_filters.getCMD());
            fp.play(ff.fileName);

            fp.ShowDialog();
        }
    }
}
