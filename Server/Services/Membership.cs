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
                Int32 result = ResultCode.Ok;


                cmd.CommandText.Append("call sp_auth_register_guest(@uuid);");
                cmd.BindParameter("@uuid", uuid);
                cmd.PostQuery(
                    () =>
                    {
                        if (cmd.Reader.Read() == true)
                        {
                            Int32 ret = cmd.Reader.GetInt32(0);
                            if (ret == 0)
                                result = ResultCode.Ok;

                            else if (ret == 1)
                                result = ResultCode.AlreadyExistsUUID;
                        }
                        else
                            result = ResultCode.UnknownError;
                    },
                    (e) =>
                    {
                        if (e != null)
                            onComplete(ResultCode.UnknownError);
                        else
                            onComplete(result);
                    });
            }
        }


        public void RegisterMember(String uuid, String userId, String userPwd, Action<Int32> onComplete)
        {
            using (DBCommand cmd = GameDB.NewCommand())
            {
                Int32 result = ResultCode.Ok;
                String passwordHash = GeneratePasswordHash(userPwd);

                cmd.CommandText.Append("call sp_auth_register_member(@uuid, @userid, @pwd);");
                cmd.BindParameter("@uuid", uuid);
                cmd.BindParameter("@userId", userId);
                cmd.BindParameter("@pwd", passwordHash);
                cmd.PostQuery(
                    (e) =>
                    {
                        if (cmd.Reader.Read() == true)
                        {
                            Int32 ret = cmd.Reader.GetInt32(0);
                            if (ret == 0)
                                result = ResultCode.Ok;

                            else if (ret == 1)
                                result = ResultCode.AlreadyExistsUUID;

                            else if (ret == 2)
                                result = ResultCode.AlreadyExistsUserId;
                        }
                        else
                            result = ResultCode.UnknownError;
                    },
                    (e) =>
                    {
                        if (e != null)
                            onComplete(ResultCode.UnknownError);
                        else
                            onComplete(result);
                    });
            }
        }


        public void LoginGuest(String uuid, Action<Int32, Int32> onComplete)
        {
            using (DBCommand cmd = GameDB.NewCommand())
            {
                Int32 result = ResultCode.Ok;
                Int32 userNo = 0;


                cmd.CommandText.Append("select userno from t_accounts where uuid=@uuid and isguest=1;");
                cmd.BindParameter("@uuid", uuid);
                cmd.PostQuery(
                    (e) =>
                    {
                        if (cmd.Reader.Read() == true)
                        {
                            userNo = cmd.Reader.GetInt32(0);
                            result = ResultCode.Ok;
                        }
                        else
                            result = ResultCode.InvalidUUID;
                    },
                    (e) =>
                    {
                        if (e != null)
                            onComplete(ResultCode.UnknownError, 0);
                        else
                            onComplete(result, userNo);
                    });
            }
        }


        public void LoginMember(String uuid, String userId, String userPwd, Action<Int32, Int32> onComplete)
        {
            using (DBCommand cmd = GameDB.NewCommand())
            {
                Int32 result = ResultCode.Ok;
                Int32 userNo = 0;
                String passwordHash = GeneratePasswordHash(userPwd);


                cmd.CommandText.Append("select userno, uuid from t_accounts where userid=@userid and userpwd=@userpwd and isguest=0;");
                cmd.BindParameter("@userid", userId);
                cmd.BindParameter("@userpwd", passwordHash);
                cmd.PostQuery(
                    (e) =>
                    {
                        if (cmd.Reader.Read() == true)
                        {
                            userNo = cmd.Reader.GetInt32(0);
                            String dbUUID = cmd.Reader.GetString(1);


                            if (dbUUID != uuid)
                                result = ResultCode.InvalidUUID;
                            else
                                result = ResultCode.Ok;
                        }
                        else
                            onComplete(ResultCode.InvalidUserId, 0);
                    },
                    (e) =>
                    {
                        if (e != null)
                            onComplete(ResultCode.UnknownError, 0);
                        else
                            onComplete(result, userNo);
                    });
            }
        }
    }
}
