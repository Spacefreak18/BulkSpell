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
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Id"), new DataColumn("Name"), new DataColumn("Errors") });

            System.IO.FileInfo[] filesList = null;

            System.IO.DirectoryInfo directory;
            directory = new System.IO.DirectoryInfo(@"C:\Paul\pdfs\");
            filesList = directory.GetFiles();

            int i = 1;
            foreach (System.IO.FileInfo file in filesList)
            {
                dt.Rows.Add(i, file.Name, "errors detected");
            }
            dataGridView1.DataSource = dt;
        }
    }
}
