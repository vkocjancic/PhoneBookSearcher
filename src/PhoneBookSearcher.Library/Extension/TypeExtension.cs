using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookSearcher.Library.Extension {
    
    /// <summary>
    /// Extension for class Type
    /// </summary>
    public static class TypeExtension {

        /// <summary>
        /// Checks wether type implements an interface
        /// </summary>
        /// <param name="type">Type to check</param>
        /// <param name="typeInterface">Interface to check</param>
        /// <returns>True if type implements interface. False, otherwise.</returns>
        public static bool DerivesFromInterface( this Type type, Type typeInterface ) {
            if (null == typeInterface)
                throw new ArgumentNullException( "Interface type" );
            return type.GetInterfaces().Contains( typeInterface );
        }

    }

}
