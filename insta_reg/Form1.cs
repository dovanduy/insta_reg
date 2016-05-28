﻿using System;
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

        private void RegMail(string name, string surname)
        {
            string parametrs;
            CookieContainer Cook = new CookieContainer();
            string post;
            string login = Translit.Front(name + "." + surname);
            login = login.ToLower();
            string domain = "mail.ru";
            string email = login + "@" + domain;
            string pass = GenPass(10);
            Random rnd = new Random();
            string year = Convert.ToString(rnd.Next(1980, 1998));
            string month = Convert.ToString(rnd.Next(1, 12));
            string day = Convert.ToString(rnd.Next(1, 28));
            string captcha = "";
            
            //1  - заходим на сайт
            string url = "https://touch.mail.ru/cgi-bin/signup";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, image/jxr, */*";
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "ru-RU");
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2486.0 Safari/537.36 Edge/13.10586";
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            request.Host = "touch.mail.ru";
            request.KeepAlive = true;
            request.CookieContainer = Cook;
            //
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            //tB_Log.Text = reader1.ReadToEnd();
            //string cookies = String.IsNullOrEmpty(response.Headers["Set-Cookie"]) ? "" : response.Headers["Set-Cookie"];
            //tB_Log.Text = cookies;

            // 2 
            parametrs = "login=" + HttpUtility.UrlEncode(login);
            parametrs += "&domain=" + HttpUtility.UrlEncode(domain);
            parametrs += "&sex=" + HttpUtility.UrlEncode("male");
            parametrs += "&birthday=" + HttpUtility.UrlEncode("{\"year\":\""+year+"\",\"month\":\""+month+"\",\"day\":\""+day+"\"}");
            parametrs += "&name=" + HttpUtility.UrlEncode("{\"first\":\"" + name + "\",\"last\":\"" + surname + "\"}");
            post = POST("https://touch.mail.ru/api/v1/user/signup", parametrs, Cook);
            //ToLog(post);
            var js = JsonConvert.DeserializeObject<Json>(post);

            /*
            dynamic parsed = JObject.Parse(post);
            dynamic body = parsed["body"];
            if (body.GetType().Name == "JObject")
            {
                string error = body["reg_anketa.capcha"]["value"];
                ToLog("Капча " + error);
            }
            else
            {

            }
            */

            // Если проблемы с логином
            if (js.body.login != null)
            {
                // Если неправильный логин
                if (js.body.login.error == "exists")
                {
                    //ToLog("Неверный логин");
                    parametrs = "email=" + HttpUtility.UrlEncode(email);
                    parametrs += "&login=" + HttpUtility.UrlEncode(login);
                    parametrs += "&birthday=" + HttpUtility.UrlEncode("{\"year\":\"" + year + "\",\"month\":\"" + month + "\",\"day\":\"" + day + "\"}");
                    post = POST("https://touch.mail.ru/api/v1/user/exists", parametrs, Cook);
                    js = JsonConvert.DeserializeObject<Json>(post);
                    email = js.body.alternatives[0];
                    //ToLog("Предложен логин: " + email);

                    // Отправляем с новым логином
                    Char delimiter = '@';
                    int pos = email.IndexOf(delimiter);
                    login = email.Substring(0, pos);
                    domain = email.Substring(pos + 1, email.Length - login.Length - 1);
                    parametrs = "email=" + HttpUtility.UrlEncode(email);
                    parametrs = "&login=" + HttpUtility.UrlEncode(login);
                    parametrs += "&domain=" + HttpUtility.UrlEncode(domain);
                    parametrs += "&sex=" + HttpUtility.UrlEncode("male");
                    parametrs += "&birthday=" + HttpUtility.UrlEncode("{\"year\":\"" + year + "\",\"month\":\"" + month + "\",\"day\":\"" + day + "\"}");
                    parametrs += "&name=" + HttpUtility.UrlEncode("{\"first\":\"" + name + "\",\"last\":\"" + surname + "\"}");
                    parametrs += "&password=" + HttpUtility.UrlEncode(pass);
                    post = POST("https://touch.mail.ru/api/v1/user/signup", parametrs, Cook);
                    //ToLog(post);
                    Json2 js2 = JsonConvert.DeserializeObject<Json2>(post);
                    captcha = js2.body;
                    //ToLog(captcha);
                }
            }
            else
            {
                 // Отправляем все с паролем
                    parametrs = "email=" + HttpUtility.UrlEncode(login + "@" + domain);
                    parametrs = "&login=" + HttpUtility.UrlEncode(login);
                    parametrs += "&domain=" + HttpUtility.UrlEncode(domain);
                    parametrs += "&sex=" + HttpUtility.UrlEncode("male");
                    parametrs += "&birthday=" + HttpUtility.UrlEncode("{\"year\":\"" + year + "\",\"month\":\"" + month + "\",\"day\":\"" + day + "\"}");
                    parametrs += "&name=" + HttpUtility.UrlEncode("{\"first\":\"" + name + "\",\"last\":\"" + surname + "\"}");
                    parametrs += "&password=" + HttpUtility.UrlEncode(pass);
                    post = POST("https://touch.mail.ru/api/v1/user/signup", parametrs, Cook);
                    //ToLog(post);
                    Json2 js2 = JsonConvert.DeserializeObject<Json2>(post);
                    captcha = js2.body;
                    //ToLog(captcha);
            }


                // Получаем капчу
                url = "https://c.mail.ru/6?r=0.71848591092699836";
                byte[] imageBytes;
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Accept = "image/png, image/svg+xml, image/jxr, image/*;q=0.8, */*;q=0.5";
                request.Headers.Add(HttpRequestHeader.AcceptLanguage, "ru-RU");
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2486.0 Safari/537.36 Edge/13.10586";
                request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                request.Host = "c.mail.ru";
                request.KeepAlive = true;
                request.CookieContainer = Cook;
                response = (HttpWebResponse)request.GetResponse();
                Stream reader2 = response.GetResponseStream();
                using (BinaryReader br = new BinaryReader(reader2))
                {
                    imageBytes = br.ReadBytes(500000);
                    br.Close();
                }
                reader.Close();
                response.Close();
                var base64 = Convert.ToBase64String(imageBytes);


                // Отправляем капчу 
                string captha_res = SendToAntigate(base64);
                if (captha_res != "error")
                {
                    string capcha_id = captha_res;
                    // Проверяем решение капчи                    
                    bool good = false;
                    while (good == false)
                    {
                        Thread.Sleep(3000);
                        string IsReady = IsReadyCapcha(capcha_id);
                        if (IsReady.IndexOf("OK|") > -1)
                        {
                            good = true;
                            //ToLog("капча разгадана");
                            // завершаем регистрацию
                            parametrs = "email=" + HttpUtility.UrlEncode(email);
                            parametrs += "&htmlencoded=" + HttpUtility.UrlEncode("false");
                            parametrs += "&reg_anketa=" + HttpUtility.UrlEncode("{\"capcha\":\"" + IsReady.Substring(3) + "\",\"id\":\"" + captcha + "\"}");
                            post = POST("https://touch.mail.ru/api/v1/user/signup/confirm", parametrs, Cook);
                            dynamic parsed = JObject.Parse(post);
                            dynamic body = parsed["body"];
                            if (body.GetType().Name == "JObject")
                            {
                                string error = body["reg_anketa.capcha"]["value"];
                                ToLog("Капча " + error);
                            }
                            else
                            {
                                string email2 = parsed["email"];
                                ToLog(email2 + ";" + pass + ";" + name + " " + surname);
                                //ToLog("Все ОК");
                                url = body;
                                // Заходим на страницу и проверяем успешность
                            }
                        }
                    }

                }


                /*
                string captcha_res = "dgfdfgd";
                parametrs = "email=" + HttpUtility.UrlEncode("anton.popov@mail.ru");
                parametrs += "&htmlencoded=" + HttpUtility.UrlEncode("false");
                parametrs += "&reg_anketa=" + HttpUtility.UrlEncode("{\"capcha\":\""+captcha_res+"\",\"id\":\""+captcha+"\"}");
                post = POST("https://touch.mail.ru/api/v1/user/signup/confirm", parametrs, Cook);
                dynamic parsed = JObject.Parse(post);
                dynamic body = parsed["body"];
                if (body.GetType().Name == "JObject")
                {
                    string error = body["reg_anketa.capcha"]["value"];
                    ToLog("Капча "+error);
                }
                else
                {
                    ToLog("Все ОК");
                }
                */

            
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
            for (i=0;i<10;i++)
            {
                int rand_name = rnd.Next(0, names_count);
                int rand_surname = rnd.Next(0, surnames_count);
                RegMail(names[rand_name], surnames[rand_surname]);
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

        // POST-запрос
        private static string POST(string url, string parametrs, CookieContainer Cook)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Accept = "*/*";
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Referer = "https://touch.mail.ru/cgi-bin/signup";
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "ru-RU");
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2486.0 Safari/537.36 Edge/13.10586";
            request.Host = "touch.mail.ru";
            request.KeepAlive = true;
            request.Headers.Add("Cache-Control", "no-cache");
            request.ServicePoint.Expect100Continue = false;
            request.CookieContainer = Cook;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            //данные запроса
            //string parametrs;
            byte[] postData = Encoding.UTF8.GetBytes(parametrs);
            request.ContentLength = postData.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(postData, 0, postData.Length);
            }
            // разбираем ответ
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string res = reader.ReadToEnd();
            //var js = JsonConvert.DeserializeObject<Json>(res);
            response.Close();
            return res;
        }
        
        // Отправить капчу в антигейт
        private static string SendToAntigate(string base64)
        {
            string url = "http://anti-captcha.com/in.php";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Accept = "*/*";
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Referer = "https://touch.mail.ru/cgi-bin/signup";
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "ru-RU");
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2486.0 Safari/537.36 Edge/13.10586";
            //request.Host = "touch.mail.ru";
            request.KeepAlive = true;
            request.Headers.Add("Cache-Control", "no-cache");
            request.ServicePoint.Expect100Continue = false;
            //request.CookieContainer = Cook;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            //данные запроса
            string parametrs = "";
            parametrs = "method=" + HttpUtility.UrlEncode("base64"); ;
            parametrs += "&key=" + HttpUtility.UrlEncode(antigate_key);
            parametrs += "&body=" + HttpUtility.UrlEncode(base64);
            byte[] postData = Encoding.UTF8.GetBytes(parametrs);
            request.ContentLength = postData.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(postData, 0, postData.Length);
            }
            // разбираем ответ
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string res = reader.ReadToEnd();
            reader.Close();
            response.Close();
            //
            if (res.IndexOf("OK|") > -1)
            {
                //string login_new = res.Substring(3);
                return res.Substring(3);
            }else{
                return "error";
            }
        }

        // Проверить решение капчи
        private static string IsReadyCapcha(string captcha_id)
        {
            string url = "http://anti-captcha.com/res.php";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Accept = "*/*";
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Referer = "https://touch.mail.ru/cgi-bin/signup";
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "ru-RU");
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2486.0 Safari/537.36 Edge/13.10586";
            request.KeepAlive = true;
            request.Headers.Add("Cache-Control", "no-cache");
            request.ServicePoint.Expect100Continue = false;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            string parametrs = "";
            parametrs = "action=" + HttpUtility.UrlEncode("get"); ;
            parametrs += "&key=" + HttpUtility.UrlEncode(antigate_key);
            parametrs += "&id=" + HttpUtility.UrlEncode(captcha_id);
            byte[] postData = Encoding.UTF8.GetBytes(parametrs);
            request.ContentLength = postData.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(postData, 0, postData.Length);
            }
            // разбираем ответ
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string res = reader.ReadToEnd();
            reader.Close();
            response.Close();
            return res;
            //
            /*
            if (res.IndexOf("OK|") > -1)
            {
                //string login_new = res.Substring(3);
                return res.Substring(3);
            }
            else
            {
                return "error";
            }*/
        }

        // Вывод сообщения в лог
        private void ToLog(string text)
        {
            tB_Log.AppendText(text+"\n");
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string file_names = Directory.GetCurrentDirectory() + @"\files\имена.txt";
            string file_surnames = Directory.GetCurrentDirectory() + @"\files\фамилии.txt";
            tB_names.Text= file_names;
            tB_surnames.Text = file_surnames;
        }
    }
}
