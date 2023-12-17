using CPI311.GameEngine;

namespace Lab3;

public class Lab3 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // *** Lab3
    Model model;
    // *** Matrix for 3D
    Matrix world;
    Matrix view;
    Matrix projection;
    // *** Camera Data
    Vector3 cameraPos = new Vector3(0, 0, 5);
    Vector3 modelPos = new Vector3(0, 0, 0);
    // *** Change World Mode
    bool mode = true;
    // *** Change Persepctive Mode
    bool perspective = true;
    // *** Yaw Pitch Roll
    float yaw = 0;
    float pitch = 0;
    float roll = 0;
    // *** scale
    float scale = 1.0f;
    // *** Project width height
    float width = 0.5f;
    float height = 0.5f;

    String cameraMode = "";
    String matrixOrder = "";

    float horizontalOffSet = 0;
    float verticalOffSet = 0;

    SpriteFont font;

    public Lab3()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        InputManager.Initialize();
        Time.Initialize();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here

        model = Content.Load<Model>("Torus");

        font = Content.Load<SpriteFont>("Font");

        // *** Add light effect
        foreach (ModelMesh mesh in model.Meshes)
        {
            foreach(BasicEffect effect in mesh.Effects)
            {
                effect.EnableDefaultLighting();
                effect.PreferPerPixelLighting = true;
            }
        }
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        InputManager.Update();
        Time.Update(gameTime);

        //WASD Keys for Camera Pos
        if (InputManager.IsKeyDown(Keys.W) && !InputManager.IsKeyDown(Keys.LeftControl) && !InputManager.IsKeyDown(Keys.LeftShift))
        {
            cameraPos += Vector3.Up * Time.ElapsedGameTime * 5;
        }
        if (InputManager.IsKeyDown(Keys.S) && !InputManager.IsKeyDown(Keys.LeftControl) && !InputManager.IsKeyDown(Keys.LeftShift))
        {
            cameraPos += Vector3.Down * Time.ElapsedGameTime * 5;
        }
        if (InputManager.IsKeyDown(Keys.D) && !InputManager.IsKeyDown(Keys.LeftControl) && !InputManager.IsKeyDown(Keys.LeftShift))
        {
            cameraPos += Vector3.Right * Time.ElapsedGameTime * 5;
        }
        if (InputManager.IsKeyDown(Keys.A) && !InputManager.IsKeyDown(Keys.LeftControl) && !InputManager.IsKeyDown(Keys.LeftShift))
        {
            cameraPos += Vector3.Left * Time.ElapsedGameTime * 5;
        }

        //Arrows for Model Pos
        if (InputManager.IsKeyDown(Keys.Up) && !InputManager.IsKeyDown(Keys.LeftShift))
        {
            modelPos += Vector3.Up * Time.ElapsedGameTime * 5;
        }
        if (InputManager.IsKeyDown(Keys.Down) && !InputManager.IsKeyDown(Keys.LeftShift))
        {
            modelPos += Vector3.Down * Time.ElapsedGameTime * 5;
        }
        if (InputManager.IsKeyDown(Keys.Right))
        {
            modelPos += Vector3.Right * Time.ElapsedGameTime * 5;
        }
        if (InputManager.IsKeyDown(Keys.Left))
        {
            modelPos += Vector3.Left * Time.ElapsedGameTime * 5;
        }

        //Yaw: Z/X, Pitch: C/V, Roll: B/N
        if(InputManager.IsKeyDown(Keys.Z))
        {
            yaw += 0.1f;
        }
        if (InputManager.IsKeyDown(Keys.X))
        {
            if(yaw <= 0)
            {
                yaw = 0;
            }
            else
            {
                yaw -= 0.1f;
            }
        }
        if (InputManager.IsKeyDown(Keys.C))
        {
            pitch += 0.1f;
        }
        if (InputManager.IsKeyDown(Keys.V))
        {
            if (pitch <= 0)
            {
                yaw = 0;
            }
            else
            {
                pitch -= 0.1f;
            }
        }
        if (InputManager.IsKeyDown(Keys.B))
        {
            roll += 0.1f;
        }
        if (InputManager.IsKeyDown(Keys.N))
        {
            if (roll <= 0)
            {
                roll = 0;
            }
            else
            {
                roll -= 0.1f;
            }
        }

        //Model Scale
        if(InputManager.IsKeyDown(Keys.LeftShift) && InputManager.IsKeyDown(Keys.Up))
        {
            scale += 0.1f;
        }
        if(InputManager.IsKeyDown(Keys.LeftShift) && InputManager.IsKeyDown(Keys.Down))
        {
            if(scale <= 0.1f)
            {
                scale = 0.1f;
            }
            else
            {
                scale -= 0.1f;
            }
        }

        //Perspective Width/Height
        if (InputManager.IsKeyDown(Keys.LeftControl) && InputManager.IsKeyDown(Keys.A))
        {
            if(width <= 0.1f)
            {
                width = 0.1f;
            }
            else
            {
                width -= 0.1f;
            }
        }
        if (InputManager.IsKeyDown(Keys.LeftControl) && InputManager.IsKeyDown(Keys.D))
        {
            width += 0.1f;
        }
        if (InputManager.IsKeyDown(Keys.LeftControl) && InputManager.IsKeyDown(Keys.S))
        {
            if (height <= 0.1f)
            {
                height = 0.1f;
            }
            else
            {
                height -= 0.1f;
            }
        }
        if (InputManager.IsKeyDown(Keys.LeftControl) && InputManager.IsKeyDown(Keys.W))
        {
            height += 0.1f;
        }

        //Change World Mode
        if (InputManager.IsKeyPressed(Keys.Space))
        {
            if(mode == true)
            {
                mode = false;
            }
            else
            {
                mode = true;
            }
        }

        if(perspective == true) cameraMode = "Perspective";
        if(perspective == false) cameraMode = "Orthographic";

        if (mode == true) matrixOrder = "Scale First";
        if (mode == false) matrixOrder = "Translation First";

        if (mode == true)
        {
            world = Matrix.CreateScale(scale) *
            Matrix.CreateFromYawPitchRoll(yaw, pitch, roll) *
            Matrix.CreateTranslation(modelPos);
        }
        else
        {
            world = Matrix.CreateTranslation(modelPos) *
                Matrix.CreateFromYawPitchRoll(yaw, pitch, roll) *
                Matrix.CreateScale(scale);
        }

        //Change Perspective Mode
        if (InputManager.IsKeyPressed(Keys.Tab))
        {
            if (perspective == true)
            {
                perspective = false;
            }
            else
            {
                perspective = true;
            }
        }

        //Move Center
        if(InputManager.IsKeyDown(Keys.LeftShift) && InputManager.IsKeyDown(Keys.W))
        {
            verticalOffSet += 0.1f;
        }
        if (InputManager.IsKeyDown(Keys.LeftShift) && InputManager.IsKeyDown(Keys.S))
        {
            verticalOffSet -= 0.1f;
        }
        if (InputManager.IsKeyDown(Keys.LeftShift) && InputManager.IsKeyDown(Keys.D))
        {
            horizontalOffSet += 0.1f;
        }
        if (InputManager.IsKeyDown(Keys.LeftShift) && InputManager.IsKeyDown(Keys.A))
        {
            horizontalOffSet -= 0.1f;
        }


        if (perspective == true)
        {
            projection = Matrix.CreatePerspectiveOffCenter(-width + horizontalOffSet, width + horizontalOffSet, -height + verticalOffSet, height + verticalOffSet, 0.1f, 1000f);

        }
        else
        {
            projection = Matrix.CreateOrthographicOffCenter(-width + horizontalOffSet, width + horizontalOffSet, -height + verticalOffSet, height + verticalOffSet, 0.1f, 1000f);
        }

        view = Matrix.CreateLookAt(
            cameraPos,
            new Vector3(0, 0, -1),
            new Vector3(0, 1, 0));
        

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        model.Draw(world, view, projection);
        _spriteBatch.Begin();
        _spriteBatch.DrawString(font, "WASD Keys: Move Camera", new Vector2(5, 20), Color.Black);
        _spriteBatch.DrawString(font, "Tab: Camera Mode", new Vector2(5, 40), Color.Black);
        _spriteBatch.DrawString(font, "Current Mode: " + cameraMode, new Vector2(5, 55), Color.Black);
        _spriteBatch.DrawString(font, "Arrow Keys: Move Object", new Vector2(5, 75), Color.Black);
        _spriteBatch.DrawString(font, "Space: Matrix Order", new Vector2(5, 95), Color.Black);
        _spriteBatch.DrawString(font, "Current Order: " + matrixOrder, new Vector2(5, 110), Color.Black);
        _spriteBatch.DrawString(font, "Z/X: Yaw Increase/Decrease", new Vector2(5, 130), Color.Black);
        _spriteBatch.DrawString(font, "C/V: Pitch Increase/Decrease", new Vector2(5, 145), Color.Black);
        _spriteBatch.DrawString(font, "B/N: Roll Increase/Decrease", new Vector2(5, 160), Color.Black);
        _spriteBatch.DrawString(font, "Shift+Up/Down: Increase/Decrease Model Scale", new Vector2(5, 180), Color.Black);
        _spriteBatch.DrawString(font, "Shift + WASD: Move Center", new Vector2(5, 200), Color.Black);
        _spriteBatch.DrawString(font, "Ctrl + WASD: Change Width/Height", new Vector2(5, 220), Color.Black);
        _spriteBatch.DrawString(font, "W/S: Icrease/Decrease Height", new Vector2(5, 235), Color.Black);
        _spriteBatch.DrawString(font, "D/A: Increase/Decrease Width", new Vector2(5, 250), Color.Black);


        _spriteBatch.End();

        base.Draw(gameTime);
    }
}

