#define RELEASE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using CLIAppManager;
using System.Threading;
using System.IO;
using System.Xml;
using System.Diagnostics;
using CLIAppManager.Utils;
using System.Threading.Tasks;

namespace CLIAppManagerWinForm
{
    

    public enum CLICommands { Help, ListDrivers, Run }

    public partial class Form1 : Form
    {

        CLIOptionUserControl[] CLIOptionControlArray;
        delegate void StringParameterDelegate(string value);

        public string ConfigFile { get; set; }
        public Hashtable  ConfigTable { get; set; }

        CLIAppManager.CLIEngineBase Engine { get; set; }

        public bool IsCLIProcessRunning { get; set; }

        private bool logWatching = false;
        log4net.Appender.MemoryAppender memlogger;
        private Thread logWatcher;

        private string m_sFileVersion = string.Empty;

        private string SelectedDriver
        {
            get
            {
                return this.cbDrivers.SelectedItem.GetType().FullName;
            }
        }

        public Form1()
        {
            InitializeComponent();
            Initialize();
        }

        Assembly CLIAppManagerAssembly { get; set; }
        string sNameSpace = "CLIAppManager.Drivers";

        /// <summary>
        /// http://www.hanselman.com/blog/HowToProgrammaticallyDetectIfAnAssemblyIsCompiledInDebugOrReleaseMode.aspx
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static bool IsDebug(Assembly assembly)
        {
            object[] attributes = assembly.GetCustomAttributes(typeof(DebuggableAttribute), true);
            if (attributes == null || attributes.Length == 0)
                return true;

            var d = (DebuggableAttribute)attributes[0];
            if (d.IsJITTrackingEnabled) return true;
            return false;
        }

        private void Initialize()
        {
            IsCLIProcessRunning = false;

            #region Populate Driver
            this.ConfigFile = string.Empty;
            ConfigTable = new Hashtable();

            Assembly asm = Assembly.GetExecutingAssembly();
            AssemblyName [] asmName=  asm.GetReferencedAssemblies();

            string asmFullName = string.Empty;
            for (int i = 0; i < asmName.Length; i++)
            {
                if (asmName[i].Name.ToUpper() == ("CLIAppManager").ToUpper())
                {
                    asmFullName = asmName[i].FullName;
                    break;
                }
            }

            CLIAppManagerAssembly = Assembly.Load(asmFullName);
            
            
            List<string> namespaceList = new List<string>();
            List<string> classlist = new List<string>();

            foreach (Type type in CLIAppManagerAssembly.GetTypes())
            {
                if (type.Namespace == sNameSpace)
                    namespaceList.Add(type.Name);
            }

            foreach (string classname in namespaceList)
            {
                classlist.Add(classname);
                Object rfb = Activator.CreateInstance(CLIAppManagerAssembly.FullName, sNameSpace + "." + classname).Unwrap();
                CLIAppManager.CLIEngineBase  engine =( CLIAppManager.CLIEngineBase) rfb;
                engine.Main(null, null, "", System.Guid.NewGuid().ToString());
                this.cbDrivers.Items.Add(rfb);
            }

            #endregion

            // Get the application configuration file.
            System.Configuration.Configuration config =
                        ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            tbConfigFile.Text = config.FilePath;
            this.grpConfigbox.Text = string.Format("Config File - {0}", Path.GetFileName(this.tbConfigFile.Text));

            #region version
            Assembly assembly = Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            m_sFileVersion = fvi.FileVersion;

            this.Text = string.Format("CLIApplication WinConsole - Version:{0} ", m_sFileVersion);
            this.lblLocation.Text =string.Format("{0}", CLIAppManager.Utils.AssemblyUtil.AssemblyDirectory);
            #endregion

            this.toolTip1.SetToolTip(this.tbConfigFile, this.tbConfigFile.Text);
            this.toolTip1.SetToolTip(this.btnHelp, "Will generate the help info in console window for selected driver");
            this.toolTip1.SetToolTip(this.btnListAllDrivers, "Will generate the help info for all drivers available.");

            this.cbDebug.Visible = true  ;

            //if (IsDebug(asm)) { this.cbDebug.Visible = true; }

            #if RELEASE
                this.cbDebug.Visible = false;
            #endif
            
            
        }

