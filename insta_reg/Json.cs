using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace insta_reg
{
    public class RegAnketaCapcha
    {
        public string value { get; set; }
        public string error { get; set; }
    }

    public class Password
    {
        public object value { get; set; }
        public string error { get; set; }
    }

    public class Login
    {
        public string value { get; set; }
        public string error { get; set; }
    }

    public class Body
    {
        public Password password { get; set; }
        public Login login { get; set; }
        public List<string> alternatives { get; set; }
//        [JsonProperty(PropertyName = "reg_anketa.captcha")]
        public RegAnketaCapcha reg_anketa { get; set; }
    }

    public class Json
    {
        public Body body { get; set; }
        public object email { get; set; }
        public int status { get; set; }
        public bool htmlencoded { get; set; }
    }

    public class Json2
    {
        public string body { get; set; }
        public object email { get; set; }
        public int status { get; set; }
        public bool htmlencoded { get; set; }
    }

}
