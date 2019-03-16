using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Security.Permissions;
using CLIAppManager.Params;
using System.IO;


namespace CLIAppManager
{
   
    public enum ExitCode : int
    {/* same code is replicated in CLIConsole */
        Success = 0,
        Failure = 1
    }

    /// <summary>
    /// Inspired from :  http://msdn.microsoft.com/en-us/magazine/cc164014.aspx
    /// </summary>
    public abstract class CLIEngineBase :ICLIEngine
    {
        private string[] m_args;
        protected bool ReadInput = true;
        protected System.IO.TextReader In = null;
        protected System.IO.TextWriter Out = null;
        protected System.IO.TextWriter Error = null;
        public System.Collections.Hashtable UserInputConfigTable = null; // Parameters from Config file
        public System.Collections.Hashtable CLIOptionsHashTable = new System.Collections.Hashtable(); //Parameters from Command Shell
       // private string base_folder = "C:\\temp"; //Base working directory

        public bool AllowGlobalCLIParams { get; set; }

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool validateArgsResult = true;
        
        /// <summary>
        /// Unique Indentifier for Running Instance of CLI Program
        /// Uniquely identify a CLI run , this is generated at very begining of CLI program 
		/// and remains same thoughout the life of the "Run"
        /// </summary>
        public string RunKey { get; set; }

        public string DEFAULT_WORKING_FOLDER
        {
            get
            {
                return "c:\\temp";
            }
        }

        /// <summary>
        /// Uniquely Identify the CLI Program , it is set at CLI Console Level
        /// </summary>
        public string CLIIdentifier { get; set; }

        public CLIEngineBase()
        {
            //by default, read from/write to standard streams
            this.In = System.Console.In;
            this.Out = System.Console.Out;
            this.Error = System.Console.Error;
        }


        /// <summary>
        /// Main Process 
        /// 
        /// ===========================================
        /// LIFE CYCLE OF MAIN PROCESS
        /// 
        /// INIT ==> where driver defines parameters
        /// POST INIT ==> clean up / post analysis of parameters
        /// PRE PROCESS ==> your choice  : instantiate the CLI process main class with properties
        /// PROCESS  ==> WHERE DRIVER CALLS THE CLI process library "Main-Start" method
        /// POST PROCESS ==> cleanup
        /// 
        /// =============================================
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="configTable"></param>
        public int Main(string[] args , Hashtable configTable,string szRunKey, string szAppIdentifier)
        {
            int iReturn= (int)ExitCode.Success;
            bool isProcessRequired = true;
            try
            {
                logger.InfoFormat(" CLI Processing started.");

                this.m_args = args;
                this.UserInputConfigTable = configTable;
                this.RunKey = szRunKey;
                this.CLIIdentifier = szAppIdentifier;

                //Pre-Init () - Not yet defined

                //Global CLI Parameters will [Yes/Not] added
                this.AllowGlobalCLIParams = false; //NO

                //Initialize the CLI Options
                Init();

                //Load Global Parameters - Child class must able to say - do not give me 
                // these extra parameters , so we need to define a Flag
                LoadGlobalParams();

                //Override in Child class for parameter cleanup or some info override
                PostInit();

                //Parse the Arguments - Parameter Handling
                
                ParseInputArgs(out isProcessRequired);


                if (isProcessRequired && this.ValidateArguments())
                {
                    #region Pre Process
                    try
                    {
                        this.PreProcess();
                    }
                    catch (Exception expre)
                    {
                        logger.Error(String.Format("Exception occurred in PreProcess . Check Driver PreProcess Method."));
                        logger.Error(String.Format("{0}", expre.ToString()));
                        throw new Exception("Method : Main during Preprocess", expre);
                    }

                    #endregion

                    #region Process
                    try
                    {
                        // Main Process
                        this.Process();
                    }
                    catch (Exception expProcess)
                    {
                        logger.Error(String.Format("Exception occurred in Processing . Check Driver Process Method."));
                        logger.Error(String.Format("{0}", expProcess.ToString()));
                        throw ;
                    }
                    #endregion

                    #region Post Process
                    try
                    {                       
                        this.PostProcess();
                    }
                    catch (Exception expPostProcess)
                    {
                        logger.Error(String.Format("Exception occurred in Post Processing . Check Driver Post Process Method."));
                        logger.Error(String.Format("{0}", expPostProcess.ToString()));
                        throw new Exception("Method : Main during PostProcess", expPostProcess);
                    }
                    #endregion
                }
                else
                {
                    //do nothing

                    // it's good to throw a exception here because it will help in while writting code , and running otherwise 
                    // when i run the program thry VS 2010 and expect the code to run thru process it fails thru here and i do not able 
                    // to read the error messages . also if i throw exception it won't hurt becasue application failed thru the validation
                    // the other alternative is to run output to text file everytime
                    // go to CMD window and run the program
                    // "CLIAppManager" "CLIAppManager.Drivers.SampleCLIProgramDriver" /CONFIG=TRUE

                    logger.Info("CLI Processing failed in Validating Input Parameters.");
                    if (!validateArgsResult) iReturn= (int)ExitCode.Failure;
                }

                logger.Info(" CLI Processing finished.");

            }
            catch (Exception e)
            {
                logger.Error(" Exception occurred in Either PreProcess or Main Process or Post Process.");             
                logger.Error(CLIUtils.GetAllExceptionInfo(e));
                logger.Error(string.Format("Returned with Error Code:{0}", (int)ExitCode.Failure));
                return (int)ExitCode.Failure;
            }
            finally
            {
            }

            return iReturn;
        }

