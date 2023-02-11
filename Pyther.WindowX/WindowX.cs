using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using PInvoke;
using static PInvoke.User32;

namespace Pyther.WindowX
{
    public enum WindowStartupLocation
    {
        Manual = 0,
        CenterScreen = 1
        // CenterOwner = 2  // not implemented yet
    }

    public enum WindowState
    {
        Normal = 0,
        Minimized = 1,
        Maximized = 2
    }

    public enum ResizeMode
    {
        NoResize = 0,
        CanMinimize = 1,
        CanResize = 2,
        CanResizeWithGrip = 3
    }

    public enum WindowStyle
    {
        None = 0,
        SingleBorderWindow = 1,  // will use normal border
        ThreeDBorderWindow = 2,  // will use normal border
        ToolWindow = 3           // will hide minimze and maximize buttons
    }

    public class WindowX : Microsoft.UI.Xaml.Window
    {
        #region Handles

        private IntPtr handle = IntPtr.Zero;
        public IntPtr Handle {
            get {
                if (handle == IntPtr.Zero) {
                    handle = WinRT.Interop.WindowNative.GetWindowHandle(this);
                }
                return handle;
            }
        }

        private WindowId windowId = new(0);
        public WindowId WindowId {
            get {
                if (windowId.Value == 0) {
                    windowId = Win32Interop.GetWindowIdFromWindow(Handle);
                }
                return windowId;
            }
        }

        private AppWindow appWindow = null;
        public AppWindow AppWindow {
            get {
                if (appWindow == null) {
                    appWindow = AppWindow.GetFromWindowId(WindowId);
                }
                return appWindow;
            }
        }

        private OverlappedPresenter presenter;
        public OverlappedPresenter Presenter {
            get {
                if (presenter == null) {
                    presenter = AppWindow.Presenter as OverlappedPresenter;
                }
                return presenter;
            }
        }

        #endregion

        #region Get DPI scaling

        private float scale = -1f;
        public float Scale {
            get {
                if (scale < 0) {
                    var dpi = PInvoke.User32.GetDpiForWindow(Handle);
                    scale = (float)dpi / 96;
                }
                return scale;
            }
        }

        #endregion

        #region Title Property

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(string), typeof(WindowX), null);

        public new string Title {
            get { return AppWindow.Title; }
            set { AppWindow.Title = value; }
        }

        #endregion

        #region Width Property

        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register(
            "Width", typeof(int), typeof(WindowX), null);

        public int Width {
            get { return AppWindow.Size.Width; }
            set {
                AppWindow.Resize(new Windows.Graphics.SizeInt32 { Width = (int)(value * Scale), Height = AppWindow.Size.Height });
            }
        }

        #endregion

        #region Height Property

        public static readonly DependencyProperty HeightProperty = DependencyProperty.Register(
            "Height", typeof(int), typeof(WindowX), null);

        public int Height {
            get { return AppWindow.Size.Height; }
            set {
                AppWindow.Resize(new Windows.Graphics.SizeInt32 { Width = AppWindow.Size.Width, Height = (int)(value * Scale) });
            }
        }

        #endregion

        #region Window Startup Location Property

        public static readonly DependencyProperty WindowStartupLocationProperty = DependencyProperty.Register(
            "WindowStartupLocation", typeof(WindowStartupLocation), typeof(WindowX), null);

        public WindowStartupLocation WindowStartupLocation {
            set {
                if (value == WindowStartupLocation.CenterScreen) {
                    DisplayArea displayArea = DisplayArea.GetFromWindowId(WindowId, DisplayAreaFallback.Nearest);
                    if (displayArea != null) {
                        var center = AppWindow.Position;
                        center.X = (displayArea.WorkArea.Width - AppWindow.Size.Width) / 2;
                        center.Y = (displayArea.WorkArea.Height - AppWindow.Size.Height) / 2;
                        AppWindow.Move(center);
                    }

                }
            }
        }

        #endregion

        #region WindowState Property

        public static readonly DependencyProperty WindowStateProperty = DependencyProperty.Register(
            "WindowState", typeof(WindowState), typeof(WindowX), null);

