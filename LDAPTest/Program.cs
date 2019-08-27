using System;
using System.IO;
using Newtonsoft.Json;

namespace LDAPTest
{
    class Program
    {
        static void Main(string[] args)
        {
            LDAPHelper ldap = new LDAPHelper("******"); //127.0.0.1 //test
            var res = ldap.Login("testuser", "******", "test");  //test


            //var test = ldap.GetTestDetail();
            var users = ldap.GetAllUsers();
            foreach (var item in users)
            {
                WriteLog(item);
            }
            Console.WriteLine("Users Ok");
            //var res2= ldap.GetDetail();
            //WriteLog(res2);
            Console.ReadLine();
        }

        private static void WriteLog(object log)
        {
            using (StreamWriter file = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\Log.txt", true))
            {
                file.WriteLine(JsonConvert.SerializeObject(log));
                file.WriteLine("________________________________________________________________________");
            }
        }
    }
}
