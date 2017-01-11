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
            label2.ForeColor = Color.Green;
            label3.ForeColor = Color.Green;

            DictionaryPath = Properties.Settings.Default.SavedWordDictionary;
            textBox2.Text = DictionaryPath;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox1.Text = folderBrowserDialog1.SelectedPath;

            label3.ForeColor = Color.Red;
            label3.Text = "Running...";
            label3.Update();

            try
            {
                runBulkSpellCheck(textBox1.Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            label3.ForeColor = Color.Green;
            label3.Text = "Ready";
            textBox2.Text = DictionaryPath;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                ShowPivotResults();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ShowPivotResults()
        {
            if (BulkSpellChecker == null)
                throw new Exception("You have not run any spell checks yet. There's nothing to analyze.");

            ReverseErrorsForm r = new ReverseErrorsForm(BulkSpellChecker.Misspellings);
            r.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                LoadDictionary();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Properties.Settings.Default.SavedWordDictionary = DictionaryPath;
            Properties.Settings.Default.Save();
        }

        private void LoadDictionary()
        {
            openFileDialog1.FileName = string.Empty;
            openFileDialog1.ShowDialog();
            textBox2.Text = openFileDialog1.FileName;
            DictionaryPath = textBox2.Text;

            if (System.IO.File.Exists(DictionaryPath) == false)
            {
                DictionaryPath = null;
                throw new Exception();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                ExportResultstoFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ExportResultstoFile()
        {
            if (BulkSpellChecker == null)
                throw new Exception("You have not run any spell checks yet. There's nothing to export.");

            saveFileDialog1.ShowDialog();
            Util.SaveDataTableToPipeDelimitedTextFile(saveFileDialog1.FileName, BulkSpellChecker.WrongWords);
        }

        private void runBulkSpellCheck(string FolderwithFilestoCheck)
        {
            if (System.IO.Directory.Exists(FolderwithFilestoCheck) == false)
                throw new Exception("You have not specified a path of files to spell check.");

            // add another check if it actually is a dictionary file?
            if (DictionaryPath == null)
            {
                throw new Exception("You have not loaded a dictionary to perform the spell check with.");
            }

            BulkSpellChecker = new BulkSpell(DictionaryPath);
            BulkSpellChecker.SpellCheck(FolderwithFilestoCheck);

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

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                runBulkSpellCheck(textBox1.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
