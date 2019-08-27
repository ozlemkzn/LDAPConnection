using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Net;

namespace LDAPTest
{
    public class LDAPHelper
    {
        string ldapAddress;

        public LDAPHelper(string ldapAddress)
        {
            this.ldapAddress = ldapAddress;
        }

        public bool Login(string username, string password, string domain)
        {
            bool validation;
            try
            {
                LdapConnection lcon = new LdapConnection(ldapAddress);

                NetworkCredential nc = new NetworkCredential(username, password, domain);
                lcon.Credential = nc;
                lcon.AuthType = AuthType.Negotiate;
                lcon.Bind(nc); // user has authenticated at this point, as the credentials were used to login to the dc.
                validation = true;
                Console.WriteLine("Login OK");
            }
            catch (LdapException ex)
            {
                Console.WriteLine(ex.Message);
                validation = false;
            }
            return validation;
        }

        //public List<Users> GetTestDetail(string path)
        //{
        //    try
        //    {
        //        string domainPath = path;//DC=****, ///DC=*****,DC=com,DC=tr LDAP://127.0.0.1/DC=test,DC=com,DC=tr
        //        DirectoryEntry searchRoot = new DirectoryEntry(domainPath);
        //        searchRoot.Username = "testuser";
        //        searchRoot.Password = "********";
        //        DirectorySearcher search = new DirectorySearcher(searchRoot);
        //        search.PageSize = int.MaxValue;
        //        search.SizeLimit = Int32.MaxValue;

        //        search.PropertiesToLoad.Add(ADProperties.LOGINNAME);
        //        search.PropertiesToLoad.Add(ADProperties.EMAILADDRESS);
        //        search.PropertiesToLoad.Add(ADProperties.CONTAINERNAME);
        //        search.PropertiesToLoad.Add(ADProperties.MOBILE);
        //        search.PropertiesToLoad.Add(ADProperties.TITLE);
        //        search.PropertiesToLoad.Add(ADProperties.DEPARTMENT);
        //        search.PropertiesToLoad.Add(ADProperties.COMPANY);
        //        search.PropertiesToLoad.Add(ADProperties.MEMBEROF);
        //        search.PropertiesToLoad.Add(ADProperties.DISPLAYNAME);
        //        search.PropertiesToLoad.Add(ADProperties.LDAPIMAGE);
        //        search.PropertiesToLoad.Add(ADProperties.USERGROUP);

        //        SearchResult result;
        //        SearchResultCollection resultCol = search.FindAll();
        //        Console.WriteLine(resultCol.Count);

        //        var user = GetDetail(resultCol[0].Path.ToString());
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //    return null;
        //}


        public List<Users> GetAllUsers()
        {
            try
            {
                List<Users> res = new List<Users>();
                string DomainPath = "LDAP://OU=******,OU=OU_Users,DC=******,DC=*****,DC=com,DC=tr";
                DirectoryEntry searchRoot = new DirectoryEntry(DomainPath);

                searchRoot.Username = "testuser";
                searchRoot.Password = "*******";
                DirectorySearcher search = new DirectorySearcher(searchRoot);
                search.PageSize = int.MaxValue;
                search.Filter = "(&(objectCategory=person)(objectClass=user))";
                search.SizeLimit = int.MaxValue;

                search.PropertiesToLoad.Add(ADProperties.LOGINNAME);
                search.PropertiesToLoad.Add(ADProperties.EMAILADDRESS);
                search.PropertiesToLoad.Add(ADProperties.CONTAINERNAME);
                search.PropertiesToLoad.Add(ADProperties.MOBILE);
                search.PropertiesToLoad.Add(ADProperties.TITLE);
                search.PropertiesToLoad.Add(ADProperties.DEPARTMENT);
                search.PropertiesToLoad.Add(ADProperties.COMPANY);
                search.PropertiesToLoad.Add(ADProperties.MEMBEROF);
                search.PropertiesToLoad.Add(ADProperties.DISPLAYNAME);
                search.PropertiesToLoad.Add(ADProperties.LDAPIMAGE);
                search.PropertiesToLoad.Add(ADProperties.USERGROUP);

                SearchResult result;
                SearchResultCollection resultCol = search.FindAll();
                Console.WriteLine(resultCol.Count);
                if (resultCol != null)
                {
                    for (int counter = 0; counter < resultCol.Count; counter++)
                    {
                        result = resultCol[counter];

                        var user = GetDetail(result.Path.ToString());

                        if (user.Properties.Contains(ADProperties.LOGINNAME) &&
                            //user.Properties.Contains(ADProperties.EMAILADDRESS) &&
                            user.Properties.Contains(ADProperties.DISPLAYNAME))
                        {

                            Users u = new Users();
                            u.counter = counter;
                            u.MemberOf = user.Properties.GetValue(ADProperties.MEMBEROF);
                            u.Email = user.Properties.GetValue(ADProperties.EMAILADDRESS);
                            u.Username = user.Properties.GetValue(ADProperties.LOGINNAME);
                            u.Group = user.Properties.GetValue(ADProperties.MEMBEROF);
                            u.MobilePhoneNumber = user.Properties.GetValue(ADProperties.MOBILE);
                            u.Title = string.Format("{0}/{1}", user.Properties.GetValue(ADProperties.NAME), user.Properties.GetValue(ADProperties.CONTAINERNAME));
                            u.UserGroup = user.Properties.GetValue(ADProperties.USERGROUP);
                            //u.UserGroupId = array.Contains(ADProperties.USERGROUP.)
                            //user.Properties.GetValue(ADProperties.TITLE) + "-" +
                            //            user.Properties.GetValue(ADProperties.DEPARTMENT) + "-" +
                            //            user.Properties.GetValue(ADProperties.COMPANY);
                            u.Path = user.Path;

                            if (user.Properties.Contains("thumbnailPhoto") == true)
                            {
                                byte[] thumbnailInBytes = (byte[])user.Properties["thumbnailPhoto"][0];
                                u.LdapImage = thumbnailInBytes;
                            }

                            res.Add(u);
                        }

                    }
                }

                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public SearchResult GetDetail(string path)
        {
            //string DomainPath = "LDAP://******/CN=Ozlem,OU=FlightCrew,OU=TR,OU=***,DC=****,DC=com,DC=tr"; //"LDAP://" + ldapAddress + "/DC=******,DC=com,DC=tr";
            DirectoryEntry searchRoot = new DirectoryEntry(path);
            searchRoot.Username = "webldap";
            searchRoot.Password = "*83_ofDy";
            DirectorySearcher searcher = new DirectorySearcher(searchRoot);

            SearchResult results = searcher.FindOne();
            return results;
        }
    }



    public static class LDAPExtension
    {

        public static string GetValue(this ResultPropertyCollection item, string name)
        {
            string result = string.Empty;
            if (item[name].Count > 0)
            {
                for (int i = 0; i < item[name].Count; i++)
                {
                    result += item[name][i].ToString(); //+ " | ";
                }
            }
            return result;
        }
    }
}
