using System.Numerics;
using Hematite;
using Hematite.ContentSystem;
using Hematite.Rendering;
using Hematite.Resources;
using Serilog;
using SDL;
using Silk.NET.OpenGL;
using Vortice.Mathematics;
using static SDL.SDL3;

unsafe
{
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Console()
        .CreateLogger();

    if (!SDL_InitSubSystem(SDL_InitFlags.SDL_INIT_VIDEO))
    {
        Log.Fatal("failed to initialize SDL: {0}", SDL_GetError());
        return 1;
    }

    SDL_Window* window = SDL_CreateWindow(
        "hematite",
        854, 480,
        SDL_WindowFlags.SDL_WINDOW_RESIZABLE | SDL_WindowFlags.SDL_WINDOW_OPENGL
    );
    if (window == null)
    {
        Log.Fatal("failed to create the window: {0}", SDL_GetError());
        return 1;
    }

    SDL_GL_SetAttribute(SDL_GLAttr.SDL_GL_ACCELERATED_VISUAL, 1);
    SDL_GL_SetAttribute(SDL_GLAttr.SDL_GL_DOUBLEBUFFER, 1);
    SDL_GL_SetAttribute(SDL_GLAttr.SDL_GL_DEPTH_SIZE, 24);
    SDL_GL_SetAttribute(SDL_GLAttr.SDL_GL_STENCIL_SIZE, 8);
    SDL_GL_SetAttribute(SDL_GLAttr.SDL_GL_CONTEXT_MAJOR_VERSION, 4);
    SDL_GL_SetAttribute(SDL_GLAttr.SDL_GL_CONTEXT_MINOR_VERSION, 5);
    SDL_GL_SetAttribute(SDL_GLAttr.SDL_GL_CONTEXT_PROFILE_MASK, SDL_GL_CONTEXT_PROFILE_CORE);

    SDL_GLContextState* glContext = SDL_GL_CreateContext(window);
    if (glContext == null)
    {
        Log.Fatal("failed to create OpenGL context: {0}", SDL_GetError());
        return 1;
    }

    SDL_GL_MakeCurrent(window, glContext);
    SDL_GL_SetSwapInterval(1);

    GL gl = GL.GetApi(p => SDL_GL_GetProcAddress(p));
    Hem.Gl = gl;
    Hem.PrintGlInformation();

    Effect effect = Effect.Simple.Value;

    int width, height;
    SDL_GetWindowSizeInPixels(window, &width, &height);

    bool shouldClose = false;
    while (!shouldClose)
    {
        SDL_Event ev;
        while (SDL_PollEvent(&ev))
        {
            switch (ev.Type)
            {
                case SDL_EventType.SDL_EVENT_QUIT:
                    shouldClose = true;
                    break;
                case SDL_EventType.SDL_EVENT_WINDOW_RESIZED:
                    width = ev.window.data1;
                    height = ev.window.data2;
                    gl.Viewport(0, 0, (uint)width, (uint)height);
                    break;
            }
        }

        gl.ClearColor(0.2f, 0.4f, 0.6f, 1.0f);
        gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        effect.SetValue(effect.GetLocation("Transform"),
            Matrix4x4.CreateRotationZ(SDL_GetTicks() * 0.001f) *
            Matrix4x4.CreateTranslation(width * 0.5f, height * 0.5f, 0) *
            Matrix4x4.CreateOrthographicOffCenter(0, width, height, 0, 0, 1000)
        );

        ImmediateRenderer.Instance.Value.RenderTri(
            new Vector2(0, -128),
            new Vector2(-128, 128),
            new Vector2(128, 128),
            new Color(255, 255, 255)
        );

        SDL_GL_SwapWindow(window);
    }

    Hem.DisposeAll();
    SDL_GL_DestroyContext(glContext);
    SDL_DestroyWindow(window);
    SDL_QuitSubSystem(SDL_InitFlags.SDL_INIT_VIDEO);
}

return 0;