using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    //Cell prefab required to know what to place
    [SerializeField]
    private Cell cellPrefab;

    //Using a Range to let the maze be set from a minimum 10x10 to a maximum of 250x250
    [SerializeField][Range(10, 250)]
    private int width, height;

    //Since recursion will be used to backtrack a grid will be maintained to know what's where
    [SerializeField]
    private Cell[,] grid;

    private void Start()
    {
        //Fill the grid so it can be used
        grid = new Cell[width, height];

        //Use nested for-loops to place the maze shape and place it in the grid
        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                grid[w, h] = Instantiate(cellPrefab, new Vector3(w, 0, h), Quaternion.identity);
                //As I find all the cells in the hierarchy chaotic add them to child object named Maze.
                grid[w, h].transform.parent = gameObject.transform.GetChild(0);
            }
        }
    }
}