using PhoneBookSearcher.Library.Config;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookSearcher.Library.Provider {

    /// <summary>
    /// Search provider for querying directory services by department
    /// </summary>
    public class DepartmentADPhoneBookSearchProvider : ADPhoneBookSearchProviderBase {

        #region Constructors

        /// <summary>
        /// Construcotr
        /// </summary>
        /// <param name="config">Configuration for directory services</param>
        public DepartmentADPhoneBookSearchProvider( ADConfiguration config )
            : base( config ) {
        }

        #endregion

        #region ADPhoneBookSearchProviderBase methods

        /// <summary>
        /// Gets entries for name query
        /// </summary>
        /// <param name="query">Search query</param>
        /// <returns>All entries in AD containing query as part of name or username</returns>
        public override List<PhoneBookSearchResult> GetEntriesForQuery( string query ) {
            if (string.IsNullOrWhiteSpace( query ))
                throw new ArgumentNullException( "Query" );
            var searcher = SetupSearcherForQuery( this.RootEntry, query );
            var results = searcher.FindAll();
            return PropagateDirectoryEntryResults( results );
        }

        /// <summary>
        /// Crates directory searcher for specified query
        /// </summary>
        /// <param name="deRoot">Directory root element</param>
        /// <param name="query">Query</param>
        /// <returns>Directory searcher object.</returns>
        protected override DirectorySearcher SetupSearcherForQuery( DirectoryEntry deRoot, string query ) {
            var searcher = new DirectorySearcher( deRoot );
            searcher.PropertiesToLoad.AddRange( new string[] { "cn", "department", "mail", "telephoneNumber" } );
            searcher.Filter = string.Format( "(&(objectClass=user)(department=*{0}*))",
                query );
            return searcher;
        }

        #endregion

    }

}
