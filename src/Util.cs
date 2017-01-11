using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

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

        public static string ConvertPdftoText(string PdfFileName)
        {
            StringBuilder pdftext = new StringBuilder();
            PdfReader pdfReader = new PdfReader(PdfFileName);

            for (int page = 1; page <= pdfReader.NumberOfPages; page++)
            {
                ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                string currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

                currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                pdftext.Append(currentText);
            }
            pdfReader.Close();
            return pdftext.ToString();
        }
    }
}
