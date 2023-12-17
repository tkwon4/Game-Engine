
using CPI311.GameEngine;

namespace Lab2;

public class Lab2 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // **** Lab02 new items
    //Sprite sprite;
    //KeyboardState prevState;
    //float radius;
    //float speed;
    //float angle;

    SpiralMover spiralMover; //Lab2 Solution

    public Lab2()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        //prevState = Keyboard.GetState();
        InputManager.Initialize();
        Time.Initialize();


        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here

        //sprite = new Sprite(Content.Load<Texture2D>("Square"));
        spiralMover = new SpiralMover(Content.Load<Texture2D>("Square"), Vector2.Zero); //other properties are set as default
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        //sprite.Position = new Vector2(GraphicsDevice.Viewport.Width/2, GraphicsDevice.Viewport.Height/2);

        /*KeyboardState currentState = Keyboard.GetState();
        if (currentState.IsKeyDown(Keys.Right) && prevState.IsKeyUp(Keys.Right))
        {
            sprite.Position += Vector2.UnitX * 5;
        }
        if (currentState.IsKeyDown(Keys.Left) && prevState.IsKeyUp(Keys.Left))
        {
            sprite.Position -= Vector2.UnitX * 5;
        }
        if (currentState.IsKeyDown(Keys.Up) && prevState.IsKeyUp(Keys.Up))
        {
            sprite.Position -= Vector2.UnitY * 5;
        }
        if (currentState.IsKeyDown(Keys.Down) && prevState.IsKeyUp(Keys.Down))
        {
            sprite.Position += Vector2.UnitY * 5;
        }
        prevState = currentState;*/

        
        InputManager.Update();
        Time.Update(gameTime);

        spiralMover.Update();

        /*if (InputManager.IsKeyDown(Keys.A))
            sprite.Position += Vector2.UnitX * -5;
        if (InputManager.IsKeyDown(Keys.D))
            sprite.Position += Vector2.UnitX * 5;
        if (InputManager.IsKeyDown(Keys.W))
            sprite.Position += Vector2.UnitY * -5;
        if (InputManager.IsKeyDown(Keys.S))
            sprite.Position += Vector2.UnitY * 5;
        if (InputManager.IsKeyDown(Keys.Space))
            sprite.Rotation += 0.05f;*/

        //spiral radius/speed control
        /*if (InputManager.IsKeyDown(Keys.Left))
            if(radius <= 1f)
            {
                radius = 1f;
            }
            else
            {
                radius -= 1f;
            }
        if (InputManager.IsKeyDown(Keys.Right))
            radius += 1f;
        if (InputManager.IsKeyDown(Keys.Up))
            speed += 0.1f;
        if (InputManager.IsKeyDown(Keys.Down))
            if(speed <= 1f)
            {
                speed = 1f;
            }
            else
            {
                speed -= 0.1f;
            }


        angle += Time.ElapsedGameTime * speed;

        //sprial movement
        //sprite.Position += new Vector2((float)(radius * Math.Cos(angle)), (float)(radius * Math.Sin(angle)));
        //sprite.Position += new Vector2((float)(radius * angle * Math.Cos(angle)), (float)(radius * angle * Math.Sin(angle)));
        sprite.Position += new Vector2((float)((radius + 10 * Math.Cos(10 * angle)) * Math.Cos(angle)), (float)((radius + 10 * Math.Cos(10 * angle)) * Math.Sin(angle)));*/


        //base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        _spriteBatch.Begin();
        spiralMover.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}

