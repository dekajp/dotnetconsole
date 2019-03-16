using System;
using log4net.Appender;
using log4net;
using log4net.Repository.Hierarchy;
using System.Diagnostics;
using System.Reflection;

namespace CLIAppManager.Utils
{
    public enum LoggingType
    {
        INFO,
        ERROR,
        WARNING
    }

    //This will write everything back to Console. this is very basic form of Logging.
    public static class Logging
    {
         private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void WriteTrace(string s)
        {
            Write(s);
        }


        public static void WriteTraceWithDateTime(string s)
        {
            Write(String.Format("{0,-25}{1,-200}", System.DateTime.Now, s));
        }

        private static void Write(string s)
        {
            logger.Info(s);
        }
    }


    public class Log4NetFileHelper
    {
        private readonly string  DEFAULT_LOG_FILENAME=string.Format("application_log_{0}.log",DateTime.Now.ToString("yyyyMMMdd_hhmm"));
        Logger root;
        public Log4NetFileHelper()
        {
            
        }

        public virtual void Init()
        {
            root = ((Hierarchy)LogManager.GetRepository()).Root;
            //root.AddAppender(GetConsoleAppender());
            //root.AddAppender(GetFileAppender(sFileName));
            root.Repository.Configured = true;
        }

        #region Public Helper Methods
        #region Console Logging
        public virtual void AddConsoleLogging()
        {
            ConsoleAppender C = GetConsoleAppender();
            AddConsoleLogging(C);
        }

        public virtual void AddConsoleLogging(ConsoleAppender C)
        {
            root.AddAppender(C);
        }
        #endregion

        #region File Logging
        public virtual FileAppender AddFileLogging()
        {
            return AddFileLogging(DEFAULT_LOG_FILENAME);
        }

        public virtual FileAppender AddFileLogging(string sFileFullPath)
        {
            return AddFileLogging(sFileFullPath, log4net.Core.Level.All);
        }

        public virtual FileAppender AddFileLogging(string sFileFullPath, log4net.Core.Level threshold)
        {
            return AddFileLogging(sFileFullPath, threshold,true);  
        }

        public virtual FileAppender AddFileLogging(string sFileFullPath, log4net.Core.Level threshold, bool bAppendfile)
        {
            FileAppender appender = GetFileAppender(sFileFullPath, threshold , bAppendfile);
            root.AddAppender(appender);
            return appender;
        }

        public virtual SmtpAppender AddSMTPLogging(string smtpHost, string From, string To, string CC, string subject, log4net.Core.Level threshhold)
        {
            SmtpAppender appender = GetSMTPAppender(smtpHost, From, To, CC, subject, threshhold);
             root.AddAppender(appender);
             return appender;
        }

        #endregion


        public log4net.Appender.IAppender GetLogAppender(string AppenderName)
        {
            AppenderCollection ac = ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root.Appenders;

            foreach(log4net.Appender.IAppender appender in ac){
                if (appender.Name == AppenderName)
                {
                    return appender;
                }
            }

            return null;
        }

        public void CloseAppender(string AppenderName)
        {
            log4net.Appender.IAppender appender = GetLogAppender(AppenderName);
            CloseAppender(appender);
        }

        private void CloseAppender(log4net.Appender.IAppender appender)
        {
            appender.Close();
        }

        #endregion

        #region Private Methods

        private SmtpAppender GetSMTPAppender(string smtpHost, string From, string To, string CC, string subject, log4net.Core.Level threshhold)
        {
            SmtpAppender lAppender = new SmtpAppender
            {
                Cc = CC,
                To = To,
                From = From,
                SmtpHost = smtpHost,
                Subject = subject,
                BufferSize = 512,
                Lossy = false,
                Layout = new log4net.Layout.PatternLayout("%date{dd-MM-yyyy HH:mm:ss,fff} %5level [%2thread] %message (%logger{1}:%line)%n"),
                Threshold = threshhold
            };
            lAppender.ActivateOptions();
                return lAppender;
            }

        private ConsoleAppender GetConsoleAppender()
        {
            ConsoleAppender lAppender = new ConsoleAppender {
                Name = "Console",
                Layout = new log4net.Layout.PatternLayout(" %message %n"),
                Threshold = log4net.Core.Level.All
             };

            lAppender.ActivateOptions();
            return lAppender;
        }
        /// <summary>
        /// DETAILED Logging 
        /// log4net.Layout.PatternLayout("%date{dd-MM-yyyy HH:mm:ss,fff} %5level [%2thread] %message (%logger{1}:%line)%n");
        ///  
        /// </summary>
        /// <param name="sFileName"></param>
        /// <param name="threshhold"></param>
        /// <returns></returns>
        private FileAppender GetFileAppender(string sFileName, log4net.Core.Level threshhold, bool bFileAppend)
        {
            FileAppender lAppender = new FileAppender {
                Name = sFileName,
                AppendToFile = bFileAppend,
                File = sFileName,
                Layout = new log4net.Layout.PatternLayout("%date{dd-MM-yyyy HH:mm:ss,fff} %5level [%2thread] %message (%logger{1}:%line)%n"),
                Threshold = threshhold
            };
            lAppender.ActivateOptions();
            return lAppender;
        }

