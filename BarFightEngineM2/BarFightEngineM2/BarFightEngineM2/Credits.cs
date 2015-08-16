using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace BarFightEngineM2
{
    class Credits
    {
        private SpriteFont font;
        private Texture2D credits;
        private Game1 parent;
        public Credits(ContentManager content,Game1 parent)
        {
            font = content.Load<SpriteFont>("Font");
            this.parent = parent;
            credits = content.Load<Texture2D>("creditsImage");
        }

        public void update(GamePadState state)
        {
            if(state.Buttons.Back == ButtonState.Pressed)
            {
                parent.resetTimers();
                parent.gameState = Game1.GameState.MainMenu;
            }
        }

        public void draw(SpriteBatch sprBatch)
        {
            sprBatch.Draw(credits, new Rectangle(0, 0, parent.ScreenWidth, parent.ScreenHeight),Color.White);

            sprBatch.DrawString(font, "Press back to go back to the main menu", new Vector2(
                parent.ScreenWidth / 2 - (font.MeasureString("Press start to go back to the main menu").X/2), parent.ScreenHeight -
                font.MeasureString("Press back to go back to the main menu").Y),Color.Red);
        }
    }
}
