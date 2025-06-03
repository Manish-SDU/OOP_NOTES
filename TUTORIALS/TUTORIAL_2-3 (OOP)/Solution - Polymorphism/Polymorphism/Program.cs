using Avalonia;
using System;

namespace Polymorphism;

class Program
{
    
    
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        if (args.Length > 0 && args[0] == "GUI")
        {
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
        else
        {
            Polymorphism.ShapeDriver.Main(args);
        }
    }
       

    // Avalonia configuration, don't remove; also used by visual designer.
   
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}