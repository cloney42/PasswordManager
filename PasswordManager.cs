// Created By Ryan Porter AKA Cloney42
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using EncryptStringSample;

namespace PasswordManager
{
    class PasswordManager
    {
        String passCode = "";
        StreamWriter sw;
        StreamReader sr;
        List<Account> Accounts = new List<Account>();
        public PasswordManager()
        {
            dispMenu();
        }
        static void Main(string[] args)
        {
            Console.Title = "Password Manager";
            PasswordManager pm = new PasswordManager();
        }
        public void dispMenu()
        {
            Console.WriteLine("PASSWORD MANAGER V1.0.0B");
            Console.WriteLine("------------------------");
            Console.WriteLine("{1} View Account Info");
            Console.WriteLine("{2} Add Account Info");
            Console.WriteLine("{3} Remove Account Info");
            Console.WriteLine("{4} Set PassCode");
            Console.WriteLine("{5} Quit");
            String menuInput = "";
            while (true)
            {
                try {
                    menuInput = Console.ReadLine();
                    if (menuInput.Equals("1") || menuInput.Equals("2") || menuInput.Equals("3") || menuInput.Equals("4") || menuInput.Equals("5"))
                    {
                        break;
                    }
                    else
                    {
                        throw new Exception();
                    }
                } catch (Exception)
                {
                    Console.WriteLine("Try Again");
                }
                
            }
            Console.Clear();
            switch (menuInput)
            {
                case "1":
                    dispInfo();
                    break;
                case "2":
                    addInfo();
                    break;
                case "3":
                    removeInfo();
                    break;
                case "4":
                    setPassCode();
                    break;
                case "5":
                    Environment.Exit(0);
                    break;
            }
        }
        public void dispInfo()
        {
            readFile();
            Console.WriteLine("DISPLAY ACCOUNTS");
            Console.WriteLine("----------------\n");
            Console.WriteLine("NUM  USERNAME    PASSWORD    SITE");
            Console.WriteLine("---------------------------------");
             for (int i = 0;i< Accounts.Count; i++)
            {
               Console.WriteLine(i+1+"\t"+Accounts[i].getUsername()+"\t"+Accounts[i].getPassword()+"\t"+Accounts[i].getSite());
             }
            Console.ReadKey();
            Console.Clear();
            dispMenu();
        }
        public void addInfo()
        {
            readFile();
            Console.WriteLine("ADD ACCOUNT INFO");
            Console.WriteLine("----------------\n");
            Console.WriteLine("What is the site of the account?");
            String site = Console.ReadLine();
            Console.WriteLine("What is the username?");
            String username =  Console.ReadLine();
            while (true)
            {
                Console.WriteLine("What is the account password?");
                String password1 = inputPassword();
                Console.WriteLine("Confirm Password");
                String password2 = inputPassword();
                if (!password1.Equals(password2))
                {
                    Console.WriteLine("Passwords are not the same. Try Again.");
                }
                else
                {
                    Console.WriteLine("Passwords Match!");
                    Account a = new Account(username, password1, site);
                    Accounts.Add(a);
                    break;
                }
            }
            Console.ReadKey();
            Console.Clear();
            writeFile();
            dispMenu();
        }
        public void removeInfo()
        {
            Console.WriteLine("REMOVE ACCOUNT INFO");
            Console.WriteLine("-------------------\n");
            readFile();
            int removeNum;
            while (true)
            {
                Console.WriteLine("What number do you want to remove?");
                String removeStr = Console.ReadLine();
                try
                {
                    removeNum = Int32.Parse(removeStr);
                    if (removeNum < 1 || removeNum > Accounts.Count)
                    {
                        throw new Exception();
                    }
                    else
                    {
                        break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid Input. Try Again.");
                }
            }
            Accounts.RemoveAt(removeNum - 1);
            Console.WriteLine("Account #"+removeNum+" removed!");
            writeFile();
            Console.ReadKey();
            Console.Clear();
            dispMenu();
        }
        public void readFile()
        {
            if (passCode.Equals(""))
            {
                setPassCode();
            }
            String username;
            String password;
            String website;
            for (int i = Accounts.Count-1; i >= 0; i--)
            {
                Accounts.RemoveAt(i);
            }
            try
            {
                sr = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), "AccountList.txt"));
            } catch (Exception)
            {
                Console.WriteLine("ERROR: No File Found");
                Console.WriteLine("\nPress any key to exit.");
                Console.ReadKey();
                Environment.Exit(0);
            }
            while (sr.Peek() >= 0)
            {
                String lineEncrypted = sr.ReadLine();
                String line = "";
                try {
                    line = StringCipher.Decrypt(lineEncrypted, passCode);
                } catch (Exception)
                {
                    Console.WriteLine("ERROR: Wrong PassCode!");
                    Console.ReadKey();
                    Console.Clear();
                    dispMenu();
                }
                int usernameLocation = line.IndexOf(":");
                username = line.Substring(0, usernameLocation);
                int passwordLocation = line.IndexOf(":", usernameLocation + 1);
                password = line.Substring(usernameLocation + 1, passwordLocation - (usernameLocation + 1));
                int siteLocation = line.IndexOf(":", passwordLocation + 1);
                website = line.Substring(passwordLocation + 1);
                Account a = new Account(username, password, website);
                Accounts.Add(a);
            }
            sr.Close();
        }
        public void writeFile()
        {
            if (passCode.Equals(""))
            {
                setPassCode();
            }
            try
            {
                sw = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), "AccountList.txt"));
            }
            catch (Exception)
            {
                Console.WriteLine("ERROR: No File Found");
                Console.WriteLine("\nPress any key to exit.");
                Console.ReadKey();
                Environment.Exit(0);
            }
            foreach (Account a in Accounts)
            {
                String line = "";
                line += a.getUsername();
                line += ":";
                line += a.getPassword();
                line += ":";
                line += a.getSite();
                try {
                    sw.WriteLine(StringCipher.Encrypt(line, passCode));
                } catch (Exception)
                {
                    Console.WriteLine("ERROR: Wrong PassCode!");
                    Console.ReadKey();
                    Console.Clear();
                    dispMenu();
                }
            }
            sw.Close();
        }
        public String inputPassword()
        {
            String password = "";
            int numStars = 0;
            while (true)
            {
                ConsoleKeyInfo keyPressed = Console.ReadKey(true);
                if (keyPressed.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (keyPressed.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password = password.Substring(0, password.Length - 1);
                        numStars--;
                    }
                    for (int i = 0; i < 1000; ++i)
                    {
                        Console.Write("\r                               ");
                    }
                    Console.Write("\r");
                    for (int i = 0; i < numStars; i++)
                    {
                        Console.Write("*");
                    }
                }
                else {
                    password += keyPressed.KeyChar;
                    Console.Write("*");
                    numStars++;
                }
            }
            Console.Write("\n");
            return password;
        }
        public void setPassCode()
        {
            while (true)
            {
                Console.WriteLine("What is the PassCode?");
                String passCode1 = inputPassword();
                Console.WriteLine("Confirm PassCode");
                String passCode2 = inputPassword();
                if (passCode1.Equals(passCode2))
                {
                    Console.WriteLine("PassCodes Match!");
                    passCode = passCode1;
                    break;
                }
                else
                {
                    Console.WriteLine("PassCodes are not the same. Try again.");
                }
            }
            Console.ReadKey();
            Console.Clear();
            dispMenu();
        }
    }
}