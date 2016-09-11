// Created By Ryan Porter AKA Cloney42
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager
{
    class Account
    {
        String username;
        String password;
        String website;
        public Account()
        {
            username = "";
            password = "";
            website = "";
        }
        public Account (String user,String pass,String site)
        {
            username = user;
            password = pass;
            website = site;
        }
        public String getUsername()
        {
            return username;
        }
        public String getPassword()
        {
            return password;
        }
        public String getSite()
        {
            return website;
        }
    }
}
