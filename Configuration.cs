using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Crozzle_App
{
    public class Configuration
    {
        #region Properties and Initialization
        public string FileName;
        public Dictionary<string, string> Variables;
        public Dictionary<string, Int32> IntVariables;
        public Dictionary<string, Int32> INTERSECTING_POINTS_PER_LETTER;// holds cost metrics for intersecting words
        public Dictionary<string, Int32> NON_INTERSECTING_POINTS_PER_LETTER;// holds cost metrics for non intersecting words
        Dictionary<KeyValuePair<int, int>, int> IntersectionCoordinates;

        static List<string> IntVariablesList = new List<string>(new string[] { "MINIMUM_NUMBER_OF_UNIQUE_WORDS","MAXIMUM_NUMBER_OF_UNIQUE_WORDS",
                                "MINIMUM_NUMBER_OF_ROWS","MAXIMUM_NUMBER_OF_ROWS",
                                "MINIMUM_NUMBER_OF_COLUMNS","MAXIMUM_NUMBER_OF_COLUMNS",
                                "MINIMUM_HORIZONTAL_WORDS","MAXIMUM_HORIZONTAL_WORDS",
                                "MINIMUM_VERTICAL_WORDS","MAXIMUM_VERTICAL_WORDS",
                                "MINIMUM_INTERSECTIONS_IN_HORIZONTAL_WORDS","MAXIMUM_INTERSECTIONS_IN_HORIZONTAL_WORDS",
                                "MINIMUM_INTERSECTIONS_IN_VERTICAL_WORDS","MAXIMUM_INTERSECTIONS_IN_VERTICAL_WORDS",
                                "MINIMUM_NUMBER_OF_THE_SAME_WORD","MAXIMUM_NUMBER_OF_THE_SAME_WORD",
                                "MINIMUM_NUMBER_OF_GROUPS","MAXIMUM_NUMBER_OF_GROUPS",
                                "POINTS_PER_WORD"});
        static List<string> VariablesList = new List<string>(new string[] { "LOGFILE_NAME", "INVALID_CROZZLE_SCORE", "UPPERCASE", "BGCOLOUR_EMPTY_TD", "BGCOLOUR_NON_EMPTY_TD" });

        public string BGCOLOUR_NON_EMPTY_TD;
        public string BGCOLOUR_EMPTY_TD;
        public Boolean UseConfigColours = true;
        public Boolean Valid = true;
        public Boolean UpperCase = true;
        List<string> AtoZ = Enumerable.Range('A', 26)
                              .Select(x => (char)x + "")
                              .ToList();
        char[,] paddedArray;
        public char[,] horizontalPaddedArray;
        char[,] verticalPaddedArray;
        int NumGroups = 1;
        int HorizontalWords = 1;
        int VerticalWords = 1;
        const string onlyColours = "^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$";// Validation for genuine hexadecimal colours
        Log log = Log.GetInstance();

        public Configuration() { }

        public Configuration(string str)
        {
            FileName = str;
            Variables = new Dictionary<string, string>();
            IntVariables = new Dictionary<string, int>();
            IntersectionCoordinates = new Dictionary<KeyValuePair<int, int>, int>();

        }
        public Configuration(char[,] array)
        {
            horizontalPaddedArray = array;
        }
        #endregion
        /// <summary>
        /// This method is divided into regions, each describing the purpose of code inside it
        /// It provides validation if all three files are found valid
        /// If any errors are reported here, it will prevent calculation of score by returning false
        /// </summary>
        /// <param name="crozzle"></param>
        /// <param name="Wordlist"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public bool ValidateCrozzle(Crozzle crozzle, WordList Wordlist, Table table)
        {
            bool valid = true;
            log.WriteLine("==================");
            log.WriteLine("Validation Started");
            log.WriteLine("==================");
            var distinctList = crozzle.WordOccurences.Distinct();


            #region Limits on duplicate words in the crozzle.
            foreach (string str in distinctList)
            {
                Func<string, bool> repeatedWords = delegate (string word) { return word == str; };
                int count = crozzle.WordOccurences.Count(repeatedWords);
                if (count > IntVariables["MAXIMUM_NUMBER_OF_THE_SAME_WORD"])
                {
                    log.WriteLine("Invalid Crozzle: Word {" + str + "} is repeated {" + count + "} times which is more than the allowed limit {" + IntVariables["MAXIMUM_NUMBER_OF_THE_SAME_WORD"] + "}");
                }
            }



            #endregion

            #region Limits on the number of valid word groups.
            int numberOfGroups = GetNumGroups(crozzle, table);
            if (IntVariables["MINIMUM_NUMBER_OF_GROUPS"] > numberOfGroups)
            {
                log.WriteLine("Invalid Crozzle: Invalid {" + numberOfGroups + "} Groups, minimum allowed {" + IntVariables["MINIMUM_NUMBER_OF_GROUPS"] + "} ");
                valid = false;
            }
            if (IntVariables["MAXIMUM_NUMBER_OF_GROUPS"] < numberOfGroups)
            {
                log.WriteLine("Invalid Crozzle: Invalid {" + numberOfGroups + "} Groups, maximum allowed {" + IntVariables["MAXIMUM_NUMBER_OF_GROUPS"] + "} ");
                valid = false;
            }
            #endregion

            #region Check Unique Numbers
            if (IntVariables["MINIMUM_NUMBER_OF_UNIQUE_WORDS"] < 1)
            {
                log.WriteLine("Invalid Configuration: The configuration file must allow altleast one unique word, Currently allows {" + IntVariables["MINIMUM_NUMBER_OF_UNIQUE_WORDS"] + "} ");
                valid = false;
            }
            else
            {
                if (Wordlist.Count < IntVariables["MINIMUM_NUMBER_OF_UNIQUE_WORDS"])
                {
                    log.WriteLine("Invalid Word List: Word List does not have {" + IntVariables["MINIMUM_NUMBER_OF_UNIQUE_WORDS"] + " } unique words");
                    Valid = false;
                }
            }

            if (Wordlist.Words.Count > IntVariables["MAXIMUM_NUMBER_OF_UNIQUE_WORDS"])
            {
                log.WriteLine("Invalid Word List: Word List exceeds Maximum allowed unique Words i.e {, " + IntVariables["MAXIMUM_NUMBER_OF_UNIQUE_WORDS"] + "}");
                valid = false;
            }
            #endregion

            #region Limits on Crozzle Grid
            if (crozzle.IntVariables["ROWS"] < IntVariables["MINIMUM_NUMBER_OF_ROWS"])
            {
                log.WriteLine("Invalid Crozzle: Configuration does not allow Rows {" + crozzle.IntVariables["ROWS"] + "} less than {" + IntVariables["MINIMUM_NUMBER_OF_ROWS"] + "}");
                valid = false;
            }
            if (crozzle.IntVariables["ROWS"] > IntVariables["MAXIMUM_NUMBER_OF_ROWS"])
            {
                log.WriteLine("Invalid Crozzle: Configuration does not allow Rows {" + crozzle.IntVariables["ROWS"] + "} more than {" + IntVariables["MAXIMUM_NUMBER_OF_ROWS"] + "}");
                valid = false;
            }
            if (crozzle.IntVariables["COLUMNS"] < IntVariables["MINIMUM_NUMBER_OF_COLUMNS"])
            {
                log.WriteLine("Invalid Crozzle: Configuration does not allow Columns {" + crozzle.IntVariables["COLUMNS"] + "} less than {" + IntVariables["MINIMUM_NUMBER_OF_COLUMNS"] + "}");
                valid = false;
            }
            if (crozzle.IntVariables["COLUMNS"] > IntVariables["MAXIMUM_NUMBER_OF_COLUMNS"])
            {
                log.WriteLine("Invalid Crozzle: Configuration does not allow Columns more than {" + IntVariables["MAXIMUM_NUMBER_OF_COLUMNS"] + "}");
                valid = false;
            }

            #endregion

            #region Limit on Number of Horizontal Words displayed 
            char[,] array = table.arrayOfWords;
            horizontalPaddedArray = GetPaddedArray(array);

            for (int r = 1; r <= horizontalPaddedArray.GetLength(0) - 2; r++)
            {
                for (int c = 1; c <= horizontalPaddedArray.GetLength(1) - 2; c++)
                {
                    if (Char.IsLetter(horizontalPaddedArray[r, c]))
                    {

                        if (horizontalPaddedArray[r, c - 1] == '\0' && horizontalPaddedArray[r, c + 1] == '\0') { }
                        else
                        {
                            FloodFillHorizontalWord(r, c);
                            HorizontalWords++;
                        }
                    }
                }
            }
            HorizontalWords = HorizontalWords - 1;
            //log.WriteLine(HorizontalWords.ToString());
            if (IntVariables["MINIMUM_HORIZONTAL_WORDS"] > HorizontalWords)
            {
                log.WriteLine("Invalid Crozzle: Number of Horizontal Words {" + HorizontalWords + "} less than minimum {" + IntVariables["MINIMUM_HORIZONTAL_WORDS"] + "}");
                valid = false;
            }
            if (IntVariables["MAXIMUM_HORIZONTAL_WORDS"] < HorizontalWords)
            {
                log.WriteLine("Invalid Crozzle: Number of Horizontal Words {" + HorizontalWords + "} exceeds allowed {" + IntVariables["MAXIMUM_HORIZONTAL_WORDS"] + "}");
                valid = false;
            }

            #endregion

            #region Limits on the number of intersecting vertical words for each horizontal word

            for (int r = 1; r <= horizontalPaddedArray.GetLength(0) - 2; r++)
            {
                for (int c = 1; c <= horizontalPaddedArray.GetLength(1) - 2; c++)
                {
                    if (horizontalPaddedArray[r, c] != '\0')
                    {
                        if ((horizontalPaddedArray[r, c - 1] != '\0' && horizontalPaddedArray[r - 1, c] != '\0') ||
                          (horizontalPaddedArray[r, c - 1] != '\0' && horizontalPaddedArray[r + 1, c] != '\0') ||
                          (horizontalPaddedArray[r, c + 1] != '\0' && horizontalPaddedArray[r - 1, c] != '\0') ||
                          (horizontalPaddedArray[r, c + 1] != '\0' && horizontalPaddedArray[r + 1, c] != '\0'))
                        {
                            IntersectionCoordinates.Add(new KeyValuePair<int, int>(r, c), 1);
                        }
                    }
                }
            }
            Dictionary<int, int> dict = new Dictionary<int, int>();

            foreach (KeyValuePair<int, int> pair in IntersectionCoordinates.Keys)
            {
                var WordNumber = horizontalPaddedArray[pair.Key, pair.Value];
                if (dict.ContainsKey(WordNumber))
                {
                    int initial = dict[WordNumber];
                    initial++;
                    dict[WordNumber] = initial;
                }
                else
                {
                    dict.Add(WordNumber, 1);
                }
            }

            foreach (KeyValuePair<int, int> Pair in dict)
            {
                if (Pair.Value < IntVariables["MINIMUM_INTERSECTIONS_IN_HORIZONTAL_WORDS"])
                {
                    log.WriteLine("Invalid Crozzle: Horizontal Word {" + Pair.Key + "} has {" + Pair.Value + "} intersections, minimum required {" + IntVariables["MINIMUM_INTERSECTIONS_IN_HORIZONTAL_WORDS"] + "}");
                    valid = false;
                }
                if (Pair.Value > IntVariables["MAXIMUM_INTERSECTIONS_IN_HORIZONTAL_WORDS"])
                {
                    log.WriteLine("Invalid Crozzle: Horizontal Word {" + Pair.Key + "} has {" + Pair.Value + "} intersections, maximum allowed {" + IntVariables["MAXIMUM_INTERSECTIONS_IN_HORIZONTAL_WORDS"] + "}");
                    valid = false;
                }

            }
            #endregion

            #region Limit on Number of Vertical Words displayed 

            verticalPaddedArray = GetPaddedArray(array);
            for (int c = 1; c <= verticalPaddedArray.GetLength(1) - 2; c++)
            {
                for (int r = 1; r <= verticalPaddedArray.GetLength(0) - 2; r++)
                {

                    if (Char.IsLetter(verticalPaddedArray[r, c]))
                    {

                        if (verticalPaddedArray[r + 1, c] == '\0' && verticalPaddedArray[r - 1, c] == '\0') { }
                        else
                        {
                            FloodFillVerticalWord(r, c);
                            VerticalWords++;
                        }
                    }
                }
            }
            VerticalWords = VerticalWords - 1;
            if (IntVariables["MINIMUM_VERTICAL_WORDS"] > VerticalWords)
            {
                log.WriteLine("Invalid Crozzle: Number of Vertical Words {" + VerticalWords + "} less than minimum {" + IntVariables["MINIMUM_VERTICAL_WORDS"] + "}");
                valid = false;
            }
            if (IntVariables["MAXIMUM_VERTICAL_WORDS"] < VerticalWords)
            {
                log.WriteLine("Invalid Crozzle: Number of Horizontal Words {" + VerticalWords + "} exceeds allowed {" + IntVariables["MAXIMUM_VERTICAL_WORDS"] + "}");
                valid = false;
            }

            #endregion

            #region Limits on the number of intersecting horizontal words for each vertical word
            Dictionary<int, int> dictVertical = new Dictionary<int, int>();


            foreach (KeyValuePair<int, int> pair in IntersectionCoordinates.Keys)
            {
                var WordNumber = verticalPaddedArray[pair.Key, pair.Value];
                if (dictVertical.ContainsKey(WordNumber))
                {
                    dictVertical[WordNumber]++;
                }
                else
                {
                    dictVertical.Add(WordNumber, 1);
                }
            }

            foreach (KeyValuePair<int, int> Pair in dictVertical)
            {
                if (Pair.Value < IntVariables["MINIMUM_INTERSECTIONS_IN_VERTICAL_WORDS"])
                {
                    log.WriteLine("Invalid Crozzle: Vertical Word {" + Pair.Key + "} has {" + Pair.Value + "} intersections, minimum required {" + IntVariables["MINIMUM_INTERSECTIONS_IN_VERTICAL_WORDS"] + "}");
                    valid = false;
                }
                if (Pair.Value > IntVariables["MAXIMUM_INTERSECTIONS_IN_VERTICAL_WORDS"])
                {
                    log.WriteLine("Invalid Crozzle: Vertical Word {" + Pair.Key + "} has {" + Pair.Value + "} intersections, maximum allowed {" + IntVariables["MAXIMUM_INTERSECTIONS_IN_VERTICAL_WORDS"] + "}");
                    valid = false;
                }

            }





            #endregion

            return valid;
        }
        /// <summary>
        /// This Flood fill algorithm is used for validation of number of Horizontal words
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void FloodFillHorizontalWord(int x, int y)
        {

            horizontalPaddedArray[x, y] = Convert.ToChar(HorizontalWords);
            if (Char.IsLetter(horizontalPaddedArray[x, y + 1]))
            {
                FloodFillHorizontalWord(x, y + 1);
            }

        }
        /// <summary>
        /// This Flood fill algorithm is used to count the number of Vertical Words 
        /// Used for Validation
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void FloodFillVerticalWord(int x, int y)
        {

            verticalPaddedArray[x, y] = Convert.ToChar(VerticalWords);
            if (Char.IsLetter(verticalPaddedArray[x + 1, y]))
            {
                FloodFillVerticalWord(x + 1, y);
            }


        }

        /// <summary>
        /// This method loads the Configuration file, check for any known errors, if errors found
        /// make the configuration file invalid and prevents score calculation and crozzle validation.
        /// </summary>
        /// <param name="crozzle"></param>
        public void GetConfiguration(Crozzle crozzle)
        {
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);

            string line;
            while ((line = sr.ReadLine()) != null)
            {
                //line = line.Trim();
                if (line.StartsWith("//"))
                {
                    continue;
                }
                else
                {
                    if (line.Length > 0)
                    {

                        if (line.Trim().StartsWith("INTERSECTING_POINTS_PER_LETTER"))
                        {
                            try
                            {
                                int i = line.IndexOf("\"");
                                int j = line.LastIndexOf("\"");
                                line = line.Substring(i + 1);
                                line = line.Remove(line.Length - 1);
                                INTERSECTING_POINTS_PER_LETTER = line.Split(new char[] { ',' }).Select(x => x.Split('=')).ToDictionary(y => y[0], y => Convert.ToInt32(y[1]));
                                continue;
                            }
                            catch (FormatException)
                            {
                                log.WriteLine("Invalid Configuration File: Non Integer Value detected in {INTERSECTING_POINTS_PER_LETTER}");
                            }

                        }
                        if (line.Trim().StartsWith("NON_INTERSECTING_POINTS_PER_LETTER"))
                        {
                            try
                            {
                                int i = line.IndexOf("\"");
                                int j = line.LastIndexOf("\"");
                                line = line.Substring(i + 1);
                                line = line.Remove(line.Length - 1);
                                NON_INTERSECTING_POINTS_PER_LETTER = line.Split(new char[] { ',' }).Select(x => x.Split('=')).ToDictionary(y => y[0], y => Convert.ToInt32(y[1]));
                                continue;
                            }
                            catch (FormatException)
                            {
                                log.WriteLine("Invalid Configuration File: Non Integer Value detected in {NON_INTERSECTING_POINTS_PER_LETTER}");
                            }

                        }
                        if (line.StartsWith("STYLE"))
                        {
                            int a = line.IndexOf("<");
                            string b = line.Substring(a);
                            int lastindex = b.LastIndexOf("\"");
                            b = b.Substring(0, lastindex);
                            crozzle.Style = b;
                            continue;
                        }
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
                                if (
                                keyAndValue[0] == "MINIMUM_NUMBER_OF_UNIQUE_WORDS" || keyAndValue[0] == "MAXIMUM_NUMBER_OF_UNIQUE_WORDS" ||
                                keyAndValue[0] == "MINIMUM_NUMBER_OF_ROWS" || keyAndValue[0] == "MAXIMUM_NUMBER_OF_ROWS" ||
                                keyAndValue[0] == "MINIMUM_NUMBER_OF_COLUMNS" || keyAndValue[0] == "MAXIMUM_NUMBER_OF_COLUMNS" ||
                                keyAndValue[0] == "MINIMUM_HORIZONTAL_WORDS" || keyAndValue[0] == "MAXIMUM_HORIZONTAL_WORDS" ||
                                keyAndValue[0] == "MINIMUM_VERTICAL_WORDS" || keyAndValue[0] == "MAXIMUM_VERTICAL_WORDS" ||
                                keyAndValue[0] == "MINIMUM_INTERSECTIONS_IN_HORIZONTAL_WORDS" || keyAndValue[0] == "MAXIMUM_INTERSECTIONS_IN_HORIZONTAL_WORDS" ||
                                keyAndValue[0] == "MINIMUM_INTERSECTIONS_IN_VERTICAL_WORDS" || keyAndValue[0] == "MAXIMUM_INTERSECTIONS_IN_VERTICAL_WORDS" ||
                                keyAndValue[0] == "MINIMUM_NUMBER_OF_THE_SAME_WORD" || keyAndValue[0] == "MAXIMUM_NUMBER_OF_THE_SAME_WORD" ||
                                keyAndValue[0] == "MINIMUM_NUMBER_OF_GROUPS" || keyAndValue[0] == "MAXIMUM_NUMBER_OF_GROUPS" ||
                                keyAndValue[0] == "POINTS_PER_WORD"
                                )
                                {
                                    if (Int32.TryParse(keyAndValue[1], out int i))
                                    {
                                        if (i >= 0)
                                        {
                                            IntVariables.Add(keyAndValue[0], Convert.ToInt32(keyAndValue[1]));
                                        }
                                        else
                                        {
                                            log.WriteLine("Invalid Configuration File: Invalid negative value given for {" + keyAndValue[0] + "} => {" + keyAndValue[1] + "}");
                                            Valid = false;
                                        }
                                    }
                                    else
                                    {
                                        log.WriteLine("Invalid Configuration File: Value given is not an integer {" + keyAndValue[0] + "} => {" + keyAndValue[1] + "}");
                                        Valid = false;
                                    }
                                }
                                else
                                {
                                    Variables.Add(keyAndValue[0].Trim(), keyAndValue[1].Trim());
                                }
                            }
                            catch (ArgumentException)
                            {
                                log.WriteLine("Invalid Configuration File: Configuration item repeated {" + keyAndValue[0] + "} => {" + keyAndValue[1] + "}");
                                Valid = false;
                            }
                            catch (FormatException)
                            {
                                log.WriteLine("Invalid Configuration File: Value given is not an integer {" + keyAndValue[0] + "} => {" + keyAndValue[1] + "}"); Valid = false;
                            }

                            if (keyAndValue[0] == "BGCOLOUR_EMPTY_TD" || keyAndValue[0] == "BGCOLOUR_NON_EMPTY_TD")
                            {
                                Regex color = new Regex(onlyColours);
                                Match match = color.Match(keyAndValue[1]);
                                if (match.Success)
                                {
                                    if (keyAndValue[0] == "BGCOLOUR_EMPTY_TD")
                                    {
                                        BGCOLOUR_EMPTY_TD = keyAndValue[1];
                                    }
                                    else
                                    {
                                        BGCOLOUR_NON_EMPTY_TD = keyAndValue[1];
                                    }
                                }
                                else
                                {
                                    log.WriteLine("Invalid Configuration File: Invalid Colour given { " + keyAndValue[0] + "} => { " + keyAndValue[1] + "}");
                                    Valid = false;
                                    UseConfigColours = false;
                                }

                            }
                            // check if uppercase is a boolean
                            if (keyAndValue[0] == "UPPERCASE")
                            {
                                if (Boolean.TryParse(keyAndValue[1], out bool uppercase)) { UpperCase = uppercase; }
                                else
                                {
                                    log.WriteLine("Invalid Configuration File: Invalid value, BOOLEAN expected { " + keyAndValue[0] + "} => { " + keyAndValue[1] + "}");
                                    Valid = false;
                                }
                            }
                        }
                    }
                }
            }
            #region Check if all Properties Exists
            foreach (string str in IntVariablesList)
            {
                if (IntVariables.ContainsKey(str)) { }
                else
                {
                    log.WriteLine("Invalid Configuration File: Missing Property { " + str + " }");
                    Valid = false;
                }

            }
            foreach (string str in VariablesList)
            {
                if (Variables.ContainsKey(str)) { }
                else
                {
                    log.WriteLine("Invalid Configuration File: Missing Property { " + str + " }");
                    Valid = false;
                }

            }
            #endregion

            #region Check Score for Each Letter Exists
            foreach (string str in AtoZ)
            {
                try
                {
                    if (INTERSECTING_POINTS_PER_LETTER.ContainsKey(str)) { }
                    else
                    {
                        log.WriteLine("Invalid Configuration File: Integer score for intersecting letter {" + str + "} is missing ");
                        Valid = false;
                    }
                    if (NON_INTERSECTING_POINTS_PER_LETTER.ContainsKey(str)) { }
                    else
                    {
                        log.WriteLine("Invalid Configuration File: Integer score for non intersecting letter {" + str + "} is missing ");
                        Valid = false;
                    }
                }
                catch (NullReferenceException)
                {
                    log.WriteLine("Invalid Configuration File: Cannot detect score values for Intersecting or Non-Intersecting letters");
                    Valid = false;
                    break;
                }
            }
            #endregion

        }

        /// <summary>
        /// This method computes the score of the crozzle
        /// It gets all the intersecting and non intersecting characters and calculates the final score using metrics from Configuration File.
        /// </summary>
        /// <param name="crozzle"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public int CalculateScore(Crozzle crozzle, Table table)
        {
            int score = 0;
            int number_of_words = crozzle.CrozzleWords.Count;

            if (number_of_words > 0)
            {
                score = number_of_words * IntVariables["POINTS_PER_WORD"];
            }

            char[,] array = table.arrayOfWords;

            // this array will be used to form another array, with bigger dimension.
            // Because we need to search each array[x,y] if it is an intersection, 
            //so we need to add some sort of padding to prevent index out of range exception.
            char[,] paddedArray = GetPaddedArray(array);

            String intersections = "";
            String non_intersections = "";

            for (int r = 1; r <= paddedArray.GetLength(0) - 2; r++)
            {
                for (int c = 1; c <= paddedArray.GetLength(1) - 2; c++)
                {
                    if (paddedArray[r, c] != '\0')
                    {
                        if ((paddedArray[r, c - 1] != '\0' && paddedArray[r - 1, c] != '\0') ||
                          (paddedArray[r, c - 1] != '\0' && paddedArray[r + 1, c] != '\0') ||
                          (paddedArray[r, c + 1] != '\0' && paddedArray[r - 1, c] != '\0') ||
                          (paddedArray[r, c + 1] != '\0' && paddedArray[r + 1, c] != '\0'))
                        {
                            intersections += paddedArray[r, c];
                        }
                        else
                        {
                            non_intersections += paddedArray[r, c];
                        }
                    }
                }
            }

            foreach (char ch in intersections)
            {
                score += INTERSECTING_POINTS_PER_LETTER[ch.ToString()];
            }

            foreach (char ch in non_intersections)
            {
                score += NON_INTERSECTING_POINTS_PER_LETTER[ch.ToString()];
            }
            return score;
        }

        #region Calculate Number of Groups
        /// <summary>
        /// This method accepts a two dimensional Char array and adds padding around it for other methods
        /// aim is to prevent exceptions.
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private char[,] GetPaddedArray(char[,] array)
        {
            char[,] paddedArray = new char[array.GetLength(0) + 2, array.GetLength(1) + 2];
            for (int x = 1; x <= paddedArray.GetLength(0) - 2; x++)
            {
                for (int y = 1; y <= paddedArray.GetLength(1) - 2; y++)
                {
                    paddedArray[x, y] = array[x - 1, y - 1];
                }
            }
            return paddedArray;
        }

        /// <summary>
        /// This method gets the number of Groups using the FloodFill recursive algorithm.
        /// </summary>
        /// <param name="crozzle"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        private int GetNumGroups(Crozzle crozzle, Table table)
        {
            char[,] array = table.arrayOfWords;
            paddedArray = GetPaddedArray(array);
            for (int r = 1; r <= paddedArray.GetLength(0) - 2; r++)
            {
                for (int c = 1; c <= paddedArray.GetLength(1) - 2; c++)
                {
                    if (Char.IsLetter(paddedArray[r, c]))
                    {
                        FloodFill(r, c);
                        NumGroups++;
                    }
                }
            }
            NumGroups = NumGroups - 1; // this is because we started from 1
            return NumGroups;
        }
        #region Flood Fill Algorithm 
        /// <summary>
        /// This is the Flood Fill algorthim which takes x and y coordinates and compute the array such that it can be
        /// used to get the desired result
        /// This is an recursive algorithm
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void FloodFill(int x, int y)
        {
            paddedArray[x, y] = Convert.ToChar(NumGroups);
            if (Char.IsLetter(paddedArray[x - 1, y])) { FloodFill(x - 1, y); } // check north
            if (Char.IsLetter(paddedArray[x + 1, y])) { FloodFill(x + 1, y); } // check south
            if (Char.IsLetter(paddedArray[x, y - 1])) { FloodFill(x, y - 1); } // check west
            if (Char.IsLetter(paddedArray[x, y + 1])) { FloodFill(x, y + 1); } // check east

        }
        #endregion
        #endregion
    }
}
