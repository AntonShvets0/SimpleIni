Simple .NET Core library for work with ini files.


Using:
```c#
var ini = IniRoot.FromFile("path/to/file.ini");
// or
var ini = new IniRoot("[ini]\n format = \"Value\"");

Console.WriteLine(ini["sectionName"]["fieldName"]);
Console.WriteLine(ini["fieldName"]);
````