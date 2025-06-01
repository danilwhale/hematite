using System.Diagnostics;
using Hematite.ContentSystem;
using Hematite.Rendering;
using Hematite.Resources;
using Serilog;
using Serilog.Core;
using Silk.NET.OpenGL;

namespace Hematite;

// global variables shared across the framework and your game
public static class Hem
{
    private static GL? _gl;

    public static GL Gl
    {
        get
        {
            Debug.Assert(_gl is not null);
            return _gl;
        }
        set
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            int majorVersion = value.GetInteger(GetPName.MajorVersion);
            int minorVersion = value.GetInteger(GetPName.MinorVersion);
            // atm we support only 4.5
            // todo add support for 3.2
            if (majorVersion < 4 || minorVersion < 5)
            {
                throw new ArgumentException("Provided OpenGL context must support at least OpenGL 4.5");
            }
            
            // now we're good
            _gl = value;
        }
    }

    public static void DisposeAll()
    {
        if (_gl is null) return;
        if (ImmediateRenderer.Instance.IsValueCreated) ImmediateRenderer.Instance.Value.Dispose();
        if (Texture2D.WhiteTexture.IsValueCreated) Texture2D.WhiteTexture.Value.Dispose();
        if (Effect.Simple.IsValueCreated) Effect.Simple.Value.Dispose();
        Content.DisposeAll();
        _gl.Dispose();
    }

    public static void PrintGlInformation()
    {
        if (Log.Logger != Logger.None)
        {
            Log.Information("renderer ({0} extension(s)):", Gl.GetInteger(GetPName.NumExtensions));
            Log.Information(" - name: {0}", Gl.GetStringS(StringName.Renderer));
            Log.Information(" - vendor: {0}", Gl.GetStringS(StringName.Vendor));
            Log.Information(" - version: {0}", Gl.GetStringS(StringName.Version));
            Log.Information(" - glsl version: {0}", Gl.GetStringS(StringName.ShadingLanguageVersion));
            return;
        }
        
        // we don't have logger bound, print information into the console
        Console.WriteLine("renderer ({0} extension(s)):", Gl.GetInteger(GetPName.NumExtensions));
        Console.WriteLine(" - name: {0}", Gl.GetStringS(StringName.Renderer));
        Console.WriteLine(" - vendor: {0}", Gl.GetStringS(StringName.Vendor));
        Console.WriteLine(" - version: {0}", Gl.GetStringS(StringName.Version));
        Console.WriteLine(" - glsl version: {0}", Gl.GetStringS(StringName.ShadingLanguageVersion));
    }
}