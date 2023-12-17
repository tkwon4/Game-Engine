using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace CPI311.GameEngine
{
    public class BoxCollider : Collider
    {
        public float Size { get; set; }
        private static Vector3[] normals = { Vector3.Up, Vector3.Down, Vector3.Right,
            Vector3.Left,Vector3.Forward, Vector3.Backward,};

        private static Vector3[] vertices = { new Vector3(-1,-1,1),
            new Vector3(1,-1,1), new Vector3(1,-1,-1), new Vector3(-1,-1,-1),
            new Vector3(-1,1,1), new Vector3(1,1,1), new Vector3(1,1,-1),
            new Vector3(-1,1,-1), };

        private static int[] indices = {
            0,1,2,  0,2,3, // Down: using vertices[0][1][2] , [0][2][3] ...
            5,4,7,  5,7,6, // Up
            4,0,3,  4,3,7, // Left
            1,5,6,  1,6,2, // Right
            4,5,1,  4,1,0, // Front
            3,2,6,  3,6,7, // Back
        };

        public override bool Collides(Collider other, out Vector3 normal)
        {
            if (other is SphereCollider)
            {
                SphereCollider collider = other as SphereCollider;
                normal = Vector3.Zero; // no collision
                bool isColliding = false;

                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        int baseIndex = i * 6 + j * 3;
                        Vector3 a = vertices[indices[baseIndex]] * Size;
                        Vector3 b = vertices[indices[baseIndex + 1]] * Size;
                        Vector3 c = vertices[indices[baseIndex + 2]] * Size;
                        Vector3 n = normals[i];
                        float d = Math.Abs( Vector3.Dot(collider.Transform.Position - a, n ));// calculate the distance to the plane 

                        if (d < collider.Radius)
                        {
                            Vector3 pointOnPlane = collider.Transform.Position - (n * d);  // calculate the closest point
                            float area1 = Vector3.Dot(Vector3.Normalize(Vector3.Cross(b - a, pointOnPlane-a)), n); // check if the point is inside of triangle
                            float area2 = Vector3.Dot(Vector3.Normalize(Vector3.Cross(c - b, pointOnPlane - b)), n);
                            float area3 = Vector3.Dot(Vector3.Normalize(Vector3.Cross(a - c, pointOnPlane - c)), n);
                            if (!(area1 < 0 || area2 < 0 || area3 < 0))
                            {
                                normal += n;
                                j = 1; // skip second triangle, if necessary
                                if (i % 2 == 0) i += 1; // skip opposite side if necessary
                                isColliding = true;
                            }
                        }
                    }
                }
                normal.Normalize();
                return isColliding;
            }
            return base.Collides(other, out normal);
        }

        //Lab 8
        public override float? Intersects(Ray ray)
        {
            Matrix worldInv = Matrix.Invert(Transform.World);
            ray.Position = Vector3.Transform(ray.Position, worldInv);
            ray.Direction = Vector3.TransformNormal(ray.Direction,
             worldInv);
            return new BoundingBox(-Vector3.One * Size, Vector3.One * Size).
            Intersects(ray);
        }
    }
}
