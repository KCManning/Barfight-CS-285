using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace BarFightEngineM2
{
    class LevelSelection
    {
        private struct LevelData
        {
            public Texture2D texture;
            public string name;
        }
        private Texture2D leftArrowTex;
        private Texture2D rightArrowTex;
        private Menu selection;
        private LinkedList<LevelData> levelData;
        private Game1 parent;
        private LinkedListNode<LevelData> data;
        SpriteFont font;
        private const int InputDelay  = 30 ;
        private int time = 0;

        public LevelSelection(Game1 parent)
        {
            levelData = new LinkedList<LevelData>();
            this.parent = parent;
        }

        public void resetTimer()
        {
            time = InputDelay;
        }

        public void LoadLevelSelection(string fileAddress,ContentManager content)
        {
            levelData.Clear();
            BFFileReader reader = new BFFileReader(fileAddress);
            reader.Open();
            string line = null;
            while((line = reader.ReadLine())!= null)
            {
                LevelData levelInfo;
                string[] param = line.Split(' ');
                levelInfo.name = param[0];
                levelInfo.texture = content.Load<Texture2D>(param[1]);
                levelData.AddLast(levelInfo);
            }
            reader.Close();
            data = levelData.First;
            //load the left and right arrow textures
            leftArrowTex = content.Load<Texture2D>("Left");
            rightArrowTex = content.Load<Texture2D>("Right");
            Texture2D exit = content.Load<Texture2D>("Exit");
            Texture2D start = content.Load<Texture2D>("Resume");
            font = content.Load<SpriteFont>("Font");
            selection = new Menu(4, new Rectangle(parent.ScreenWidth/2 -(exit.Width/2),parent.ScreenHeight/2+15,exit.Width,exit.Height),start,exit);
            time = InputDelay;
        }

        public void Update(GamePadState state)
        {
            selection.update(state);
            if(time < 0)
            {
                if(state.ThumbSticks.Left.X > 0.1)
                {
                    if(data.Next!= null)
                    {
                        data = data.Next;
                    }
                    else
                    {
                        data = levelData.First;
                    }
                    time = InputDelay;
                }
                else
                if(state.ThumbSticks.Left.X < -0.1)
                {
                    if(data.Previous != null)
                    {
                        data = data.Previous;
                    }
                    else
                    {
                        data = levelData.Last;
                    }
                    time = InputDelay;
                }
                switch (selection.SelectedIndex)
                {
                    case 0:
                        {
                            //load the room
                            if (state.Buttons.X == ButtonState.Pressed)
                            {
                                parent.SelectedRoom = ("Files/Rooms/" + data.Value.name + ".txt");
                                parent.gameState = Game1.GameState.PlayerSelector;
                                //parent.loadCharacterSelection();
                            }
                            break;
                        }
                    case 1:
                        {
                            if (state.Buttons.X == ButtonState.Pressed)
                            {
                                parent.resetTimers();
                                parent.gameState = Game1.GameState.MainMenu;
                            }
                            break;
                        }
                }
            }
            else
            {
                time--;
            }

        }

        public void Draw(SpriteBatch sprBatch)
        {
            //draw the datas texture
            sprBatch.Draw(data.Value.texture,new Rectangle(parent.ScreenWidth/2-200,parent.ScreenHeight/2-400,400,400),Color.White);
            //draw the left level
            if (data.Previous != null)
            {
                sprBatch.Draw(data.Previous.Value.texture, new Rectangle(parent.ScreenWidth / 2 - 400, parent.ScreenHeight / 2 - 400, 200, 200), Color.White);
            }
            else
            {
                sprBatch.Draw(levelData.Last.Value.texture, new Rectangle(parent.ScreenWidth / 2 - 400, parent.ScreenHeight / 2 - 400, 200, 200), Color.White);
            }

            if(data.Next != null)
            {
                sprBatch.Draw(data.Next.Value.texture, new Rectangle(parent.ScreenWidth / 2 +200, parent.ScreenHeight / 2 - 400, 200, 200), Color.White);
            }
            else
            {
                sprBatch.Draw(levelData.First.Value.texture, new Rectangle(parent.ScreenWidth / 2 + 200, parent.ScreenHeight / 2 - 400, 200, 200), Color.White);
            }
            //draw the text
            sprBatch.DrawString(font, data.Value.name, new Vector2(parent.ScreenWidth / 2 - (font.MeasureString(data.Value.name).X / 2), parent.ScreenHeight / 2), Color.Yellow);
            selection.draw(sprBatch);
        }
    }
}
