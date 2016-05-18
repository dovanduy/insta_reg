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

            //1 этап
            string url1 = "https://touch.mail.ru/cgi-bin/signup";
            HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(url1);
            HttpWebResponse response1 = (HttpWebResponse)request1.GetResponse();
            StreamReader reader1 = new StreamReader(response1.GetResponseStream());
            //tB_Log.Text = reader1.ReadToEnd();
            string cookies = String.IsNullOrEmpty(response1.Headers["Set-Cookie"]) ? "" : response1.Headers["Set-Cookie"];
            tB_Log.Text = cookies;

            //2 этап
            //string url = "https://touch.mail.ru/api/v1/user/exists";
            string url = "https://touch.mail.ru/cgi-bin/signup";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Host = "touch.mail.ru";
            request.KeepAlive=false;
            request.Accept = "*/*";
            //request.Headers.Add(HttpRequestHeader.ori);
            //origin
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Referer = "https://touch.mail.ru/cgi-bin/signup";
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4");
            request.Headers.Add(HttpRequestHeader.Cookie, cookies);
            request.ServicePoint.Expect100Continue = false;
            //post-данные
            request.Method = "POST";
            var postData = "login=fjdsdkfjd";
            postData += "&email=fjdsdkfjd@mail.ru";
            postData += "&domain=mail.ru";
            postData += "&sex=male";
            postData += "&birthday={\"year\":\"1994\",\"month\":\"10\",\"day\":\"20\"}";
            postData += "&name={\"first\":\"Антон\",\"last\":\"Попов\"}";
            HttpUtility.
            HttpUtility.UrlEncode(postData);
            var data = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            //UTF8Encoding encoding = new UTF8Encoding();


            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            //tB_Log.Text = reader.ReadToEnd();
            //MessageBox.Show(reader.ReadToEnd());
            ToLog("ok");

        }

        // Вывод сообщения в лог
        private void ToLog(string text)
        {
            tB_Log.AppendText(text+"\n");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url = "https://touch.mail.ru/cgi-bin/signup";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            tB_Log.Text = reader.ReadToEnd();

        }
    }
}
