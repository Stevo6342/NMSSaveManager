namespace NMSSaveManager.Framework
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.createNewGameButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.availableGamesList = new System.Windows.Forms.DataGridView();
            this.loadSelectedButton = new System.Windows.Forms.Button();
            this.importSaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LastSaveTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gameSavesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.deleteButton = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.availableGamesList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gameSavesBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // createNewGameButton
            // 
            this.createNewGameButton.Location = new System.Drawing.Point(12, 45);
            this.createNewGameButton.Name = "createNewGameButton";
            this.createNewGameButton.Size = new System.Drawing.Size(112, 23);
            this.createNewGameButton.TabIndex = 0;
            this.createNewGameButton.Text = "Create New Game";
            this.createNewGameButton.UseVisualStyleBackColor = true;
            this.createNewGameButton.Click += new System.EventHandler(this.createNewGameButton_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(730, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeToolStripMenuItem,
            this.importSaveToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.closeToolStripMenuItem.Text = "&Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.settingsToolStripMenuItem.Text = "&Settings";
            // 
            // availableGamesList
            // 
            this.availableGamesList.AllowUserToAddRows = false;
            this.availableGamesList.AllowUserToDeleteRows = false;
            this.availableGamesList.AllowUserToOrderColumns = true;
            this.availableGamesList.AllowUserToResizeRows = false;
            this.availableGamesList.AutoGenerateColumns = false;
            this.availableGamesList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.availableGamesList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.availableGamesList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.LastSaveTime});
            this.availableGamesList.DataSource = this.gameSavesBindingSource;
            this.availableGamesList.Location = new System.Drawing.Point(12, 74);
            this.availableGamesList.MultiSelect = false;
            this.availableGamesList.Name = "availableGamesList";
            this.availableGamesList.RowHeadersVisible = false;
            this.availableGamesList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.availableGamesList.Size = new System.Drawing.Size(706, 166);
            this.availableGamesList.TabIndex = 2;
            this.availableGamesList.SelectionChanged += new System.EventHandler(this.availableGamesList_SelectionChanged);
            // 
            // loadSelectedButton
            // 
            this.loadSelectedButton.Enabled = false;
            this.loadSelectedButton.Location = new System.Drawing.Point(585, 45);
            this.loadSelectedButton.Name = "loadSelectedButton";
            this.loadSelectedButton.Size = new System.Drawing.Size(133, 23);
            this.loadSelectedButton.TabIndex = 1;
            this.loadSelectedButton.Text = "Load Selected Game";
            this.loadSelectedButton.UseVisualStyleBackColor = true;
            this.loadSelectedButton.Click += new System.EventHandler(this.loadSelectedButton_Click);
            // 
            // importSaveToolStripMenuItem
            // 
            this.importSaveToolStripMenuItem.Name = "importSaveToolStripMenuItem";
            this.importSaveToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.importSaveToolStripMenuItem.Text = "&Import Save";
            this.importSaveToolStripMenuItem.Click += new System.EventHandler(this.importSaveToolStripMenuItem_Click);
            // 
            // LastSaveTime
            // 
            this.LastSaveTime.DataPropertyName = "LastSaveTime";
            dataGridViewCellStyle1.Format = "G";
            dataGridViewCellStyle1.NullValue = null;
            this.LastSaveTime.DefaultCellStyle = dataGridViewCellStyle1;
            this.LastSaveTime.HeaderText = "LastSaveTime";
            this.LastSaveTime.Name = "LastSaveTime";
            this.LastSaveTime.ReadOnly = true;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // gameSavesBindingSource
            // 
            this.gameSavesBindingSource.DataSource = typeof(NMSSaveManager.ObjectBase.Collections.GameDataInfoCollection);
            // 
            // deleteButton
            // 
            this.deleteButton.Enabled = false;
            this.deleteButton.Location = new System.Drawing.Point(130, 45);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(138, 23);
            this.deleteButton.TabIndex = 4;
            this.deleteButton.Text = "Remove Selected Game";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 253);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.loadSelectedButton);
            this.Controls.Add(this.availableGamesList);
            this.Controls.Add(this.createNewGameButton);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NMS Save Manager v1.0";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.availableGamesList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gameSavesBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button createNewGameButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.DataGridView availableGamesList;
        private System.Windows.Forms.BindingSource gameSavesBindingSource;
        private System.Windows.Forms.Button loadSelectedButton;
        private System.Windows.Forms.ToolStripMenuItem importSaveToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastSaveTime;
        private System.Windows.Forms.Button deleteButton;
    }
}

