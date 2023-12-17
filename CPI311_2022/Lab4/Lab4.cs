using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using CPI311.GameEngine;

namespace Lab4
{
    public class Lab4 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // ** Lab4
        Model model1;
        Transform modelTransform1;
        Transform cameraTransform;
        Camera camera;

        Model parent;
        Transform parentTransform;
        public Lab4()
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
            model1 = Content.Load<Model>("Torus");
            modelTransform1 = new Transform();
            parent = Content.Load<Model>("Sphere");
            parentTransform = new Transform();

            modelTransform1.Parent = parentTransform;

            cameraTransform = new Transform();
            cameraTransform.LocalPosition = Vector3.Backward * 5;
            camera = new Camera();
            camera.Transform = cameraTransform;

            foreach (ModelMesh mesh in model1.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }
            }

            foreach (ModelMesh mesh in parent.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
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

            InputManager.Update();
            Time.Update(gameTime);
            // TODO: Add your update logic here
            if (InputManager.IsKeyDown(Keys.W))
                cameraTransform.LocalPosition += cameraTransform.Forward * Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.S))
                cameraTransform.LocalPosition += cameraTransform.Backward * Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.A))
                cameraTransform.Rotate(Vector3.Up, Time.ElapsedGameTime);
            if (InputManager.IsKeyDown(Keys.D))
                cameraTransform.Rotate(Vector3.Down, Time.ElapsedGameTime);

            if (InputManager.IsKeyDown(Keys.Up))
                parentTransform.LocalPosition += parentTransform.Forward * Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.Down))
                parentTransform.LocalPosition += parentTransform.Backward * Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.Left))
                parentTransform.Rotate(Vector3.Up, Time.ElapsedGameTime);
            if (InputManager.IsKeyDown(Keys.Right))
                parentTransform.Rotate(Vector3.Down, Time.ElapsedGameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            model1.Draw(modelTransform1.World, camera.View, camera.Projection);
            parent.Draw(parentTransform.World, camera.View, camera.Projection);

            base.Draw(gameTime);
        }
    }
}