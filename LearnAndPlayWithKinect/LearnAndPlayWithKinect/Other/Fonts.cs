using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace LearnAndPlayWithKinect.Other
{
    class Fonts
    {
        public SpriteFont AngryBirds32;
        public SpriteFont Kootenay16;
        public SpriteFont Kootenay22;
        public SpriteFont Miramonte;
        public SpriteFont Moire32;
        public SpriteFont Motorwerk;
        public SpriteFont Pericles46;
        public SpriteFont SegoeScript32;
        public SpriteFont Roboto60;
        public SpriteFont Calibri40;

        public Fonts()
        {

        }

        public void loadContent(ContentManager content)
        {
            Console.WriteLine("**************** LOAD CONTENT: Fonts ****************");
            AngryBirds32 = content.Load<SpriteFont>("fonts\\AngryBirds32");
            Kootenay16 = content.Load<SpriteFont>("fonts\\Kootenay16");
            Kootenay22 = content.Load<SpriteFont>("fonts\\Kootenay22");
            Miramonte = content.Load<SpriteFont>("fonts\\Miramonte");
            Moire32 = content.Load<SpriteFont>("fonts\\Moire32");
            Motorwerk = content.Load<SpriteFont>("fonts\\Motorwerk");
            Pericles46 = content.Load<SpriteFont>("fonts\\Pericles46");
            SegoeScript32 = content.Load<SpriteFont>("fonts\\SegoeScript32");
            Roboto60 = content.Load<SpriteFont>("fonts\\Roboto60");
            Calibri40 = content.Load<SpriteFont>("fonts\\Calibri40");
        }
    }
}
