using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Videogame.Engine.Input;
using Videogame.Engine.ASS;
using Videogame.Engine.Audio;
using Videogame.Engine.Camera;

public class Core : Game 
{
    internal static Core s_instance;
    public static Core Instance => s_instance;

    public static GraphicsDeviceManager Graphics { get; private set; }
    public static new GraphicsDevice GraphicsDevice { get; private set; }
    public static SpriteBatch SpriteBatch { get; private set; }
    public static ContentManager Content { get; private set; }
    public static Input Input { get; private set; }
    public static Camera Camera { get; private set; }

    private static Scene s_activeScene;
    private static Scene s_nextScene;

    private const int LOGICAL_WIDTH = 320;
    private const int LOGICAL_HEIGHT = 180;
    private bool isResizing = false;
    private RenderTarget2D gameRenderTarget;
    private int scale, offsetX, offsetY, renderWidth, renderHeight;

    public Core(int width, int height, string title, bool isFullscreen, bool isVsync, bool isBorderless)
    {
        if (s_instance != null)
            throw new InvalidOperationException("only one core instance is allowed!");

        s_instance = this;

        Graphics = new GraphicsDeviceManager(this);
        Graphics.PreferredBackBufferWidth = width;
        Graphics.PreferredBackBufferHeight = height;
        Graphics.IsFullScreen = isFullscreen;
        Graphics.SynchronizeWithVerticalRetrace = isVsync;
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
        AudioManager.Initialize();
        Camera = new Camera(new Vector2(LOGICAL_WIDTH / 2f, LOGICAL_HEIGHT / 2f));

        base.Initialize();

        gameRenderTarget = new RenderTarget2D(GraphicsDevice, LOGICAL_WIDTH, LOGICAL_HEIGHT);
        ApplyIntScaling();
    }

    protected override void Update(GameTime gameTime)
    {
        Input.Update(gameTime);
        AudioManager.Update();

        if (Input.Keyboard.WasKeyPressed(Controls.Fullscreen))
        {
            Graphics.IsFullScreen = !Graphics.IsFullScreen;
			Graphics.ApplyChanges();
        }

        if (s_nextScene != null)
            TransitionScene();

        s_activeScene?.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.SetRenderTarget(gameRenderTarget);
        GraphicsDevice.Clear(Color.Black);

        SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, Camera.GetViewMatrix());
        s_activeScene?.Draw();
        SpriteBatch.End();

        GraphicsDevice.SetRenderTarget(null);

        GraphicsDevice.Clear(Color.Black);

        SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null);
        SpriteBatch.Draw(gameRenderTarget, new Rectangle(offsetX, offsetY, renderWidth, renderHeight), Color.White);
        SpriteBatch.End();

        // SpriteBatch.Begin(...);
        // ... UI SHIT ...
        // SpriteBatch.End();

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

    private void OnDeviceReset(object sender, EventArgs e) => ApplyIntScaling();
    private void OnClientSizeChanged(object sender, EventArgs e) => ApplyIntScaling();

    private void ApplyIntScaling()
    {
        if (isResizing) return;
        isResizing = true;

        int windowWidth = Window.ClientBounds.Width;
        int windowHeight = Window.ClientBounds.Height;

        int scaleX = windowWidth / LOGICAL_WIDTH;
        int scaleY = windowHeight / LOGICAL_HEIGHT;
        int scaleXY = Math.Max(1, Math.Min(scaleX, scaleY));

        scale = scaleXY;
        renderWidth = LOGICAL_WIDTH * scale;
        renderHeight = LOGICAL_HEIGHT * scale;
        offsetX = (windowWidth - renderWidth) / 2;
        offsetY = (windowHeight - renderHeight) / 2;

        isResizing = false;
    }

    protected override void UnloadContent()
    {
        base.UnloadContent();
        AudioManager.Dispose();
    }
}