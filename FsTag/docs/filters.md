# Filters
 
Filters are a powerful mechanism for file selection used in 
tagging. Filters are used in various commands using their 
name/alias, such as "glob \*.txt" for a glob filter. That 
is, `... alias filter ...`.

> Note that escaped backslashes and regular slashes are 
> treated the same.

Currently implemented and supported filters are formatted follows:

- Filter Name \[ filter\_alias, another\_alias \]
Description 

---------

- Relative \[ r, rel, relative \] 
A relative filter is simply a relative path from the current 
working directory.
Ex: r:some/path/relative/to/the/current/working/directory.txt

- Absolute \[ a, abs, absolute \]
Ex: a:C:/some/rooted/path.txt

Absolute paths work for for disk roots or UNIX roots (/).

- Regex \[ re, regex \]
Ex: re:.*\.txt

Any regex pattern is valid. Be wary of possible timeouts.

- Glob/Formatted \[ f, g, formatted, glob \]
Ex: f:*.txt

*If no format is specified, the CLI will attempt to default to a relative or
absolute path.*


As for globbing, FsTag uses a custom glob implementation... **TODO**
