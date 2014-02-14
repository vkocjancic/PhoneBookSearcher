using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookSearcher.Library {
    
    /// <summary>
    /// Data-type class for storing search results
    /// </summary>
    public class PhoneBookSearchResult {

        #region Properties

        /// <summary>
        /// Gets or sets person's department
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// Gets or sets person's full name
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets person's e-mail address
        /// </summary>
        public string MailAddress { get; set; }

        /// <summary>
        /// Gets or sets person's telephone number
        /// </summary>
        public string TelephoneNumber { get; set; }

        #endregion

    }

}
