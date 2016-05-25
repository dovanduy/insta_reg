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
            string key = "c84e9c641ae9112f5eae64b03110b186";
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
                    ToLog("Неверная капча");
                }
                else
                {
                    ToLog("Все ОК");
                }

                //https://c.mail.ru/6?r=0.007272182020318985
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
            long capid = 0;
            string img_url = "https://c.mail.ru/6?r=0.007272182020318985";
            string ans = Upload(img_url, "jpg", out capid);

            if (!ans.StartsWith("ERROR") && answer != "NO_DATA")
            {
                int tries = 0;
                DateTime capStarted = DateTime.Now;
                while (/*AppStarted && Started &&*/ ans != "" && capid > 0 && tries < 150)
                {
                    tries++;
                    System.TimeSpan diffResult = DateTime.Now.Subtract(capStarted);
                    if (diffResult.Seconds > 60)
                    {
                        break;
                    }
                    ans = get(capid);
                    if (ans == "CAPCHA_NOT_READY")
                        Thread.Sleep(500);
                    else
                        break;
                }
            }
            /*
            WebClient client = new WebClient();
            Uri uri = new Uri("http://media-cache-ec0.pinimg.com/736x/3a/17/bf/3a17bfc0ca13db827e72a4da16abe111.jpg");
            client.DownloadFileAsync(uri, "C:/Users/Екатерина/Documents/Git/picture.jpg");
            ToLog("Скачали картинку");
            */

            /*
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
            }*/


            //ToLog(Translit.Front("Антон Попов"));
            /*
            string json_resp = "{\"Tracks\":[{\"Artist\":\"Artist1\",\"Album\":\"Album1\",\"Title\":\"Title1\",\"Year\":\"2015\"}]}";
            ToLog(json_resp);
            var js = JsonConvert.DeserializeObject<Json>(json_resp);
            ToLog(js.body.login.value);*/
            //ToLog(Json.Login);

        }

        public static string MultiFormData(string Key, string Value, string Boundary)
        {
            string output = "--" + Boundary + "\r\n"; output += "Content-Disposition: form-data; name=\"" + Key + "\"\r\n\r\n";
            output += Value + "\r\n";
            return output;
        }

        public static string MultiFormDataFile(string Key, string Value, string FileName, string FileType, string Boundary)
        {
            string output = "--" + Boundary + "\r\n";
            output += "Content-Disposition: form-data; name=\"" + Key + "\"; filename=\"" + FileName + "\"\r\n";
            output += "Content-Type: " + FileType + " \r\n\r\n";
            output += Value + "\r\n";
            return output;
        }

        public static System.Drawing.Imaging.ImageFormat DetectImageFormat(string fmt)
        {
            switch (fmt.ToLower())
            {
                case "image/jpg": return System.Drawing.Imaging.ImageFormat.Jpeg;
                case "image/jpeg": return System.Drawing.Imaging.ImageFormat.Jpeg;
                case "image/gif": return System.Drawing.Imaging.ImageFormat.Gif;
                case "image/png": return System.Drawing.Imaging.ImageFormat.Png;
                case "image/bmp": return System.Drawing.Imaging.ImageFormat.Bmp;
                case "image/tiff": return System.Drawing.Imaging.ImageFormat.Jpeg;
                case "jpg": return System.Drawing.Imaging.ImageFormat.Jpeg;
                case "jpeg": return System.Drawing.Imaging.ImageFormat.Jpeg;
                case "gif": return System.Drawing.Imaging.ImageFormat.Gif;
                case "png": return System.Drawing.Imaging.ImageFormat.Png;
                case "bmp": return System.Drawing.Imaging.ImageFormat.Bmp;
                case "tiff": return System.Drawing.Imaging.ImageFormat.Jpeg;
                default: return null;
            }
        }

        public static byte[] imageToBytes(Image imageIn, System.Drawing.Imaging.ImageFormat imgFormat)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, imgFormat);
            return ms.ToArray();
        }

        public string Upload(string url, string cap_ext, out long id)
        {
            id = -1;
            ua = browser("http://anti-captcha.net/in.aspx");
            ua.Method = "POST";
            ua.Timeout = 60000;
            ua.ReadWriteTimeout = 60000;

            string sBoundary = DateTime.Now.Ticks.ToString("x");
            ua.ContentType = "multipart/form-data; boundary=" + sBoundary;
            string sPostMultiString = "";
            sPostMultiString += MultiFormData("method", "post", sBoundary);
            sPostMultiString += MultiFormData("key", this.key, sBoundary);
            sPostMultiString += MultiFormData("softkey", "c06b73466", sBoundary);
            sPostMultiString += MultiFormData("file", url, sBoundary);
            sPostMultiString += MultiFormData("is_russian", "1", sBoundary);
            sPostMultiString += MultiFormData("phrase", "0", sBoundary);
            sPostMultiString += MultiFormData("regsense", "1", sBoundary);
            sPostMultiString += MultiFormData("numeric", "0", sBoundary);
            sPostMultiString += MultiFormData("calc", "0", sBoundary);
            sPostMultiString += MultiFormData("min_len", "4", sBoundary);
            sPostMultiString += MultiFormData("max_len", "30", sBoundary);

            string sFileContent = "";

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.AllowAutoRedirect = true;
            req.Method = "GET";
            //WebProxy myProxy = new WebProxy("");
            //req.Proxy = myProxy;
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 1.1.4322; .NET CLR 2.0.50727; .NET CLR 3.0.04506.590; .NET CLR 3.5.20706)";
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();

            Image image = System.Drawing.Image.FromStream(stream);

            byte[] bi = imageToBytes(image, DetectImageFormat(cap_ext));
            sFileContent = Encoding.Default.GetString(bi);

            stream.Close();

            string filename = "captcha." + cap_ext;
            sPostMultiString += MultiFormDataFile("file", sFileContent, filename, "image/" + cap_ext, sBoundary);
            sPostMultiString += "--" + sBoundary + "--\r\n\r\n";
            byte[] byteArray = Encoding.Default.GetBytes(sPostMultiString);
            ua.ContentLength = byteArray.Length;
            try
            {
                ua.GetRequestStream().Write(byteArray, 0, byteArray.Length);
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)ua.GetResponse();
                StreamReader myStreamReadermy = new StreamReader(myHttpWebResponse.GetResponseStream(), Encoding.GetEncoding(1251));
                result_page = myStreamReadermy.ReadToEnd();
                //result_headers = myHttpWebResponse.Headers.ToString();

                string[] pars = result_page.Split(new char[1] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                if (pars[0] == "OK")
                    id = Convert.ToInt64(pars[1]);
                return result_page;
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }

        public string get(long cap_id)
        {
            if (this.req("http://anti-captcha.net/res.aspx?key=" + key + "&action=get&id=" + cap_id + "&"))
            {
                try
                {
                    string[] result = result_page.Split(new char[1] { '|' });
                    return result[1];
                }
                catch
                {
                    return result_page;
                }
            }
            return "";
        }

        public string get_balance(string key)
        {
            if (this.req("http://anti-captcha.net/res.aspx?key=" + key + "&action=getbalance"))
            {
                return result_page;
            }
            return "-1";
        }

        public bool reportbad(long cap_id)
        {
            if (this.req("http://anti-captcha.net/res.aspx?key=" + key + "&action=reportbad&id=" + cap_id))
            {
                if (result_page.IndexOf("ERROR") >= 0)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        private bool req(string url)
        {
            ua = browser(url);
            ua.Method = "GET";
            ua.KeepAlive = true;
            try
            {
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)ua.GetResponse();
                StreamReader myStreamReadermy = new StreamReader(myHttpWebResponse.GetResponseStream(), Encoding.GetEncoding(1251));
                result_page = myStreamReadermy.ReadToEnd();
                result_headers = myHttpWebResponse.Headers.ToString();
                return true;
            }
            catch (WebException ex)
            {
                //Console.WriteLine(ex);
                return false;
            }
        }


    }
}
