﻿using System;
using System.Windows;
using System.Windows.Forms;
using FlowDirection = System.Windows.FlowDirection;

namespace Flies
{
    class Fly
    {
        private Random random;

        private double xDelta = 7;
        private int counter = 0;
        private double yDelta = 7;
        private double currentX = 0;
        private double currentY = 0;
        private double currentRotationAngle = 0;
        private FlowDirection flowDirection;

        private double canvasWidth;
        private double canvasHeight;
        private double imageWidth;
        private double imageHeight;

        public Fly(Random random, FlowDirection flowDirection, double canvasWidth, double canvasHeight, double imageWidth, double imageHeight)
        {
            this.random = random;
            this.flowDirection = flowDirection;
            this.canvasWidth = canvasWidth;
            this.canvasHeight = canvasHeight;
            this.imageWidth = imageWidth;
            this.imageHeight = imageHeight;
        }

        public void move()
        {
            if (collidesWithX())
            {
                xDelta *= -1;
                currentRotationAngle *= -1;

                if (xDelta >= 0)
                {
                    flowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    flowDirection = FlowDirection.RightToLeft;
                }
            }

            if (collidesWithY())
            {
                yDelta *= -1;
                currentRotationAngle *= -1;
            }

            adaptAngle();

            currentX += xDelta * getXModifier();
            currentY += yDelta * getYModifier();

            if (isCursorTouching() && counter == 0)
            {
                if(xDelta < 0)
                {
                    xDelta -= 70;
                }
                if (xDelta > 0)
                {
                    xDelta += 70;
                }
                if (yDelta < 0)
                {
                    yDelta -= 70;
                }
                if (yDelta > 0)
                {
                    yDelta += 70;
                }
                counter++;
            }
            if (counter > 0)
            {
                counter++;
                if (counter == 61)
                {
                    counter = 0;
                    if(yDelta > 0)
                    {
                        yDelta = 7;
                    }
                    if (yDelta < 0)
                    {
                        yDelta = -7;
                    }
                    if (xDelta > 0)
                    {
                        xDelta = 7;
                    }
                    if (xDelta < 0)
                    {
                        xDelta = -7;
                    }
                }
            }
            Console.WriteLine(isCursorTouching());
        }

        private void adaptAngle()
        {
            if (random.NextDouble() < 0.1)
            {
                bool invertedAngle = xDelta > 0 && yDelta < 0 || xDelta < 0 && yDelta > 0;
                int angleDelta = random.Next(15);
                bool addDelta = random.Next(2) == 1 ? true : false;

                if (addDelta)
                {
                    int upperLimit = invertedAngle ? 0 : 90;

                    currentRotationAngle += angleDelta;
                    if (currentRotationAngle > upperLimit)
                    {
                        currentRotationAngle = upperLimit;
                    }
                }
                else
                {
                    int lowerLimit = invertedAngle ? -90 : 0;

                    currentRotationAngle -= angleDelta;
                    if (currentRotationAngle < lowerLimit)
                    {
                        currentRotationAngle = lowerLimit;
                    }
                }
            }
        }

        private double getXModifier()
        {
            double absAngle = Math.Abs(currentRotationAngle);
            return 1 - (absAngle / 90);
        }

        private double getYModifier()
        {
            double absAngle = Math.Abs(currentRotationAngle);
            return absAngle / 90;
        }

        private bool collidesWithX()
        {
            return currentX + xDelta < 0 || currentX + xDelta + imageWidth > canvasWidth;
        }

        private bool isCursorTouching()
        {
            bool X = false;
            bool Y = false;
            //x
            if (Cursor.Position.X < currentX)
            {
                if(currentX - Cursor.Position.X < 50)
                {
                    X = true;
                }
            }
            if (Cursor.Position.X > currentX)
            {
                if (Cursor.Position.X - currentX < 50)
                {
                    X = true;
                }
            }
            //y
            if (Cursor.Position.Y < currentY)
            {
                if (currentY - Cursor.Position.Y < 50)
                {
                    Y = true;
                }
            }
            if (Cursor.Position.Y > currentY)
            {
                if (Cursor.Position.Y - currentY < 50)
                {
                    Y = true;
                }
            }
            //final check
            if(X && Y)
            {
                return true;
            }
            return false;
        }
        private bool collidesWithY()
        {
            if (yDelta < 0)
            {
                return currentY + yDelta - getVerticalOverhead() < 0;
            }
            else
            {
                return currentY + yDelta + imageHeight + getVerticalOverhead() > canvasHeight;
            }
        }

        private double getVerticalOverhead()
        {
            if (xDelta >= 0 && yDelta >= 0)
            {
                return getYModifier() * imageWidth + getYModifier() * imageHeight;
            }
            else if (xDelta >= 0 && yDelta < 0)
            {
                return getYModifier() * imageWidth;
            }
            else if (xDelta < 0 && yDelta >= 0)
            {
                return getYModifier() * imageHeight;
            }
            else
            {
                return 0;
            }
        }

        public double CurrentX
        {
            get
            {
                return currentX;
            }

            set
            {
                currentX = value;
            }
        }

        public double CurrentY
        {
            get
            {
                return currentY;
            }

            set
            {
                currentY = value;
            }
        }

        public double CurrentRotationAngle
        {
            get
            {
                return currentRotationAngle;
            }

            set
            {
                currentRotationAngle = value;
            }
        }

        public FlowDirection FlowDirection
        {
            get
            {
                return flowDirection;
            }

            set
            {
                flowDirection = value;
            }
        }
    }
}
