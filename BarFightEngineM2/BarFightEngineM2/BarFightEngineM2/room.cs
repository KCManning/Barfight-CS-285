using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using System.Collections.Generic;
using System.IO;
namespace BarFightEngineM2
{
    public struct Light
    {
        public Vector3 position;
        public float power;
        public float ambientPower;
       
    }

    public class RefModel
    {
        private string modelID;
        private Matrix worldMatrix;
        private Origen origen;
        private float rotX, rotY, rotZ;
        private float transX, transY, transZ;
        public RefModel()
        {
            modelID = "";
            worldMatrix = Matrix.Identity;
            origen = new Origen();
            rotX = 0;
            rotY = 0;
            rotZ = 0;
            transX = 1;
            transY = 1;
            transZ = 1;
        }

        public void updateMatrix()
        {
            //multiply world by the translation matrix
            worldMatrix = Matrix.CreateTranslation(origen.OrigenX, origen.OrigenY, origen.OrigenZ);

            //multiply word by the rotation matrix
            worldMatrix = Matrix.CreateFromYawPitchRoll(rotY, rotX, rotZ) * worldMatrix;

            //multiply the world by the scale matrix
            worldMatrix = Matrix.CreateScale(transX, transY, transZ) * worldMatrix;


        }

        public Origen ORIGEN
        {
            get
            {
                return origen;
            }
            set
            {
                origen = value;
            }

        }

        public string ModelID
        {
            get
            {
                return modelID;
            }
            set
            {
                modelID = value;
            }
        }

        public Matrix WorldMatrix
        {
            get
            {
                return worldMatrix;
            }
            set
            {
                worldMatrix = value;
            }
        }

        public float X
        {
            get
            {
                return origen.OrigenX;
            }
            set
            {
                origen.OrigenX = value;
            }
        }

        public float Y
        {
            get
            {
                return origen.OrigenY;
            }
            set
            {
                origen.OrigenY = value;
            }
        }

        public float Z
        {
            get
            {
                return origen.OrigenZ;
            }
            set
            {
                origen.OrigenZ = value;
            }
        }

        public float RotX
        {
            get
            {
                return rotX;
            }
            set
            {
                rotX = value;
            }
        }

        public float RotY
        {
            get
            {
                return rotY;
            }
            set
            {
                rotY = value;
            }
        }

        public float RotZ
        {
            get
            {
                return rotZ;
            }
            set
            {
                rotZ = value;
            }
        }

        public float TransX
        {
            get
            {
                return transX;
            }
            set
            {
                transX = value;
            }
        }

        public float TransY
        {
            get
            {
                return transY;
            }
            set
            {
                transY = value;
            }
        }

        public float TransZ
        {
            get
            {
                return transZ;
            }
            set
            {
                transZ = value;
            }
        }

        public Origen.Collision_Type CollisionType
        {
            get
            {
                return origen.Type;
            }
            set
            {
                origen.Type = value;
            }
        }

        public float Radius
        {
            get
            {
                switch (origen.Type)
                {
                    case Origen.Collision_Type.BC:
                        {
                            return ((BC)(origen)).Radius;
                        }
                    case Origen.Collision_Type.BS:
                        {
                            return ((BS)(origen)).Radius;
                        }
                }
                //defualt and return 0 if there is no radius on this model
                return 0;
            }
            set
            {
                switch (origen.Type)
                {
                    case Origen.Collision_Type.BC:
                        {
                            ((BC)(origen)).Radius = value;
                            break;
                        }

                    case Origen.Collision_Type.BS:
                        {
                            ((BS)(origen)).Radius = value;
                            break;
                        }
                }
            }
        }


        public float HalfHeight
        {
            get
            {
                switch (origen.Type)
                {
                    case Origen.Collision_Type.AABB:
                        {
                            return ((AABB)(origen)).HalfHeight;
                        }
                    case Origen.Collision_Type.BC:
                        {
                            return ((BC)(origen)).HalfHeight;
                        }
                }
                //return 0 becuase there is no halfHeight
                return 0;
            }
            set
            {
                switch (origen.Type)
                {
                    case Origen.Collision_Type.AABB:
                        {
                            ((AABB)(origen)).HalfHeight = value;
                            break;
                        }
                    case Origen.Collision_Type.BC:
                        {
                            ((BC)(origen)).HalfHeight = value;
                            break;
                        }
                }
            }
        }

        public float HalfWidth
        {
            get
            {
                switch (origen.Type)
                {
                    case Origen.Collision_Type.AABB:
                        {
                            return ((AABB)(origen)).HalfWidth;
                        }
                }
                //return 0 becuase this collision shape is not a AABB
                return 0;
            }
            set
            {
                switch (origen.Type)
                {
                    case Origen.Collision_Type.AABB:
                        {
                            ((AABB)(origen)).HalfWidth = value;
                            break;
                        }
                }
            }
        }

