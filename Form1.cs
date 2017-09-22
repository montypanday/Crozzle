using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crozzle_App
{
    public partial class Form1 : Form
    {
        Crozzle crozzle;
        AboutBox AboutBox;

        WordList Wordlist;
        Configuration Configuration;
        Log log;
        Table table;
        public Form1()
        {
            InitializeComponent();
            crozzle = new Crozzle();
            AboutBox = new AboutBox();
            log = Log.GetInstance();
            Wordlist = new WordList();
            Configuration = new Configuration();
        }

        private void openCrozzleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFD.InitialDirectory = "C:";
            if (openFD.ShowDialog() != DialogResult.Cancel)
            {
                crozzle.SetFileName(openFD.FileName);
                crozzle.ReadCrozzleFile();
                Wordlist = crozzle.GetWordList();
                Wordlist.GetWords();
                Configuration = crozzle.GetConfiguration();
                Configuration.GetConfiguration(crozzle);
                table = new Table(crozzle);
                webBrowser1.Navigate("about:blank");
                HtmlDocument doc = webBrowser1.Document;
                doc.Write(String.Empty);
                webBrowser1.DocumentText = table.GetHtmlTable(Configuration.BGCOLOUR_EMPTY_TD, Configuration.BGCOLOUR_NON_EMPTY_TD, Configuration.UseConfigColours, Configuration.UpperCase);
                var newpath = "";
                var file = Configuration.Variables["LOGFILE_NAME"];
                if (Path.IsPathRooted(file) == false)
                {

                    file = Path.GetDirectoryName(openFD.FileName) + "\\" + file;
                }
                log.FileName = file;
                log.WriteLogToFile();

            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutCrozzleAppToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox.ShowDialog();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }



        private void validateCrozzleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string score;
            if (crozzle.Valid == true && Configuration.Valid == true && Wordlist.valid == true)
            {
                
                if(Configuration.ValidateCrozzle(crozzle, Wordlist, table))
                {
                    score = Configuration.CalculateScore(crozzle, table).ToString();
                    log.WriteLine("Valid Crozzle: You Scored {" + score + "}");
                    
                }
                else
                {
                    score = Configuration.Variables["INVALID_CROZZLE_SCORE"];
                }
            }
            else
            {
                score = Configuration.Variables["INVALID_CROZZLE_SCORE"];
            }
            log.WriteLogToFile();

            string HtmlUntilBodyTag = table.GetHtmlTable(Configuration.BGCOLOUR_EMPTY_TD, Configuration.BGCOLOUR_NON_EMPTY_TD, Configuration.UseConfigColours, Configuration.UpperCase);
            HtmlUntilBodyTag += "<br/><div><p><b> Score = " + score + "</b></p></div>";
           
            webBrowser1.Navigate("about:blank");
            HtmlDocument doc = webBrowser1.Document;
            doc.Write(String.Empty);
            webBrowser1.DocumentText = HtmlUntilBodyTag;
            //log.WriteAllErrorsToFile(Configuration.Variables["LOGFILE_NAME"]);
        }

        private void closeCrozzleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Restarts the application for another Crozzle.
            Application.Restart();
        }
    }
}
