using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BarFightEngineM2
{
    public class BFModel
    {
        private Texture2D[] textures;
        public Model model;
        private Vector3 position;
        private Vector3 rotation;
        private Matrix world;

        public BFModel()
        {
            textures = null;
            position = new Vector3();
            rotation = new Vector3();
        }


        public void updateWorld()
        {
            world *= Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z);
            world *= Matrix.CreateTranslation(position);
        }


        public void copy(BFModel other)
        {
            this.model = other.model;
            this.RotX = other.RotX;
            this.RotY = other.RotY;
            this.RotZ = other.RotZ;
            this.textures = other.Textures;
            this.X = other.X;
            this.Y = other.Y;
            this.Z = other.Z;
        }
        public Texture2D[] Textures
        {
            get
            {
                return textures;
            }
            set
            {
                textures = value;
            }
        }

        public float X
        {
            get
            {
                return position.X;
            }
            set
            {
                position.X = value;
            }
        }

        public float Y
        {
            get
            {
                return position.Y;
            }
            set
            {
                position.Y = value;
            }
        }

        public float Z
        {
            get
            {
                return position.Z;
            }
            set
            {
                position.Z = value;
            }
        }

        public float RotX
        {
            get
            {
                return rotation.X;
            }
            set
            {
                rotation.X = value;
            }
        }

        public float RotY
        {
            get
            {
                return rotation.Y;
            }
            set
            {
                rotation.Y = value;
            }
        }

        public float RotZ
        {
            get
            {
                return rotation.Z;
            }
            set
            {
                rotation.Z = value;
            }
        }



        public Matrix World
        {
            get
            {
                return world;
            }
            set
            {
                world = value;
            }
        }



    }
}
