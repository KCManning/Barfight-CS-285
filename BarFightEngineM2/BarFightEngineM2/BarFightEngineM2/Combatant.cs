using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using AnimationAux;

namespace BarFightEngineM2
{
    public enum Stance
    {
        LEFTPUNCH,
        RIGHTPUNCH,
        RIGHTJAB,
        LEFTJAB,
        FLINCHING,
        BLOCKING,
        DRINKING,
        NOTHING

        
    }

    /// <summary>
    /// A class for player characters
    /// </summary>
    public class Combatant : RefModel
    {
        private AABB leftFist;
        private AABB rightFist;
        private PlayerIndex player;
        private float lastX, lastY, lastZ;
        public AnimatedModel model;
        //private bool isPunching = false;
        //private bool isBlocking = false;
        //private bool isCringing = false;
        private const float PUNCHDAMAGE = 10;
        private const float JABDAMAGE = 5;
        private const float PUNCHSPEED = 2;
        private const float JABSPEED = 5;
        private Stance stance;
        private bool left;
        private float hp;
        private float drunkBuffer;
        private const float drunkDepletion = 0.05f;
        private float animationSpeed;
        private DrinkSpawner drink;
        public static Menu PauseMenu;
        public static bool isPaused = false;
        private SoundEffectInstance punch1, punch2, punch3, punch4;
        private int leftRumbleStrength = 0,rightRumbleStrength;
        private  Game1 parent;
        private bool isConnected;
        public static int disconnected;
        private static Room room;
        public Combatant(Game1 parent,Room room) : base()
        {
            leftFist = new AABB(0, 0, 0, 0.5f, 0.5f, 0.5f);
            rightFist = new AABB(0,0,0, 0.5f, 0.5f, 0.5f);
            hp = 100;
            drunkBuffer = 0;
            this.parent = parent;
            punch1 = parent.Content.Load<SoundEffect>("punch1").CreateInstance();
            punch2 = parent.Content.Load<SoundEffect>("punch2").CreateInstance();
            punch3 = parent.Content.Load<SoundEffect>("punch3").CreateInstance();
            punch4 = parent.Content.Load<SoundEffect>("punch4").CreateInstance();
            punch1.IsLooped = punch2.IsLooped = punch3.IsLooped = punch4.IsLooped = false;
            disconnected = -1;
            Combatant.room = room;
        }

        public void loadCombatant(ContentManager content,string combatDef,PlayerIndex player)
        {
            this.player = player;
            BFFileReader reader = new BFFileReader(combatDef);
            List<Texture2D> textures = new List<Texture2D>();
            reader.Open();
            string line = "";
            #region Combatant parser
            while ((line = reader.ReadLine()) != null)
            {
                string[] param = line.Split(' ');
                switch(param[0])
                {
                    case "ModelID":
                        {
                            ModelID = param[1];
                            break;
                        }
                    case "HalfWidth":
                        {
                            HalfWidth = float.Parse(param[1]);
                            break;
                        }
                    case "HalfHeight":
                        {
                            HalfHeight = float.Parse(param[1]);
                            break;
                        }
                    case "HalfLength":
                        {
                            HalfLength = float.Parse(param[1]);
                            break;
                        }
                    case "Radius":
                        {
                            Radius = float.Parse(param[1]);
                            break;
                        }
                    case "TransX":
                        {
                            TransX = float.Parse(param[1]);
                            break;
                        }
                    case "TransY":
                        {
                            TransY = float.Parse(param[1]);
                            break;
                        }
                    case "TransZ":
                        {
                            TransZ = float.Parse(param[1]);
                            break;
                        }
                    case "Texture":
                        {
                            textures.Add(content.Load<Texture2D>(param[1]));
                            break;
                        }
                    case "CollideType":
                        {
                            switch (param[1].ToUpper())
                            {
                                case "AABB":
                                    {
                                        ORIGEN = new AABB();
                                        CollisionType = Origen.Collision_Type.AABB;

                                        break;
                                    }
                                case "NONE":
                                    {
                                        CollisionType = Origen.Collision_Type.NONE;
                                        break;
                                    }
                                case "BC":
                                    {
                                        ORIGEN = new BC();
                                        CollisionType = Origen.Collision_Type.BC;
                                        break;
                                    }
                                case "BS":
                                    {
                                        ORIGEN = new BS();
                                        CollisionType = Origen.Collision_Type.BS;
                                        break;
                                    }
                            }
                            break;
                        }

                }
            }
            #endregion
            reader.Close();
            model = new AnimatedModel(ModelID);
            model.LoadContent(content);
            //load the textures for the model
            model.textures = textures.ToArray();
            //play an animation
            model.PlayAnimation("TakePunchLeft", false);
            RotX -= (float)Math.PI/2;
        }
        public Stance AttackStance
        {
            get
            {
                return stance;
            }
            set
            {
                stance = value;
            }
        }

