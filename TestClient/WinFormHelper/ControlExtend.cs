using System;
using System.Windows.Forms;
using System.Drawing;



namespace TestClient.WinFormHelper
{
    public static class ControlExtend
    {
        public static void PerformOnMainThread(this Control control, Action action)
        {
            if (control.InvokeRequired)
                control.BeginInvoke(action);
            else
                action();
        }


        public static void AddLine(this TextBox textBox, String format, params object[] args)
        {
            PerformOnMainThread(textBox, () =>
            {
                textBox.Text += String.Format(format, args);
                textBox.SelectionStart += textBox.TextLength;
                textBox.ScrollToCaret();
            });
        }


        public static void AddString(this RichTextBox textBox, Color foreColor, String format, params object[] args)
        {
            PerformOnMainThread(textBox, () =>
            {
                Int32 pos = textBox.TextLength;

                textBox.AppendText(String.Format(format, args));
                textBox.Select(pos, textBox.TextLength);
                textBox.SelectionColor = foreColor;
            });
        }


        public static void AddLine(this RichTextBox textBox, Color foreColor, String format, params object[] args)
        {
            PerformOnMainThread(textBox, () =>
            {
                Int32 pos = textBox.TextLength;

                textBox.AppendText(String.Format(format, args) + "\r\n");
                textBox.Select(pos, textBox.TextLength);
                textBox.SelectionColor = foreColor;
                textBox.ScrollToCaret();
            });
        }


        public static System.Windows.Forms.Timer CreateTimer(this Form form, Int32 interval, Action action)
        {
            var timer = new System.Windows.Forms.Timer();
            timer.Interval = interval;
            timer.Tick += delegate (object sender, EventArgs e)
            {
                if (action != null)
                    action();
            };
            return timer;
        }
    }
}
