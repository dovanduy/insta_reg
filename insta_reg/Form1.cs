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
            Random rnd = new Random();
            string year = Convert.ToString(rnd.Next(1980, 1998));
            string month = Convert.ToString(rnd.Next(1, 12));
            string day = Convert.ToString(rnd.Next(1, 28));
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
            parametrs += "&domain=" + HttpUtility.UrlEncode("mail.ru");
            parametrs += "&sex=" + HttpUtility.UrlEncode("male");
            parametrs += "&birthday=" + HttpUtility.UrlEncode("{\"year\":\""+year+"\",\"month\":\""+month+"\",\"day\":\""+day+"\"}");
            parametrs += "&name=" + HttpUtility.UrlEncode("{\"first\":\"" + name + "\",\"last\":\"" + surname + "\"}");
            post = POST("https://touch.mail.ru/api/v1/user/signup", parametrs, Cook);
            ToLog(post);
            var js = JsonConvert.DeserializeObject<Json>(post);

            // Проверяем полученные данные
            if (js.body.login.error == "exists")
            {
                ToLog("Неверный логин");
                parametrs = "email=" + HttpUtility.UrlEncode(login + "@mail.ru");
                parametrs += "&login=" + HttpUtility.UrlEncode(login);
                parametrs += "&birthday=" + HttpUtility.UrlEncode("{\"year\":\"" + year + "\",\"month\":\"" + month + "\",\"day\":\"" + day + "\"}");
                post = POST("https://touch.mail.ru/api/v1/user/exists", parametrs, Cook);
                js = JsonConvert.DeserializeObject<Json>(post);
                string alternative = js.body.alternatives[0];
                ToLog("Предложен логин: " + alternative);

                // Отправляем с новым логином
                Char delimiter = '@';
                int pos = alternative.IndexOf(delimiter);
                string login_new = alternative.Substring(0, pos);
                string domain = alternative.Substring(pos + 1, alternative.Length - login_new.Length - 1);
                parametrs = "email=" + HttpUtility.UrlEncode(alternative);
                parametrs = "&login=" + HttpUtility.UrlEncode(login_new);
                parametrs += "&domain=" + HttpUtility.UrlEncode(domain);
                parametrs += "&sex=" + HttpUtility.UrlEncode("male");
                parametrs += "&birthday=" + HttpUtility.UrlEncode("{\"year\":\"" + year + "\",\"month\":\"" + month + "\",\"day\":\"" + day + "\"}");
                parametrs += "&name=" + HttpUtility.UrlEncode("{\"first\":\"" + name + "\",\"last\":\"" + surname + "\"}");
                parametrs += "&password=" + HttpUtility.UrlEncode(GenPass(10));
                post = POST("https://touch.mail.ru/api/v1/user/signup", parametrs, Cook);
                ToLog(post);
                Json2 js2 = JsonConvert.DeserializeObject<Json2>(post);
                string captcha = js2.body;
                ToLog(captcha);

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
                            ToLog("капча разгадана");
                            // завершаем регистрацию
                            parametrs = "email=" + HttpUtility.UrlEncode(alternative);
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
                                ToLog("Все ОК");
                                url = body;
                                // Заходим на страницу и проверяем успешность
                                /*
                                 * 
                                 * 
                                 * 
                                 * 
                                */
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
        }

        private void btn_StartRegMail_Click(object sender, EventArgs e)
        {
            string name = "Галина";
            string surname = "Пирожкова";

            RegMail(name, surname);

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
    }
}
