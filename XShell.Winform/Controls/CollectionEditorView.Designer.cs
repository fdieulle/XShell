namespace XShell.Winform.Controls
{
    partial class CollectionEditorView
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.addButton = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            this.cloneButton = new System.Windows.Forms.Button();
            this.moveUpButton = new System.Windows.Forms.Button();
            this.moveDownButton = new System.Windows.Forms.Button();
            this.clearButton = new System.Windows.Forms.Button();
            this.fastDataListView1 = new BrightIdeasSoftware.FastDataListView();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fastDataListView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 7;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.addButton, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.removeButton, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.cloneButton, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.moveUpButton, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.moveDownButton, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.clearButton, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.fastDataListView1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(539, 506);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // addButton
            // 
            this.addButton.BackgroundImage = global::XShell.Winform.Properties.Resources.add;
            this.addButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.addButton.FlatAppearance.BorderSize = 0;
            this.addButton.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
            this.addButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.addButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addButton.Location = new System.Drawing.Point(3, 3);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(23, 23);
            this.addButton.TabIndex = 0;
            this.addButton.UseVisualStyleBackColor = true;
            // 
            // removeButton
            // 
            this.removeButton.BackgroundImage = global::XShell.Winform.Properties.Resources.remove;
            this.removeButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.removeButton.FlatAppearance.BorderSize = 0;
            this.removeButton.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
            this.removeButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.removeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.removeButton.Location = new System.Drawing.Point(32, 3);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(23, 23);
            this.removeButton.TabIndex = 1;
            this.removeButton.UseVisualStyleBackColor = true;
            // 
            // cloneButton
            // 
            this.cloneButton.BackgroundImage = global::XShell.Winform.Properties.Resources.clone;
            this.cloneButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cloneButton.FlatAppearance.BorderSize = 0;
            this.cloneButton.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
            this.cloneButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.cloneButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cloneButton.Location = new System.Drawing.Point(61, 3);
            this.cloneButton.Name = "cloneButton";
            this.cloneButton.Size = new System.Drawing.Size(23, 23);
            this.cloneButton.TabIndex = 2;
            this.cloneButton.UseVisualStyleBackColor = true;
            // 
            // moveUpButton
            // 
            this.moveUpButton.BackgroundImage = global::XShell.Winform.Properties.Resources.move_up;
            this.moveUpButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.moveUpButton.FlatAppearance.BorderSize = 0;
            this.moveUpButton.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
            this.moveUpButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.moveUpButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.moveUpButton.Location = new System.Drawing.Point(90, 3);
            this.moveUpButton.Name = "moveUpButton";
            this.moveUpButton.Size = new System.Drawing.Size(23, 23);
            this.moveUpButton.TabIndex = 3;
            this.moveUpButton.UseVisualStyleBackColor = true;
            // 
            // moveDownButton
            // 
            this.moveDownButton.BackgroundImage = global::XShell.Winform.Properties.Resources.move_down;
            this.moveDownButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.moveDownButton.FlatAppearance.BorderSize = 0;
            this.moveDownButton.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
            this.moveDownButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.moveDownButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.moveDownButton.Location = new System.Drawing.Point(119, 3);
            this.moveDownButton.Name = "moveDownButton";
            this.moveDownButton.Size = new System.Drawing.Size(23, 23);
            this.moveDownButton.TabIndex = 4;
            this.moveDownButton.UseVisualStyleBackColor = true;
            // 
            // clearButton
            // 
            this.clearButton.BackgroundImage = global::XShell.Winform.Properties.Resources.clear;
            this.clearButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.clearButton.FlatAppearance.BorderSize = 0;
            this.clearButton.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
            this.clearButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.clearButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearButton.Location = new System.Drawing.Point(148, 3);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(23, 23);
            this.clearButton.TabIndex = 5;
            this.clearButton.UseVisualStyleBackColor = true;
            // 
            // fastDataListView1
            // 
            this.fastDataListView1.CellEditUseWholeCell = false;
            this.tableLayoutPanel1.SetColumnSpan(this.fastDataListView1, 7);
            this.fastDataListView1.DataSource = null;
            this.fastDataListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fastDataListView1.FullRowSelect = true;
            this.fastDataListView1.GridLines = true;
            this.fastDataListView1.Location = new System.Drawing.Point(3, 32);
            this.fastDataListView1.Name = "fastDataListView1";
            this.fastDataListView1.ShowGroups = false;
            this.fastDataListView1.Size = new System.Drawing.Size(533, 471);
            this.fastDataListView1.TabIndex = 6;
            this.fastDataListView1.UseCompatibleStateImageBehavior = false;
            this.fastDataListView1.View = System.Windows.Forms.View.Details;
            this.fastDataListView1.VirtualMode = true;
            // 
            // CollectionEditorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "CollectionEditorView";
            this.Size = new System.Drawing.Size(539, 506);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fastDataListView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.Button cloneButton;
        private System.Windows.Forms.Button moveUpButton;
        private System.Windows.Forms.Button moveDownButton;
        private System.Windows.Forms.Button clearButton;
        private BrightIdeasSoftware.FastDataListView fastDataListView1;
    }
}
