using Silk.NET.OpenGL;

namespace Hematite.Resources;

public sealed class Texture2D : IDisposable
{
    public static readonly Lazy<Texture2D> WhiteTexture = 
        new(() => new Texture2D(Hem.Gl, [255, 255, 255, 255], 1, 1));
    
    public readonly GL Gl;
    public readonly uint Id;
    public uint Width;
    public uint Height;

    public Texture2D(
        GL gl,
        ReadOnlySpan<byte> pixels, uint width, uint height,
        SizedInternalFormat internalFormat = SizedInternalFormat.Rgba8, PixelFormat format = PixelFormat.Rgba,
        TextureWrapMode wrapMode = TextureWrapMode.Repeat,
        TextureMinFilter minFilter = TextureMinFilter.Nearest,
        TextureMagFilter magFilter = TextureMagFilter.Nearest)
    {
        Gl = gl;
        Id = gl.CreateTexture(TextureTarget.Texture2D);
        
        gl.TextureParameterI(Id, TextureParameterName.TextureWrapS, (int)wrapMode);
        gl.TextureParameterI(Id, TextureParameterName.TextureWrapT, (int)wrapMode);
        gl.TextureParameterI(Id, TextureParameterName.TextureMagFilter, (int)magFilter);
        gl.TextureParameterI(Id, TextureParameterName.TextureMinFilter, (int)minFilter);
        
        uint levels = (uint)(1 + (int)Math.Floor(Math.Log(Math.Max(width, height), 2)));
        gl.TextureStorage2D(Id, levels, internalFormat, width, height);
        gl.TextureSubImage2D(Id, 0, 0, 0, width, height, format, PixelType.UnsignedByte, pixels);

        Width = width;
        Height = height;
    }

    public void Bind(uint unit = 0)
    {
        Gl.BindTextureUnit(unit, Id);
    }
    
    public void Dispose()
    {
        Gl.DeleteTexture(Id);
    }
}