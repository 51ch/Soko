using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soko
{
    class Creature
    {
        private int xPosition;
        private int yPosition;

        public enum Direction
        {
            Right,
            Left,
            Up,
            Down
        }

        public int xPos
        {
            get
            {
                return xPosition;
            }
            set
            {
                xPosition = value;
            }
        }
        public int yPos
        {
            get
            {
                return yPosition;
            }
            set
            {
                yPosition = value;
            }
        }
        public Creature(int x, int y)
        {
            xPos = x;
            yPos = y;
        }
    }
}
