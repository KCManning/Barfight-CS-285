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
    class CharacterSelection
    {
        private struct CharacterInfo
        {
            public AnimatedModel model;
            public string name;
            public string modelName;
        }
        private int playerIndex;
        private int numOfPlayers;
        private Game1 parent;
        private LinkedList<CharacterInfo> combatants;
        private SpriteFont font;
        private VertexBuffer buffer;
        private Menu menu;
        private Matrix  view, projection;
        private Matrix world1, world2, world3;
        private LinkedListNode<CharacterInfo> data;
        private Texture2D wallTexture;
        private List<string> selectedCombatants;
        private const int inputDelay = 30;
        int timer;
        public CharacterSelection(Game1 parent,GraphicsDevice device)
        {
            this.numOfPlayers = numOfPlayers;
            combatants = new LinkedList<CharacterInfo>();
            this.parent = parent;
            //set up the back wall for the vertex buffer
            Shader.Vertex[] vertices = new Shader.Vertex[4];
            float xWidth = 32, yWidth = 5, zWidth = 32, wallYDensity = 1, wallXDensity = 5;

            //set up the data for the back wall
            vertices[0] = new Shader.Vertex(new Vector3(-xWidth, yWidth, zWidth), new Vector2(0, 0), new Vector3(0, 1, 0));
            vertices[1] = new Shader.Vertex(new Vector3(-xWidth, 0, zWidth), new Vector2(0, wallYDensity), new Vector3(0, 1, 0));
            vertices[2] = new Shader.Vertex(new Vector3(xWidth, yWidth, zWidth), new Vector2(wallXDensity, 0), new Vector3(0, 1, 0));
            vertices[3] = new Shader.Vertex(new Vector3(xWidth, 0, zWidth), new Vector2(wallXDensity, wallYDensity), new Vector3(0, 1, 0));

            buffer = new VertexBuffer(device, Shader.Vertex.VertexDeclaration, 4, BufferUsage.WriteOnly);
            buffer.SetData(vertices);

            //set up the camera and world matrices
            view = Matrix.CreateLookAt(new Vector3(0, 0,30), new Vector3(0, 0, 0), Vector3.UnitY);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, device.Viewport.AspectRatio, 0.2f, 1000);
            world1 = Matrix.CreateScale(0.1f)*Matrix.CreateRotationX(-MathHelper.PiOver4)* Matrix.CreateTranslation(new Vector3(0, 0, 10));
            world2 = Matrix.CreateScale(0.1f) * Matrix.CreateRotationX(-MathHelper.PiOver4) * Matrix.CreateTranslation(new Vector3(-5, 0, 5));
            world3 = Matrix.CreateScale(0.1f) * Matrix.CreateRotationX(-MathHelper.PiOver4) * Matrix.CreateTranslation(new Vector3(5, 0, 5));
            selectedCombatants = new List<string>();
            timer = inputDelay;
        }

        public void loadCombatants(string address,ContentManager content)
        {
            BFFileReader reader = new BFFileReader(address);
            reader.Open();
            string line;
            while((line = reader.ReadLine()) != null)
            {
                string dataName = "";
                string[] param1 = line.Split(' ');
                List<Texture2D> textures = new List<Texture2D>();
                BFFileReader reader2 = new BFFileReader("Files/Combatants/" + param1[1] + ".txt");
                reader2.Open();
                string line2;
                while ((line2 = reader2.ReadLine()) != null)
                {
                    string[] param = line2.Split(' ');
                    if(param[0] == "Texture")
                    {
                        textures.Add(content.Load<Texture2D>(param[1]));
                    }
                    else
                    if(param[0] == "ModelID")
                    {
                        dataName = param[1];
                    }
                }
                CharacterInfo info = new CharacterInfo();
                AnimatedModel model = new AnimatedModel(dataName);
                string name = param1[0];
                string modelName = param1[1];
                info.model = model;
                info.model.LoadContent(content);
                info.name = name;
                info.modelName = modelName;
                info.model.textures = textures.ToArray();
                combatants.AddLast(info);
                reader2.Close();
                numOfPlayers = parent.NumOfPlayers;
            }
            font = content.Load<SpriteFont>("Font");
            //play all of the animations on the models
            foreach(CharacterInfo info in combatants)
            {
                info.model.PlayAnimation("WalkFight", true);
            }
            data = combatants.First;
            reader.Close();
            wallTexture = content.Load<Texture2D>("walls");
            Texture2D start, cancel;
            start = content.Load<Texture2D>("Start");
            cancel = content.Load<Texture2D>("Exit");
            menu = new Menu(4, new Rectangle(parent.ScreenWidth / 2 - (start.Width / 2), parent.ScreenHeight / 2, start.Width, start.Height), start, cancel);

        }


        public void update()
        {
            //update all of the combatants models
            foreach(CharacterInfo info in combatants)
            {
                info.model.Update(1);
            }
            GamePadState state = GamePad.GetState((PlayerIndex)playerIndex);

            if(timer < 0)
            {
                if(state.ThumbSticks.Left.X > 0.1)
                {
                    if(data.Previous != null)
                    {
                        data = data.Previous;
                    }
                    else
                    {
                        data = combatants.Last;
                    }
                    timer = inputDelay;
                }
                else
                if(state.ThumbSticks.Left.X < -0.1)
                {
                    if(data.Next != null)
                    {
                        data = data.Next;
                    }
                    else
                    {
                        data = combatants.First;
                    }
                    timer = inputDelay;
                }

                switch (menu.SelectedIndex)
                {
                    case 0:
                        {
                            if (state.Buttons.X == ButtonState.Pressed)
                            {
                                if (playerIndex == numOfPlayers-1)
                                {
                                    selectedCombatants.Add(data.Value.modelName);
                                    parent.gameState = Game1.GameState.InGame;
                                    parent.stopSound();
                                    parent.loadRoom(parent.SelectedRoom, selectedCombatants.ToArray());
                                }
                                else
                                {
                                    playerIndex++;
                                    selectedCombatants.Add(data.Value.modelName);

                                }
                                timer = inputDelay;

                            }
                            break;
                        }
                    case 1:
                        {
                            if (state.Buttons.X == ButtonState.Pressed)
                            {
                                //exit back to the level selection
                                if (playerIndex == 0)
                                {
                                    parent.gameState = Game1.GameState.LevelSelection;
                                    parent.resetTimers();
                                }
                                else
                                {
                                    playerIndex--;
                                    data = combatants.First;
                                    parent.resetTimers();
                                }
                                timer = inputDelay;
                            }
                            break;
                        }
                }

            }
            else
            {
                timer--;
            }
            menu.update(state);
           
            
        }

        public void draw(SpriteBatch sprBatch,GraphicsDevice device, Shader shader,Light light)
        {
            Effect effect = shader.EFFECT;
            effect.CurrentTechnique = effect.Techniques["Simplest"];
            effect.Parameters["xWorldViewProjection"].SetValue(Matrix.Identity * view * projection);
            effect.Parameters["xTexture"].SetValue(wallTexture);
            effect.Parameters["xWorld"].SetValue(Matrix.Identity);
            effect.Parameters["xLightPos"].SetValue(light.position);
            effect.Parameters["xLightPower"].SetValue(light.power);
            effect.Parameters["xAmbient"].SetValue(light.ambientPower);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                //draw the wall
                device.SetVertexBuffer(buffer);
                device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            }

            //draw the combatants
            data.Value.model.Draw(device, world1, view, projection);


            sprBatch.Begin();
            //draw the player index
            sprBatch.DrawString(font, "Player " + (playerIndex+1) + " choose your combatant.", new Vector2(parent.ScreenWidth / 2 - 85 - font.MeasureString("Player " + playerIndex + " choose your combatant.").X, parent.ScreenHeight / 2 - 32), Color.Yellow);
            //draw the selected character
            sprBatch.DrawString(font, "Selected character:"+ data.Value.name, new Vector2(parent.ScreenWidth / 2 + 85 , parent.ScreenHeight / 2 - 32), Color.Yellow);
            menu.draw(sprBatch);
            sprBatch.End();
        }

        public void resetTimer()
        {
            timer = inputDelay;
        }

        public int NumOfPlayers
        {
            get
            {
                return numOfPlayers;
            }
            set
            {
                numOfPlayers = value;
            }
        }

    }
}
