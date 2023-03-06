# Contribution Guidelines

## General Codebase Changes

As of now, there are no strictly enforced guidelines. Try to keep to the spirit
of existing code. When done making changes, create a detailed pull request. Use
commits to the best of your ability. Additionally, see 
[Understandig the Codebase](#understanding-the-codebase).

## Branching

Forks and clones for contribution should be based from the dev branch.

## Development Environment

Due to the use of resource files which allow C# generation, it is heavily
recommended that you use an established IDE such as 
[Visual Studio](https://visualstudio.microsoft.com/downloads/) or 
[Jetbrains Rider](https://www.jetbrains.com/rider/).

## Localization

Command, argument, and option localization is placed in
[/FsTag/Descriptiosn.resx](./FsTag/Descriptions.resx). These are used in
key-based localization for commands, options, and operands.

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
should have the `[Commands("name", Description = "...")]` and `[Subcommand]` 
attributes. On descriptions, they should be mapped to localization keys in
the aforementioned Descriptions.resx. Thus, command declarations should
look like the following;

```cs
[Command("command", Description = nameof(Descriptions.MyCommandDescriptions))
[Subcommand]
public class MyCommand { }
```

Additionally, new commands must be nested under `Program`, using it partially;

```cs
public partial class Program
{
  [Command("command", Description = nameof(Descriptions.MyCommandDescriptions))
  [Subcommand]
  public class MyCommand { }
}
```

`MyCommand` will then be accessible via `fstag command`.

This structure is to allow the interceptor in `Program` to be effectively global, 
in that it automatically implements --verbose, --quiet, and --dryrun. Commands 
arent strictly required to use these, and also note that --quiet is automatically
implemented in `WriteFormatter`. --dryrun is also implemented, albeit in various 
places, to avoid file, system, or configuration changes when running a command. For example,
the builtin `FileIndex` checks if `Program.Dryrun` is true before writing to the
index file.

A note on quiet, though: user prompts should be treated as returning a negative response,
such as no in a yes/no prompt, when --quiet is set.

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

Just like commands, operands and options should be specified using the
`[Option]` and `[Operand]` attributes, using the same syntax as `[Command]`
shown above. These too, should define descriptions using keys from
Descriptions.resx.

### Output Formatting

The `WriteFormatter` is a static helper in FsTag/Helpers to simplify and 
abstract console output. It uses an `IConsole` from the interceptor described 
above, which is useful for unit testing. It also colors output according to the 
type of output message. It has methods for writing plain text, information,
warnings, and errors with respective appropriate display colors.

Output information for just that, information. Output warnings when something
went wrong, but execution can safely continue. Output errors when something
went wrong, akin to an exception, which cannot be safely recovered from. 
On that, exceptions do not need to be explicitly caught for logging purposes, 
as that is already done so in the top-level interceptor. Wherein, an error is 
output, followed by basic exception information.

If, for whatever reason, `WriteFormatter` cannot be used, `Program.IConsole`
should be used instead for unit testing purposes.

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

### Configuration

All configuration settings are implemented in FsTag/Data/Configuration.cs.
This is a class which is serialized/deserializd by JSON.NET, thus requiring
the `[JsonProperty]` attribute. All configuration properties should be
snake_case, with the C# identifier being UpperCamelCase.
