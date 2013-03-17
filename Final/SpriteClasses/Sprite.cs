﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpriteClasses
{
    public class Sprite
    {
        // This variable will hold our position - make it a property so game class
        //can use it to change position when mouse moved
        protected Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        public Texture2D TextureImage { get; set; }
        //origin of sprite, either null or the center of the image
        //make these protected so derived classes can change them
        protected Vector2 spriteOrigin;
        public Vector2 SpriteOrigin
        {
            get { return spriteOrigin; }
            set { spriteOrigin = value; }
        }
        public bool SetOrigin { get; set; }

        //vector so it has independant x and y values
        protected Vector2 velocity;
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        //velocity set in constructor, needs a 
        //separate property so it doesn't get zeroed
        //when sprite idles
        protected Vector2 originalVelocity;
        public Vector2 OriginalVelocity
        {
            get { return originalVelocity; }
            set { originalVelocity = value; }
        }
        //current rotation value
        public float Rotation { get; set; }
        //speed of rotation of the sprite
        public float RotationSpeed { get; set; }
        public float Scale { get; set; }
        public SpriteEffects SpriteEffect { get; set; }

        //is he active or not (should he be updated and drawn?)
        public bool Active { get; set; }

        //rectangle occupied by texture - bounding rectangle
        public virtual Rectangle CollisionRectangle
        {
            get
            {
                return new Rectangle((int)(position.X - SpriteOrigin.X * Scale), (int)(position.Y - SpriteOrigin.Y * Scale),
                   (int)(TextureImage.Width * Scale), (int)(TextureImage.Height * Scale));
            }
        }

        // base version
        public Sprite(Texture2D textureImage, Vector2 position, Vector2 velocity, bool setOrigin,
            float rotation, float scale, SpriteEffects spriteEffect)
        {
            Position = position;
            TextureImage = textureImage;
            OriginalVelocity = velocity;
            Velocity = velocity;
            SetOrigin = setOrigin;
            if (SetOrigin)
            {
                SpriteOrigin = new Vector2(TextureImage.Width/2, TextureImage.Height/2);
            }
            Rotation = rotation;
            Scale = scale;            
            SpriteEffect = spriteEffect;

            Active = true;
        }
        //version that does not keep sprite on screen
        public virtual void Update(GameTime gameTime)
        {
            if (Active)
            {
                // time between frames
                float timeLapse = (float)(gameTime.ElapsedGameTime.TotalSeconds);
                //move the sprite
                position += Velocity * timeLapse;
                
                // Scale radians by time between frames so rotation is uniform
                // rate on all systems. Cap between 0 & 2PI for full rotation.
                //Rotation += RotationSpeed * timeLapse;
                Rotation = Rotation % (MathHelper.Pi * 2.0f);
            }
        }
        //version that keeps sprite on screen
        public virtual void Update(GameTime gameTime, GraphicsDevice Device)
        {
            if (Active)
            {
                //call overload to do rotation and basic movement
                Update(gameTime);
                //keep on screen
                if (Position.X > Device.Viewport.Width - SpriteOrigin.X * Scale)
                {                    
                    position.X = Device.Viewport.Width - SpriteOrigin.X * Scale;
                    velocity.X = -Velocity.X;
                }
                else if (Position.X < SpriteOrigin.X * Scale)
                {
                    position.X = SpriteOrigin.X * Scale;
                    velocity.X = -Velocity.X;
                }

                if (Position.Y > Device.Viewport.Height - SpriteOrigin.Y * Scale)
                {
                    position.Y = Device.Viewport.Height - SpriteOrigin.Y * Scale;
                    velocity.Y = -Velocity.Y;
                }
                else if (Position.Y < SpriteOrigin.Y * Scale)
                {
                    position.Y = SpriteOrigin.Y * Scale;
                    velocity.Y = -Velocity.Y;
                }
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Active)
            {
                spriteBatch.Draw(TextureImage,
                     position,
                     null,
                     Color.White,
                     Rotation,
                     SpriteOrigin,
                     Scale,
                     SpriteEffect,
                     0);
                }
        }

        public virtual void Draw( SpriteBatch spriteBatch)
        {
            if (Active)
            {
                spriteBatch.Draw(TextureImage,
                     position,
                     null,
                     Color.White,
                     Rotation,
                     SpriteOrigin,
                     Scale,
                     SpriteEffect,
                     0);
            }
        }

        // These match up with the Arrow keys
        public virtual void Up()
        {
            velocity.Y -= OriginalVelocity.Y;
        }

        public virtual void Down()
        {
            velocity.Y += OriginalVelocity.Y;
        }

        public virtual void Right()
        {
            velocity.X += OriginalVelocity.X;
        }

        public virtual void Left()
        {
            velocity.X -= OriginalVelocity.X;
        }

        public virtual void Idle()
        {
            Velocity *= .95f;
        }

        //is there a collision with another sprite?
        public bool CollisionSprite(Sprite sprite)
        {
            return CollisionRectangle.Intersects(sprite.CollisionRectangle);
        }
        //is there a collision with the mouse?
        public bool CollisionMouse(int x, int y)
        {
            return CollisionRectangle.Contains(x, y);
        }
    }
}
