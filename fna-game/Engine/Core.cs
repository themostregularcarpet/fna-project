using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Videogame.Engine.Input;
using Videogame.Engine.ASS;
using Videogame.Engine.UI;
using Videogame.Engine.Screen;

public class Core : Game 
{
    internal static Core s_instance;
    public static Core Instance => s_instance;

    public static GraphicsDeviceManager Graphics { get; private set; }
    public static new GraphicsDevice GraphicsDevice { get; private set; }
    public static SpriteBatch SpriteBatch { get; private set; }
    public static ContentManager Content { get; private set; }
    public static Camera Camera { get; private set; }
    public static Input Input { get; private set; }
    public static Fonts Fonts { get; private set; }

    private static Scene s_activeScene;
    private static Scene s_nextScene;

    private const int LOGICAL_WIDTH = 320;
    private const int LOGICAL_HEIGHT = 180;
    private const int UI_LOGICAL_WIDTH = 1280;
    private const int UI_LOGICAL_HEIGHT = 720;

    private ScreenRenderer screenRenderer;

    public Core(int width, int height, string title, bool isFullscreen, bool isVsync, bool isBorderless)
    {
        if (s_instance != null)
            throw new InvalidOperationException("only one core instance is allowed!");

        s_instance = this;

        Graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = width,
            PreferredBackBufferHeight = height,
            IsFullScreen = isFullscreen,
            SynchronizeWithVerticalRetrace = isVsync,
        };

        Graphics.DeviceReset += OnDeviceReset;

        Window.Title = title;
        Window.AllowUserResizing = true;
		Window.IsBorderlessEXT = isBorderless;
        Window.ClientSizeChanged += OnClientSizeChanged;

        SDL_SetWindowMinimumSize(Window.Handle, LOGICAL_WIDTH, LOGICAL_HEIGHT);

        Content = base.Content;
        Content.RootDirectory = "Content";

        IsFixedTimeStep = false;
        IsMouseVisible = true;
    }

    [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)]
    private static extern bool SDL_SetWindowMinimumSize(IntPtr window, int minWidth, int minHeight);

    protected override void Initialize()
    {
        GraphicsDevice = base.GraphicsDevice;
        SpriteBatch = new SpriteBatch(GraphicsDevice);
        Input = new Input();
        Fonts = new Fonts();
        Fonts.Load();
        Camera = new Camera(new Vector2(LOGICAL_WIDTH / 2f, LOGICAL_HEIGHT / 2f));

        base.Initialize();

        screenRenderer = new ScreenRenderer(LOGICAL_WIDTH, LOGICAL_HEIGHT, UI_LOGICAL_WIDTH, UI_LOGICAL_HEIGHT);
    }

    protected override void Update(GameTime gameTime)
    {
        Input.Update(gameTime);

        if (Input.Keyboard.WasKeyPressed(Controls.Fullscreen))
        {
            Graphics.ToggleFullScreen();
        }

        if (s_nextScene != null)
            TransitionScene();

        s_activeScene?.Update(gameTime);
        UiManager.UpdateUi(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.SetRenderTarget(screenRenderer.GameRenderTarget);
        GraphicsDevice.Clear(Color.Black);

        // in-game object related drawings
        SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, Camera.GetViewMatrix());
        s_activeScene?.Draw();
        SpriteBatch.End();

        GraphicsDevice.SetRenderTarget(null);

        GraphicsDevice.Clear(Color.Black);

        // render target related drawings
        SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null);
        SpriteBatch.Draw(screenRenderer.GameRenderTarget, new Rectangle(screenRenderer.OffsetX, screenRenderer.OffsetY, screenRenderer.RenderWidth, screenRenderer.RenderHeight), Color.White);
        SpriteBatch.End();

        GraphicsDevice.ScissorRectangle = screenRenderer.UiScissorRectangle;
        
        // ui related drawings
        SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearClamp, null, ScreenRenderer.ScissorState, null, screenRenderer.UiTransformMatrix);
        UiManager.DrawUi();
        SpriteBatch.End();

        base.Draw(gameTime);
    }

    public static void ChangeScene(Scene scene)
    {
        if (s_activeScene != scene)
        {
            s_activeScene = scene;
            s_activeScene?.Initialize();
        }
    }

    public static void TransitionScene()
    {
        s_activeScene?.Dispose();
        GC.Collect();
        s_activeScene = s_nextScene;
        s_nextScene = null;
        s_activeScene?.Initialize();
    }

    private void OnDeviceReset(object sender, EventArgs e) => screenRenderer?.UpdateViewport();
    private void OnClientSizeChanged(object sender, EventArgs e) => screenRenderer?.UpdateViewport();
}
