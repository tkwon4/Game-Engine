using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPI311.GameEngine;
using System.Transactions;

namespace Final_Project
{
    public class Asteroid : GameObject
    {
        public bool isActive { get; set; }
        public String powerUp;
        Random random = new Random();
        public Asteroid(ContentManager Content, Camera camera, GraphicsDevice graphicsDevice, Light light, String power)
            : base()
        {
            powerUp = power;

            // *** Add Rigidbody
            Rigidbody rigidbody = new Rigidbody();
            
            Transform.LocalPosition += new Vector3(random.Next(-200, 200), 0, random.Next(-200, 50));
            rigidbody.Transform = Transform;
            rigidbody.Mass = 1;
            rigidbody.Speed = 1;
            Add<Rigidbody>(rigidbody);
            // *** Add Renderer
            Texture2D texture = Content.Load<Texture2D>("asteroid1");
            Renderer renderer = new Renderer(Content.Load<Model>("asteroid4"),
                Transform, camera, Content, graphicsDevice, light, 1, null, 20f, texture);
            Add<Renderer>(renderer);
            // *** Add collider
            SphereCollider sphereCollider = new SphereCollider();
            sphereCollider.Radius = renderer.ObjectModel.Meshes[0].BoundingSphere.Radius * 0.03f;
            sphereCollider.Transform = Transform;
            Add<Collider>(sphereCollider);
            //*** Additional Property (for Asteroid, isActive = true)
            isActive = true;
        }

        public override void Update()
        {       
            if (!isActive) return;
            base.Update();
        }

        public override void Draw()
        {
            if (!isActive) return;
            base.Draw();
        }
    }
}
