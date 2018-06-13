namespace XShell.Demo.Winform.Screens.Screen
{
    partial class MyScreenView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.filePathLabel = new System.Windows.Forms.Label();
            this.filePathTextBox = new System.Windows.Forms.TextBox();
            this.browseFilePathButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.filePathLabel, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.filePathTextBox, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.browseFilePathButton, 2, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(490, 291);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // filePathLabel
            // 
            this.filePathLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.filePathLabel.AutoSize = true;
            this.filePathLabel.Location = new System.Drawing.Point(3, 6);
            this.filePathLabel.Name = "filePathLabel";
            this.filePathLabel.Size = new System.Drawing.Size(74, 20);
            this.filePathLabel.TabIndex = 0;
            this.filePathLabel.Text = "File path:";
            // 
            // filePathTextBox
            // 
            this.filePathTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filePathTextBox.Location = new System.Drawing.Point(83, 3);
            this.filePathTextBox.Name = "filePathTextBox";
            this.filePathTextBox.Size = new System.Drawing.Size(368, 26);
            this.filePathTextBox.TabIndex = 1;
            // 
            // browseFilePathButton
            // 
            this.browseFilePathButton.Location = new System.Drawing.Point(457, 3);
            this.browseFilePathButton.Name = "browseFilePathButton";
            this.browseFilePathButton.Size = new System.Drawing.Size(30, 26);
            this.browseFilePathButton.TabIndex = 2;
            this.browseFilePathButton.Text = "...";
            this.browseFilePathButton.UseVisualStyleBackColor = true;
            // 
            // MyScreenView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MyScreenView";
            this.Size = new System.Drawing.Size(490, 291);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label filePathLabel;
        private System.Windows.Forms.TextBox filePathTextBox;
        private System.Windows.Forms.Button browseFilePathButton;
    }
}
