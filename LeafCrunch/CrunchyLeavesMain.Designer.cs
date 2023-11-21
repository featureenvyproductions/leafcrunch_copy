namespace LeafCrunch
{
    partial class CrunchyLeavesMain
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pnHelpMenu = new System.Windows.Forms.Panel();
            this.lblHelpText = new System.Windows.Forms.Label();
            this.pbLevel1 = new System.Windows.Forms.PictureBox();
            this.pnHelpMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLevel1)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pnHelpMenu
            // 
            this.pnHelpMenu.Controls.Add(this.lblHelpText);
            this.pnHelpMenu.Location = new System.Drawing.Point(12, 12);
            this.pnHelpMenu.Name = "pnHelpMenu";
            this.pnHelpMenu.Size = new System.Drawing.Size(819, 464);
            this.pnHelpMenu.TabIndex = 9;
            this.pnHelpMenu.Visible = false;
            // 
            // lblHelpText
            // 
            this.lblHelpText.AutoSize = true;
            this.lblHelpText.Location = new System.Drawing.Point(373, 174);
            this.lblHelpText.Name = "lblHelpText";
            this.lblHelpText.Size = new System.Drawing.Size(129, 16);
            this.lblHelpText.TabIndex = 0;
            this.lblHelpText.Text = "this is some help text";
            // 
            // pbLevel1
            // 
            this.pbLevel1.Image = global::LeafCrunch.Properties.Resources.level1;
            this.pbLevel1.Location = new System.Drawing.Point(0, 0);
            this.pbLevel1.Name = "pbLevel1";
            this.pbLevel1.Size = new System.Drawing.Size(845, 543);
            this.pbLevel1.TabIndex = 1;
            this.pbLevel1.TabStop = false;
            // 
            // CrunchyLeavesMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 540);
            this.Controls.Add(this.pbLevel1);
            this.Controls.Add(this.pnHelpMenu);
            this.Name = "CrunchyLeavesMain";
            this.Text = "Crunchy Leaves";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CrunchyLeavesMain_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CrunchyLeavesMain_KeyUp);
            this.pnHelpMenu.ResumeLayout(false);
            this.pnHelpMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLevel1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox pbLevel1;
        private System.Windows.Forms.Panel pnHelpMenu;
        private System.Windows.Forms.Label lblHelpText;
    }
}

