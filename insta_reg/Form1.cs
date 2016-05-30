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

namespace insta_reg
{
    public partial class Form1 : Form
    {
        public const string antigate_key = "c84e9c641ae9112f5eae64b03110b186";

        public Form1()
        {
            InitializeComponent();
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

            Random rnd = new Random();
            int i;
            int max = Convert.ToInt32(nUD_CountMail.Value);
            for (i=0;i<max;i++)
            {
                RegMail mailreg = new RegMail();
                int rand_name = rnd.Next(0, names_count);
                int rand_surname = rnd.Next(0, surnames_count);
                ToLog(mailreg.Reger(names[rand_name], surnames[rand_surname], GenPass(10)));
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
        }

        private void button2_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://m.facebook.com/");

        }

        private void button3_Click(object sender, EventArgs e)
        {
            mshtml.HTMLDocument doc = axWebBrowser1.Document as mshtml.HTMLDocument;
            foreach (IHTMLElement a in doc.all)
            {
                if ((a.className == "ok-btn-go ok-btn") &&
                    (a.tagName == "A"))
                {
                    a.click();
                }
            }

            foreach (HtmlElement he in webBrowser1.Document.GetElementsByTagName("span"))
            {
                ToLog(he.OuterHtml);
            }
        }
    }
}


// Предусмотреть очистку в классах регистрации
// Обработчик занятого логина в ФБ