using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace BulkSpell
{
    public class Util
    {
        public static void SaveDataTableToPipeDelimitedTextFile(string filename, DataTable dt)
        {
            using (System.IO.StreamWriter sw = System.IO.File.CreateText(filename))
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
