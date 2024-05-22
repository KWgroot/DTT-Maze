using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellFinder : MonoBehaviour
{
    ///<summary>
    ///This class will only have one purpose and that is to find all the cells and return an array with all the cells.
    /// </summary>

    [SerializeField] private MazeGenerator mazeGenerator;
    [SerializeField] private Cell[] cells;
    private Cell[,] cellGrid;

    private GameObject[] walls;
    private int rowCount = 0, childCount = 0, x, z;

    //We will receive the gameobject that houses all walls for the maze and the amount of cells we expect to find.
    public Cell[] FindCells(Transform wallsParent, int cellAmount, int mazeHeight, int mazeWidth)
    {
        cells = new Cell[cellAmount];
        cellGrid = new Cell[mazeWidth, mazeHeight];
        walls = new GameObject[wallsParent.transform.childCount];

        //First the easy part, assign all the walls we have to our array
        for (int i = 0; i < walls.Length; i++)
        {
            walls[i] = wallsParent.transform.GetChild(i).gameObject;
        }

        //Then the hard part, considering we are building from southwest to northeast that's how the cells are searched for.
        int eastWestWalls = 0;      //we will check one side wall at a time, and considering the order the cells are stored in
        int northSouthWalls = 0;    //it is required to use a small increment in the middle between each side wall.
        int rowCount = 0;           //we check row by row and with this we can increment what row we're looking at
        int cellProgress = 0;       //check the cell number we're looking for

        for (int i = 0; i < mazeHeight; i++)
        {
            cells[cellProgress] = new Cell();

            cells[cellProgress].westWall = walls[eastWestWalls];
            cells[cellProgress].southWall = walls[northSouthWalls + (mazeHeight + 1) * mazeWidth];

            northSouthWalls++;
            eastWestWalls++;

            cells[cellProgress].northWall = walls[northSouthWalls + (mazeHeight + 1) * mazeWidth + mazeHeight - 1];
            cells[cellProgress].eastWall = walls[eastWestWalls];

            cells[cellProgress].SetPosition();

            x = (int)cells[cellProgress].position.x;
            z = (int)cells[cellProgress].position.z;

            cellGrid[x + mazeWidth / 2, z + mazeHeight / 2] = cells[cellProgress];

            rowCount++;
            cellProgress++;         //Next cell or row depending if we've reached the end of this row

            if (rowCount == mazeHeight && cellProgress < cells.Length)
            {
                //if we did reach the end of the row, start from it's beginning point
                eastWestWalls++;
                rowCount = 0;
                i = -1;
            }
        }

        StartCoroutine(mazeGenerator.ApplyAlgorithm(null, cells[0], cellGrid));
        return cells;
    }
}
