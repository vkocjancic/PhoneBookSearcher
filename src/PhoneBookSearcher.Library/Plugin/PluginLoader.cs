using PhoneBookSearcher.PluginLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PhoneBookSearcher.Library.Extension;

namespace PhoneBookSearcher.Library.Plugin {
    
    /// <summary>
    /// Plugin loader for CTI plugins
    /// </summary>
    public class PluginLoader : IPluginLoader, IDisposable {

        #region Properties

        /// <summary>
        /// Gets CTI plugins
        /// </summary>
        public List<ICti> CtiPlugins { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public PluginLoader() {
            this.CtiPlugins = new List<ICti>();
        }

        #endregion

        #region IPluginLoader methods

        /// <summary>
        /// Loads CTI plugins
        /// </summary>
        /// <param name="pathPlugins">Path where CTI plugins are located</param>
        public void Load( string pathPlugins ) {
            if (null == pathPlugins)
                throw new ArgumentNullException( "Plugin Path" );
            var infoDir = new DirectoryInfo( pathPlugins );
            if (infoDir.Exists) {
                FileInfo[] rginfoAssemblies = infoDir.GetFiles( "*.dll" );
                LoadAssemblies( rginfoAssemblies );
            }
        }

        private void LoadAssemblies( FileInfo[] rginfoAssemblies ) {
            foreach (FileInfo infoAssembly in rginfoAssemblies) {
                Assembly assembly = Assembly.LoadFrom( infoAssembly.FullName );
                LoadAllClassesImplementingPluginInterfaces( assembly );
            }
        }

        private void LoadAllClassesImplementingPluginInterfaces( Assembly assembly ) {
            foreach (Type type in assembly.GetTypes()) {
                if (type.DerivesFromInterface( typeof( ICti ) )) {
                    this.CtiPlugins.Add( Activator.CreateInstance(type) as ICti );
                }
            }
        }

        #endregion

        #region IDisposable methods

        /// <summary>
        /// Disposes of used resources
        /// </summary>
        public void Dispose() {
            if (null != this.CtiPlugins)
                this.CtiPlugins.Clear();
            this.CtiPlugins = null;
        }

        #endregion

    }

}
