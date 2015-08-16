using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
namespace BarFightEngineM2
{
    class PlayerSelector
    {
        private Menu menu;
        private Texture2D left, right;
        private Game1 parent;
        private const int MinNumOfPlayers = 2, MaxNumOfPlayers = 4;
        private int numOfPlayers;
        private SpriteFont font;
        private const int InputDelay = 30;
        private int timer;
        public PlayerSelector(ContentManager content,Game1 parent)
        {
            this.parent = parent;
            left = content.Load<Texture2D>("Left");
            right = content.Load<Texture2D>("Right");
            Texture2D go, exit;
            go = content.Load<Texture2D>("Start");
            exit = content.Load <Texture2D>("Exit");
            menu = new Menu(4, new Rectangle(parent.ScreenWidth / 2 - (go.Width / 2), parent.ScreenHeight / 2, go.Width, go.Height), go, exit);
            font = content.Load<SpriteFont>("Font");
            timer = InputDelay;
            numOfPlayers = MinNumOfPlayers;
        }

        public void resetTimer()
        {
            timer = InputDelay;
        }

        public void update(GamePadState state)
        {
            menu.update(state);
            if(timer < 0)
            {
                if(state.ThumbSticks.Left.X > 0.1)
                {
                    if(numOfPlayers < MaxNumOfPlayers)
                    {
                        numOfPlayers++;
                    }
                    timer = InputDelay;
                }
                if(state.ThumbSticks.Left.X < -0.1)
                {
                    if(numOfPlayers > MinNumOfPlayers)
                    {
                        numOfPlayers--;
                    }
                    timer = InputDelay;
                }
                switch(menu.SelectedIndex)
                {
                    case 0:
                        {
                            if (state.Buttons.X == ButtonState.Pressed)
                            {
                                parent.gameState = Game1.GameState.CharacterSelection;
                                parent.NumOfPlayers = numOfPlayers;
                                parent.loadCharacterSelection();
                            }
                            break;
                        }
                    case 1:
                        {
                            if(state.Buttons.X == ButtonState.Pressed)
                            {
                                parent.resetTimers();
                                parent.playMusic();
                                parent.gameState = Game1.GameState.LevelSelection;
                            }
                            break;
                        }
                }
            }
            else
            {
                timer--;
            }
        }

        public void draw(SpriteBatch sprBatch)
        {
            //draw the left arrow
            sprBatch.Draw(left, new Rectangle(parent.ScreenWidth / 2 - (left.Width)-8, parent.ScreenHeight/2 - left.Height, left.Width, left.Height), Color.White);
            //draw the right arrow
            sprBatch.Draw(right, new Rectangle(parent.ScreenWidth / 2  + 8, parent.ScreenHeight/2 - right.Height, right.Width, right.Height), Color.White);

            //draw the num of player
            sprBatch.DrawString(font, numOfPlayers.ToString(), new Vector2(parent.ScreenWidth / 2 - (font.MeasureString(numOfPlayers.ToString()).X / 2), parent.ScreenHeight / 2 - font.MeasureString(numOfPlayers.ToString()).Y),Color.Red);
            menu.draw(sprBatch);
        }
    }
}
