
namespace CPI311.Labs;

public class Lab1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    SpriteFont font; // Lab0 used Texture 2D

    private Fraction a = new Fraction(3, 4);
    private Fraction b = new Fraction(8, 3);


    public Lab1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here

        font = Content.Load<SpriteFont>("Font");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        _spriteBatch.Begin();
        //_spriteBatch.DrawString(font, a + " * " + b + " = " + Fraction.multiply(a, b), new Vector2(100, 100), Color.Black);
        _spriteBatch.DrawString(font, a + " * " + b + " = " + (a*b), new Vector2(100, 100), Color.Black);
        _spriteBatch.DrawString(font, a + " / " + b + " = " + (a / b), new Vector2(100, 150), Color.Black);
        _spriteBatch.DrawString(font, a + " + " + b + " = " + (a + b), new Vector2(100, 200), Color.Black);
        _spriteBatch.DrawString(font, a + " - " + b + " = " + (a - b), new Vector2(100, 250), Color.Black);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}

