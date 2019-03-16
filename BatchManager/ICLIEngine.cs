using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace CLIAppManager
{
    public interface ICLIEngine
    {
        int Main(string[] args, Hashtable configTable, string szRunKey, string szAppIdentifier);             
    }
}
