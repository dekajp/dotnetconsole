using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CLIAppManager.Params;
using System.Reflection;

namespace CLIAppManager.Drivers
{
    public class SampleCLIProgramDriver : CLIEngineBase 
    {
        //Main Worker
        SampleCLIProgram.ProcessMain CLIProcessMain;
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SampleCLIProgramDriver()
        {

        }


        protected override void Init()
        {

            //Allow the Global Parameters
            base.AllowGlobalCLIParams = true;           
            
            //Create all CLI parameters
            #region CLI Parameters

            # region SAMPLE_PARAM_1  
            //TODO ://This needs to move to Base Class
            //Default is FREE text
            CLIParam SAMPLE_PARAM_1Param = new Params.CLIParam
            {
                ParamName = "SAMPLE_PARAM_1",
                ParamDesc = "This is a sample parameter",
                ParamRequired = true,
                ParamDefaultValue = "DEFAULT_VALUE_OF_PARAMETER_IF_ANY"    
            };
            this.CLIOptionsHashTable.Add(SAMPLE_PARAM_1Param.ParamName.ToUpper(), SAMPLE_PARAM_1Param);
            #endregion
            
            # region TRUEFALSE_PARAM_1
            //TODO ://This needs to move to Base Class
            //Default is FREE text
            CLIParam TRUEFALSE_PARAM_1Param = new Params.CLIParam
            {
                ParamName = "TRUEFALSE_PARAM_1",
                ParamDesc = "This is a sample parameter of TRUE or FALSE",
                ParamRequired = true,
                ParamDefaultValue = "TRUE",                
                ParameterType=CLIParamType.TRUE_FALSE

            };
            this.CLIOptionsHashTable.Add(TRUEFALSE_PARAM_1Param.ParamName.ToUpper(), TRUEFALSE_PARAM_1Param);
            #endregion

            # region LOOKUP_PARAM
            CLIParam LOOKUP_PARAM = new Params.CLIParam
            {
                ParamName = "LOOKUP_PARAM",
                ParamDesc = "LOOKUP",
                ParamRequired = true,
                ParamDefaultValue = "LIST_VALUE_2",              
                ParameterType = CLIParamType.LIST
            };
            LOOKUP_PARAM.ParamList.Add("LIST_VALUE_1");
            LOOKUP_PARAM.ParamList.Add("LIST_VALUE_2");
            LOOKUP_PARAM.ParamList.Add("LIST_VALUE_3");
            this.CLIOptionsHashTable.Add(LOOKUP_PARAM.ParamName.ToUpper(), LOOKUP_PARAM);
            #endregion

            #endregion

        }

        protected override void PostInit()
        {
            base.PostInit();
        }    

        protected override void PreProcess()
        {
            base.PreProcess();

            CLIProcessMain = new SampleCLIProgram.ProcessMain
            {
                SampleParam = Convert.ToString(base.GetParameterValueFromDictionary("SAMPLE_PARAM_1")),
                //FolderSampleParam = Convert.ToString(base.GetParameterValueFromDictionary("FOLDER_PARAM_1")),
                //FileSampleParam = Convert.ToString(base.GetParameterValueFromDictionary("FILE_PARAM_1")),
                LOOKUP_PARAMSampleParam = Convert.ToString(base.GetParameterValueFromDictionary("LOOKUP_PARAM")),
                TRUEFALSE_PARAMSampleParam = Convert.ToBoolean(base.GetParameterValueFromDictionary("TRUEFALSE_PARAM_1")),
                //PASSWORD_PARAM_1SampleParam = Convert.ToString(base.GetParameterValueFromDictionary("PASSWORD_PARAM_1"))
            };

            CLIProcessMain.RunKey = this.RunKey;
            CLIProcessMain.AppIdentifier = this.CLIIdentifier;
        }

        protected override void Process()
        {

            try
            {
                //Start the DataLoader Process
                CLIProcessMain.Process();
            }
            catch (Exception e)
            {
                logger.Error(e.Message);   
                throw new Exception(string.Format("Method:{0}", e.TargetSite), e);

                //TODO : Event writting of exception
            }
            finally
            {
                CLIProcessMain = null;
            }


            return;
        }

        protected override void Usage()
        {
            string sUsage = string.Empty;
            
            //Create a Blank Object
            //this will be used in Auto release testing 
            CLIProcessMain = new SampleCLIProgram.ProcessMain();
            AssemblyName Assemblyversion = typeof(SampleCLIProgram.ProcessMain).Assembly.GetName();


            //Main Section
            logger.Info("");
            logger.Info(new string('*',60));
            logger.Info("Sample CLI Program for prototyping.");
            logger.Info("");
            logger.Info(String.Format("Version:{0}", Assemblyversion.Version));
            logger.Info("");
            logger.Info(new string('*', 60));
            logger.Info(@"
Sample CLI Program for rapid prototyping 
");

            //Parameter Section
            base.WriteParameterHelpInfo();

            //Sample Usage Section
            logger.Info("");
            logger.Info(new string('-', 60));
            logger.Info("Sample Comamnd : ");
            logger.Info(new string('-', 60));
            logger.Info("Usage : CLIConsole.exe \"CLIAppManager\" \"CLIAppManager.Drivers.SampleCLIProgramDriver\" ?");

            //Sample Usage for APP Config
            logger.Info("");
            logger.Info(new string('-', 60));
            logger.Info("Reserved Options for using CLI Program");
            logger.Info(new string('-', 60));
            logger.Info("/CONFIG=TRUE will use Application Config file to populate all CLIParameters");
            logger.Info("Usage : CLIConsole.exe \"CLIAppManager\" \"CLIAppManager.Drivers.SampleCLIProgramDriver\" /CONFIG=TRUE");
            logger.Info("");
            logger.Info("");
            logger.Info("/P will promp user for input");
            logger.Info("Usage : CLIConsole.exe \"CLIAppManager\" \"CLIAppManager.Drivers.SampleCLIProgramDriver\" /P");
            logger.Info("");
            logger.Info("");
            logger.Info("Provide parameters info thru comamnd line");
            logger.Info("Usage : CLIConsole.exe \"CLIAppManager\" \"CLIAppManager.Drivers.SampleCLIProgramDriver\" /SAMPLE_PARAM_1=HelloWorld");
            logger.Info("");
            logger.Info("");

        }

        public override string ToString()
        {
            return "Sample CLI Application";
        }
    }
}
