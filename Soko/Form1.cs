using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        Graphics g;    //  графический объект — некий холст
        Bitmap buf;  //  буфер для Bitmap-изображения
        List<Rectangle> rectangleList;
        Pen brush;
        Map currentMap;

        int cellSize;

        public Form1()
        {
            InitializeComponent();

            rectangleList = new List<Rectangle>();

            brush = new Pen(Color.Black, 3);
            currentMap = new Map(6, 5);
            //currentMap = new Map(LoadMap());

            this.KeyPreview = true;
            this.KeyDown += KeyDown_Event;

            ResizePictureBox();
            FillRectangles();
            UpdateFrame();
            MessageBox.Show("Кнопки управления:" + '\n' +
                "R - загрузить/перезагрузить карту" + '\n'+
                "WASD - управление красным игроком" + '\n'+
                "Стрелки клавиатуры - управление синим игроком" + '\n'+
                "Правила:" + '\n'+
                "Доставьте коробки разных цветов в клетки с соответствующим цветом");


            //this.Refresh();

        }
        private void KeyDown_Event(object sender, KeyEventArgs e)
        //при нажатии клавиши меняет положение игрока на одну клетку
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    currentMap.MoveTo(currentMap.player1, Creature.Direction.Up);
                    break;
                case Keys.Up:
                    currentMap.MoveTo(currentMap.player2, Creature.Direction.Up);
                    break;
                case Keys.A:
                    currentMap.MoveTo(currentMap.player1, Creature.Direction.Left);
                    break;
                case Keys.Left:
                    currentMap.MoveTo(currentMap.player2, Creature.Direction.Left);
                    break;
                case Keys.S:
                    currentMap.MoveTo(currentMap.player1, Creature.Direction.Down);
                    break;
                case Keys.Down:
                    currentMap.MoveTo(currentMap.player2, Creature.Direction.Down);
                    break;
                case Keys.D:
                    currentMap.MoveTo(currentMap.player1, Creature.Direction.Right);
                    break;
                case Keys.Right:
                    currentMap.MoveTo(currentMap.player2, Creature.Direction.Right);
                    break;
                case Keys.R:
                    currentMap = new Map(LoadMap());
                    ResizePictureBox();
                    FillRectangles();
                    UpdateFrame();
                    break;
                default:
                    //MessageBox.Show();
                    break;
            }
            UpdateFrame();
            if (currentMap.isWinnable())
            {
                MessageBox.Show("Обе коробки доставлены! Поздравляем! Вы прошли карту!");
            }

        } 

        private void FillRectangles()
        //заполняет список клеток с координатами соответствующими их положению в сетке
        {
            rectangleList.Clear();
            foreach(Cell cell in currentMap.grid)
            {
                rectangleList.Add(new Rectangle(cell.xPos * cellSize, cell.yPos * cellSize, cellSize, cellSize));
            }
        } 

        private void ResizePictureBox()
        //вычисляет размер клеток и меняет размер PictureBox в зависимости от размера окна
        {
            double cellHeight = (this.Size.Height - bg_pictureBox.Location.Y*2) / (currentMap.Height+1);
            double cellWidth = (this.Size.Width- bg_pictureBox.Location.X*2) / (currentMap.Width+1);
            cellSize = Math.Min((int)cellHeight, (int)cellWidth);
            bg_pictureBox.Width = cellSize * currentMap.Width +cellSize*2;
            bg_pictureBox.Height = cellSize * currentMap.Height+cellSize*2;
        }

        private void Form1_Resize(object sender, EventArgs e) 
        //при изменении размера окна меняет размер picturebox и размер клеток
        {
            ResizePictureBox();
            FillRectangles();
            UpdateFrame();
            this.Refresh();
        }

        private void UpdateFrame() 
        //обновляет содержимое picturebox
        {
            buf = new Bitmap(bg_pictureBox.Width, bg_pictureBox.Height); // создает битмап с размером равным picBox
            g = Graphics.FromImage(buf);   // инициализация g
            g.DrawRectangles(brush, rectangleList.ToArray());
            foreach(Cell cell in currentMap.grid)
            {
                if (cell.Type==Cell.cellType.Close)
                {
                    g.FillRectangle(new SolidBrush(Color.Black), FindRectByPos(cell.xPos, cell.yPos, rectangleList));
                }
            }
            g.FillRectangle(new SolidBrush(Color.Red), FindRectByPos(currentMap.GetFinishCell(Cell.cellType.RedFinish).xPos, currentMap.GetFinishCell(Cell.cellType.RedFinish).yPos, rectangleList));
            g.FillRectangle(new SolidBrush(Color.Blue), FindRectByPos(currentMap.GetFinishCell(Cell.cellType.BlueFinish).xPos, currentMap.GetFinishCell(Cell.cellType.BlueFinish).yPos, rectangleList));
            g.DrawEllipse(new Pen(Color.Red, 3), FindRectByPos(currentMap.redCart.xPos, currentMap.redCart.yPos, rectangleList));
            g.DrawEllipse(new Pen(Color.Blue, 3), FindRectByPos(currentMap.blueCart.xPos, currentMap.blueCart.yPos, rectangleList));
            g.DrawEllipse(brush, FindRectByPos(currentMap.player1.xPos, currentMap.player1.yPos, rectangleList));
            g.DrawEllipse(brush, FindRectByPos(currentMap.player2.xPos, currentMap.player2.yPos, rectangleList));
            bg_pictureBox.Image = buf;
        }
        private Rectangle FindRectByPos(int xPos, int yPos, List<Rectangle> rectangles)
        {
            return rectangleList.Find(x => x.X == xPos * cellSize && x.Y == yPos * cellSize);
        }

        private int[,] LoadMap()
        {
            //c - close cell
            //o - open cell
            //p1 - player1 spawn
            //p2 - player2 spawn
            //c1 - cart1 spawn
            //c2 - cart2 spawn
            int[,] newMap = {
                {21,2},
                {22,2},
                {23,2},
                {20,3},
                {21,3},
                {22,3},
                {23,3},
                {20,4},
                {21,4},
                {22,4},
                {23,4},
                {21,5},
                {22,5},
                {23,5},
                {21,6},
                {12,7},
                {13,7},
                {17,7},
                {18,7},
                {21,7},
                {12,8},
                {13,8},
                {17,8},
                {18,8},
                {19,8},
                {20,8},
                {21,8},
                {22,8},
                {7,9},
                {8,9},
                {9,9},
                {10,9},
                {11,9},
                {12,9},
                {13,9},
                {17,9},
                {18,9},
                {19,9},
                {20,9},
                {21,9},
                {6,10},
                {7,10},
                {8,10},
                {9,10},
                {13,10},
                {16,10},
                {17,10},
                {18,10},
                {19,10},
                {20,10},
                {21,10},
                {6,11},
                {7,11},
                {8,11},
                {9,11},
                {13,11},
                {14,11},
                {17,11},
                {21,11},
                {6,12},
                {7,12},
                {8,12},
                {13,12},
                {14,12},
                {17,12},
                {8,13},
                {12,13},
                {13,13},
                {17,13},
                {19,13},
                {8,14},
                {13,14},
                {17,14},
                {18,14},
                {19,14},
                {20,14},
                {21,14},
                {8,15},
                {9,15},
                {10,15},
                {13,15},
                {16,15},
                {17,15},
                {18,15},
                {19,15},
                {20,15},
                {21,15},
                {2,16},
                {3,16},
                {4,16},
                {5,16},
                {6,16},
                {7,16},
                {8,16},
                {9,16},
                {10,16},
                {11,16},
                {13,16},
                {14,16},
                {17,16},
                {18,16},
                {19,16},
                {20,16},
                {3,17},
                {8,17},
                {10,17},
                {11,17},
                {13,17},
                {14,17},
                {20,17},
                {3,18},
                {8,18},
                {10,18},
                {13,18},
                {14,18},
                {20,18},
                {3,19},
                {4,19},
                {5,19},
                {6,19},
                {10,19},
                {13,19},
                {20,19},
                {3,20},
                {4,20},
                {5,20},
                {6,20},
                {10,20},
                {12,20},
                {13,20},
                {19,20},
                {20,20},
                {3,21},
                {4,21},
                {5,21},
                {8,21},
                {9,21},
                {10,21},
                {12,21},
                {13,21},
                {14,21},
                {15,21},
                {19,21},
                {20,21},
                {21,21},
                {5,22},
                {8,22},
                {9,22},
                {10,22},
                {12,22},
                {13,22},
                {14,22},
                {15,22},
                {16,22},
                {17,22},
                {18,22},
                {19,22},
                {20,22},
                {5,23},
                {8,23},
                {9,23},
                {10,23},
                {12,23},
                {13,23},
                {14,23},
                {15,23},
                {16,23},
                {19,23},
                {20,23},
                {4,24},
                {5,24},
                {6,24},
                {8,24},
                {10,24},
                {13,24},
                {19,24},
                {20,24},
                {4,25},
                {5,25},
                {6,25},
                {4,26},
                {5,26},
                {6,26}
            };

            return newMap;
        }
    }
}
