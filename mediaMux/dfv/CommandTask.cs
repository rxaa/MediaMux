using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace df
{
    class CommandTask
    {
        public bool enableMultiTask = false;
        public static volatile Process inst_ = null;

        public int exitCode = 0;
        public Action<string> onRes = (s) =>
        {

        };

        public Action<string> onErr = (s) =>
        {

        };

        public bool isUserKill = false;

       


        public Task exec(string fileName, string cmd)
        {
            if (!enableMultiTask)
            {
                if (inst_ != null)
                    throw new Exception(dfv.lang.dat.Already_has_a_task);
            }

            return Task.Run(() =>
            {
                Process p = new Process();
                if (!enableMultiTask)
                {
                    inst_ = p;
                }
                try
                {
                    p.StartInfo.FileName = fileName;

                    p.StartInfo.UseShellExecute = false;

                    p.StartInfo.Arguments = cmd;    //command
                    p.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                    p.StartInfo.StandardErrorEncoding = Encoding.UTF8;
                    p.StartInfo.UseShellExecute = false;  ////不使用系统外壳程序启动进程
                    p.StartInfo.CreateNoWindow = true;  //不显示dos程序窗口

                    p.StartInfo.RedirectStandardInput = true;

                    p.StartInfo.RedirectStandardOutput = true;

                    p.StartInfo.RedirectStandardError = true;//把外部程序错误输出写到StandardError流中

                    string res = "";
                    string err = "";
                    p.ErrorDataReceived += (send, e) =>
                    {
                        var dat = e.Data;
                        if (dat != null)
                        {
                            err += dat + "\r\n";
                            if (onErr != null)
                                onErr(dat);
                        }

                    };

                    p.OutputDataReceived += (send, e) =>
                    {
                        var dat = e.Data;
                        if (dat != null)
                        {
                            res += dat + "\r\n";
                            if (onRes != null)
                                onRes(dat);
                        }
                    };

                    if (!p.Start())
                    {
                        throw new Exception(fileName + " start error");
                    }

                    p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                    p.BeginErrorReadLine();//开始异步读取
                    p.BeginOutputReadLine();

                    p.WaitForExit();

                    if (p.StartInfo.UserName == "user kill")
                        isUserKill = true;

                    exitCode = p.ExitCode;

                    p.Close();//关闭进程

                    p.Dispose();//释放资源



#if DEBUG
                    if (err != "")
                    {
                        dfv.log(err);
                    }

                    if (res != "")
                    {
                        dfv.log(res);
                    }
#endif

                }
                finally
                {
                    if (!enableMultiTask)
                    {
                        inst_ = null;
                    }
                }

            });

        }
    }
}
