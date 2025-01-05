using Microsoft.ProgramSynthesis.Utils.Interactive;
using OmniGenerator.Lib.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Tools
{
    public static class ConsoleWriter
    {
        private static BlockingCollection<string> _queue = new BlockingCollection<string>();

        static ConsoleWriter()
        {
            var thread = new Thread(() =>
            {
                while (true)
                {
                    var item = _queue.Take();
                    Console.WriteLine(item);
                }
            });
            thread.IsBackground = true;
            thread.Start();
        }

        public static void WriteLine(string value)
        {
            _queue.Add(value);
        }

        public static void WriteLine(IProgressReport report)
        {
            Console.Clear();
            WriteLine(string.Empty);
            report.GetTextReport().ForEach(x => WriteLine(x));
        }
    }
}
