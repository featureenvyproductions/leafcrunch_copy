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
            this.pbGreenLeaf01 = new System.Windows.Forms.PictureBox();
            this.pbPlayer = new System.Windows.Forms.PictureBox();
            this.pbLevel1 = new System.Windows.Forms.PictureBox();
            this.lblRainbowPoints = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbGreenLeaf01)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLevel1)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pbGreenLeaf01
            // 
            this.pbGreenLeaf01.Image = global::LeafCrunch.Properties.Resources.greenleaf;
            this.pbGreenLeaf01.Location = new System.Drawing.Point(397, 245);
            this.pbGreenLeaf01.Name = "pbGreenLeaf01";
            this.pbGreenLeaf01.Size = new System.Drawing.Size(49, 50);
            this.pbGreenLeaf01.TabIndex = 2;
            this.pbGreenLeaf01.TabStop = false;
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
            // lblRainbowPoints
            // 
            this.lblRainbowPoints.AutoSize = true;
            this.lblRainbowPoints.Location = new System.Drawing.Point(92, 42);
            this.lblRainbowPoints.Name = "lblRainbowPoints";
            this.lblRainbowPoints.Size = new System.Drawing.Size(0, 16);
            this.lblRainbowPoints.TabIndex = 3;
            // 
            // CrunchyLeavesMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 540);
            this.Controls.Add(this.lblRainbowPoints);
            this.Controls.Add(this.pbGreenLeaf01);
            this.Controls.Add(this.pbPlayer);
            this.Controls.Add(this.pbLevel1);
            this.Name = "CrunchyLeavesMain";
            this.Text = "Crunchy Leaves";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CrunchyLeavesMain_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CrunchyLeavesMain_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.pbGreenLeaf01)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLevel1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox pbPlayer;
        private System.Windows.Forms.PictureBox pbLevel1;
        private System.Windows.Forms.PictureBox pbGreenLeaf01;
        private System.Windows.Forms.Label lblRainbowPoints;
    }
}

