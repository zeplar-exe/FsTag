==Filters==

Filters are a powerful mechanism for file selection used in tagging. All filters
are formatted as to have a prefix, such as f:*.txt for a glob filter.

Escaped backslashes and regular slashes are treated the same.

Currently implemented and supported filters are as follows:

- Relative
Ex: r:some/path/relative/to/the/current/working/directory.txt

- Absolute 
Ex: a:C:/some/rooted/path.txt

Absolute paths work for for disk roots or UNIX roots (/).

- Regex
Ex: re:.*\.txt

Any regex pattern is valid. Be wary of possible timeouts.

- Glob/Formatted
Ex: f:*.txt

*If no format is specified, the CLI will attempt to default to a relative or
absolute path.*


As for globbing, FsTag uses a custom glob implementation
