using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace insta_reg
{
    class CurSet
    {
        public string file_names;
        public string file_surnames;

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
                using (StreamReader sr = new StreamReader(fs, Encoding.Default))
                {
                    string temp = string.Empty;
                    list.Add(temp);
                }
                fs.Close();

                return list;
            }
            catch
            {
                throw new System.Exception("Не получилось открыть файл");
            }
        }
    }
}
