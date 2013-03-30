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
using SpriteClasses;

namespace Final
{
    class ShopIcon : Sprite
    {
        public bool active;
        public ShopIcon(Texture2D texture, Vector2 position, int type, bool active, int cost)
            : base(texture, position, new Vector2(0, 0), true, 0, 0.09f, SpriteEffects.None, null, 0)
        {



        }



    }
}
