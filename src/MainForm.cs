using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BulkSpell
{
    public partial class MainForm : Form
    {
        private BulkSpell BulkSpellChecker;
        private string DictionaryPath = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                runBulkSpellCheck();
            }
            catch
            {

            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ReverseErrorsForm r = new ReverseErrorsForm(BulkSpellChecker.Misspellings);
            r.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = string.Empty;
            openFileDialog1.ShowDialog();
            textBox2.Text = openFileDialog1.FileName;
            DictionaryPath = textBox2.Text;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
            Util.SaveDataTableToPipeDelimitedTextFile(saveFileDialog1.FileName, BulkSpellChecker.WrongWords);
        }

        private void runBulkSpellCheck()
        {
            folderBrowserDialog1.ShowDialog();
            textBox1.Text = folderBrowserDialog1.SelectedPath;

            BulkSpellChecker = new BulkSpell(DictionaryPath);
            BulkSpellChecker.SpellCheck(textBox1.Text);

            dataGridView1.DataSource = BulkSpellChecker.WrongWords;
            label2.Text = BulkSpellChecker.Misspellings.Count.ToString();

            if (BulkSpellChecker.Misspellings.Count > 0)
            {
                label2.ForeColor = Color.Red;
            }
            else
            {
                label2.ForeColor = Color.Green;
            }
        }

    }
}
