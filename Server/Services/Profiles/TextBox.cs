using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis.Data.MySql;
using IndieAPI.Server.UserManagement;



namespace IndieAPI.Server.Services.Profiles
{
    public class TextBox
    {
        private User _user;
        private String _textData;


        public String TextData
        {
            get { return _textData; }
            set
            {
                _textData = value;
                UpdateToDB();
            }
        }





        public TextBox(User user)
        {
            _user = user;
        }


        public void LoadFromDB(DataReader reader)
        {
            _textData = "";
            if (reader.Read())
                _textData = reader.GetString(0);
        }


        private void UpdateToDB()
        {
            using (DBCommand cmd = GameDB.NewCommand())
            {
                cmd.CommandText.Append("insert into t_textbox(userno, textdata) values(@userno, @data)");
                cmd.CommandText.Append(" on duplicate key update textdata=@data;");
                cmd.BindParameter("@userno", _user.UserNo);
                cmd.BindParameter("@data", _textData);
                cmd.PostQueryNoReader();
            }
        }
    }
}
