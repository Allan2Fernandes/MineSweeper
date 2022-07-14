using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Minesweeper
{
    internal class Cell
    {
        Coordinate coordinate;
        Boolean isFlagged; //If flagged, it doesn't let you trigger the mine
        Boolean isATrap;
        Boolean isVisited;
        Boolean mineIsTriggered;
        Canvas canvas;
        public static int cellLength = 30;
        int xPixelPosition;
        int yPixelPosition;
        Rectangle rectangleFill;
        Rectangle rectangleStroke;
        int numOfNeighbourMines = 0;
        TextBlock numOfMinesTextBlock;
        public Boolean isDFSVisited;

        public Cell(Coordinate coordinate, Canvas canvas)
        {
            this.coordinate = coordinate;
            this.canvas = canvas;

            isFlagged = false;
            isATrap = false;
            isVisited = false;
            isDFSVisited = false;

            xPixelPosition = this.coordinate.getXPixelPosition();
            yPixelPosition = this.coordinate.getYPixelPosition();

            //Construct the rectangle here
            rectangleFill = new Rectangle
            {
                Height = cellLength,
                Width = cellLength
            };
            rectangleFill.Fill = new SolidColorBrush(Colors.Red);
            rectangleStroke = new Rectangle
            {
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Height = cellLength,
                Width = cellLength
            };
            numOfMinesTextBlock = new TextBlock();
            numOfMinesTextBlock.FontSize = 25;
            
            
        }

        public void visitIt()
        {
            if (isFlagged)
            {
                return;
            }
            if (!isVisited)
            {
                isVisited = true;
            }
            if (isATrap)
            {
                triggerTheMine();
            }
            else
            {
                //Do a DFS or BFS and uncover all neighbouring 0s
            }
            drawTheCell(true);
        }

        public Boolean containsAMine()
        {
            return isATrap;
        }

        public void placeTheMine()
        {
            isATrap = true;
        }

        public void flagIt()
        {
            if (isVisited)
            {
                return;
            }
            if (isFlagged)
            {
                isFlagged = false;
            }
            else
            {
                isFlagged = true;
            }
            drawTheCell(false);
        }

        public void triggerTheMine()
        {
            mineIsTriggered = true;
        }

        public void setNumOfNeighbourMines(int numOfNeighbourMines)
        {
            this.numOfNeighbourMines = numOfNeighbourMines;
        }

        public int getNumOfNeighbourmines()
        {
            return numOfNeighbourMines;
        }

        public void drawTheCell(Boolean revealNeighbourNumber)
        {
            canvas.Children.Remove(rectangleFill);
            canvas.Children.Remove(rectangleStroke);
            canvas.Children.Remove(numOfMinesTextBlock);


            if (!isVisited)
            {
                //Default green background. Maybe an alternating grid
                rectangleFill.Fill = new SolidColorBrush(Colors.Green);
            }
            else
            {
                rectangleFill.Fill = new SolidColorBrush(Colors.Yellow);
            }
            if (isFlagged)
            {
                rectangleFill.Fill = new SolidColorBrush(Colors.Red);
            }else if (mineIsTriggered)
            {
                rectangleFill.Fill = new SolidColorBrush(Colors.Black);
                
            }
            
            canvas.Children.Add(rectangleFill);
            Canvas.SetLeft(rectangleFill, xPixelPosition);
            Canvas.SetTop(rectangleFill, yPixelPosition);
            canvas.Children.Add(rectangleStroke);
            Canvas.SetLeft(rectangleStroke, xPixelPosition);
            Canvas.SetTop(rectangleStroke, yPixelPosition);


            if (revealNeighbourNumber)
            {
                canvas.Children.Add(numOfMinesTextBlock);
                numOfMinesTextBlock.Text = numOfNeighbourMines.ToString();
                if(numOfNeighbourMines == 0)
                {
                    numOfMinesTextBlock.Text = "";
                }
                Canvas.SetLeft(numOfMinesTextBlock, xPixelPosition + 8);
                Canvas.SetTop(numOfMinesTextBlock, yPixelPosition - 4);
            }
        }

        public void neighbourRevealTest()
        {
            canvas.Children.Remove(rectangleFill);
            canvas.Children.Remove(rectangleStroke);
            canvas.Children.Remove(numOfMinesTextBlock);
            rectangleFill.Fill = new SolidColorBrush(Colors.Aqua);

            canvas.Children.Add(rectangleFill);
            Canvas.SetLeft(rectangleFill, xPixelPosition);
            Canvas.SetTop(rectangleFill, yPixelPosition);
            canvas.Children.Add(rectangleStroke);
            Canvas.SetLeft(rectangleStroke, xPixelPosition);
            Canvas.SetTop(rectangleStroke, yPixelPosition);
        }

        public void printCoordinates()
        {
            Debug.WriteLine("xCoord is " + coordinate.getXCoordinate() + ", yCoord is " + coordinate.getYCoordinate());
        }
       
    }
}
