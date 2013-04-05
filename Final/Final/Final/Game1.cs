using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SpriteClasses;
using Particles;
//using CameraLib;

namespace Final
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        #region Variables
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MouseState prevMouseState;
        KeyboardState prevKeyState;
        SpriteFont font;

        ParticleEngine particleEngine;
        ShopIcon laserIcon1, laserIcon2, laserIcon3, laserIcon4, laserIcon1G, laserIcon2G, laserIcon3G, laserIcon4G;
        ShopIcon shieldIcon1, shieldIcon2, shieldIcon3, shieldIcon4;
        enum GameState
        {
            mainMenu,
            options,
            playing,
            Shop
        }
        GameState currentGameState = GameState.mainMenu;
        enum laserState
        {
            One,
            Two,
            Three
        }
        laserState LaserState = laserState.One;
        enum ShieldState
        {
            None,
            One,
            Two,
            Three,
            Four
        }
        ShieldState shieldState = ShieldState.None;

        mButton btnPlay, btnOptions, btnShop;
        MultiBackground b;
        Sprite player, bg, gui;
        Texture2D playerTex, back, laser1;
        Texture2D laser01, laser3, shield1, shield2, shield3, shield4;
        Texture2D cursor;
        Vector2 cursorPos;
        Sprite playerShield;
        List<Sprite> laserList = new List<Sprite>();
        List<Sprite> enemyLaserList = new List<Sprite>();
        //List<SpriteFromSheet> enemyList = new List<SpriteFromSheet>();
        SpriteFromSheet enemy1;
        SpriteFromSheet enemy2;

        const float FIRE_DELAY = 150f;
        float fireDelay = FIRE_DELAY;
        const float ENEMY_FIRE_DELAY = 300f;
        float enemyFireDelay = ENEMY_FIRE_DELAY;


        float angle, angle2;
        #endregion
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
            graphics.ApplyChanges();
            //IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            
            playerTex = Content.Load<Texture2D>(@"Ship_Sprites/ship01");
            //back = Content.Load<Texture2D>("Background");
            back = Content.Load<Texture2D>("bg");
            player = new Sprite(playerTex, new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), new Vector2(5, 5),
                true, 0, 1f, SpriteEffects.None, null, 0);
            
            
            enemy1 = new SpriteFromSheet(Content.Load<Texture2D>("Ship_Sprites/Enemy/Ship_1_Sheet"), new Vector2(100, 100), new Vector2(0, 0), true, 0,
                0.4f, SpriteEffects.None, null, 0, new Vector2(92, 106), new Vector2(0, 0), new Vector2(2, 1), 0.1f);
            
            
            
            enemy2 = new SpriteFromSheet(Content.Load<Texture2D>("Ship_Sprites/Enemy/Ship_2_Sheet"), new Vector2(600, 100), new Vector2(0, 0), true, 0,
                0.4f, SpriteEffects.None, null, 0, new Vector2(90, 106), new Vector2(0, 0), new Vector2(2, 1), 0.1f);

            bg  = new Sprite(back, new Vector2(0, 0), new Vector2(0, 0), true, 0, 1f, SpriteEffects.None, null, 0);
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

            font = Content.Load<SpriteFont>("Font");
            #region particles
            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(Content.Load<Texture2D>("starSmall"));
            particleEngine = new ParticleEngine(textures, new Vector2(400, 240));
            #endregion

            #region lasers,GUI&Shields
            gui = new Sprite(Content.Load<Texture2D>("shopGUI"), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Vector2.Zero, true,
                0f, 1f, SpriteEffects.None, null, 0);

            laser1 = Content.Load<Texture2D>(@"Ship_sprites/lasers/laser1");
            laser01 = Content.Load<Texture2D>(@"Ship_sprites/lasers/laser01");
            laser3 = Content.Load<Texture2D>(@"Ship_sprites/lasers/laser3");

            shield1 = Content.Load<Texture2D>(@"Ship_sprites/shields/shield1");
            shield2 = Content.Load<Texture2D>(@"Ship_sprites/shields/shield2");
            shield3 = Content.Load<Texture2D>(@"Ship_sprites/shields/shield3");
            shield4 = Content.Load<Texture2D>(@"Ship_sprites/shields/shield4");
            playerShield = new Sprite((shield1), player.Position, Vector2.Zero, true, 0, 1f, SpriteEffects.None, null, 0);
            #endregion

            #region buttons
            btnPlay = new mButton(Content.Load<Texture2D>("play_button"), graphics.GraphicsDevice);
            btnPlay.setPosition(new Vector2(graphics.PreferredBackBufferWidth/2 -125, 300));
            btnOptions = new mButton(Content.Load<Texture2D>("options_button"), graphics.GraphicsDevice);
            btnOptions.setPosition(new Vector2(graphics.PreferredBackBufferWidth/2 -225, 400));
            btnShop = new mButton(Content.Load<Texture2D>("Shop_Button"), graphics.GraphicsDevice);
            btnShop.setPosition(new Vector2(graphics.PreferredBackBufferWidth - 250, 0));
            #endregion

            #region ShopIcons
            //------------Shop Icons-------------------------
            laserIcon1 = new ShopIcon(Content.Load<Texture2D>("ShopIcons\\Icon.1_42"), new Vector2(312, 163), 1, true, 50);
            laserIcon2 = new ShopIcon(Content.Load<Texture2D>("ShopIcons\\Icon.1_43"), new Vector2(312, 219), 1, true, 150);
            laserIcon3 = new ShopIcon(Content.Load<Texture2D>("ShopIcons\\Icon.1_44"), new Vector2(312, 275), 1, true, 300);
            laserIcon4 = new ShopIcon(Content.Load<Texture2D>("ShopIcons\\Icon.1_45"), new Vector2(312, 332), 1, true, 500);

            shieldIcon1 = new ShopIcon(Content.Load<Texture2D>("ShopIcons\\Icon.1_42"), new Vector2(372, 163), 1, true, 50);
            shieldIcon2 = new ShopIcon(Content.Load<Texture2D>("ShopIcons\\Icon.1_42"), new Vector2(372, 219), 1, true, 50);
            shieldIcon3 = new ShopIcon(Content.Load<Texture2D>("ShopIcons\\Icon.1_42"), new Vector2(372, 275), 1, true, 50);
            shieldIcon4 = new ShopIcon(Content.Load<Texture2D>("ShopIcons\\Icon.1_42"), new Vector2(372, 332), 1, true, 50);
            //----------Greyed Out Versions------------------
            laserIcon1G = new ShopIcon(Content.Load<Texture2D>("ShopIcons\\Icon.1_42_Grey"), new Vector2(312, 163), 1, true, 50);
            laserIcon2G = new ShopIcon(Content.Load<Texture2D>("ShopIcons\\Icon.1_43_Grey"), new Vector2(312, 219), 1, true, 150);
            laserIcon3G = new ShopIcon(Content.Load<Texture2D>("ShopIcons\\Icon.1_44_Grey"), new Vector2(312, 275), 1, true, 300);
            laserIcon4G = new ShopIcon(Content.Load<Texture2D>("ShopIcons\\Icon.1_45_Grey"), new Vector2(312, 332), 1, true, 500);
            #endregion


            cursor = Content.Load<Texture2D>("cursor");

            #region multiBack
            b = new MultiBackground(GraphicsDevice);
            b.AddLayer(back, 1, 5f);
            b.SetMoveLeftRight();
            b.StartMoving();
            #endregion

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
            MouseState mouse = Mouse.GetState();
            cursorPos = new Vector2(mouse.X -25, mouse.Y -25);
            float elapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            fireDelay -= elapsed;
            enemyFireDelay -= elapsed;


            switch (currentGameState)
            {

                #region ShopUpdate
                case GameState.Shop:

                    b.Update(gameTime);
                    particleEngine.Update();
                    UpdateInput();


                    break;
                #endregion

                #region MenuUpdate
                case GameState.mainMenu:
                    btnPlay.Update(mouse);
                    if (btnPlay.isClicked == true) currentGameState = GameState.playing;

                    btnOptions.UpdateLong(mouse);
                    if (btnOptions.isClicked == true) currentGameState = GameState.options;

                 

                    particleEngine.EmitterLocation = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2);
                    particleEngine.Update();
                    
                    
                    b.Update(gameTime);

                    break;
                #endregion

                #region PlayingUpdate
                case GameState.playing:
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    this.Exit();
           
                   
                    enemy2.Update(gameTime, GraphicsDevice);
                    player.Update(gameTime, GraphicsDevice);
                    enemy1.Update(gameTime);
                    playerShield.Update(gameTime);
                    playerShield.Position = player.Position;
                    UpdateInput();
                     for (int i = 0; i < laserList.Count; i++)
                      {

                        laserList[i].Update(gameTime);

                        if (laserList[i].Position.Y < 0f || laserList[i].Position.X < 0f || laserList[i].Position.X > graphics.GraphicsDevice.Viewport.Width
                        || laserList[i].Position.Y > graphics.GraphicsDevice.Viewport.Height)
                        {
                            laserList.RemoveAt(i);

                        }


                    }

                     for (int i = 0; i < enemyLaserList.Count; i++)
                     {
                         enemyLaserList[i].Update(gameTime);

                         if (enemyLaserList[i].IsOffScreen(GraphicsDevice))
                         {
                             enemyLaserList.RemoveAt(i);
                         }


                     }

                    break;
                #endregion

                #region optionsUpdate
                case GameState.options:
                    
                    
                    break;
                #endregion
            }
            // Allows the game to exit
            

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
           
           
            switch (currentGameState)
            {
                #region ShopDraw
                case GameState.Shop:

                    spriteBatch.Begin();

                    b.Draw();
                   // particleEngine.Draw(spriteBatch);
                    gui.Draw(gameTime, spriteBatch);
                   
                   
                        laserIcon1G.Draw(gameTime, spriteBatch);
                        laserIcon2G.Draw(gameTime, spriteBatch);
                        laserIcon3G.Draw(gameTime, spriteBatch);
                        laserIcon4G.Draw(gameTime, spriteBatch);

                       /* shield1.Draw(gameTime, spriteBatch);
                        shield2.Draw(gameTime, spriteBatch);
                        shield3.Draw(gameTime, spriteBatch);
                        shield4.Draw(gameTime, spriteBatch);
                    */
                        string laser1String = "Laser 1.0";
                        string laser2String = "Laser 1.5";
                        string laser3String = "Laser 2.0";
                        string laser4String = "Laser 2.5";
                        //string error = "Requires Previous Component"; 

                      
                        Color fontColor = Color.LightBlue;
                        MouseState mouse = Mouse.GetState();  
                        //Vector2 loc = new Vector2(mouse.X, mouse.Y);
                        //string locstring = loc.ToString();
                        Rectangle descRect = new Rectangle(300, 425, 200, 35);
                        Vector2 descLoc = new Vector2(300, 410);
                        
                  

                        if (laserIcon1G.CollisionRectangle.Contains(mouse.X, mouse.Y))
                        {
                            spriteBatch.DrawString(font, laser1String, descLoc, fontColor);
                        }
                        if (laserIcon2G.CollisionRectangle.Contains(mouse.X, mouse.Y))
                        {
                            spriteBatch.DrawString(font, laser2String, descLoc, fontColor);
                        }
                        if (laserIcon3G.CollisionRectangle.Contains(mouse.X, mouse.Y))
                        {
                            spriteBatch.DrawString(font, laser3String, descLoc, fontColor);
                        }
                        if (laserIcon4G.CollisionRectangle.Contains(mouse.X, mouse.Y))
                        {
                            spriteBatch.DrawString(font, laser4String, descLoc, fontColor);
                        }
                        drawError(gameTime);
                       
                    spriteBatch.Draw(cursor, cursorPos, Color.White);
                    spriteBatch.End();

                  
                    break;
                #endregion

                #region MenuDraw
                case GameState.mainMenu:
                    
                    spriteBatch.Begin();
                   //spriteBatch.Draw(back, new Rectangle(0, 0, 800, 600), Color.White);
                    b.Draw();

                    particleEngine.Draw(spriteBatch);
                    
                    btnPlay.Draw(spriteBatch);
                    btnOptions.Draw(spriteBatch);
                  
                    spriteBatch.Draw(cursor, cursorPos, Color.White);
                    
                    spriteBatch.End();

                    break;
                #endregion

                #region gameDraw
                case GameState.playing:

                    spriteBatch.Begin();
                    spriteBatch.Draw(Content.Load<Texture2D>("Background"), new Rectangle(0, 0, 800, 600), Color.White);
                    enemy2.Draw(gameTime, spriteBatch);
                    player.Draw(gameTime, spriteBatch);
                    enemy1.Draw(gameTime, spriteBatch);
                    spriteBatch.Draw(cursor, cursorPos, Color.White);
                    foreach (Sprite lasers in laserList)
                    {
                       switch(LaserState)
                       {
                           case laserState.One:

                               lasers.Draw(gameTime, spriteBatch);
                               
                               break;
                           case laserState.Two:

                               lasers.Draw(gameTime, spriteBatch);

                               break;
                           case laserState.Three:
                               lasers.Draw(gameTime, spriteBatch);
                               break;
                    }
                       
                      

                    }

                    foreach (Sprite laser in enemyLaserList)
                    {
                        laser.Draw(gameTime, spriteBatch);

                    }
                     switch (shieldState)
                       {
                           case ShieldState.One:
                               playerShield.TextureImage = shield1;
                               break;
                           case ShieldState.Two:
                               playerShield.TextureImage = shield2;
                               break;
                           case ShieldState.Three:
                               playerShield.TextureImage = shield3;
                               break;
                           case ShieldState.Four:
                               playerShield.TextureImage = shield4;
                               break;

                       }
                       playerShield.Draw(gameTime, spriteBatch);
                    btnShop.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
                #endregion

                #region optionsDraw
                case GameState.options:

                    

                    break;
                #endregion
            }

          
            base.Draw(gameTime);
        }


        private void UpdateInput()
        {
            bool keyPressed = false;
            bool keyPressed2 = false;
            KeyboardState keyState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            //MouseState currMouseState = Mouse.GetState();

            switch (currentGameState)
            {
                
                    #region GameInput
                case GameState.playing:
            MouseState currMouseState = Mouse.GetState();
            if (keyState.IsKeyDown(Keys.Up)
              //|| keyState.IsKeyDown(Keys.W)
              || gamePadState.DPad.Up == ButtonState.Pressed
              || Math.Abs(gamePadState.ThumbSticks.Left.Y) > 0)
            {
                player.Up();
                //p1.Up();
                keyPressed = true;
            }
            if (keyState.IsKeyDown(Keys.Down)
              //|| keyState.IsKeyDown(Keys.S)
              || gamePadState.DPad.Down == ButtonState.Pressed
              || gamePadState.ThumbSticks.Left.Y < -0.5f)
            {
                player.Down();
                //p1.Down();
                keyPressed = true;
            }
            if (keyState.IsKeyDown(Keys.Left)
              //|| keyState.IsKeyDown(Keys.A)
              || gamePadState.DPad.Left == ButtonState.Pressed
              || gamePadState.ThumbSticks.Left.X < -0.5f)
            {
                player.Left();
                //p1.Left();
                keyPressed = true;
            }
            if (keyState.IsKeyDown(Keys.Right)
              //|| keyState.IsKeyDown(Keys.D)
              || gamePadState.DPad.Right == ButtonState.Pressed
              || gamePadState.ThumbSticks.Left.X > 0.5f)
            {
                player.Right();
                //p1.Right();
                keyPressed = true;
            }
            if (keyState.IsKeyDown(Keys.W))
            {
                //p2.Up();
                keyPressed2 = true;
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                //p2.Left();
                keyPressed2 = true;
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                //p2.Down();
                keyPressed2 = true;
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                //p2.Right();
                keyPressed2 = true;
            }
            if (keyState.IsKeyDown(Keys.H) && prevKeyState != keyState)
            {
                currentGameState = GameState.Shop;
                
                
            }
           



            if (!keyPressed)
            {
                player.Idle();

                //p1.Idle();
                
            } 
            if (!keyPressed2)
            {
                //player.Idle();
                //p2.Idle();
            }
           
           
          

            if (currMouseState.X != prevMouseState.X ||
                currMouseState.Y != prevMouseState.Y)
            {
                //player.Rotation
                Vector2 mouseLoc = new Vector2(currMouseState.X, currMouseState.Y);
                Vector2 direction = (player.Position) - mouseLoc;
                angle = (float)(Math.Atan2(-direction.Y, -direction.X));
                player.Rotation = angle + (float)45.5;
            } 
                //enemies rotation
                Vector2 direction2 = (player.Position) - (enemy1.Position);
                angle2 = (float)(Math.Atan2(-direction2.Y, -direction2.X));
                enemy1.Rotation = angle2 + (float)45.5;

                //enemies moving towards player
                direction2.Normalize();
                enemy1.Velocity = new Vector2(1, 1);
               
                if (Vector2.Distance(enemy1.Position, player.Position) < 150)
                {
                    enemy1.Velocity *= -enemy1.Velocity;
                    startShooting();

                }
               enemy1.Position += enemy1.Velocity * direction2;

             if (keyState.IsKeyDown(Keys.Q))
             {
                 //LaserState = laserState.One;
             }
             if (keyState.IsKeyDown(Keys.W))
             {
                 //LaserState = laserState.Two;
             }
             if (keyState.IsKeyDown(Keys.E))
             {
                 enemy1.Velocity = new Vector2(0, 5);
             }
            


            if (currMouseState.LeftButton == ButtonState.Pressed){
                if (fireDelay <= 0f)
                {

                    switch (LaserState)
                    {


                        case laserState.One:
                            Sprite laserShot = new Sprite(laser1, new Vector2(player.Position.X - laser1.Bounds.X / 2, player.Position.Y - laser1.Bounds.Y / 2),
                                new Vector2((float)Math.Cos((angle)), (float)Math.Sin((angle))) * 600f, true, 0, 1f, SpriteEffects.None, null, 0);
                            laserShot.Rotation = angle + (float)45.5;

                            laserList.Add(laserShot);

                            break;


                        case laserState.Two:
                            Sprite laserShot2 = new Sprite(laser01, new Vector2(player.Position.X - laser01.Bounds.X / 2, player.Position.Y - laser01.Bounds.Y / 2),
                                        new Vector2((float)Math.Cos((angle)), (float)Math.Sin((angle))) * 600f, true, 0, 1f, SpriteEffects.None, null, 0);
                            laserShot2.Rotation = angle + (float)45.5;

                            laserList.Add(laserShot2);

                            break;
                        case laserState.Three:
                            Sprite laserShot3 = new Sprite(laser3, new Vector2(player.Position.X - laser01.Bounds.X / 2, player.Position.Y - laser01.Bounds.Y / 2),
                                        new Vector2((float)Math.Cos((angle)), (float)Math.Sin((angle))) * 600f, true, 0, 0.2f, SpriteEffects.None, null, 0);
                            laserShot3.Rotation = angle + (float)45.5;

                            laserList.Add(laserShot3);

                            break;
                    }
                    fireDelay = FIRE_DELAY;
                }

        }
            prevMouseState = currMouseState;
            prevKeyState = keyState;
        
              break;
                #endregion


                #region ShopInput
                case GameState.Shop:
              MouseState currMouseState2 = Mouse.GetState();
              if (currMouseState2.LeftButton == ButtonState.Pressed)
              {
               
                  Vector2 mouseloc = new Vector2(currMouseState2.X, currMouseState2.Y);
                  Console.WriteLine(mouseloc);
                  
                  if(laserIcon1G.CollisionMouse(currMouseState2.X, currMouseState2.Y))
                  {
                      //Console.WriteLine("HITIIIT");
                      laserIcon1G.TextureImage = Content.Load<Texture2D>("ShopIcons\\Icon.1_42");
                      laserIcon1G.active = true;
                      LaserState = laserState.Two;
                      shieldState = ShieldState.One;
                  }
                  
                  if ((laserIcon2G.CollisionMouse(currMouseState2.X, currMouseState2.Y)) && laserIcon1G.active == true)
                  {
                      //Console.WriteLine("HITIIIT");
                      laserIcon2G.TextureImage = Content.Load<Texture2D>("ShopIcons\\Icon.1_43");
                      laserIcon2G.active = true;
                      LaserState = laserState.Three;
                      shieldState = ShieldState.Two;
                  }
                  if ((laserIcon3G.CollisionMouse(currMouseState2.X, currMouseState2.Y)) && laserIcon2G.active == true)
                  {
                      //Console.WriteLine("HITIIIT");
                      laserIcon3G.TextureImage = Content.Load<Texture2D>("ShopIcons\\Icon.1_44");
                      laserIcon3G.active = true;
                      shieldState = ShieldState.Three;
                  }
                  if ((laserIcon4G.CollisionMouse(currMouseState2.X, currMouseState2.Y)) && laserIcon3G.active == true)
                  {
                      //Console.WriteLine("HITIIIT");
                      laserIcon4G.TextureImage = Content.Load<Texture2D>("ShopIcons\\Icon.1_45");
                      laserIcon4G.active = true;
                      shieldState = ShieldState.Four;
                  }





                  //currentGameState = GameState.mainMenu;
                  if(currMouseState2 != prevMouseState)
                  {
                    if (currMouseState2.X >= 524 && currMouseState2.Y >= 94
                        && currMouseState2.X <= 540 && currMouseState2.Y <= 98)
                    {
                        currentGameState = GameState.playing;
                        break;
                    }
                  }

              }
              else
              {
                  //Console.WriteLine("released");
              }
              if (keyState.IsKeyDown(Keys.H) && (currentGameState == GameState.Shop) && prevKeyState != keyState)
              {
                  currentGameState = GameState.playing;
              }

              prevMouseState = currMouseState2;
              prevKeyState = keyState;

            break;
                #endregion



            }//end switch
          

        }//end updateInput

        private void spawnEnemies()
        {
            spawnSmallEnemies();
            spawnMediumEnemies();
            spawnBigEnemies();

        }
        private void spawnSmallEnemies()
        {




        }
        private void spawnMediumEnemies()
        {




        }
        private void spawnBigEnemies()
        {




        }
        private void startShooting()
        {
            if (enemyFireDelay <= 0f)
            {

                Sprite enemyLaser = new Sprite(Content.Load<Texture2D>(@"Ship_Sprites/Lasers/laser3"), new Vector2(enemy1.Position.X - laser3.Bounds.X / 2,
                    enemy1.Position.Y - laser3.Bounds.Y / 2), new Vector2((float)Math.Cos((-angle2)), (float)Math.Sin((angle2))) * -600f,
                    true, 0, 0.2f, SpriteEffects.None, null, 0);

                enemyLaserList.Add(enemyLaser);
                enemyFireDelay = ENEMY_FIRE_DELAY;
            } 
        }

        private void drawError(GameTime gameTime)
        {
            string error = "Requires Previous \n    Component";
            Color fontColor = Color.LightBlue;
            MouseState mouse = Mouse.GetState();
            Vector2 descLoc = new Vector2(300, 430);



            if (laserIcon2G.CollisionRectangle.Contains(mouse.X, mouse.Y) && laserIcon1G.active == false)
            {
                spriteBatch.DrawString(font, error, descLoc, fontColor);
            }
            if (laserIcon3G.CollisionRectangle.Contains(mouse.X, mouse.Y) && laserIcon2G.active == false)
            {
                spriteBatch.DrawString(font, error, descLoc, fontColor);
            }
            if (laserIcon4G.CollisionRectangle.Contains(mouse.X, mouse.Y) && laserIcon3G.active == false)
            {
                spriteBatch.DrawString(font, error, descLoc, fontColor);
            }

           
        }


    }//End Class
}
