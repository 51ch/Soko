using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        public MainWindow()
        {
            InitializeComponent();
            currentMap = new Map(LoadMap());
            cellSize = CalculateCellSize();
            FieldGrid.Columns = currentMap.Width;
            FieldGrid.Rows = currentMap.Height;
            FieldGrid.Width = cellSize;
            FieldGrid.Height = cellSize;
            //ImageBrush ib = new ImageBrush();
            //ib.TileMode = TileMode.Tile;
            //ib.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/tiles/wall.jpg", UriKind.Absolute)); 

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
        private int CalculateCellSize()
        {
            int newSize;
            double cellHeight = (scene.ActualHeight) / (currentMap.Height);
            double cellWidth = (scene.ActualWidth) / (currentMap.Width);
            newSize = Math.Min((int)cellHeight, (int)cellWidth);
            return newSize;
        }

        private void GridResize()
        {
            cellSize = CalculateCellSize();
            FieldGrid.Width = cellSize;
            FieldGrid.Height = cellSize;
        }
    }
}
