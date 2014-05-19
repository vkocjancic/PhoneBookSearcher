using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoneBookSearcher.Library.Extension;
using PhoneBookSearcher.Library.Plugin;
using PhoneBookSearcher.PluginLibrary;

namespace PhoneBookSearcher.Tests.Extension {

    [TestClass]
    public class TypeExtensionTest {

        #region Declarations

        PluginLoader m_loader;

        #endregion

        [TestInitialize]
        public void BeforeEachTest() {
            m_loader = new PluginLoader();
        }

        [TestCleanup]
        public void AfterEachTest() {
            if (null != m_loader)
                m_loader.Dispose();
        }

        [TestMethod]
        public void DerivesFromInterface_paramNull_throwsArgumentNullException() {
            try {
                m_loader.GetType().DerivesFromInterface( null );
            }
            catch (ArgumentNullException ex) {
                if (string.IsNullOrWhiteSpace( ex.ParamName ) || !ex.ParamName.Equals( "Interface type" ))
                    Assert.Fail( string.Format( "Expected 'Interface type' as parameter name. Got '{0}' instead.", ex.ParamName ) );
            }
            catch (Exception ex) {
                Assert.Fail( string.Format( "{0} exception was thrown. Expected ArgumentNullException.", ex.GetType() ) );
            }
        }

        [TestMethod]
        public void DerivesFromInterface_TypeImplementsInterface_returnsTrue() {
            Assert.IsTrue( m_loader.GetType().DerivesFromInterface( typeof( IPluginLoader ) ) );
        }

        [TestMethod]
        public void DerivesFromInterface_TypeDoesNotImplementsInterface_returnsFalse() {
            Assert.IsFalse( m_loader.GetType().DerivesFromInterface( typeof( ICti ) ) );
        }

    }

}
