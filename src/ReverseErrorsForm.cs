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
    public partial class ReverseErrorsForm : Form
    {
        DataTable WrongWords;
        private Dictionary<string, List<string>> Misspellings;
        public ReverseErrorsForm(Dictionary<string, List<string>> misspelled_words)
        {
            InitializeComponent();
            Misspellings = misspelled_words;
        }

        private void ReverseErrorsForm_Load(object sender, EventArgs e)
        {          
            WrongWords = new DataTable();
            WrongWords.Columns.AddRange(new DataColumn[3] { new DataColumn("Word"), new DataColumn("Occurances"), new DataColumn("Files") });

            foreach (KeyValuePair<string, List<string>> element in Misspellings)
            {
                string files = "";
                foreach (string b in element.Value)
                {
                    files = files + b + ", ";
                }
                files = files.Remove(files.Length - 2);

                WrongWords.Rows.Add(element.Key, element.Value.Count, files);
            }
            dataGridView1.DataSource = WrongWords;
            dataGridView1.AutoResizeColumns();
            dataGridView1.Sort(this.dataGridView1.Columns["Occurances"], ListSortDirection.Descending);
            //dataGridView1.Columns[2].Width = 200;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ExportPivotResults();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ExportPivotResults()
        {
            saveFileDialog1.FileName = "Mispellings";
            saveFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.ShowDialog();
            Util.SaveDataTableToPipeDelimitedTextFile(saveFileDialog1.FileName, WrongWords);
        }
    }
}
