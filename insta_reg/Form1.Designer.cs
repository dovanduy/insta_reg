namespace insta_reg
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_StartRegMail = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.nUD_CountMail = new System.Windows.Forms.NumericUpDown();
            this.tB_Log = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.nUD_CountMail)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_StartRegMail
            // 
            this.btn_StartRegMail.Location = new System.Drawing.Point(264, 13);
            this.btn_StartRegMail.Name = "btn_StartRegMail";
            this.btn_StartRegMail.Size = new System.Drawing.Size(75, 21);
            this.btn_StartRegMail.TabIndex = 0;
            this.btn_StartRegMail.Text = "Начать";
            this.btn_StartRegMail.UseVisualStyleBackColor = true;
            this.btn_StartRegMail.Click += new System.EventHandler(this.btn_StartRegMail_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Зарегать почту";
            // 
            // nUD_CountMail
            // 
            this.nUD_CountMail.Location = new System.Drawing.Point(120, 13);
            this.nUD_CountMail.Name = "nUD_CountMail";
            this.nUD_CountMail.Size = new System.Drawing.Size(118, 20);
            this.nUD_CountMail.TabIndex = 2;
            this.nUD_CountMail.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // tB_Log
            // 
            this.tB_Log.Location = new System.Drawing.Point(13, 84);
            this.tB_Log.Multiline = true;
            this.tB_Log.Name = "tB_Log";
            this.tB_Log.Size = new System.Drawing.Size(577, 258);
            this.tB_Log.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(515, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(119, 47);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(118, 21);
            this.button2.TabIndex = 5;
            this.button2.Text = "Отправить решение";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(13, 48);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 354);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tB_Log);
            this.Controls.Add(this.nUD_CountMail);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_StartRegMail);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.nUD_CountMail)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_StartRegMail;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nUD_CountMail;
        private System.Windows.Forms.TextBox tB_Log;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox1;
    }
}

