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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MouseState prevMouseState;

        ParticleEngine particleEngine;

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


        mButton btnPlay, btnOptions, btnShop;



        MultiBackground b;
        Sprite player, bg, gui;
        Texture2D playerTex, back, laser1;
        Texture2D laser01;
        Texture2D cursor;
        Vector2 cursorPos;
        List<Sprite> laserList = new List<Sprite>();

        const float FIRE_DELAY = 150f;
        float fireDelay = FIRE_DELAY;
        

        float angle;

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




       



            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(Content.Load<Texture2D>("starSmall"));
            particleEngine = new ParticleEngine(textures, new Vector2(400, 240));

            gui = new Sprite(Content.Load<Texture2D>("shopGUI"), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Vector2.Zero, true,
                0f, 1f, SpriteEffects.None, null, 0);


            laser1 = Content.Load<Texture2D>(@"Ship_sprites/lasers/laser1");
            laser01 = Content.Load<Texture2D>(@"Ship_sprites/lasers/laser01");

            btnPlay = new mButton(Content.Load<Texture2D>("play_button"), graphics.GraphicsDevice);
            btnPlay.setPosition(new Vector2(graphics.PreferredBackBufferWidth/2 -125, 300));
            btnOptions = new mButton(Content.Load<Texture2D>("options_button"), graphics.GraphicsDevice);
            btnOptions.setPosition(new Vector2(graphics.PreferredBackBufferWidth/2 -225, 400));
            btnShop = new mButton(Content.Load<Texture2D>("Shop_Button"), graphics.GraphicsDevice);
            btnShop.setPosition(new Vector2(graphics.PreferredBackBufferWidth - 250, 0));


            cursor = Content.Load<Texture2D>("cursor");

            b = new MultiBackground(GraphicsDevice);
            b.AddLayer(back, 1, 5f);
            b.SetMoveLeftRight();
            b.StartMoving();

            
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


            switch (currentGameState)
            {
                
                
                case GameState.Shop:

                    b.Update(gameTime);
                    particleEngine.Update();


                    break;

                case GameState.mainMenu:
                    btnPlay.Update(mouse);
                    if (btnPlay.isClicked == true) currentGameState = GameState.playing;

                    btnOptions.UpdateLong(mouse);
                    if (btnOptions.isClicked == true) currentGameState = GameState.options;

                 

                    particleEngine.EmitterLocation = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2);
                    particleEngine.Update();
                    
                    
                    b.Update(gameTime);

                    break;

                case GameState.playing:
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    this.Exit();
           
                    UpdateInput();
                    player.Update(gameTime, GraphicsDevice);
                    btnShop.Update(mouse);
                    if (btnShop.isClicked == true) currentGameState = GameState.Shop;


                     for (int i = 0; i < laserList.Count; i++)
                      {

                        laserList[i].Update(gameTime);


                        if (laserList[i].Position.Y < 0f || laserList[i].Position.X < 0f || laserList[i].Position.X > graphics.GraphicsDevice.Viewport.Width
                        || laserList[i].Position.Y > graphics.GraphicsDevice.Viewport.Height)
                         {
                            laserList.RemoveAt(i);

                }


            }

                    break;

                case GameState.options:
                    
                    
                    break;

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
                
                case GameState.Shop:

                    spriteBatch.Begin();

                    b.Draw();
                    particleEngine.Draw(spriteBatch);
                    gui.Draw(gameTime, spriteBatch);
                    spriteBatch.Draw(cursor, cursorPos, Color.White);


                    spriteBatch.End();

                    break;
                
                
                
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

                case GameState.playing:

                    spriteBatch.Begin();
                    spriteBatch.Draw(Content.Load<Texture2D>("Background"), new Rectangle(0, 0, 800, 600), Color.White);
                    player.Draw(gameTime, spriteBatch);
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
                    }

                    }
                    btnShop.Draw(spriteBatch);
                    spriteBatch.End();
                    break;



                case GameState.options:

                    

                    break;
            }

          

            

           
            base.Draw(gameTime);
        }


        private void UpdateInput()
        {
            bool keyPressed = false;
            bool keyPressed2 = false;
            KeyboardState keyState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

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
           
           
            MouseState currMouseState = Mouse.GetState();

            if (currMouseState.X != prevMouseState.X ||
                currMouseState.Y != prevMouseState.Y)
            {
                //player.Rotation
                Vector2 mouseLoc = new Vector2(currMouseState.X, currMouseState.Y);

                
                Vector2 direction = (player.Position) - mouseLoc;
 
                angle = (float)(Math.Atan2(-direction.Y, -direction.X));
                                
                player.Rotation = angle + (float)45.5;
                
               


            }

             if (keyState.IsKeyDown(Keys.Q))
             {
                 LaserState = laserState.One;
             }
             if (keyState.IsKeyDown(Keys.W))
             {
                 LaserState = laserState.Two;
             }
             if (keyState.IsKeyDown(Keys.E))
             {
                 LaserState = laserState.Three;
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
                    }
                    fireDelay = FIRE_DELAY;
                }

        }
            prevMouseState = currMouseState;
        }

      



    }//End Class
}
