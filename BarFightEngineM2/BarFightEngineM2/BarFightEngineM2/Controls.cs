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
    class Controls
    {
        private Texture2D controls;
        private SpriteFont font;
        private Game1 parent;
        public Controls(ContentManager content,Game1 parent)
        {
            controls = content.Load<Texture2D>("controller");
            font = content.Load<SpriteFont>("Font");
            this.parent = parent;
        }

        public void update(GamePadState state)
        {
            if(state.Buttons.Back == ButtonState.Pressed)
            {
                parent.gameState = Game1.GameState.MainMenu;
                parent.resetTimers();
            }
        }

        public void draw(SpriteBatch sprbatch)
        {
            sprbatch.Draw(controls, new Rectangle(0, 0, parent.ScreenWidth, parent.ScreenHeight),Color.White);
            sprbatch.DrawString(font, "Press back to go back to the main menu.", new Vector2(
                parent.ScreenWidth / 2 - (font.MeasureString("Press back to go back to the main menu.").X / 2),
                parent.ScreenHeight - font.MeasureString("Press back to go back to the main menu.").Y), Color.Red);
        }
        
    }
}
