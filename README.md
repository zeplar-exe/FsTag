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

## Basic Usage 

The core functionality of the CLI is accessed via tagging:

```bash
> fstag test.txt
```

All tagged files are stored in a session-specific index, which can be viewed with the `print` command:

```
> fstag print index
test.txt;
```

> There is no limit to how many files can be tagged (besides disk space for the index file, of course).

Once ready to act on indexed files, the various `bulk` commands can be used:

- `fstag bulk delete`
- *crickets chirp in the distance*

For more details, run `fstag bulk -h`.

### Sessions

A tag 'session' is simply a group of tagged files which can be switched between, like so;

```
> fstag session switch my_session
```

This will change the session to my_session, as well as create it if necessary. 

## Documentation

See [FsTag/docs](./FsTag/docs/).

The CLI also offers these files via the `fstag docs` command.

```
> fstag docs filters
Filters are a powerful mechanism for file selection used in tagging. All filters...
```

## Contribution

See [Contributing.md](./CONTRIBUTING.md).
