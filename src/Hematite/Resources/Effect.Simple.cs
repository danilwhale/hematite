namespace Hematite.Resources;

public sealed partial class Effect
{
    public static readonly Lazy<Effect> Simple = new(() => new Effect(Hem.Gl,
        """
        #version 450 core
        
        layout (location = 0) in vec3 vPosition;
        layout (location = 1) in vec2 vTexCoord;
        layout (location = 2) in vec4 vColor;
        
        out VS_OUT {
            vec2 TexCoord;
            vec4 Color;
        } f;
        
        layout (std140, binding = 0) uniform Camera {
            mat4 View;
            mat4 Projection;
        };
        
        uniform mat4 World;
        
        void main() {
            gl_Position = Projection * View * World * vec4(vPosition, 1.0);
            f.TexCoord = vTexCoord;
            f.Color = vColor;
        }
        """,
        """
        #version 450 core
        
        layout (location = 0) out vec4 oColor;
        
        in VS_OUT {
            vec2 TexCoord;
            vec4 Color;
        } f;
        
        layout (location = 0) uniform sampler2D Texture;
        
        void main() {
            oColor = texture(Texture, f.TexCoord) * f.Color;
            if (oColor.a < 0.1) discard;
        }
        """
    ));
}