# qtcl

The Quick Text Command Language (QTCL) is a simple, text based interpreted language. The primary goal is to provide plaintext files with a small amount of programmable outputs from templates. The language is designed with the mindset of keeping the feature set minimal but robust and having a low barrier to understanding and use.

This repository is for the command line utility called “qtcl“ written by Alex Colson using .NET/C#. Currently this program is only available for Windows, but cross platform support is planned. The program implements what should be considered the standard library for QTCL templates.

# Example

> [!NOTE]
> For our example, we'll assume the datetime is 12PM on Jan 1, 1970 and the prompted name value is set to "Bob".

A simple template file:
```
Hello {!name}!
Right now the time is "{Now}".
```

When parsed, the plaintext output would be:
```
Hello Bob!
Right now the time is "1/1/1970 12:00:00 PM".
```
