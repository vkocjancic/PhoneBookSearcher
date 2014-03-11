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
        private ADConfiguration m_config;
        private PhoneBookSearch m_searcher;

        #endregion

        #region Properties

        public ObservableCollection<PhoneBookSearchResult> SearchResults { get; private set; }

        #endregion

        public MainWindow() {
            this.SearchResults = new ObservableCollection<PhoneBookSearchResult>();
            InitializeSearchComponents();
            InitializeTrayIcon();
            InitializeComponent();
        }

        #region btnSearch event handlers

        private void btnSearch_Click( object sender, RoutedEventArgs e ) {
            CleanUpSearchResults();
            lvResults.Visibility = System.Windows.Visibility.Visible;
            var type = GetSearchTypeFromSelection();
            var results = TriggerSearchForType( type );
            ApplySortOrder( ref results, type );
            BindResultsToListView( lvResults, results );           
            HandleListViewGroupingsForType( lvResults, type );
            // clean up temporary data
            results.Clear();
            results = null;
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

        private void InitializeSearchComponents() {
            m_config = new ADConfiguration() {
                RootEntryUri = new Uri( Settings.Default.AdDirectoryEntry )
            };
        }

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

        private void CleanUpSearchResults() {
            m_searcher = null;
            if (null != this.SearchResults) {
                this.SearchResults.Clear();
                this.SearchResults = null;
            }
        }

        private Library.Enums.SearchType GetSearchTypeFromSelection() {
            var type = Library.Enums.SearchType.Name;
            if (rbtnDepartment.IsChecked.Value)
                type = Library.Enums.SearchType.Department;
            else if (rbtnPhoneNumber.IsChecked.Value)
                type = Library.Enums.SearchType.PhoneNumber;
            return type;
        }

        private List<PhoneBookSearchResult> TriggerSearchForType( Library.Enums.SearchType type ) {
            var query = new PhoneBookQuery() {
                SearchType = type,
                StringToSearch = tbSearch.Text
            };
            switch (type) {
                case Library.Enums.SearchType.Department:
                    m_searcher = new PhoneBookSearch( new DepartmentADPhoneBookSearchProvider( m_config ) );
                    break;
                case Library.Enums.SearchType.PhoneNumber:
                    m_searcher = new PhoneBookSearch( new PhoneNumberADPhoneBookSearchProvider( m_config ) );
                    break;
                case Library.Enums.SearchType.Name:
                default:
                    m_searcher = new PhoneBookSearch( new NameADPhoneBookSearchProvider( m_config ) );
                    break;
            }
            return m_searcher.Search( query );
        }

        private void ApplySortOrder( ref List<PhoneBookSearchResult> results, Library.Enums.SearchType type ) {
            switch (type) {
                case Library.Enums.SearchType.Department:
                    results = results.OrderBy( o => o.Department ).ThenBy( o => o.FullName ).ToList();
                    break;
                case Library.Enums.SearchType.PhoneNumber:
                case Library.Enums.SearchType.Name:
                default:
                    results = results.OrderBy( o => o.FullName ).ToList();
                    break;
            }
        }

        private void BindResultsToListView( ListView lv, List<PhoneBookSearchResult> results ) {
            this.SearchResults = new ObservableCollection<PhoneBookSearchResult>( results );
            lv.ItemsSource = this.SearchResults;
        }

        private void HandleListViewGroupingsForType( ListView lv, Library.Enums.SearchType type ) {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView( lv.ItemsSource );
            if (Library.Enums.SearchType.Department == type) {
                PropertyGroupDescription groupDescription = new PropertyGroupDescription( "Department" );
                view.GroupDescriptions.Add( groupDescription );
            }
            else
                view.GroupDescriptions.Clear();
        }

        #endregion

    }
}
