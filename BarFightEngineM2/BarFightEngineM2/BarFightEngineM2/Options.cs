using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace BarFightEngineM2
{
    public struct Settings
    {
        public bool AA;
        public int width;
        public int height;
        public bool rumble;
        public bool sfx;
        public bool music;
    }
    public class Options
    {
        private int timer;
        private const int inputDelay = 15;
        private static int[][] screenResos = { new int[] { 1280, 720 },
        new int[] {1920,1080},
            new int[] { 3840, 2160 } };
        private int resoIndex = 0;
        private Menu menu;
        private string message;
        private Game1 parent;
        private int width, height;
        private int x, y;
        private Settings settings;

        public Options(ContentManager content,Game1 parent)
        {
            this.parent = parent;
            Texture2D AA,resolution,rumble,sfx,music;
            AA = content.Load<Texture2D>("antialiasing");
            resolution = content.Load<Texture2D>("resolution");
            rumble = content.Load<Texture2D>("rumble");
            sfx = content.Load<Texture2D>("soundfx");
            music = content.Load<Texture2D>("music");

            x = parent.ScreenWidth / 2 - (AA.Width / 2);
            y = parent.ScreenHeight / 2 - ((4 + AA.Height * 5) / 2);
            width = AA.Width;
            height = AA.Height;
            menu = new Menu(4, new Rectangle(x, y, width, height), AA, resolution, rumble, sfx, music);
            settings = new Settings();
            settings.AA = true;
            settings.height = screenResos[0][1];
            settings.width = screenResos[0][0];

        }

        public void update(GamePadState state)
        {
            menu.update(state);
        }

        public void draw(SpriteBatch sprbatch)
        {

        }

    }
}
