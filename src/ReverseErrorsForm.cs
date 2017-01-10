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
        DataTable dt;
        private Dictionary<string, List<string>> WrongWords;// = new Dictionary<string, List<string>>();
        public ReverseErrorsForm(Dictionary<string, List<string>> t)
        {
            InitializeComponent();
            WrongWords = t;
        }

        private void ReverseErrorsForm_Load(object sender, EventArgs e)
        {
            
            dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Word"), new DataColumn("Occurances"), new DataColumn("Files") });

            foreach (KeyValuePair<string, List<string>> element in WrongWords)
            {

                string files = "";
                foreach (string b in element.Value)
                {
                    files = files + b + ", ";
                }
                files = files.Remove(files.Length - 2);

                dt.Rows.Add(element.Key, element.Value.Count, files);
            }
            dataGridView1.DataSource = dt;
            dataGridView1.Columns[2].Width = 200;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();

            Util.SaveDataTableToPipeDelimitedTextFile(saveFileDialog1.FileName, dt);
        }
    }
}
