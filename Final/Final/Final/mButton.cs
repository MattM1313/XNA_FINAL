using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Final
{
    class mButton
    {
        Texture2D texture;
        Vector2 position;
        Rectangle rectangle;


        Color color = new Color(255, 255, 255);

        public Vector2 size, size2;

        public mButton(Texture2D newTexture, GraphicsDevice graphics)
        {

            texture = newTexture;
            //size2 = new Vector2(graphics.Viewport.Width / 1, graphics.Viewport.Height / 7);
            size2 = new Vector2(450, 80);
            size = new Vector2(250, 80);
        }
        bool down;
        public bool isClicked;
        public void Update(MouseState mouse)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);


            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);

            if (mouseRectangle.Intersects(rectangle))
            {
                System.Diagnostics.Debug.WriteLine(color.A);
                if (color.R == 255) down = false;
                if (color.R == 0) down = true;
                if (down) color.R += 5; else color.R -= 5;
                
                if (mouse.LeftButton == ButtonState.Pressed) isClicked = true;

                 
            

            }
            else if (color.R < 0)
            {
                color.R += 5;
                isClicked = false;
            }
            if(color.R < 255 && rectangle.Contains(mouseRectangle) == false)
            {
                color.R += 5;
            }
            
           
            
            
        
        }

        public void UpdateLong(MouseState mouse)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, (int)size2.X, (int)size2.Y);


            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);

            if (mouseRectangle.Intersects(rectangle))
            {
                System.Diagnostics.Debug.WriteLine(color.A);
                if (color.R == 255) down = false;
                if (color.R == 0) down = true;
                if (down) color.R += 5; else color.R -= 5;

                if (mouse.LeftButton == ButtonState.Pressed) isClicked = true;




            }
            else if (color.R < 0)
            {
                color.R += 5;
                isClicked = false;
            }
            if (color.R < 255 && rectangle.Contains(mouseRectangle) == false)
            {
                color.R += 5;
            }





        }

      
        public void setPosition(Vector2 newPosition)
        {
            position = newPosition;
        }
        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(texture, rectangle, color);
        }




        

    }
}
