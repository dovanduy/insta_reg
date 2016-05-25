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

namespace insta_reg
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_StartRegMail_Click(object sender, EventArgs e)
        {
            //переменные
            string parametrs;
            CookieContainer Cook = new CookieContainer();
            string post;
            //JsonConvert json;

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
            parametrs = "login=" + HttpUtility.UrlEncode("anton.popov");
            parametrs += "&domain=" + HttpUtility.UrlEncode("mail.ru");
            parametrs += "&sex=" + HttpUtility.UrlEncode("male");
            parametrs += "&birthday=" + HttpUtility.UrlEncode("{\"year\":\"1980\",\"month\":\"1\",\"day\":\"20\"}");
            parametrs += "&name=" + HttpUtility.UrlEncode("{\"first\":\"Антон\",\"last\":\"Попов\"}");
            post = POST("https://touch.mail.ru/api/v1/user/signup", parametrs, Cook);
            ToLog(post);
            var js = JsonConvert.DeserializeObject<Json>(post);

            // Проверяем полученные данные
            if (js.body.login.error == "exists")
            {
                ToLog("Неверный логин");
                parametrs = "email=" + HttpUtility.UrlEncode("anton.popov@mail.ru");
                parametrs += "&login=" + HttpUtility.UrlEncode("anton.popov");
                parametrs += "&birthday=" + HttpUtility.UrlEncode("{\"year\":\"1980\",\"month\":\"1\",\"day\":\"20\"}");
                post = POST("https://touch.mail.ru/api/v1/user/exists", parametrs, Cook);
                js = JsonConvert.DeserializeObject<Json>(post);
                string alternative = js.body.alternatives[0];
                ToLog("Предложен логин: "+ alternative);

                // Отправляем с новым логином
                Char delimiter = '@';
                int pos = alternative.IndexOf(delimiter);
                string login_new = alternative.Substring(0, pos);
                string domain = alternative.Substring(pos+1, alternative.Length - login_new.Length - 1);
                parametrs = "email=" + HttpUtility.UrlEncode(alternative);
                parametrs = "&login=" + HttpUtility.UrlEncode(login_new);
                parametrs += "&domain=" + HttpUtility.UrlEncode(domain);
                parametrs += "&sex=" + HttpUtility.UrlEncode("male");
                parametrs += "&birthday=" + HttpUtility.UrlEncode("{\"year\":\"1980\",\"month\":\"1\",\"day\":\"20\"}");
                parametrs += "&name=" + HttpUtility.UrlEncode("{\"first\":\"Антон\",\"last\":\"Попов\"}");
                parametrs += "&password=" + HttpUtility.UrlEncode(GenPass(10));
                post = POST("https://touch.mail.ru/api/v1/user/signup", parametrs, Cook);
                ToLog(post);
                Json2 js2 = JsonConvert.DeserializeObject<Json2>(post);
                string captcha = js2.body;
                ToLog(captcha);

                // Решаем каптчу
                string captcha_res = "dgfdfgd";

                // Отправляем каптчу
                parametrs = "email=" + HttpUtility.UrlEncode("anton.popov@mail.ru");
                parametrs += "&htmlencoded=" + HttpUtility.UrlEncode("false");
                parametrs += "&reg_anketa=" + HttpUtility.UrlEncode("{\"capcha\":\""+captcha_res+"\",\"id\":\""+captcha+"\"}");
                post = POST("https://touch.mail.ru/api/v1/user/signup/confirm", parametrs, Cook);
                //js2 = JsonConvert.DeserializeObject<Json2>(post);
                //ToLog("Ответ: " + js2.body);
                //

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

        // Вывод сообщения в лог
        private void ToLog(string text)
        {
            tB_Log.AppendText(text+"\n");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string json1 = @"{'body':{ 'reg_anketa.capcha':{ 'value': 'dgfdfgd', 'error': 'invalid'} },'email':'anton.popov@mail.ru','status':400,'htmlencoded':false}";
            string json2 = @"{'body':'good','email':'anton.popov@mail.ru','status':400,'htmlencoded':false}";

            dynamic parsed = JObject.Parse(json2);
            dynamic body = parsed["body"];

            if (body.GetType().Name == "JObject")
            {
                ToLog("Капча");
            }
            else
            {
                ToLog("Все ОК");
            }


            //ToLog(Translit.Front("Антон Попов"));
            /*
            string json_resp = "{\"Tracks\":[{\"Artist\":\"Artist1\",\"Album\":\"Album1\",\"Title\":\"Title1\",\"Year\":\"2015\"}]}";
            ToLog(json_resp);
            var js = JsonConvert.DeserializeObject<Json>(json_resp);
            ToLog(js.body.login.value);*/
            //ToLog(Json.Login);

        }


    }
}
