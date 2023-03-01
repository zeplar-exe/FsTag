# Documentation

This file describes some protocols and notes for editing documentation files.

## Use markdownlint

A .markdownlint.json configuration file is supplied in this directory. 
As such, markdownlint is best used with the Visual Studio Code 
[extension](https://marketplace.visualstudio.com/items?itemName=DavidAnson.vscode-markdownlint).

## Aliases

An alias for a documentation file is defined in the YAML frontmatter like so;

```markdown
---
alias: [ alias_1, alias_2 ]
---
```

An alias declaration must be an array of names, of which there are no limitations for. 
These aliases are alternative names which can be used in place of the file name. For
example, if this file had the following alias definition;

```yaml
alias: [ docs_alias ]
```

Then, it can be accessed via the CLI like so;

```shell
> fstag docs docs_alias
```