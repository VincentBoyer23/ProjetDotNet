using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bacchus
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            splitContainer1.Panel1MinSize = 200;
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void importerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormImportCSV FormCSV = new FormImportCSV();
            FormCSV.ShowDialog(); 
        }

        private void exporterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormExportCSV FormCSV = new FormExportCSV();
            FormCSV.ShowDialog();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {

        }
    }
}
