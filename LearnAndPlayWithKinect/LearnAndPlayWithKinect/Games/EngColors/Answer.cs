using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using LearnAndPlayWithKinect.Other;

namespace LearnAndPlayWithKinect.Games.EngColors
{
    enum AnswerType
    {
        Good,
        Bad,
    }

    class Answer
    {
        public int responseTime;

        Texture2D txAnswerGood;
        Texture2D txAnswerBad;
        float opacity;
        AnswerType answer;

        public Answer(Texture2D txAnswerGood, Texture2D txAnswerBad)
        {
            this.txAnswerBad = txAnswerBad;
            this.txAnswerGood = txAnswerGood;
            responseTime = 0;
            answer = AnswerType.Bad;
            opacity = 1.0f;
        }

        public void setAnswer(AnswerType type, int responseTime)
        {
            this.answer = type;
            this.responseTime = responseTime;
            this.opacity = 1.0f;
        }

        public Texture2D getAnswer()
        {
            switch (answer)
            {
                case AnswerType.Bad:
                    {
                        return this.txAnswerBad;
                    }
                case AnswerType.Good:
                    {
                        return this.txAnswerGood;
                    }
                default:
                    {
                        return this.txAnswerBad;
                    }
            }
        }

        public AnswerType getAnswerType()
        {
            return answer;
        }

        public void decreaseTime()
        {
            if (this.responseTime - 1 >= 0) this.responseTime--;
            else this.responseTime = 0;

            if (opacity > 0)
            {
                opacity -= 0.005f;
            }
            else
            {
                opacity = 0.0f;
            }
        }

        public float getOpacity()
        {
            return opacity;
        }
    }
}
