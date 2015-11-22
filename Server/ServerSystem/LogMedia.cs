using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Aegis;



namespace IndieAPI.Server.Routine
{
    public static class LogMedia
    {
        private static TextBox _textBox;
        private static StreamWriter _textFile;





        public static void SetTextBoxLogger(TextBox tb)
        {
            if (tb == null)
                return;

            _textBox = tb;
            Logger.Written += TextBoxLog;
        }


        public static void SetTextFileLogger(String path, String filePrefix)
        {
            if (path == null)
                return;

            if (Directory.Exists(".\\log") == false)
                Directory.CreateDirectory(".\\log");

            String filename = String.Format(".\\log\\{0}_{1}_{2:D2}{3:D2}_{4:D2}{5:D2}.log",
                                            filePrefix,
                                            DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                            DateTime.Now.Hour, DateTime.Now.Minute);
            _textFile = new StreamWriter(filename);
            Logger.Written += TextFileLog;
        }


        public static void DeleteAllLogger()
        {
            Logger.Written -= TextBoxLog;
            Logger.Written -= TextFileLog;

            if (_textFile != null)
            {
                _textFile.Close();
                _textFile = null;
            }
        }


        private static void TextBoxLog(LogType type, Int32 level, String log)
        {
            if (_textBox.InvokeRequired)
                _textBox.BeginInvoke((MethodInvoker)delegate { TextBoxLog(type, level, log); });
            else
            {
                String message = String.Format("[{0}, {1}] {2}\r\n", type, level, log);

                _textBox.Text += message;
                _textBox.SelectionStart = _textBox.TextLength;
                _textBox.ScrollToCaret();
            }
        }


        private static void TextFileLog(LogType type, Int32 level, String log)
        {
            String text = String.Format("[{0}/{1} {2}:{3}:{4} {5}] {6}",
                                        DateTime.Now.Month, DateTime.Now.Day,
                                        DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second,
                                        type, log);

            _textFile.WriteLine(text);
            _textFile.Flush();
        }
    }
}
