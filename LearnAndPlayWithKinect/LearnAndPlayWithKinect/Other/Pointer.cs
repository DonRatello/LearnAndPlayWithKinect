using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using LearnAndPlayWithKinect.Other;
using LearnAndPlayWithKinect.Kinect;

namespace LearnAndPlayWithKinect.Other
{
    class Pointer
    {
        Texture2D txPointer;
        Vector2 posPointer;
        bool isEnabled;
        public Rectangle rectanglePointer;

        SpriteFont Miramonte;

        public Pointer()
        {
            isEnabled = false;
            posPointer = new Vector2();
            rectanglePointer = new Rectangle();
        }

        public void loadContent(ContentManager content)
        {
            Console.WriteLine("**************** LOAD CONTENT: Pointer ****************");
            txPointer = content.Load<Texture2D>("hand-pointer-icon");
            Miramonte = content.Load<SpriteFont>("fonts\\Moire32");
        }

        public void update(float X, float Y)
        {
            if (!isEnabled) return;

            posPointer = new Vector2(X, Y);
            rectanglePointer = new Rectangle((int)posPointer.X, (int)posPointer.Y, txPointer.Width, txPointer.Height);
        }

        public void updateByKinect(TrackedSkeleton skeleton)
        {
            if (!isEnabled) return;

            if (skeleton.getJoint(JointType.WristRight).Position.X == 0 && skeleton.getJoint(JointType.WristRight).Position.Y == 0) return;

            //Bazujac na pozycji prawej reki, dopasowuje wspolrzedne
            Microsoft.Kinect.Vector4 newPos = PointerAdjustment.AdjustToScreen(normalizeJoint(true, skeleton));
            posPointer = new Vector2(newPos.X, newPos.Y);

            rectanglePointer = new Rectangle((int)posPointer.X, (int)posPointer.Y, txPointer.Width, txPointer.Height);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            if (!isEnabled) return;

            spriteBatch.Draw(txPointer, posPointer, Color.White);

            if (Settings.PROJECT_STATE == Settings.Environment.DEVELOPMENT)
            {
                spriteBatch.DrawString(Miramonte, "X: " + posPointer.X.ToString(), new Vector2(posPointer.X+33, posPointer.Y), Color.Red, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.0f);
                spriteBatch.DrawString(Miramonte, "Y: " + posPointer.Y.ToString(), new Vector2(posPointer.X+33, posPointer.Y+20), Color.Red, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.0f);
            } 
        }

        public void Enable()
        {
            isEnabled = true;
        }

        public void Disable()
        {
            isEnabled = false;
        }

        /// <summary>
        /// Funkcja ta przestawia punkt zero z miednicy na poziom glowy
        /// </summary>
        /// <param name="rightHanded"></param>
        /// <param name="skeleton"></param>
        /// <returns></returns>
        private Joint normalizeJoint(bool rightHanded, TrackedSkeleton skeleton)
        {
            Joint head = skeleton.getJoint(JointType.ShoulderCenter);
            Joint wrist = skeleton.getJoint(JointType.WristRight);

            Joint newJoint = new Joint();

            SkeletonPoint newPoint = new SkeletonPoint()
            {
                 X = wrist.Position.X,
                 Y = wrist.Position.Y - head.Position.Y
            };

            newJoint.Position = newPoint;

            return newJoint;
        }
    }
}
