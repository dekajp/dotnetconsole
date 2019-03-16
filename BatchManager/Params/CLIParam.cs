using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLIAppManager.Params
{
    public class CLIParam : CLIParamBase
    {
        public CLIParam()
        {
        }

        public override string ParamInfo()
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Empty);
            sb.AppendLine(string.Format("\t{0 ,-25}:{1}","CLI Parameter Name", this.ParamName));
            sb.AppendLine(string.Format("\t{0 ,-25}:{1}", "Is Required", this.ParamRequired));
            sb.AppendLine(string.Format("\t{0 ,-25}:{1}", "Default Value", this.ParamDefaultValue));
            sb.AppendLine(string.Format("\t{0 ,-25}:{1}", "[Value]", this.ParamValue));
            sb.AppendLine(string.Format("\t{0 ,-25}:{1}", "Parameter Type", this.ParameterType));
            sb.AppendLine(string.Format("\t{0 ,-25}:{1}", "Description", this.ParamDesc));
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }
    }
}