        #region Protected Methods

        protected string[] Arguments
        {
            get { return this.m_args; }
        }

        private void LoadGlobalParams()
        {
            if (!this.AllowGlobalCLIParams) return;

            # region TRACE
            CLIParam TraceParam = new Params.CLIParam
            {
                ParamName = "GBL_TRACE_SWITCH",
                ParamDesc = string.Format(@"
To Trace the Program in Actual Driver/Library.  Now application uses the Log4Net 
for logging and it is True as default. "),
                ParamRequired = true,
                ParamDefaultValue = true,            
                ParameterType = CLIParamType.TRUE_FALSE

            };
            this.CLIOptionsHashTable.Add(TraceParam.ParamName.ToUpper(), TraceParam);
            #endregion

        }

        protected virtual void  PostInit()
        {

           
        }

        protected virtual void Init()
        {
            //Initialize the CLI Parameters Options

            #region NO_NAME
            Params.CLIParam NO_NAMEParam = new Params.CLIParam
            {
                ParamName = "NO_NAME",
                ParamDesc = "No Name Param",
                ParamRequired = true,
                ParamDefaultValue = string.Empty,
                ParamValue = null
            };
            this.CLIOptionsHashTable.Add(NO_NAMEParam.ParamName.ToUpper(), NO_NAMEParam);
            #endregion
        }

        /// <summary>
        /// Basic Validation , if the parameter is required it must have a value
        /// </summary>
        /// <returns></returns>
        protected virtual bool ValidateArguments()
        {
            //there must be a better way to manage this without 2 parameters
            bool bReturn=true;
            bool bTemporaryResult=true;


            foreach (string sKey in this.CLIOptionsHashTable.Keys)
            {
                Params.CLIParam CLIParam = (Params.CLIParam) this.CLIOptionsHashTable[sKey];


                if (CLIParam.ParamRequired)
                {
                    #region Required Params Check
                    if (CLIParam.ParamValue != null && Convert.ToString(CLIParam.ParamValue).Length>0 )
                    {
                        //Good data  ?? TODO: Not checking for Empty String , Left it for child class 
                        
                        bTemporaryResult = ParamIsFolderValid(CLIParam);
                        bReturn = bReturn && bTemporaryResult;

                        bTemporaryResult = ParamIsFileValid(CLIParam);
                        bReturn = bReturn && bTemporaryResult;

                        bTemporaryResult = ParamIsTrueFalse(CLIParam);
                        bReturn = bReturn && bTemporaryResult;
                    }
                    else if (CLIParam.ParamDefaultValue != null || Convert.ToString(CLIParam.ParamDefaultValue).Length > 0)
                    {
                        logger.Info(String.Format("Parameter {0} is missing. Assinging default Value {1}", CLIParam.ParamName, CLIParam.ParamDefaultValue));
                        CLIParam.ParamValue = CLIParam.ParamDefaultValue;
                    }
                    else
                    {
                        logger.Info(String.Format("Parameter {0} is missing.", CLIParam.ParamName));
                        bReturn = false;
                    }
                    #endregion
                }
                else
                {
                    // Non Required Params check
                    bTemporaryResult = ParamIsFolderValid(CLIParam);
                    bReturn = bReturn && bTemporaryResult;

                    bTemporaryResult = ParamIsFileValid(CLIParam);
                    bReturn = bReturn && bTemporaryResult;

                    bTemporaryResult = ParamIsTrueFalse(CLIParam);
                    bReturn = bReturn && bTemporaryResult;

                    bTemporaryResult = ParamIsList(CLIParam);
                    bReturn = bReturn && bTemporaryResult;
                }
            }

            if (!bReturn) 
            {
                //write All parameters since it failed
                #region write params
                try
                {
                    WriteParameterWithValue();
                }
                catch
                {
                    
                }

                #endregion
            }

            validateArgsResult = bReturn;
            return bReturn;
        }
        
        
        #region Parameters Handling

        private void GetParamNameAndValue(string sInput, out string sParamName, out string sValue)
        {
            sParamName = string.Empty;
            sValue = string.Empty;
           
            //All parameters must start from "/"
            if(sInput.Substring(0,1).Equals("/") && sInput.IndexOf("=",0) > 0){  
                
                //remove the / form sInput
                sInput = sInput.Substring(1, sInput.Length - 1);

                char [] splitSeparator=  {'='};

                sParamName = sInput.Substring(0, sInput.IndexOf("=", 0));
                sValue = sInput.Substring(sInput.IndexOf("=", 0)+1);

                //String[] sParamKeyValuePair = sInput.Split(splitSeparator);

                //if (sParamKeyValuePair.Length == 2)
                //{
                //    sParamName = sParamKeyValuePair[0];
                //    sValue = sParamKeyValuePair[1];
                //}
                //else
                //{
                //    //Invalid Parameter
                //    throw (new Exception(String.Format("Invalid Parameter Length , Must be 2. Input:{0} , Length={1} .", sInput , sParamKeyValuePair.Length)));
                //}

            }else{
                //Invalid Parameter
                throw ( new Exception("Invalid Parameter :"+sInput));
            }
        }

        /// <summary>
        /// Load the Parameters value from the config file.
        /// the config hashtable is already loaded in the memory in start of the program , here the code is copying the values into the CLI parameters
        /// </summary>
        protected virtual void LoadInputArgsFromConfig()
        {
            foreach (string key in CLIOptionsHashTable.Keys)
            {
                ((Params.CLIParam)CLIOptionsHashTable[key.ToUpper()]).ParamValue = UserInputConfigTable[key.ToUpper()];
                 
            }
        }


        protected virtual void LoadInputFromConsole()
        {
            foreach (string key in CLIOptionsHashTable.Keys)
            {
                logger.Info(((Params.CLIParam)CLIOptionsHashTable[key.ToUpper()]).ParamInfo());

                string line;
                logger.Info(string.Empty);

               // do { 
                logger.Info("Provide input value : <Press Enter for Default> ");
                    line = Console.ReadLine();
                    if (line != null)
                    {
                        if (line.Length == 0)
                        {
                            logger.Info("      " + line);
                            ((Params.CLIParam)CLIOptionsHashTable[key.ToUpper()]).ParamValue = ((Params.CLIParam)CLIOptionsHashTable[key.ToUpper()]).ParamDefaultValue;
                            logger.Info(((Params.CLIParam)CLIOptionsHashTable[key.ToUpper()]).ParamInfo());
                        }
                        else
                        {
                            logger.Info("      " + line);
                            ((Params.CLIParam)CLIOptionsHashTable[key.ToUpper()]).ParamValue = line;
                            logger.Info(((Params.CLIParam)CLIOptionsHashTable[key.ToUpper()]).ParamInfo());
                        }
                    }
                //} while (line != null);
            }
        }

        /// <summary>
        /// Parse the Input Args
        /// Check for Help ?
        /// Check if Config File needs to be used for parameters         /// 
        /// </summary>
        /// <param name="isProcessRequired"> Whether full processing is required or not . Diffrentiate Help and CLI Execution </param>
        private void ParseInputArgs(out bool isProcessRequired)
        {
            //Parse the input argument
            string sParamName = string.Empty;
            string sValue = string.Empty;
            isProcessRequired = false;

            if (this.m_args == null) return;
            //Check if input is about the ?  - help command
            if (this.m_args.Length == 1 && this.m_args[0].Equals("?"))
            {
                this.Usage();
                
                return;
            }
            
            //TODO : Reserved KeyWords /P or /CONFIG            
            isProcessRequired = true;
            if (this.m_args.Length == 1 && String.Equals(this.m_args[0] ,"/CONFIG=TRUE", StringComparison.OrdinalIgnoreCase))
            {
                this.LoadInputArgsFromConfig();
                return; // this return is actually avoiding overriding feature 

                // If Config parameters needs to be overriden  , it can be done by command parameters // this feature is not yet implemented
            }

            if (this.m_args.Length == 1 && String.Equals(this.m_args[0],"/P",StringComparison.OrdinalIgnoreCase))
            {

                this.LoadInputFromConsole();
                return;
            }

            logger.InfoFormat("Total Length of Parameters :{0}", this.m_args.Length);
            string sDebugParamInfo = string.Empty;
            //Actual Processing of All inputs
            for (int i = 0; i < this.m_args.Length; i++)
            {
                try
                {                   

                    GetParamNameAndValue(m_args[i], out sParamName, out sValue);

                    sDebugParamInfo += string.Format("Parameter :{0}, Value:{1}", sParamName, sValue);
                    //TODO : All params including reserved should be handled here , after all they all are parameters
                    if (IsKeyWord(sParamName))
                    {
                    }
                    else
                    {
                        ((Params.CLIParam)this.CLIOptionsHashTable[sParamName.ToUpper()]).ParamValue = sValue;
                    }
                }
                catch 
                {
                    string serror = string.Empty;
                    if (sParamName.Trim().Length < 1)
                    {
                        serror=string.Format(@"
Error : Parameter name is blank or this could be result of space in parameter value. 
Use Double quotes to send parameters which has space.
e.g param=c:\Program Files   (incorrect)
    param=""c:\Program Files"" (Correct)
");
                    }
                    throw (new Exception(String.Format("CLIOption Mismatch. Parameter {0} Not found.{1}{2}{3}{4} ", sParamName, System.Environment.NewLine, serror, System.Environment.NewLine,sDebugParamInfo)));
                }
            }
        }

        private bool IsKeyWord(string szKey){
            if (String.Equals(szKey,"CONFIG",StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/system.io.filesysteminfo.aspx
        /// </summary>
        /// <param name="CLIParam"></param>
        /// <returns></returns>
        private bool ParamIsFolderValid(Params.CLIParam CLIParam)
        {
            bool bReturn = true;

            #region Folder Check
            //Check if it is Folder ? then check if folder exists 
            if (CLIParam.ParameterType == CLIParamType.FOLDER)
            {
                try
                {
                    FileSystemInfo dirInfo = new DirectoryInfo(Convert.ToString(CLIParam.ParamValue));
                    //Check whether folder exists
                    if (dirInfo.Exists)
                    {
                        //Folder exists -- Good to Go
                    }
                    else
                    {
                        // If default value is supplied then create the folder
                        logger.Info(String.Format("Parameter {0}  - Folder not found at {1}.", CLIParam.ParamName, CLIParam.ParamValue));

                        //Check if default value is supplied                        
                        logger.Info(String.Format("Parameter {0}  - Folder not found at {1}. Default Value of the Parameter:{2}", CLIParam.ParamName, CLIParam.ParamValue,CLIParam.ParamDefaultValue));

                        dirInfo = new DirectoryInfo(Convert.ToString(CLIParam.ParamDefaultValue));
                        if (dirInfo.Exists)
                        {
                            //Great Default value directory exists
                            CLIParam.ParamValue = CLIParam.ParamDefaultValue;
                            logger.Info(String.Format("Parameter {0}  - Folder found at {1}. Default Value of the Parameter:{2}", CLIParam.ParamName, CLIParam.ParamValue, CLIParam.ParamDefaultValue));
                        }
                        else
                        {
                            if (CLIParam.ParamRequired)
                            {
                                throw new Exception("Required folder does not exists." + CLIParam.ToString());
                            }
                        }

                        //System.IO.Directory.CreateDirectory(base_folder + "\\" + Convert.ToString(CLIParam.ParamDefaultValue));
                        //logger.Info(String.Format("Parameter {0}  - Folder created at {1}.", CLIParam.ParamName, base_folder + "\\" + Convert.ToString(CLIParam.ParamDefaultValue)));
                    }
                }
                catch (Exception e)
                {
                    logger.Info(String.Format("Parameter {0}  - error message {1}.", CLIParam.ParamName, e.Message));
                    bReturn = false;
                }

            }
            #endregion
            return bReturn;
        }

        private bool ParamIsTrueFalse(Params.CLIParam CLIParam)
        {
            bool bReturn = true;

            #region Folder Check
            //Check if it is Folder ? then check if folder exists 
            if (CLIParam.ParameterType == CLIParamType.TRUE_FALSE)
            {
                try
                {
                    //Check whether folder exists
                    Convert.ToBoolean(CLIParam.ParamValue);
                    
                }
                catch (Exception e)
                {
                    logger.Info(String.Format("Parameter {0} can only be true or false - error message {1}.", CLIParam.ParamName, e.Message));
                    bReturn = false;
                }

            }
            #endregion
            return bReturn;
        }
        
        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/system.io.filesysteminfo.aspx
        /// </summary>
        /// <param name="CLIParam"></param>
        /// <returns></returns>
        private bool ParamIsFileValid(Params.CLIParam CLIParam)
        {
            bool bReturn = true;

            #region File Check
            //Check if it is File ? then check if File exists 
            if (CLIParam.ParameterType == CLIParamType.FILE)
            {
                try
                {
                    FileSystemInfo fsinfo = new FileInfo(Convert.ToString(CLIParam.ParamValue));
                    //Check whether File exists
                    if (fsinfo.Exists)
                    {
                    }
                    else
                    {
                        // show error message
                        logger.Info(String.Format("Parameter {0}  - File does not exists at {1}", CLIParam.ParamName,CLIParam.ParamValue));
                        bReturn = false;
                    }
                }
                catch (Exception e)
                {
                    logger.Info(String.Format("Parameter {0}  - error message {1}.", CLIParam.ParamName, e.Message));
                    bReturn = false;
                }

            }
            #endregion

            return bReturn ;
        }

        private bool ParamIsList(Params.CLIParam CLIParam)
        {
            bool bReturn = true;

            #region Folder Check
            //Check if it is Folder ? then check if folder exists 
            if (CLIParam.ParameterType == CLIParamType.LIST)
            {
                try
                {
                    if (CLIParam.ParamList.IndexOf(CLIParam.ParamValue.ToString()) > 0)
                    {
                        bReturn = true;
                    }
                    else
                    {
                        bReturn = false;
                    }
                }               
                catch (Exception e)
                {
                    logger.Info(String.Format("Parameter {0} can only be true or false - error message {1}.", CLIParam.ParamName, e.Message));
                    bReturn = false;
                }

            }
            #endregion
            return bReturn;
        }
        #endregion

        /// <summary>
        /// Before running the CLI process spits out all the parameters
        /// </summary>
        protected virtual void PreProcess()
        {
            //Preprocessing Note :
            logger.Info("");
            logger.Info("PreProcessing Started.");
            logger.Info("Unique RunKey :"+this.RunKey);
            logger.Info("Application Identifier (CLI Identifier) :" + this.CLIIdentifier);
            logger.Info("Date Time  :" + System.DateTime.Now.ToString("F")); 
            logger.Info("");

            WriteParameterWithValue();

            return;
        }

        protected virtual void PostProcess()
        {
            //override this to add custom logic that 
            //executes just after standard in is processed

            //Preprocessing Note :
            logger.Info("");
            logger.Info("Post Processing Started.");
            logger.Info("");
            return;
        }


        public string GetVersionInfo(string sname)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            AssemblyName[] name = assembly.GetReferencedAssemblies();

            for (int i = 0; i < name.Length; i++)
            {
                AssemblyName asm = name[i];
                if (String.Equals(asm.Name, sname, StringComparison.OrdinalIgnoreCase))
                {
                    return asm.Version.ToString();
                }

            }

            return "Version not found for " + sname;
        }

        abstract protected void Process();

        abstract protected void Usage();


      
        public object GetParameterValueFromDictionary(string sParamConfigKeyToUpper)
        {
            CLIParam bp = null;
            try
            {
                if (this.CLIOptionsHashTable == null)
                {
                    logger.Error("CLIOptionsHashTable is null.");
                }
                else
                {
                    bp = ((CLIParam)this.CLIOptionsHashTable[sParamConfigKeyToUpper.ToUpper()]);
                    logger.InfoFormat("Parameter Key :{0} found in CLI Parameter Dictionary Value:{1}", sParamConfigKeyToUpper,(bp.ParameterType==CLIParamType.PASSWORD ? "XXX-XXX-XXX": bp.ParamValue));
                    return bp.ParamValue;
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Parameter Key :{0} not found in the CLI Parameter Dictionary",sParamConfigKeyToUpper),ex);
            }

            return string.Empty;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sParamConfigKeyToUpper"></param>
        /// <returns></returns>
        public bool RemoveParameterFromConfigDictionary(string sParamConfigKeyToUpper)
        {
            try
            {
                if (this.CLIOptionsHashTable == null)
                {
                    logger.Error("CLIOptionsHashTable is null.");
                }
                else
                {
                    if (this.CLIOptionsHashTable[sParamConfigKeyToUpper.ToUpper()]==null){
                        logger.WarnFormat("Removal failed.Parameter Key :{0} not found in the CLI Parameter Dictionary", sParamConfigKeyToUpper);
                    }else{
                        this.CLIOptionsHashTable.Remove(sParamConfigKeyToUpper.ToUpper());
                        logger.InfoFormat(@"Prameter Key:{0} removed", sParamConfigKeyToUpper);
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.Warn(string.Format("Removal failed.Parameter Key :{0} not found in the CLI Parameter Dictionary", sParamConfigKeyToUpper), ex);
                return false;
            }

            return false;
        }

        public void WriteParameterWithValue()
        {
            //Security Note:
            logger.Info("");
            logger.Info("--------------------------------------------------------------------------------");
            logger.Info("Security Note :");
            logger.Info("--------------------------------------------------------------------------------");
            logger.Info("Log file may contains Database trusted connections information(e.g server name , user id or password) or FTP Server information.");
            logger.Info("");

            //Parameter Section
            logger.Info("");
            logger.Info("--------------------------------------------------------------------------------");
            logger.Info("Parameters Info");
            logger.Info("--------------------------------------------------------------------------------");
            logger.Info("");
            int iParamCount = 0;
            logger.Info(String.Format("{0,-6}{1,-30}{2,-100}", "COUNT", "PARAMETER NAME", "PARAMETER VALUE"));
            logger.Info("--------------------------------------------------------------------------------");

            Hashtable settings = this.CLIOptionsHashTable;
            ArrayList keys = new ArrayList();
            keys.AddRange(settings.Keys);
            keys.Sort();
            foreach (object key in keys)
            {
                iParamCount++;
                Params.CLIParam bp = (CLIAppManager.Params.CLIParam)this.CLIOptionsHashTable[key];
                if (bp.ParameterType == CLIParamType.PASSWORD)
                {
                    logger.Info(String.Format("{0,-6}{1,-30}{2,-100}", iParamCount, bp.ParamName, "XXX-XXXX-XXXX"));
                }
                else
                {
                    logger.Info(String.Format("{0,-6}{1,-30}{2,-100}", iParamCount, bp.ParamName, bp.ParamValue));
                }
            }
            logger.Info("");
            logger.Info("");
            logger.Info("");
        }

        public void WriteParameterHelpInfo()
        {
            //Parameter Section
            logger.Info("");
            logger.Info("-----------------------------------------------");
            logger.Info("Parameters Info");
            logger.Info("-----------------------------------------------");

            int iParamCount = 0;
            Hashtable settings = this.CLIOptionsHashTable;
            ArrayList keys = new ArrayList();
            keys.AddRange(settings.Keys);
            keys.Sort();
            foreach (object key in keys)
            {
                iParamCount++;
                Params.CLIParam bp = (CLIAppManager.Params.CLIParam)this.CLIOptionsHashTable[key];
                logger.Info(String.Format("Param ({0})==> {1}", iParamCount, bp.ParamInfo()));

            }
        }

        #endregion
    }

}
