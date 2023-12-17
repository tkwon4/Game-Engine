using CPI311.GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Audio;

namespace Final_Project
{
    public class Final_Project : Game
    {

        public class Scene
        {
            public delegate void CallMethod();
            public CallMethod Update;
            public CallMethod Draw;
            public Scene(CallMethod update, CallMethod draw)
            { Update = update; Draw = draw; }
        }

        Dictionary<String, Scene> scenes;
        Scene currentScene;


        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        List<GUIElement> guiElements;

        SpriteFont font;
        SpriteFont title;
        Texture2D texture;

        List<String> powerUps = new List<String>();
        String currentPowerup = "None";
        int ammoCount = 1;

        Camera camera;
        Transform cameraTransform;
        Light light;
        BoxCollider boxCollider;

        Texture2D background;

        Ship ship;

        List<Ammo> ammoList = new List<Ammo>();
        List<Asteroid> asteroidList = new List<Asteroid>();

        bool gameOver = false;
        bool activeAmmo = false;
        bool activeShooting = false;

        int currentScore = 0;
        int highScore = 0;
        int asteroidsLeft = 15;

        SoundEffect gunSound;
        SoundEffect explosionSound;
        SoundEffectInstance soundInstance;

        ParticleManager particleManager;
        Texture2D particleTex;
        Effect particleEffect;

        public Final_Project()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic
            InputManager.Initialize();
            Time.Initialize();
            ScreenManager.Initialize(_graphics);

            scenes = new Dictionary<string, Scene>();
            guiElements = new List<GUIElement>();

            boxCollider = new BoxCollider();
            boxCollider.Size = 250;


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            ScreenManager.Setup(1920, 1080);

            background = Content.Load<Texture2D>("B1_stars");
            texture = Content.Load<Texture2D>("Square");
            font = Content.Load<SpriteFont>("Font");
            title = Content.Load<SpriteFont>("Title");
            explosionSound = Content.Load<SoundEffect>("explosion2");
            gunSound = Content.Load<SoundEffect>("tx0_fire1");

            particleManager = new ParticleManager(GraphicsDevice, 1);
            particleEffect = Content.Load<Effect>("ParticleShader-complete");
            particleTex = Content.Load<Texture2D>("fire");

            light = new Light();
            Transform lightTransform = new Transform();
            lightTransform.LocalPosition = Vector3.Backward * 10 + Vector3.Right * 5;
            light.Transform = lightTransform;

            cameraTransform = new Transform();
            cameraTransform.LocalPosition += Vector3.Up * 250;
            cameraTransform.Rotate(Vector3.Left, MathHelper.PiOver2);
            camera = new Camera();
            camera.Transform = cameraTransform;
            camera.FarPlane = 300f;

            ship = new Ship(Content, camera, GraphicsDevice, light);
            ship.Transform.LocalScale = new Vector3(0.03f, 0.03f, 0.03f);


            Button instructions = new Button();
            instructions.Text = "Instructions";
            instructions.Texture = texture;
            instructions.Bounds = new Rectangle(GraphicsDevice.Viewport.Width/2-150, GraphicsDevice.Viewport.Height/2 + 50, 300, 20);
            instructions.Action += SwitchScenes1;
            guiElements.Add(instructions);

            Button play = new Button();
            play.Text = "Start Game";
            play.Texture = texture;
            play.Bounds = new Rectangle(GraphicsDevice.Viewport.Width / 2 - 150, GraphicsDevice.Viewport.Height/ 2 - 35, 300, 20);
            play.Action += SwitchScenes2;
            guiElements.Add(play);

            scenes.Add("Menu", new Scene(MainMenuUpdate, MainMenuDraw));
            scenes.Add("Instructions", new Scene(InstructionUpdate, InstructionDraw));
            scenes.Add("Play", new Scene(PlayUpdate, PlayDraw));
            currentScene = scenes["Menu"];
        }

        protected override void Update(GameTime gameTime)
        {
            /*if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();*/

            // TODO: Add your update logic here
            InputManager.Update();
            Time.Update(gameTime);

            currentScene.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            currentScene.Draw();

            base.Draw(gameTime);
        }

        void SwitchScenes1(GUIElement element)
        {
            currentScene = (currentScene == scenes["Instructions"] ? scenes["Menu"] : scenes["Instructions"]);
        }

        void SwitchScenes2(GUIElement element)
        {
            resetGame();
            currentScene = (currentScene == scenes["Play"] ? scenes["Menu"] : scenes["Play"]);
        }

