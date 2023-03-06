# [File] Index

The file index is, at its simplest, a list of tagged files. Commands like 
`fstag tag` and `fstag rm` manipulate this, adding and deleting from it 
respectively.

This index is session-based, wherein, there is a single index file per session. 
As such, it is located in`%AppData%/fstag/session/$current_session/index.nsv`. 
Note that `.nsv` stands for "newline-seperated values".

To view the index of the current session, use `fstag print`;

```shell
> fstag print index
C:/test.txt
C:/test2.txt
C:/test3.txt
C:/test4.txt
...
```