        private WindowState windowState = WindowState.Normal;
        public WindowState WindowState {
            get {
                return windowState;
            }
            set {
                windowState = value;
                try {
                    switch (value) {
                        case WindowState.Maximized:
                            PInvoke.User32.ShowWindow(Handle, PInvoke.User32.WindowShowStyle.SW_MAXIMIZE);
                            break;
                        case WindowState.Minimized:
                            PInvoke.User32.ShowWindow(Handle, PInvoke.User32.WindowShowStyle.SW_MINIMIZE);
                            break;
                            // case WindowState.Normal:
                            //    PInvoke.User32.ShowWindow(Handle, PInvoke.User32.WindowShowStyle.SW_RESTORE);
                            //    break;
                    }
                } catch {

                }
            }
        }

        #endregion

        #region ResizeMode Property

        public static readonly DependencyProperty ResizeModeProperty = DependencyProperty.Register(
            "ResizeMode", typeof(ResizeMode), typeof(WindowX), null);

        private ResizeMode resizeMode = ResizeMode.CanResizeWithGrip;
        public ResizeMode ResizeMode {
            get { return resizeMode; }
            set {
                resizeMode = value;
                switch (value) {
                    case ResizeMode.NoResize:
                        Presenter.IsResizable = false;
                        Presenter.IsMinimizable = false;
                        Presenter.IsMaximizable = false;
                        break;
                    case ResizeMode.CanMinimize:
                        Presenter.IsResizable = false;
                        Presenter.IsMinimizable = true;
                        Presenter.IsMaximizable = false;
                        break;
                    case ResizeMode.CanResize:
                    case ResizeMode.CanResizeWithGrip:
                        Presenter.IsResizable = true;
                        Presenter.IsMinimizable = true;
                        Presenter.IsMaximizable = true;
                        break;
                }
            }
        }

        #endregion

        #region WindowStyle Property

        public static readonly DependencyProperty WindowStyleProperty = DependencyProperty.Register(
            "WindowStyle", typeof(WindowStyle), typeof(WindowX), null);
        private WindowStyle windowStyle = WindowStyle.SingleBorderWindow;

        public WindowStyle WindowStyle {
            get { return windowStyle; }
            set {
                windowStyle = value;
                switch (value) {
                    case WindowStyle.None:
                        Presenter.SetBorderAndTitleBar(false, false);
                        break;
                    case WindowStyle.SingleBorderWindow:
                    case WindowStyle.ThreeDBorderWindow:
                        Presenter.SetBorderAndTitleBar(true, true);
                        break;
                    case WindowStyle.ToolWindow:
                        Presenter.SetBorderAndTitleBar(true, true);
                        ResizeMode = ResizeMode.NoResize;
                        break;
                }
            }
        }

        #endregion

        #region Icon Property
        private Icon iconObj;
        private string icon;

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            "Icon", typeof(string), typeof(WindowX), null);

        public string Icon {
            set {
                if (value != icon) {
                    try {
                        if (value == "application" || value == "app") {
                            string exe = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                            iconObj = System.Drawing.Icon.ExtractAssociatedIcon(exe);
                            AppWindow.SetIcon(Win32Interop.GetIconIdFromIcon(iconObj.Handle));
                        }
                        else if (value != null && value.StartsWith("abs:")) {
                            AppWindow.SetIcon(value[4..]);
                        }
                        else if (value != null && value.StartsWith("rel:")) {
                            AppWindow.SetIcon(AppContext.BaseDirectory + value[4..]);
                        }
                        else {
                            Assembly assembly = Assembly.GetEntryAssembly();
                            var res = assembly.GetManifestResourceNames().FirstOrDefault(s => s.Equals(value, StringComparison.InvariantCultureIgnoreCase));
                            iconObj = new Icon(assembly.GetManifestResourceStream(res));
                            AppWindow.SetIcon(Win32Interop.GetIconIdFromIcon(iconObj.Handle));
                        }                        
                        icon = value;
                    } catch (Exception) { }
                }
            }
            get => icon;
        }
        #endregion
    }
}
