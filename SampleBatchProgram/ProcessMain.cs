using System;
using System.Collections.Generic;

using System.Text;
using System.IO;

namespace SampleCLIProgram
{
    /// <summary>
    /// </summary>
    public class ProcessMain
    {

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Public methods
        public string RunKey { get; set; }
        public string AppIdentifier { get; set; }
        public string SampleParam { get; set; }
        public bool TraceSwitch
        {
            get;
            set;
        }
        public string SMTPServer { get; set; }
        public string SMTPToEmail { get; set; }
        public string SMTPFromEmail { get; set; }
        public string SMTPCCEmail { get; set; }
        public string SMTPSubject
        {
            get
            {
                return string.Format("SampleCLIApplication - Error At User:{0}/{1} Machine :{2} Location:{3}", System.Environment.UserDomainName, System.Environment.UserName, System.Environment.MachineName, CLIAppManager.Utils.AssemblyUtil.AssemblyDirectory);
            }
        }

        public string FolderSampleParam { get; set; }
        public string FileSampleParam { get; set; }
        public string LOOKUP_PARAMSampleParam { get; set; }
        public bool TRUEFALSE_PARAMSampleParam { get; set; }
        public string PASSWORD_PARAM_1SampleParam { get; set; }

        public  string LogFileName { get; set; }
        #endregion

        #region private 
        private string LogFile { get; set; }
        #endregion

        public ProcessMain()
        {
            //constructor
        }

        private void InitLogging()
        {
            #region Enable Logging

            // Two types of logging is added in the process
            // 1) File logging - file name is unique to run instance - date/time stamped
            // 2) Email logging - Threshold = log4net.Core.Level.Error  means only Errors (email logging all errors will be send as email)

            CLIAppManager.Utils.Log4NetFileHelper log = new CLIAppManager.Utils.Log4NetFileHelper();
            log.Init();
            this.LogFile = string.Empty;
            // If trace is off then no need to file logging
            if (this.TraceSwitch)
            {
                string sLogile = (this.LogFileName == null || this.LogFileName.Length < 1) ? string.Format("SampleCLIApplication_{0}.log", DateTime.Now.ToString("yyyyMMMdd_hhmm")) : this.LogFileName;
                this.LogFile = sLogile;

                this.LogFileName = sLogile;
                log.AddFileLogging(Path.Combine(CLIAppManager.Utils.AssemblyUtil.AssemblyDirectory, this.LogFileName), log4net.Core.Level.All, false);
            }
            else
            {
                logger.Warn("Trace Switch is False, Hence no trace file is generated");
            }

            //SMTP trace will be generated at any cost - by design . so that any error will be notified
            log.AddSMTPLogging(this.SMTPServer, this.SMTPFromEmail, this.SMTPToEmail, this.SMTPCCEmail, this.SMTPSubject, log4net.Core.Level.Error);
            #endregion

        }

        public void Process()
        {
            
            try
            {
                InitLogging();

                #region Process
                logger.Info(" Sample CLI program started successfully.");
                logger.InfoFormat(@" App Identifier :{0} 
                                                Runkey :{1}
                                                Parameter Value : {2}
                                              ", this.AppIdentifier, this.RunKey, this.SampleParam);


                logger.Debug("Sample Debug message thru Log4net");
                logger.Warn("Sample Warning message  thru Log4net");
                logger.Fatal("Sample fatal message  thru Log4net");


                logger.InfoFormat("Parameter :{0} Value is {1}", "SAMPLE_PARAM_1", SampleParam);
                logger.InfoFormat("Parameter :{0} Value is {1}", "FOLDER_PARAM_1", FolderSampleParam);
                logger.InfoFormat("Parameter :{0} Value is {1}", "FILE_PARAM_1", FileSampleParam);
                logger.InfoFormat("Parameter :{0} Value is {1}", "LOOKUP_PARAM", LOOKUP_PARAMSampleParam);
                logger.InfoFormat("Parameter :{0} Value is {1}", "TRUEFALSE_PARAM_1", TRUEFALSE_PARAMSampleParam);
                

                //throw Exception Forcefully.
               // throw new System.ArgumentException("Testing Exception block !!!!", "Testing Exception block !!!!");

                logger.Info(" Sample CLI program finished.");


                if (this.TraceSwitch)
                {
                    //Close the file log , before you send email
                    CLIAppManager.Utils.Log4NetFileHelper helper = new CLIAppManager.Utils.Log4NetFileHelper();
                    helper.CloseAppender(Path.Combine(CLIAppManager.Utils.AssemblyUtil.AssemblyDirectory, this.LogFileName));

                    StringBuilder Sbody = new StringBuilder();

                    Sbody.AppendLine(System.Environment.NewLine);
                    Sbody.AppendFormat("User:{0}/{1} Machine :{2} Location:{3}", System.Environment.UserDomainName, System.Environment.UserName, System.Environment.MachineName, CLIAppManager.Utils.AssemblyUtil.AssemblyDirectory);
                    Sbody.AppendLine(System.Environment.NewLine);
                    // Sbody.AppendLine(WD.ToString());                    
                    Sbody.AppendLine(System.Environment.NewLine);
                    string sSubject = string.Format("Sample CLI APplication");
                    string sBody = Sbody.ToString();

                    string[] strAttach = new string[1];
                    strAttach[0] = Path.Combine(CLIAppManager.Utils.AssemblyUtil.AssemblyDirectory, this.LogFile);
                    
                    //SEND YOUR EMAIL HERE
                    //Send(this.SMTPFromEmail, this.SMTPToEmail, sBody, sSubject, strAttach, false);


                    #endregion
                }

                
            }
            catch (Exception ex)
            {
                //Data should be serializable only
                ex.Data.Add("RunKey", RunKey);
                ex.Data.Add("AppIdentifier", AppIdentifier);
                ex.Data.Add("SampleParam", SampleParam);
                ex.Data.Add("TraceSwitch", TraceSwitch);
                //Log4net error
                logger.Error(String.Format(ex.Message));
                logger.Error(ex.StackTrace);
                //throw the exception back without loosing the inner exception
                throw new Exception(string.Format("Method:{0}", ex.TargetSite), ex);

            }

        }

        
    }
}
