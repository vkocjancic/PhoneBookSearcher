using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoneBookSearcher.Library.Common;
using PhoneBookSearcher.Library;

namespace PhoneBookSearcher.Tests {

    [TestClass]
    public class ConsolePhoneBookArgumentsHandlerTest {

        #region Constructor

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void Constructor_argumentsNull_throwArgumentNullException() {
            var handler = new ConsolePhoneBookArgumentsHandler( null );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void Constructor_argumentsEmpty_throwArgumentNullException() {
            var handler = new ConsolePhoneBookArgumentsHandler( new string[] { } );
        }

        [TestMethod]
        public void Constructor_argumentsSet_argumentsSet() {
            var expected = new string[] { "bla" };
            var handler = new ConsolePhoneBookArgumentsHandler( expected );
            Assert.IsNotNull( handler.Arguments );
            Assert.AreEqual( expected, handler.Arguments );
        }

        #endregion

        #region CreateQuery

        [TestMethod]
        public void CreateQuery_argumentsTextOnly_nameSearch() {
            var expected = new PhoneBookQuery() {
                SearchType = PhoneBookSearcher.Library.Enums.SearchType.Name,
                StringToSearch = "bla bla bla"
            };
            var args = new string[] { "bla", "bla", "bla" };
            var handler = new ConsolePhoneBookArgumentsHandler( args );
            var actual = handler.CreateQuery();
            Assert.AreEqual( expected.SearchType, actual.SearchType, "Search type" );
            Assert.AreEqual( expected.StringToSearch, actual.StringToSearch, "String to search" );
        }

        [TestMethod]
        [ExpectedException( typeof( OperationCanceledException ) )]
        public void CreateQuery_argumentsInvalidSwitch_throwsOperationCanceledException() {
            var args = new string[] { "-", "bla", "bla", "bla" };
            var handler = new ConsolePhoneBookArgumentsHandler( args );
            var actual = handler.CreateQuery();
        }

        [TestMethod]
        public void CreateQuery_argumentsSwitchDSetAndNoText_departmentSearch_stringToSearchEmpty() {
            var expected = new PhoneBookQuery() {
                SearchType = PhoneBookSearcher.Library.Enums.SearchType.Department,
                StringToSearch = string.Empty
            };
            var args = new string[] { "-d" };
            var handler = new ConsolePhoneBookArgumentsHandler( args );
            var actual = handler.CreateQuery();
            Assert.AreEqual( expected.SearchType, actual.SearchType, "Search type" );
            Assert.AreEqual( expected.StringToSearch, actual.StringToSearch, "String to search" );
        }

        [TestMethod]
        public void CreateQuery_argumentsSwitchDSetAndText_departmentSearch() {
            var expected = new PhoneBookQuery() {
                SearchType = PhoneBookSearcher.Library.Enums.SearchType.Department,
                StringToSearch = "bla bla bla"
            };
            var args = new string[] { "-d", "bla", "bla", "bla" };
            var handler = new ConsolePhoneBookArgumentsHandler( args );
            var actual = handler.CreateQuery();
            Assert.AreEqual( expected.SearchType, actual.SearchType, "Search type" );
            Assert.AreEqual( expected.StringToSearch, actual.StringToSearch, "String to search" );
        }

        #endregion


    }

}
