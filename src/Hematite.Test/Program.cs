using System.Numerics;
using Hematite.Graphics;
using Hematite.Input;
using Hematite.Windowing;
using Vortice.Mathematics;
using static Hematite.hmLib;

if (!hmTryInitialize()) return 1;

hmWindowDescriptor descriptor = new()
{
    Title = "test",
    Size = new SizeI(854, 480),
    Border = hmWindowBorder.Resizable
};

hmWindow window = hmWindowSetCurrent(hmMakeWindow(in descriptor)!);

hmWindowSetMinSize(null, new SizeI(854, 480));

hmEffect? effect = hmEffectLoad(null,
    """
    #version 330 core

    layout (location = 0) in vec3 vPosition;
    layout (location = 1) in vec4 vColor;

    out vec4 fColor;

    void main()
    {
        gl_Position = vec4(vPosition, 1.0);
        fColor = vColor;
    }
    """,
    """
    #version 330 core

    out vec4 oColor;

    in vec4 fColor;

    void main()
    {
        oColor = fColor;
    }
    """
);
if (effect == null) return 1;

Span<Vertex> vertices =
[
    new(new Vector3(0.0f, 0.25f, 0), new Color(255, 0, 0)),
    new(new Vector3(0.25f, -0.25f, 0), new Color(0, 255, 0)),
    new(new Vector3(-0.25f, -0.25f, 0), new Color(0, 0, 255))
];
hmBuffer? vertexBuffer = hmMakeBuffer<Vertex>(null, (uint)vertices.Length);
if (vertexBuffer == null) return 1;
if (hmBufferTryLock(vertexBuffer, 0, vertexBuffer.SizeInBytes, hmBufferAccess.WriteOnly, out hmBufferData? data))
{
    vertices.CopyTo(data.AsSpan<Vertex>());
    hmBufferTryUnlock(vertexBuffer);
}

hmMesh? mesh = hmMakeMesh(null);
if (mesh == null) return 1;
hmMeshAttachVertexBuffer(mesh, vertexBuffer);
hmMeshAttachVertexFormat(mesh, new hmVertexFormat(
    new hmVertexFormatElement(hmVertexFormatElementType.Float, false, 3),
    new hmVertexFormatElement(hmVertexFormatElementType.UnsignedByte, true, 4)
));

hmInputSetMouseLocked(null, true);

while (!hmWindowShouldClose(null))
{
    hmWindowSetTitle(null, hmInputGetMouseVelocity(null).ToString());
    if (hmInputIsKeyJustPressed(null, hmKeyCode.Escape))
    {
        hmInputSetMouseLocked(null, !hmInputIsMouseLocked(null));
    }
    
    hmWindowClearColor(null, new Color(0.2f, 0.4f, 0.8f));
    hmEffectUse(effect);
    hmMeshRender(mesh, hmPrimitiveTopology.TriangleList, 0, 3);
    hmUpdate();
}

hmDestroyMesh(mesh);
hmDestroyBuffer(vertexBuffer);
hmDestroyEffect(effect);
hmDestroyWindow(window);
hmDestroy();
return 0;

readonly struct Vertex(Vector3 position, Color color)
{
    public readonly Vector3 Position = position;
    public readonly Color Color = color;
}