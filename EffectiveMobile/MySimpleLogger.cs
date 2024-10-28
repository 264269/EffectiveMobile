using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EffectiveMobile
{
    internal class MySimpleLogger
    {
        private static MySimpleLogger instance;
        private string _file;

        private MySimpleLogger(string file)
        {
            _file = file;
            CreateFileIfNecessary();
        }

        public static MySimpleLogger GetInstance()
        {
            if (instance == null)
                CreateInstance("./log.txt");
            return instance;
        }

        public static void CreateInstance(string file)
        {
            if (instance == null)
                instance = new MySimpleLogger(file);
        }

        public void Log(string message)
        {
            File.AppendAllLines(_file, new List<string>() { message });
        }

        public void CreateFileIfNecessary()
        {
            if (File.Exists(_file))
                return;

            string? dir = Path.GetDirectoryName(_file);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);
        }
    }
}
