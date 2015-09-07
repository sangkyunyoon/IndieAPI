using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis.Data.MySql;



namespace Server.Services.UserData
{
    public class TextBox
    {
        private User _parentUser;
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
            _parentUser = user;
        }


        public void LoadFromDB()
        {
            using (DBCommand cmd = SyncZoneDB.NewCommand())
            {
                cmd.CommandText.Append($"select textdata from t_textbox where userno={_parentUser.UserNo};");


                DataReader reader = cmd.Query();
                if (reader.Read())
                    _textData = reader.GetString(0);
                else
                    _textData = "";
            }
        }


        private void UpdateToDB()
        {
            using (DBCommand cmd = SyncZoneDB.NewCommand())
            {
                cmd.CommandText.Append("insert into t_textbox(userno, textdata) values(@userno, @data)");
                cmd.CommandText.Append(" on duplicate key update textdata=@data;");
                cmd.BindParameter("@userno", _parentUser.UserNo);
                cmd.BindParameter("@data", _textData);
                cmd.PostQuery();
            }
        }
    }
}