        public float HalfLength
        {
            get
            {
                switch (origen.Type)
                {
                    case Origen.Collision_Type.AABB:
                        {
                            return ((AABB)(origen)).HalfLength;
                        }
                    default:
                        {
                            return 0;
                        }
                }
            }
            set
            {
                switch (origen.Type)
                {
                    case Origen.Collision_Type.AABB:
                        {
                            ((AABB)(origen)).HalfLength = value;
                            break;
                        }
                }
            }
        }
    }

    public class DrinkSpawner : RefModel
    {
        private float drinkStrength;
        private float spawnTime;
        private bool hasDrink;
        private float startX, startY, startZ;
        public const float timerInc = 0.5f;
        public float drinkSpawnTime;


        public DrinkSpawner() : base()
        {
            drinkStrength = 0;
            spawnTime = 0;
            hasDrink = true;
        }

        public float DrinkSpawnTime
        {
            get
            {
                return drinkSpawnTime;
            }
            set
            {
                drinkSpawnTime = value;
            }
        }

        public float StartX
        {
            get
            {
                return startX;
            }
            set
            {
                startX = value;
            }
        }

        public float StartY
        {
            get
            {
                return startY;
            }
            set
            {
                startY = value;
            }
        }

        public float StartZ
        {
            get
            {
                return startZ;
            }
            set
            {
                startZ = value;
            }
        }

        public float DrinkStrength
        {
            get
            {
                return drinkStrength;
            }
            set
            {
                drinkStrength = value;
            }
        }


        public float SpawnTime
        {
            get
            {
                return spawnTime;
            }
            set
            {
                spawnTime = value;
                if(spawnTime < 0)
                {
                    //reset the drink
                    hasDrink = true;
                    spawnTime = DrinkSpawnTime;

                    //reset the drinks coords
                    X = StartX;
                    Y = StartY;
                    Z = StartZ;
                    RotX = 0;
                    RotY = 0;
                    RotZ = 0;
                    updateMatrix();
                }
            }
        }

        public bool HasDrink
        {
            get
            {
                return hasDrink;
            }
            set
            {
                hasDrink = value;
            }
        }
    }



    public class Room
    {
        private List<Combatant> combatants;

        private SoundEffect music;
        private SoundEffectInstance instance;
        private Dictionary<string, BFModel> models;
        private List<RefModel> parts;
        private List<DrinkSpawner> drinks;
        private SpriteFont font;
        private Texture2D floorTex, ceilingTex, leftWallTex, rightWallTex, frontWallTex, backWallTex;
        private Vector3 lightPos;
        private float lightPower;
        private float ambientPower;
        private VertexBuffer floorBuffer, ceilingBuffer, leftWallBuffer, rightWallBuffer, frontWallBuffer, backWallBuffer;
        private IndexBuffer positiveBuffer, negativeBuffer;
        private float xWidth, yWidth, zWidth;
        private Matrix view, projection;
        private float floorXDensity, floorYDensity;
        private AABB bounds;
        private float wallXDensity, wallYDensity;
        private float barX, barY, barWidth;
        private GamePadState[] controllers;
        private Game1 parent;
        private int numOfPlayers;
        private SoundEffectInstance winSong;
        private HUD hud;
        private bool hasWon;
        private int winner;
        public Room(GraphicsDevice device, Game1 parent)
        {
            //init model keys 
            models = new Dictionary<string, BFModel>();

            //init music
            music = null;

            //init view and projection matrixes

            view = Matrix.CreateLookAt(new Vector3(0, 5, 5), Vector3.Zero, Vector3.UnitY);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, device.Viewport.AspectRatio, 0.2f, 1000);

            //init the light
            lightPos = new Vector3();

            //set up parts list
            parts = new List<RefModel>();

            combatants = new List<Combatant>();
            hud = new HUD(parent.Content,parent.ScreenWidth,parent.ScreenHeight);

            drinks = new List<DrinkSpawner>();
            font = parent.Content.Load<SpriteFont>("Font");
            this.parent = parent;
        }

        public void loadCombatants(ContentManager content,string[] selectedCombatants, int numOfPlayers)
        {
            for (int i = 0; i < numOfPlayers; i++)
            {
                Combatant player = new Combatant(parent,this);
                player.loadCombatant(content, "Files/Combatants/"+ selectedCombatants[i]+ ".txt", (PlayerIndex)i);
                player.X = barX + (barWidth / 8 * i);
                player.Z = barY;
                player.LastX = barX + (barWidth / 8 * i);
                player.LastZ = barY;
                combatants.Add(player);

            }
            this.numOfPlayers = numOfPlayers;
        }

