using PhoneBookSearcher.Library.Config;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookSearcher.Library.Provider {
    
    /// <summary>
    /// Abstract class for AD search providers
    /// </summary>
    public abstract class ADPhoneBookSearchProviderBase : IPhoneBookSearchProvider {

        #region Properties

        /// <summary>
        /// Gets LDAP configuration
        /// </summary>
        public ADConfiguration Configuration { get; private set; }

        /// <summary>
        /// Gets Active directory's root entry
        /// </summary>
        public DirectoryEntry RootEntry { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config">LDAP configuration object.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <remarks>Username and Password fields in LDAP configuration can be null (default), 
        /// if user running an application has enough priviliges to execute queries on AD.</remarks>
        public ADPhoneBookSearchProviderBase( ADConfiguration config ) {
            if (null == config)
                throw new ArgumentNullException( "AD configuration" );
            if (null == config.RootEntryUri)
                throw new ArgumentNullException( "Root uri is not set" );
            this.Configuration = config;
            this.RootEntry = SetupRootEntryForConfiguration( this.Configuration );
        }

        #endregion

        #region IPhoneBookSearchProvider methods

        /// <summary>
        /// Gets all entries for specified query
        /// </summary>
        /// <param name="query">Query to search for</param>
        /// <returns>All directory entries containing query in specified properties</returns>
        public abstract List<PhoneBookSearchResult> GetEntriesForQuery( string query );

        #endregion

        #region Abstract methods

        /// <summary>
        /// Crates directory searcher for specified query
        /// </summary>
        /// <param name="deRoot">Directory root element</param>
        /// <param name="query">Query</param>
        /// <returns>Directory searcher object.</returns>
        protected abstract DirectorySearcher SetupSearcherForQuery( DirectoryEntry deRoot, string query );

        #endregion

        #region Virtual methods

        /// <summary>
        /// Propagate search results form directory search
        /// </summary>
        /// <param name="results">Directory search results</param>
        /// <returns>List of phone book search results</returns>
        protected virtual List<PhoneBookSearchResult> PropagateDirectoryEntryResults( SearchResultCollection results ) {
            var resultsPB = new List<PhoneBookSearchResult>();
            foreach (SearchResult result in results) {
                if (!IsResultValid( result ))
                    continue;
                resultsPB.Add( new PhoneBookSearchResult() {
                    Department = result.Properties["department"][0].ToString(),
                    FullName = result.Properties["cn"][0].ToString(),
                    MailAddress = result.Properties["mail"][0].ToString(),
                    TelephoneNumber = result.Properties["telephoneNumber"][0].ToString()
                } );
            }
            return resultsPB;
        }

        #endregion

        #region Private methods

        private DirectoryEntry SetupRootEntryForConfiguration( ADConfiguration config ) {
            var entry = new DirectoryEntry( config.RootEntryUri.OriginalString );
            if ((!string.IsNullOrWhiteSpace( config.Username )) &&
                (!string.IsNullOrWhiteSpace( config.Password ))) {
                entry.Username = config.Username;
                entry.Password = config.Password;
            }
            return entry;
        }

        private bool IsResultValid( SearchResult result ) {
            var fValid = true;
            if ((!result.Properties.Contains( "cn" )) ||
                (0 == result.Properties["cn"].Count))
                fValid = false;
            if ((!result.Properties.Contains( "department" )) ||
                (0 == result.Properties["department"].Count))
                fValid = false;
            if ((!result.Properties.Contains( "mail" )) ||
                (0 == result.Properties["mail"].Count))
                fValid = false;
            if ((!result.Properties.Contains( "telephoneNumber" )) ||
                (0 == result.Properties["telephoneNumber"].Count))
                fValid = false;
            return fValid;
        }

        #endregion

    }

}
