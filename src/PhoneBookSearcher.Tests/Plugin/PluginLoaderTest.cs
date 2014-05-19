using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoneBookSearcher.Library.Plugin;

namespace PhoneBookSearcher.Tests.Plugin {
    
    [TestClass]
    public class PluginLoaderTest {

        #region Declaratons

        private PluginLoader m_loader;

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
        public void Constructor_called_PluginListIsNotNull() {
            Assert.IsNotNull( m_loader.CtiPlugins );
        }

        [TestMethod]
        public void Constructor_called_PluginListIntializedButNoPluginsLoaded() {
            Assert.AreEqual( 0, m_loader.CtiPlugins.Count );
        }

        [TestMethod]
        public void Load_ParamNull_throwsArgumentNullException() {
            try {
                m_loader.Load( null );
            }
            catch (ArgumentNullException ex) {
                if (string.IsNullOrWhiteSpace(ex.ParamName) || !ex.ParamName.Equals( "Plugin Path" ))
                    Assert.Fail( string.Format( "Expected 'Plugin Path' as parameter name. Got '{0}' instead.", ex.ParamName ) );
            }
            catch (Exception ex) {
                Assert.Fail( string.Format( "{0} exception was thrown. Expected ArgumentNullException.", ex.GetType() ) );
            }
        }

    }

}
