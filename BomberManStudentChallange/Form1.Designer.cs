namespace BomberManStudentChallange
{
    partial class FormGame
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
            this.timerBomberMovement = new System.Windows.Forms.Timer(this.components);
            this.timerBom = new System.Windows.Forms.Timer(this.components);
            this.timerLedakan = new System.Windows.Forms.Timer(this.components);
            this.timerGenerateMusuh = new System.Windows.Forms.Timer(this.components);
            this.timerMovementMusuh1 = new System.Windows.Forms.Timer(this.components);
            this.timerMovementMusuh2 = new System.Windows.Forms.Timer(this.components);
            this.timerMovementMusuh3 = new System.Windows.Forms.Timer(this.components);
            this.timerGame = new System.Windows.Forms.Timer(this.components);
            this.timerPintu = new System.Windows.Forms.Timer(this.components);
            this.timerCekBonus = new System.Windows.Forms.Timer(this.components);
            this.timerTime = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timerBomberMovement
            // 
            this.timerBomberMovement.Interval = 10;
            this.timerBomberMovement.Tick += new System.EventHandler(this.timerBomberMovement_Tick);
            // 
            // timerBom
            // 
            this.timerBom.Interval = 1000;
            this.timerBom.Tick += new System.EventHandler(this.timerBom_Tick);
            // 
            // timerLedakan
            // 
            this.timerLedakan.Interval = 1000;
            this.timerLedakan.Tick += new System.EventHandler(this.timerLedakan_Tick);
            // 
            // timerGenerateMusuh
            // 
            this.timerGenerateMusuh.Interval = 1;
            this.timerGenerateMusuh.Tick += new System.EventHandler(this.timerGenerateMusuh_Tick);
            // 
            // timerMovementMusuh1
            // 
            this.timerMovementMusuh1.Tick += new System.EventHandler(this.timerMovementMusuh1_Tick);
            // 
            // timerMovementMusuh2
            // 
            this.timerMovementMusuh2.Tick += new System.EventHandler(this.timerMovementMusuh2_Tick);
            // 
            // timerMovementMusuh3
            // 
            this.timerMovementMusuh3.Tick += new System.EventHandler(this.timerMovementMusuh3_Tick);
            // 
            // timerGame
            // 
            this.timerGame.Tick += new System.EventHandler(this.timerGame_Tick);
            // 
            // timerPintu
            // 
            this.timerPintu.Tick += new System.EventHandler(this.timerPintu_Tick);
            // 
            // timerCekBonus
            // 
            this.timerCekBonus.Interval = 1;
            this.timerCekBonus.Tick += new System.EventHandler(this.timerCekBonus_Tick);
            // 
            // timerTime
            // 
            this.timerTime.Interval = 1000;
            this.timerTime.Tick += new System.EventHandler(this.timerTime_Tick);
            // 
            // FormGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(990, 721);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.Name = "FormGame";
            this.Text = "BomberGIRL";
            this.Load += new System.EventHandler(this.FormGame_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FormGame_KeyUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timerBomberMovement;
        private System.Windows.Forms.Timer timerBom;
        private System.Windows.Forms.Timer timerLedakan;
        private System.Windows.Forms.Timer timerGenerateMusuh;
        private System.Windows.Forms.Timer timerMovementMusuh1;
        private System.Windows.Forms.Timer timerMovementMusuh2;
        private System.Windows.Forms.Timer timerMovementMusuh3;
        private System.Windows.Forms.Timer timerGame;
        private System.Windows.Forms.Timer timerPintu;
        private System.Windows.Forms.Timer timerCekBonus;
        private System.Windows.Forms.Timer timerTime;
    }
}