        //private FileAppender GetFileAppender(string sFileName)
        //{
        //    return GetFileAppender(sFileName, log4net.Core.Level.All,true);
        //}

        #endregion

        private void  ConfigureLog(string sFileName)
        {
          
           
        }
    }


     #region Imports

    

    #endregion

    /// <summary>
    /// log4net Log helper
    /// http://stackoverflow.com/questions/1520820/can-this-simple-log4net-wrapper-be-improved
    /// </summary>
    public sealed class LoggerHelper
    {
        #region Constants and Fields

        /// <summary>
        /// Determines whether the DEBUG Mode is enabled.
        /// </summary>
        private readonly bool isDebugEnabled;

        /// <summary>
        /// The is error enabled.
        /// </summary>
        private readonly bool isErrorEnabled;

        /// <summary>
        /// Determines whether the FATAL Mode is enabled.
        /// </summary>
        private readonly bool isFatalEnabled;

        /// <summary>
        /// Determines whether the INFO Mode is enabled.
        /// </summary>
        private readonly bool isInfoEnabled;

        /// <summary>
        /// Determines whether the WARN Mode is enabled.
        /// </summary>
        private readonly bool isWarnEnabled;

        /// <summary>
        /// The logger object
        /// </summary>
        private readonly ILog log;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the Logger class.
        /// </summary>
        public LoggerHelper()
            : this(new StackTrace().GetFrame(1).GetMethod().DeclaringType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Logger class.
        /// </summary>
        /// <param name="type">
        /// The type of logger.
        /// </param>
        public LoggerHelper(Type type)
        {
            this.log = LogManager.GetLogger(type);

            this.isDebugEnabled = this.log.IsDebugEnabled;
            this.isErrorEnabled = this.log.IsErrorEnabled;
            this.isInfoEnabled = this.log.IsInfoEnabled;
            this.isFatalEnabled = this.log.IsFatalEnabled;
            this.isWarnEnabled = this.log.IsWarnEnabled;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Logs the debug message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Debug(string message)
        {
            if (this.isDebugEnabled)
            {
                MethodBase methodBase = new StackTrace().GetFrame(1).GetMethod();
                this.log.Debug(methodBase.Name + " : " + message);
            }
        }

        /// <summary>
        /// Logs the debug message and the exception.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        public void Debug(string message, Exception exception)
        {
            if (this.isDebugEnabled)
            {
                MethodBase methodBase = new StackTrace().GetFrame(1).GetMethod();
                this.log.Debug(methodBase.Name + " : " + message, exception);
            }
        }

        /// <summary>
        /// Logs the error message.
        /// </summary>
        /// <param name="errorMessage">
        /// The error message.
        /// </param>
        public void Error(string errorMessage)
        {
            if (this.isErrorEnabled)
            {
                MethodBase methodBase = new StackTrace().GetFrame(1).GetMethod();
                this.log.Error(methodBase.Name + " : " + errorMessage);
            }
        }

        /// <summary>
        /// Logs the error message and the exception.
        /// </summary>
        /// <param name="errorMessage">
        /// The error message.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        public void Error(string errorMessage, Exception exception)
        {
            if (this.isErrorEnabled)
            {
                MethodBase methodBase = new StackTrace().GetFrame(1).GetMethod();
                this.log.Error(methodBase.Name + " : " + errorMessage, exception);
            }
        }

        /// <summary>
        /// Logs the fatal error message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Fatal(string message)
        {
            if (this.isFatalEnabled)
            {
                MethodBase methodBase = new StackTrace().GetFrame(1).GetMethod();
                this.log.Fatal(methodBase.Name + " : " + message);
            }
        }

        /// <summary>
        /// Logs the fatal error message and the exception.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        public void Fatal(string message, Exception exception)
        {
            if (this.isFatalEnabled)
            {
                MethodBase methodBase = new StackTrace().GetFrame(1).GetMethod();
                this.log.Fatal(methodBase.Name + " : " + message, exception);
            }
        }

        /// <summary>
        /// Logs the info message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Info(string message)
        {
            if (this.isInfoEnabled)
            {
                MethodBase methodBase = new StackTrace().GetFrame(1).GetMethod();
                this.log.Info(methodBase.Name + " : " + message);
            }
        }

        /// <summary>
        /// Logs the info message and the exception.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        public void Info(string message, Exception exception)
        {
            if (this.isInfoEnabled)
            {
                MethodBase methodBase = new StackTrace().GetFrame(1).GetMethod();
                this.log.Info(methodBase.Name + " : " + message, exception);
            }
        }

        /// <summary>
        /// Logs the warning message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Warn(string message)
        {
            if (this.isWarnEnabled)
            {
                MethodBase methodBase = new StackTrace().GetFrame(1).GetMethod();
                this.log.Warn(methodBase.Name + " : " + message);
            }
        }

        /// <summary>
        /// Logs the warning message and the exception.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        public void Warn(string message, Exception exception)
        {
            if (this.isWarnEnabled)
            {
                MethodBase methodBase = new StackTrace().GetFrame(1).GetMethod();
                this.log.Warn(methodBase.Name + " : " + message, exception);
            }
        }

        #endregion
    }
}
