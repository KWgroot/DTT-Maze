using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FindOpenCell : MonoBehaviour
{
    private Cell[,] cellGrid;
    private List<Cell> unvisitedCells = new List<Cell>();

    private int mazeHeight, mazeWidth;
    private Cell cellWest, cellEast, cellSouth, cellNorth;

    public Cell GetUnvisitedCell(Cell currentCell, Cell[,] cellGrid, int mazeHeight, int mazeWidth)
    {
        this.mazeHeight = mazeHeight;
        this.mazeWidth = mazeWidth;

        if (this.cellGrid == null)
            this.cellGrid = cellGrid;

        GetUnvisitedCells(currentCell);

        return unvisitedCells.OrderBy(x => Random.Range(1, 10)).FirstOrDefault();
    }

    private void GetUnvisitedCells(Cell currentCell)
    {
        unvisitedCells.Clear();

        int x = (int)currentCell.position.x + mazeWidth / 2;
        int z = (int)currentCell.position.z + mazeHeight / 2;

        if (x - 1 >= 0) // West
        {
            cellWest = cellGrid[x - 1, z];

            if (cellWest.visited == false)
                unvisitedCells.Add(cellWest);
        }

        if (x + 1 < mazeWidth) // East
        {
            cellEast = cellGrid[x + 1, z];

            if (cellEast.visited == false)
                unvisitedCells.Add((cellEast));
        }

        if (z - 1 >= 0)
        {
            cellSouth = cellGrid[x, z - 1];

            if (cellSouth.visited == false)
                unvisitedCells.Add(cellSouth);
        }

        if (z + 1 < mazeHeight)
        {
            cellNorth = cellGrid[x, z + 1];

            if (cellNorth.visited == false)
                unvisitedCells.Add(cellNorth);
        }
    }
}
