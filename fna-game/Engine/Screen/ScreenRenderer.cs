using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Videogame.Engine.Screen;

public class ScreenRenderer
{
    private int logicalWidth;
    private int logicalHeight;
    private int uiLogicalWidth;
    private int uiLogicalHeight;
    private int scale, offsetX, offsetY, renderWidth, renderHeight;
    private bool isResizing = false;
    private Matrix uiTransformMatrix;
    private Rectangle uiScissorRectangle;
    
    public int OffsetX => offsetX;
    public int OffsetY => offsetY;
    public int RenderWidth => renderWidth;
    public int RenderHeight => renderHeight;
    public RenderTarget2D GameRenderTarget;
    public Matrix UiTransformMatrix => uiTransformMatrix;
    public Rectangle UiScissorRectangle => uiScissorRectangle;

    public static readonly RasterizerState ScissorState = new RasterizerState
    {
        ScissorTestEnable = true
    };

    public ScreenRenderer(int logWidth, int logHeight, int uiLogWidth = 1280, int uiLogHeight = 720)
    {
        logicalWidth = logWidth;
        logicalHeight = logHeight;
        uiLogicalWidth = uiLogWidth;
        uiLogicalHeight = uiLogHeight;

        GameRenderTarget = new RenderTarget2D(Core.GraphicsDevice, logicalWidth, logicalHeight);
        UpdateViewport();
    }
    
    public void UpdateViewport()
    {
        if (isResizing)
        {
            return;
        }
            
        isResizing = true;

        int windowWidth = Core.GraphicsDevice.PresentationParameters.BackBufferWidth;
        int windowHeight = Core.GraphicsDevice.PresentationParameters.BackBufferHeight;

        int scaleX = windowWidth / logicalWidth;
        int scaleY = windowHeight / logicalHeight;
        scale = Math.Max(1, Math.Min(scaleX, scaleY));

        renderWidth = logicalWidth * scale;
        renderHeight = logicalHeight * scale;
        offsetX = (windowWidth - renderWidth) / 2;
        offsetY = (windowHeight - renderHeight) / 2;

        float uiScaleX = (float)renderWidth / uiLogicalWidth;
        float uiScaleY = (float)renderHeight / uiLogicalHeight;
        float uiScale = Math.Min(uiScaleX, uiScaleY);

        int uiRenderWidth = (int)(uiLogicalWidth * uiScale);
        int uiRenderHeight = (int)(uiLogicalHeight * uiScale);
        int uiOffsetX = offsetX + (renderWidth - uiRenderWidth) / 2;
        int uiOffsetY = offsetY + (renderHeight - uiRenderHeight) / 2;

        uiTransformMatrix = Matrix.CreateScale(uiScale, uiScale, 1) * Matrix.CreateTranslation(uiOffsetX, uiOffsetY, 0);
        uiScissorRectangle = new Rectangle(offsetX, offsetY, renderWidth, renderHeight);

        isResizing = false;
    }
}
    