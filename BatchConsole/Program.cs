using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Xml;
using System.Reflection;
using System.Security.Permissions;
using log4net;
using System.IO;


namespace CLIConsole
{
    
    enum ExitCode : int
    {/* same code is replicated in CLIAppManager */
        Success = 0,        
        Failure = 1
    }

    /// <summary>
    /// Adapted from :  http://msdn.microsoft.com/en-us/magazine/cc164014.aspx
    /// </summary>
    class Program
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
        static int Main(string[] args)
        {

            CLIAppManager.Utils.Log4NetFileHelper log = new CLIAppManager.Utils.Log4NetFileHelper();
            log.Init(); //Initialize
            log.AddConsoleLogging(); //Add Console Logging
            log.AddFileLogging(Path.Combine(AssemblyDirectory, "CLIConsole.log")); //Add Console Logging
            log.AddFileLogging(Path.Combine(AssemblyDirectory, "CLIConsole_error.log"),log4net.Core.Level.Error); //Add Console Logging

            try
            {

                #region Command
                //Parameter Section
                //logger.Info("");
                //logger.Info("");
                //logger.Info(new string('*', 60));
                //logger.Info(new string('-', 60));
                //logger.Info("CLI Driver");
                //logger.Info(new string('-', 60));
                //logger.Info("");
                //logger.Info(string.Format("Argument length :{0}", args.Length));
                //logger.Info("");
                //string sinfo = string.Empty;
                //sinfo+=String.Format("{0,15}", "> CLIConsole.exe");
                
                //for (int i = 0; i < args.Length; i++)
                //{
                //    sinfo += String.Format("{0}", " " + args[i] + " ");
                //}
                //logger.Info(sinfo);
                //logger.Info("");
                //logger.Info(new string('-', 60));
                #endregion

                #region Check - Validation
                if (args.Length == 1 && args[0].Equals("?"))
                {
                    Usage();
                    //logger.Info(string.Format("Return :{0}", ExitCode.Success));
                    return (int) ExitCode.Success;
                }



                if (args.Length < 2)
                {
                    logger.Error ("Two arguments required:");
                    logger.Error ("assembly and class name.");
                    Usage();
                    logger.Info(string.Format("Return :{0}", ExitCode.Failure));
                    return (int) ExitCode.Failure;
                }
                #endregion

                CLIAppManager.ICLIEngine engine = null;

                #region Parameter Formatting
                string assemName = args[0];
                string className = args[1];

                string[] newargs = new string[args.Length - 2];

                for (int i = 2; i < args.Length; i++)
                {
                    newargs[i - 2] = args[i];
                }

            
                string szRunKey = System.Guid.NewGuid().ToString();

                #endregion


                
                try
                {
                    //Create Remote Object handle
                    System.Runtime.Remoting.ObjectHandle engineHandle =
                         Activator.CreateInstance(assemName, className);

                    // Get the Object
                    engine = (CLIAppManager.ICLIEngine)engineHandle.Unwrap();
                }
                catch (Exception e)
                {
                    logger.Error (e.Message);
                    throw ;

                }

                //Load Configuraton from Config table
                //Config HashTable
                Hashtable GlobalConfigTable = new Hashtable();
                Hashtable LocalConfigTable = new Hashtable();
                GetAppConfiguration("GLOBAL_CONFIG", GlobalConfigTable);
                GetAppConfiguration(className, LocalConfigTable);

                MergeConfig(GlobalConfigTable, LocalConfigTable);
                

                //Driver Name
                string szAppIdentifier = GetAppIdentifier(className);

                //Pass the Command Arguments and Configuration items to CLI Driver Program             
                int iRet = engine.Main(newargs, LocalConfigTable, szRunKey, szAppIdentifier);
                logger.Info("Returned :"+iRet);
            }
            catch (Exception ex)
            {
                logger.Error (String.Format(@"Please ensure that you have defined config section in SectionGroup and CLIDriversGroup in app.config file."));
                logger.Error (ex.Message);
                logger.Info(string.Format("Failed at CLIConsole. Returned with Error Code :{0}",(int)ExitCode.Failure));

                return (int)ExitCode.Failure;
            }
            finally
            {

            }

            return (int) ExitCode.Success;
        }


        private static void WriteVersions()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            AssemblyName[] name = assembly.GetReferencedAssemblies();

