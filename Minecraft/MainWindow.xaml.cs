
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Media;

using System.Windows.Shapes;
using System.Windows.Threading;

namespace Minesweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Graph graph;
        Boolean gameOver;
        TextBlock gameOverText;
        int numOfMines = 150;
        int numOfActualMines;
        int numOfFlaggedMines;
        public static int visitedCells = 0;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        
        
        long currentTime;
        double startTime;
        double timeUsed;

        public MainWindow()
        {
            InitializeComponent();
            grid.Background = new SolidColorBrush(Colors.Aqua);
            gameOver = false;
            graph = new Graph(canvas, numOfMines);
            graph.drawGraph();
            numOfActualMines = graph.getActualNumberOfMines();
            totalMinesLabel.Content = "Total Mines: " + numOfActualMines;
            numOfFlaggedMines = 0;
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += dispatcher_timer_tick;
            dispatcherTimer.Start();
            startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        private void dispatcher_timer_tick(object sender, EventArgs e)
        {
            if (!gameOver)
            {
                currentTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                timeUsed = currentTime - startTime;
                int timeInSeconds = (int)(timeUsed/1000);
                int timeInMinutes = (timeInSeconds/60);
                int secondsToDisplay = timeInSeconds % 60;
                timeLabel.Content = String.Format("Time: {0}:{1}", timeInMinutes, secondsToDisplay);
            }
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
            visitedCells++;
            
            if(cell.getNumOfNeighbourmines() == 0 && !cell.isFlagged)
            {
                graph.clearVacantMineField(cell);
            }
            if (cell.containsAMine() && !cell.isFlagged)
            {
                gameOver = true;
                gameOverText = new TextBlock();
                //Reveal all bombs
                graph.revealAllMines();

                gameOverText.Text = "Game Over";
                gameOverText.FontSize = 150;
                gameOverText.Foreground = Brushes.IndianRed;
                canvas.Children.Add(gameOverText);
                Canvas.SetLeft(gameOverText, 100);
                Canvas.SetTop(gameOverText, 300);
            }
            scoreLabel.Content = "Score: " + visitedCells;
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
            if (cell.containsAMine() && cell.isFlagged)
            {
                numOfFlaggedMines++;
                flaggedMinesLabel.Content = "Flagged Mines: " + numOfFlaggedMines;
                if(numOfFlaggedMines == numOfActualMines)
                {
                    gameOver = true;
                    gameOverText = new TextBlock();

                    gameOverText.Text = "Weiner";
                    gameOverText.FontSize = 150;
                    gameOverText.Foreground = Brushes.IndianRed;
                    canvas.Children.Add(gameOverText);
                    Canvas.SetLeft(gameOverText, 200);
                    Canvas.SetTop(gameOverText, 300);
                }
            }else if(cell.containsAMine() && !cell.isFlagged)
            {
                numOfFlaggedMines--;
                flaggedMinesLabel.Content = "Flagged Mines: " + numOfFlaggedMines;
            }
        }

        private void windowResetEventHandler(object sender, System.Windows.Input.KeyEventArgs e)
        {
          //Reset Everything
            if (e.Key.ToString().Equals("Space"))
            {
                Debug.WriteLine("Resetting");
                graph = new Graph(canvas, numOfMines);
                graph.drawGraph();
                gameOver = false;
                canvas.Children.Remove(gameOverText);
                numOfFlaggedMines = 0;
                flaggedMinesLabel.Content = "Flagged Mines: " + numOfFlaggedMines;
                numOfActualMines = graph.getActualNumberOfMines();
                totalMinesLabel.Content = "Total Mines: " + numOfActualMines;
                visitedCells = 0;
                scoreLabel.Content = "Score: " + visitedCells;
                startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                timeLabel.Content = String.Format("Time: {0}:{1}", 00, 00);
            }
           
            
        }
    }
}
