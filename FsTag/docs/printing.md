# Printing

`fstag print ...` is a special command, similar to `fstag docs` for viewing 
various data related to the CLI. For example, `fstag print index` will print
semicolon-delimited vomit, consisting of every tagged file in the current 
session.

```shell
> fstag print index
C:/some_file.txt;C:/some_other_file.txt;D:/file_on_another_drive.md
```

When printing, a **key** is supplied, such as `index` in the example above.
Any number of keys can be printed in one command like so,

```shell
fstag print index raw_config
C:/some_file.txt;C:/some_other_file.txt;D:/file_on_another_drive.md
{
    "my_config_value": 0,
    ...
}
```

For a complete list of print keys, simply use the `fstag print` command with
no keys attached.