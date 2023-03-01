# Recursion

Various commands allow for recursive operations on the filesystem. Such as
`fstag tag ...` and `fstag rm ...`. In every case, it is usable via the
--recursive <depth> option like so;

```shell
> fstag tag glob *.txt --recursive 5
...
```

This will set a recurse depth of 5, meaning a maximum of 5 subfolders down 
from the current working directory. If the depth is 0, no recursion takes
place. If the depth is less than 0, the operation will recurse every
sub-directory present, down the leaves of the directory tree.
