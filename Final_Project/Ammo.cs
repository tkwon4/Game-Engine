using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPI311.GameEngine;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Final_Project
{
    public class Ammo : GameObject
    {
        public bool isActive { get; set; }
        public int bounceNum = 0;
        public int maxBounce;
        public Ammo(ContentManager Content, Camera camera, GraphicsDevice graphicsDevice, Light light, Ship ship, String currentPowerUp, int max)
            : base()
        {
            maxBounce = max;
            // *** Add Rigidbody
            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = Transform;
            rigidbody.Mass = 1;
            Vector3 direction;
            if (currentPowerUp == "Reverse Shot")
            {
                direction = ship.Transform.Backward;
            }
            else
            {
                direction = ship.Transform.Forward;
            }
            direction.Normalize();
            rigidbody.Velocity = direction * 100;
            Add<Rigidbody>(rigidbody);
            // *** Add Renderer
            Texture2D texture = Content.Load<Texture2D>("Square");
            Renderer renderer = new Renderer(Content.Load<Model>("Sphere"),
                Transform, camera, Content, graphicsDevice, light, 1, "SimpleShading", 20f, texture);
            Add<Renderer>(renderer);
            // *** Add collider
            SphereCollider sphereCollider = new SphereCollider();
            sphereCollider.Radius = renderer.ObjectModel.Meshes[0].BoundingSphere.Radius * 1f;
            sphereCollider.Transform = Transform;
            Add<Collider>(sphereCollider);
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
