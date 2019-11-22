Community Script Hook RDR2 .NET
============================

**THIS IS A PRE RELEASE BUILD, USE HOW YOU WANT TO! EXPECT ERRORS, CRASHES, ETC**

This is an ASI plugin for Red Dead Redemption 2, ported from [**ScriptHookVDotNet**](https://github.com/crosire/scripthookvdotnet/), based on the C++ ScriptHook by Alexander Blade, which allows running scripts written in any .NET language in-game.

The issues page should be primarily used for bug reports and enhancement ideas. Questions related to RDR2 scripting in general are better off in forums dedicated to this purpose, like [NexusMods](https://www.nexusmods.com/reddeadredemption2) or [gtaforums.com](https://gtaforums.com/forum/459-modding/).

## Known Issues
* Returning pointers causes crash.
* (sometimes) Passing strings through native causes crash (conversion from const char** pointer to string)

## Requirements

* [C++ ScriptHookRDR2 by Alexander Blade](http://www.dev-c.com/rdr2/scripthookrdr2/)
* [.NET Framework ≥ 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48)
* [Visual C++ Redistributable for Visual Studio 2019 x64](https://support.microsoft.com/en-us/help/2977003/the-latest-supported-visual-c-downloads)

## Downloads

Pre-built binaries can be found on the [releases](https://github.com/crosire/scripthookrdr2dotnet/releases) page.

## Contributing

You'll need Visual Studio 2017 or higher to open the project file and the [Script Hook RDR2 SDK](http://dev-c.com/rdr2/scripthookrdr2/) extracted into "[/sdk](/sdk)".

Additionally, create an environment variable called `RDR2DN_VERSION` and set it to the version you are attempting to build.

Any contributions to the project are welcomed, it's recommended to use GitHub [pull requests](https://help.github.com/articles/using-pull-requests/).

## License

All the source code except for the Vector, Matrix and Quaternion classes, which are licensed separately, is licensed under the conditions of the [zlib license](LICENSE.txt).


### Credits
Crosire (ScriptHookVDotNet)
Jedijosh920 - lots of help, natives for methods
