using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookSearcher.Library.Common {

    /// <summary>
    /// Handles arguments passed by console application
    /// </summary>
    public class ConsolePhoneBookArgumentsHandler : IPhoneBookArgumentsHandler {

        #region Properties

        /// <summary>
        /// Gets command-line arguments
        /// </summary>
        public string[] Arguments { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="args">Command-line arguments</param>
        public ConsolePhoneBookArgumentsHandler( string[] args ) {
            if ((null == args) || (0 == args.Length))
                throw new ArgumentNullException( "Arguments" );
            this.Arguments = args;
        }

        #endregion

        #region IPhoneBookArgumentsHandler methods

        /// <summary>
        /// Creates query from arguments
        /// </summary>
        /// <returns>Phone book query</returns>
        public PhoneBookQuery CreateQuery() {
            Enums.SearchType typeSearch = Enums.SearchType.Name;
            var strToSearch = string.Empty;
            if (this.Arguments[0].Equals( "-d" )) {
                typeSearch = Enums.SearchType.Department;
                strToSearch = string.Join( " ", this.Arguments, 1, this.Arguments.Length - 1 );
            }
            else if (this.Arguments[0].Equals( "-n" )) {
                typeSearch = Enums.SearchType.PhoneNumber;
                strToSearch = string.Join( " ", this.Arguments, 1, this.Arguments.Length - 1 );
            }
            else if (this.Arguments[0].StartsWith( "-" ))
                throw new OperationCanceledException( "Invalid switch was provided" );
            else {
                strToSearch = string.Join( " ", this.Arguments );
            }
            return new PhoneBookQuery() {
                SearchType = typeSearch,
                StringToSearch = strToSearch
            };
        }

        #endregion

    }

}
