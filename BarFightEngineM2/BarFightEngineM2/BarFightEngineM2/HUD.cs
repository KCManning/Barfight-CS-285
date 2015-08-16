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
    public class HUD
    {
        private int screenWidth, screenHeight;
        private Texture2D tooth, back, mouth, bar;
        private SpriteFont font;
        private int numOfPlayers = 0;
        
        private float[][] healthAndDrunkBuffers;
        public HUD(ContentManager content,int screenWidth,int screenHeight)
        {
            tooth = content.Load<Texture2D>("Tooth");
            back = content.Load<Texture2D>("MouthBack");
            mouth = content.Load<Texture2D>("MouthBase");
            bar = content.Load<Texture2D>("bar");
            font = content.Load<SpriteFont>("Font");
            healthAndDrunkBuffers = new float[4][];
            for(int i =0;i < 4;i++)
            {
                healthAndDrunkBuffers[i] = new float[2];
            }
            this.screenHeight = screenHeight;
            this.screenWidth = screenWidth;

        }

        public void update(Combatant[] combatants,int numOfPlayers)
        {
            for(int i = 0;i < numOfPlayers;i++)
            {
                healthAndDrunkBuffers[i][0] = combatants[i].HP;
                healthAndDrunkBuffers[i][1] = combatants[i].DrunkBuffer;
            }
            this.numOfPlayers = numOfPlayers;
        }

        public void draw(SpriteBatch sprbatch)
        {
            switch(numOfPlayers)
            {
                case 1:
                    {
                        //draw the first player
                        drawPlayerInfo(sprbatch, 0, 0, healthAndDrunkBuffers[0][0], healthAndDrunkBuffers[0][1]);
                        break;
                    }
                case 2:
                    {
                        ///draw the first player
                        drawPlayerInfo(sprbatch, 0, 0, healthAndDrunkBuffers[0][0], healthAndDrunkBuffers[0][1]);
                        //draw the second player
                        drawPlayerInfo(sprbatch, screenWidth-back.Width, 0, healthAndDrunkBuffers[1][0], healthAndDrunkBuffers[1][1]);
                        break;
                    }
                case 3:
                    {
                        ///draw the first player
                        drawPlayerInfo(sprbatch, 0, 0, healthAndDrunkBuffers[0][0], healthAndDrunkBuffers[0][1]);
                        //draw the second player
                        drawPlayerInfo(sprbatch, screenWidth - back.Width, 0, healthAndDrunkBuffers[1][0], healthAndDrunkBuffers[1][1]);
                        //draw the third player
                        drawPlayerInfo(sprbatch, 0, screenHeight-back.Height, healthAndDrunkBuffers[2][0], healthAndDrunkBuffers[2][1]);
                        break;
                    }
                case 4:
                    {
                        ///draw the first player
                        drawPlayerInfo(sprbatch, 0, 0, healthAndDrunkBuffers[0][0], healthAndDrunkBuffers[0][1]);
                        //draw the second player
                        drawPlayerInfo(sprbatch, screenWidth - back.Width, 0, healthAndDrunkBuffers[1][0], healthAndDrunkBuffers[1][1]);
                        //draw the third player
                        drawPlayerInfo(sprbatch, 0, screenHeight - back.Height, healthAndDrunkBuffers[2][0], healthAndDrunkBuffers[2][1]);
                        //draw the fourth player
                        drawPlayerInfo(sprbatch, screenWidth - back.Width, screenHeight - back.Height, healthAndDrunkBuffers[3][0], healthAndDrunkBuffers[3][1]);
                        break;
                    }
            }
        }

        private void drawPlayerInfo(SpriteBatch sprbatch,int x,int y,float hp,float drunkness)
        {
            //draw the back
            sprbatch.Draw(back, new Vector2(x, y), Color.White);
            //draw the hp
            if(hp/10 > 5)
            {
                for(int i = 0;i < 5;i++)
                {
                    sprbatch.Draw(tooth, new Vector2(x + (tooth.Width * (i + 1)), y + (56-tooth.Height)), Color.White);   
                }

                //now draw the next four teeth
                for (int i = 0; i < (hp/10)-6;i++)
                {
                    sprbatch.Draw(tooth, new Vector2(x + (tooth.Width * (i + 1))+(tooth.Width/2)+4, y + 56+7) , Color.White);
                }
            }
            else
            {
                for (int i = 0; i < (hp/10); i++)
                {
                    sprbatch.Draw(tooth, new Vector2(x + (tooth.Width * (i + 1)), y + (56 - tooth.Height)), Color.White);
                }
            }

            //now draw the mouth over that.
            sprbatch.Draw(mouth, new Vector2(x, y), Color.White);
            sprbatch.Draw(mouth, new Vector2(x, y), Color.Red * (drunkness/100));
        }

    }
}
