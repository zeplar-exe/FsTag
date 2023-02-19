# Contribution Guidelines

## General Codebase Changes

As of now, there are no strictly enforced guidelines. Try to keep to the spirit
of existing code. When done making changes, create a detailed pull request. Use
commits to the best of your ability

## Documentation

Command, argument, and option documentation is placed in
[/FsTag/Descriptiosn.resx](./FsTag/Descriptions.resx). These are used via the
LocalizedCommand and LocalizedOption attributes (wrappers for the
CommandDotNet.Command attribute) wherein, the second paramtere represents the
resource name within Descriptions.resx.

Conceptual documentation is placed in [/FsTag/docs](./FsTag/docs). These should
focus on individual concepts, such as diverse ways that a command uses its
parameters that cannot be easily represented in help text. Keep note that these
files are directly printed to the console via the `fstag docs [module]` command.

## Development Environment

Due to the use of resource files which allow C# generation, it is heavily
recommended that you use an established IDE such as 
[Visual Studio](https://visualstudio.microsoft.com/downloads/) or 
[Jetbrains Rider](https://www.jetbrains.com/rider/).