using df;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaMux
{
    public class LanguageFile : LangDfv
    {
        public string Lossless_concat = "Lossless concat";
        public string Lossless_split = "Lossless split";
        public string File_streams = "File streams";
        public string Duration = "Duration";
        public string Dont_change = "Don't change";
        public string Split_Concat = "Split/Concat";
        public string HaveToAddFile = "Please add files first.";
        public string FileName = "File name";
        public string Progressing = "Progressing";
        public string Complete = "Compelete!";
        public string Remove_files = "Remove files";
        public string Clear_all_files = "Clear all files";
        public string Extract_file = "Extract file";
        public string Clear = "Clear";
        public string Container = "Container:";
        public string Start_split = "Start split";
        public string Start_concat = "Start concat";
        public string Muxer = "Multiplexer";
        public string Number_of_files = "Number of files:";
        public string Stopped = "Stopped";

        public string Split_mode = "Split mode:";
        public string Specific_time = "Specific time range(one time point each line)";
        public string After_duration = "After output duration(sec)";
        public string After_file_size = "After output file size(Mbytes)";
        public string Force_close = "A task still on processing, force shutdown?";
        public string Abort_current_task = "Abort current task?";
        public string Stream_name = "Stream name:";
        public string Stream_title = "Stream title:";
        public string Stream_language = "Stream language:";
        public string Delay_ms = "Delay(in ms):";
        public string Delay_sec = "Delay(in sec):";
        public string Start_time = "Start time(sec):";
        public string Select_all = "Select all";
        public string Disable_all = "Disable all";
        public string Default_ = "Default:";
        public string Add_attachment = "Add mkv attachment";
        public string Extract_all_audio = "Extract all audio";
        public string Remove_attachment = "Remove attachment";
        public string View_details = "View details";
        public string File_name = "File name:";
        public string Mimetype = "Mimetype:";
        public string Edit_tags = "Edit tags";
        public string Bit_Rate = "Bit rate(kbits/s):";
        public string Audio_code = "Audio code:(empty for lossless copy)";
        public string Video_code = "Video code:(empty for lossless copy)";
        public string Speed = "Encode speed(Preset):";
        public string Json_error = "JSON format error:";
        public string Crf = "Video quality(crf):";
        public string Pixel_formats = "Pixel formats:";
        public string Destination = "Destination:";
        public string Title_ = "Title:";
        public string AllAudioExtractTo = "Extract All audios to source file's directory?";
        public string Please_input_title = "Please input title!";
        public string Display_wh = "Display size:";
        public string Tune_ = "Tune:";

        public string Double_click_to_select = "Double click to select:";

        public string Convert_all_stream = "Convert all streams";

        public string New_version = "New version";


        public string check_update = "Check update";
        public string check_update_descr = "Check new Release when startup.";

        public string Config = "Config";
        public string Stop = "Stop";
        public string Open_location = "Open location";
        public string Edit_parameters = "Edit parameters";

        public string Command_line = "Command line";

        public string Extract_select_file = "Extract all selected files?";


        public string GUI = "GUI";
        public string Filter = "Filter";
        public string Language_ = "Language:";
        public string Font_size_ = "Font size:";

        public string Frame_rate_ = "Frame rate(FPS):";

        public string Convert_each_file = "Convert each file";

        public string Log = "Log";

        public string language_file = "Language";
        public string language_file_descr = "Change UI language. Need restart.";
        public string font = "Font";
        public string font_descr = "Change UI Font.";



        public string Language = "Language";
        public string Font = "Font";
        public string Subtitle = "Subtitle";
        public string subtitle_font = "subtitle font";
        public string subtitle_font_descr = "";
        public string subtitle_color = "subtitle color";
        public string subtitle_color_descr = "";

        public string subtitle_outline = "subtitle outline";
        public string subtitle_outline_descr = "The value is float number, it specifies the width of the outline around the text.";
        public string subtitle_outline_color = "subtitle outline color";
        public string subtitle_outline_color_descr = "";
        public string subtitle_shadow = "subtitle shadow";
        public string subtitle_shadow_descr = "This specifies the depth of the drop shadow behind the text";
        public string subtitle_shadow_color = "subtitle shadow color";
        public string subtitle_shadow_color_descr = "";
        public string subtitle_transparency = "subtitle transparency";
        public string subtitle_transparency_descr = "0 - 100, 0 = not transparent, 100 = 100% transparent";
        public string subtitle_alignment = "subtitle alignment";
        public string subtitle_alignment_descr = @"This sets how text is 'justified' within the Left/Right onscreen margins. Values may be 1=Left, 2=Centered, 3=Right. 4 = 'Toptitle', 5 = left-justified toptitle, 6=Top centered 8 = 'Midtitle'.";
        public string margin_vertical = "margin vertical";
        public string margin_vertical_descr = @"This defines the vertical Margin in pixels. For a subtitle, it is the distance from the bottom of the screen.
For a toptitle, it is the distance from the top of the screen.
For a midtitle, the value is ignored - the text will be vertically centred";

        public string override_ass = "override ass font";
        public string override_ass_descr = "Force override Advanced SubStation Alpha (ASS) font.";
    }
}
