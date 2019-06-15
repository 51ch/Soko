using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Cell
    {
        public enum cellType
        {
            Open,
            Close,
            DeadEnd,
            RedFinish,
            BlueFinish
        }
        private int xPosition;
        private int yPosition;
        private cellType type;
        private bool busy;
        private Creature nestedObject;
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
        public cellType Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }
        public bool isBusy
        {
            get
            {
                return busy;
            }
            set
            {
                busy = value;
            }
        }
        public void setObject(Creature obj)
        {
            nestedObject = obj;
            isBusy = true;
        }
        public Creature getObject()
        {
            if (isBusy)
            {
                return nestedObject;
            }
            else
                return null;
        }
        public void clearCell()
        {
            isBusy = false;
            nestedObject = null;
        }
        public Cell(int x, int y)
        {
            xPos = x;
            yPos = y;
            this.Type = cellType.Open;
            this.isBusy = false;
        }
        public Cell(int x, int y, cellType t)
        {
            xPos = x;
            yPos = y;
            this.Type = t;
            this.isBusy = false;
        }
        public Cell(int x, int y, cellType t, bool b)
        {
            xPos = x;
            yPos = y;
            this.Type = t;
            this.isBusy = b;
        }
    }
}
