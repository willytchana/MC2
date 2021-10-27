using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC2.BO
{
    public class User
    {
        public string Fullname { get; set; }
        public string Email { get; set; }
        public long PhoneNumber { get; set; }
        public string Password { get; set; }

        public User()
        {

        }

        public User(string fullname, string email, long phoneNumber, string password)
        {
            Fullname = fullname;
            Email = email;
            PhoneNumber = phoneNumber;
            Password = password;
        }

        public User(User user):this(user?.Fullname, user?.Email, user?.PhoneNumber ?? 0, user?.Password)
        {

        }

        public override bool Equals(object obj)
        {
            return obj is User user &&
                   Email == user.Email &&
                   PhoneNumber == user.PhoneNumber;
        }

        public override int GetHashCode()
        {            
            return Email.GetHashCode() * PhoneNumber.GetHashCode();
        }
    }
}
