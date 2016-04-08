using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using CivilApp;
using System.Web.Security;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace CivilApp.Models
{
    public class CivilAppMemberShipProvider : MembershipProvider
    {
        private MachineKeySection machineKey;

        CivilAppEntities _Entity = new CivilAppEntities();

        public override string ApplicationName
        {
            get
            {
                return "Civil App (CivilApp)";
            }
            set
            {
                ApplicationName = value;
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {

            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(oldPassword) &&
                string.IsNullOrWhiteSpace(newPassword))
            {
                var user = _Entity
                    .Usuarios
                    .Where(users => users.UserLogIn == username
                        && EncryptText(oldPassword) == users.Password).FirstOrDefault();


                if (user != null)
                {
                    user.Password = EncryptText(newPassword);

                    _Entity.SaveChanges();
                    return true;
                }
                return false;

            }
            return false;

        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            var user = _Entity.Usuarios.Where(a => a.UserLogIn == username).FirstOrDefault();

            MembershipUser member = new MembershipUser(username, user.Nombre, null, user.Email, "",
                        "", true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);
            status = MembershipCreateStatus.Success;

            return member;
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData = false)
        {
            var user = _Entity.Usuarios.Where(a => a.UserLogIn == username).FirstOrDefault();

            _Entity.Usuarios.Remove(user);
            _Entity.SaveChanges();

            return true;
        }

        public override bool EnablePasswordReset
        {
            get { return true; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return true; }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer = "")
        {
            if (!string.IsNullOrWhiteSpace(username))
            {
                var user = _Entity.Usuarios.Where(a => a.UserLogIn == username).FirstOrDefault();
                if (user != null)
                {
                    return EncryptText(user.Password);
                }
            }

            return "";
        }

        public override MembershipUser GetUser(string username, bool userIsOnline = true)
        {
            if (!string.IsNullOrWhiteSpace(username))
            {
                var user = _Entity.Usuarios.Where(a => a.UserLogIn == username).FirstOrDefault();

                if (user != null)
                {
                    MembershipUser member = new MembershipUser(user.UserLogIn, user.Nombre, null, user.Email, "",
                        "", true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);

                    return member;
                }
            }

            return null;

        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                var user = _Entity.Usuarios.Where(a => a.Email == email).FirstOrDefault();

                if (user != null)
                {
                    MembershipUser member = new MembershipUser(user.UserLogIn, user.Nombre, null, user.Email, "",
                        "", true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);

                    return member.ProviderName;
                }




            }

            return null;
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return 3; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return 1; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return 6; }
        }

        public override int PasswordAttemptWindow
        {
            get { return 1; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return MembershipPasswordFormat.Encrypted; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            var user = _Entity.Usuarios.Where(a => a.UserLogIn == userName).FirstOrDefault();
            user.EstadoUsuarioID = 2;
            return false;
        }

        public override void UpdateUser(MembershipUser user)
        {//Implementar este metodo
            var users = _Entity.Usuarios.Where(a => a.UserLogIn == user.UserName).FirstOrDefault();

        }

        public override bool ValidateUser(string username, string password)
        {
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                var EncryptPassword = EncryptText(password);
                var user = _Entity.Usuarios
                    .Where(a => a.UserLogIn == username &&
                       a.Password == EncryptPassword
                        && a.EstadoUsuarioID != 2)
                    .FirstOrDefault();

                if (user != null)
                {
                    return true;
                }

            }

            return false;

        }



        public string EncryptText(string clearText)
        {
            string EncryptionKey = "abc123";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string DecryptText(string cipherText)
        {
            string EncryptionKey = "abc123";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        private byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        public string GetUserRealyName(string userName)
        {
            var user = _Entity.Usuarios.Where(a => a.UserLogIn == userName).FirstOrDefault();

            return user.Nombre;

        }

        public int UserAuthenticateNow(string UserName)
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                var user = _Entity
                    .Usuarios
                    .Where(a => a.UserLogIn == UserName)
                    .FirstOrDefault();

                return user.UsuarioID;
            }

            return 0;

        }

        public int GetUserByLogName(string UserName)
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                var user = _Entity
                    .Usuarios
                    .Where(a => a.Nombre == UserName)
                    .FirstOrDefault();

                return user.UsuarioID;
            }

            return 0;

        }
    }
}