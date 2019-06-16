using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Soko
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Map currentMap;
        int cellSize;
        UniformGrid FieldGrid;
        ImageBrush ib_CloseCell;
        ImageBrush ib_OpenCell;
        ImageBrush ib_DeadEndCell;
        ImageBrush ib_RedFinish;
        ImageBrush ib_BlueFinish;
        ImageBrush ib_Player_Red;
        ImageBrush ib_Player_Blue;
        ImageBrush ib_Chest_Red;
        ImageBrush ib_Chest_Blue;

        Rectangle rect_Player_Red;
        Rectangle rect_Player_Blue;
        Rectangle rect_Chest_Red;
        Rectangle rect_Chest_Blue;

        int currentFrame_P1;
        int currentRow_P1;
        int currentFrame_P2;
        int currentRow_P2;

        int xPos_Player_Red;
        int yPos_Player_Red;
        int xPos_Player_Blue;
        int yPos_Player_Blue;
        int xPos_Chest_Red;
        int yPos_Chest_Red;
        int xPos_Chest_Blue;
        int yPos_Chest_Blue;

        int frameCount;
        int gameSpeed;
        int fps;
        double charH;
        double charW;
        public MainWindow()
        {
            InitializeComponent();
            //CreateNewGame(new Map(8, 8)); //создать новую игру с тестовой картой
            CreateNewGame(new Map(LoadMap())); //создать новую игру с загруженной картой
        }
        private void CreateNewGame(Map newMap)
        {
            FieldGrid = new UniformGrid(); //сетка тайлов игровой карты
            currentMap = newMap; //объект игровой логики
            cellSize = CalculateCellSize(); //размер клеток, тайлов, персонажей

            //Создание кистей
            ib_CloseCell = new ImageBrush();
            ib_OpenCell = new ImageBrush();
            ib_DeadEndCell = new ImageBrush();
            ib_RedFinish = new ImageBrush();
            ib_BlueFinish = new ImageBrush();
            ib_Player_Red = new ImageBrush();
            ib_Player_Blue = new ImageBrush();
            ib_Chest_Red = new ImageBrush();
            ib_Chest_Blue = new ImageBrush();

            //Создание ректанглов для отрисовки
            rect_Player_Red = new Rectangle();
            rect_Player_Blue = new Rectangle();
            rect_Chest_Red = new Rectangle();
            rect_Chest_Blue = new Rectangle();

            //Указание источников изображения для кистей
            ib_CloseCell.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/tiles/wall.jpg", UriKind.Absolute));
            ib_OpenCell.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/tiles/floor.jpg", UriKind.Absolute));
            ib_DeadEndCell.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/tiles/floor_stop.jpg", UriKind.Absolute));
            ib_RedFinish.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/tiles/floor_stop.jpg", UriKind.Absolute));
            ib_BlueFinish.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/tiles/floor_stop.jpg", UriKind.Absolute));
            ib_Player_Red.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/sprites/char_red.png", UriKind.Absolute));
            ib_Player_Blue.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/sprites/char_blue.png", UriKind.Absolute));
            ib_Chest_Red.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/sprites/chest_red.png", UriKind.Absolute));
            ib_Chest_Blue.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/sprites/chest_blue.png", UriKind.Absolute));

            //Размеры ректанглов равны размеру клеток
            rect_Chest_Red.Height = cellSize;
            rect_Chest_Red.Width = cellSize;
            rect_Chest_Blue.Height = cellSize;
            rect_Chest_Blue.Width = cellSize;
            rect_Player_Red.Height = cellSize;
            rect_Player_Red.Width = cellSize;
            rect_Player_Blue.Height = cellSize;
            rect_Player_Blue.Width = cellSize;

            //Заполнение ректанглов изображением сундуков
            rect_Chest_Red.Fill = ib_Chest_Red;
            rect_Chest_Blue.Fill = ib_Chest_Blue;

            //Размеры viewBox или размеры спрайта
            charH = 64; //высота спрайта
            charW = 64; //ширина спрайта
            //Настройки кисти для анимации Красного Игрока
            currentRow_P1 = 0;
            currentFrame_P1 = 0;
            ib_Player_Red.AlignmentX = AlignmentX.Left;
            ib_Player_Red.AlignmentY = AlignmentY.Top;
            ib_Player_Red.Stretch = Stretch.UniformToFill;
            ib_Player_Red.ViewboxUnits = BrushMappingMode.Absolute;
            ib_Player_Red.Viewbox = new Rect(charW * currentFrame_P1, charH * currentRow_P1, charW * (currentFrame_P1 + 1), charH * (currentRow_P1 + 1));
            rect_Player_Red.Fill = ib_Player_Red;

            //Настройки кисти для анимации Синего игрока
            currentRow_P2 = 0;
            currentFrame_P2 = 0;
            ib_Player_Blue.AlignmentX = AlignmentX.Left;
            ib_Player_Blue.AlignmentY = AlignmentY.Top;
            ib_Player_Blue.Stretch = Stretch.UniformToFill;
            ib_Player_Blue.ViewboxUnits = BrushMappingMode.Absolute;
            ib_Player_Blue.Viewbox = new Rect(charW * currentFrame_P2, charH * currentRow_P2, charW * (currentFrame_P2 + 1), charH * (currentRow_P2 + 1));
            rect_Player_Blue.Fill = ib_Player_Blue;

            //Создание сетки и заполнение тайлами
            FieldGrid.Columns = currentMap.Width;
            FieldGrid.Rows = currentMap.Height;
            FieldGrid.Width = cellSize * FieldGrid.Columns;
            FieldGrid.Height = cellSize * FieldGrid.Rows;
            foreach (Cell cell in currentMap.grid)
            {
                Rectangle rect = new Rectangle();
                if (cell.Type == Cell.cellType.Open)
                {
                    rect.Fill = ib_OpenCell;
                }
                else if (cell.Type == Cell.cellType.DeadEnd)
                {
                    rect.Fill = ib_DeadEndCell;
                }
                else if (cell.Type == Cell.cellType.RedFinish)
                {
                    rect.Fill = ib_RedFinish;
                }
                else if (cell.Type == Cell.cellType.BlueFinish)
                {
                    rect.Fill = ib_BlueFinish;
                }
                else
                {
                    rect.Fill = ib_CloseCell;
                }
                FieldGrid.Children.Add(rect);
            }
            //Добавление объектов в сцену
            scene.Children.Add(FieldGrid);
            scene.Children.Add(rect_Chest_Red);
            scene.Children.Add(rect_Chest_Blue);
            scene.Children.Add(rect_Player_Red);
            scene.Children.Add(rect_Player_Blue);

            //Копирование текущих координат игровых объектов
            xPos_Player_Red = currentMap.playerRed.xPos * cellSize;
            yPos_Player_Red = currentMap.playerRed.yPos * cellSize;
            xPos_Player_Blue = currentMap.playerBlue.xPos * cellSize;
            yPos_Player_Blue = currentMap.playerBlue.yPos * cellSize;
            xPos_Chest_Red = currentMap.chestRed.xPos * cellSize;
            yPos_Chest_Red = currentMap.chestRed.yPos * cellSize;
            xPos_Chest_Blue = currentMap.chestBlue.xPos * cellSize;
            yPos_Chest_Blue = currentMap.chestBlue.yPos * cellSize;


            //Настройки анимации
            frameCount = 9; //Количество кадров анимации
            fps = 30; //Количество обновлений объектов рендера в секунду
            //gameSpeed = 3; //Скорость перемещения объектов по полю
            gameSpeed = 1 + (int)(cellSize / fps); //Скорость перемещения объектов по полю


            //Создание таймера, который запускает эвент с определенным интервалом
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / fps);
            dispatcherTimer.Start();

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

        private int CalculateCellSize()  //Расчёт размера клетки исходя из размера экрана и количества клеток          
        {
            int newSize;
            double cellHeight = (this.Height) / (currentMap.Height);
            double cellWidth = (this.Width) / (currentMap.Width);
            newSize = Math.Min((int)cellHeight, (int)cellWidth);
            return newSize;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (currentMap.isWinnable()
                &&(currentMap.chestRed.currentState == Creature.State.Idle)
                &&(currentMap.chestBlue.currentState == Creature.State.Idle))
            {
                MessageBox.Show("Обе коробки доставлены! Поздравляем! Вы прошли карту!");
                //this.Close(); //Закрыть игру или вместо этого запустить новую карту
                CreateNewGame(new Map(LoadMap()));
            }
            //Если состояние игрока/сундука == "в движении", то изменять координаты ректангла в сторону движения
            if (currentMap.playerRed.currentState==Creature.State.Moving)
            {
                switch (currentMap.playerRed.lastDirection)
                {
                    case Creature.Direction.Up:
                        currentRow_P1 = 0;
                        yPos_Player_Red -=  gameSpeed % (Math.Abs(yPos_Player_Red - currentMap.playerRed.yPos * cellSize) +1);
                        break;
                    case Creature.Direction.Left:
                        currentRow_P1 = 1;
                        xPos_Player_Red -= gameSpeed % (Math.Abs(xPos_Player_Red - currentMap.playerRed.xPos * cellSize) + 1);
                        break;
                    case Creature.Direction.Down:
                        currentRow_P1 = 2;
                        yPos_Player_Red += gameSpeed % (Math.Abs(yPos_Player_Red - currentMap.playerRed.yPos * cellSize) + 1);
                        break;
                    case Creature.Direction.Right:
                        currentRow_P1 = 3;
                        xPos_Player_Red += gameSpeed % (Math.Abs(xPos_Player_Red - currentMap.playerRed.xPos * cellSize) + 1);
                        break;
                    default:
                        break;
                }
                currentFrame_P1 = (currentFrame_P1 + 1 + frameCount) % frameCount;
            }
            if (currentMap.playerBlue.currentState == Creature.State.Moving)
            {
                switch (currentMap.playerBlue.lastDirection)
                {
                    case Creature.Direction.Up:
                        currentRow_P2 = 0;
                        yPos_Player_Blue -= gameSpeed % (yPos_Player_Blue - currentMap.playerBlue.yPos * cellSize + 1);
                        break;
                    case Creature.Direction.Left:
                        currentRow_P2 = 1;
                        xPos_Player_Blue -= gameSpeed % (xPos_Player_Blue - currentMap.playerBlue.xPos * cellSize + 1);
                        break;
                    case Creature.Direction.Down:
                        currentRow_P2 = 2;
                        yPos_Player_Blue += gameSpeed % (currentMap.playerBlue.yPos * cellSize - yPos_Player_Blue + 1);
                        break;
                    case Creature.Direction.Right:
                        currentRow_P2 = 3;
                        xPos_Player_Blue += gameSpeed % (currentMap.playerBlue.xPos * cellSize - xPos_Player_Blue + 1);
                        break;
                    default:
                        break;
                }
                currentFrame_P2 = (currentFrame_P2 + 1 + frameCount) % frameCount;
            }
            if (currentMap.chestRed.currentState == Creature.State.Moving)
            {
                switch (currentMap.chestRed.lastDirection)
                {
                    case Creature.Direction.Up:
                        yPos_Chest_Red -= gameSpeed % (Math.Abs(yPos_Chest_Red - currentMap.chestRed.yPos * cellSize) + 1);
                        break;
                    case Creature.Direction.Left:
                        xPos_Chest_Red -= gameSpeed % (Math.Abs(xPos_Chest_Red - currentMap.chestRed.xPos * cellSize) + 1);
                        break;
                    case Creature.Direction.Down:
                        yPos_Chest_Red += gameSpeed % (Math.Abs(yPos_Chest_Red - currentMap.chestRed.yPos * cellSize) + 1);
                        break;
                    case Creature.Direction.Right:
                        xPos_Chest_Red += gameSpeed % (Math.Abs(xPos_Chest_Red - currentMap.chestRed.xPos * cellSize) + 1);
                        break;
                    default:
                        break;
                }
            }
            if (currentMap.chestBlue.currentState == Creature.State.Moving)
            {
                switch (currentMap.chestBlue.lastDirection)
                {
                    case Creature.Direction.Up:
                        yPos_Chest_Blue -= gameSpeed % (Math.Abs(yPos_Chest_Blue - currentMap.chestBlue.yPos * cellSize) + 1);
                        break;
                    case Creature.Direction.Left:
                        xPos_Chest_Blue -= gameSpeed % (Math.Abs(xPos_Chest_Blue - currentMap.chestBlue.xPos * cellSize) + 1);
                        break;
                    case Creature.Direction.Down:
                        yPos_Chest_Blue += gameSpeed % (Math.Abs(yPos_Chest_Blue - currentMap.chestBlue.yPos * cellSize) + 1);
                        break;
                    case Creature.Direction.Right:
                        xPos_Chest_Blue += gameSpeed % (Math.Abs(xPos_Chest_Blue - currentMap.chestBlue.xPos * cellSize) + 1);
                        break;
                    default:
                        break;
                }
            }

            //Если координаты ректангла совпадают с координатами игрока/сундука, то состояние обозначить как "покой"
            if (xPos_Player_Red == currentMap.playerRed.xPos * cellSize && yPos_Player_Red == currentMap.playerRed.yPos * cellSize)
            {
                currentMap.playerRed.Arrived();
                currentFrame_P1 = 0;
            }
            if (xPos_Player_Blue == currentMap.playerBlue.xPos * cellSize && yPos_Player_Blue == currentMap.playerBlue.yPos * cellSize)
            {
                currentMap.playerBlue.Arrived();
                currentFrame_P2 = 0;
            }
            if (xPos_Chest_Red == currentMap.chestRed.xPos * cellSize && yPos_Chest_Red == currentMap.chestRed.yPos * cellSize)
            {
                currentMap.chestRed.Arrived();
            }
            if (xPos_Chest_Blue == currentMap.chestBlue.xPos * cellSize && yPos_Chest_Blue == currentMap.chestBlue.yPos * cellSize)
            {
                currentMap.chestBlue.Arrived();
            }

            //Рендер ректангла на основе координат
            rect_Player_Red.RenderTransform = new TranslateTransform(xPos_Player_Red, yPos_Player_Red);
            rect_Player_Blue.RenderTransform = new TranslateTransform(xPos_Player_Blue, yPos_Player_Blue);
            rect_Chest_Red.RenderTransform = new TranslateTransform(xPos_Chest_Red, yPos_Chest_Red);
            rect_Chest_Blue.RenderTransform = new TranslateTransform(xPos_Chest_Blue, yPos_Chest_Blue);

            //Изменение кадра анимации на основе номера строки и номера столбца
            ib_Player_Red.Viewbox = new Rect(charW * currentFrame_P1, charH * currentRow_P1, charW * (currentFrame_P1 + 1), charH * (currentRow_P1 + 1));
            ib_Player_Blue.Viewbox = new Rect(charW * currentFrame_P2, charH * currentRow_P2, charW * (currentFrame_P2 + 1), charH * (currentRow_P2 + 1));
            rect_Player_Red.Fill = ib_Player_Red;
            rect_Player_Blue.Fill = ib_Player_Blue;
        }

        private void GameScreen_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    currentMap.MoveTo(currentMap.playerRed, Creature.Direction.Up);
                    break;
                case Key.Up:
                    currentMap.MoveTo(currentMap.playerBlue, Creature.Direction.Up);
                    break;
                case Key.A:
                    currentMap.MoveTo(currentMap.playerRed, Creature.Direction.Left);
                    break;
                case Key.Left:
                    currentMap.MoveTo(currentMap.playerBlue, Creature.Direction.Left);
                    break;
                case Key.S:
                    currentMap.MoveTo(currentMap.playerRed, Creature.Direction.Down);
                    break;
                case Key.Down:
                    currentMap.MoveTo(currentMap.playerBlue, Creature.Direction.Down);
                    break;
                case Key.D:
                    currentMap.MoveTo(currentMap.playerRed, Creature.Direction.Right);
                    break;
                case Key.Right:
                    currentMap.MoveTo(currentMap.playerBlue, Creature.Direction.Right);
                    break;
                case Key.R:
                    //currentMap = new Map(LoadMap());
                    break;
                default:
                    //MessageBox.Show();
                    break;
            }

        }

    }
}
