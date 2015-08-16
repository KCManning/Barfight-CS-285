using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Media;

namespace BarFightEngineM2
{
    public class Player
    {
        private Vector2 position;
        public Player()
        {

        }

        public Vector2 Position
        {
            get
            {
                return position;
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

    }
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private Shader shader;
        private Room room;
        private NetworkSession networkSession;
        private PacketWriter packetWriter = new PacketWriter();
        private PacketReader packetReader = new PacketReader();
        private GamePadState currentGamePad;
        private int numOfLocalPlayers;
        private int numOfTotalPlayers;
        private const int maxLocalPlayers = 4;
        private int screenWidth, screenHeight;
        private Player[] players;
        private LevelSelection selector;
        private CharacterSelection characterSelector;
        private Menu MainMenu;
        private GameState state;
        private Light light;
        private string selectedRoom;
        private PlayerSelector playerSelector;
        private Texture2D mainBack;
        private SoundEffectInstance backMusic;
        private const int inputDelay = 30;
        private int timer;
        private Credits credits;
        private Options options;
        private Controls controls;

        public int NumOfPlayers
        {
            get
            {
                return numOfLocalPlayers;
            }
            set
            {
                numOfLocalPlayers = value;
            }
        }
        public void stopSound()
        {
            backMusic.Stop();
        }

        public string SelectedRoom
        {
            get
            {
                return selectedRoom;
            }
            set
            {
                selectedRoom = value;
            }
        }
        public enum GameState
        {
            MainMenu,
            LevelSelection,
            CharacterSelection,
            InGame,
            PlayerSelector,
            Controls,
            Options,
            Credits
        }
        public Player[] Players
        {
            get
            {
                return players;
            }
        }
        private string errorMessage;
        private bool inMenu = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //Components.Add(new GamerServicesComponent(this));
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            screenHeight = 1080;
            screenWidth = 1920;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.PreferredBackBufferWidth = screenWidth;
            //never set fullscreen to true, dont do it.
            graphics.IsFullScreen = true;
            graphics.PreferMultiSampling = true;
            graphics.ApplyChanges();

            light = new Light();
            light.position = new Vector3(0, 12, 15);
            light.power = 1.0f;
            light.ambientPower = 1.2f;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            shader = new Shader(Content);

            //load the font
            font = Content.Load<SpriteFont>("font");

            room = new Room(GraphicsDevice, this);
            Texture2D texture1, texture2,texture3,texture4,texture5;
            texture1 = Content.Load<Texture2D>("Start");
            texture2 = Content.Load<Texture2D>("Exit");
            texture3 = Content.Load<Texture2D>("credits");
            texture4 = Content.Load<Texture2D>("controls");
            texture5 = Content.Load<Texture2D>("options");

            state = GameState.MainMenu;
            MainMenu = new Menu(4, new Rectangle(screenWidth / 2 - (texture1.Width / 2), screenHeight / 2 - (texture1.Height / 2), texture1.Width, texture2.Height), texture1, texture2,texture3,texture4,texture5);
            selector = new LevelSelection(this);
            characterSelector = new CharacterSelection( this, GraphicsDevice);
            playerSelector = new PlayerSelector(Content, this);

            mainBack = Content.Load<Texture2D>("Barback");

            //load the background music
            backMusic = Content.Load<SoundEffect>("BARFIGHT").CreateInstance();
            backMusic.IsLooped = true;
            backMusic.Play();
            credits = new Credits(Content, this);
            controls = new Controls(Content, this);
            options = new Options(Content, this);
            //loadRoom("Files/Rooms/room1.txt");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }



        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //for(int i =0;i < 4;i++)
            //{
            //    if(GamePad.GetState((PlayerIndex)i).Buttons.Back == ButtonState.Pressed)
            //    {
            //        this.Exit();
            //    }
            //}
            timer--;


            // TODO: Add your update logic here

            //if (networkSession!= null)
            //{
            //    foreach(LocalNetworkGamer gamer in networkSession.LocalGamers)
            //    {
            //        UpdateLocalGamer(gamer);
            //    }

            //    networkSession.Update();

            //    if (networkSession == null)
            //        return;

            //    foreach(LocalNetworkGamer gamer in networkSession.LocalGamers)
            //    {
            //        ReadIncomingPackets(gamer);
            //    }
            //    //update the room
            //    room.update(gameTime);
            //}
            //else
            //{
            //    UpdateMenuScreen();
            //}
            switch(state)
            {
                case GameState.Options:
                    {
                        break; 
                    }
                case GameState.Controls:
                    {
                        GamePadState state = GamePad.GetState(PlayerIndex.One);
                        controls.update(state);

                        break;
                    }
                case GameState.Credits:
                    {
                        GamePadState state = GamePad.GetState(PlayerIndex.One);
                        credits.update(state);
                        break;
                    }
                case GameState.InGame:
                    {
                        room.update(gameTime);
                        break;
                    }

                case GameState.MainMenu:
                    {
                        GamePadState gState = GamePad.GetState(PlayerIndex.One);
                        MainMenu.update(gState);
                        switch(MainMenu.SelectedIndex)
                        {
                            case 0:
                                {
                                    if (timer < 0)
                                    {
                                        if (gState.Buttons.X == ButtonState.Pressed)
                                        {
                                            timer = inputDelay;
                                            state = GameState.LevelSelection;
                                            //load the levelselector
                                            selector.LoadLevelSelection("Files/LevelData/Levels.txt", Content);
                                        }
                                    }
                                    else timer--;

                                    //set the game state to the level selection state
                                    break;                                    
                                }
                            case 1:
                                {
                                    if (gState.Buttons.X == ButtonState.Pressed)
                                        Exit();
                                    break;
                                }
                            case 2:
                                {
                                    if(timer < 0)
                                    {
                                        if(gState.Buttons.X == ButtonState.Pressed)
                                        {
                                            timer = inputDelay;
                                            state = GameState.Credits;

                                        }
                                    }
                                    break;
                                }
                            case 3:
                                {
                                    if(timer < 0)
                                    {
                                        if(gState.Buttons.X == ButtonState.Pressed)
                                        {
                                            timer = inputDelay;
                                            state = GameState.Controls;
                                        }
                                    }
                                    break;
                                }
                        }
                        break;
                    }
                case GameState.LevelSelection:
                    {
                        selector.Update(GamePad.GetState(PlayerIndex.One));
                        break;
                    }
                case GameState.CharacterSelection:
                    {
                        characterSelector.update();
                        break;
                    }
                case GameState.PlayerSelector:
                    {
                        playerSelector.update(GamePad.GetState(PlayerIndex.One));
                        break;
                    }
            }
            



            base.Update(gameTime);
        }

