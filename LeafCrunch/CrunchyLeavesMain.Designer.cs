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
            this.lblCountDown = new System.Windows.Forms.Label();
            this.pnHelpMenu = new System.Windows.Forms.Panel();
            this.lblHelpText = new System.Windows.Forms.Label();
            this.pbStationaryHazard = new System.Windows.Forms.PictureBox();
            this.pbHazard = new System.Windows.Forms.PictureBox();
            this.pbMovingObstacle = new System.Windows.Forms.PictureBox();
            this.pbGenericObstacle = new System.Windows.Forms.PictureBox();
            this.pbPineCone01 = new System.Windows.Forms.PictureBox();
            this.pbLevel1 = new System.Windows.Forms.PictureBox();
            this.pnHelpMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbStationaryHazard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbHazard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMovingObstacle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGenericObstacle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPineCone01)).BeginInit();
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
            // lblCountDown
            // 
            this.lblCountDown.AutoSize = true;
            this.lblCountDown.Location = new System.Drawing.Point(742, 31);
            this.lblCountDown.Name = "lblCountDown";
            this.lblCountDown.Size = new System.Drawing.Size(0, 16);
            this.lblCountDown.TabIndex = 7;
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
            // pbStationaryHazard
            // 
            this.pbStationaryHazard.Image = global::LeafCrunch.Properties.Resources.stationaryHazard;
            this.pbStationaryHazard.Location = new System.Drawing.Point(405, 328);
            this.pbStationaryHazard.Name = "pbStationaryHazard";
            this.pbStationaryHazard.Size = new System.Drawing.Size(49, 50);
            this.pbStationaryHazard.TabIndex = 13;
            this.pbStationaryHazard.TabStop = false;
            // 
            // pbHazard
            // 
            this.pbHazard.Image = global::LeafCrunch.Properties.Resources.hazard;
            this.pbHazard.Location = new System.Drawing.Point(258, 466);
            this.pbHazard.Name = "pbHazard";
            this.pbHazard.Size = new System.Drawing.Size(49, 50);
            this.pbHazard.TabIndex = 12;
            this.pbHazard.TabStop = false;
            // 
            // pbMovingObstacle
            // 
            this.pbMovingObstacle.Image = global::LeafCrunch.Properties.Resources.movingObstacle;
            this.pbMovingObstacle.Location = new System.Drawing.Point(496, 424);
            this.pbMovingObstacle.Name = "pbMovingObstacle";
            this.pbMovingObstacle.Size = new System.Drawing.Size(49, 50);
            this.pbMovingObstacle.TabIndex = 11;
            this.pbMovingObstacle.TabStop = false;
            // 
            // pbGenericObstacle
            // 
            this.pbGenericObstacle.Image = global::LeafCrunch.Properties.Resources.genericObstacle;
            this.pbGenericObstacle.Location = new System.Drawing.Point(556, 147);
            this.pbGenericObstacle.Name = "pbGenericObstacle";
            this.pbGenericObstacle.Size = new System.Drawing.Size(49, 50);
            this.pbGenericObstacle.TabIndex = 10;
            this.pbGenericObstacle.TabStop = false;
            // 
            // pbPineCone01
            // 
            this.pbPineCone01.Image = global::LeafCrunch.Properties.Resources.pinecone;
            this.pbPineCone01.Location = new System.Drawing.Point(195, 253);
            this.pbPineCone01.Name = "pbPineCone01";
            this.pbPineCone01.Size = new System.Drawing.Size(49, 50);
            this.pbPineCone01.TabIndex = 8;
            this.pbPineCone01.TabStop = false;
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
            this.Controls.Add(this.pbStationaryHazard);
            this.Controls.Add(this.pbHazard);
            this.Controls.Add(this.pbMovingObstacle);
            this.Controls.Add(this.pbGenericObstacle);
            this.Controls.Add(this.pbPineCone01);
            this.Controls.Add(this.lblCountDown);
            this.Controls.Add(this.lblRainbowPoints);
            this.Controls.Add(this.pbLevel1);
            this.Controls.Add(this.pnHelpMenu);
            this.Name = "CrunchyLeavesMain";
            this.Text = "Crunchy Leaves";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CrunchyLeavesMain_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CrunchyLeavesMain_KeyUp);
            this.pnHelpMenu.ResumeLayout(false);
            this.pnHelpMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbStationaryHazard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbHazard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMovingObstacle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGenericObstacle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPineCone01)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLevel1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox pbLevel1;
        private System.Windows.Forms.Label lblRainbowPoints;
        private System.Windows.Forms.Label lblCountDown;
        private System.Windows.Forms.PictureBox pbPineCone01;
        private System.Windows.Forms.Panel pnHelpMenu;
        private System.Windows.Forms.Label lblHelpText;
        private System.Windows.Forms.PictureBox pbGenericObstacle;
        private System.Windows.Forms.PictureBox pbMovingObstacle;
        private System.Windows.Forms.PictureBox pbHazard;
        private System.Windows.Forms.PictureBox pbStationaryHazard;
    }
}

