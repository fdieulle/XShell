namespace XShell.Winform.Controls
{
    partial class ListBoxEditorView
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
            this.topTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.topLeftFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.addButton = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            this.cloneButton = new System.Windows.Forms.Button();
            this.moveUpButton = new System.Windows.Forms.Button();
            this.moveDownButton = new System.Windows.Forms.Button();
            this.clearButton = new System.Windows.Forms.Button();
            this.topRightFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.importButton = new System.Windows.Forms.Button();
            this.exportButton = new System.Windows.Forms.Button();
            this.listBox = new System.Windows.Forms.ListBox();
            this.topTableLayoutPanel.SuspendLayout();
            this.topLeftFlowLayoutPanel.SuspendLayout();
            this.topRightFlowLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // topTableLayoutPanel
            // 
            this.topTableLayoutPanel.AutoSize = true;
            this.topTableLayoutPanel.ColumnCount = 3;
            this.topTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.topTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.topTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.topTableLayoutPanel.Controls.Add(this.topLeftFlowLayoutPanel, 0, 0);
            this.topTableLayoutPanel.Controls.Add(this.topRightFlowLayoutPanel, 2, 0);
            this.topTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.topTableLayoutPanel.Name = "topTableLayoutPanel";
            this.topTableLayoutPanel.RowCount = 1;
            this.topTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.topTableLayoutPanel.Size = new System.Drawing.Size(591, 35);
            this.topTableLayoutPanel.TabIndex = 0;
            // 
            // topLeftFlowLayoutPanel
            // 
            this.topLeftFlowLayoutPanel.AutoSize = true;
            this.topLeftFlowLayoutPanel.Controls.Add(this.addButton);
            this.topLeftFlowLayoutPanel.Controls.Add(this.removeButton);
            this.topLeftFlowLayoutPanel.Controls.Add(this.cloneButton);
            this.topLeftFlowLayoutPanel.Controls.Add(this.moveUpButton);
            this.topLeftFlowLayoutPanel.Controls.Add(this.moveDownButton);
            this.topLeftFlowLayoutPanel.Controls.Add(this.clearButton);
            this.topLeftFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topLeftFlowLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.topLeftFlowLayoutPanel.Name = "topLeftFlowLayoutPanel";
            this.topLeftFlowLayoutPanel.Size = new System.Drawing.Size(174, 29);
            this.topLeftFlowLayoutPanel.TabIndex = 0;
            this.topLeftFlowLayoutPanel.WrapContents = false;
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(3, 3);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(23, 23);
            this.addButton.TabIndex = 0;
            this.addButton.UseVisualStyleBackColor = true;
            // 
            // removeButton
            // 
            this.removeButton.Location = new System.Drawing.Point(32, 3);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(23, 23);
            this.removeButton.TabIndex = 1;
            this.removeButton.UseVisualStyleBackColor = true;
            // 
            // cloneButton
            // 
            this.cloneButton.Location = new System.Drawing.Point(61, 3);
            this.cloneButton.Name = "cloneButton";
            this.cloneButton.Size = new System.Drawing.Size(23, 23);
            this.cloneButton.TabIndex = 2;
            this.cloneButton.UseVisualStyleBackColor = true;
            // 
            // moveUpButton
            // 
            this.moveUpButton.Location = new System.Drawing.Point(90, 3);
            this.moveUpButton.Name = "moveUpButton";
            this.moveUpButton.Size = new System.Drawing.Size(23, 23);
            this.moveUpButton.TabIndex = 3;
            this.moveUpButton.UseVisualStyleBackColor = true;
            // 
            // moveDownButton
            // 
            this.moveDownButton.Location = new System.Drawing.Point(119, 3);
            this.moveDownButton.Name = "moveDownButton";
            this.moveDownButton.Size = new System.Drawing.Size(23, 23);
            this.moveDownButton.TabIndex = 4;
            this.moveDownButton.UseVisualStyleBackColor = true;
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(148, 3);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(23, 23);
            this.clearButton.TabIndex = 5;
            this.clearButton.UseVisualStyleBackColor = true;
            // 
            // topRightFlowLayoutPanel
            // 
            this.topRightFlowLayoutPanel.AutoSize = true;
            this.topRightFlowLayoutPanel.Controls.Add(this.importButton);
            this.topRightFlowLayoutPanel.Controls.Add(this.exportButton);
            this.topRightFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topRightFlowLayoutPanel.Location = new System.Drawing.Point(530, 3);
            this.topRightFlowLayoutPanel.Name = "topRightFlowLayoutPanel";
            this.topRightFlowLayoutPanel.Size = new System.Drawing.Size(58, 29);
            this.topRightFlowLayoutPanel.TabIndex = 1;
            this.topRightFlowLayoutPanel.WrapContents = false;
            // 
            // importButton
            // 
            this.importButton.Location = new System.Drawing.Point(3, 3);
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size(23, 23);
            this.importButton.TabIndex = 1;
            this.importButton.UseVisualStyleBackColor = true;
            // 
            // exportButton
            // 
            this.exportButton.Location = new System.Drawing.Point(32, 3);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(23, 23);
            this.exportButton.TabIndex = 0;
            this.exportButton.UseVisualStyleBackColor = true;
            // 
            // listBox
            // 
            this.listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox.FormattingEnabled = true;
            this.listBox.Location = new System.Drawing.Point(0, 35);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(591, 256);
            this.listBox.TabIndex = 1;
            // 
            // ListBoxEditorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listBox);
            this.Controls.Add(this.topTableLayoutPanel);
            this.Name = "ListBoxEditorView";
            this.Size = new System.Drawing.Size(591, 291);
            this.topTableLayoutPanel.ResumeLayout(false);
            this.topTableLayoutPanel.PerformLayout();
            this.topLeftFlowLayoutPanel.ResumeLayout(false);
            this.topRightFlowLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel topTableLayoutPanel;
        private System.Windows.Forms.FlowLayoutPanel topLeftFlowLayoutPanel;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.Button cloneButton;
        private System.Windows.Forms.Button moveUpButton;
        private System.Windows.Forms.Button moveDownButton;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.FlowLayoutPanel topRightFlowLayoutPanel;
        private System.Windows.Forms.Button importButton;
        private System.Windows.Forms.Button exportButton;
        private System.Windows.Forms.ListBox listBox;
    }
}
