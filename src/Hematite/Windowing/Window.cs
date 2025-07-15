using Hematite.Graphics;
using Vortice.Mathematics;

namespace Hematite.Windowing;

public abstract class Window : IDisposable
{
    public static Window? Current { get; set; }

    public static Window Make(ref readonly WindowDescriptor descriptor)
    {
        if (Platform.Current is null)
        {
            throw new InvalidOperationException("Platform has not been initialized or no platforms were registered");
        }
        
        ArgumentException.ThrowIfNullOrEmpty(descriptor.Title);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(descriptor.Size.Width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(descriptor.Size.Height);
        
        // TODO use appropriate api for each os (except for mac :tf:)
        WindowDescriptor newDescriptor = descriptor with
        {
            Api = DriverApi.OpenGl
        };
        return Platform.Current.MakeWindow(in newDescriptor);
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

    public abstract event Action<Int2>? Resized;
    public abstract event Action<Int2>? Moved; 
    
    public void MakeCurrent() => Current = this;
    public void Close() => ShouldClose = true;

    public abstract void Update();

    public abstract void Dispose();
}