using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soko
{
    class Map
    {
        private int height;
        private int width;
        public List<Cell> grid;
        public Creature playerRed;
        public Creature playerBlue;
        public Creature chestRed;
        public Creature chestBlue;

        public int Height
        {
            get
            {
                return height;
            }
        }
        public int Width
        {
            get
            {
                return width;
            }
        }
        public Map(int h, int w)
        {
            height = h;
            width = w;
            grid = new List<Cell>();
            playerRed = new Creature(0, 0);
            playerBlue = new Creature(1, 0);
            chestRed = new Creature(0, 1);
            chestBlue = new Creature(1, 1);
            for (int rownumber=0; rownumber < height; rownumber++)
            {
                for (int colnumber=0; colnumber < width; colnumber++)
                {
                    grid.Add(new Cell(colnumber, rownumber));
                }
            }
            GetCell(playerRed.xPos, playerRed.yPos).setObject(playerRed);
            GetCell(playerBlue.xPos, playerBlue.yPos).setObject(playerBlue);
            GetCell(chestRed.xPos, chestRed.yPos).setObject(chestRed);
            GetCell(chestBlue.xPos, chestBlue.yPos).setObject(chestBlue);
            GetCell(0, 2).Type = Cell.cellType.RedFinish;
            GetCell(1, 2).Type = Cell.cellType.BlueFinish;
        }
        public Map(int[,] mapArr)
        {
            height = 28;
            width = 25;
            grid = new List<Cell>();
            playerRed = new Creature(2, 16);
            playerBlue = new Creature(11, 16);
            chestRed = new Creature(9, 22);
            chestBlue = new Creature(5, 25);

            for (int rownumber = 0; rownumber < height; rownumber++)
            {
                for (int colnumber = 0; colnumber < width; colnumber++)
                {
                    grid.Add(new Cell(colnumber, rownumber, Cell.cellType.Close));
                }
            }
            int p = mapArr.Length;
            for (int i = 0; i< (mapArr.Length/2); i++)
            {
                int x = mapArr[i, 0];
                int y = mapArr[i, 1];
                GetCell(x, y).Type = Cell.cellType.Open;
            }

            GetCell(playerRed.xPos, playerRed.yPos).setObject(playerRed);
            GetCell(playerBlue.xPos, playerBlue.yPos).setObject(playerBlue);
            GetCell(chestRed.xPos, chestRed.yPos).setObject(chestRed);
            GetCell(chestBlue.xPos, chestBlue.yPos).setObject(chestBlue);
            GetCell(23, 3).Type = Cell.cellType.RedFinish;
            GetCell(23, 4).Type = Cell.cellType.BlueFinish;
        }
        public Cell GetCell(int x, int y)
        {
            return grid.Find(z => z.xPos == x && z.yPos == y);
        }
        public Cell GetFinishCell(Cell.cellType fincolor)
        {
            return grid.Find(z => z.Type == fincolor);
        }
        public Cell GetNearCell(Cell currentCell, Creature.Direction direction)
        {
            switch(direction)
            {
                case Creature.Direction.Right:
                    return grid.Find(z => z.xPos == currentCell.xPos+1 && z.yPos == currentCell.yPos);
                case Creature.Direction.Left:
                    return grid.Find(z => z.xPos == currentCell.xPos-1 && z.yPos == currentCell.yPos);
                case Creature.Direction.Up:
                    return grid.Find(z => z.xPos == currentCell.xPos && z.yPos == currentCell.yPos-1);
                case Creature.Direction.Down:
                    return grid.Find(z => z.xPos == currentCell.xPos && z.yPos == currentCell.yPos+1);
                default:
                    return grid.Find(z => z.xPos == currentCell.xPos && z.yPos == currentCell.yPos);
            }
        }
        public void MoveTo(Creature player, Creature.Direction direction)
        {
            Cell currentCell = GetCell(player.xPos, player.yPos);
            Cell nextCell = GetNearCell(currentCell, direction);

            if ((nextCell != null)&&(player.currentState!=Creature.State.Moving))
            {
                if ((nextCell.Type == Cell.cellType.Open) || 
                    (nextCell.Type == Cell.cellType.RedFinish)||
                    (nextCell.Type == Cell.cellType.BlueFinish))
                {
                    if (nextCell.isBusy)
                    {
                        MoveTo(nextCell.getObject(), direction);
                        if(!nextCell.isBusy)
                        {
                            player.MovingTo(direction);
                            nextCell.setObject(player);
                            player.xPos = nextCell.xPos;
                            player.yPos = nextCell.yPos;
                            currentCell.clearCell();
                        }
                    }
                    else
                    {
                        player.MovingTo(direction);
                        nextCell.setObject(player);
                        player.xPos = nextCell.xPos;
                        player.yPos = nextCell.yPos;
                        currentCell.clearCell();
                    }
                }
            }
        }
        public bool isWinnable()
        {
            Cell redCell = grid.Find(z => z.Type == Cell.cellType.RedFinish);
            Cell blueCell = grid.Find(z => z.Type == Cell.cellType.BlueFinish);
            if (redCell.getObject() == chestRed && blueCell.getObject() == chestBlue)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
