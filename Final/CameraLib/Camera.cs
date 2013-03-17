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
using Microsoft.Xna.Framework.Media;

namespace CameraLib
{
   public class Camera
    {
        public Matrix transform;
        Vector2 center;

        public Camera()
        {

        }

        public void Update(Vector2 position)
        {
            center = new Vector2(position.X - 200, position.Y - 250);
            transform = Matrix.CreateScale(new Vector3(1, 1, 0)) * 
                Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0));


        }


    }
}
