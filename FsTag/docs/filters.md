---
note: Simplified descriptions of the filters listed here should also be placed in filters_simple.md
---
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
Ex: r some/path/relative/to/the/current/working/directory.txt

- Absolute \[ a, abs, absolute \]
Absolute paths work for for disk roots or UNIX roots (/).
- Ex: abs C:/some/rooted/path.txt

- Regex \[ re, regex \]
Any regex pattern is valid. Be wary of possible timeouts. 
Ex: re .*\.txt

- Glob/Formatted \[ f, g, formatted, glob \]
See `fstag docs globbing` for more information.
Ex: g *.txt

*If no format is specified, the CLI will attempt to default to a relative or
absolute path.*