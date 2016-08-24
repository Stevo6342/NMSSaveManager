using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace NMSSaveManager.Framework
{
    public partial class CreateNewGameDialog : Form
    {
        public String NewGameName
        {
            get
            {
                return name.Text;
            }
        }
        public String ExistingDataDirectory { get; set; }
        public String SavePath { get { return name.Text.Replace(" ", String.Empty) + "_saves"; } }
        public bool OverwriteExisting { get; set; }

        public CreateNewGameDialog()
        {
            InitializeComponent();
        }

        public CreateNewGameDialog(String existingPath)
        {
            ExistingDataDirectory = existingPath;
            InitializeComponent();
            createFromExistingPathCheckbox.Enabled = true;
            createFromExistingPathCheckbox.Checked = true;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(Program.GameDataPath + "\\" + SavePath))
            {
                String[] subDirs = Directory.GetDirectories(Program.GameDataPath + "\\" + SavePath, "st_*");
                if(subDirs.Length > 0)
                {
                    switch(MessageBox.Show("Save data already detected under this name, keep it for this game?\n\nWarning: Saying no will overwrite this data permanently!!", "Save Data Detected", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation))
                    {
                        case DialogResult.Yes:
                            OverwriteExisting = false;
                            break;
                        case DialogResult.No:
                            OverwriteExisting = true;
                            break;
                        case DialogResult.Cancel:
                            return;
                    }
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
