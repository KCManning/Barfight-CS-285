using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AnimationAux;
using Microsoft.Xna.Framework.Content;

namespace BarFightEngineM2
{
    public class AnimatedModel
    {
        #region Fields

        private Dictionary<string, short> indices;

        private string currentAnimation;

        public Texture2D[] textures;

        //private Texture2D texture;

        /// <summary>
        /// The actual underlying XNA model
        /// </summary>
        private Model model = null;

        /// <summary>
        /// Extra data associated with the XNA model
        /// </summary>
        private ModelExtra modelExtra = null;

        /// <summary>
        /// The model bones
        /// </summary>
        private List<Bone> bones = new List<Bone>();

        /// <summary>
        /// The model asset name
        /// </summary>
        private string assetName = "";

        /// <summary>
        /// An associated animation clip player
        /// </summary>
        private AnimationPlayer player = null;

        #endregion

        #region Properties

        /// <summary>
        /// The actual underlying XNA model
        /// </summary>
        public Model Model
        {
            get { return model; }
        }

        /// <summary>
        /// The underlying bones for the model
        /// </summary>
        public List<Bone> Bones { get { return bones; } }

        /// <summary>
        /// The model animation clips
        /// </summary>
        public List<AnimationClip> Clips { get { return modelExtra.Clips; } }

        #endregion

        #region Construction and Loading

        /// <summary>
        /// Constructor. Creates the model from an XNA model
        /// </summary>
        /// <param name="assetName">The name of the asset for this model</param>
        public AnimatedModel(string assetName)
        {
            this.assetName = assetName;
            indices = new Dictionary<string, short>();

        }

        /// <summary>
        /// Load the model asset from content
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            this.model = content.Load<Model>(assetName);
            modelExtra = model.Tag as ModelExtra;


            ObtainBones();
            for(short i =0;i < Clips.Count;i++)
            {
                indices.Add(Clips[i].Name, i);
            }
            for(short i = 0;i < bones.Count;i++)
            {
                indices.Add(bones[i].Name, i);
            }

            //texture = content.Load<Texture2D>("male01_diffuse_black");

        }


        #endregion

        #region Bones Management

        /// <summary>
        /// Get the bones from the model and create a bone class object for
        /// each bone. We use our bone class to do the real animated bone work.
        /// </summary>
        private void ObtainBones()
        {
            bones.Clear();
            foreach (ModelBone bone in model.Bones)
            {
                // Create the bone object and add to the heirarchy
                Bone newBone = new Bone(bone.Name, bone.Transform, bone.Parent != null ? bones[bone.Parent.Index] : null);

                // Add to the bones for this model
                bones.Add(newBone);
            }
        }

        /// <summary>
        /// Find a bone in this model by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Bone FindBone(string name)
        {
            foreach (Bone bone in Bones)
            {
                if (bone.Name == name)
                    return bone;
            }
            return null;
        }
        #endregion


        public void PlayAnimation(string animationName,bool looping)
        {
            PlayClip(Clips[indices[animationName]]);
            player.Looping = looping;
            currentAnimation = animationName;
        }

        public bool isPlaying(string animationName)
        {
            return (animationName.Equals(currentAnimation));
        }

        #region Animation Management

        /// <summary>
        /// Play an animation clip
        /// </summary>
        /// <param name="clip">The clip to play</param>
        /// <returns>The player that will play this clip</returns>
        private AnimationPlayer PlayClip(AnimationClip clip)
        {
            // Create a clip player and assign it to this model
            player = new AnimationPlayer(clip, this);
            return player;
        }

        public bool isFinished()
        {
            return (player.Position >= player.Duration);
        }

        public Matrix getBone(string boneName)
        {
            return bones[indices[boneName]].AbsoluteTransform;
        }

        public void loadTextures(string modelName,ContentManager content)
        {
            Model model = content.Load<Model>(modelName);

            int i = 0;
            foreach(ModelMesh mesh in model.Meshes)
            {
                int k = 0;
                foreach (BasicEffect effect in mesh.Effects)
                {
                    ((SkinnedEffect)(this.model.Meshes[i].Effects[0])).Texture  =effect.Texture;
                        k++;
                }
                i++;
            }
        }

        #endregion

        #region Updating

        /// <summary>
        /// Update animation for the model.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(float multiplier)
        {
            if (player != null)
            {
                player.Update(multiplier);
            }
        }

        public void Rewind()
        {
            player.Rewind();
        }

        #endregion

        #region Drawing

        /// <summary>
        /// Draw the model
        /// </summary>
        /// <param name="graphics">The graphics device to draw on</param>
        /// <param name="camera">A camera to determine the view</param>
        /// <param name="world">A world matrix to place the model</param>
        public void Draw(GraphicsDevice graphics, Matrix world,Matrix view,Matrix projection)
        {
            if (model == null)
                return;

            //
            // Compute all of the bone absolute transforms
            //

            Matrix[] boneTransforms = new Matrix[bones.Count];

            for (int i = 0; i < bones.Count; i++)
            {
                Bone bone = bones[i];
                bone.ComputeAbsoluteTransform();

                boneTransforms[i] = bone.AbsoluteTransform;
            }

            //
            // Determine the skin transforms from the skeleton
            //
            Matrix[] skeleton = new Matrix[modelExtra.Skeleton.Count];
            for (int s = 0; s < modelExtra.Skeleton.Count; s++)
            {
                Bone bone = bones[modelExtra.Skeleton[s]];
                skeleton[s] = bone.SkinTransform * bone.AbsoluteTransform;
            }
            int j = 0;
            // Draw the model.
            foreach (ModelMesh modelMesh in model.Meshes)
            {
                foreach (Effect effect in modelMesh.Effects)
                {
                    if (effect is BasicEffect)
                    {
                        BasicEffect beffect = effect as BasicEffect;
                        beffect.World = boneTransforms[modelMesh.ParentBone.Index] * world;
                        beffect.View = view;
                        beffect.Projection = projection;
                        beffect.EnableDefaultLighting();
                        beffect.Texture = textures[j];
                        beffect.PreferPerPixelLighting = true;
                    }

                    if (effect is SkinnedEffect)
                    {
                        SkinnedEffect seffect = effect as SkinnedEffect;
                        seffect.World = boneTransforms[modelMesh.ParentBone.Index] * world;
                        seffect.View = view;
                        seffect.Projection = projection;
                        seffect.EnableDefaultLighting();
                        seffect.PreferPerPixelLighting = true;
                        if(j <= textures.Length-1)
                        seffect.Texture = textures[j];
                        else
                        {
                            seffect.Texture = textures[textures.Length - 1];
                        }
                        seffect.SetBoneTransforms(skeleton);
                        
                    }
                }

                modelMesh.Draw();
                j++;
            }
        }


        #endregion



    }
}