        public void update(GameTime time)
        {
            float frameTime = time.ElapsedGameTime.Milliseconds/ 14.0f ;
            //update each of the players
            if(!hasWon)
            foreach (Combatant combatant in combatants)
            {

                combatant.update(frameTime);
            }
            else
            {
                if(GamePad.GetState((PlayerIndex)winner).Buttons.Start == ButtonState.Pressed)
                {
                    //exit back to the main menu
                    parent.resetTimers();
                    parent.gameState = Game1.GameState.MainMenu;
                }
            }

            float sumX = 0, sumY = 0, sumZ = 0;
            foreach (Combatant combatant in combatants)
            {
                sumX += combatant.X;
                sumY += combatant.Y;
                sumZ += combatant.Z;
            }
            if (Combatant.isPaused)
                return;
            else
            {
                if(Combatant.disconnected != -1)
                {
                    GamePadState state = GamePad.GetState((PlayerIndex)Combatant.disconnected);
                    if(state.IsConnected)
                    {
                        Combatant.isPaused = false;
                    }
                }
            }
            //resolve the collisions
            resolveCollisions();
            //create the averidge point
            Vector3 averidgePoint = new Vector3(sumX / combatants.Count, sumY / combatants.Count, sumZ / combatants.Count);
            //calculate the some of the distances
            float sumDistance = 0.0f;
            foreach (Combatant combatant in combatants)
            {
                if(!combatant.IsDead)
                sumDistance += (float)Math.Sqrt(Math.Pow(combatant.X - averidgePoint.X, 2) + Math.Pow(combatant.Y - averidgePoint.Y, 2) + Math.Pow(combatant.Z - averidgePoint.Z, 2));
            }
            if((winner = getWinner()) == -1)
            view = Matrix.CreateLookAt(new Vector3(averidgePoint.X , sumDistance + 12, averidgePoint.Z - sumDistance), averidgePoint, Vector3.UnitY);
            else
            {
                hasWon = true;
                instance.Stop();
                winSong.Play();
                int i = 0;
                foreach(Combatant combatant in combatants)
                {
                    combatant.model.Update(1);
                    if(i != winner)
                    {
                        if(!combatant.model.isPlaying("KnockOut"))
                        {
                            combatant.model.PlayAnimation("KnockOut", false);
                        }
                    }
                    i++;
                }
            }

            hud.update(combatants.ToArray(), numOfPlayers);
        }

        public void stopSound()
        {
            instance.Stop();
        }

        public int getWinner()
        {
            List<int> notDead = new List<int>();
            //loop through eahc combatant and see if they are dead or not
            int i = 0;
            foreach(Combatant combatant in combatants)
            {
                if(!combatant.IsDead)
                {
                    notDead.Add(i);
                }
                i++;
            }

            if(notDead.Count == 1)
            {
                return notDead[0];
            }
            else
            {
                return -1;
            }


        }

        private void resolveCollisions()
        {
            //resolves the objects collisions with things on the map
            foreach (Combatant combatant in combatants)
            {
                foreach (RefModel model in parts)
                {
                    switch (model.ORIGEN.Type)
                    {
                        case Origen.Collision_Type.AABB:
                            {
                                //check for and resolve collicions with the model
                                if (((AABB)(combatant.ORIGEN)).collides((AABB)(model.ORIGEN)))
                                { 
                                    combatant.X = combatant.LastX;
                                    combatant.Y = combatant.LastY;
                                    combatant.Z = combatant.LastZ;
                                }

                                break;
                            }
                    }


                }
                //now check to see if this combatant is outside the room and if it is move it back
                if (Math.Abs(combatant.X) > xWidth - combatant.HalfWidth)
                {
                    combatant.X = combatant.LastX;
                }
                if (Math.Abs(combatant.Y) > yWidth - combatant.HalfLength)
                {
                    combatant.Y = combatant.LastY;
                }
                if (Math.Abs(combatant.Z) > zWidth - combatant.HalfHeight)
                {
                    combatant.Z = combatant.LastZ;
                }

                //now resolve collisions with other combatants
                foreach(Combatant other in combatants)
                {
                    if(combatant != other && !other.IsDead)
                    {
                        //check for collisions
                        if (((AABB)(combatant.ORIGEN)).collides((AABB)(other.ORIGEN)))
                        {
                            combatant.X = combatant.LastX;
                            combatant.Y = combatant.LastY;
                            combatant.Z = combatant.LastZ;
                            float angle =(float) (Math.Atan2(combatant.Y - other.Y, other.X - combatant.X)*(180/Math.PI));
                            //now attack the player if the player is within the direction of the other player
                            other.damage(combatant);

                        }

                        //now check for collisions with the other combatants fists

                        //check with right fist
                        if(((AABB)(other.ORIGEN)).collides(combatant.RightFist))
                        {
                            //there was a collision with the right fist so attack the other player
                            other.damage(combatant);
                        }

                        //check with left fist
                        if (((AABB)(other.ORIGEN)).collides(combatant.LeftFist))
                        {
                            //there was a collision with the right fist so attack the other player
                            other.damage(combatant);
                        }
                    }

                }

                //check for collisions with bottles
                foreach(DrinkSpawner drink in drinks)
                {
                    if (((AABB)(combatant.ORIGEN)).collides(((AABB)(drink.ORIGEN))) && drink.HasDrink)
                    {
                        //if that player is pressing the a button then drink the drink
                        if(GamePad.GetState(combatant.Index).Buttons.A == ButtonState.Pressed)
                        {
                            combatant.Drink(drink);
                            drink.HasDrink = false;
                        }
                    }

                    //update the drink
                    if(!drink.HasDrink)
                    drink.SpawnTime -= (float)0.5;
                }

            }
        }


