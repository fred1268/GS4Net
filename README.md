# GS4Net (Ghostscript for .NET)

`GS4Net` is a C# thin wrapper around the Ghostscript DLL

## Prerequisites

In order to use GS4Net, you need to download the 32-bits (gsdll32.dll) or 64-bits (gsdll64.dll) version of the Ghostscript DLLs. They can be found at [https://www.ghostscript.com/download/gsdnld.html](https://www.ghostscript.com/download/gsdnld.html)

## Usage

From your C# code, you just have to call `GS4Net.GS4Net.Generate(pdf, png, null, null)` to generate the cover of the input PDF as a PNG.

This should work in both 32Bits (x86) and 64bits (x64) platforms.

There are plenty of flags you can tweak, all being described at [https://www.ghostscript.com/doc/9.52/Use.htm#Parameter_switches](https://www.ghostscript.com/doc/9.52/Use.htm#Parameter_switches)

All the `-d` and `-s` flags are supported, with default values being used when possible. Of course, you can provide override for these default values.

### Default values

For the `-d` flag:
```
FirstPage = 1
LastPage = 1
UseCropBox
DEVICEXRESOLUTION = 72
DEVICEYRESOLUTION = 72
```

For the `-s` flag:
```
PAPERSIZE = a7
OutputFile = cover.png
DEVICE = png16m
```

In addition, other default values have been used to avoid Ghostscript outputing to default output.

## Examples

```
Dictionary<string, string> dArgs = new Dictionary<string, string>();
dArgs.Add("DEVICEXRESOLUTION", "300");
dArgs.Add("DEVICEYRESOLUTION", "300");
dArgs.Add("COLORSCREEN", "false");
Dictionary<string, string> sArgs = new Dictionary<string, string>();
sArgs.Add("PAPERSIZE", "A4");
// Use Generate32 or Generate64 depending on your architecture
GS4Net.GS4Net.Generate32("document.pdf", "thumb.png", dArgs, sArgs);
```

This overrides the DPI for both X and Y, as well as the paper size and thumbnail name. Also, it uses Black & White for PNG output.

Also note that you can remove a default parameter by using something like: `dArgs.Add("PAPERSIZE", GS4Net.GS4Net.REMOVE)`. This can be helpful if you want, for instance, fix the papersize to a custom size.

## Author

Fred Thomas (https://linkedin.com/in/ctoasaservice)

## Licence

`GS4Net` is available under the MIT license. See the LICENSE file for more info.
