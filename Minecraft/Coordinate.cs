using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    internal class Coordinate
    {
        Coordinate coordinate;
        int xCoord;
        int yCoord;
        int xPixelPosition;
        int yPixelPosition;

        public Coordinate(int xCoord, int yCoord)
        {
            this.xCoord = xCoord;
            this.yCoord = yCoord;
            xPixelPosition = xCoord * Cell.cellLength;
            yPixelPosition = yCoord * Cell.cellLength;
        }

        public int getXCoordinate()
        {
            return xCoord;
        }
        
        public int getYCoordinate()
        {
            return yCoord;
        }

        public int getXPixelPosition()
        {
            return xPixelPosition;
        }

        public int getYPixelPosition()
        {
            return yPixelPosition;
        }

        public static int getXCoord(int xPixelPosition)
        {
            return (int) (xPixelPosition / Cell.cellLength);
        }

        public static int getYCoord(int yPixelPosition)
        {
            return ((int) (yPixelPosition / Cell.cellLength));
        }
    }

}
