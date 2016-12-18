using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using LearnAndPlayWithKinect.Other;

namespace LearnAndPlayWithKinect.Games.Memory
{
    class CardPositions
    {
        public List<Vector2> vectors = new List<Vector2>
        {
            // Pierwszy rzad
            new Vector2(35, 35),
            new Vector2(130 + 35 + 20, 35),
            new Vector2(130 + 35 + 20 + 130 + 20, 35),
            new Vector2(130 + 35 + 20 + 130 + 20 + 130 + 20, 35),
            new Vector2(130 + 35 + 20 + 130 + 20 + 130 + 20 + 130 + 20, 35),

            // Drugi rzad
            new Vector2(35, 35 + 130 + 20),
            new Vector2(130 + 35 + 20, 35 + 130 + 20),
            new Vector2(130 + 35 + 20 + 130 + 20, 35 + 130 + 20),
            new Vector2(130 + 35 + 20 + 130 + 20 + 130 + 20, 35 + 130 + 20),
            new Vector2(130 + 35 + 20 + 130 + 20 + 130 + 20 + 130 + 20, 35 + 130 + 20),

            //Trzeci rzad
            new Vector2(35, 35 + 130 + 20 + 130 + 20),
            new Vector2(130 + 35 + 20, 35 + 130 + 20 + 130 + 20),
            new Vector2(130 + 35 + 20 + 130 + 20, 35 + 130 + 20 + 130 + 20),
            new Vector2(130 + 35 + 20 + 130 + 20 + 130 + 20, 35 + 130 + 20 + 130 + 20),
            new Vector2(130 + 35 + 20 + 130 + 20 + 130 + 20 + 130 + 20, 35 + 130 + 20 + 130 + 20),

            //Czwarty rzad
            new Vector2(35, 35 + 130 + 20 + 130 + 20 + 130 + 20), 
            new Vector2(130 + 35 + 20, 35 + 130 + 20 + 130 + 20 + 130 + 20),
            new Vector2(130 + 35 + 20 + 130 + 20, 35 + 130 + 20 + 130 + 20 + 130 + 20),
            new Vector2(130 + 35 + 20 + 130 + 20 + 130 + 20, 35 + 130 + 20 + 130 + 20 + 130 + 20),
            new Vector2(130 + 35 + 20 + 130 + 20 + 130 + 20 + 130 + 20, 35 + 130 + 20 + 130 + 20+ 130 + 20),
        };

        public CardPositions()
        {

        }

        public Vector2 getRandomPosition()
        {
            //Jezeli lista wektorow jest pusta to zwracam wektor (0,0)
            if (vectors.Count == 0) return Vector2.Zero;

            // Szukam losowego wektora, zapisuje do zmiennej i usuwam go z listy po to aby sie nie powtorzyl
            MersenneTwister mt = new MersenneTwister();
            int index = mt.Next(0, vectors.Count);
            Vector2 returnVector = vectors.ElementAt(index);

            vectors.RemoveAt(index);

            return returnVector;
        }
    }
}
