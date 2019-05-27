using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Bacchus.Controller;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bacchus.Model;

namespace Bacchus
{
    public partial class FormExportCSV : Form
    {
        public FormExportCSV()
        {
            InitializeComponent();
        }

        private void buttonOpenFileForm_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog1 = new SaveFileDialog())
            {



                saveFileDialog1.Filter = "fichier csv |*.csv";
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    textBoxFileName.Text = saveFileDialog1.FileName;
                }
            }
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            var fileName = textBoxFileName.Text;
            List<Article> articles = MeinController.GetAllArticles();
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.WriteLine("Description;Ref;Marque;Famille;Sous-Famille;Prix H.T.");
                foreach (Article article in articles)
                {
                    sw.WriteLine(article.toCSV());

                }
            }
        }
    }
}
