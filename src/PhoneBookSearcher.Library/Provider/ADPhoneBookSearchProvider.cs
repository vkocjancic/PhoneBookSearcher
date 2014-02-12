using PhoneBookSearcher.Library.Config;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookSearcher.Library.Provider {

    /// <summary>
    /// Class representing Active directory implementation of PhoneBookSearchProvider
    /// </summary>
    public class ADPhoneBookSearchProvider : IPhoneBookSearchProvider {

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
        public ADPhoneBookSearchProvider( ADConfiguration config ) {
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
        /// Gets all user objects in Active directory that contain query in 'cn', or 'sAMAccountName' fields
        /// </summary>
        /// <param name="query">Query to search for</param>
        /// <returns>List of all user objects in objects in Active directory that contain query in 'cn', or 
        /// 'sAMAccountName' fields</returns>
        public List<PhoneBookSearchResult> GetEntriesByName( string query ) {
            if (string.IsNullOrWhiteSpace( query ))
                throw new ArgumentNullException( "Query" );
            var searcher = SetupSearcherForQuery( this.RootEntry, query );
            var results = searcher.FindAll();
            return PropagateDirectoryEntryResults( results );
        }

        #endregion

        #region Private methods

        private DirectoryEntry SetupRootEntryForConfiguration( ADConfiguration config ) {
            var entry = new DirectoryEntry(config.RootEntryUri.OriginalString);
            if ((!string.IsNullOrWhiteSpace( config.Username )) &&
                (!string.IsNullOrWhiteSpace( config.Password ))) {
                    entry.Username = config.Username;
                    entry.Password = config.Password;
            }
            return entry;
        }

        private DirectorySearcher SetupSearcherForQuery( DirectoryEntry deRoot, string query ) {
            var searcher = new DirectorySearcher( deRoot );
            searcher.PropertiesToLoad.AddRange( new string[] { "cn", "mail", "telephoneNumber" } );
            searcher.Filter = string.Format( "(&(objectClass=user)(| (cn=*{0}*)(sAMAccountName=*{0}*)))",
                query );
            return searcher;
        }

        private List<PhoneBookSearchResult> PropagateDirectoryEntryResults( SearchResultCollection results ) {
            var resultsPB = new List<PhoneBookSearchResult>();
            foreach (SearchResult result in results) {
                if (!IsResultValid( result ))
                    continue;
                resultsPB.Add( new PhoneBookSearchResult() {
                    FullName = result.Properties["cn"][0].ToString(),
                    MailAddress = result.Properties["mail"][0].ToString(),
                    TelephoneNumber = result.Properties["telephoneNumber"][0].ToString()
                } );
            }
            return resultsPB;
        }

        private bool IsResultValid( SearchResult result ) {
            var fValid = true;
            if ((!result.Properties.Contains( "cn" )) ||
                (0 == result.Properties["cn"].Count))
                fValid = false;
            if ((!result.Properties.Contains( "telephoneNumber" )) ||
                (0 == result.Properties["telephoneNumber"].Count))
                fValid = false;
            if ((!result.Properties.Contains( "mail" )) ||
                (0 == result.Properties["mail"].Count))
                fValid = false;
            return fValid;
        }

        #endregion

    }

}
