using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FindOpenCell : MonoBehaviour
{
    private List<Cell> unvisitedCells = new List<Cell>();

    private Cell[,] cellGrid;
    private Cell cellWest, cellEast, cellSouth, cellNorth;

    /// <summary>
    /// This function simply calls the internal function to get a list of all unvisted cells adjacent to the current cell and then returns it.
    /// </summary>
    /// <param name="currentCell">Cell who's neighbours will be checked.</param>
    /// <param name="cellGrid">A integer based grid that stores all found cells.</param>
    /// <param name="mazeWidth">Width of the maze.</param>
    /// <param name="mazeHeight">Height of the maze.</param>
    /// <returns></returns>
    public List<Cell> GetUnvisitedCell(Cell currentCell, Cell[,] cellGrid, int mazeWidth, int mazeHeight)
    {
        if (this.cellGrid == null)
            this.cellGrid = cellGrid; // Dont need duplicates

        GetUnvisitedCells(currentCell, mazeWidth, mazeHeight);

        return unvisitedCells;
    }

    /// <summary>
    /// Collects all valid neighbour cells to the current cell in a list.
    /// </summary>
    /// <param name="currentCell">Cell who's neighbours will be checked.</param>
    /// <param name="mazeWidth">Width of the maze.</param>
    /// <param name="mazeHeight">Height of the maze.</param>
    private void GetUnvisitedCells(Cell currentCell, int mazeWidth, int mazeHeight)
    {
        unvisitedCells.Clear();

        int x = (int)currentCell.gridPos.x; //Due to setting up a much simpler grid we can now just check
        int z = (int)currentCell.gridPos.y; //the positions of each cell in grid and find our neighbours.

        if (cellGrid.Length <= 0)
            return;

        if (x + 1 < mazeWidth)                  // Cell to the right
        {
            cellEast = cellGrid[x + 1, z];      // Grab the cell

            if (cellEast.visited == false)      // If it's not visited yet
                unvisitedCells.Add(cellEast);   // Add to the list
        }

        if (x - 1 >= 0)
        {
            cellWest = cellGrid[x - 1, z];

            if (cellWest.visited == false)
                unvisitedCells.Add(cellWest);
        }

        if (z + 1 < mazeHeight)
        {
            cellNorth = cellGrid[x, z + 1];

            if (cellNorth.visited == false)
                unvisitedCells.Add(cellNorth);
        }

        if (z - 1 >= 0)
        {
            cellSouth = cellGrid[x, z - 1];

            if (cellSouth.visited == false)
                unvisitedCells.Add(cellSouth);
        }
    }
}