        public void damage(Combatant attacker)
        {
            //attack the player
            switch(attacker.stance)
            {
                case Stance.NOTHING:
                    {
                        //attacker is not doing anything so return
                        return;
                    }
                case Stance.LEFTJAB:
                    {
                        if(stance != Stance.FLINCHING)
                        {
                            //check to see if the player is blocking and if the player is do quarter damage
                            if(stance == Stance.BLOCKING)
                            {
                                //make the player flinch
                                stance = Stance.FLINCHING;
                                //play the animation
                                model.PlayAnimation("TakePunchLeft",false);
                                punch1.Play();
                                animationSpeed = 5;
                                //check to see if the drunk buffer is greater then 0 and if it is damage the drunk buffer instead.
                                if (drunkBuffer > 0)
                                {
                                    DrunkBuffer -= JABDAMAGE / 4;
                                }
                                else
                                {
                                    HP -= JABDAMAGE / 4;
                                }
                                leftRumbleStrength = (int)JABDAMAGE*10 / 4;

                            }
                            else
                            {
                                //make the player flinch
                                stance = Stance.FLINCHING;
                                //play the animation
                                model.PlayAnimation("TakePunchLeft", false);
                                punch1.Play();
                                animationSpeed = 3;
                                //check to see if the drunk buffer is greater then 0 and if it is damage the drunk buffer instead.
                                if (drunkBuffer > 0)
                                {
                                    DrunkBuffer -= JABDAMAGE;
                                }
                                else
                                {
                                    HP -= JABDAMAGE;
                                }
                                leftRumbleStrength = (int)JABDAMAGE * 10;
                            }
                        }
                        break;
                    }
                case Stance.RIGHTJAB:
                    {
                        if (stance != Stance.FLINCHING)
                        {
                            //check to see if the player is blocking and if the player is do quarter damage
                            if (stance == Stance.BLOCKING)
                            {
                                //make the player flinch
                                stance = Stance.FLINCHING;
                                //play the animation
                                model.PlayAnimation("TakePunchRight", false);
                                punch2.Play();
                                animationSpeed = 5;
                                //check to see if the drunk buffer is greater then 0 and if it is damage the drunk buffer instead.
                                if (drunkBuffer > 0)
                                {
                                    DrunkBuffer -= JABDAMAGE / 4;
                                }
                                else
                                {
                                    HP -= JABDAMAGE / 4;
                                }
                                rightRumbleStrength = (int)JABDAMAGE * 10 / 4;

                            }
                            else
                            {
                                //make the player flinch
                                stance = Stance.FLINCHING;
                                //play the animation
                                model.PlayAnimation("TakePunchRight", false);
                                punch2.Play();
                                animationSpeed = 3;
                                //check to see if the drunk buffer is greater then 0 and if it is damage the drunk buffer instead.
                                if (drunkBuffer > 0)
                                {
                                    DrunkBuffer -= JABDAMAGE;
                                }
                                else
                                {
                                    HP -= JABDAMAGE;
                                }
                                rightRumbleStrength = (int)JABDAMAGE * 10;
                            }
                        }
                        break;
                    }
                case Stance.LEFTPUNCH:
                    {
                        if (stance != Stance.FLINCHING)
                        {
                            //check to see if the player is blocking and if the player is do quarter damage
                            if (stance == Stance.BLOCKING)
                            {
                                //make the player flinch
                                stance = Stance.FLINCHING;
                                //play the animation
                                model.PlayAnimation("TakePunchLeft", false);
                                animationSpeed = 4;
                                punch3.Play();
                                //check to see if the drunk buffer is greater then 0 and if it is damage the drunk buffer instead.
                                if (drunkBuffer > 0)
                                {
                                    DrunkBuffer -= PUNCHDAMAGE / 4;
                                }
                                else
                                {
                                    HP -= PUNCHDAMAGE / 4;
                                }
                                leftRumbleStrength = (int)PUNCHDAMAGE * 10 / 4;
                            }
                            else
                            {
                                //make the player flinch
                                stance = Stance.FLINCHING;
                                //play the animation
                                model.PlayAnimation("TakePunchLeft", false);
                                animationSpeed = 2;
                                punch3.Play();
                                //check to see if the drunk buffer is greater then 0 and if it is damage the drunk buffer instead.
                                if (drunkBuffer > 0)
                                {
                                    DrunkBuffer -= PUNCHDAMAGE;
                                }
                                else
                                {
                                    HP -= PUNCHDAMAGE;
                                }
                                leftRumbleStrength = (int)PUNCHDAMAGE * 10;
                            }
                        }
                        break;
                    }
                case Stance.RIGHTPUNCH:
                    {
                        if (stance != Stance.FLINCHING)
                        {
                            //check to see if the player is blocking and if the player is do quarter damage
                            if (stance == Stance.BLOCKING)
                            {
                                //make the player flinch
                                stance = Stance.FLINCHING;
                                //play the animation
                                model.PlayAnimation("TakePunchRight", false);
                                animationSpeed = 4;
                                punch4.Play();
                                //check to see if the drunk buffer is greater then 0 and if it is damage the drunk buffer instead.
                                if (drunkBuffer > 0)
                                {
                                    DrunkBuffer -= PUNCHDAMAGE / 4;
                                }
                                else
                                {
                                    HP -= PUNCHDAMAGE / 4;
                                }
                                rightRumbleStrength = (int)PUNCHDAMAGE * 10 / 4;

                            }
                            else
                            {
                                //make the player flinch
                                stance = Stance.FLINCHING;
                                //play the animation
                                model.PlayAnimation("TakePunchRight", false);
                                animationSpeed = 2;
                                punch4.Play();
                                //check to see if the drunk buffer is greater then 0 and if it is damage the drunk buffer instead.
                                if (drunkBuffer > 0)
                                {
                                    DrunkBuffer -= PUNCHDAMAGE;
                                }
                                else
                                {
                                    HP -= PUNCHDAMAGE;
                                }
                                rightRumbleStrength = (int)PUNCHDAMAGE * 10;
                            }
                        }
                        break;
                    }

            }

            
        }

