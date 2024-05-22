using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FindOpenCell : MonoBehaviour
{
    private List<Cell> unvisitedCells = new List<Cell>();
    private Cell[,] cellGrid;

    private int mazeHeight, mazeWidth;
    private Cell cellWest, cellEast, cellSouth, cellNorth;

    public Cell GetUnvisitedCell(Cell currentCell, Cell[,] cellGrid, int mazeHeight, int mazeWidth)
    {
        this.mazeHeight = mazeHeight;
        this.mazeWidth = mazeWidth;

        if (this.cellGrid == null)
            this.cellGrid = cellGrid;

        //unvisitedCells = GetUnvisitedCells(currentCell);

        return unvisitedCells.OrderBy(x => Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<Cell> GetUnvisitedCells(Cell currentCell)
    {
        int x = (int)currentCell.position.x;
        int z = (int)currentCell.position.z;

        if (x - 1 >= mazeWidth) // West
        {
            cellWest = cellGrid[x - 1, z];

            if (cellWest.visited == false)
                yield return cellWest;
        }

        if (x + 1 < mazeWidth) // East
        {
            cellEast = cellGrid[x + 1, z];

            if (cellEast.visited == false)
                yield return cellEast;
        }

        if (z - 1 >= mazeHeight)
        {
            cellSouth = cellGrid[x, z - 1];

            if (cellSouth.visited == false)
                yield return cellSouth;
        }

        if (z + 1 < mazeHeight)
        {
            cellNorth = cellGrid[x, z + 1];

            if (cellNorth.visited == false)
                yield return cellNorth;
        }
    }
}