        private void CbDrivers_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitOptionPanel();
        }

        private void InitOptionPanel()
        {
            Object rfb = Activator.CreateInstance(CLIAppManagerAssembly.FullName, this.SelectedDriver).Unwrap();
            this.Engine = (CLIAppManager.CLIEngineBase)rfb;
            this.Engine.Main(null, null, "", System.Guid.NewGuid().ToString());

            BuildOptionPanel();
        }

        private void BuildOptionPanel()
        {
            this.panel1.Controls.Clear();
            Point prvLoc = new Point(0, 0);           
            CLIOptionControlArray = new CLIOptionUserControl[this.Engine.CLIOptionsHashTable.Keys.Count];

            //Sort the parameters
            Hashtable settings = this.Engine.CLIOptionsHashTable;
            ArrayList keys = new ArrayList();
            keys.AddRange(settings.Keys);
            keys.Sort();

            int i = 0;
            Point prevlocation = new Point(0,0);
            foreach (object key in keys)
            {

                CLIAppManager.Params.CLIParam param = (CLIAppManager.Params.CLIParam)this.Engine.CLIOptionsHashTable[key];
                CLIOptionControlArray[i] = new CLIOptionUserControl(param) {
                    Width = this.panel1.Width - 50,
                    AutoSize = true
                };
                CLIOptionControlArray[i].InfoLoad += new CLIOptionUserControl.InfoLoadEventHandler(Form1_InfoLoad);
                if (i == 0)
                {
                    prevlocation.Y += 3;
                    prevlocation.X += 3;
                    CLIOptionControlArray[i].Location = prevlocation;
                }
                else
                {
                    CLIOptionControlArray[i].Location = new System.Drawing.Point(
                                 prevlocation.X,
                                 prevlocation.Y + CLIOptionControlArray[i].Height+3);
                }

                prevlocation = CLIOptionControlArray[i].Location;

                i++;
            }

            this.panel1.Controls.AddRange(CLIOptionControlArray);
                 
        }

        void Form1_InfoLoad(object sender ,CLIParamBase CLIparam)
        {
            CLIparam.ParamValue = ((CLIOptionUserControl)sender).Value;
            System.Windows.Forms.MessageBox.Show(Convert.ToString(CLIparam.ParamInfo()),"Parameter Info :"+CLIparam.ParamName);
        }

