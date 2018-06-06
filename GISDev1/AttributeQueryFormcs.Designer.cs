namespace GISDev1
{
    partial class AttributeQueryFormcs
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
            this.comLayerName = new System.Windows.Forms.ComboBox();
            this.listFields = new System.Windows.Forms.ListBox();
            this.listUniqueValue = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comLayerName
            // 
            this.comLayerName.FormattingEnabled = true;
            this.comLayerName.Location = new System.Drawing.Point(291, 23);
            this.comLayerName.Name = "comLayerName";
            this.comLayerName.Size = new System.Drawing.Size(304, 26);
            this.comLayerName.TabIndex = 0;
            this.comLayerName.SelectedIndexChanged += new System.EventHandler(this.comLayerName_SelectedIndexChanged);
            // 
            // listFields
            // 
            this.listFields.FormattingEnabled = true;
            this.listFields.ItemHeight = 18;
            this.listFields.Location = new System.Drawing.Point(59, 106);
            this.listFields.Name = "listFields";
            this.listFields.Size = new System.Drawing.Size(240, 130);
            this.listFields.TabIndex = 1;
            this.listFields.SelectedIndexChanged += new System.EventHandler(this.listFields_SelectedIndexChanged);
            this.listFields.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listFields_MouseDoubleClick_1);
            // 
            // listUniqueValue
            // 
            this.listUniqueValue.FormattingEnabled = true;
            this.listUniqueValue.ItemHeight = 18;
            this.listUniqueValue.Location = new System.Drawing.Point(344, 106);
            this.listUniqueValue.Name = "listUniqueValue";
            this.listUniqueValue.Size = new System.Drawing.Size(251, 130);
            this.listUniqueValue.TabIndex = 2;
            this.listUniqueValue.SelectedIndexChanged += new System.EventHandler(this.listUniqueValue_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(409, 259);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(152, 33);
            this.button1.TabIndex = 3;
            this.button1.Text = "获取唯一值";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(59, 310);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(272, 59);
            this.textBox1.TabIndex = 4;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(520, 362);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 35);
            this.button2.TabIndex = 5;
            this.button2.Text = "查询";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(56, 274);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 18);
            this.label1.TabIndex = 6;
            this.label1.Text = "条件";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(59, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 18);
            this.label2.TabIndex = 7;
            this.label2.Text = "图层名称";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(59, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 18);
            this.label3.TabIndex = 8;
            this.label3.Text = "字段";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(344, 85);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 18);
            this.label4.TabIndex = 9;
            this.label4.Text = "值";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(622, 362);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(72, 35);
            this.button3.TabIndex = 10;
            this.button3.Text = "退出";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // AttributeQueryFormcs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(722, 409);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listUniqueValue);
            this.Controls.Add(this.listFields);
            this.Controls.Add(this.comLayerName);
            this.Name = "AttributeQueryFormcs";
            this.Text = "AttributeQueryFormcs";
            this.Load += new System.EventHandler(this.AttributeQueryFormcs_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comLayerName;
        private System.Windows.Forms.ListBox listFields;
        private System.Windows.Forms.ListBox listUniqueValue;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button3;
    }
}