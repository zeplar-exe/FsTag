# FsTag

![license](https://img.shields.io/github/license/zeplar-exe/FsTag)

FsTag is a utility command line interface for performing bulk operations on files.

## Features

- Powerful file filtering and selection
- Flexible "session" functionality for managing specific groups of files
- Cross-platform compatibility for Windows, Mac, and Linux.

## Installation

See versions and downloads in [Releases](https://github.com/zeplar-exe/FsTag/releases).

Additionally, add the executable to your PATH 
- on [Windows](https://stackoverflow.com/a/41895179/16324801)
- on [Mac](https://apple.stackexchange.com/a/41586)
- on [Linux](https://unix.stackexchange.com/a/183299)

## Usage

See full documentation here: 

The core functionality of the CLI is accessed via tagging:

```bash
> fstag test.txt
```

All tagged files are stored in a session-specific index, which can be viewed with the `print` command:

```bas
> fstag print --delimiter=";"
test.txt;
```

> There is no limit to how many files can be tagged (besides disk space, of course). 

Once ready to act on indexed files, the various `bulk` commands can be used:

- `fstag bulk delete`
- *crickets chirp in the distance*

For more details, run `fstag -h`

### Tag Filters

When tagging files, a filter is used sourced from the working directory. Currently, the following filters are supported:

- Regex: `fstag tag re:*.tx\w)`
- Glob: `fstag tag f:**/*.txt`
- Relative: `fstag tag r:test.txt`
- Absolute: `fstag tag a:C:\Users\me\test.txt`

If none are specified, the filter is assumed to be relative or absolute.