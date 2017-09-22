using System.Linq;
using System;
using System.Collections.Generic;

namespace Crozzle_App
{
    public class Table
    {
        private int Rows = 0;
        private int Columns = 0;
        public char[,] arrayOfWords;// this array is used throughout the code for validation
        public string Style;

        private List<Word> CrozzleWords;
        Log log;

        public Table()
        {
        }
        /// <summary>
        /// This method accepts a crozzle object and gather necessary information for computing html
        /// </summary>
        /// <param name="crozzle"></param>
        public Table(Crozzle crozzle)
        {
            Rows = crozzle.IntVariables["ROWS"];
            Columns = crozzle.IntVariables["COLUMNS"];
            CrozzleWords = crozzle.CrozzleWords.Keys.ToList();
            log = Log.GetInstance();
            Style = crozzle.Style;

        }

        /// <summary>
        /// This method forms the Html Table
        /// </summary>
        /// <param name="emptyColour"></param>
        /// <param name="nonEmptyColour"></param>
        /// <param name="useConfigColours"></param>
        /// <param name="uppercase"></param>
        /// <returns></returns>
        public string GetHtmlTable(string emptyColour, string nonEmptyColour, bool useConfigColours, bool uppercase)
        {
            string html = "<html><head>";
            html += Style;
            html += "</head><body>";
            html += GetTableContents(emptyColour, nonEmptyColour, useConfigColours, uppercase);
            html += "</body></html>";
            return html;
        }
        /// <summary>
        /// This method gets the array elements and compute the html table
        /// </summary>
        /// <param name="emptyColour"></param>
        /// <param name="nonEmptyColour"></param>
        /// <param name="useConfigColours"></param>
        /// <param name="uppercase"></param>
        /// <returns></returns>
        public string GetTableContents(string emptyColour, string nonEmptyColour, bool useConfigColours, bool uppercase)
        {
            char[,] array = new char[Rows, Columns];

            foreach (Word word in CrozzleWords)
            {
                char[] characters = word.Value.ToCharArray();
                Queue<char> wrd = new Queue<char>();
                foreach (char c in characters)
                {
                    char ch = c;
                    if (uppercase == true)
                    {
                        ch = Char.ToUpper(ch);
                    }
                    else
                    {
                        if (uppercase == false)
                        {
                            ch = Char.ToLower(ch);
                        }
                    }
                    wrd.Enqueue(ch);
                }

                int startsAt = word.startsAt - 1;

                if (word.OrientationIdentifier == "ROW")
                {
                    try
                    {
                        while (wrd.Count != 0)
                        {
                            int Row = word.RorColNumber - 1;
                            array[Row, startsAt] = wrd.Dequeue();
                            startsAt++;
                        }
                    }
                    catch (System.IndexOutOfRangeException)
                    {
                        log.WriteLine("Invalid Crozzle File: {" + word.ToString() + "} does not reside in Crozzle dimensions");
                    }
                }
                else
                {
                    try
                    {
                        while (wrd.Count != 0)
                        {
                            int Column = word.RorColNumber - 1;
                            array[startsAt, Column] = wrd.Dequeue();
                            startsAt++;
                        }
                    }
                    catch (System.Exception)
                    {
                        log.WriteLine("Invalid Crozzle File: {" + word.ToString() + "} does not reside in Crozzle dimensions");
                    }
                }
            }
            arrayOfWords = array;
            var contents = "<table>";
            for (int r = 0; r < Rows; r++)
            {
                contents += "<tr>";
                for (int c = 0; c < Columns; c++)
                {
                    if (array[r, c] != '\0')
                    {
                        if (useConfigColours) { contents += "<td bgcolor='" + nonEmptyColour + "'>" + array[r, c] + "</td>"; }
                        else { contents += "<td bgcolor='#aed6f1'>" + array[r, c] + "</td>"; }
                    }
                    else
                    {
                        if (useConfigColours) { contents += "<td bgcolor='" + emptyColour + "'>" + array[r, c] + "</td>"; }
                        else { contents += "<td bgcolor='#D3D3D3'>" + array[r, c] + "</td>"; }
                    }
                }
                contents += "</tr>";
            }
            contents += "</table>";
            return contents;
        }
    }
}
