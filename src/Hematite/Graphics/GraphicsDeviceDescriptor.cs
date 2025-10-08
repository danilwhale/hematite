namespace Hematite.Graphics;

public readonly record struct GraphicsDeviceDescriptor(
    PixelFormat PixelFormat,
    DepthFormat DepthFormat,
    int SampleCount,
    int SampleBuffersCount,
    bool VSync
);