        public void loadCharacterSelection()
        {
            characterSelector.loadCombatants("Files/LevelData/Characters.txt",Content);
            state = GameState.CharacterSelection;

        }

        public void loadRoom(string roomFile,string[] selectedCombatants)
        {
            //set up the room
            room.loadRoom(roomFile, Content, shader, GraphicsDevice, selectedCombatants);
        }

        public GameState gameState
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
            }
        }

        public void playMusic()
        {
            backMusic.Play();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //if(networkSession == null)
            //{
            //    DrawMenuScreen();
            //}
            //else
            //{
            //    room.draw(shader, GraphicsDevice);
            //}
            switch (state)
            {
                case GameState.Controls:
                    {
                        GraphicsDevice.Clear(Color.Blue);
                        spriteBatch.Begin();
                        controls.draw(spriteBatch);
                        spriteBatch.End();
                        break;
                    }
                case GameState.Credits:
                    {
                        GraphicsDevice.Clear(Color.BlueViolet);
                        spriteBatch.Begin();
                        credits.draw(spriteBatch);
                        spriteBatch.End();
                        break;
                    }
                case GameState.MainMenu:
                    {
                        GraphicsDevice.Clear(Color.Blue);
                        spriteBatch.Begin();
                        spriteBatch.Draw(mainBack, Vector2.Zero, Color.White);
                        MainMenu.draw(spriteBatch);
                        spriteBatch.End();
                        
                        break;
                    }
                case GameState.InGame:
                    {
                        GraphicsDevice.Clear(Color.Black);
                        GraphicsDevice.BlendState = BlendState.Opaque;
                        GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                        room.draw(shader, GraphicsDevice, spriteBatch);
                        spriteBatch.Begin();

                        room.Hud.draw(spriteBatch);
                        if (Combatant.isPaused)
                        {
                            if(Combatant.disconnected == -1)
                            Combatant.PauseMenu.draw(spriteBatch);
                            else
                            {
                                spriteBatch.DrawString(font, "Player " + (Combatant.disconnected + 1)+ " has disconnected. Please reconnect controller " + (Combatant.disconnected+1) + ".",
                                    new Vector2(ScreenWidth / 2 - (font.MeasureString("Player " + Combatant.disconnected + "has disconnected. Please reconnect controller " + Combatant.disconnected + ".").X / 2),
                                    ScreenHeight / 2 - font.MeasureString("Player " + Combatant.disconnected + "has disconnected. Please reconnect controller " + Combatant.disconnected + ".").Y), Color.BlueViolet);
                            }
                        }
                        spriteBatch.End();
                        break;
                    }
                case GameState.LevelSelection:
                    {
                        GraphicsDevice.Clear(Color.Orange);
                        spriteBatch.Begin();
                        selector.Draw(spriteBatch);
                        spriteBatch.End();
                        break;
                    }
                case GameState.CharacterSelection:
                    {
                        GraphicsDevice.Clear(Color.Red);
                        GraphicsDevice.BlendState = BlendState.Opaque;
                        GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                        characterSelector.draw(spriteBatch, GraphicsDevice, shader,light);
                        break;
                    }
                case GameState.PlayerSelector:
                    {
                        GraphicsDevice.Clear(Color.Yellow);
                        spriteBatch.Begin();
                        playerSelector.draw(spriteBatch);
                        spriteBatch.End();
                        break;
                    }
            }
            
            

            base.Draw(gameTime);
        }
        /// <summary>
        /// Draws the startup screen used to create and join network sessions.
        /// </summary>
        void DrawMenuScreen()
        {
            string message = string.Empty;

            if (!string.IsNullOrEmpty(errorMessage))
                message += "Error:\n" + errorMessage.Replace(". ", ".\n") + "\n\n";

            message += "A = create session\n" +
                       "B = join session";

            spriteBatch.Begin();

            spriteBatch.DrawString(font, message, new Vector2(161, 161), Color.Black);
            spriteBatch.DrawString(font, message, new Vector2(160, 160), Color.White);

            spriteBatch.End();
        }

        
        void DrawMessage(string message)
        {
            if (!BeginDraw())
                return;

            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.DrawString(font, message, new Vector2(161, 161), Color.Black);
            spriteBatch.DrawString(font, message, new Vector2(160, 160), Color.White);

            spriteBatch.End();

            EndDraw();
        }

        /// <summary>
        /// Handles input.
        /// </summary>
        private void HandleInput()
        {
            currentGamePad = GamePad.GetState(PlayerIndex.One);


            // Check for exit.
            if (IsActive && currentGamePad.Buttons.Back == ButtonState.Pressed)
            {
                Exit();
            }
        }




        /// <summary>
        /// Checks if the specified button is pressed on either keyboard or gamepad.
        /// </summary>





        /// <summary>
        /// Menu screen provides options to create or join network sessions.
        /// </summary>
        void UpdateMenuScreen()
        {
            if (IsActive)
            {
                if (Gamer.SignedInGamers.Count == 0)
                {
                    // If there are no profiles signed in, we cannot proceed.
                    // Show the Guide so the user can sign in.
                    Guide.ShowSignIn(maxLocalPlayers, false);
                }
                else if (currentGamePad.Buttons.A == ButtonState.Pressed)
                {
                    // Create a new session?
                    CreateSession();
                }
                else if (currentGamePad.Buttons.B == ButtonState.Pressed)
                {
                    // Join an existing session?
                    JoinSession();
                }
            }
        }


        /// <summary>
        /// Starts hosting a new network session.
        /// </summary>
        void CreateSession()
        {
            DrawMessage("Creating session...");

            try
            {
                networkSession = NetworkSession.Create(NetworkSessionType.SystemLink,
                                                       maxLocalPlayers, maxLocalPlayers * 2);

                HookSessionEvents();
                //load the room
                //loadRoom("Files/Rooms/room1.txt");
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }
        }


        /// <summary>
        /// Joins an existing network session.
        /// </summary>
        void JoinSession()
        {
            DrawMessage("Joining session...");

            try
            {
                // Search for sessions.
                using (AvailableNetworkSessionCollection availableSessions =
                            NetworkSession.Find(NetworkSessionType.SystemLink,
                                                maxLocalPlayers, null))
                {
                    if (availableSessions.Count == 0)
                    {
                        errorMessage = "No network sessions found.";
                        return;
                    }

                    // Join the first session we found.
                    networkSession = NetworkSession.Join(availableSessions[0]);

                    HookSessionEvents();
                    //load the room
                    //loadRoom("Files/Rooms/room1.txt");
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }
        }


        /// <summary>
        /// After creating or joining a network session, we must subscribe to
        /// some events so we will be notified when the session changes state.
        /// </summary>
        void HookSessionEvents()
        {
            networkSession.GamerJoined += GamerJoinedEventHandler;
            networkSession.SessionEnded += SessionEndedEventHandler;
        }


        /// <summary>
        /// This event handler will be called whenever a new gamer joins the session.
        /// We use it to allocate a Tank object, and associate it with the new gamer.
        /// </summary>
        void GamerJoinedEventHandler(object sender, GamerJoinedEventArgs e)
        {
            int gamerIndex = networkSession.AllGamers.IndexOf(e.Gamer);

            e.Gamer.Tag = new Player();

        }


        /// <summary>
        /// Event handler notifies us when the network session has ended.
        /// </summary>
        void SessionEndedEventHandler(object sender, NetworkSessionEndedEventArgs e)
        {
            errorMessage = e.EndReason.ToString();

            networkSession.Dispose();
            networkSession = null;
        }


        /// <summary>
        /// Updates the state of the network session, moving the tanks
        /// around and synchronizing their state over the network.
        /// </summary>
        void UpdateNetworkSession()
        {
            // Update our locally controlled tanks, and send their
            // latest position data to everyone in the session.
            foreach (LocalNetworkGamer gamer in networkSession.LocalGamers)
            {
                UpdateLocalGamer(gamer);
            }

            // Pump the underlying session object.
            networkSession.Update();

            // Make sure the session has not ended.
            if (networkSession == null)
                return;

            // Read any packets telling us the positions of remotely controlled tanks.
            foreach (LocalNetworkGamer gamer in networkSession.LocalGamers)
            {
                ReadIncomingPackets(gamer);
            }
        }


        /// <summary>
        /// Helper for updating a locally controlled gamer.
        /// </summary>
        void UpdateLocalGamer(LocalNetworkGamer gamer)
        {
            // Look up what tank is associated with this local player.
            Player player = gamer.Tag as Player;
            GamePadState gamePad = GamePad.GetState(gamer.SignedInGamer.PlayerIndex);

            player.X = gamePad.ThumbSticks.Left.X;
            player.Y = gamePad.ThumbSticks.Left.Y;

            // Write the tank state into a network packet.
            packetWriter.Write(player.Position);


            // Send the data to everyone in the session.
            gamer.SendData(packetWriter, SendDataOptions.InOrder);
        }


        /// <summary>
        /// Helper for reading incoming network packets.
        /// </summary>
        void ReadIncomingPackets(LocalNetworkGamer gamer)
        {
            // Keep reading as long as incoming packets are available.
            while (gamer.IsDataAvailable)
            {
                NetworkGamer sender;

                // Read a single packet from the network.
                gamer.ReceiveData(packetReader, out sender);

                // Discard packets sent by local gamers: we already know their state!
                if (sender.IsLocal)
                    continue;

                // Look up the tank associated with whoever sent this packet.
                Player remotePlayer = sender.Tag as Player;

                // Read the state of this tank from the network packet.
                Vector2 pos = packetReader.ReadVector2();
                remotePlayer.X = pos.X;
                remotePlayer.Y = pos.Y;

            }
        }

        public int ScreenHeight
        {
            get
            {
                return screenHeight;
            }
            set
            {
                screenHeight = value;
            }
        }

        public int ScreenWidth
        {
            get
            {
                return screenWidth;
            }
            set
            {
                screenWidth = value;
            }
        }

        public void resetTimers()
        {
            selector.resetTimer();
            characterSelector.resetTimer();
            playerSelector.resetTimer();
            timer = inputDelay;
            
        }
    }



}

