---
alias: [ glob ]
---
# Globbing

Globbing is implemented via the DotNet.Glob package. It currently 
supports the following patterns;

*Shamelessly copied from their README...*

| Wildcard  | Description | Example | Matches | Does not match |
| --------  | ----------- | ------- | ------- | -------------- |
| \* |  matches any number of any characters including none	| Law\*| Law, Laws, or Lawyer	|
| ?	| matches any single character	| ?at	| Cat, cat, Bat or bat	| at |
| [abc] |	matches one character given in the bracket |	[CB]at |	Cat or Bat	| cat or bat |
| [a-z] |	matches one character from the range given in the bracket	| Letter[0-9]	| Letter0, Letter1, Letter2 up to Letter9	| Letters, Letter or Letter10 |
| [!abc] | matches one character that is not given in the bracket | [!C]at | Bat, bat, or cat | Cat |
| [!a-z] | matches one character that is not from the range given in the bracket | Letter[!3-5] | Letter1, Letter2, Letter6 up to Letter9 and Letterx etc. | Letter3, Letter4, Letter5 or Letterxx |
| `**` |  matches any number of path / directory segments. When used must be the only contents of a segment. | /\*\*/some.\* | /foo/bar/bah/some.txt, /some.txt, or /foo/some.txt	|

When used in tagging, for example, the glob will be matched according to the 
current working directory.
