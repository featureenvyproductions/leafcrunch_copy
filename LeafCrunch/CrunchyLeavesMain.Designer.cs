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
            this.lblRainbowPoints = new System.Windows.Forms.Label();
            this.pbYellowLeaf01 = new System.Windows.Forms.PictureBox();
            this.pbRedLeaf01 = new System.Windows.Forms.PictureBox();
            this.pbOrangeLeaf01 = new System.Windows.Forms.PictureBox();
            this.pbGreenLeaf01 = new System.Windows.Forms.PictureBox();
            this.pbPlayer = new System.Windows.Forms.PictureBox();
            this.pbLevel1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbYellowLeaf01)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRedLeaf01)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbOrangeLeaf01)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGreenLeaf01)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLevel1)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblRainbowPoints
            // 
            this.lblRainbowPoints.AutoSize = true;
            this.lblRainbowPoints.Location = new System.Drawing.Point(92, 42);
            this.lblRainbowPoints.Name = "lblRainbowPoints";
            this.lblRainbowPoints.Size = new System.Drawing.Size(0, 16);
            this.lblRainbowPoints.TabIndex = 3;
            // 
            // pbYellowLeaf01
            // 
            this.pbYellowLeaf01.Image = global::LeafCrunch.Properties.Resources.yellowleaf;
            this.pbYellowLeaf01.Location = new System.Drawing.Point(385, 403);
            this.pbYellowLeaf01.Name = "pbYellowLeaf01";
            this.pbYellowLeaf01.Size = new System.Drawing.Size(49, 50);
            this.pbYellowLeaf01.TabIndex = 6;
            this.pbYellowLeaf01.TabStop = false;
            // 
            // pbRedLeaf01
            // 
            this.pbRedLeaf01.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.pbRedLeaf01.Image = global::LeafCrunch.Properties.Resources.redleaf;
            this.pbRedLeaf01.Location = new System.Drawing.Point(95, 426);
            this.pbRedLeaf01.Name = "pbRedLeaf01";
            this.pbRedLeaf01.Size = new System.Drawing.Size(49, 50);
            this.pbRedLeaf01.TabIndex = 5;
            this.pbRedLeaf01.TabStop = false;
            // 
            // pbOrangeLeaf01
            // 
            this.pbOrangeLeaf01.Image = global::LeafCrunch.Properties.Resources.orangeleaf;
            this.pbOrangeLeaf01.Location = new System.Drawing.Point(697, 386);
            this.pbOrangeLeaf01.Name = "pbOrangeLeaf01";
            this.pbOrangeLeaf01.Size = new System.Drawing.Size(49, 50);
            this.pbOrangeLeaf01.TabIndex = 4;
            this.pbOrangeLeaf01.TabStop = false;
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
            // CrunchyLeavesMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 540);
            this.Controls.Add(this.pbYellowLeaf01);
            this.Controls.Add(this.pbRedLeaf01);
            this.Controls.Add(this.pbOrangeLeaf01);
            this.Controls.Add(this.lblRainbowPoints);
            this.Controls.Add(this.pbGreenLeaf01);
            this.Controls.Add(this.pbPlayer);
            this.Controls.Add(this.pbLevel1);
            this.Name = "CrunchyLeavesMain";
            this.Text = "Crunchy Leaves";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CrunchyLeavesMain_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CrunchyLeavesMain_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.pbYellowLeaf01)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRedLeaf01)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbOrangeLeaf01)).EndInit();
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
        private System.Windows.Forms.PictureBox pbOrangeLeaf01;
        private System.Windows.Forms.PictureBox pbRedLeaf01;
        private System.Windows.Forms.PictureBox pbYellowLeaf01;
    }
}

