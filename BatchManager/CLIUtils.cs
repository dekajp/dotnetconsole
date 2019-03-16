using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace CLIAppManager
{
    public class CLIUtils
    {
        public CLIUtils(){
        }
        
      
        public static string GetAllExceptionInfo(Exception ex)
        {
            return CLIAppManager.Utils.ExceptionInfoUtil.GetAllExceptionInfo(ex);
        }

    }
}
