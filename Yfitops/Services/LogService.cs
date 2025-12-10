using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yfitops.Services
{
    public static class LogService
    {
        private static readonly string FilePath = "app.log";

        public static void Write(string message)
        {
            File.AppendAllText(FilePath, $"{DateTime.Now}: {message}\n");
        }
    }

}
