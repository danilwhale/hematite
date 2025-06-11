using Hematite.Windowing;
using Vortice.Mathematics;

namespace Hematite.Platforms;

internal interface hmIPlatform
{
    bool TryInitialize();
    void Update();
    void Destroy();
    
    hmWindow? MakeWindow(ref readonly hmWindowDescriptor descriptor);
    bool WindowShouldClose(hmWindow window);
    void WindowUpdate(hmWindow window);
    hmWindowBorder WindowGetBorder(hmWindow window);
    void WindowSetBorder(hmWindow window, hmWindowBorder border);
    hmWindowState WindowGetState(hmWindow window);
    void WindowSetState(hmWindow window, hmWindowState state);
    string? WindowGetTitle(hmWindow window);
    void WindowSetTitle(hmWindow window, string? title);
    SizeI WindowGetSize(hmWindow window);
    void WindowSetSize(hmWindow window, SizeI size);
    SizeI WindowGetMinSize(hmWindow window);
    void WindowSetMinSize(hmWindow window, SizeI minSize);
    SizeI WindowGetMaxSize(hmWindow window);
    void WindowSetMaxSize(hmWindow window, SizeI maxSize);
    Int2 WindowGetPosition(hmWindow window);
    void WindowSetPosition(hmWindow window, Int2 position);
    float WindowGetOpacity(hmWindow window);
    void WindowSetOpacity(hmWindow window, float opacity);
    void DestroyWindow(hmWindow window);
}