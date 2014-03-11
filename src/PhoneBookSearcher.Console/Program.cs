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
            var searcher = GenerateSearcherForQueryType(config, query.SearchType );
            var results = searcher.Search( query );
            PrintResultsForSearchType( results, query.SearchType );
        }

        private static PhoneBookSearch GenerateSearcherForQueryType( 
            ADConfiguration config, Library.Enums.SearchType type ) {
            var searcher = null as PhoneBookSearch;
            switch(type) {
                case Library.Enums.SearchType.Department:
                    searcher = new PhoneBookSearch( new DepartmentADPhoneBookSearchProvider( config ) );
                    break;
                case Library.Enums.SearchType.PhoneNumber:
                    searcher = new PhoneBookSearch( new PhoneNumberADPhoneBookSearchProvider( config ) );
                    break;
                case Library.Enums.SearchType.Name:                   
                default:
                    searcher = new PhoneBookSearch( new NameADPhoneBookSearchProvider( config ) );
                    break;
            }
            return searcher;
        }

        private static void PrintResultsForSearchType( 
            List<PhoneBookSearchResult> results, Library.Enums.SearchType type ) {
            IResultPrinter printer = null;
            switch (type) {
                case Library.Enums.SearchType.Department:
                    results = results.OrderBy(o => o.Department).ThenBy( o => o.FullName ).ToList();
                    printer = new ConsoleDepartmentResultPrinter();
                    break;
                case Library.Enums.SearchType.PhoneNumber:
                case Library.Enums.SearchType.Name:
                default:
                    results = results.OrderBy( o => o.FullName ).ToList();
                    printer = new ConsoleNameResultPrinter();
                    break;
            }
            printer.Print( results );
        }

        static void PrintUsage() {
            System.Console.WriteLine( "Usage: pbs [-switch] <name_to_search>" );
            System.Console.WriteLine( "Switches:\n\t-d\tsearch by department" );
            System.Console.WriteLine( "\t-n\tsearch by number" );
        }

    }

}