        public bool loadRoom(string roomAddress, ContentManager content, Shader shader, GraphicsDevice device,string[] selectCombatants)
        {
            //load the menu
            Texture2D exit = content.Load<Texture2D>("Exit");
            Texture2D resume = content.Load<Texture2D>("Resume");

            Combatant.PauseMenu = new Menu(4, new Rectangle(
                parent.ScreenWidth / 2 - (exit.Width / 2),
                parent.ScreenHeight / 2 - (exit.Height / 2 * 2),
                exit.Width,
                exit.Height),
                resume, exit);
            hasWon = false;
            BFFileReader reader = new BFFileReader(roomAddress);
            reader.Open();

            Dictionary<string, RefModel> modelDefs = new Dictionary<string, RefModel>();
            string line;
            //read all of the info in the script
            #region room parser
            while ((line = reader.ReadLine()) != null)
            {
                string[] param = line.Split(' ');

                //read the param
                switch (param[0])
                {
                    case "ModelDef":
                        {
                            //add the new modelname to the hashtable
                            BFModel model = new BFModel();//***
                            model.model = content.Load<Model>(param[1]);
                            shader.setEffects(model);
                            //add the model to the hashtable of models
                            models.Add(param[1], model);
                            break;
                        }
                    case "Music":
                        {
                            //set music
                            music = content.Load<SoundEffect>(param[1]);
                            break;
                        }
                    case "Model":
                        {
                            string modelName = param[1];
                            RefModel refModel = new RefModel();
                            while ((line = reader.ReadLine()) != null && line != "}")
                            {
                                //keep reading into this modelRef
                                param = line.Split(' ');
                                switch (param[0])
                                {
                                    case "ModelID":
                                        {
                                            //set the model id of this ref mode
                                            refModel.ModelID = param[1];

                                            break;
                                        }
                                    case "CollideType":
                                        {
                                            switch (param[1].ToUpper())
                                            {
                                                case "AABB":
                                                    {
                                                        refModel.ORIGEN = new AABB();
                                                        refModel.CollisionType = Origen.Collision_Type.AABB;

                                                        break;
                                                    }
                                                case "NONE":
                                                    {
                                                        refModel.CollisionType = Origen.Collision_Type.NONE;
                                                        break;
                                                    }
                                                case "BC":
                                                    {
                                                        refModel.ORIGEN = new BC();
                                                        refModel.CollisionType = Origen.Collision_Type.BC;
                                                        break;
                                                    }
                                                case "BS":
                                                    {
                                                        refModel.ORIGEN = new BS();
                                                        refModel.CollisionType = Origen.Collision_Type.BS;
                                                        break;
                                                    }
                                            }
                                            break;
                                        }
                                    case "OrigenX":
                                        {
                                            refModel.X = float.Parse(param[1]);
                                            break;
                                        }
                                    case "OrigenY":
                                        {
                                            refModel.Y = float.Parse(param[1]);
                                            break;
                                        }
                                    case "OrigenZ":
                                        {
                                            refModel.Z = float.Parse(param[1]);
                                            break;
                                        }
                                    case "RotX":
                                        {
                                            refModel.RotX = float.Parse(param[1]);
                                            break;
                                        }
                                    case "RotY":
                                        {
                                            refModel.RotY = float.Parse(param[1]);
                                            break;
                                        }
                                    case "RotZ":
                                        {
                                            refModel.RotZ = float.Parse(param[1]);
                                            break;
                                        }
                                    case "HalfWidth":
                                        {
                                            refModel.HalfWidth = float.Parse(param[1]);
                                            break;
                                        }
                                    case "HalfHeight":
                                        {
                                            refModel.HalfHeight = float.Parse(param[1]);
                                            break;
                                        }
                                    case "HalfLength":
                                        {
                                            refModel.HalfLength = float.Parse(param[1]);
                                            break;
                                        }
                                    case "Radius":
                                        {
                                            refModel.Radius = float.Parse(param[1]);
                                            break;
                                        }
                                    case "TransX":
                                        {
                                            refModel.TransX = float.Parse(param[1]);
                                            break;
                                        }
                                    case "TransY":
                                        {
                                            refModel.TransY = float.Parse(param[1]);
                                            break;
                                        }
                                    case "TransZ":
                                        {
                                            refModel.TransZ = float.Parse(param[1]);
                                            break;
                                        }
                                }

                            }

                            refModel.updateMatrix();
                            //add the refmodel to the list of modelsDefs
                            modelDefs.Add(modelName, refModel);
                            break;
                        }

                    case "DrinkSpawner":
                        {
                            string modelID = "";
                            float X = 0, Y = 0, Z = 0, RotX = 0, RotY = 0, RotZ = 0,spawnTime = 0,drinkStrength = 0,
                                halfWidth = 0,halfLength = 0,halfHeight = 0,transX = 0,transY = 0,transZ = 0;
                            while ((line = reader.ReadLine()) != null && line != "}")
                            {
                                param = line.Split(' ');
                                switch (param[0])
                                {
                                    case "X":
                                        {
                                            X = float.Parse(param[1]);
                                            break;
                                        }
                                    case "Y":
                                        {
                                            Y = float.Parse(param[1]);
                                            break;
                                        }
                                    case "Z":
                                        {
                                            Z = float.Parse(param[1]);
                                            break;
                                        }
                                    case "RotX":
                                        {
                                            RotX = float.Parse(param[1]);
                                            break;
                                        }
                                    case "RotY":
                                        {
                                            RotY = float.Parse(param[1]);
                                            break;
                                        }
                                    case "RotZ":
                                        {
                                            RotZ = float.Parse(param[1]);
                                            break;
                                        }
                                    case "ModelID":
                                        {
                                            modelID = param[1];
                                            break;
                                        }
                                    case "SpawnTime":
                                        {
                                            spawnTime = float.Parse(param[1]);
                                            break;
                                        }
                                    case "DrinkStrength":
                                        {
                                            drinkStrength = float.Parse(param[1]);
                                            break;
                                        }
                                    case "HalfWidth":
                                        {
                                            halfWidth = float.Parse(param[1]);
                                            break;
                                        }
                                    case "HalfHeight":
                                        {
                                            halfHeight= float.Parse(param[1]);
                                            break;
                                        }
                                    case "HalfLength":
                                        {
                                            halfLength = float.Parse(param[1]);
                                            break;
                                        }
                                    case "TransX":
                                        {
                                            transX = float.Parse(param[1]);
                                            break;
                                        }
                                    case "TransZ":
                                        {
                                            transZ = float.Parse(param[1]);
                                            break;
                                        }
                                    case "TransY":
                                        {
                                            transY = float.Parse(param[1]);
                                            break;
                                        }

                                }

                            }

                            DrinkSpawner drinkSpawner = new DrinkSpawner();
                            drinkSpawner.CollisionType = Origen.Collision_Type.AABB;
                            drinkSpawner.ORIGEN = new AABB();
                            drinkSpawner.X = X;
                            drinkSpawner.Y = Y;
                            drinkSpawner.Z = Z;
                            drinkSpawner.StartX = X;
                            drinkSpawner.StartY = Y;
                            drinkSpawner.StartZ = Z;
                            drinkSpawner.DrinkStrength = drinkStrength;
                            drinkSpawner.HalfHeight = halfHeight;
                            drinkSpawner.HalfLength = halfLength;
                            drinkSpawner.HalfWidth = halfWidth;
                            drinkSpawner.ModelID = modelID;
                            drinkSpawner.RotX = RotX;
                            drinkSpawner.RotY = RotY;
                            drinkSpawner.RotZ = RotZ;
                            drinkSpawner.TransX = transX;
                            drinkSpawner.TransY = transY;
                            drinkSpawner.TransZ = transZ;
                            drinkSpawner.DrinkSpawnTime = spawnTime;
                            drinkSpawner.SpawnTime = spawnTime;
                            drinkSpawner.updateMatrix();
                            drinks.Add(drinkSpawner);
                            break;
                        }

                    case "ModelGroup":
                        {
                            List<string> includedModels = new List<string>();
                            float X = 0, Y = 0, Z = 0, RotX = 0, RotY = 0, RotZ = 0;
                            while ((line = reader.ReadLine()) != null && line != "}")
                            {
                                param = line.Split(' ');
                                switch (param[0])
                                {
                                    case "X":
                                        {
                                            X = float.Parse(param[1]);
                                            break;
                                        }
                                    case "Y":
                                        {
                                            Y = float.Parse(param[1]);
                                            break;
                                        }
                                    case "Z":
                                        {
                                            Z = float.Parse(param[1]);
                                            break;
                                        }
                                    case "RotX":
                                        {
                                            RotX = float.Parse(param[1]);
                                            break;
                                        }
                                    case "RotY":
                                        {
                                            RotY = float.Parse(param[1]);
                                            break;
                                        }
                                    case "RotZ":
                                        {
                                            RotZ = float.Parse(param[1]);
                                            break;
                                        }
                                    case "IncludeModel":
                                        {
                                            includedModels.Add(param[1]);
                                            break;
                                        }
                                }

                            }

                            //now add the models to the scenes models
                            foreach (string modelName in includedModels)
                            {
                                //load the model to copy the model data from
                                RefModel temp = (RefModel)modelDefs[modelName];
                                RefModel copy = new RefModel();
                                copy.ORIGEN = new AABB();
                                copy.ORIGEN.Type = Origen.Collision_Type.AABB;
                                copy.HalfHeight = temp.HalfHeight;
                                copy.HalfLength = temp.HalfLength;
                                copy.HalfWidth = temp.HalfWidth;
                                copy.X = temp.X + X;
                                copy.Y = temp.Y + Y;
                                copy.Z = temp.Z + Z;
                                copy.RotX = temp.RotX + RotX;
                                copy.RotY = temp.RotY + RotY;
                                copy.RotZ = temp.RotZ + RotZ;
                                copy.TransX = temp.TransX;
                                copy.TransY = temp.TransY;
                                copy.TransZ = temp.TransZ;
                                copy.ModelID = temp.ModelID;
                                copy.updateMatrix();
                                //add the copy
                                parts.Add(copy);
                            }
                            break;
                        }

                    case "XWidth":
                        {
                            xWidth = float.Parse(param[1]);
                            break;
                        }

                    case "YWidth":
                        {
                            yWidth = float.Parse(param[1]);
                            break;
                        }

                    case "ZWidth":
                        {
                            zWidth = float.Parse(param[1]);
                            break;
                        }
                    case "LeftWallTexture":
                        {
                            leftWallTex = content.Load<Texture2D>(param[1]);
                            break;
                        }
                    case "RightWallTexture":
                        {
                            rightWallTex = content.Load<Texture2D>(param[1]);
                            break;
                        }
                    case "FrontWallTexture":
                        {
                            frontWallTex = content.Load<Texture2D>(param[1]);
                            break;
                        }
                    case "BackWallTexture":
                        {
                            backWallTex = content.Load<Texture2D>(param[1]);
                            break;
                        }
                    case "CeilingTexture":
                        {
                            ceilingTex = content.Load<Texture2D>(param[1]);
                            break;
                        }
                    case "FloorTexture":
                        {
                            floorTex = content.Load<Texture2D>(param[1]);
                            break;
                        }

                    case "Light":
                        {
                            switch (param[1])
                            {
                                case "X":
                                    {
                                        lightPos.X = float.Parse(param[2]);
                                        break;
                                    }
                                case "Y":
                                    {
                                        lightPos.Y = float.Parse(param[2]);
                                        break;
                                    }
                                case "Z":
                                    {
                                        lightPos.Z = float.Parse(param[2]);
                                        break;
                                    }
                                case "Power":
                                    {
                                        lightPower = float.Parse(param[2]);
                                        break;
                                    }
                                case "Ambient":
                                    {
                                        ambientPower = float.Parse(param[2]);
                                        break;
                                    }

                            }
                            break;
                        }
                    case "FloorXDensity":
                        {
                            floorXDensity = float.Parse(param[1]);
                            break;
                        }
                    case "FloorYDensity":
                        {
                            floorYDensity = float.Parse(param[1]);
                            break;
                        }
                    case "WallXDensity":
                        {
                            wallXDensity = float.Parse(param[1]);
                            break;
                        }
                    case "WallYDensity":
                        {
                            wallYDensity = float.Parse(param[1]);
                            break;
                        }
                    case "BarX":
                        {
                            barX = float.Parse(param[1]);
                            break;
                        }
                    case "BarY":
                        {
                            barY = float.Parse(param[1]);
                            break;
                        }
                    case "BarWidth":
                        {
                            barWidth = float.Parse(param[1]);
                            break;
                        }

                }
            }
            #endregion
            //close the file
            reader.Close();

            //play the music if the music exists
            if (music != null)
            {
                instance = music.CreateInstance();
                instance.IsLooped = true;
                instance.Play();
            }

            winSong = content.Load<SoundEffect>("WinSong").CreateInstance();
            winSong.IsLooped = true;
            //set up the bounding box for the room

            //init the buffers
            initBuffers(device);

            bounds = new AABB(0, 0, 0, xWidth, yWidth, zWidth);
            //load the combatants after clearing the list of the old ones
            combatants.Clear();
            loadCombatants(content,selectCombatants,parent.NumOfPlayers);

            return true;

        }

