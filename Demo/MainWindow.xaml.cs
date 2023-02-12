// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Windowing;

namespace Demo
{
    /// <summary>
    /// Inherit from Pyther.WindowX.WindowX
    /// </summary>
    public sealed partial class MainWindow : Pyther.WindowX.WindowX
    {
        public MainWindow() {
            this.InitializeComponent();

            // You can set all properties using XAML or via code.
            /*
            this.Title = "A Pyther.WindowX Example";
            this.Icon = "Demo.Assets.window.ico";
            this.Width = 800;
            this.Height = 500;
            this.WindowStartupLocation = Pyther.WindowX.WindowStartupLocation.CenterScreen;
            this.WindowStyle = Pyther.WindowX.WindowStyle.ToolWindow;
            */
        }
    }
}
