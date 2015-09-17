using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis;
using Aegis.Data.MySql;



namespace Server.Services
{
    public class Membership
    {
        public static Membership Instance { get { return Singleton<Membership>.Instance; } }





        private Membership()
        {
        }


        public static String GeneratePasswordHash(String source)
        {
            String saltKey = "Indie+API@Salt#";
            Int32 count;


            source += saltKey;
            count = source.Length % 5;

            for (int i = 0; i < count; ++i)
                source = MD5Hash(source);

            return source;
        }


        private static String MD5Hash(String source)
        {
            StringBuilder result = new StringBuilder();
            byte[] bytePassword = Encoding.ASCII.GetBytes(source);
            byte[] byteHash = (new System.Security.Cryptography.MD5CryptoServiceProvider()).ComputeHash(bytePassword);


            for (int i = 0; i < byteHash.Length; ++i)
                result.Append(byteHash[i].ToString("X2"));

            return result.ToString();
        }


        public void RegisterGuest(String udid, Action<Int32> onComplete)
        {
            using (DBCommand cmd = GameDB.NewCommand())
            {
                cmd.CommandText.Append("call sp_auth_register_guest(@udid);");
                cmd.BindParameter("@udid", udid);
                cmd.PostQuery((reader) =>
                {
                    reader.Read();
                    Int32 ret = reader.GetInt32(0);
                    if (ret == 0)
                        onComplete(ResultCode.Ok);

                    else if (ret == 1)
                        onComplete(ResultCode.AlreadyExistsUDID);
                });
            }
        }


        public void RegisterMember(String udid, String userId, String userPwd, Action<Int32> onComplete)
        {
            using (DBCommand cmd = GameDB.NewCommand())
            {
                String passwordHash = GeneratePasswordHash(userPwd);

                cmd.CommandText.Append("call sp_auth_register_member(@udid, @userid, @pwd);");
                cmd.BindParameter("@udid", udid);
                cmd.BindParameter("@userId", userId);
                cmd.BindParameter("@pwd", passwordHash);
                cmd.PostQuery((reader) =>
                {
                    reader.Read();
                    Int32 ret = reader.GetInt32(0);
                    if (ret == 0)
                        onComplete(ResultCode.Ok);

                    else if (ret == 1)
                        onComplete(ResultCode.AlreadyExistsUDID);

                    else if (ret == 2)
                        onComplete(ResultCode.AlreadyExistsUserId);
                });
            }
        }


        public void LoginGuest(String udid, Action<Int32, Int32> onComplete)
        {
            using (DBCommand cmd = GameDB.NewCommand())
            {
                cmd.CommandText.Append("select userno from t_accounts where udid=@udid and isguest=1;");
                cmd.BindParameter("@udid", udid);
                cmd.PostQuery((reader) =>
                {
                    if (reader.Read() == true)
                    {
                        Int32 userNo = reader.GetInt32(0);
                        onComplete(ResultCode.Ok, userNo);
                    }
                    else
                        onComplete(ResultCode.InvalidUDID, 0);
                });
            }
        }


        public void LoginMember(String udid, String userId, String userPwd, Action<Int32, Int32> onComplete)
        {
            using (DBCommand cmd = GameDB.NewCommand())
            {
                String passwordHash = GeneratePasswordHash(userPwd);


                cmd.CommandText.Append("select userno, udid from t_accounts where userid=@userid and userpwd=@userpwd and isguest=0;");
                cmd.BindParameter("@userid", userId);
                cmd.BindParameter("@userpwd", passwordHash);
                cmd.PostQuery((reader) =>
                {
                    if (reader.Read() == true)
                    {
                        Int32 dbUserNo = reader.GetInt32(0);
                        String dbUdid = reader.GetString(1);


                        if (dbUdid != udid)
                            onComplete(ResultCode.InvalidUDID, 0);
                        else
                            onComplete(ResultCode.Ok, dbUserNo);
                    }
                    else
                        onComplete(ResultCode.InvalidUserId, 0);
                });
            }
        }
    }
}
