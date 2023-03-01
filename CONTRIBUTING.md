# Contribution Guidelines

## General Codebase Changes

As of now, there are no strictly enforced guidelines. Try to keep to the spirit
of existing code. When done making changes, create a detailed pull request. Use
commits to the best of your ability. Additionally, see 
[Understandig the Codebase](#understanding-the-codebase).

## Development Environment

Due to the use of resource files which allow C# generation, it is heavily
recommended that you use an established IDE such as 
[Visual Studio](https://visualstudio.microsoft.com/downloads/) or 
[Jetbrains Rider](https://www.jetbrains.com/rider/).

## Localization

Command, argument, and option localization is placed in
[/FsTag/Descriptiosn.resx](./FsTag/Descriptions.resx). These are used via the
LocalizedCommand and LocalizedOption attributes (wrappers for the
CommandDotNet.Command attribute) wherein, the second paramtere represents the
resource name within Descriptions.resx.

## Documentation

Conceptual documentation is placed in [/FsTag/docs](./FsTag/docs). These should
focus on individual concepts, such as diverse ways that a command uses its
parameters that cannot be easily represented in help text. Keep note that these
files are directly printed to the console via the `fstag docs [module]` command.

As of now, documentation is not localized.

## Understanding the Codebase

Although relatively simple (as of writing this), there is a good body of 
information contributors should be aware of, which will be detailed in this 
section.

> File paths here are relative to the root of the repository, usually.

### Commands

Commands should be placed in FsTag/Commands. As per CommandDotNet, each commmand
should have the `[LocalizedCommand]` and `[Subcommand]` attributes. Note the use
of `LocalizeCommand`, which takes 2 parameters: the command name and the resx
localization key in FsTag/Resources/Descriptions.resx. Note that 
`[LocalizedCommand]` inherits from `CommandDotNet.Command`, meaning it contains
the properties present there as well.

```cs
[LocalizedCommand("command", nameof(Descriptions.MyCommandDescriptions))
[Subcommand]
public class MyCommand { }
```

Additionally, new commands must be nested under `Program`, using it partially;

```cs
public partial class Program
{
  [LocalizedCommand("command", nameof(Descriptions.MyCommandDescriptions))
  [Subcommand]
  public class MyCommand { }
}
```

`MyCommand` will then be accessible via `fstag command`.

This is to allow the interceptor in `Program` to be effectively global, in that
it automatically implements --verbose, --quiet, and --dryrun. Commands arent
strictly required to use these, and also note that --quiet is automatically
implemented in `WriteFormatter`. --dryrun is also implemented, albeit in various 
places, to avoid file, system, or configuration changes when running a command. For example,
the builtin `FileIndex` checks if `Program.Dryrun` is true before writing to the
index file.

Note that `WriteFormatter` is a static helper in FsTag/Helpers to simplify and 
abstract console output. It uses an `IConsole` from the interceptor described 
above, which is useful for unit testing. It also colors output according to the 
type of output message.

On --verbose, this should be used when a more complex form of output should be 
shown, such as a longer description of something.

Alas; for bulk commands, the same partial rule applies;

```cs
public partial class Program
{
  public partial class BulkCommands
  {
    [LocalizedCommand("command", nameof(Descriptions.MyCommandDescriptions))
    [Subcommand]
    public class MyCommand { }
  }
}
```

`MyCommand` will thus be accessible via `fstag bulk command`.

On that note, bulk commands should be placed in FsTag/Commands/Bulk.

#### Options and Operands/Arguments

As of now, options should use the `[LocalizedOption]` attribute, which
takes a short and/or long name, plus a localization key present in
FsTag/Resources/Descriptions.resx. Exactly the same as `[LocalizedCommand]`.
Note that `[LocalizedOption]` inherits from `CommandDotNet.Option`, meaning
it contains all of the properties present there as well.

As for operands/arguments, there is no `[LocalizedOperand]` attribute
ready for use.

### AppData

The `AppData` class is static "helper" which serves to abstract access to the 
file system and common operatins with a unified collection of interfaces.
These interfaces are placedd in FsTag/Data/Interfaces, and are manually placed
in `AppData`. The default implementations of these interfaces are placed in
FsTag/Data/Builtin. Naturally, they pertain to the expected functionality of 
the CLI and are instantiated by default in `AppData`. However, this is subject 
to change.

### Localization

Localization is achieved using .resx files, which should be located in
`FsTag/Resources/`. For this reason, it is reccomended that contributors use 
Visual Studio, Rider, or any established .NET IDE. Localization keys should
be abbreviated or shortened where possible as to avoid long lines for 
attributes.
