using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EffectiveMobile
{
    internal class MySimpleLogger
    {
        public const string DEFAULT_DELIVERY_LOG = "./log/log.txt";
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
                instance = new MySimpleLogger(DEFAULT_DELIVERY_LOG);
            return instance;
        }

        public static void CreateInstance(string file)
        {
            if (instance == null)
                instance = new MySimpleLogger(file);
        }

        public void Log(string message)
        {
            try { File.AppendAllLines(_file, new List<string>() { message }); }
            catch { }
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
