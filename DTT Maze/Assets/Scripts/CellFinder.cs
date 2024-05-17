using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellFinder : MonoBehaviour
{
    ///<summary>
    ///This class will only have one purpose and that is to find all the cells and return an array with all the cells.
    /// </summary>

    [SerializeField] private MazeGenerator mazeGenerator;

    [SerializeField]
    private Cell[] cells;
    private GameObject[] walls;
    private int rowCount = 0, childCount = 0;

    //We will receive the gameobject that houses all walls for the maze and the amount of cells we expect to find.
    public void FindCells(GameObject wallsParent, int cellAmount, int mazeHeight, int mazeWidth)
    {
        cells = new Cell[cellAmount];
        walls = new GameObject[wallsParent.transform.childCount];

        //First the easy part, assign all the walls we have to our array
        for (int i = 0; i < walls.Length; i++)
        {
            walls[i] = wallsParent.transform.GetChild(i).gameObject;
        }

        //Then the hard part, considering we are building from southwest to northeast that's how the cells are searched for.
        //To the current cell assign west and south, then go to the next child which should be the vertical wall
        //and give the remaining two walls to this cell and then continue this until all cells are found.
        for (int i = 0; i < mazeHeight - 1; i++)
        {
            //Realized in my current setup I would loop once too many, this prevents that possibilty entirely.
            if (i >= cells.Length || rowCount >= walls.Length)
                break;

            cells[i].westWall = walls[rowCount];
            cells[i].southWall = walls[childCount + (mazeHeight + 1) * mazeWidth];

            childCount++;

            cells[i].northWall = walls[childCount + (mazeHeight + 1) * mazeWidth + mazeHeight - 1];

            rowCount++;

            cells[i].eastWall = walls[rowCount];
        }

        mazeGenerator.CreateMaze();
    }
}
