using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis;
using Aegis.Data.MySql;



namespace IndieAPI.Server.Services
{
    public class Membership
    {
        public static Membership Instance { get { return Singleton<Membership>.Instance; } }





        private Membership()
        {
        }


        public static String GeneratePasswordHash(String source)
        {
            Int32 count, i;

            source = String.Format("Indie+API@{0}#salt_", source);
            count = source.Length % 5 + 1;
            for (i = 0; i < count; ++i)
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


        public void RegisterGuest(String uuid, Action<Int32> onComplete)
        {
            using (DBCommand cmd = GameDB.NewCommand())
            {
                cmd.CommandText.Append("call sp_auth_register_guest(@uuid);");
                cmd.BindParameter("@uuid", uuid);
                cmd.PostQuery(() =>
                {
                    cmd.Reader.Read();
                    Int32 ret = cmd.Reader.GetInt32(0);
                    if (ret == 0)
                        onComplete(ResultCode.Ok);

                    else if (ret == 1)
                        onComplete(ResultCode.AlreadyExistsUUID);
                });
            }
        }


        public void RegisterMember(String uuid, String userId, String userPwd, Action<Int32> onComplete)
        {
            using (DBCommand cmd = GameDB.NewCommand())
            {
                String passwordHash = GeneratePasswordHash(userPwd);

                cmd.CommandText.Append("call sp_auth_register_member(@uuid, @userid, @pwd);");
                cmd.BindParameter("@uuid", uuid);
                cmd.BindParameter("@userId", userId);
                cmd.BindParameter("@pwd", passwordHash);
                cmd.PostQuery(() =>
                {
                    cmd.Reader.Read();
                    Int32 ret = cmd.Reader.GetInt32(0);
                    if (ret == 0)
                        onComplete(ResultCode.Ok);

                    else if (ret == 1)
                        onComplete(ResultCode.AlreadyExistsUUID);

                    else if (ret == 2)
                        onComplete(ResultCode.AlreadyExistsUserId);
                });
            }
        }


        public void LoginGuest(String uuid, Action<Int32, Int32> onComplete)
        {
            using (DBCommand cmd = GameDB.NewCommand())
            {
                cmd.CommandText.Append("select userno from t_accounts where uuid=@uuid and isguest=1;");
                cmd.BindParameter("@uuid", uuid);
                cmd.PostQuery(() =>
                {
                    if (cmd.Reader.Read() == true)
                    {
                        Int32 userNo = cmd.Reader.GetInt32(0);
                        onComplete(ResultCode.Ok, userNo);
                    }
                    else
                        onComplete(ResultCode.InvalidUUID, 0);
                });
            }
        }


        public void LoginMember(String uuid, String userId, String userPwd, Action<Int32, Int32> onComplete)
        {
            using (DBCommand cmd = GameDB.NewCommand())
            {
                String passwordHash = GeneratePasswordHash(userPwd);


                cmd.CommandText.Append("select userno, uuid from t_accounts where userid=@userid and userpwd=@userpwd and isguest=0;");
                cmd.BindParameter("@userid", userId);
                cmd.BindParameter("@userpwd", passwordHash);
                cmd.PostQuery(() =>
                {
                    if (cmd.Reader.Read() == true)
                    {
                        Int32 dbUserNo = cmd.Reader.GetInt32(0);
                        String dbUUID = cmd.Reader.GetString(1);


                        if (dbUUID != uuid)
                            onComplete(ResultCode.InvalidUUID, 0);
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