        private void BtnExecute_Click(object sender, EventArgs e)
        {
            try
            {
             
   
                string message = "You are about to start the CLIProcess , which runs in background. To stop a CLI process , Kill the process from Task Manager.";
                string caption = "Warning";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                // Displays the MessageBox.

                result = MessageBox.Show(message, caption, buttons);

                if (result == System.Windows.Forms.DialogResult.Yes)
                {                    
                    StartProcessInThread(CLICommands.Run);
                }

                
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.StackTrace);

                throw new Exception(string.Format("Method:{0}", ex.TargetSite), ex);
            }
        }

        void Shell_LoggingEvent(object sender, LogEventArgs e)
        {
            if (InvokeRequired)
            {
                // We're not in the UI thread, so we need to call BeginInvoke
                BeginInvoke(new StringParameterDelegate(WriteTraceInfo), new object[] { e.LogMessage });
                return;
            }
           
            
        }

        private void WriteTraceInfo(string s)
        {
            if (this.rtbOutput.Text.Length > 0)
            {
                rtbOutput.AppendText(Environment.NewLine);
            }
            this.rtbOutput.AppendText(Convert.ToString(s));
        }


        private string  CLIConsoleExe
        {
            get { return string.Format(@"""{0}{1}CLIConsole.exe"" ", CLIAppManager.Utils.AssemblyUtil.AssemblyDirectory, Path.DirectorySeparatorChar); }
        }

       

        private void BtnGenerateCmd_Click(object sender, EventArgs e)
        {
            string sParam = BuildCommand();
            this.rtbCmd.Text = CLIConsoleExe + " " + sParam;

            System.Windows.Forms.MessageBox.Show("Command Generated Sucessfully !!");
        }

        #region EXE - process


        private Hashtable BuildCommandReturnConfigTable()
        {
            Hashtable ht = new Hashtable();
            for (int i = 0; i < this.CLIOptionControlArray.Length; i++)
            {
                ht.Add(this.CLIOptionControlArray[i].ParamNameValue.ToUpper(), this.CLIOptionControlArray[i].Value);
            }

            return ht;
           
        }

        private string BuildCommand()
        {
            string sParam = string.Empty;

            sParam = string.Format(@" ""CLIAppManager"" ""{0}"" ", this.SelectedDriver);

            for (int i = 0; i < this.CLIOptionControlArray.Length; i++)
            {
                sParam += " ";
                sParam += this.CLIOptionControlArray[i].ParamCommandOption;
                sParam += " ";
            }

            return  sParam;
        }

        private string BuildCommandhelp()
        {
            string sParam = string.Empty;

            sParam = string.Format(@" ""CLIAppManager"" ""{0}"" ? ", this.SelectedDriver);


            return  sParam;
        }

        private string BuildCommandListDrivers()
        {
            return  "?";
        }
        private void StartProcessInThread(CLICommands eCLIcmd)
        {
            //http://msdn.microsoft.com/en-us/library/aa645740(v=vs.71).aspx#vcwlkthreadingtutorialexample1creating

            if (IsCLIProcessRunning)
            {
                System.Windows.Forms.MessageBox.Show("CLI Application is running in backgroud.Please wait for it to finish.", "Info");
                return;
            }

            if (this.cbDebug.Checked && this.cbDebug.Visible)
            {
                StartProcessWithoutShell(eCLIcmd);
                return;
            }

            ShellWorker oShell = new ShellWorker(this);
            string sParam = string.Empty;
            if (eCLIcmd == CLICommands.Help)
            {
                sParam = BuildCommandhelp();
            }
            else if (eCLIcmd == CLICommands.Run)
            {           
                sParam = BuildCommand();
            }
            else if (eCLIcmd == CLICommands.ListDrivers)
            {
                sParam = BuildCommandListDrivers();
            }
            oShell.LoggingEvent += new LogEventHandler(Shell_LoggingEvent);
           // oShell.EXEPath = CLIConsoleExe;
           // oShell.ExeParams =  sParam;

            this.rtbCmd.Text = CLIConsoleExe +" "+sParam;
            this.rtbOutput.Text = string.Empty;

            IsCLIProcessRunning = true;

            // Create the thread object, passing in the Alpha.Beta method
            // via a ThreadStart delegate. This does not start the thread.
            //Thread oThread = new Thread(new ThreadStart(oShell.CallShell));

            //oThread.Start();

            Action<string, string> shell = delegate (string execomamnd, string exeparams)
            {
                oShell.CallShell(execomamnd, exeparams);
            };

            Task.Run(() => shell(CLIConsoleExe, sParam));

        }


        private void StartProcessWithoutShell(CLICommands eCLIcmd)
        {
            //http://msdn.microsoft.com/en-us/library/aa645740(v=vs.71).aspx#vcwlkthreadingtutorialexample1creating

            if (IsCLIProcessRunning)
            {
                System.Windows.Forms.MessageBox.Show("CLI Application is running in backgroud.Please wait for it to finish.", "Info");
                return;
            }

            Hashtable htconfig = new Hashtable();
            if ((eCLIcmd == CLICommands.Run ||eCLIcmd == CLICommands.Help ) && this.SelectedDriver.Length > 0)                
            {
                try
                {
                    this.rtbOutput.Clear();
                    logWatching = true;

                    htconfig = BuildCommandReturnConfigTable();


                    memlogger = new log4net.Appender.MemoryAppender
                    {
                        Layout = new log4net.Layout.PatternLayout("%date{dd-MM-yyyy HH:mm:ss,fff} %5level [%2thread] %message (%logger{1}:%line)%n")
                    };

                    // Could use a fancier Configurator if you don't want to catch every message  
                    log4net.Config.BasicConfigurator.Configure(memlogger);

                    // Since there are no events to catch on logging, we dedicate  
                    // a thread to watching for logging events  
                    logWatcher = new Thread(new ThreadStart(LogWatcher));
                    logWatcher.Start();

                    Object rfb = Activator.CreateInstance(CLIAppManagerAssembly.FullName, this.SelectedDriver).Unwrap();
                    CLIAppManager.CLIEngineBase engine = (CLIAppManager.CLIEngineBase)rfb;
                    engine.CLIOptionsHashTable.Clear();
                    int iret = engine.Main(new string[] { "/CONFIG=TRUE" }, htconfig, System.Guid.NewGuid().ToString(), string.Empty);

                    //Sleep the main thread so the memlogger flush all output to Console window
                    Thread.Sleep(500);

                }
                catch
                {
                }
                finally
                {
                    logWatching = false;
                    logWatcher.Join(); 
                }

            }
        }

        private void RunDebugMode()
        {

        }

        private void LogWatcher()
        {
            // we loop until the Form is closed  
            while (logWatching)
            {
                log4net.Core.LoggingEvent[] events = memlogger.GetEvents();
                if (events != null && events.Length > 0)
                {
                    // if there are events, we clear them from the logger,  
                    // since we're done with them  
                    memlogger.Clear();
                    foreach (log4net.Core.LoggingEvent ev in events)
                    {
                     
                        // the line we want to log  
                         string line = ev.LoggerName + ": " + ev.RenderedMessage + "\r\n";
                        Shell_LoggingEvent(null, new LogEventArgs(string.Format("{0}-{1}",DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss,fff"),line)));
                       
                    }
                }
                // nap for a while, don't need the events on the millisecond.  
                Thread.Sleep(50);
            }
        }  

        #endregion



        private void BtnBrowsefldr_Click(object sender, EventArgs e)
        {
            // Show the FolderBrowserDialog.
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
               string folderName = folderBrowserDialog1.SelectedPath;
               this.tbLogFile.Text = folderName + System.IO.Path.DirectorySeparatorChar + "output" + System.DateTime.Now.ToString("yyyyMMMdd_hh_mm_ss") + ".log";
            }
        }

        private void BtnHelp_Click(object sender, EventArgs e)
        {
            if (this.cbDrivers.SelectedIndex < 0)
            {
                System.Windows.Forms.MessageBox.Show("Select driver !!","Error");
                return;
            }
            
            try
            {

                StartProcessInThread(CLICommands.Help);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.StackTrace);

                throw new Exception(string.Format("Method:{0}", ex.TargetSite), ex);
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {

                if (tbLogFile.Text.Length < 1)
                {
                    System.Windows.Forms.MessageBox.Show("File name unknown !!","Error");
                    return;
                }

                StringBuilder sb = new StringBuilder();
                sb.Append(this.rtbOutput.Text);

                string filename = tbLogFile.Text.Trim();


                using (StreamWriter outfile =
                    new StreamWriter(filename))
                {
                    outfile.Write(sb.ToString());
                }

                System.Windows.Forms.MessageBox.Show(string.Format(@"File Generated successfully at {0}",filename));
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
                throw new Exception(string.Format("Method:{0}", ex.TargetSite), ex);
            }
        }

        private void BtnListAllDrivers_Click(object sender, EventArgs e)
        {
            try
            {

                StartProcessInThread(CLICommands.ListDrivers);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.StackTrace);

                throw new Exception(string.Format("Method:{0}", ex.TargetSite), ex);
            }
        }

        private void BtnConfigFile_Click(object sender, EventArgs e)
        {
            ConfigFile = this.tbConfigFile.Text;

            OpenFileDialog();

            this.tbConfigFile.Text = ConfigFile;
            this.toolTip1.SetToolTip(this.tbConfigFile, this.tbConfigFile.Text);

            this.grpConfigbox.Text=string.Format("Config File - {0}",Path.GetFileName(this.tbConfigFile.Text));
            
        }


        private void OpenFileDialog()
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog {                
                Filter = "Config Files (*.config)|*.config|txt files (*.txt)|*.txt|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true,
                Multiselect = false,
                InitialDirectory = Application.StartupPath
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this.ConfigFile= openFileDialog1.FileName;                  
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }            
        }


        #region config handling
        private Hashtable GetAppConfiguration(string className, Hashtable hashConfigTable , string filepath)
        {
            
            //// Get the application configuration file.
            System.Configuration.Configuration config;

            if (filepath.Length > 0)
            {
                System.Configuration.ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap {
                    ExeConfigFilename = filepath
                };

                config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            }
            else
            {
                config =
                        ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            }

            ConfigurationManager.RefreshSection("configuration");

            string sFormatClassName = className;
            sFormatClassName = sFormatClassName.Replace(".", "_");

            ConfigurationSectionGroup mySectiongrp = config.GetSectionGroup("CLIDriversGroup");
            ConfigurationSection mySection = mySectiongrp.Sections[sFormatClassName];

            XmlDocument document = new XmlDocument();
            document.LoadXml(mySection.SectionInformation.GetRawXml());

            XmlNodeList nodelist = document.SelectNodes("//add");

            foreach (XmlNode node in nodelist)
            {
                XmlAttributeCollection attrcoll = node.Attributes;
                string valuefromgui = string.Empty;
                bool bValueSet = false;
                foreach (XmlAttribute attr in attrcoll)
                {
                    if (attr.Name.ToUpper().Equals("KEY"))
                    {
                        valuefromgui = Convert.ToString(attr.Value.ToUpper());
                        bValueSet = true;
                    }

                    if (bValueSet && attr.Name.ToUpper().Equals("VALUE"))
                    {
                        hashConfigTable.Add(valuefromgui, Convert.ToString(attr.Value));
                    }
                }
            }
            return hashConfigTable;
        }

        private void MergeConfig(Hashtable global, Hashtable local)
        {
            foreach (string sKey in global.Keys)
            {
                if (local[sKey] == null)
                {
                    local.Add(sKey, global[sKey]);
                    //logger.Info(string.Format(@"Adding Global Config value for key={0},value:{1}", sKey, local[sKey]));
                }
                else
                {
                    //logger.Info(string.Format(@"Skipped Global Config value for key={0},value:{1}", sKey, global[sKey]));
                }
            }

        }
        private string GetAppIdentifier(string className)
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
        

        //private void SaveAppConfiguration(string className, Hashtable hashConfigTable, string filepath)
        //{

           
        //    ArrayList keysArrList = new ArrayList();
        //    keysArrList.AddRange(hashConfigTable.Keys);
        //    keysArrList.Sort();
            


        //    //// Get the application configuration file.
        //    System.Configuration.Configuration config =
        //                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        //    if (filepath.Length > 0)
        //    {
        //        System.Configuration.ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
        //        configFileMap.ExeConfigFilename = filepath;

        //        config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
        //    }

        //    string sFormatClassName = className;
        //    sFormatClassName = sFormatClassName.Replace(".", "_");

        //    ConfigurationSectionGroup mySectiongrp = config.GetSectionGroup("CLIDriversGroup");
        //    ConfigurationSection mySection = mySectiongrp.Sections[sFormatClassName];

        //    XmlDocument document = new XmlDocument();
        //    document.LoadXml(mySection.SectionInformation.GetRawXml());

        //    XmlNodeList nodelist = document.SelectNodes("//add");

        //    foreach (XmlNode node in nodelist)
        //    {
        //        XmlAttributeCollection attrcoll = node.Attributes;
        //        string valuefromgui=string.Empty;
        //        bool bValueSet = false;
        //        foreach (XmlAttribute attr in attrcoll)
        //        {
        //            if (attr.Name.ToUpper().Equals("KEY"))
        //            {
        //                valuefromgui=Convert.ToString(hashConfigTable[attr.Value.ToUpper()]);
        //                bValueSet = true;
        //            }

        //            if (bValueSet && attr.Name.ToUpper().Equals("VALUE"))
        //            {
        //                attr.Value = valuefromgui;
        //            }
        //        }
        //    }

        //    mySection.SectionInformation.SetRawXml(document.OuterXml);          

        //    config.Save(ConfigurationSaveMode.Full);
        //    ConfigurationManager.RefreshSection(sFormatClassName);            
        //}


        private bool SaveAppConfigurationAll(string className, Hashtable hashConfigTable, string filepath)
        {           

            ArrayList keysArrList = new ArrayList();
            keysArrList.AddRange(hashConfigTable.Keys);
            keysArrList.Sort();

            //Get the application configuration file.
            System.Configuration.Configuration config =
                        ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (filepath.Length > 0)
            {
                System.Configuration.ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap
                {
                    ExeConfigFilename = filepath
                };

                config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            }

            string sFormatClassName = className;
            sFormatClassName = sFormatClassName.Replace(".", "_");


            ConfigurationSectionGroup mySectiongrp = config.GetSectionGroup("CLIDriversGroup");
            ConfigurationSection mySection = mySectiongrp.Sections[sFormatClassName];

            XmlDocument document = new XmlDocument();
            document.LoadXml(mySection.SectionInformation.GetRawXml());


            foreach (Object okey in keysArrList)
            {
                XmlNode node = GetNodeAvailable(document, okey.ToString());

                XmlAttributeCollection attrcoll = node.Attributes;
               
                foreach (XmlAttribute attr in attrcoll)
                {                   
                    if ( String.Equals(attr.Name ,"VALUE",StringComparison.OrdinalIgnoreCase))
                    {
                        XmlComment newComment;
                        newComment = document.CreateComment(string.Format(" Modified by CLI WinConsole Version:{0} on Date:{1} PREVIOUS_VALUE:{2}  By:{3}", m_sFileVersion, DateTime.Now, attr.Value,System.Environment.UserName));

                        XmlElement element = attr.OwnerElement;
                        element.AppendChild(newComment);
                        attr.Value = Convert.ToString(hashConfigTable[okey]);
                    }
                }
            }

            mySection.SectionInformation.SetRawXml(document.OuterXml);

            //Before save take a backup
           // FileSystemUtil fsutil = new FileSystemUtil();
           // string sNewfilename=string.Format("{0}_{1}.config",Path.GetFileNameWithoutExtension(filepath), DateTime.Now.ToString("yyyyMMMdd_hhmmss"));
          //  fsutil.FileCopy(filepath, Path.Combine(Path.GetDirectoryName(filepath), "Backup", "config", sNewfilename));


            //final Save
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection(sFormatClassName);

            return true;
        }

        private bool IsConfigNodeAvailable(XmlDocument document, string sKey)
        {
            
            XmlNodeList nodelist = document.SelectNodes("//add");

            foreach (XmlNode node in nodelist)
            {
                XmlAttributeCollection attrcoll = node.Attributes;
                string valuefromgui = string.Empty;               
                foreach (XmlAttribute attr in attrcoll)
                {
                    if (String.Equals(attr.Name,"KEY",StringComparison.OrdinalIgnoreCase))
                    {
                        return true;                       
                    }
                }
            }

            return false;
        }

        private XmlNode GetNodeAvailable(XmlDocument document, string sKey)
        {
            XmlNodeList nodelist = document.SelectNodes("//add");

            foreach (XmlNode node in nodelist)
            {
                XmlAttributeCollection attrcoll = node.Attributes;
                string valuefromgui = string.Empty;
                foreach (XmlAttribute attr in attrcoll)
                {
                    if (string.Equals(attr.Name,"KEY",StringComparison.OrdinalIgnoreCase)&& String.Equals(attr.Value,sKey,StringComparison.OrdinalIgnoreCase) )
                    {
                        return node;
                    }
                }
            }

            //create node
            XmlNode xmlnode= document.CreateNode(XmlNodeType.Element,"add","");
            XmlNode xmlnodeattrkey = document.CreateNode(XmlNodeType.Attribute, "key", "");
            XmlNode xmlnodeattrvalue = document.CreateNode(XmlNodeType.Attribute, "value", "");
            
           
            XmlComment newComment;
            newComment = document.CreateComment(string.Format(" Generated/Modified by CLI WinConsole Version:{0} on Date:{1} By:{2}", m_sFileVersion, DateTime.Now,System.Environment.UserName));

            xmlnode.Attributes.Append((XmlAttribute)xmlnodeattrkey);
            xmlnode.Attributes.Append((XmlAttribute)xmlnodeattrvalue);

            ((XmlAttribute)xmlnodeattrkey).Value = sKey;

            XmlElement root = document.DocumentElement;
            root.AppendChild(newComment);
            root.AppendChild(xmlnode);            
          
            return xmlnode;
        }

        #endregion

        private void BtnLoadparam_Click(object sender, EventArgs e)
        {

            if (this.Engine == null)
            {
                System.Windows.Forms.MessageBox.Show(string.Format(@"Select CLI Driver."), "Info");
                return;
            }

            //Load Configuraton from Config table
            //Config HashTable
            Hashtable GlobalConfigTable = new Hashtable();
            Hashtable LocalConfigTable = new Hashtable();
            GetAppConfiguration("GLOBAL_CONFIG", GlobalConfigTable,tbConfigFile.Text);
            GetAppConfiguration(this.SelectedDriver, LocalConfigTable, tbConfigFile.Text);

            MergeConfig(GlobalConfigTable, LocalConfigTable);

            LoadInputArgsFromConfig(LocalConfigTable);
            BuildOptionPanel();

            System.Windows.Forms.MessageBox.Show(string.Format(@"Config File :{0} Loaded successfully.", tbConfigFile.Text),"Info");

        }

        protected virtual void LoadInputArgsFromConfig(System.Collections.Hashtable UserInputConfigTable)
        {
            foreach (string key in this.Engine.CLIOptionsHashTable.Keys)
            {
                ((CLIAppManager.Params.CLIParam)this.Engine.CLIOptionsHashTable[key.ToUpper()]).ParamValue = UserInputConfigTable[key.ToUpper()];

            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show("By closing the Winform , CLI Process can not be stopped. There is a possibility that it is might be running in background.", "Warning");
        }

        private void BtnSaveConfig_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show(string.Format(@"Work in progress - NOT IMPLEMENTED"), "Info");
            //return;

            Hashtable ht = new Hashtable();

            foreach (Control c in this.panel1.Controls)
            {
                CLIAppManagerWinForm.CLIOptionUserControl usercontrol = (CLIAppManagerWinForm.CLIOptionUserControl)c;

                ht.Add(usercontrol.ParamNameValue, usercontrol.Value);
            }

            SaveAppConfigurationAll(this.SelectedDriver, ht, tbConfigFile.Text);

            System.Windows.Forms.MessageBox.Show(string.Format(@"Save successfull. [{0}]", tbConfigFile.Text), "Info");
            return;
        }

        /// <summary>
        /// http://www.claassen.net/geek/blog/2005/06/log4net-scrollingtextbox.html
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            logWatching = false;
            if (logWatcher!=null) logWatcher.Join(); 
        }

      
    }
}
