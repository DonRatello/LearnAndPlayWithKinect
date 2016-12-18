using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using LearnAndPlayWithKinect.Other;

namespace LearnAndPlayWithKinect.Games.Gymnastics.MovesController
{
    class MainController
    {
        Moves activeMove = Moves.RIGHT_LEG_LEFT;

        LeftHand leftHand;
        LeftLeg leftLeg;
        RightHand rightHand;
        RightLeg rightLeg;
        Hip hip;
        Head head;

        Skeleton skeleton;
        MersenneTwister twister;

        /// <summary>
        /// Aktualny czas
        /// </summary>
        int TIME = 0;

        /// <summary>
        /// Górna granica przedziału czasowego
        /// </summary>
        int TIME_MAX = 30;

        public MainController()
        {
            leftHand = new LeftHand(TIME_MAX);
            leftLeg = new LeftLeg(TIME_MAX);
            rightHand = new RightHand(TIME_MAX);
            rightLeg = new RightLeg(TIME_MAX);
            hip = new Hip(TIME_MAX);
            head = new Head(TIME_MAX);
            twister = new MersenneTwister();
        }

        /// <summary>
        /// Losowanie aktywnego ruchu i zwrócenie go
        /// </summary>
        /// <returns></returns>
        public Moves getRandomMove()
        {
            var moves = Enum.GetValues(typeof(Moves)).Cast<Moves>().ToList();
            activeMove = moves.ElementAt(twister.Next(0, moves.Count));

            return activeMove;
        }

        /// <summary>
        /// Zwrocenie aktywnego ruchu
        /// </summary>
        /// <returns></returns>
        public Moves getMove()
        {
            return activeMove;
        }

        /// <summary>
        /// Aktualizacja szkieletu
        /// </summary>
        /// <param name="skeleton"></param>
        public void updateSkeleton(Skeleton skeleton)
        {
            this.skeleton = skeleton;
        }

        /// <summary>
        /// Zwiększenie zmiennej czasowej
        /// </summary>
        public void incrementTime()
        {
            if ((TIME + 1) <= TIME_MAX)
                TIME++;
            else
                TIME = 0;
        }

        /// <summary>
        /// Getter czasu
        /// </summary>
        /// <returns></returns>
        public int getTime()
        {
            return TIME;
        }

        /// <summary>
        /// Metoda sprawdzająca czy ruch został wykonany. Należy pamiętać aby zawsze trzymać w klasie aktualny szkielet
        /// </summary>
        /// <returns></returns>
        public bool isMoveDone()
        {
            if (skeleton == null) return false;

            bool moveDone = false;

            switch (activeMove)
            {
                case Moves.HEAD_LEFT:
                    {
                        moveDone = head.moveLeft(TIME, skeleton.Joints[JointType.Head]);
                        break;
                    }
                case Moves.HEAD_RIGHT:
                    {
                        moveDone = head.moveRight(TIME, skeleton.Joints[JointType.Head]);
                        break;
                    }
                case Moves.HIP_DOWN:
                    {
                        moveDone = hip.moveDown(TIME, skeleton.Joints[JointType.HipCenter]);
                        break;
                    }
                case Moves.HIP_UP:
                    {
                        moveDone = hip.moveUp(TIME, skeleton.Joints[JointType.HipCenter]);
                        break;
                    }
                case Moves.LEFT_HAND_DOWN:
                    {
                        moveDone = leftHand.moveDown(TIME, skeleton.Joints[JointType.HandLeft]);
                        break;
                    }
                case Moves.LEFT_HAND_LEFT:
                    {
                        moveDone = leftHand.moveLeft(TIME, skeleton.Joints[JointType.HandLeft]);
                        break;
                    }
                case Moves.LEFT_HAND_RIGHT:
                    {
                        moveDone = leftHand.moveRight(TIME, skeleton.Joints[JointType.HandLeft]);
                        break;
                    }
                case Moves.LEFT_HAND_UP:
                    {
                        moveDone = leftHand.moveUp(TIME, skeleton.Joints[JointType.HandLeft]);
                        break;
                    }
                case Moves.LEFT_LEG_LEFT:
                    {
                        moveDone = leftLeg.moveLeft(TIME, skeleton.Joints[JointType.FootLeft]);
                        break;
                    }
                case Moves.LEFT_LEG_RIGHT:
                    {
                        moveDone = leftLeg.moveRight(TIME, skeleton.Joints[JointType.FootLeft]);
                        break;
                    }
                case Moves.LEFT_LEG_UP:
                    {
                        moveDone = leftLeg.moveUp(TIME, skeleton.Joints[JointType.FootLeft]);
                        break;
                    }
                case Moves.RIGHT_HAND_DOWN:
                    {
                        moveDone = rightHand.moveDown(TIME, skeleton.Joints[JointType.HandRight]);
                        break;
                    }
                case Moves.RIGHT_HAND_LEFT:
                    {
                        moveDone = rightHand.moveLeft(TIME, skeleton.Joints[JointType.HandRight]);
                        break;
                    }
                case Moves.RIGHT_HAND_RIGHT:
                    {
                        moveDone = rightHand.moveRight(TIME, skeleton.Joints[JointType.HandRight]);
                        break;
                    }
                case Moves.RIGHT_HAND_UP:
                    {
                        moveDone = rightHand.moveUp(TIME, skeleton.Joints[JointType.HandRight]);
                        break;
                    }
                case Moves.RIGHT_LEG_LEFT:
                    {
                        moveDone = rightLeg.moveLeft(TIME, skeleton.Joints[JointType.FootRight]);
                        break;
                    }
                case Moves.RIGHT_LEG_RIGHT:
                    {
                        moveDone = rightLeg.moveRight(TIME, skeleton.Joints[JointType.FootRight]);
                        break;
                    }
                case Moves.RIGHT_LEG_UP:
                    {
                        moveDone = rightLeg.moveRight(TIME, skeleton.Joints[JointType.FootRight]);
                        break;
                    }
            }

            if (moveDone)
            {
                cleanJoints();
            }

            return moveDone;
        }

        void cleanJoints()
        {
            rightHand.cleanJoints();
            rightLeg.cleanJoints();
            leftLeg.cleanJoints();
            leftHand.cleanJoints();
            hip.cleanJoints();
            head.cleanJoints();
        }
    }
}
