namespace SCS_Logbook.View
{
    partial class LiveView
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
            doExit = true;
            if (!updateThread.Join(1000))
            {
                updateThread.Abort();
            }

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
            this.lbl_speed = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_rpm = new System.Windows.Forms.Label();
            this.pb_rpm = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // lbl_speed
            // 
            this.lbl_speed.AutoSize = true;
            this.lbl_speed.Font = new System.Drawing.Font("LcdD", 50F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)), true);
            this.lbl_speed.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbl_speed.Location = new System.Drawing.Point(317, 83);
            this.lbl_speed.Name = "lbl_speed";
            this.lbl_speed.Size = new System.Drawing.Size(128, 67);
            this.lbl_speed.TabIndex = 0;
            this.lbl_speed.Text = "000";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Castellar", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(322, 150);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 41);
            this.label1.TabIndex = 1;
            this.label1.Text = "kmh";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Castellar", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(177, 150);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 41);
            this.label2.TabIndex = 3;
            this.label2.Text = "rpm";
            // 
            // lbl_rpm
            // 
            this.lbl_rpm.AutoSize = true;
            this.lbl_rpm.Font = new System.Drawing.Font("LcdD", 50F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)), true);
            this.lbl_rpm.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbl_rpm.Location = new System.Drawing.Point(150, 83);
            this.lbl_rpm.Name = "lbl_rpm";
            this.lbl_rpm.Size = new System.Drawing.Size(161, 67);
            this.lbl_rpm.TabIndex = 2;
            this.lbl_rpm.Text = "0000";
            // 
            // pb_rpm
            // 
            this.pb_rpm.Location = new System.Drawing.Point(162, 67);
            this.pb_rpm.Name = "pb_rpm";
            this.pb_rpm.Size = new System.Drawing.Size(273, 13);
            this.pb_rpm.TabIndex = 4;
            // 
            // LiveView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pb_rpm);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbl_rpm);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbl_speed);
            this.Name = "LiveView";
            this.Text = "LiveView";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LiveView_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_speed;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_rpm;
        private System.Windows.Forms.ProgressBar pb_rpm;
    }
}