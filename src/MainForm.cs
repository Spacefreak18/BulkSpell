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
        private string SpellCheckPath = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Green;
            label3.ForeColor = Color.Green;

            SpellCheckPath = Properties.Settings.Default.SavedSpellCheckPath;
            textBox1.Text = SpellCheckPath;
            DictionaryPath = Properties.Settings.Default.SavedWordDictionary;
            textBox2.Text = DictionaryPath;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string tempCheckFolder = string.Empty;

            folderBrowserDialog1.ShowDialog();
            tempCheckFolder = folderBrowserDialog1.SelectedPath;

            PrepareUIForSpellCheckRun();

            try
            {
                await Task.Run(() => runBulkSpellCheck(tempCheckFolder));
            }
            catch(Exception ex)
            {
                textBox1.Text = tempCheckFolder;
                MessageBox.Show(ex.Message);
            }

            UpdateUIWithSpellCheckResults();
            
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
                LoadDictionaryWithDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Properties.Settings.Default.SavedWordDictionary = DictionaryPath;
            Properties.Settings.Default.Save();
        }

        private void LoadDictionaryWithDialog()
        {
            openFileDialog1.FileName = string.Empty;
            openFileDialog1.ShowDialog();
            textBox2.Text = openFileDialog1.FileName;

            LoadDictionary();
        }

        private void LoadDictionary()
        {
            string tempDictionaryPath = string.Empty;

            tempDictionaryPath = textBox2.Text;
            if (System.IO.File.Exists(tempDictionaryPath) == false)
            {
                tempDictionaryPath = null;
                throw new Exception("The dictionary file you are attempting to use does not exist.");
            }
            DictionaryPath = tempDictionaryPath;
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
            {
                textBox1.Text = SpellCheckPath;
                throw new Exception("You have not specified a path of files to spell check which exists.");
            }
            SpellCheckPath = FolderwithFilestoCheck;

            // add another check if it actually is a dictionary file?
            LoadDictionary();
            if (DictionaryPath == null)
                throw new Exception("You have not loaded a dictionary to perform the spell check with.");

            BulkSpellChecker = new BulkSpell(DictionaryPath);
            BulkSpellChecker.SpellCheck(SpellCheckPath);
        } 

        private async void button5_Click(object sender, EventArgs e)
        {
            string tempCheckFolder = textBox1.Text;

            PrepareUIForSpellCheckRun();

            try
            {
                await Task.Run(() => runBulkSpellCheck(tempCheckFolder));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            UpdateUIWithSpellCheckResults();
            SetFields(true);
            label3.ForeColor = Color.Green;
            label3.Text = "Ready";
            textBox1.Text = SpellCheckPath;
            textBox2.Text = DictionaryPath;
            Properties.Settings.Default.SavedSpellCheckPath = SpellCheckPath;
            Properties.Settings.Default.Save();
        }

        private void PrepareUIForSpellCheckRun()
        {
            dataGridView1.DataSource = null;
            label2.ForeColor = Color.Green;
            label2.Text = "0";
            label3.ForeColor = Color.Red;
            label3.Text = "Running...";
            label3.Update();
            SetFields(false);
        }

        private void UpdateUIWithSpellCheckResults()
        {
            dataGridView1.DataSource = BulkSpellChecker.WrongWords;
            dataGridView1.AutoResizeColumns();
            label2.Text = BulkSpellChecker.Misspellings.Count.ToString();

            SetFields(true);
            label3.ForeColor = Color.Green;
            label3.Text = "Ready";
            textBox1.Text = SpellCheckPath;
            Properties.Settings.Default.SavedSpellCheckPath = SpellCheckPath;
            Properties.Settings.Default.Save();
            textBox2.Text = DictionaryPath;

            if (BulkSpellChecker.Misspellings.Count > 0)
                label2.ForeColor = Color.Red;
            else
                label2.ForeColor = Color.Green;
        }

        private void SetFields(bool State)
        {
            dataGridView1.Enabled = State;

            textBox1.Enabled = State;
            textBox2.Enabled = State;

            button1.Enabled = State;
            button2.Enabled = State;
            button3.Enabled = State;
            button4.Enabled = State;
            button5.Enabled = State;
        }

    }
}
