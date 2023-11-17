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
            this.pbPlayer = new System.Windows.Forms.PictureBox();
            this.pbLevel1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLevel1)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pbPlayer
            // 
            this.pbPlayer.Image = global::LeafCrunch.Properties.Resources.player_static;
            this.pbPlayer.Location = new System.Drawing.Point(473, 126);
            this.pbPlayer.Name = "pbPlayer";
            this.pbPlayer.Size = new System.Drawing.Size(49, 50);
            this.pbPlayer.TabIndex = 0;
            this.pbPlayer.TabStop = false;
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
            this.Controls.Add(this.pbPlayer);
            this.Controls.Add(this.pbLevel1);
            this.Name = "CrunchyLeavesMain";
            this.Text = "Crunchy Leaves";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CrunchyLeavesMain_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CrunchyLeavesMain_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.pbPlayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLevel1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox pbPlayer;
        private System.Windows.Forms.PictureBox pbLevel1;
    }
}

