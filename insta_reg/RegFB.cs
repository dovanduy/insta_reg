using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace insta_reg
{
    class RegFB
    {
        public string Reger(string name, string surname, string email, string pass, string birthday, string sex)
        {
            string parametrs = "";
            string post = "";
            CookieContainer Cook = new CookieContainer();
            string[] b = birthday.Split('.');
            string day = b[0];
            string month = b[1];
            string year = b[2];
            if (sex == "male")
                sex = "2";
            //
            // Заходим на сайт
            string url = "https://m.facebook.com/";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, image/jxr, */*";
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "ru-RU");
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2486.0 Safari/537.36 Edge/13.10586";
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            request.Host = "m.facebook.com";
            request.KeepAlive = true;
            request.CookieContainer = Cook;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            response.Close();
            reader.Close();

            // Заходим на страницу регистрации
            //url = "https://m.facebook.com/reg";
            url = "https://m.facebook.com/reg/?cid=103&refid=8";
            request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, image/jxr, */*";
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "ru-RU");
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2486.0 Safari/537.36 Edge/13.10586";
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            request.Host = "m.facebook.com";
            request.Referer = "https://m.facebook.com/";
            request.KeepAlive = true;
            request.CookieContainer = Cook;
            response = (HttpWebResponse)request.GetResponse();
            reader = new StreamReader(response.GetResponseStream());
            response.Close();
            reader.Close();

            // Регаемся
            //parametrs = "lsd=" + HttpUtility.UrlEncode("AVqZ-udP");
            //parametrs += "&charset_test=" + HttpUtility.UrlEncode("ˆ,?,ˆ,?,?,Ä,ª");
            parametrs += "&ccp=" + HttpUtility.UrlEncode("4");

            //parametrs += "&reg_instance=" + HttpUtility.UrlEncode("ohpMVwITluRhy4vgUWJRiKo4");
            parametrs += "&submission_request=" + HttpUtility.UrlEncode("true");
            parametrs += "&helper=" + HttpUtility.UrlEncode("");
            parametrs += "&firstname=" + HttpUtility.UrlEncode(name);
            parametrs += "&lastname=" + HttpUtility.UrlEncode(surname);
            parametrs += "&reg_email__=" + HttpUtility.UrlEncode(email);
            parametrs += "&sex=" + HttpUtility.UrlEncode(sex);
            parametrs += "&birthday_day=" + HttpUtility.UrlEncode(day);
            parametrs += "&birthday_month=" + HttpUtility.UrlEncode(month);
            parametrs += "&birthday_year=" + HttpUtility.UrlEncode(year);
            parametrs += "&reg_passwd__=" + HttpUtility.UrlEncode(pass);
            parametrs += "&submit=" + HttpUtility.UrlEncode("Регистрация");
            post = POST("https://m.facebook.com/reg/?cid=103", parametrs, Cook);

            return "";
        }

        // POST-запрос
        private string POST(string url, string parametrs, CookieContainer Cook)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Accept = "text/html, application/xhtml+xml, image/jxr, */*";
            //request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Referer = "https://m.facebook.com/reg/?cid=103&refid=8";
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "ru-RU");
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2486.0 Safari/537.36 Edge/13.10586";
            request.Host = "m.facebook.com";
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

    }
}