        void MainMenuUpdate()
        {
            foreach (GUIElement element in guiElements)
                element.Update();
        }
        void MainMenuDraw()
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            foreach (GUIElement element in guiElements)
                element.Draw(_spriteBatch, font);
            _spriteBatch.DrawString(title, "Debris Destruction", new Vector2(GraphicsDevice.Viewport.Width / 2 - 280, GraphicsDevice.Viewport.Height / 2 - 200), Color.White);
            _spriteBatch.End();
        }

        void InstructionUpdate()
        {
            if (InputManager.IsKeyReleased(Keys.Escape))
                currentScene = scenes["Menu"];
        }
        void InstructionDraw()
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            _spriteBatch.DrawString(font, "Instructions:", new Vector2(GraphicsDevice.Viewport.Width / 2-600, 100), Color.White);
            _spriteBatch.DrawString(font, "You Are Tasked With Removing The Debris From This Sector Of Space", new Vector2(GraphicsDevice.Viewport.Width / 2 - 600, 200), Color.White);
            _spriteBatch.DrawString(font, "By Bouncing Shots Off An Invisible ForceField", new Vector2(GraphicsDevice.Viewport.Width / 2 - 600, 300), Color.White);
            _spriteBatch.DrawString(font, "Unfortunately, You Are Only Given Limited Resources To Remove The Debris", new Vector2(GraphicsDevice.Viewport.Width / 2 - 600, 400), Color.White);
            _spriteBatch.DrawString(font, "Once You Run Out Of Real Shots You Lose" , new Vector2(GraphicsDevice.Viewport.Width / 2 - 600, 500), Color.White);
            _spriteBatch.DrawString(font, "Destroying Debris Gives A Random Power Up", new Vector2(GraphicsDevice.Viewport.Width / 2 - 600, 600), Color.White);
            _spriteBatch.DrawString(font, "You Regain 2 Shots After Clearing The Asteroids Then 15 More Spawn", new Vector2(GraphicsDevice.Viewport.Width / 2 - 600, 700), Color.White);
            _spriteBatch.DrawString(font, "A/D: Angle Your Ship Left And Right", new Vector2(GraphicsDevice.Viewport.Width / 2 - 600, 800), Color.White);
            _spriteBatch.DrawString(font, "Left Click: Real Shot (Maximum 10 & Bounces 2 Times Before Despawning)", new Vector2(GraphicsDevice.Viewport.Width / 2 - 600, 900), Color.White);
            _spriteBatch.DrawString(font, "ESC: Return To Main Menu (Yes Even From Here)", new Vector2(GraphicsDevice.Viewport.Width / 2 - 600, 1000), Color.White);
            _spriteBatch.DrawString(font, "Power-Ups:", new Vector2(GraphicsDevice.Viewport.Width / 2, 100), Color.White);
            _spriteBatch.DrawString(font, "-Double Shot: Fires 2 Shots In A Spread", new Vector2(GraphicsDevice.Viewport.Width / 2, 200), Color.White);
            _spriteBatch.DrawString(font, "-Triple Shot: Fires 3 Shots In A Spread", new Vector2(GraphicsDevice.Viewport.Width / 2, 300), Color.White);
            _spriteBatch.DrawString(font, "-Reverse Shot: Fires Another Shot Behind The Ship", new Vector2(GraphicsDevice.Viewport.Width / 2, 400), Color.White);
            _spriteBatch.DrawString(font, "-Extra Bounce: Shot Bounces One More Time", new Vector2(GraphicsDevice.Viewport.Width / 2, 500), Color.White);
            _spriteBatch.End();
        }

        void PlayUpdate()
        {
            if (InputManager.IsKeyReleased(Keys.Escape))
            {
                currentScene = scenes["Menu"];
                wipeCurrent();
            }

            //Test For Video Demo
            if(InputManager.IsKeyPressed(Keys.Z))
            {
                currentPowerup = "None";
            }
            else if (InputManager.IsKeyPressed(Keys.X))
            {
                currentPowerup = "Double Shot";
            }
            else if (InputManager.IsKeyPressed(Keys.C))
            {
                currentPowerup = "Triple Shot";
            }
            else if (InputManager.IsKeyPressed(Keys.V))
            {
                currentPowerup = "Reverse Shot";
            }
            else if (InputManager.IsKeyPressed(Keys.B))
            {
                currentPowerup = "Extra Bounce";
            }

            if (gameOver == false)
            {
                if (powerUps.Count > 0)
                {
                    //currentPowerup = powerUps[0];
                }
                else
                {
                    //currentPowerup = "None";
                }

                if (asteroidsLeft <= 0)
                {
                    asteroidsLeft = 15;
                    resetAsteroid();
                    if(ammoCount+5 > 10)
                    {
                        ammoCount = 10;
                    }
                    else
                    {
                        ammoCount += 3;
                    }
                }

                //Check Active Ammo
                activeAmmo = false;

                foreach (Ammo bullet in ammoList)
                {
                    if (bullet.isActive == true)
                    {
                        activeAmmo = true;
                    }
                }

                if (activeAmmo == true)
                {
                    activeShooting = true;
                }
                else
                {
                    activeShooting = false;
                }

                if (activeShooting == false && ammoCount <= 0)
                {
                    gameOver = true;
                    if (currentScore > highScore)
                    {
                        highScore = currentScore;
                    }
                }

                foreach (Ammo bullet in ammoList)
                    bullet.Update();
                foreach (Asteroid asteroid in asteroidList)
                    asteroid.Update();

                //Box Collision
                Vector3 normal;
                for (int i = 0; i < ammoList.Count; i++)
                {
                    if (boxCollider.Collides(ammoList[i].Collider, out normal) && ammoList[i].isActive == true)
                    {
                        soundInstance = gunSound.CreateInstance();
                        soundInstance.Play();
                        ammoList[i].bounceNum++;
                        if (Vector3.Dot(normal, ammoList[i].Rigidbody.Velocity) < 0)
                            ammoList[i].Rigidbody.Impulse +=
                               Vector3.Dot(normal, ammoList[i].Rigidbody.Velocity) * -2 * normal;
                        if (ammoList[i].bounceNum == ammoList[i].maxBounce)
                        {
                            ammoList[i].isActive = false;
                        }
                    }
                }

                //Sphere Collision
                for (int i = 0; i < asteroidList.Count; i++)
                {
                    for (int j = 0; j < ammoList.Count; j++)
                    {
                        if (asteroidList[i].Collider.Collides(ammoList[j].Collider, out normal) && ammoList[j].isActive == true && asteroidList[i].isActive == true)
                        {
                            Random random = new Random();
                            Particle particle = particleManager.getNext();
                            particle.Position = asteroidList[i].Transform.Position;
                            particle.Velocity = new Vector3(
          random.Next(-5, 5), 2, random.Next(-50, 50));
                            particle.Acceleration = new Vector3(0, 3, 0);
                            particle.MaxAge = random.Next(1, 6);
                            particle.Init();
                            soundInstance = explosionSound.CreateInstance();
                            soundInstance.Play();
                            asteroidList[i].isActive = false;
                            asteroidsLeft--;
                            currentScore += 100;
                            powerUps.Add(asteroidList[i].powerUp);

                        }
                    }
                } particleManager.Update();

                foreach (Ammo bullet in ammoList)
                    bullet.Transform.LocalPosition = new Vector3(bullet.Transform.LocalPosition.X, 0, bullet.Transform.LocalPosition.Z);

                //Shoot Ammo
                if (InputManager.IsMousePressed(0))
                {
                    if (ammoCount > 0)
                    {
                        soundInstance = gunSound.CreateInstance();
                        soundInstance.Play();
                        Shoot();
                        if (powerUps.Count > 0)
                        {
                            powerUps.RemoveAt(0);
                        }
                        ammoCount--;
                    }
                }

                ship.Update();
            }
            else
            {
                wipeCurrent();
            }
            
        }
        void PlayDraw()
        {
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            _spriteBatch.Begin();
            _spriteBatch.Draw(background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            if(gameOver == false)
            {
                _spriteBatch.DrawString(font, "Shots Available: " + ammoCount, new Vector2(100, 100), Color.White);
                _spriteBatch.DrawString(font, "Next PowerUp: " + currentPowerup, new Vector2(100, 200), Color.White);
                _spriteBatch.DrawString(font, "Score: " + currentScore, new Vector2(100, 300), Color.White);
                _spriteBatch.DrawString(font, "High Score: " + highScore, new Vector2(100, 400), Color.White);
            }
            else
            {
                _spriteBatch.DrawString(title, "GAME OVER!", new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.White);
            }
            _spriteBatch.End();
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            if (gameOver == false)
            {
                ship.Draw();

                foreach (Asteroid asteroid in asteroidList)
                    asteroid.Draw();
                foreach (Ammo bullet in ammoList)
                    bullet.Draw();
            }

            GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
            particleEffect.CurrentTechnique = particleEffect.Techniques["particle"];
            particleEffect.CurrentTechnique.Passes[0].Apply();
            particleEffect.Parameters["ViewProj"].SetValue(camera.View * camera.Projection);
            particleEffect.Parameters["World"].SetValue(Matrix.Identity);
            particleEffect.Parameters["CamIRot"].SetValue(Matrix.Invert(Matrix.CreateFromQuaternion(camera.Transform.Rotation)));
            particleEffect.Parameters["Texture"].SetValue(particleTex);
            particleManager.Draw(GraphicsDevice);

            GraphicsDevice.RasterizerState = RasterizerState.CullNone;
        }

        public void Shoot()
        {
            if (currentPowerup == "None")
            {
                Ammo bullet1 = new Ammo(Content, camera, GraphicsDevice, light, ship, "None", 3);
                bullet1.Transform.LocalPosition = ship.Transform.LocalPosition;
                bullet1.Transform.LocalPosition += ship.Transform.Forward * 100;
                ammoList.Add(bullet1);
            }
            if (currentPowerup == "Extra Bounce")
            {
                Ammo bullet1 = new Ammo(Content, camera, GraphicsDevice, light, ship, "None", 4);
                bullet1.Transform.LocalPosition = ship.Transform.LocalPosition;
                bullet1.Transform.LocalPosition += ship.Transform.Forward * 100;
                ammoList.Add(bullet1);
            }
            else if (currentPowerup == "Reverse Shot")
            {
                Ammo bullet1 = new Ammo(Content, camera, GraphicsDevice, light, ship, "Reverse Shot", 3);
                bullet1.Transform.LocalPosition = ship.Transform.LocalPosition;
                bullet1.Transform.LocalPosition += ship.Transform.Forward * 100;
                Ammo bullet2 = new Ammo(Content, camera, GraphicsDevice, light, ship, "None", 3);
                bullet2.Transform.LocalPosition = ship.Transform.LocalPosition;
                bullet2.Transform.LocalPosition += ship.Transform.Backward * 100;
                ammoList.Add(bullet1);
                ammoList.Add(bullet2);
            }
            else if (currentPowerup == "Double Shot")
            {
                Ammo bullet2 = new Ammo(Content, camera, GraphicsDevice, light, ship, "None", 3);
                bullet2.Transform.LocalPosition = ship.Transform.LocalPosition;
                bullet2.Transform.LocalPosition += ship.Transform.Forward * 100;
                bullet2.Transform.LocalPosition += ship.Transform.Left * 500;
                Ammo bullet3 = new Ammo(Content, camera, GraphicsDevice, light, ship, "None", 3);
                bullet3.Transform.LocalPosition = ship.Transform.LocalPosition;
                bullet3.Transform.LocalPosition += ship.Transform.Forward * 100;
                bullet3.Transform.LocalPosition += ship.Transform.Right * 500;
                ammoList.Add(bullet2);
                ammoList.Add(bullet3);
            }
            else if (currentPowerup == "Triple Shot")
            {
                Ammo bullet1 = new Ammo(Content, camera, GraphicsDevice, light, ship, "None", 3);
                bullet1.Transform.LocalPosition = ship.Transform.LocalPosition;
                bullet1.Transform.LocalPosition += ship.Transform.Forward * 100;
                Ammo bullet2 = new Ammo(Content, camera, GraphicsDevice, light, ship, "None", 3);
                bullet2.Transform.LocalPosition = ship.Transform.LocalPosition;
                bullet2.Transform.LocalPosition += ship.Transform.Forward * 100;
                bullet2.Transform.LocalPosition += ship.Transform.Left * 500;
                Ammo bullet3 = new Ammo(Content, camera, GraphicsDevice, light, ship, "None", 3);
                bullet3.Transform.LocalPosition = ship.Transform.LocalPosition;
                bullet3.Transform.LocalPosition += ship.Transform.Forward * 100;
                bullet3.Transform.LocalPosition += ship.Transform.Right * 500;
                ammoList.Add(bullet1);
                ammoList.Add(bullet2);
                ammoList.Add(bullet3);
            }
        }

        public void resetAsteroid()
        {
            for(int i = 0; i < 15; i++)
            {
                String powerType = "";
                Random random = new Random();
                int type = random.Next(1, 5);
                if(type == 1)
                {
                    powerType = "Double Shot";
                }
                else if (type == 2)
                {
                    powerType = "Triple Shot";
                }
                else if (type == 3)
                {
                    powerType = "Reverse Shot";
                }
                else if (type == 4)
                {
                    powerType = "Extra Bounce";
                }
                Asteroid asteroid = new Asteroid(Content, camera, GraphicsDevice, light, powerType);
                asteroid.Transform.LocalScale = new Vector3(0.03f, 0.03f, 0.03f);
                asteroidList.Add(asteroid);
            }
        }

        public void wipeCurrent()
        {
            while(ammoList.Count > 0)
            {
                ammoList.RemoveAt(0);
            }
            while (asteroidList.Count > 0)
            {
                asteroidList.RemoveAt(0);
            }
            while(powerUps.Count > 0)
            {
                powerUps.RemoveAt(0);
            }
        }

        public void resetGame()
        {
            ammoCount = 10;
            asteroidsLeft = 15;
            currentScore = 0;
            currentPowerup = "None";
            gameOver = false;
            resetAsteroid();
        }
    }
}