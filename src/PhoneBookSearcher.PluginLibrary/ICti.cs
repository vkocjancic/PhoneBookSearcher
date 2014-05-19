using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookSearcher.PluginLibrary {
    
    /// <summary>
    /// Interface for CTI implementation
    /// </summary>
    public interface ICti {

        /// <summary>
        /// Makes call to passed phone number
        /// </summary>
        /// <param name="phoneNumber">Phone number to call</param>
        void MakeCallTo( string phoneNumber );

    }

}
