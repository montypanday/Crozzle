using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crozzle_App
{
    public class WordList
    {
        public bool valid = true;
        public string FileName = "";
        public HashSet<string> Words;
        const string onlyAlphabets = "^[a-zA-Z]+$";
        Log log = Log.GetInstance();
        public int Count; // this count is meant for all entries in word list, valid and invalid words. Later used for finding more errors.

        public WordList(){}
        /// <summary>
        /// It accepts the filename of word list file 
        /// </summary>
        /// <param name="filename"></param>
        public WordList(string filename)
        {
            FileName = filename;
            valid = true;
            Words = new HashSet<string>();
        }        
        /// <summary>
        /// This method stores the list of words in a hashset
        /// Therefore it cannot have duplicates.
        /// </summary>
        public void GetWords()
        {
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);

            string str = sr.ReadToEnd();

            string[] keyAndValue = str.Split(new char[] { ',' });
            Count = keyAndValue.Length;
            for(int i=0; i<=keyAndValue.Length-1; i++)
            {   
                Regex reg = new Regex(onlyAlphabets);
                Match match = reg.Match(keyAndValue[i]);

                if(!(match.Success))
                {
                    log.WriteLine("Invalid Word List:  {" + keyAndValue[i] + "}  is not a valid word");
                }
                if (!(Words.Add(keyAndValue[i])))
                {
                    valid = false;
                    log.WriteLine("Invalid Word List:  {" + keyAndValue[i] + "}  comes more than once");
                }
            }
            keyAndValue = null;
           
            sr.Close();
            fs.Close();
        }
    }
}
