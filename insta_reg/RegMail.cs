using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace insta_reg
{
    class RegMail
    {
        public string Reger(string name, string surname, string sex, string pass)
        {
            string parametrs;
            CookieContainer Cook = new CookieContainer();
            string post;
            string login = Translit.Front(name + "." + surname);
            login = login.ToLower();
            string domain = "mail.ru";
            string email = login + "@" + domain;
            //string pass = GenPass(10);
            Random rnd = new Random();
            string year = Convert.ToString(rnd.Next(1980, 1998));
            string month = Convert.ToString(rnd.Next(1, 12));
            string day = Convert.ToString(rnd.Next(1, 28));
            // пол
            string sexxx="female";
            if (sex == "2")
                sexxx = "male";
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
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            parametrs = "login=" + HttpUtility.UrlEncode(login);
            parametrs += "&domain=" + HttpUtility.UrlEncode(domain);
            parametrs += "&sex=" + HttpUtility.UrlEncode(sexxx);
            parametrs += "&birthday=" + HttpUtility.UrlEncode("{\"year\":\"" + year + "\",\"month\":\"" + month + "\",\"day\":\"" + day + "\"}");
            parametrs += "&name=" + HttpUtility.UrlEncode("{\"first\":\"" + name + "\",\"last\":\"" + surname + "\"}");
            post = POST("https://touch.mail.ru/api/v1/user/signup", parametrs, Cook);
            //ToLog(post);
            var js = JsonConvert.DeserializeObject<Json>(post);

            // Если неправильный логин
            if (js.body.login != null && js.body.login.error == "exists")
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
                parametrs += "&sex=" + HttpUtility.UrlEncode(sexxx);
                parametrs += "&birthday=" + HttpUtility.UrlEncode("{\"year\":\"" + year + "\",\"month\":\"" + month + "\",\"day\":\"" + day + "\"}");
                parametrs += "&name=" + HttpUtility.UrlEncode("{\"first\":\"" + name + "\",\"last\":\"" + surname + "\"}");
                parametrs += "&password=" + HttpUtility.UrlEncode(pass);
                post = POST("https://touch.mail.ru/api/v1/user/signup", parametrs, Cook);
                //ToLog(post);
                Json2 js2 = JsonConvert.DeserializeObject<Json2>(post);
                captcha = js2.body;
                //ToLog(captcha);
            }
            else
            {
                // Отправляем все с паролем
                parametrs = "email=" + HttpUtility.UrlEncode(login + "@" + domain);
                parametrs = "&login=" + HttpUtility.UrlEncode(login);
                parametrs += "&domain=" + HttpUtility.UrlEncode(domain);
                parametrs += "&sex=" + HttpUtility.UrlEncode(sexxx);
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

            
            // Инициализируем каптчу
            Captcha Captcha = new Captcha();
            // Отправляем капчу 
            string captha_res = Captcha.SendToAntigate(base64);
            if (captha_res != "error")
            {
                string capcha_id = captha_res;
                // Проверяем решение капчи                    
                bool good = false;
                while (good == false)
                {
                    Thread.Sleep(3000);
                    string IsReady = Captcha.IsReadyCapcha(capcha_id);
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
                            throw new System.Exception(error);
                            //return new 
                            //ToLog("Капча " + error);
                        }
                        else
                        {
                            string email2 = parsed["email"];
                            string link = parsed["body"];
                            //ToLog(email2 + ";" + pass + ";" + name + " " + surname);
                            //ToLog("Все ОК");
                            return email2 + ";" + pass + ";" + name + ";" + surname + ";" + day + ";" + month +";" + year + ";" + sex + ";" + link;
                            //url = body;
                            // Заходим на страницу и проверяем успешность
                        }
                    }
                }
                return "Все ок";
            }
            else
            {
                throw new System.Exception("Капча не была отправлена!");
            }
        }

        // POST-запрос
        private string POST(string url, string parametrs, CookieContainer Cook)
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

        private void Clear()
        {

        }
    }
}
