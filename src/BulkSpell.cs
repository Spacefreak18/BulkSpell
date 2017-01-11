using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using NetSpell.SpellChecker;

namespace BulkSpell
{
    public class BulkSpell
    {
        public Dictionary<string, List<string>> Misspellings;
        public DataTable WrongWords;

        private NetSpell.SpellChecker.Dictionary.WordDictionary WordDictionary;
        private Spelling spellchecker;  

        // needed when the mispelled word event fires
        private int MisspellingsCount;
        private string currentfilename;

        public BulkSpell(string DictionaryFile)
        {
            WordDictionary = new NetSpell.SpellChecker.Dictionary.WordDictionary();
            WordDictionary.DictionaryFile = DictionaryFile;
            WordDictionary.Initialize();
        }

        public void SpellCheck(string FolderWithFilesToBeChecked)
        {
            spellchecker = new NetSpell.SpellChecker.Spelling();
            spellchecker.Dictionary = WordDictionary;
            spellchecker.MisspelledWord += new NetSpell.SpellChecker.Spelling.MisspelledWordEventHandler(this.WordMispelled);

            Misspellings = new Dictionary<string, List<string>>();
            WrongWords = new DataTable();
            WrongWords.Columns.AddRange(new DataColumn[3] { new DataColumn("Id"), new DataColumn("Name"), new DataColumn("Errors") });

            // check if files exist or are in use
            System.IO.FileInfo[] filesList = null;
            System.IO.DirectoryInfo directory;
            directory = new System.IO.DirectoryInfo(FolderWithFilesToBeChecked);
            filesList = directory.GetFiles();

            string text;
            int i = 1;
            foreach (System.IO.FileInfo file in filesList)
            {
                currentfilename = file.Name;

                string mime;
                byte[] buffer = new byte[256];
                using (System.IO.FileStream fs = new System.IO.FileStream(file.FullName, System.IO.FileMode.Open))
                {
                    if (fs.Length >= 256)
                        fs.Read(buffer, 0, 256);
                    else
                        fs.Read(buffer, 0, (int)fs.Length);
                }

                mime = FileTypeCheck.GetMimeType(buffer, file.FullName);
                if (mime == "application/pdf")
                {
                    text = Util.ConvertPdftoText(file.FullName);
                }
                else
                {
                    if (mime == "application/octet-stream")
                    {
                        bool isbinary = FileTypeCheck.IsBinary(buffer);

                        if (isbinary == false)
                        {
                            using (var streamReader = new System.IO.StreamReader(file.FullName, Encoding.UTF8))
                            {
                                text = streamReader.ReadToEnd();
                            }
                        }
                        else
                            text = string.Empty;
                    }
                    else
                        text = string.Empty;
                }


                spellchecker.Text = text;
                spellchecker.ShowDialog = false;

                MisspellingsCount = 0;
                spellchecker.WordIndex = -1;
                while (spellchecker.WordIndex < spellchecker.WordCount - 1)
                {
                    spellchecker.SpellCheck(spellchecker.WordIndex + 1);
                    spellchecker.WordIndex = spellchecker.WordIndex + 1;
                }


                WrongWords.Rows.Add(i, file.Name, MisspellingsCount.ToString());
                i++;
            }
        }

        private void WordMispelled(object sender, NetSpell.SpellChecker.SpellingEventArgs e)
        {
            List<string> tempFilesList = new List<string>();

            if (Misspellings.ContainsKey(spellchecker.CurrentWord))
            {
                tempFilesList = Misspellings[spellchecker.CurrentWord];
                Misspellings.Remove(spellchecker.CurrentWord);
            }

            if (!tempFilesList.Contains(currentfilename))
            {
                tempFilesList.Add(currentfilename);
            }

            Misspellings.Add(spellchecker.CurrentWord, tempFilesList);
            MisspellingsCount += 1;
        }
    }
}
