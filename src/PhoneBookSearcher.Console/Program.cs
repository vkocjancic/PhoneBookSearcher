using PhoneBookSearcher.Console.Properties;
using PhoneBookSearcher.Library;
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
            if (0 == args.Length) {
                System.Console.WriteLine( "Usage: pbs <name_to_search>" );
                Environment.Exit( 0 );
            }
            var config = new ADConfiguration() {
                RootEntryUri = new Uri( Settings.Default.AdDirectoryEntry )
            };
            var query = new PhoneBookQuery() {
                PersonName = string.Join(" ", args)
            };
            var searcher = new PhoneBookSearch( new ADPhoneBookSearchProvider( config ) );
            var results = searcher.Search( query ).OrderBy( o => o.FullName ).ToList();
            var printer = new ConsoleResultPrinter();
            printer.Print( results );
        }

    }

}
