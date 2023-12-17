using CPI311.GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project
{
    public class Ship : GameObject
    {
        public Ship(ContentManager Content, Camera camera, GraphicsDevice graphicsDevice, Light light)
            : base()
        {
            InputManager.Initialize();
            Transform.LocalPosition = new Vector3(0, 0, 200);
            // *** Add Rigidbody
            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = Transform;
            rigidbody.Mass = 1;
            Add<Rigidbody>(rigidbody);
            // *** Add Renderer
            Texture2D texture = Content.Load<Texture2D>("wedge_p1_diff_v1");
            Renderer renderer = new Renderer(Content.Load<Model>("p1_wedge"), Transform, camera, Content, graphicsDevice, light, 1, null, 20f, texture);
            Add<Renderer>(renderer);
            // *** Add collider
            SphereCollider sphereCollider = new SphereCollider();
            sphereCollider.Radius = renderer.ObjectModel.Meshes[0].BoundingSphere.Radius * 0.01f;
            sphereCollider.Transform = Transform;
            Add<Collider>(sphereCollider);
        }

        public override void Update()
        {
            InputManager.Update();
            if (InputManager.IsKeyDown(Keys.A))
            {
                this.Transform.Rotate(Vector3.Up, Time.ElapsedGameTime);
            }
            if (InputManager.IsKeyDown(Keys.D))
            {
                this.Transform.Rotate(Vector3.Down, Time.ElapsedGameTime);
            }
        }
    }
}
