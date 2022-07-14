
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Media;

using System.Windows.Shapes;


namespace Minesweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Graph graph;
        Boolean gameOver;
        public MainWindow()
        {
            InitializeComponent();
            gameOver = false;
            graph = new Graph(canvas);
            graph.drawGraph();
            
        }

        private void leftMouseUpEventHandler(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (gameOver)
            {
                return;
            }
            int xCoord = Coordinate.getXCoord((int)e.GetPosition(canvas).X);
            int yCoord = Coordinate.getYCoord((int)e.GetPosition(canvas).Y);
            Cell cell = graph.getCellAtPosition(new Coordinate(xCoord, yCoord));
            cell.visitIt();
            if(cell.getNumOfNeighbourmines() == 0)
            {
                graph.clearVacantMineField(cell);
            }
            if (cell.containsAMine())
            {
                gameOver = true;
            }
        }

        private void rightMouseUpEventHandler(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (gameOver)
            {
                return;
            }
            int xCoord = Coordinate.getXCoord((int)e.GetPosition(canvas).X);
            int yCoord = Coordinate.getYCoord((int)e.GetPosition(canvas).Y);
            Cell cell = graph.getCellAtPosition(new Coordinate(xCoord, yCoord));
            cell.flagIt();
        }
    }
}
