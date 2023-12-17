using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPI311.GameEngine
{
    public class Transform : Component, IUpdateable
    {
        private Vector3 localPosition;
        private Quaternion localRotation;
        private Vector3 localScale;
        private Matrix world;

        // ** Update Lab4-C
        private Transform parent; //instance variable

        public Transform Parent //Property Parent
        {
            get { return parent; }
            set 
            {
                if (parent != null) parent.Children.Remove(this);
                parent = value;
                if (parent != null) parent.Children.Add(this);
                UpdateWorld();
            }
        }

        private List<Transform> Children { get; set; } //Property Children

        public Vector3 LocalPosition
        {
            get { return localPosition; }
            set { localPosition = value; UpdateWorld(); }
        }

        public Vector3 LocalScale
        {
            get { return localScale; }
            set { localScale = value; UpdateWorld(); }
        }

        public Quaternion LocalRotation
        {
            get { return localRotation; }
            set { localRotation = value; UpdateWorld(); }
        }

        private void UpdateWorld()
        {
            world = Matrix.CreateScale(localScale) *
            Matrix.CreateFromQuaternion(localRotation) *
            Matrix.CreateTranslation(localPosition);
            
            // ** Lab4-C
            if (parent != null)
                world *= parent.World;
            foreach (Transform child in Children)
                child.UpdateWorld();
        }

        public void Update()
        {
            UpdateWorld();
        }

        // ** Not Local But World Position Property
        public Vector3 Position
        {
            get { return World.Translation; }
        }

        public Quaternion Rotation
        {
            get { return Quaternion.CreateFromRotationMatrix(World); }
            set
            {
                if (Parent == null) LocalRotation = value;
                else
                {
                    Vector3 scale, pos; Quaternion rot;
                    world.Decompose(out scale, out rot, out pos);
                    Matrix total = Matrix.CreateScale(scale) *
                          Matrix.CreateFromQuaternion(value) *
                          Matrix.CreateTranslation(pos);
                    LocalRotation = Quaternion.CreateFromRotationMatrix(
                         Matrix.Invert(Matrix.CreateScale(LocalScale)) * total *
                         Matrix.Invert(Matrix.CreateTranslation(LocalPosition)
                         * Parent.world));
                }
            }
        }

        public Matrix World { get { return world; } }   

        public Vector3 Forward { get { return world.Forward; } }
        public Vector3 Backward { get { return world.Backward; } }
        public Vector3 Right { get { return world.Right; } }    
        public Vector3 Up { get { return world.Up; } }      
        public Vector3 Left { get { return world.Left; } }  
        public Vector3 Down { get { return world.Down; } }

        // ** Constructor
        public Transform()
        {
            localPosition = Vector3.Zero;
            localRotation = Quaternion.Identity;
            localScale = Vector3.One;
            Children = new List<Transform>();
            UpdateWorld();
        }

        // ** Rotate Method
        public void Rotate(Vector3 axis, float angle)
        {
            LocalRotation *= Quaternion.CreateFromAxisAngle(axis, angle);
        }

    }
}
