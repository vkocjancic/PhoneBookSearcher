using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoneBookSearcher.Library;
using Moq;
using PhoneBookSearcher.Library.Provider;
using System.Collections.Generic;

namespace PhoneBookSearcher.Tests {

    [TestClass]
    public class PhoneBookSearchTest {

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void Constructor_providerNull_throwsArgumentNullException() {
            var search = new PhoneBookSearch( null );
        }

        [TestMethod]
        public void Constructor_providerSet_providerSet() {
            var provider = new Mock<IPhoneBookSearchProvider>();
            var search = new PhoneBookSearch( provider.Object );
            Assert.IsNotNull( search.Provider );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void Search_queryNull_throwsArgumentNullException() {
            var provider = new Mock<IPhoneBookSearchProvider>();
            var search = new PhoneBookSearch( provider.Object );
            var results = search.Search( null );
        }

        [TestMethod]
        public void Search_querySet_returnsResults() {
            List<PhoneBookSearchResult> expected = new List<PhoneBookSearchResult>();
            expected.Add(new PhoneBookSearchResult());
            var provider = new Mock<IPhoneBookSearchProvider>();
            provider.Setup( p => p.GetEntriesByName( It.IsAny<string>() ) ).Returns( expected );
            var search = new PhoneBookSearch( provider.Object );
            var query = new PhoneBookQuery() { PersonName = "test" };
            var results = search.Search( query );
            provider.Verify( p => p.GetEntriesByName( "test" ), Times.Once() );
            Assert.AreEqual( expected.Count, results.Count );
        }

    }

}
