using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoneBookSearcher.Library.Provider;
using PhoneBookSearcher.Library.Config;

namespace PhoneBookSearcher.Tests {

    [TestClass]
    public class NameADPhoneBookSearchProviderTest {

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void Constructor_noConfig_throwsArgumentNullException() {
            var provider = new NameADPhoneBookSearchProvider( null );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void Constructor_configSet_NoRootEntryUri_throwsArgumentNullException() {
            var provider = new NameADPhoneBookSearchProvider( new ADConfiguration() );
        }

        [TestMethod]
        public void Constructor_configSet_configSet() {
            var config = new ADConfiguration() {
                RootEntryUri = new Uri( "LDAP://mycopany.com/DC=mycopmany,DC=com" )
            };
            var provider = new NameADPhoneBookSearchProvider( config );
            Assert.IsNotNull( provider.Configuration );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void GetEntriesForQuery_queryNull_throwsArgumentNullException() {
            var config = new ADConfiguration() {
                RootEntryUri = new Uri( "LDAP://mycopany.com/DC=mycopmany,DC=com" )
            };
            var provider = new NameADPhoneBookSearchProvider( config );
            provider.GetEntriesForQuery( null );
        }

    }

}
