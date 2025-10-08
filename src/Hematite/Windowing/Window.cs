using Hematite.Graphics;
using Hematite.Platforms;
using Vortice.Mathematics;

namespace Hematite.Windowing;

public abstract class Window : IDisposable
{
    public static Window? Current { get; set; }

    public static Window Make(ref readonly WindowDescriptor windowDescriptor, ref readonly GraphicsDeviceDescriptor deviceDescriptor)
    {
        if (Platform.Current is null)
        {
            throw new InvalidOperationException("Platform has not been initialized or no platforms were registered");
        }
        
        ArgumentException.ThrowIfNullOrEmpty(windowDescriptor.Title);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(windowDescriptor.Size.Width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(windowDescriptor.Size.Height);
        
        // TODO use appropriate api for each os (except for mac :tf:)
        WindowDescriptor newDescriptor = windowDescriptor with
        {
            Api = DriverApi.OpenGl
        };
        return Platform.Current.MakeWindow(
            in newDescriptor, in deviceDescriptor,
            Driver.GetForApi(newDescriptor.Api) ?? throw new ArgumentException("No driver has been found for the window", nameof(windowDescriptor)));
    }
    
    public abstract nint Handle { get; }
    public abstract bool ShouldClose { get; protected set; }
    public abstract WindowBorder Border { get; set; }
    public abstract WindowState State { get; set; }
    public abstract string? Title { get; set; }
    public abstract SizeI Size { get; set; }
    public abstract SizeI MinSize { get; set; }
    public abstract SizeI MaxSize { get; set; }
    public abstract Int2 Position { get; set; }
    public abstract float Opacity { get; set; }
    public GraphicsDevice GraphicsDevice { get; protected init; }

    public abstract event Action<Int2>? Resized;
    public abstract event Action<Int2>? Moved; 
    
    public void MakeCurrent()
    {
        Current = this;
        GraphicsDevice.MakeCurrent();
    }

    public void Close() => ShouldClose = true;

    public abstract void Update();

    protected virtual void Dispose(bool disposed)
    {
    }

    public void Dispose()
    {
        GraphicsDevice.Dispose();
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}