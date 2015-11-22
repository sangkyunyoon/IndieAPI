using System;
using System.Windows.Forms;
using System.Reflection;



namespace TestClient.WinFormHelper
{
    public static class UIViews
    {
        private static Form _parentForm, _curForm;
        private static Panel _panel;





        public static void Initialize(Form mainForm, Panel panel)
        {
            _parentForm = mainForm;
            _panel = panel;
        }


        public static void ChangeView<T>() where T : Form
        {
            if (_curForm != null)
            {
                _parentForm.Controls.Remove(_curForm);

                _curForm.Parent = null;
                _curForm.Hide();
                _curForm.GetType().GetMethod("OnViewLeaved")?.Invoke(_curForm, null);
                _curForm = null;
            }


            {
                ConstructorInfo constructor = typeof(T).GetConstructor(
                                                            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                                                            , null, new Type[0], null);
                if (constructor != null)
                    _curForm = (T)constructor.Invoke(null);
                else
                    throw new Exception(String.Format("No matches constructor on {0}.", typeof(T).Name));
            }


            {
                _curForm.Text = "";
                _curForm.TopLevel = false;
                _curForm.ControlBox = false;
                _curForm.FormBorderStyle = FormBorderStyle.None;
                _parentForm.Controls.Add(_curForm);

                _curForm.Parent = _panel;
                _curForm.Show();
                _curForm.GetType().GetMethod("OnViewEntered")?.Invoke(_curForm, null);
            }
        }
    }
}
