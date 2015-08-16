using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace BarFightEngineM2
{
    public class Menu
    {
        private Texture2D[] textures;
        private Rectangle[] boxes;
        private int selectedIndex;
        private int offSet;
        private int time = 0;
        private const int InputDelay = 15;
        public Menu(int offSet,Rectangle buttonDimensions,params Texture2D[] textures)
        {
            this.textures = textures;
            boxes = new Rectangle[textures.Length];
            for(int i = 0;i < boxes.Length;i++)
            {
                boxes[i] = new Rectangle(buttonDimensions.X, buttonDimensions.Y +(i*buttonDimensions.Height) + offSet, buttonDimensions.Width, buttonDimensions.Height);
            }
            this.offSet = offSet;
        }

        public void update(GamePadState state)
        {
            if (time < 0)
            {
                if (-state.ThumbSticks.Left.Y < -0.2)
                {
                    if (selectedIndex == 0)
                    {
                        selectedIndex = textures.Length - 1;
                    }
                    else
                    {
                        selectedIndex--;
                    }
                    time = InputDelay;
                }
                else
                if (-state.ThumbSticks.Left.Y > 0.2)
                {
                    if (selectedIndex == textures.Length - 1)
                    {
                        selectedIndex = 0;
                    }
                    else
                    {
                        selectedIndex++;
                    }
                    time = InputDelay;
                }
            }
            else
            {
                time--;
            }
        }

        public void draw(SpriteBatch sprBatch)
        {
            for(int i = 0;i < textures.Length; i++)
            {
                if (i != selectedIndex)
                {
                    sprBatch.Draw(textures[i], boxes[i], Color.White);
                }
                else
                {
                    sprBatch.Draw(textures[i], boxes[i], Color.Yellow*1);
                }

            }
        }

        public bool IsSelected(int index)
        {
            return (index == selectedIndex);
        }

        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }
            set
            {
                if(value > textures.Length-1)
                {
                    selectedIndex = 0;
                }
                else
                if(value < 0)
                {
                    selectedIndex = textures.Length - 1;
                }
            }
        }

        public void dispose()
        {
            foreach(Texture2D texture in textures)
            {
                texture.Dispose();
            }
        }
    }
}
