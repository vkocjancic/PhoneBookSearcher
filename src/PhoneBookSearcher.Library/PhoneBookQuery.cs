using PhoneBookSearcher.Library.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookSearcher.Library {

    /// <summary>
    /// Data-type class used to store query information
    /// </summary>
    public class PhoneBookQuery {

        #region Properties

        /// <summary>
        /// Gets or sets search type
        /// </summary>
        public SearchType SearchType { get; set; }

        /// <summary>
        /// Gets or sets string to search for
        /// </summary>
        public string StringToSearch { get; set; }

        #endregion

    }

}
