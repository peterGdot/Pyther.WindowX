# Pyther.WindowX

Window extension class for Win UI 3 Applications.

This simple class extends the _Microsoft.UI.Xaml.Window_ class with the following properties you may know from WPF, but are currently missing in Win UI 3:

 - `Title` - the window title
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
    Width="800"
    Height="500"
    WindowStartupLocation="CenterScreen"
    WindowStyle="ToolWindow"
    >
    ...
</winX:WindowX>
```

that's it!

This code requires [PInvoke](https://github.com/dotnet/pinvoke) to run.
