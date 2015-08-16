using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace BarFightEngineM2
{
    
    public class Shader
    {
        private Effect effect;

        public struct Vertex
        {
            private Vector3 position;
            private Vector2 texCoord;
            private Vector3 normal;

            public Vertex(Vector3 position,Vector2 texCoord,Vector3 normal)
            {
                this.position = position;
                this.texCoord = texCoord;
                this.normal = normal;
            }

            public readonly static VertexDeclaration VertexDeclaration =
                new VertexDeclaration(
                    new VertexElement(0, VertexElementFormat.Vector3,
                        VertexElementUsage.Position, 0),
                    new VertexElement(sizeof(float) * 3,
                        VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
                    new VertexElement(sizeof(float) * 5,
                    VertexElementFormat.Vector3, VertexElementUsage.Normal, 0));
        }


        public Effect EFFECT
        {
            get
            {
                return effect;
            }
        }

        public Shader(ContentManager content)
        {
            effect = content.Load<Effect>("Effect");
        }

        public void setEffects(BFModel model)
        {
            List<Texture2D> ltextures = new List<Texture2D>();
            foreach (ModelMesh mesh in model.model.Meshes)
            {
                foreach(Effect cureffect in mesh.Effects)
                {
                    ltextures.Add(cureffect.Parameters["Texture"].GetValueTexture2D());
                }
            }

            foreach (ModelMesh mesh in model.model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = effect.Clone();
                }
            }

            //now set up the textures of the model
            model.Textures = ltextures.ToArray();


        }

        public void draw(BFModel model,Matrix world,Matrix view,Matrix projection,
            string technique,Vector3 lightPos,float lightPow,float ambientPow)
        {
            Matrix[] modelTransformations = new Matrix[model.model.Bones.Count];
            model.model.CopyAbsoluteBoneTransformsTo(modelTransformations);
            int i = 0;
            foreach(ModelMesh mesh in model.model.Meshes)
            {
                foreach(Effect currentEffect in mesh.Effects)
                {
                    Matrix worldMatrix = modelTransformations[mesh.ParentBone.Index] * world;
                   // currentEffect.CurrentTechnique = effect.Techniques[technique];
                    currentEffect.Parameters["xWorldViewProjection"].SetValue(worldMatrix * view * projection);
                    currentEffect.Parameters["xTexture"].SetValue(model.Textures[i++]);
                    currentEffect.Parameters["xWorld"].SetValue(worldMatrix);

                    currentEffect.Parameters["xLightPos"].SetValue(lightPos);
                    currentEffect.Parameters["xLightPower"].SetValue(lightPow);
                    currentEffect.Parameters["xAmbient"].SetValue(ambientPow);
                }
                mesh.Draw();
            }
        }
    }
}
