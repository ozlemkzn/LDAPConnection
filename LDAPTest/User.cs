using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDAPTest
{
    public class Users
    {
        public int counter { get; set; }
        public string MemberOf { get; set; }
        /// <summary>
        /// mail
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// sAMAccountName
        /// </summary>
        public string Username { get; set; }
        public byte[] LdapImage { get; set; }
        public string Image { get; set; }
        /// <summary>
        /// telephoneNumber
        /// </summary>
        public string MobilePhoneNumber { get; set; }
        /// <summary>
        /// title,department,company
        /// </summary>
        public string UserGroupId { get; set; }
        public string UserGroup { get; set; }
        public bool IsNotifyEmailActive { get; set; }
        /// <summary>
        /// memberOf
        /// </summary>
        public object Group { get; set; }
        /// <summary>
        ///  name/cn
        /// </summary>
        public string Title { get; set; }
        public string Path { get; set; }
    }
}
