using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookSearcher.Library.Common {

    /// <summary>
    /// Interface for handling phone book arguments
    /// </summary>
    public interface IPhoneBookArgumentsHandler {

        /// <summary>
        /// Creates phone book query from arguments
        /// </summary>
        /// <returns>Phone book query</returns>
        PhoneBookQuery CreateQuery();

    }

}
