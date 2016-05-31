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
            this.components = new System.ComponentModel.Container();
            this.btn_StartRegMail = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.nUD_CountMail = new System.Windows.Forms.NumericUpDown();
            this.tB_Log = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tB_names = new System.Windows.Forms.TextBox();
            this.tB_surnames = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.rb_men = new System.Windows.Forms.RadioButton();
            this.rb_women = new System.Windows.Forms.RadioButton();
            this.webControl1 = new Awesomium.Windows.Forms.WebControl(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.nUD_CountMail)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_StartRegMail
            // 
            this.btn_StartRegMail.Location = new System.Drawing.Point(119, 75);
            this.btn_StartRegMail.Name = "btn_StartRegMail";
            this.btn_StartRegMail.Size = new System.Drawing.Size(145, 39);
            this.btn_StartRegMail.TabIndex = 0;
            this.btn_StartRegMail.Text = "Зарегать email-ы";
            this.btn_StartRegMail.UseVisualStyleBackColor = true;
            this.btn_StartRegMail.Click += new System.EventHandler(this.btn_StartRegMail_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Зарегать почту";
            // 
            // nUD_CountMail
            // 
            this.nUD_CountMail.Location = new System.Drawing.Point(17, 93);
            this.nUD_CountMail.Name = "nUD_CountMail";
            this.nUD_CountMail.Size = new System.Drawing.Size(96, 20);
            this.nUD_CountMail.TabIndex = 2;
            this.nUD_CountMail.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // tB_Log
            // 
            this.tB_Log.Location = new System.Drawing.Point(12, 163);
            this.tB_Log.Multiline = true;
            this.tB_Log.Name = "tB_Log";
            this.tB_Log.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tB_Log.Size = new System.Drawing.Size(302, 347);
            this.tB_Log.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(663, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Имена";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Фамилии";
            // 
            // tB_names
            // 
            this.tB_names.Location = new System.Drawing.Point(70, 12);
            this.tB_names.Name = "tB_names";
            this.tB_names.Size = new System.Drawing.Size(244, 20);
            this.tB_names.TabIndex = 7;
            // 
            // tB_surnames
            // 
            this.tB_surnames.Location = new System.Drawing.Point(70, 45);
            this.tB_surnames.Name = "tB_surnames";
            this.tB_surnames.Size = new System.Drawing.Size(244, 20);
            this.tB_surnames.TabIndex = 8;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 134);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "браузер";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(93, 134);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 11;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // rb_men
            // 
            this.rb_men.AutoSize = true;
            this.rb_men.Location = new System.Drawing.Point(334, 9);
            this.rb_men.Name = "rb_men";
            this.rb_men.Size = new System.Drawing.Size(71, 17);
            this.rb_men.TabIndex = 14;
            this.rb_men.TabStop = true;
            this.rb_men.Text = "Мужской";
            this.rb_men.UseVisualStyleBackColor = true;
            // 
            // rb_women
            // 
            this.rb_women.AutoSize = true;
            this.rb_women.Location = new System.Drawing.Point(334, 25);
            this.rb_women.Name = "rb_women";
            this.rb_women.Size = new System.Drawing.Size(72, 17);
            this.rb_women.TabIndex = 15;
            this.rb_women.TabStop = true;
            this.rb_women.Text = "Женский";
            this.rb_women.UseVisualStyleBackColor = true;
            // 
            // webControl1
            // 
            this.webControl1.Location = new System.Drawing.Point(320, 62);
            this.webControl1.Size = new System.Drawing.Size(418, 448);
            this.webControl1.Source = new System.Uri("about:blank", System.UriKind.Absolute);
            this.webControl1.TabIndex = 16;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 522);
            this.Controls.Add(this.webControl1);
            this.Controls.Add(this.rb_women);
            this.Controls.Add(this.rb_men);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.tB_surnames);
            this.Controls.Add(this.tB_names);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tB_Log);
            this.Controls.Add(this.nUD_CountMail);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_StartRegMail);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tB_names;
        private System.Windows.Forms.TextBox tB_surnames;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.RadioButton rb_men;
        private System.Windows.Forms.RadioButton rb_women;
        private Awesomium.Windows.Forms.WebControl webControl1;
    }
}

