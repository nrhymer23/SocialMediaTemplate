using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace KSUCorner.Models
{
    public class UserRepository
    {
        public static void CreateUser(string username, string firstname, string lastname,
                                         string email, string password, string gender,
                                         int month, int day, int year,
                                         string securityquestion, string securityanswer)
        {
            using (KSUCornerDBEntities1 db = new KSUCornerDBEntities1())
            {
                Account user = new Account();

                user.UserName = username;
                user.FirstName = firstname;
                user.LastName = lastname;
                user.Email = email;
                user.EmailConfirmed = false;
                user.Password = password;
                user.Gender = gender;
                user.BirthDate = new DateTime(year, month, day);
                user.SecurityQuestion = securityquestion;
                user.SecurityAnswer = securityanswer;
                user.LastLoginDate = DateTime.Now;
                user.LastUpdateDate = DateTime.Now;
                user.IsActivated = false;
                user.IsLockedOut = false;
                user.LastLockedOutDate = DateTime.Now;
                user.LastLockedOutReason = "";
                user.CreateDate = DateTime.Now;

                db.Accounts.Add(user);
                db.SaveChanges();
            }
        }

        public string GetUserNameByEmail(string email)
        {
            using (KSUCornerDBEntities1 db = new KSUCornerDBEntities1())
            {
                var result = from u in db.Accounts where (u.Email == email) select u;

                if (result.Count() != 0)
                {
                    var dbuser = result.FirstOrDefault();

                    return dbuser.UserName;
                }
                else
                {
                    return "";
                }
            }
        }

        public MembershipUser GetUser(string username)
        {
            using (KSUCornerDBEntities1 db = new KSUCornerDBEntities1())
            {
                var result = from u in db.Accounts where (u.UserName == username) select u;

                if (result.Count() != 0)
                {
                    var dbuser = result.FirstOrDefault();

                    int _providerUserKey = dbuser.AccountID;
                    string _username = dbuser.UserName;
                    string _email = dbuser.Email;
                    string _passwordQuestion = dbuser.SecurityQuestion;
                    string _comment = "";
                    bool _isApproved = dbuser.IsActivated;
                    bool _isLockedOut = dbuser.IsLockedOut;
                    DateTime _creationDate = dbuser.CreateDate;
                    DateTime _lastLoginDate = dbuser.LastLoginDate;
                    DateTime _lastActivityDate = DateTime.Now;
                    DateTime _lastPasswordChangedDate = DateTime.Now;
                    DateTime _lastLockedOutDate = dbuser.LastLockedOutDate;

                    MembershipUser user = new MembershipUser("CustomMembershipProvider",
                                                              _username,
                                                              _providerUserKey,
                                                              _email,
                                                              _passwordQuestion,
                                                              _comment,
                                                              _isApproved,
                                                              _isLockedOut,
                                                              _creationDate,
                                                              _lastLoginDate,
                                                              _lastActivityDate,
                                                              _lastPasswordChangedDate,
                                                              _lastLockedOutDate);

                    return user;
                }
                else
                {
                    return null;
                }
            }
        }

        public MembershipUser GetUser(object providerUserKey)
        {
            using (KSUCornerDBEntities1 db = new KSUCornerDBEntities1())
            {
                var result = from u in db.Accounts where (u.AccountID.ToString() == providerUserKey.ToString()) select u;

                if (result.Count() != 0)
                {
                    var dbuser = result.FirstOrDefault();

                    int _providerUserKey = dbuser.AccountID;
                    string _username = dbuser.UserName;
                    string _email = dbuser.Email;
                    string _passwordQuestion = dbuser.SecurityQuestion;
                    string _comment = "";
                    bool _isApproved = dbuser.IsActivated;
                    bool _isLockedOut = dbuser.IsLockedOut;
                    DateTime _creationDate = dbuser.CreateDate;
                    DateTime _lastLoginDate = dbuser.LastLoginDate;
                    DateTime _lastActivityDate = DateTime.Now;
                    DateTime _lastPasswordChangedDate = DateTime.Now;
                    DateTime _lastLockedOutDate = dbuser.LastLockedOutDate;

                    MembershipUser user = new MembershipUser("CustomMembershipProvider",
                                                              _username,
                                                              _providerUserKey,
                                                              _email,
                                                              _passwordQuestion,
                                                              _comment,
                                                              _isApproved,
                                                              _isLockedOut,
                                                              _creationDate,
                                                              _lastLoginDate,
                                                              _lastActivityDate,
                                                              _lastPasswordChangedDate,
                                                              _lastLockedOutDate);

                    return user;
                }
                else
                {
                    return null;
                }
            }
        }

        public string GetPassword(string username, string answer)
        {
            using (KSUCornerDBEntities1 db = new KSUCornerDBEntities1())
            {
                var result = from u in db.Accounts where (u.UserName == username) select u;

                if (result.Count() != 0)
                {
                    var dbuser = result.FirstOrDefault();

                    if (dbuser.SecurityAnswer.Trim().ToLower() == answer.Trim().ToLower())
                        return dbuser.Password;
                    else
                        return "";
                }
                else
                {
                    return "";
                }
            }
        }

        public bool ValidateUser(string username, string password)
        {
            using (KSUCornerDBEntities1 db = new KSUCornerDBEntities1())
            {
                var result = from u in db.Accounts where (u.UserName == username) select u;

                if (result.Count() != 0)
                {
                    var dbuser = result.First();

                    if (dbuser.Password == password)
                        return true;
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }
        }

        public static void SetPassword(string username, string newpassword)
        {
            using (KSUCornerDBEntities1 db = new KSUCornerDBEntities1())
            {
                var result = from u in db.Accounts where (u.UserName == username) select u;
                if (result.Count() != 0)
                {
                    var dbuser = result.FirstOrDefault();
                    dbuser.Password = newpassword;
                    db.SaveChanges();
                }
            }
        }

        public static void UpdateLoginDate(string username)
        {
            using (KSUCornerDBEntities1 db = new KSUCornerDBEntities1())
            {
                var result = from x in db.Accounts
                             where (x.UserName.ToLower() == username.ToLower())
                             select x;
                if (result.Count() > 0)
                {
                    Account user = result.FirstOrDefault();
                    user.LastLoginDate = DateTime.Now;
                    db.SaveChanges();
                }
            }
        }
    }
}
