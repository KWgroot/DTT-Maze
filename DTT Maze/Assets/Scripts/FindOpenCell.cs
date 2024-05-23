using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FindOpenCell : MonoBehaviour
{
    private List<Cell> unvisitedCells = new List<Cell>();

    private Cell[,] cellGrid;
    private Cell cellWest, cellEast, cellSouth, cellNorth;

    public Cell GetUnvisitedCell(Cell currentCell, Cell[,] cellGrid, int mazeWidth, int mazeHeight)
    {
        if (this.cellGrid == null)
            this.cellGrid = cellGrid;

        GetUnvisitedCells(currentCell, mazeWidth, mazeHeight);

        return unvisitedCells.OrderBy(x => Random.Range(1, 10)).FirstOrDefault();
    }

    private void GetUnvisitedCells(Cell currentCell, int mazeWidth, int mazeHeight)
    {
        unvisitedCells.Clear();

        int x = (int)currentCell.gridPos.x;
        int z = (int)currentCell.gridPos.y;

        if (x + 1 < mazeWidth)
        {
            cellEast = cellGrid[x + 1, z];

            if (cellEast.visited == false)
                unvisitedCells.Add(cellEast);
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
