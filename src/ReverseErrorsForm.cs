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
            dt.Columns.AddRange(new DataColumn[2] { new DataColumn("Word"), new DataColumn("Files") });

            foreach (KeyValuePair<string, List<string>> element in WrongWords)
            {

                string files = "";
                foreach (string b in element.Value)
                {
                    files = files + b + ", ";
                }
                files = files.Remove(files.Length - 2);

                dt.Rows.Add(element.Key, files);
            }
            dataGridView1.DataSource = dt;
            dataGridView1.Columns[1].Width = 200;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();

            using (System.IO.StreamWriter sw = System.IO.File.CreateText(saveFileDialog1.FileName))
            {
                foreach (DataRow row in dt.Rows)
                {
                    bool firstCol = true;
                    foreach (DataColumn col in dt.Columns)
                    {
                        if (!firstCol) sw.Write("|");
                        sw.Write(row[col].ToString());
                        firstCol = false;
                    }
                    sw.WriteLine();
                }
            }
        }
    }
}
