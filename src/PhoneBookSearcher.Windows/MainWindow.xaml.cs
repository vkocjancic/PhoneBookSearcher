using PhoneBookSearcher.Library;
using PhoneBookSearcher.Library.Config;
using PhoneBookSearcher.Library.Provider;
using PhoneBookSearcher.Windows.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PhoneBookSearcher.Windows {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        #region Declarations

        private System.Windows.Forms.NotifyIcon m_iconNotify;

        #endregion

        #region Properties

        public ObservableCollection<PhoneBookSearchResult> SearchResults { get; private set; }

        #endregion

        public MainWindow() {
            this.SearchResults = new ObservableCollection<PhoneBookSearchResult>();
            InitializeTrayIcon();
            InitializeComponent();
        }

        #region btnSearch event handlers

        private void btnSearch_Click( object sender, RoutedEventArgs e ) {
            lvResults.Visibility = System.Windows.Visibility.Visible;
            var config = new ADConfiguration() {
                RootEntryUri = new Uri( Settings.Default.AdDirectoryEntry )
            };
            var query = new PhoneBookQuery() {
                SearchType = Library.Enums.SearchType.Name,
                StringToSearch = tbSearch.Text
            };
            var searcher = new PhoneBookSearch( new ADPhoneBookSearchProvider( config ) );
            var results = searcher.Search( query );
            this.SearchResults = new ObservableCollection<PhoneBookSearchResult>( 
                results.OrderBy( o => o.FullName ).ToList() );
            lvResults.ItemsSource = this.SearchResults;
        }

        #endregion

        #region tbSearch event handlers

        private void tbSearch_KeyUp( object sender, KeyEventArgs e ) {
            var tb = sender as TextBox;
            btnSearch.IsEnabled = (0 != tb.Text.Length);
        }

        #endregion

        #region Window event handlers

        private void Window_Closing( object sender, System.ComponentModel.CancelEventArgs e ) {
            if (null != m_iconNotify)
                m_iconNotify.Dispose();
            m_iconNotify = null;
        }

        private void Window_StateChanged( object sender, EventArgs e ) {
            if (WindowState.Minimized == WindowState) {
                Hide();
                if (null != m_iconNotify)
                    m_iconNotify.ShowBalloonTip( 2000 );
            }
        }

        private void Window_IsVisibleChanged( object sender, DependencyPropertyChangedEventArgs e ) {
            ShowHideTrayIcon();
        }

        #endregion

        #region m_iconNotify event handlers

        private void m_iconNotify_DoubleClick( object sender, EventArgs e ) {
            Show();
            WindowState = System.Windows.WindowState.Normal;
        }

        #endregion

        #region Private methods

        private void InitializeTrayIcon() {
            m_iconNotify = new System.Windows.Forms.NotifyIcon();
            m_iconNotify.BalloonTipText = "Application has been minimized. Double-click tray icon to restore.";
            m_iconNotify.BalloonTipTitle = "PhoneBookSearcher";
            m_iconNotify.Icon = new System.Drawing.Icon( 
                Application.GetResourceStream( 
                    new Uri( "pack://application:,,,/Icons/phone.ico" ) 
                ).Stream );
            m_iconNotify.DoubleClick += new EventHandler( m_iconNotify_DoubleClick );
        }

        private void ShowHideTrayIcon() {
            if (null != m_iconNotify)
                m_iconNotify.Visible = (!IsVisible);
        }

        #endregion

    }
}
