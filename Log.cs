using System;
using System.Collections.Generic;
using System.IO;

namespace Crozzle_App
{
    public class Log
    {
        Form2 f; 
        public string FileName = "";
        List<string> Errors = new List<string>();

        private static Log _instance;
        public Log()
        {
            f = Form2.GetInstance();
        }

        /// <summary>
        /// This method is meant to return the current instance, if it is the first time create a new instance.
        /// </summary>
        /// <returns></returns>
        public static Log GetInstance()
        {
            if (_instance == null) _instance = new Log();
            return _instance;
        }

        /// <summary>
        /// This method is meant to Write all information like errors and messages. It is to be noted that
        /// it does not write on file every time. For better performance, it writes two times, after file validation and 
        /// after crozzle validation
        /// </summary>
        /// <param name="str"></param>
        public void WriteLine(string str)
        {
            Errors.Add(str);
            
            string error = "<span>" + str + "</span><br/>";
            f.WriteLine(error);
            f.Show();
            
        }
        
        /// <summary>
        /// This method is called to write all Errors in log file.
        /// </summary>
        public void WriteLogToFile()
        {

            StreamWriter writer = File.AppendText(FileName);

            foreach (string str in Errors)
            {
                writer.WriteLine(str);
            }
            writer.Close();

        }

        public override string ToString()
        {
            return base.ToString();
        }

    }

}
