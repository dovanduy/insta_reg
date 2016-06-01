using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Text.RegularExpressions;
using mshtml;
using Awesomium.Core;

namespace insta_reg
{
    public partial class Form1 : Form
    {
        public const string antigate_key = "c84e9c641ae9112f5eae64b03110b186";

        public bool Loaded = false;

        public Form1()
        {
            WebCore.Initialize(new WebConfig());
            WebPreferences wp = new WebPreferences();
            wp.AcceptLanguage = "ru-ru";
            wp.ProxyConfig = "5.1.50.138:8080";
            WebSession ws = WebCore.CreateWebSession(wp);
            
            InitializeComponent();
            webControl1.WebSession = ws;


            /*
            var ws = WebCore.CreateWebSession(new WebPreferences() { ProxyConfig = "255.255.255:8080" });
            webControl1 = new Awesomium.Windows.Forms.WebControl() { WebSession = ws };
            this.Controls.Add(webControl1);*/
        }

        // Открытие файла
        private static List<string> OpenFile(string file)
        {
            if (file == "null")
                throw new System.Exception("Укажите файл!");
            if (File.Exists(file) != true)
                throw new System.Exception("Указанный файл не найден!");
            try
            { 
                var list = new List<string>();
                
                FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs, Encoding.Default);
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        line = Regex.Replace(line, @"\s|\.|\( ", "");
                        list.Add(line);
                    } 
                }
                fs.Close();
                //string[] notWhiteSpace = File.ReadAllLines(file, Encoding.Default).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                //list.AddRange(notWhiteSpace);
                return list;
            }
            catch
            {
                throw new System.Exception("Не получилось открыть файл");
            }
        }

        // Начать регистрацию
        private void btn_StartRegMail_Click(object sender, EventArgs e)
        {              
            // Получаем список имен
            var names = new List<string>();
            int names_count = 0;
            try
            {
                names = OpenFile(tB_names.Text);
                names_count = names.Count;
                ToLog("Имен: "+ Convert.ToString(names_count));
                //foreach (string line in names)
                  //  ToLog(line);
            }
            catch (Exception ex)
            {
                ToLog("Произошла ошибка с файлом имен: "+ ex.Message);
            }

            // Получаем список фамилий
            var surnames = new List<string>();
            int surnames_count = 0;
            try
            {
                surnames = OpenFile(tB_surnames.Text);
                surnames_count = surnames.Count;
                ToLog("Фамилий: " + Convert.ToString(surnames_count));
            }
            catch (Exception ex)
            {
                ToLog("Произошла ошибка с файлом фамилий: " + ex.Message);
            }

            // Устанавливаем пол
            string sex = "";
            if (rb_women.Checked == true)
            {
                sex = "1";
            }
            else if (rb_men.Checked == true)
            {
                sex = "2";
            }

            Random rnd = new Random();
            int i;
            int max = Convert.ToInt32(nUD_CountMail.Value);
            for (i=0;i<max;i++)
            {
                RegMail mailreg = new RegMail();
                int rand_name = rnd.Next(0, names_count);
                int rand_surname = rnd.Next(0, surnames_count);
                // 
                string res = mailreg.Reger(names[rand_name], surnames[rand_surname], sex, GenPass(10));
                ToLog(res);
                //RegFB2(res);
            }
            
            

        }

        // Генератор паролей
        private static string GenPass(int count)
        {
            string abc = "qwertyuiopasdfghjklzxcvbnm123456789";
            Random rnd = new Random();
            string pass = "";
            for (int i = 0; i < count; i++)
            {
                pass += abc[rnd.Next(abc.Length)];
            }
            return pass;
        }

        // Вывод сообщения в лог
        private void ToLog(string text)
        {
            tB_Log.AppendText(text+"\n");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RegFB fbreg = new RegFB();

            string text = "eduard-bobrov@inbox.ru;v2tt9elnbk;Александр;Власенков";

            string[] t = text.Split(';');

            string name = "Эдуард";
            string surname = "Попов";
            string email = t[0];
            string pass = t[1];
            string birthday = "20.05.1994";
            string sex = "male";

            fbreg.Reger(name, surname, email, pass, birthday, sex);
            //aleksandr - vlasenkov@bk.ru; 9tumgbw6it; Александр Власенков

        }

        // загрузка формы
        private void Form1_Load(object sender, EventArgs e)
        {
            string file_names = Directory.GetCurrentDirectory() + @"\files\имена.txt";
            string file_surnames = Directory.GetCurrentDirectory() + @"\files\фамилии.txt";
            tB_names.Text= file_names;
            tB_surnames.Text = file_surnames;
            rb_men.Checked = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {

            //RegFB("lozovskij@bk.ru;i82ikdrvpn;Валентин;Лозовский;8;5;1980;2");
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //throw new NotImplementedException();
            Loaded = true;
        }

        public void RegFB2(string data)
        {
            
            string[] d = data.Split(';');
            string name = d[2];
            string surname = d[3];
            string email = d[0];
            string bday = d[4];
            string bmonth = d[5];
            string byear = d[6];
            string sex = d[7];
            string pass = d[1];
            string mail_link = d[8];
            //string mail_link = "";
            //
            webControl1.Source = new Uri("https://m.facebook.com/");
            webControl1.LoadingFrameComplete += (s, m) =>
            {
                if (m.IsMainFrame)
                    Loaded = true;
            };
            while (!Loaded)
            {
                Thread.Sleep(50);
                WebCore.Update();
            }
            ToLog(webControl1.Source.AbsoluteUri);
            ToLog(webControl1.Source.AbsolutePath);
            //ToLog("ищем кнопку регистрации");
            dynamic document = (JSObject)webControl1.ExecuteJavascriptWithResult("document");
            using (document)
            {
                dynamic elem = document.getElementsByClassName("_54k8 _56bs _56bw _56bv");
                elem[0].click();
            }
            Loaded = false;
            webControl1.LoadingFrameComplete += (s, m) =>
            {
                if (m.IsMainFrame)
                    Loaded = true;
            };
            while (!Loaded)
            {
                Thread.Sleep(50);
                WebCore.Update();
            }
            ToLog(webControl1.Source.AbsoluteUri);
            ToLog(webControl1.Source.AbsolutePath);
            //ToLog("Заполняю поля");
            document = (JSObject)webControl1.ExecuteJavascriptWithResult("document");
            using (document)
            {
                document.getElementsByName("firstname")[0].value = name;
                document.getElementsByName("lastname")[0].value = surname;
                document.getElementsByName("reg_email__")[0].value = email;
                document.getElementsByName("birthday_day")[0].value = bday;
                document.getElementsByName("birthday_month")[0].value = bmonth;
                document.getElementsByName("birthday_year")[0].value = byear;
                document.getElementsByName("reg_passwd__")[0].value = pass;
                dynamic elem = document.getElementById("sex");
                elem.value = sex;
                elem = document.getElementById("signup_button");
                elem.click();
            }
            Loaded = false;
            webControl1.LoadingFrameComplete += (s, m) =>
            {
                if (m.IsMainFrame)
                    Loaded = true;
            };
            while (!Loaded)
            {
                Thread.Sleep(50);
                WebCore.Update();
            }
            ToLog(webControl1.Source.AbsoluteUri);
            ToLog(webControl1.Source.AbsolutePath);
            if (webControl1.Source.AbsolutePath == "/confirmemail.php")
            {
                ToLog("Подтверждаем по почте");
            }
            else
            {
                ToLog("Плохой прокси");
            }
            /*
            webControl1.Source = new Uri(mail_link);
            webControl1.LoadingFrameComplete += (s, m) =>
            {
                if (m.IsMainFrame)
                    Loaded = true;
            };
            while (!Loaded)
            {
                Thread.Sleep(50);
                WebCore.Update();
            }
            ToLog(webControl1.Source.AbsoluteUri);
            ToLog(webControl1.Source.AbsolutePath);
            */



            /*
            document = (JSObject)webControl1.ExecuteJavascriptWithResult("document");
            using (document)
            {
                dynamic elem = document.getElementById("nux-nav-button");
                elem.click();
            }*/
        }

        public void RegFB(string data)
        {
            string[] d = data.Split(';');
            string name = d[2];
            string surname = d[3];
            string email = d[0];
            string bday = d[4];
            string bmonth = d[5];
            string byear = d[6];
            string sex = d[7];
            string pass = d[1];

            HtmlElementCollection elems;
            /*
dynamic elem = document.getElementsByTagName("input");
for (int i = 0; i < elem.length; i++)
{
    if (elem[i].getAttribute("name") == "firstname")
        elem[i].setAttribute("value", name);
    if (elem[i].getAttribute("name") == "lastname")
        elem[i].setAttribute("value", surname);
    if (elem[i].getAttribute("name") == "reg_email__")
        elem[i].setAttribute("value", email);
    if (elem[i].getAttribute("name") == "birthday_day")
        elem[i].setAttribute("value", bday);
    if (elem[i].getAttribute("name") == "birthday_month")
        elem[i].setAttribute("value", bmonth);
    if (elem[i].getAttribute("name") == "birthday_year")
        elem[i].setAttribute("value", byear);
    if (elem[i].getAttribute("name") == "reg_passwd__")
        elem[i].setAttribute("value", pass);
}
*/
            //webBrowser1.Navigate("https://m.facebook.com/", "_self", null, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2486.0 Safari/537.36 Edge/13.10586");
            //webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted);
            // нажимаем кнопку
            /*
            Loaded = false;
            while (!Loaded)
            {
                Application.DoEvents();
            }
            elems = webBrowser1.Document.GetElementsByTagName("a");
            foreach (HtmlElement a in elems)
            {
                if (a.GetAttribute("className") == "n o bs bp q")
                {
                    a.InvokeMember("click");
                }
            }
            // заполняем поля
            Loaded = false;
            while (!Loaded)
            {
                Application.DoEvents();
            }
            elems = webBrowser1.Document.GetElementsByTagName("input");
            foreach (HtmlElement el in elems)
            {
                if (el.GetAttribute("name") == "firstname")
                    el.SetAttribute("value", name);
                if (el.GetAttribute("name") == "lastname")
                    el.SetAttribute("value", surname);
                if (el.GetAttribute("name") == "reg_email__")
                    el.SetAttribute("value", email);
                if (el.GetAttribute("name") == "birthday_day")
                    el.SetAttribute("value", bday);
                if (el.GetAttribute("name") == "birthday_month")
                    el.SetAttribute("value", bmonth);
                if (el.GetAttribute("name") == "birthday_year")
                    el.SetAttribute("value", byear);
                if (el.GetAttribute("name") == "reg_passwd__")
                    el.SetAttribute("value", pass);
            }
            elems = webBrowser1.Document.GetElementsByTagName("select");
            foreach (HtmlElement el in elems)
            {
                if (el.GetAttribute("name") == "sex")
                    el.SetAttribute("value", sex);
            }
            */
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string data = textBox1.Text;
            openMail(data);
            //RegFB2("aleksej-kulagin91@mail.ru;v5ovmyuqlt;Алексей;Кулагин;7;10;1991;2;https://auth.mail.ru/cgi-bin/auth?fromreg=touch&agent=AG_ROflNytiCnzmPoT1pAv9H&Login=aleksej-kulagin91%40mail.ru&Page=https%3A%2F%2Ftouch.mail.ru%2Fmessages%2Finbox");
        }

        public void openMail(string data)
        {
            string[] d = data.Split(';');
            string name = d[2];
            string surname = d[3];
            string email = d[0];
            string bday = d[4];
            string bmonth = d[5];
            string byear = d[6];
            string sex = d[7];
            string pass = d[1];
            string mail_link = d[8];
            webMail.Source = new Uri(mail_link);
            webMail.LoadingFrameComplete += (s, m) =>
            {
                if (m.IsMainFrame)
                    Loaded = true;
            };
            while (!Loaded)
            {
                Thread.Sleep(50);
                WebCore.Update();
            }
            ToLog(webMail.Source.AbsoluteUri);
            ToLog(webMail.Source.AbsolutePath);
            //
            dynamic document = (JSObject)webMail.ExecuteJavascriptWithResult("document");
            using (document)
            {
                document.getElementsByClassName("js-login-page__external__input_password login-page__external_input login-page__external_input__password")[0].value = pass;
                document.getElementsByClassName("btn btn_main js-login-page__external__submit")[0].click();
            }
            Loaded = false;
            while (!Loaded)
            {
                Thread.Sleep(50);
                WebCore.Update();
            }
            ToLog(webMail.Source.AbsoluteUri);
            ToLog(webMail.Source.AbsolutePath);
            document = (JSObject)webMail.ExecuteJavascriptWithResult("document");
            using (document)
            {
                dynamic elem = document.getElementsByClassName("js-message-link messageline__link");
                for (int i = 0; i < elem.length; i++)
                {
                    ToLog(elem[i].getElementsByClassName("messageline__subject"));
                    
                }
                
            }
        }

    /*
        private void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Текстовый документ (*.txt)|*.txt|Все файлы (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName);
                streamWriter.WriteLine(tB_Log.Text);
                streamWriter.Close();
            }
        }*/
        // Предусмотреть очистку в классах регистрации
        // Обработчик занятого логина в ФБ
    }
}