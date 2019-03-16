using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLIAppManager
{

    public enum CLIParamType { FILE, FOLDER, PASSWORD, FREE_TEXT,TRUE_FALSE,LIST,INT}  // FLOAT , LOOKUP

    public abstract class CLIParamBase
    {

        public CLIParamBase()
        {
            this.ParameterType = CLIParamType.FREE_TEXT;
            this.ParamDefaultValue = string.Empty;
            this.ParamRequired = false;
            this.ParamValue = string.Empty;
        }
    
        public string ParamName { get; set; } //Name of parameter
        public string ParamDesc { get; set; } //Description of Parameter
        public bool ParamRequired { get; set; }//Whether the parameter is required or not
        public object ParamValue { get; set; }//Value of Parameter
        public object ParamDefaultValue { get; set; }//Default Value
        public CLIParamType ParameterType { get; set; } //Parameter Type Array holds CLIParamType

        List<string> _paramList;
        public List<string> ParamList
        {  
            get
            {
                return _paramList ?? (_paramList = new List<string>());
            }
            set
            {
                _paramList = value;
            }
        }
        abstract public string ParamInfo(); //info about the parameter
        
    }
}