        public void Drink(DrinkSpawner drink)
        {
            if(stance != Stance.DRINKING)
            {
                stance = Stance.DRINKING;
                this.drink = drink;
                DrunkBuffer += drink.DrinkStrength;
            }
            ////play the drink animation 
            //stance = Stance.DRINKING;
            //if (!model.isPlaying("GrabDrink"))
            //{
            //    //play the drinking aniamtion
            //    model.PlayAnimation("GrabDrink", false);
            //    this.drink = drink;
            //    DrunkBuffer += drink.DrinkStrength;
            //    animationSpeed = 1;
            //    DrunkBuffer += drink.DrinkStrength;
            //}

        }

        public float LastX
        {
            get
            {
                return lastX;
            }
            set
            {
                lastX = value;
            }
        }

        public float LastY
        {
            get
            {
                return lastY;
            }
            set
            {
                lastY = value;
            }
        }

        public float LastZ
        {
            get
            {
                return lastZ;
            }
            set
            {
                lastZ = value;
            }
        }

        public float HP
        {
            get
            {
                return hp;
            }
            set
            {
                hp = value;
            }
        }

        public float DrunkBuffer
        {
            get
            {
                return drunkBuffer;
            }
            set
            {
                drunkBuffer = value;
                if(drunkBuffer < 0)
                {
                    drunkBuffer = 0;
                }
            }
        }

