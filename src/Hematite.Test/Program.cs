using Hematite;
using Vortice.Mathematics;
using static Hematite.hmLib;

if (!hmTryInitialize()) return 1;

hmWindowDescriptor descriptor = new()
{
    Title = "test",
    Size = new SizeI(854, 480),
    Border = hmWindowBorder.Resizable
};

hmContext context = hmMakeContext();
context.Window = hmMakeWindow(in descriptor);
hmContextSetCurrent(context);

hmWindowSetMinSize(null, new SizeI(854, 480));

while (!hmWindowShouldClose(null))
{
    hmWindowSetTitle(null, hmWindowGetPosition(null).ToString());
    hmWindowSetOpacity(null, MathF.Sin(SDL.SDL3.SDL_GetTicks() * 0.001f) * 0.4f + 0.6f);
    hmUpdate();
}

hmDestroyContext(context);
hmDestroy();
return 0;