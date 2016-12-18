using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using LearnAndPlayWithKinect.Other;

namespace LearnAndPlayWithKinect.Interfaces
{
    interface iScreen
    {
        void loadContent(ContentManager content);

        GameType update(Rectangle cursorRect);

        void draw(SpriteBatch spriteBatch);
    }
}
