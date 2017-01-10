using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetSpell.SpellChecker;

namespace BulkSpell
{
    public partial class MainForm : Form
    {
        private Spelling s;
        private int mispellings;
        private Dictionary<string, List<string>> WrongWords = new Dictionary<string,List<string>>();
        private string currentfilename;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.components = new System.ComponentModel.Container();

            this.s = new NetSpell.SpellChecker.Spelling(this.components);

            this.s.MisspelledWord +=
                new NetSpell.SpellChecker.Spelling.MisspelledWordEventHandler(
                                            this.spelling_Mispelled);

            folderBrowserDialog1.ShowDialog();

            textBox1.Text = folderBrowserDialog1.SelectedPath;

            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Id"), new DataColumn("Name"), new DataColumn("Errors") });

            System.IO.FileInfo[] filesList = null;

            System.IO.DirectoryInfo directory;
            directory = new System.IO.DirectoryInfo(textBox1.Text);
            filesList = directory.GetFiles();

            string text;
            int i = 1;
            foreach (System.IO.FileInfo file in filesList)
            {
                currentfilename = file.Name;

                using (var streamReader = new System.IO.StreamReader(file.FullName, Encoding.UTF8))
                {
                    text = streamReader.ReadToEnd();
                }
                s.Text = text;
                s.ShowDialog = false;

                mispellings = 0;
                while (s.WordIndex + 1 < s.WordCount)
                {
                    s.SpellCheck(s.WordIndex + 1);
                }
                

                dt.Rows.Add(i, file.Name, mispellings.ToString());
                i++;
            }
            dataGridView1.DataSource = dt;
            label2.Text = WrongWords.Count.ToString();

            if (WrongWords.Count > 0 )
            {
                label2.ForeColor = Color.Red;
            }
            else
            {
                label2.ForeColor = Color.Green;
            }
        }

        private void spelling_Mispelled(object sender, NetSpell.SpellChecker.SpellingEventArgs e)
        {
            List<string> tempFilesList = new List<string>();

            if (WrongWords.ContainsKey(s.CurrentWord))
            {
                tempFilesList = WrongWords[s.CurrentWord];
                WrongWords.Remove(s.CurrentWord);
            }      

            if (!tempFilesList.Contains(currentfilename))
            {
                tempFilesList.Add(currentfilename);
            }
            
            WrongWords.Add(s.CurrentWord, tempFilesList);
            mispellings += 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ReverseErrorsForm r = new ReverseErrorsForm(WrongWords);
            r.Show();
        }

    }
}
