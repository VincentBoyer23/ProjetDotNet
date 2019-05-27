using Bacchus.Controller;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Bacchus
{
    public partial class FormImportCSV : Form
    {
        int countCurrentFile;
        public FormImportCSV()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            addToBDD();
        }

        private void buttonOpenFileForm_Click(object sender, EventArgs e)
        {

            using (OpenFileDialog opf = new OpenFileDialog())
            {
                opf.Filter = "fichier csv |*.csv";
                opf.ShowDialog();
                var filename = opf.FileName;
                textBoxFileName.Text = filename;
                using (StreamReader sr = new StreamReader(filename))
                {
                    int count = 0;
                    while ((sr.ReadLine()) != null)
                    {
                        count++;
                    }
                    countCurrentFile = count;
                    opf.Dispose();
                }

            }

        }

        private void FormImportCSV_Load(object sender, EventArgs e)
        {

        }

        private void buttonWipeAddDB_Click(object sender, EventArgs e)
        {
            textBoxLog.AppendText("c'est la purge, cachez-vous, "+MeinController.DEUSVULT()+ " rows deleted\r\n\r\n");
            MeinController.PourJérusalem();
            addToBDD();
        }

        private void addToBDD()
        {
            var fileName = textBoxFileName.Text;
            using (StreamReader sr = new StreamReader(fileName))
            {
                string line;
                string[] columns = new string[6];
                bool firstLine = true;
                progressBarImport.Maximum = countCurrentFile - 1;
                progressBarImport.Value = 0;
                int success = 0, failure = 0;
                while ((line = sr.ReadLine()) != null)
                {

                    
                    columns = line.Split(';');
                    if (!firstLine)
                    {

                        var inserted = MeinController.insertArticle(columns[1], columns[0], columns[3], columns[4], columns[2], float.Parse(columns[5]), 0);
                        if (inserted) success++;
                        else failure++ ;

                        progressBarImport.Value++;
                        textBoxLog.AppendText("Article ajoutée : " + columns[1] + "  " + columns[0] + "\r\n\r\n");
                    }
                    else
                    {
                        firstLine = false;
                    }




                }

                textBoxLog.AppendText(success + " articles ajoutés avec succès, " + failure + " erreurs");
            }
        }
    }
}
