using System;
using System.Collections.Generic;
using System.IO;

namespace Crozzle_App
{
     public class Crozzle
    {
        public string FileName = "";
        public string Style = "";

        public Dictionary<string, string> Variables;
        public Dictionary<string, Int32> IntVariables;

        internal Dictionary<Word,string> CrozzleWords = new Dictionary<Word, string>();
        public List<string> WordOccurences;
        Log log = Log.GetInstance();

        public Boolean Valid = true;

        public Crozzle()
        {
            Variables = new Dictionary<string, string>();
            IntVariables = new Dictionary<string, int>();
            WordOccurences = new List<string>();
        }

        public void SetFileName(string filename)
        {
            FileName = filename;
        }
        /// <summary>
        /// This method returns a WordList object 
        /// It checks if the filename is relative and absolute.
        /// Creates a new instance of wordList with file path
        /// and returns it
        /// </summary>
        /// <returns></returns>
        public WordList GetWordList()
        {
            string file = Variables["WORDLIST_FILE"];
            string newpath = "";
            if (Path.IsPathRooted(file) == false)
            {
                file = file.Substring(1, file.Length - 1);
                newpath = Path.GetDirectoryName(FileName) + file;
            }
            else
            {
                file = file.Substring(1, file.Length - 1);
                newpath = file;
            }
            return new WordList(newpath);
        }

        /// <summary>
        /// This method returns a configuration object 
        /// Checks if the path is relative or absolute
        /// </summary>
        /// <returns></returns>
        public Configuration GetConfiguration()
        {
            string file = Variables["CONFIGURATION_FILE"];
            string newpath = "";
            if (Path.IsPathRooted(file) == false)
            {
                file = file.Substring(1, file.Length - 1);
                newpath = Path.GetDirectoryName(FileName) + file;
            }
            else
            {
                file = file.Substring(1, file.Length - 1);
                newpath = file;
            }
            return new Configuration(newpath);
        }

        /// <summary>
        /// This method reads the content from crozzle file
        /// Validates the crozzle file
        /// if It marks it invalid, Score will not be calculated and Validation with Configuration file will not work
        /// </summary>
        public void ReadCrozzleFile()
        {
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);

            string line;
            while ((line = sr.ReadLine()) != null)
            {
                line = line.Trim();
                if (line.StartsWith("//"))
                {
                    continue;
                }
                else
                {
                    if (line.Length > 0)
                    {
                        if (line.IndexOf('/') > 0)
                        {
                            var startOfComment = line.IndexOf('/');
                            line = line.Remove(startOfComment);
                            line = line.Trim();
                        }
                        string[] keyAndValue = line.Split(new char[] { '=', ',' });
                        if (keyAndValue.Length == 2)
                        {
                            if (keyAndValue[1].StartsWith("\""))
                            {
                                keyAndValue[1] = keyAndValue[1].Substring(1, keyAndValue[1].Length - 2);
                            }
                            try
                            {
                                if(keyAndValue[0] == "ROWS"|| keyAndValue[0] == "COLUMNS")
                                {
                                    IntVariables.Add(keyAndValue[0], Convert.ToInt32(keyAndValue[1]));
                                }
                                else
                                {
                                    Variables.Add(keyAndValue[0].Trim(), keyAndValue[1].Trim());
                                }
                            }
                            catch(FormatException)
                            {

                            }
                            
                            
                        }
                        if (keyAndValue.Length == 4)
                        {
                            try
                            {
                                var a = Convert.ToInt32(keyAndValue[1]);
                                var b = Convert.ToInt32(keyAndValue[3]);
                                CrozzleWords.Add(new Word(keyAndValue[0], a, keyAndValue[2], b),keyAndValue[3]);
                                WordOccurences.Add(keyAndValue[2]);


                            }
                            catch (System.FormatException)
                            {
                                this.Valid = false;
                                log.WriteLine("Invalid Crozzle File:  " + keyAndValue[0] + " = " + keyAndValue[1] + "," + keyAndValue[2] + "," + keyAndValue[3] + " is not a valid word data");
                            }
                        }
                    }
                }
            }
        }
    }
}




