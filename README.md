# AzAnt
Azure Ant tokenization test app to validate the config files for project migrations from NAnt/AnthillPro.

When migrating from AHP to VSTS/Azure I needed to convert our NAnt config and build scripts into comma separated value (csv) files for importation. To the csv file I added a column for the token name, description, and one for each environment (dev, test, prod, etc). I then created the source web.token.config (or app.) files that would be processed and output to web.config. The result of tokenizing two dozen projects, a half dozen environments, and hundreds of settings resulted in a massive validation effort that had to be conducted through building projects via the website when all of the tokenization wasn't yet complete. Using this app I could quickly test all permutations in my local repository by generating each config file for each environment and diff it against the one already deployed.

By default the app will search for the first `*.csv` file, then using the 'Name' and 'Prod' columns will replace all `__token__` tokens in all `*.token*` files and generate relative output files. Add 'r' to perform a recursive search.

The Visual Studio 2019 project is configured to publish a single file win-x64 executable to publish\. This was written in c# against .Net Core 3.0.

Searches are case-insensitive.

```
Usage: AzAnt [i:varsfile] [t:tokenized] [s:sub] [e:environment] [x:prefix] [y:postfix] [d:dir] [h] [q] [r] [w] [v]
               [g:genfolder] [f:outrootfolder] [n:name column]

 d The working directory. Default: .
 e The column name of the environment to use. Default: PROD
 f The output root folder of the directory structure for output files. Default: .
 g The generated files directory relative to the working directory. Default: .
 h Display this help message
 i The file containing the variables. The first one found will be used. Default: *.csv
 n The column to use to identify the tokens. Default: Name
 q Query just the output tokens and skip processing.
 r Perform a recursive search for tokenization files.
 s The substitute substring of the generated output files. Default is an empty string.
 t The unique substring of the tokenized filename(s) to process. Default: .token
 w 'What if' - print any files to the console instead of writing to disk.
 x The prefix for the tokens. Default: __
 y The postfix for the tokens if different than the prefix.
 v Verbose output. Default: False

Examples:
 AzAnt e:test
      The first *.csv will be used to process [a].token[b] into [a].[b] for column 'test'
 AzAnt i:tokens.csv t:web.token.config o:web.config e:dev
      Tokens.csv column 'dev' will be used to process web.token.config into web.config
 AzAnt n:token t:.repl. o:.
     The first *.csv will transform [a].repl.[b] into [a].[b] using column name 'token' for values in column 'PROD'
 AzAnt q e:test
     The token values for environment 'test' will be displayed, then exit with no files written.'
 ```
