using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookSearcher.Library.Config {

    /// <summary>
    /// Data-type class for storing AD configuration
    /// </summary>
    public class ADConfiguration {

        #region Properties

        /// <summary>
        /// Gets or sets URI of root entry (e.g. LDAP://mycopany.com/DC=mycopmany,DC=com)
        /// </summary>
        public Uri RootEntryUri { get; set; }

        /// <summary>
        /// Gets or sets LDAP username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets LDAP password
        /// </summary>
        public string Password { get; set; }

        #endregion
        
    }

}