        public void dispose()
        {
            //dispose of the instance
            instance.Stop();
            instance.Dispose();
            music.Dispose();
            models.Clear();

        }

        public HUD Hud
        {
            get
            {
                return hud;
            }
        }
        #region drawing
        public void draw(Shader shader, GraphicsDevice device,SpriteBatch sprbatch)
        {
            Effect effect = shader.EFFECT;
            effect.CurrentTechnique = effect.Techniques["Simplest"];
            effect.Parameters["xWorldViewProjection"].SetValue(Matrix.Identity * view * projection);
            effect.Parameters["xTexture"].SetValue(floorTex);
            effect.Parameters["xWorld"].SetValue(Matrix.Identity);
            effect.Parameters["xLightPos"].SetValue(lightPos);
            effect.Parameters["xLightPower"].SetValue(lightPower);
            effect.Parameters["xAmbient"].SetValue(ambientPower);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                //draw the floor
                device.SetVertexBuffer(floorBuffer);
                device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            }

            effect.Parameters["xTexture"].SetValue(frontWallTex);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                //draw the front wall
                device.SetVertexBuffer(frontWallBuffer);
                device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            }

            effect.Parameters["xTexture"].SetValue(rightWallTex);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                //draw the right wall
                device.SetVertexBuffer(rightWallBuffer);
                device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            }

            effect.Parameters["xTexture"].SetValue(backWallTex);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                //draw the back wall
                device.SetVertexBuffer(backWallBuffer);
                device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            }

            effect.Parameters["xTexture"].SetValue(leftWallTex);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                //draw the left wall
                device.SetVertexBuffer(leftWallBuffer);
                device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            }

            effect.Parameters["xTexture"].SetValue(ceilingTex);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                //draw the ceiling
                device.SetVertexBuffer(ceilingBuffer);
                device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            }


            //now draw the models
            foreach (RefModel model in parts)
            {
                BFModel bfmodel = (BFModel)(models[model.ModelID]);
                //draw the model
                shader.draw(bfmodel, model.WorldMatrix, view, projection, "Simplest", lightPos, lightPower, ambientPower);
            }
            //draw the player
            foreach (Combatant player in combatants)
            {
                //shader.draw((BFModel)(models[player.ModelID]), player.WorldMatrix, view, projection, "Simplest", lightPos, lightPower, ambientPower);
                player.draw(device, view, projection);
                if(player.DrinkModel!= null)
                {
                    shader.draw((BFModel)(models[player.DrinkModel.ModelID]), player.DrinkModel.WorldMatrix, view, projection, "Simplest", lightPos, lightPower, ambientPower);
                }
            }

            //draw all of the drinks
            foreach(DrinkSpawner drink in drinks)
            {
                if(drink.HasDrink)
                shader.draw((BFModel)(models[drink.ModelID]), drink.WorldMatrix, view, projection, "Simplest", lightPos, lightPower, ambientPower);
            }

            if(hasWon)
            {
                sprbatch.Begin();
                sprbatch.DrawString(font,"Player " + (winner+1) + " has won! Press start to exit back to the menu",new Vector2(parent.ScreenWidth/2-(font.MeasureString("Player " + winner + " has won! Press start to exit back to the menu").X/2),parent.ScreenHeight/2
                    ),Color.GreenYellow);
                sprbatch.End();
                winSong.Stop();
            }
            
        }
        #endregion

        #region InitBuffers
        private void initBuffers(GraphicsDevice device)
        {
            Shader.Vertex[] vertices = new Shader.Vertex[4];
            //set the vertices for the vertices
            vertices[0] = new Shader.Vertex(new Vector3(-xWidth, 0, zWidth), new Vector2(0, 0), new Vector3(0, 1, 0));
            vertices[1] = new Shader.Vertex(new Vector3(-xWidth, 0, -zWidth), new Vector2(0, floorYDensity), new Vector3(0, 1, 0));
            vertices[2] = new Shader.Vertex(new Vector3(xWidth, 0, zWidth), new Vector2(floorXDensity, 0), new Vector3(0, 1, 0));
            vertices[3] = new Shader.Vertex(new Vector3(xWidth, 0, -zWidth), new Vector2(floorXDensity, floorYDensity), new Vector3(0, 1, 0));

            //set up the vertex buffer for the vertices
            floorBuffer = new VertexBuffer(device, Shader.Vertex.VertexDeclaration, 4, BufferUsage.WriteOnly);
            //set the data for the vertices vertex buffer
            floorBuffer.SetData(vertices);



            vertices[0] = new Shader.Vertex(new Vector3(-xWidth, yWidth, zWidth), new Vector2(0, 0), new Vector3(0, 1, 0));
            vertices[1] = new Shader.Vertex(new Vector3(-xWidth, 0, zWidth), new Vector2(0, wallYDensity), new Vector3(0, 1, 0));
            vertices[2] = new Shader.Vertex(new Vector3(xWidth, yWidth, zWidth), new Vector2(wallXDensity, 0), new Vector3(0, 1, 0));
            vertices[3] = new Shader.Vertex(new Vector3(xWidth, 0, zWidth), new Vector2(wallXDensity, wallYDensity), new Vector3(0, 1, 0));

            //set up the front wall vertex buffer
            frontWallBuffer = new VertexBuffer(device, Shader.Vertex.VertexDeclaration, 4, BufferUsage.WriteOnly);
            frontWallBuffer.SetData(vertices);

            //set up the data for the right wall
            vertices[0] = new Shader.Vertex(new Vector3(xWidth, yWidth, zWidth), new Vector2(0, 0), new Vector3(0, 1, 0));
            vertices[1] = new Shader.Vertex(new Vector3(xWidth, 0, zWidth), new Vector2(0, wallYDensity), new Vector3(0, 1, 0));
            vertices[2] = new Shader.Vertex(new Vector3(xWidth, yWidth, -zWidth), new Vector2(wallXDensity, 0), new Vector3(0, 1, 0));
            vertices[3] = new Shader.Vertex(new Vector3(xWidth, 0, -zWidth), new Vector2(wallXDensity, wallYDensity), new Vector3(0, 1, 0));

            rightWallBuffer = new VertexBuffer(device, Shader.Vertex.VertexDeclaration, 4, BufferUsage.WriteOnly);
            rightWallBuffer.SetData(vertices);

            //set up the data for the back wall
            vertices[0] = new Shader.Vertex(new Vector3(xWidth, yWidth, -zWidth), new Vector2(0, 0), new Vector3(0, 1, 0));
            vertices[1] = new Shader.Vertex(new Vector3(xWidth, 0, -zWidth), new Vector2(0, wallYDensity), new Vector3(0, 1, 0));
            vertices[2] = new Shader.Vertex(new Vector3(-xWidth, yWidth, -zWidth), new Vector2(wallXDensity, 0), new Vector3(0, 1, 0));
            vertices[3] = new Shader.Vertex(new Vector3(-xWidth, 0, -zWidth), new Vector2(wallXDensity, wallYDensity), new Vector3(0, 1, 0));

            //set the bufers for the back wall
            backWallBuffer = new VertexBuffer(device, Shader.Vertex.VertexDeclaration, 4, BufferUsage.WriteOnly);
            backWallBuffer.SetData(vertices);

            //set up the data for the left wall
            vertices[0] = new Shader.Vertex(new Vector3(-xWidth, yWidth, -zWidth), new Vector2(0, 0), new Vector3(0, 1, 0));
            vertices[1] = new Shader.Vertex(new Vector3(-xWidth, 0, -zWidth), new Vector2(0, wallYDensity), new Vector3(0, 1, 0));
            vertices[2] = new Shader.Vertex(new Vector3(-xWidth, yWidth, zWidth), new Vector2(wallXDensity, 0), new Vector3(0, 1, 0));
            vertices[3] = new Shader.Vertex(new Vector3(-xWidth, 0, zWidth), new Vector2(wallXDensity, wallYDensity), new Vector3(0, 1, 0));

            //set the buffer for the left wall
            leftWallBuffer = new VertexBuffer(device, Shader.Vertex.VertexDeclaration, 4, BufferUsage.WriteOnly);
            leftWallBuffer.SetData(vertices);

            //set up the data for the ceiling
            vertices[0] = new Shader.Vertex(new Vector3(-xWidth, yWidth, -zWidth), new Vector2(0, 0), new Vector3(0, 1, 0));
            vertices[1] = new Shader.Vertex(new Vector3(-xWidth, yWidth, zWidth), new Vector2(0, floorYDensity), new Vector3(0, 1, 0));
            vertices[2] = new Shader.Vertex(new Vector3(xWidth, yWidth, -zWidth), new Vector2(floorXDensity, 0), new Vector3(0, 1, 0));
            vertices[3] = new Shader.Vertex(new Vector3(xWidth, yWidth, zWidth), new Vector2(floorXDensity, floorYDensity), new Vector3(0, 1, 0));

            //set the buffer for the  ceiling

            ceilingBuffer = new VertexBuffer(device, Shader.Vertex.VertexDeclaration, 4, BufferUsage.WriteOnly);
            ceilingBuffer.SetData(vertices);



        }
        #endregion
    }
}
