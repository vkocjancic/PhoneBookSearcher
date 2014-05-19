using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookSearcher.Library.Plugin {

    /// <summary>
    /// Interface for plugin loaders
    /// </summary>
    public interface IPluginLoader {

        #region Methods

        /// <summary>
        /// Loads all plugins
        /// </summary>
        /// <param name="pathPlugins">Path where plugins are located</param>
        void Load( string pathPlugins );

        #endregion
        
    }
}
