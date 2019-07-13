using System;
using System.Windows;
using System.Windows.Forms;
using FlowDirection = System.Windows.FlowDirection;

namespace Flies
{
    class Fly
    {
        private static int flyScaredTime = 2 * 30;
        static private double defaultXDelta = 7;
        static private double defaultYDelta = 7;

        private Random random;
        private double xDelta = defaultXDelta;
        private int counter = 0;
        private double yDelta = defaultYDelta;
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

            speedUpFly(70);
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
        private void speedUpFly(int d)
        {
            if (isCursorTouching() && counter == 0)
            {
                yDelta = d * Math.Sign(yDelta);
                xDelta = d * Math.Sign(xDelta);

                counter = 1;
            }
            else if (counter == flyScaredTime)
            {
                counter = 0;
                yDelta = defaultYDelta * Math.Sign(yDelta);
                xDelta = defaultXDelta * Math.Sign(xDelta);
            }
            else if (counter > 0)
            {
                counter++;
            }
        }

        private bool isCursorTouching()
        {
            bool isNearLeft = Cursor.Position.X < currentX && currentX - Cursor.Position.X < 50;
            bool isNearRight = Cursor.Position.X > currentX && Cursor.Position.X - currentX < 50;
            bool isNearBottom = Cursor.Position.Y < currentY && currentY - Cursor.Position.Y < 50;
            bool isNearTop = Cursor.Position.Y > currentY && Cursor.Position.Y - currentY < 50;

            bool nearX = isNearLeft || isNearRight;
            bool nearY = isNearTop || isNearBottom;

            return nearX && nearY;
            
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
