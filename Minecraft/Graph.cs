using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Minesweeper
{
    internal class Graph
    {
        Cell[,] cellGrid;
        int xNumberofCells;
        int yNumberofCells;
        Canvas canvas;
        int[,] adjMatrix;
        Cell[] cellList; //the index in the cellList corresponds to their indices in the adjMatrix
        Stack<Cell> cellStack;
        DispatcherTimer dispatchTimer;
        int actualNumOfMines;

        public Graph(Canvas canvas, int numOfMines)
        {
            this.canvas = canvas;
            xNumberofCells = (int)canvas.Width/Cell.cellLength;
            yNumberofCells = (int)canvas.Height/Cell.cellLength;
            
            cellGrid = new Cell[xNumberofCells, yNumberofCells];

            for (int i = 0; i < cellGrid.GetLength(0); i++)
            {
                for (int j = 0; j < cellGrid.GetLength(1); j++)
                {
                    cellGrid[i, j] = new Cell(new Coordinate(i, j), this.canvas);
                }
            }
            
            //Set up the cellList
            cellList = new Cell[xNumberofCells * yNumberofCells];
            //Populate the cellList
            int cellIndexCounter = 0;
            for (int i = 0; i < cellGrid.GetLength(0); i++)
            {
                for (int j = 0; j < cellGrid.GetLength(1); j++)
                {
                    cellList[cellIndexCounter++] = cellGrid[i, j];
                }
            }
            //Set the adjanceny values here
            adjMatrix = new int[xNumberofCells * yNumberofCells, xNumberofCells * yNumberofCells];
            setUpAdjMatrix();

            //CalculateActualNumberOfMines;
            this.actualNumOfMines = setMines(numOfMines);
            calculateNumOfMineNeighbours();

        }

        public int getActualNumberOfMines()
        {
            return actualNumOfMines;
        }

        public void printAdjMatrix()
        {
            for (int i = 0; i < adjMatrix.GetLength(1); i++)
            {
                for (int j = 0; j < adjMatrix.GetLength(0); j++)
                {
                    Console.Write(adjMatrix[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }

        public void revealAllNeighboursTest(int i, int j)
        {
            revealAllNeighboursOfCellTest(cellGrid[i, j]);
        }

        public void revealAllNeighboursOfCellTest(Cell cell)
        {
            int indexOfCelIlInCellList = getCellListIndexOfCell(cell);

            for (int j = 0; j < cellList.Length; j++)
            {
                if (adjMatrix[indexOfCelIlInCellList, j] == 1)
                {
                    cellList[j].neighbourRevealTest();
                    cellList[j].printCoordinates();
                }
            }
        }

        public void setUpAdjMatrix()
        {
            Cell currentCell;
            for (int i = 0; i < cellGrid.GetLength(0); i++)
            {
                for (int j = 0; j < cellGrid.GetLength(1); j++)
                {
                    currentCell = cellGrid[i, j];
                    int indexOfCurrentCellInCellList = getCellListIndexFromCoordinates(i,j);

                    //Top
                    if(j > 0)
                    {
                        int topIndex = getCellListIndexFromCoordinates(i, j - 1);
                        adjMatrix[indexOfCurrentCellInCellList, topIndex] = 1;
                    }
                    
                    //Top right
                    if(i < xNumberofCells-1 && j > 0)
                    {
                        int topRightIndex = getCellListIndexFromCoordinates(i + 1, j - 1);
                        adjMatrix[indexOfCurrentCellInCellList, topRightIndex] = 1;
                    }
                    
                    //Right
                    if(i < xNumberofCells - 1)
                    {
                        int rightIndex = getCellListIndexFromCoordinates(i + 1, j);
                        adjMatrix[indexOfCurrentCellInCellList, rightIndex] = 1;

                    }
                   
                    //Bottom right
                    if(i < xNumberofCells-1 && j < yNumberofCells - 1)
                    {
                        int bottomRightIndex = getCellListIndexFromCoordinates(i + 1, j + 1);
                        adjMatrix[indexOfCurrentCellInCellList, bottomRightIndex] = 1;
                    }

                    //Bottom
                    if (j < yNumberofCells-1)
                    {
                        int bottomIndex = getCellListIndexFromCoordinates(i, j + 1);
                        adjMatrix[indexOfCurrentCellInCellList, bottomIndex] = 1;
                    }

                    //Bottom left
                    if (i > 0 && j < yNumberofCells-1)
                    {
                        int bottomLeftIndex = getCellListIndexFromCoordinates(i - 1, j + 1);
                        adjMatrix[indexOfCurrentCellInCellList, bottomLeftIndex] = 1;
                    }

                    //Left
                    if (i > 0)
                    {
                        int leftIndex = getCellListIndexFromCoordinates(i - 1, j);
                        adjMatrix[indexOfCurrentCellInCellList, leftIndex] = 1;
                    }

                    //Top left
                    if (i > 0 && j > 0)
                    {
                        int topLeftIndex = getCellListIndexFromCoordinates(i - 1, j - 1);
                        adjMatrix[indexOfCurrentCellInCellList, topLeftIndex] = 1;
                    }
                }
            }
        }

        public int getCellListIndexFromCoordinates(int i, int j)
        {
            return i * yNumberofCells + j;
        }

        public int getCellListIndexOfCell(Cell cell)
        {
            for (int i = 0; i < cellList.Length; i++)
            {
                if (cellList[i] == cell)
                {
                    return i;
                }
            }
            Debug.WriteLine("Cell not found");
            return 0;
        }

        public void clearVacantMineField(Cell startCell)
        {
            cellStack = new Stack<Cell>();
            
            Cell traversalCell = startCell;
            cellStack.Push(traversalCell);
            traversalCell.isDFSVisited = true;
            
            while(cellStack.Count != 0)
            {
                traversalCell = cellStack.Pop();
                traversalCell.visitIt();
                MainWindow.visitedCells++;
                pushAppropriateNeighboursToStack(traversalCell);
                
            }
        }

        public void pushAppropriateNeighboursToStack(Cell cell)
        {
            int indexOfCelIlInCellList = getCellListIndexOfCell(cell);

            for (int j = 0; j < cellList.Length; j++)
            {
                if (adjMatrix[indexOfCelIlInCellList, j] == 1 && cell.getNumOfNeighbourmines() == 0 && !cellList[j].isDFSVisited)
                {
                    cellStack.Push(cellList[j]);
                    cellList[j].isDFSVisited = true;
                }
            }
        }


        public void drawGraph()
        {
            for (int i = 0; i < cellGrid.GetLength(0); i++)
            {
                for (int j = 0; j < cellGrid.GetLength(1); j++)
                {
                    cellGrid[i, j].drawTheCell(false);
                }
            }
        }

        public Cell getCellAtPosition(Coordinate coordinate)
        {
            return cellGrid[coordinate.getXCoordinate(), coordinate.getYCoordinate()];
        }

        public int setMines(int numOfMines)
        {
            Random random = new Random();
            for (int a = 0; a < numOfMines; a++)
            {
                int i = random.Next(0, xNumberofCells);
                int j = random.Next(0, yNumberofCells);
                cellGrid[i, j].placeTheMine();
            }

            int actualNumOfMines = 0;
            for (int i = 0; i < cellList.Length; i++)
            {
                if (cellList[i].containsAMine())
                {
                    actualNumOfMines++;
                }
            }
            return actualNumOfMines;
        }

        public void calculateNumOfMineNeighbours()
        {
            for (int i = 0; i < cellGrid.GetLength(0); i++)
            {
                for (int j = 0; j < cellGrid.GetLength(1); j++)
                {
                    int numOfMineNeighbours = 0;

                    //Top
                    if(j > 0 && cellGrid[i, j - 1].containsAMine())
                    {
                        numOfMineNeighbours++;
                    }
                    //Top right
                    if (i<xNumberofCells-1 && j > 0 && cellGrid[i+1, j - 1].containsAMine())
                    {
                        numOfMineNeighbours++;
                    }
                    //Right
                    if (i<xNumberofCells-1 && cellGrid[i+1, j].containsAMine())
                    {
                        numOfMineNeighbours++;
                    }

                    //Bottom right
                    if (i<xNumberofCells-1 && j<yNumberofCells -1 && cellGrid[i + 1, j + 1].containsAMine())
                    {
                        numOfMineNeighbours++;
                    }

                    //Bottom
                    if (j < yNumberofCells-1 && cellGrid[i, j + 1].containsAMine())
                    {
                        numOfMineNeighbours++;
                    }

                    //Bottom left
                    if (i > 0 && j < yNumberofCells-1 && cellGrid[i-1, j + 1].containsAMine())
                    {
                        numOfMineNeighbours++;
                    }
                    //Left
                    if (i > 0 && cellGrid[i-1, j].containsAMine())
                    {
                        numOfMineNeighbours++;
                    }

                    //Top left
                    if (i > 0 && j > 0 && cellGrid[i - 1, j - 1].containsAMine())
                    {
                        numOfMineNeighbours++;
                    }
                    cellGrid[i, j].setNumOfNeighbourMines(numOfMineNeighbours);
                }
            }
        }

        public void revealAllMines()
        {
            Debug.WriteLine("Rvealing mines");
            for (int i = 0; i < cellList.Length; i++)
            {
                if (cellList[i].containsAMine() && !cellList[i].isFlagged)
                {
                    cellList[i].triggerTheMine();
                    cellList[i].drawTheCell(false);

                }
            }
        }
    }
}
