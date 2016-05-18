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
            /*
            int CountMail = Convert.ToInt32(nUD_CountMail.Value);
            ToLog(Convert.ToString(CountMail));
            */

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
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            //tB_Log.Text = reader1.ReadToEnd();
            string cookies = String.IsNullOrEmpty(response.Headers["Set-Cookie"]) ? "" : response.Headers["Set-Cookie"];
            tB_Log.Text = cookies;

            //2- отправляю данные
            
            url = "https://touch.mail.ru/cgi-bin/signup";
            request = (HttpWebRequest)WebRequest.Create(url);
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
            //request.CookieContainer = new CookieContainer(); //инициализируем контейнер
            //request.CookieContainer.Add(ckCol); //добавляем наши куки

            string parameters = "name=ant";
            //string parameters = "name={\"first\":\"Антон\",\"last\":\"Попов\"}";
            /*
            
            string postParameters = HttpUtility.UrlPathEncode(parameters);
            ToLog(postParameters);
            request.ContentLength = postParameters.Length;
            using (var writer = new StreamWriter(request.GetRequestStream(), Encoding.UTF8))
            {
                writer.Write(postParameters);
            }*/


            byte[] postData = Encoding.UTF8.GetBytes(HttpUtility.UrlEncode(parameters));
            request.ContentLength = postData.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(postData, 0, postData.Length);
            }
        }

        // Вывод сообщения в лог
        private void ToLog(string text)
        {
            tB_Log.AppendText(text+"\n");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string parameters = "{\"first\":\"Антон\",\"last\":\"Попов\"}";
            byte[] postData = Encoding.UTF8.GetBytes(HttpUtility.UrlEncode(parameters));
            ToLog(Convert.ToString(postData));
            /*
            request.ContentLength = postData.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(postData, 0, postData.Length);
            }
            */

        }
    }
}
