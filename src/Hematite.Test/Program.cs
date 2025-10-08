using Hematite;
using Hematite.Graphics;
using Hematite.Graphics.OpenGl;
using Hematite.Windowing;
using Hematite.Windowing.SDL;
using Vortice.Mathematics;

SdlPlatform.Register();
OpenGLDriver.Register();

// try to force SDL3 platform (in case you're registering several platforms)
Platform.SetTarget(SdlPlatform.Identifier);

WindowDescriptor windowDescriptor = new(
    Title: "test", 
    Size: new SizeI(640, 480),
    Border: WindowBorder.Resizable);
GraphicsDeviceDescriptor deviceDescriptor = new(
    PixelFormat: PixelFormat.Rgba8,
    DepthFormat: DepthFormat.D0,
    SampleCount: 0,
    SampleBuffersCount: 0,
    VSync: true);

using Window window = Window.Make(in windowDescriptor, in deviceDescriptor);
window.MakeCurrent();

GraphicsDevice device = window.GraphicsDevice;

while (!window.ShouldClose)
{
    device.Clear(ClearDeviceMask.Color, color: Colors.CornflowerBlue);
    window.Update();
}