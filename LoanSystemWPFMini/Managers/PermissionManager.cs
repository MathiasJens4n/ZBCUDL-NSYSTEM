using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;

namespace LoanSystemWPFMini.Managers
{
    public class PermissionManager
    {
        // figure out LDAPS/LDAP Secure
        private static readonly string ldp = "LDAP://ldap.efif.dk/ou=zbc,ou=UserAccounts,dc=efif,dc=dk";
        public bool IsTeacher(string name) {
            using (var entry = new DirectoryEntry(ldp)) {
                DirectorySearcher s = new DirectorySearcher(entry,
                    "(&" +
                        "(objectCategory=person)" +
                        $"(sAMAccountName={name})" +
                    ")",
                    new[] { "memberof" });
                var result = s.FindOne();
                if (result == null) {
                    throw new Exception();
                }
                List<string> groups = new List<string>();
                foreach (string group in result.Properties["memberof"])
                {
                    groups.Add(group.Split(',')[0].Split('=')[1]);
                }
                
                //if (_gruppe == "zbc_alle_elever")
                //{
                //}
                //else if (_gruppe == "ZBC-Ansatte(Alle)" || _gruppe == "ZBC-all-teachers" || _gruppe == "ZBC-Pers-alle")
                //{
                //}
                return groups.Contains("ZBC-Ansatte(Alle)") || groups.Contains("ZBC-all-teachers") || groups.Contains("ZBC-Pers-alle");
            }
        }

        public bool CanLoan(string name, string password)
        {
            DirectoryEntry ldapConnection = new DirectoryEntry(ldp);
            ldapConnection.Username = name;
            ldapConnection.Password = password;
            ldapConnection.AuthenticationType = AuthenticationTypes.Secure;

            return IsValidEntry(ldapConnection);            
        }

        private bool IsValidEntry(DirectoryEntry de)
        {
            try
            {
                var _ = de.NativeObject;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
