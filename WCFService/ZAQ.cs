using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCFService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ZAQ" in both code and config file together.
    public class ZAQ : IZAQ
    {
        public User GetUser()
        {
            User user = new User
            {

                FirstName = "John",
                LastName = "JohnDoe",
                Username = "JohnDoe",
                Password = "JohnDoe",
                Department = "Engineer",
                Email = "JohnDoe@Engineer.com",
                Phone = "9876543210",
                Address = "India",
            };

            return user;
        }
    }
}
