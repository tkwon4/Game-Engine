using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPI311.GameEngine
{
    public class Renderer : Component, IRenderable
    {
        public Material Material { get; set; }
        public Model ObjectModel;
        public Transform ObjectTransform;
        public int CurrentTechnique;
        public GraphicsDevice g;
        public Camera Camera;
        public Light Light;


        public Renderer(Model objModel, Transform objTransform, Camera camera,
                        ContentManager content, GraphicsDevice graphicsDevice, Light light, int currentTechnique,
                        String filename, float shininess, Texture2D texture)
        {
            if (filename != null)
                Material = new Material(objTransform.World, camera, light, content, 
                                        filename, currentTechnique, shininess, texture);
            else
                Material = null;
            ObjectModel = objModel;
            ObjectTransform = objTransform;
            Camera = camera;
            Light = light;
            CurrentTechnique = currentTechnique;    
            g = graphicsDevice;

        }
        public virtual void Draw()
        {
            if (Material != null) //SHader Case
            {
                Material.Camera = Camera; // Update Material's properties
                Material.World = ObjectTransform.World;
                for (int i = 0; i < Material.Passes; i++)
                {
                    Material.Apply(i); // Look at the Material's Apply method
                    foreach (ModelMesh mesh in ObjectModel.Meshes)
                        foreach (ModelMeshPart part in mesh.MeshParts)
                        {
                            g.SetVertexBuffer(part.VertexBuffer);
                            g.Indices = part.IndexBuffer;
                            g.DrawIndexedPrimitives(PrimitiveType.TriangleList, part.VertexOffset, 0, part.PrimitiveCount);
                        }
                }
            }
            else ObjectModel.Draw(ObjectTransform.World, Camera.View, Camera.Projection); //Default Case
        }
    }
}