            for (int i = 0; i < name.Length; i++)
            {
                AssemblyName asm = name[i];

                logger.Info("");
                logger.Info(String.Format("Assembly Name :{0} , Version:{1}", asm.Name, asm.Version));
                logger.Info("");
            }
        }

        private static string GetVersionInfo(string sname)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            AssemblyName[] name = assembly.GetReferencedAssemblies();

            for (int i = 0; i < name.Length; i++)
            {
                AssemblyName asm = name[i];
                if (String.Equals(asm.Name,sname, StringComparison.OrdinalIgnoreCase))
                {
                    return asm.Version.ToString();
                }

            }

            return "Version not found for " + sname;
        }


        /// <summary>
        /// Usage  ?
        /// NOTE : Add usage about the new drivers here.
        /// </summary>
        private static void Usage()
        {
           // logger.Info(new string('*',60));
            logger.Info("CLI Console");
            logger.Info("-- Command Line Interface written in C#.");
            logger.Info("CLI Interface can be very easily enteded to supports Pipe to link multiple console applications.This is very common feature in unix applications");
            logger.Info("> CLIConsole.exe \"CLIAppManager\" \"CLIAppManager.Drivers.FindFilesProgramDriver\" | CLIConsole.exe \"CLIAppManager\" \"CLIAppManager.Drivers.ProcessFileSampleDriver\"  ");
            logger.Info("CLI Interface can also be executed from a windows app for non power users.");
            logger.Info("");
            logger.Info(String.Format("Version:{0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()));
            logger.Info("");
            logger.Info("CLI Manager");
            logger.Info("-- Manager is a level of abstraction to group all realted-drivers toghether . Drivers are individual .net framework console application(s)");
            logger.Info("");
            logger.Info(String.Format("Version:{0}", GetVersionInfo("CLIAppManager")));
            logger.Info("");
           // logger.Info(new string('*', 60));
            logger.Info(@" ");
            logger.Info(new string('-', 60));
            logger.Info(string.Format(@"Reserved Options"));
            logger.Info(new string('-', 60));
            logger.Info(string.Format(@"?         :  help"));
            logger.Info(string.Format(@"/P        :  Prompt for user input"));
            logger.Info(string.Format(@"/CONFIG   :  Read from config file"));
            logger.Info(new string(' ', 60));
           // logger.Info(new string(' ', 60));
            //logger.Info(new string(' ', 60));
            //logger.Info(new string(' ', 60));
            logger.Info("Sample CLI Program for rapid prototyping");
            logger.Info("> CLIConsole.exe \"{Manager App}\" \"{Drivers}\" ?");
            logger.Info("> CLIConsole.exe \"CLIAppManager\" \"CLIAppManager.Drivers.SampleCLIProgramDriver\" ?");
            logger.Info(new string(' ', 60));
            logger.Info("SampleCLIProgramDriver is a driver to Execute SampleCLIApplication.Having a driver in between CLI and application(s) ,makes sure that all applications will have same interface to input.");
            // logger.Info(new string(' ', 60));
            // logger.Info(new string(' ', 60));

            logger.Info("");

        }

        /// <summary>
        /// Load Configuration specific to the Driver Program 
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        private static Hashtable GetAppConfiguration(string className, Hashtable hashConfigTable)
        {
            /*
             * Using HashTable makes easy to find the items by Key , but it's messes up  the sequence              * 
             * */

           


            // Get the application configuration file.
            System.Configuration.Configuration config =
                        ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            // Get the collection of the section groups.       
            ConfigurationSectionGroupCollection sectionGroups = config.SectionGroups;

            //Loop thru each config elements
            foreach (ConfigurationSectionGroup sectionGroup in sectionGroups)
            {
                if (sectionGroup.Name == "CLIDriversGroup")
                {
                    //Get to the Section
                    foreach (ConfigurationSection configurationSection in sectionGroup.Sections)
                    {
                        //Since Period (.) not working in the XML tags
                        string sFormatClassName = className.ToUpper();
                        sFormatClassName = sFormatClassName.Replace(".", "_");

                        //Pick the Section required by the Class Name
                        if (String.Equals(configurationSection.SectionInformation.Name , sFormatClassName,StringComparison.OrdinalIgnoreCase))
                        {
                            var section =
                            ConfigurationManager.GetSection(configurationSection.SectionInformation.SectionName) as NameValueCollection;

                            foreach (string s in section.AllKeys)
                            {
                                hashConfigTable.Add(s.ToUpper().Trim(), Convert.ToString(section[s]).Trim());
                            }
                        }

                    }
                }
            }

            return hashConfigTable;
        }

        private static void MergeConfig(Hashtable global , Hashtable local)
        {
            foreach (string sKey in global.Keys)
            {
                if (local[sKey] == null)
                {                    
                    local.Add(sKey, global[sKey]);
                    logger.Info(string.Format(@"Adding Global Config value for key={0},value:{1}",sKey,local[sKey]));
                }else{
                    logger.Info(string.Format(@"Skipped Global Config value for key={0},value:{1}",sKey,global[sKey]));                    
                }
            }

        }
        private static string GetAppIdentifier(string className)
        {
            string szAppIdentifier = string.Empty;

            if (className.ToUpper().Equals(""))
            {
            }
            else if (className.ToUpper().Equals(""))
            {
            }
            else
            {
                //extract the Driver Class name
                className = className.ToUpper();
                string[] splits = className.Split(new char[] { '.' });
                string szClassname = splits[splits.Length - 1];
                szAppIdentifier = szClassname;
                if (szClassname.Length > 20)
                {
                    szAppIdentifier = szClassname.Substring(0, 19);
                }

            }

            return szAppIdentifier;
        }
    }
}
