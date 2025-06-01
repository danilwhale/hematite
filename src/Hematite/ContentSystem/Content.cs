using Hematite.Resources;
using ReFuel.Stb;
using Silk.NET.OpenGL;

namespace Hematite.ContentSystem;

public static class Content
{
    public const string RootPath = "Content";
    public const string EffectsSubDirectory = "Effects";
    public const string TexturesSubDirectory = "Textures";
    
    private static readonly Dictionary<string, Texture2D> Textures = [];
    private static readonly Dictionary<ValueTuple<string, string>, Effect> Effects = [];

    public static Texture2D GetTexture(GL? gl, string assetName)
    {
        if (Textures.TryGetValue(assetName, out Texture2D? value))
        {
            return value;
        }

        using Stream stream = File.OpenRead(Path.Combine(RootPath, TexturesSubDirectory, assetName));
        using StbImage image = StbImage.Load(stream, StbiImageFormat.Rgba);
        return Textures[assetName] = new Texture2D(gl ?? Hem.Gl, image.AsSpan<byte>(), (uint)image.Width, (uint)image.Height);
    }

    public static Effect GetEffect(GL? gl, string vertexShaderAssetName, string fragmentShaderAssetName)
    {
        if (Effects.TryGetValue((vertexShaderAssetName, fragmentShaderAssetName), out Effect? value))
        {
            return value;
        }

        string vertexSource = File.ReadAllText(Path.Combine(RootPath, EffectsSubDirectory, vertexShaderAssetName));
        string fragmentSource = File.ReadAllText(Path.Combine(RootPath, EffectsSubDirectory, fragmentShaderAssetName));
        return Effects[(vertexShaderAssetName, fragmentShaderAssetName)] = new Effect(gl ?? Hem.Gl, vertexSource, fragmentSource);
    }

    public static void DisposeAll()
    {
        foreach ((_, Texture2D texture) in Textures) texture.Dispose();
        foreach ((_, Effect effect) in Effects) effect.Dispose();
    }
}