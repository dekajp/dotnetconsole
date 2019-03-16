using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace CLIAppManager.Utils
{
    public class ExceptionInfoUtil
    {
        public static string GetAllExceptionInfo(Exception ex)
        {
            StringBuilder sbexception = new StringBuilder();

            sbexception.AppendLine(string.Format("=================================================="));
            sbexception.AppendLine(string.Format(" Main Exception.ToString() : {0}{1} ", Environment.NewLine, ex.ToString()));
            sbexception.AppendLine(string.Format("=================================================="));
            
            int i = 1;
            sbexception.Append(GetExceptionInfo(ex, i));

            while (ex.InnerException != null)
            {
                i++;
                ex = ex.InnerException;
                sbexception.Append(GetExceptionInfo(ex, i));
            }

          
            return sbexception.ToString();
        }

        private static string GetExceptionInfo(Exception ex, int count)
        {
            StringBuilder sbexception = new StringBuilder();
            sbexception.AppendLine(string.Format(""));
            sbexception.AppendLine(string.Format(""));
            sbexception.AppendLine(new string('*', 60));            
            sbexception.AppendLine(string.Format(" Inner Exception : No.{0} ", count));
            sbexception.AppendLine(new string('*', 60));
            sbexception.AppendLine(new string('-', 60));
            sbexception.AppendLine(string.Format(" Error Message : {0} ", ex.Message));
            sbexception.AppendLine(new string('-', 60));

            try
            {
                sbexception.AppendLine(new string('-', 60));
                sbexception.AppendLine(string.Format(" Data parameters Count at Source :{0}", ex.Data.Count));
                sbexception.AppendLine(new string('-', 60));

                string skey = string.Empty;
                foreach (object key in ex.Data.Keys)
                {
                    try
                    {
                        if (key != null)
                        {
                            skey = Convert.ToString(key);
                            sbexception.AppendLine(string.Format(" Key :{0} , Value:{1}", skey, Convert.ToString(ex.Data[key])));
                        }
                        else
                        {
                            sbexception.AppendLine(string.Format(" Key is null"));
                        }
                    }
                    catch (SerializationException se)
                    {
                        //If ex.Data[] contains non serializable item - it will throw exception
                        sbexception.AppendLine(string.Format("**  Exception occurred when writting log *** [{0}] ", se.Message));
                    }
                    catch (Exception e1)
                    {
                        sbexception.AppendLine(string.Format("**  Exception occurred when writting log *** [{0}] ", e1.Message));
                    }
                }
            }
            catch (Exception ex1)
            {
                sbexception.AppendLine(string.Format("**  Exception occurred when writting log *** [{0}] ", ex1.Message));
            }


            sbexception.AppendLine(new string('-', 60));
            sbexception.AppendLine(string.Format(" Source : {0} ", ex.Source));
            sbexception.AppendLine(new string('-', 60));
            sbexception.AppendLine(string.Format(" StackTrace : {0} ", ex.StackTrace));
            sbexception.AppendLine(new string('-', 60));
            sbexception.AppendLine(string.Format(" TargetSite : {0} ", ex.TargetSite));
            sbexception.AppendLine(new string('-', 60));
            sbexception.AppendLine(string.Format(" Finished Writting Exception info :{0} ", count));
            sbexception.AppendLine(new string('-', 60));
            sbexception.AppendLine(string.Format("************************************************"));
            sbexception.AppendLine(string.Format(""));
            sbexception.AppendLine(string.Format(""));

            return sbexception.ToString();

        }
    }
}
