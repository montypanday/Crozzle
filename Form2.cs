using System;
using System.Windows.Forms;

namespace Crozzle_App
{
    public partial class Form2 : Form
    {
        private static Form2 _instance;
        public static Form2 GetInstance()
        {
            if (_instance == null) _instance = new Form2();
            return _instance;
        }

        public Form2()
        {
            InitializeComponent();
        }

        public string start = "<html><head></head><body>";

        public string end = "</body></html>";

        private void Form2_FormClosing(Object sender, FormClosingEventArgs e)
        {

            this.Hide();
            this.Parent = null;
            e.Cancel = true;
        }
        public void WriteLine(string message)
        {
            webBrowser1.Navigate("about:blank");
            HtmlDocument doc = webBrowser1.Document;
            doc.Write(String.Empty);
            webBrowser1.DocumentText = start + message + end;
            start += message;
        }
    }
}
