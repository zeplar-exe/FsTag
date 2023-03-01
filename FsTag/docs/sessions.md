# Sessions

A session, at its core, is a collection of tagged files under a unique
identifier. It serves to isolate tagging operations with separated file
indexes.

By default, the 'default' session is used. If this session ceases to exist, 
it will be created given no other option.

To switch (and create, if necessary) sesssions, use 
`fstag session switch my_session`. All sessions are stored as directories
in %AppData%/fstag/sessions. Thus, a session name cannot contain invalid
path characters, such as / and &.

To delete a session and all of its contents, use `fstag session rm my_session`.
This will delete the directory, and it won't be moved to the recycle bin.

To list all currently existing sessions, use `fstag session` on its own. The
currently in-use session with be marked with 3 asteriks;

```shell
> fstag session
default
***my_session
other_session
```