        public PlayerIndex Index
        {
            get
            {
                return this.player;
            }
        }

        public AABB LeftFist
        {
            get
            {
                return leftFist;
            }
        }

        public bool IsDead
        {
            get
            {
                return (HP <= 0);
            }
        }

        public AABB RightFist
        {
            get
            {
                return rightFist;
            }
        }

        public void update(float frameMultiplier)
        {
            GamePadState state = GamePad.GetState(player);

            if(!state.IsConnected)
            {
                isConnected = false;
                isPaused = true;
                disconnected = (int)player;
            }

            if(!isConnected)
            {
                if(state.IsConnected)
                {
                    isPaused = false;
                    isConnected = true;
                    disconnected = -1;
                }
            }

            //check to see if the player is pausing
            if(state.Buttons.Start == ButtonState.Pressed)
            {
                isPaused = true;
            }
            PauseMenu.update(state);
            if(!isPaused)
            {
                if(!IsDead)
                {
                    switch(stance)
                    {
                        case Stance.FLINCHING:
                            {
                                if(model.isFinished())
                                {
                                    stance = Stance.NOTHING;
                                }
                                leftRumbleStrength -= 20;
                                rightRumbleStrength -= 20;
                                break;
                                ;
                            }
                        case Stance.DRINKING:
                            {
                                //check to see if the drink animation is playing
                                if(!model.isPlaying("GrabDrink"))
                                {
                                    model.PlayAnimation("GrabDrink", false);
                                    animationSpeed = 1;
                                }
                                
                                if(model.isFinished())
                                {
                                    drink = null;
                                    stance = Stance.NOTHING;
                                }
                                else
                                {
                                    //update the drink
                                    Vector3 newDrinkPos = Vector3.Transform(model.getBone("palm_middle_R").Translation, WorldMatrix);
                                    drink.X = newDrinkPos.X;
                                    drink.Y = newDrinkPos.Y;
                                    drink.Z = newDrinkPos.Z;
                                    drink.RotX = model.getBone("palm_middle_R").Down.X;
                                    drink.RotY = model.getBone("palm_middle_R").Up.Y;
                                    drink.RotZ = model.getBone("palm_middle_R").Left.Z;
                                    drink.updateMatrix();
                                }
                                break;
                            }
                        case Stance.NOTHING:
                            {
                                leftRumbleStrength = rightRumbleStrength = 0;
                                //check for attacks

                                //check for jab
                                if(state.Buttons.Y == ButtonState.Pressed)
                                {
                                    if(left)
                                    {
                                        stance = Stance.LEFTJAB;
                                    }
                                    else
                                    {
                                        stance = Stance.RIGHTJAB;
                                    }
                                }

                                //check for punch
                                if(state.Buttons.X == ButtonState.Pressed)
                                {
                                    if(left)
                                    {
                                        stance = Stance.LEFTPUNCH;
                                    }
                                    else
                                    {
                                        stance = Stance.RIGHTPUNCH;
                                    }
                                }

                                //check for block
                                if(state.Buttons.RightShoulder == ButtonState.Pressed)
                                {
                                    stance = Stance.BLOCKING;
                                }

                                if (Math.Abs(state.ThumbSticks.Left.X / 2) > 0.1 || Math.Abs(state.ThumbSticks.Left.Y / 2) > 0.1)
                                {
                                    if (!model.isPlaying("WalkFight"))
                                    {
                                        model.PlayAnimation("WalkFight", true);
                                    }
                                    LastX = X;
                                    X = X - (state.ThumbSticks.Left.X / 2);//*frameMultiplier);
                                    LastZ = Z;
                                    Z = Z + (state.ThumbSticks.Left.Y / 2);//*frameMultiplier);
                                    RotY = (float)Math.Atan2(state.ThumbSticks.Left.X * -1, state.ThumbSticks.Left.Y);
                                    animationSpeed = (float)Math.Sqrt(Math.Pow(state.ThumbSticks.Left.X * 4, 2) + Math.Pow(state.ThumbSticks.Left.Y * 4, 2));
                                }
                                else
                                {
                                    animationSpeed = 1;
                                    if (!model.isPlaying("StandingStill"))
                                    {
                                        model.PlayAnimation("StandingStill", true);
                                    }
                                }
                                break;
                            }
                        case Stance.BLOCKING:
                            {
                                //check to see if the blocking animation is playing and if it is not play it
                                if(!model.isPlaying("Blocking"))
                                {
                                    //play the block animation
                                    model.PlayAnimation("Blocking", false);
                                    animationSpeed = 1;
                                }

                                //check to see if the block animation is finished and is it is stop blocking
                                if(model.isFinished())
                                {
                                    stance = Stance.NOTHING;
                                }

                                break;
                            }
                        case Stance.LEFTJAB:
                            {
                                //check to see if the jabbing animation is playing and if it is not play it
                                if(!model.isPlaying("LeftJab"))
                                {
                                    model.PlayAnimation("LeftJab", false);
                                    animationSpeed = JABSPEED;
                                }

                                //check to see if it is finished
                                if(model.isFinished())
                                {
                                    left = false;
                                    stance = Stance.NOTHING;
                                }
                                break;
                            }
                        case Stance.RIGHTJAB:
                            {
                                if(!model.isPlaying("RightJab"))
                                {
                                    model.PlayAnimation("RightJab", false);
                                    animationSpeed = JABSPEED;
                                }

                                if(model.isFinished())
                                {
                                    left = true;
                                    stance = Stance.NOTHING;
                                }
                                break;
                            }
                        case Stance.LEFTPUNCH:
                            {
                                if(!model.isPlaying("LeftPunch"))
                                {
                                    model.PlayAnimation("LeftPunch", false);
                                    animationSpeed = PUNCHSPEED;
                                }

                                if(model.isFinished())
                                {
                                    left = false;
                                    stance = Stance.NOTHING;
                                }
                                break;
                            }

                        case Stance.RIGHTPUNCH:
                            {
                                if(!model.isPlaying("RightPunch"))
                                {
                                    model.PlayAnimation("RightPunch", false);
                                    animationSpeed = PUNCHSPEED;
                                }

                                if(model.isFinished())
                                {
                                    left = true;
                                    stance = Stance.NOTHING;
                                }
                                break;
                            }
                    }
                    model.Update(animationSpeed);
                    DrunkBuffer -= drunkDepletion;
                    //update the matrix
                    updateMatrix();
                    //update the fists positions

                    //update the left fist
                    Vector3 point = Vector3.Transform(model.getBone("hand_L").Translation, WorldMatrix);
                    leftFist.OrigenX = point.X;
                    leftFist.OrigenY = point.Y;
                    leftFist.OrigenZ = point.Z;

                    //update the right fist
                    point = Vector3.Transform(model.getBone("hand_R").Translation, WorldMatrix);
                    rightFist.OrigenX = point.X;
                    rightFist.OrigenY = point.Y;
                    rightFist.OrigenZ = point.Z;
                    GamePad.SetVibration(player, leftRumbleStrength / 100.0f, rightRumbleStrength / 100.0f);
                }
                else
                {
                    //make sure the stance is nothing
                    stance = Stance.NOTHING;
                    GamePad.SetVibration(player, 0, 0);
                    //player is dead so play the death animation
                    if(!model.isPlaying("KnockOut"))
                    {
                        model.PlayAnimation("KnockOut", false);
                    }
                    model.Update(animationSpeed);
                }
            }
            else
            {
                if ((PauseMenu.IsSelected(0) && state.Buttons.X == ButtonState.Pressed))
                {
                    isPaused = false;
                }
                else
                if (PauseMenu.IsSelected(1) && state.Buttons.X == ButtonState.Pressed)
                {
                    isPaused = false;
                    parent.resetTimers();
                    parent.playMusic();
                    room.stopSound();
                    parent.gameState = Game1.GameState.MainMenu;

                }
            }
            


        }

        public void draw(GraphicsDevice device,Matrix view,Matrix projection)
        {
            model.Draw(device, WorldMatrix, view, projection);
        }

        public DrinkSpawner DrinkModel
        {
            get
            {
                return drink;
            }
            set
            {
                drink = value;
            }

        }
    }
}