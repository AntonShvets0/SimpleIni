Simple .NET Core library for work with ini files.


Using:
```c#
var ini = Ini.FromFile("path/to/file.ini");
// or
var ini = new Ini("[ini]\n format = \"Value\"");

Console.WriteLine(ini["sectionName"]["fieldName"]);
Console.WriteLine(ini["fieldName"]);
````