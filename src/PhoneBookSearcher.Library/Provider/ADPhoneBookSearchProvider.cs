using PhoneBookSearcher.Library.Config;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookSearcher.Library.Provider {

    public class ADPhoneBookSearchProvider : IPhoneBookSearchProvider {

        #region Properties

        public ADConfiguration Configuration { get; private set; }
        public DirectoryEntry RootEntry { get; private set; }

        #endregion

        #region Constructors

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
