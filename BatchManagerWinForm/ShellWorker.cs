using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CLIAppManagerWinForm
{
    public delegate void LogEventHandler(object sender, LogEventArgs e);

    public class LogEventArgs
    {

        public LogEventArgs()
        {
        }

        public LogEventArgs(string s)
            : this(s, Convert.ToInt32(System.Diagnostics.TraceLevel.Verbose))
        {

        }

        public LogEventArgs(string s, int msgLevel)
        {
            this.LogMessage = s;
            this.MsgLevel = msgLevel;
        }


        public string LogMessage { get; set; }

        public int MsgLevel { get; set; }
    }

    public class LogEvent 
    {
        public event LogEventHandler LoggingEvent;

        public LogEvent()
        {
        }

        public virtual void OnLogEvent(LogEventArgs args)
        {
            LoggingEvent?.Invoke(this, args);
        }
    } 
    
    public class ShellWorker :LogEvent
    {

        private Form1 CallerForm { get; set; }

        public ShellWorker(Form1 callerFrm)
        {
            this.CallerForm = callerFrm;
        }

        private string EXEPath { get; set; }
        private string ExeParams { get; set; }

        //public void CallShell()
        //{
        //    CallShell(this.EXEPath, this.ExeParams);
        //}
        public int CallShell(string exeCommand , string Parameters )
        {
            this.EXEPath = exeCommand;
            this.ExeParams = Parameters;

            //http://ss64.com/nt/cmd.html
            /*
            This function will actually take the shell string and envoke the appropriate process
             passing it the arguments to do the work
            */

            // Initialize the process and its StartInfo properties.
            System.Diagnostics.Process ProcessEXE = new System.Diagnostics.Process();


            try
            {

                ProcessEXE.StartInfo.FileName = exeCommand;


                // Set UseShellExecute to false for redirection.
                //  false if the process should be created directly from the executable file
                ProcessEXE.StartInfo.UseShellExecute = false;
                ProcessEXE.StartInfo.WorkingDirectory = System.Environment.CurrentDirectory;

                //EnableRaisingEvents property indicates whether the component should be notified when the operating system has shut down a process
               
                ProcessEXE.StartInfo.Arguments = Parameters;

                ProcessEXE.StartInfo.RedirectStandardOutput = true;
                ProcessEXE.StartInfo.RedirectStandardError = true;
                ProcessEXE.EnableRaisingEvents = true;
                ProcessEXE.StartInfo.CreateNoWindow = true;
                
                ProcessEXE.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler(ProcessEXE_OutputDataReceived);
                ProcessEXE.ErrorDataReceived+=new System.Diagnostics.DataReceivedEventHandler(ProcessEXE_OutputDataReceived);
              
                ProcessEXE.Start();

                
                // Start the asynchronous read of the sort output stream.
                ProcessEXE.BeginErrorReadLine();
                ProcessEXE.BeginOutputReadLine();

                //The WaitForExit overload is used to make the current thread wait until the associated process terminates
                ProcessEXE.WaitForExit();

                if (ProcessEXE.ExitCode == 0)
                {
                    //  WriteTraceInfo(string.Format("BCP Process exited with exit code {0}", BCPProcessBCPEXE.ExitCode));
                    this.CallerForm.IsCLIProcessRunning = false;
                   // System.Windows.Forms.MessageBox.Show(string.Format(@"Process:{0} completed , Returned:{1}", ProcessEXE.StartInfo.FileName , ProcessEXE.ExitCode), "Info");
                }
                else
                {
                    this.CallerForm.IsCLIProcessRunning = false;
                   // WriteTraceInfo(string.Format("BCP Process exited with exit code {0}", BCPProcessBCPEXE.ExitCode));
                   // System.Windows.Forms.MessageBox.Show(string.Format(@"Process:{0} completed , Returned:{1}", ProcessEXE.StartInfo.FileName, ProcessEXE.ExitCode), "Error");
                }
            }
            catch (Exception ex)
            {                
                throw new Exception(string.Format("Method:{0}", ex.TargetSite), ex);
            }

            return ProcessEXE.ExitCode;
        }

        void ProcessEXE_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            string s=string.Empty;
            if (e == null) return;

            if (e.Data == null) { s = string.Empty; }
            else
            {
                s = Convert.ToString(e.Data);
            }
            OnLogEvent(new LogEventArgs(Convert.ToString(s)));
        }


    }
}
