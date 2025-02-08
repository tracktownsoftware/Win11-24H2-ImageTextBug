namespace RenderIssues
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
            buttonCompareImages = new Button();
            textBoxStatus = new TextBox();
            buttonExit = new Button();
            SuspendLayout();
            // 
            // buttonCompareImages
            // 
            buttonCompareImages.Location = new Point(12, 12);
            buttonCompareImages.Name = "buttonCompareImages";
            buttonCompareImages.Size = new Size(130, 34);
            buttonCompareImages.TabIndex = 0;
            buttonCompareImages.Text = "Compare Images";
            buttonCompareImages.UseVisualStyleBackColor = true;
            buttonCompareImages.Click += buttonCompareImages_Click;
            // 
            // textBoxStatus
            // 
            textBoxStatus.Location = new Point(12, 53);
            textBoxStatus.Multiline = true;
            textBoxStatus.Name = "textBoxStatus";
            textBoxStatus.Size = new Size(468, 299);
            textBoxStatus.TabIndex = 2;
            // 
            // buttonExit
            // 
            buttonExit.Location = new Point(157, 13);
            buttonExit.Name = "buttonExit";
            buttonExit.Size = new Size(90, 34);
            buttonExit.TabIndex = 3;
            buttonExit.Text = "Exit";
            buttonExit.UseVisualStyleBackColor = true;
            buttonExit.Click += buttonExit_Click;
            // 
            // Form1
            // 
            AcceptButton = buttonCompareImages;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = buttonExit;
            ClientSize = new Size(492, 364);
            Controls.Add(buttonExit);
            Controls.Add(textBoxStatus);
            Controls.Add(buttonCompareImages);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(1);
            Name = "Form1";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Image Comparison";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonCompareImages;
        private TextBox textBoxStatus;
        private Button buttonExit;
    }
}
