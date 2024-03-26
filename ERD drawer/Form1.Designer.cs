namespace ERD_drawer
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            pictureBox1 = new PictureBox();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            createToolStripMenuItem = new ToolStripMenuItem();
            createToolStripMenuItem1 = new ToolStripMenuItem();
            entityToolStripMenuItem = new ToolStripMenuItem();
            relationshipToolStripMenuItem = new ToolStripMenuItem();
            attributeToolStripMenuItem = new ToolStripMenuItem();
            deleteToolStripMenuItem = new ToolStripMenuItem();
            renameToolStripMenuItem = new ToolStripMenuItem();
            attributeToolStripMenuItem1 = new ToolStripMenuItem();
            setAsKeyToolStripMenuItem = new ToolStripMenuItem();
            setAsNotKeyToolStripMenuItem = new ToolStripMenuItem();
            specialToolStripMenuItem = new ToolStripMenuItem();
            changeQuantityLeftToolStripMenuItem = new ToolStripMenuItem();
            selectToolStripMenuItem = new ToolStripMenuItem();
            allToolStripMenuItem = new ToolStripMenuItem();
            noneToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            saveImageToolStripMenuItem = new ToolStripMenuItem();
            saveAsjpgToolStripMenuItem = new ToolStripMenuItem();
            timer1 = new System.Windows.Forms.Timer(components);
            openFileDialog1 = new OpenFileDialog();
            saveFileDialog1 = new SaveFileDialog();
            saveImageFileDialog = new SaveFileDialog();
            saveAspngToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.BackColor = Color.LightGray;
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox1.Location = new Point(12, 36);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(2255, 799);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            pictureBox1.Paint += pictureBox1_Paint;
            pictureBox1.MouseDown += pictureBox1_MouseDown;
            pictureBox1.MouseMove += pictureBox1_MouseMove;
            pictureBox1.MouseUp += pictureBox1_MouseUp;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(24, 24);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, createToolStripMenuItem, selectToolStripMenuItem, helpToolStripMenuItem, saveImageToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(2279, 33);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, saveToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(54, 29);
            fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            openToolStripMenuItem.Size = new Size(235, 34);
            openToolStripMenuItem.Text = "Open...";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            saveToolStripMenuItem.Size = new Size(235, 34);
            saveToolStripMenuItem.Text = "Save...";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // createToolStripMenuItem
            // 
            createToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { createToolStripMenuItem1, deleteToolStripMenuItem, renameToolStripMenuItem, attributeToolStripMenuItem1, specialToolStripMenuItem });
            createToolStripMenuItem.Name = "createToolStripMenuItem";
            createToolStripMenuItem.Size = new Size(87, 29);
            createToolStripMenuItem.Text = "Actions";
            // 
            // createToolStripMenuItem1
            // 
            createToolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { entityToolStripMenuItem, relationshipToolStripMenuItem, attributeToolStripMenuItem });
            createToolStripMenuItem1.Name = "createToolStripMenuItem1";
            createToolStripMenuItem1.Size = new Size(239, 34);
            createToolStripMenuItem1.Text = "Create...";
            // 
            // entityToolStripMenuItem
            // 
            entityToolStripMenuItem.Name = "entityToolStripMenuItem";
            entityToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.E;
            entityToolStripMenuItem.Size = new Size(320, 34);
            entityToolStripMenuItem.Text = "Entity";
            entityToolStripMenuItem.Click += entityToolStripMenuItem_Click;
            // 
            // relationshipToolStripMenuItem
            // 
            relationshipToolStripMenuItem.Name = "relationshipToolStripMenuItem";
            relationshipToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.R;
            relationshipToolStripMenuItem.Size = new Size(320, 34);
            relationshipToolStripMenuItem.Text = "Relationship";
            relationshipToolStripMenuItem.Click += relationshipToolStripMenuItem_Click;
            // 
            // attributeToolStripMenuItem
            // 
            attributeToolStripMenuItem.Name = "attributeToolStripMenuItem";
            attributeToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.A;
            attributeToolStripMenuItem.Size = new Size(320, 34);
            attributeToolStripMenuItem.Text = "Attribute";
            attributeToolStripMenuItem.Click += attributeToolStripMenuItem_Click;
            // 
            // deleteToolStripMenuItem
            // 
            deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            deleteToolStripMenuItem.ShortcutKeys = Keys.Delete;
            deleteToolStripMenuItem.Size = new Size(239, 34);
            deleteToolStripMenuItem.Text = "Delete";
            deleteToolStripMenuItem.Click += deleteToolStripMenuItem_Click;
            // 
            // renameToolStripMenuItem
            // 
            renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            renameToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.R;
            renameToolStripMenuItem.Size = new Size(239, 34);
            renameToolStripMenuItem.Text = "Rename";
            renameToolStripMenuItem.Click += renameToolStripMenuItem_Click;
            // 
            // attributeToolStripMenuItem1
            // 
            attributeToolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { setAsKeyToolStripMenuItem, setAsNotKeyToolStripMenuItem });
            attributeToolStripMenuItem1.Name = "attributeToolStripMenuItem1";
            attributeToolStripMenuItem1.Size = new Size(239, 34);
            attributeToolStripMenuItem1.Text = "Attribute...";
            // 
            // setAsKeyToolStripMenuItem
            // 
            setAsKeyToolStripMenuItem.Name = "setAsKeyToolStripMenuItem";
            setAsKeyToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.K;
            setAsKeyToolStripMenuItem.Size = new Size(337, 34);
            setAsKeyToolStripMenuItem.Text = "Set as key";
            setAsKeyToolStripMenuItem.Click += setAsKeyToolStripMenuItem_Click;
            // 
            // setAsNotKeyToolStripMenuItem
            // 
            setAsNotKeyToolStripMenuItem.Name = "setAsNotKeyToolStripMenuItem";
            setAsNotKeyToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.N;
            setAsNotKeyToolStripMenuItem.Size = new Size(337, 34);
            setAsNotKeyToolStripMenuItem.Text = "Set as not key";
            setAsNotKeyToolStripMenuItem.Click += setAsNotKeyToolStripMenuItem_Click;
            // 
            // specialToolStripMenuItem
            // 
            specialToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { changeQuantityLeftToolStripMenuItem });
            specialToolStripMenuItem.Name = "specialToolStripMenuItem";
            specialToolStripMenuItem.Size = new Size(239, 34);
            specialToolStripMenuItem.Text = "Relationship";
            // 
            // changeQuantityLeftToolStripMenuItem
            // 
            changeQuantityLeftToolStripMenuItem.Name = "changeQuantityLeftToolStripMenuItem";
            changeQuantityLeftToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.Q;
            changeQuantityLeftToolStripMenuItem.Size = new Size(369, 34);
            changeQuantityLeftToolStripMenuItem.Text = "Change quantities";
            changeQuantityLeftToolStripMenuItem.Click += changeQuantityLeftToolStripMenuItem_Click;
            // 
            // selectToolStripMenuItem
            // 
            selectToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { allToolStripMenuItem, noneToolStripMenuItem });
            selectToolStripMenuItem.Name = "selectToolStripMenuItem";
            selectToolStripMenuItem.Size = new Size(74, 29);
            selectToolStripMenuItem.Text = "Select";
            // 
            // allToolStripMenuItem
            // 
            allToolStripMenuItem.Name = "allToolStripMenuItem";
            allToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.A;
            allToolStripMenuItem.Size = new Size(255, 34);
            allToolStripMenuItem.Text = "All";
            allToolStripMenuItem.Click += allToolStripMenuItem_Click_1;
            // 
            // noneToolStripMenuItem
            // 
            noneToolStripMenuItem.Name = "noneToolStripMenuItem";
            noneToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Space;
            noneToolStripMenuItem.Size = new Size(255, 34);
            noneToolStripMenuItem.Text = "None";
            noneToolStripMenuItem.Click += noneToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(65, 29);
            helpToolStripMenuItem.Text = "Help";
            helpToolStripMenuItem.Click += helpToolStripMenuItem_Click;
            // 
            // saveImageToolStripMenuItem
            // 
            saveImageToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { saveAsjpgToolStripMenuItem, saveAspngToolStripMenuItem });
            saveImageToolStripMenuItem.Name = "saveImageToolStripMenuItem";
            saveImageToolStripMenuItem.Size = new Size(78, 29);
            saveImageToolStripMenuItem.Text = "Image";
            // 
            // saveAsjpgToolStripMenuItem
            // 
            saveAsjpgToolStripMenuItem.Name = "saveAsjpgToolStripMenuItem";
            saveAsjpgToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;
            saveAsjpgToolStripMenuItem.Size = new Size(317, 34);
            saveAsjpgToolStripMenuItem.Text = "Save as .jpg";
            saveAsjpgToolStripMenuItem.Click += saveAsjpgToolStripMenuItem_Click;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 1;
            timer1.Tick += timer1_Tick;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "data";
            openFileDialog1.Filter = "JSON|*.json";
            // 
            // saveFileDialog1
            // 
            saveFileDialog1.FileName = "data";
            saveFileDialog1.Filter = "JSON|*.json";
            // 
            // saveImageFileDialog
            // 
            saveImageFileDialog.FileName = "entity_relationship_diagram";
            saveImageFileDialog.Filter = "JPG|*.jpg";
            // 
            // saveAspngToolStripMenuItem
            // 
            saveAspngToolStripMenuItem.Name = "saveAspngToolStripMenuItem";
            saveAspngToolStripMenuItem.Size = new Size(317, 34);
            saveAspngToolStripMenuItem.Text = "Save as .png";
            saveAspngToolStripMenuItem.Click += saveAspngToolStripMenuItem_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2279, 847);
            Controls.Add(pictureBox1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            MinimumSize = new Size(500, 500);
            Name = "Form1";
            Text = "Entity-Relationship Diagram Drawer (ERDD)";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private OpenFileDialog openFileDialog1;
        private SaveFileDialog saveFileDialog1;
        private ToolStripMenuItem createToolStripMenuItem;
        private ToolStripMenuItem createToolStripMenuItem1;
        private ToolStripMenuItem entityToolStripMenuItem;
        private ToolStripMenuItem relationshipToolStripMenuItem;
        private ToolStripMenuItem attributeToolStripMenuItem;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ToolStripMenuItem renameToolStripMenuItem;
        private ToolStripMenuItem attributeToolStripMenuItem1;
        private ToolStripMenuItem setAsKeyToolStripMenuItem;
        private ToolStripMenuItem setAsNotKeyToolStripMenuItem;
        private ToolStripMenuItem specialToolStripMenuItem;
        private ToolStripMenuItem changeQuantityLeftToolStripMenuItem;
        private ToolStripMenuItem selectToolStripMenuItem;
        private ToolStripMenuItem allToolStripMenuItem;
        private ToolStripMenuItem noneToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem saveImageToolStripMenuItem;
        private SaveFileDialog saveImageFileDialog;
        private ToolStripMenuItem saveAsjpgToolStripMenuItem;
        private ToolStripMenuItem saveAspngToolStripMenuItem;
    }
}
