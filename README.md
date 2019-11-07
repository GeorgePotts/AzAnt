# AzAnt
Azure tokenization app to help test migrations from NAnt/AnthillPro projects.

By default the app will search for the first `*.csv` file, then using the 'Name' and 'Prod' columns will replace all `__token__` tokens in all `*.token*` files and generate relative output files.

The Visual Studio project is configured to publish a single file win-x64 executable to publish\.

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
