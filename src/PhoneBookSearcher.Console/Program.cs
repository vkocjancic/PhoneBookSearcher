using PhoneBookSearcher.Console.Properties;
using PhoneBookSearcher.Library;
using PhoneBookSearcher.Library.Common;
using PhoneBookSearcher.Library.Config;
using PhoneBookSearcher.Library.Printer;
using PhoneBookSearcher.Library.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookSearcher.Console {

    class Program {

        static void Main( string[] args ) {
            PhoneBookQuery query = null;
            try {
                var handlerArgs = new ConsolePhoneBookArgumentsHandler( args );
                query = handlerArgs.CreateQuery();
            }
            catch (Exception ex) {
                System.Console.WriteLine( "Error:\n{0}", ex.Message );
                PrintUsage();
                Environment.Exit( 0 );
            }
            if (string.IsNullOrWhiteSpace( query.StringToSearch )) {
                PrintUsage();
                Environment.Exit( 0 );
            }
            var config = new ADConfiguration() {
                RootEntryUri = new Uri( Settings.Default.AdDirectoryEntry )
            };
            var searcher = null as PhoneBookSearch;
            if (Library.Enums.SearchType.Name == query.SearchType)
                searcher = new PhoneBookSearch( new NameADPhoneBookSearchProvider( config ) );
            else
                searcher = new PhoneBookSearch( new DepartmentADPhoneBookSearchProvider( config ) );
            var results = searcher.Search( query );
            IResultPrinter printer = null;
            if (Library.Enums.SearchType.Name == query.SearchType) {
                results = results.OrderBy( o => o.FullName ).ToList();
                printer = new ConsoleNameResultPrinter();
            }
            else {
                results = results.OrderBy( o => o.Department ).ToList();
                printer = new ConsoleDepartmentResultPrinter();
            }
            printer.Print( results );
        }

        static void PrintUsage() {
            System.Console.WriteLine( "Usage: pbs [-switch] <name_to_search>" );
            System.Console.WriteLine( "Switches:\n\t-d\tsearch by department" );
        }

    }

}
