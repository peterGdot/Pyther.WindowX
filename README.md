# Pyther.WindowX

Window extension class for Win UI 3 Applications.

This simple class extends the _Microsoft.UI.Xaml.Window_ class with the following properties you may know from WPF, but are currently missing in Win UI 3:

 - `Title` - the window title
 - `Icon` - the window icon (from a resource, using the application icon or from an external file path)
 - `Width` - the window width
 - `Height` - the window height
 - `WindowStartupLocation` - the window startup location (`Manual` or `CenterScreen`)
 - `WindowState` - the window state (`Normal`, `Minimized` or `Maximized`)
 - `ResizeMode` - the resize modes (`NoResize`, `CanMinimize`, `CanResize` or `CanResizeWithGrip`)
 - `WindowStyle` - the window sytle (`None`, `SingleBorderWindow`, `ThreeDBorderWindow` or `ToolWindow`)

All properties are made available in the XAML layout files. It will also take DPI scaling into account.

To use it in your code, simple inherit from the `Pyther.WindowX.WindowX` class.

```cs
    public sealed partial class MainWindow : Pyther.WindowX.WindowX
    {
        public MainWindow() {
            this.InitializeComponent();
        }
    }
```

and modify your XAML root tag
```xml
<winX:WindowX
    ...
    xmlns:winX="using:Pyther.WindowX"
    ...
    Title="A Pyther.WindowX Example"
    Icon="Demo.Assets.window.ico"
    Width="800"
    Height="500"
    WindowStartupLocation="CenterScreen"
    WindowStyle="ToolWindow"
    >
    ...
</winX:WindowX>
```
that's it!

In addition to XAML, you can of course set all properties by code:
```cs
    public sealed partial class MainWindow : Pyther.WindowX.WindowX
    {
        public MainWindow() {
            this.InitializeComponent();

            this.Title = "A Pyther.WindowX Example";
            this.Icon = "Demo.Assets.window.ico";
            this.Width = 800;
            this.Height = 500;
            this.WindowStartupLocation = Pyther.WindowX.WindowStartupLocation.CenterScreen;
            this.WindowStyle = Pyther.WindowX.WindowStyle.ToolWindow;
        }
    }
```

This code requires [PInvoke](https://github.com/dotnet/pinvoke) and [System.Drawing.Common](https://www.nuget.org/packages/System.Drawing.Common) to run.

You will find all the required source in _Pyther.WindowX/WindowX.cs_.

## Some notes about the *Icon* property

The are 4 ways of using an icon:

1. **embedded resource** - This is the default way to include an icon into your application. You add them as an _embedded(!)_ resource and access it using the resource resolve syntax `"namespace.path.file"` like `Icon="Demo.Assets.window.ico"`.
2. **using the application icon** - If you has set an application icon, you can simply (re)use it with `Icon="application"` or `Icon="app"`.
3. **using relative file path** - To load an icon from an external file relative to your application directory, you can use the `rel:` prefix like `Icon="rel:media\window.ico"`.
4. **using absolute file path** - And finally you can also load from an external absolute file path using the `abs:` prefix like `Icon="abs:E:\Projects\Pyther\WindowX\window.ico"`.

It's also worth to mention, that no _Exception_ will be thrown, if setting an icon failed.