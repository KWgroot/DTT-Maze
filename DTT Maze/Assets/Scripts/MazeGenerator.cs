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

    private List<GameObject> mazeObjects = new List<GameObject>();

    //250x250 is insanely huge, doesn't seem right?
    //Any setting where width or height exceeds 100 have no animations, only finished maze.

    private void Start()
    {
        //We already know the width and height, adjust camera to the middle position.
        Camera cam = Camera.main;
        cam.transform.position = new Vector3(width/2f - 0.5f, 2, height /2f - 0.5f); //The actual height is not important as 
        //And adjust the size of the view.                                           //the cam is set to ortographic.
        cam.orthographicSize = Mathf.Max(width / 3.8f, height / 1.9f);

        //Fill the grid so it can be used
        grid = new Cell[width, height];

        //Use nested for-loops to place the maze shape and place it in the grid
        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                grid[w, h] = Instantiate(cellPrefab, new Vector3(w, 0, h), cellPrefab.transform.rotation);
                //As I find all the cells in the hierarchy chaotic add them to child object named Maze.
                grid[w, h].transform.parent = gameObject.transform.GetChild(0);

                //Either 3d or 2d maze objects are now batched through this collection.
                foreach(Transform child in grid[w, h].transform)
                    mazeObjects.Add(child.gameObject);
            }
        }
        //Batch static objects at runtime.
        StaticBatchingUtility.Combine(mazeObjects.ToArray(), transform.GetChild(0).gameObject);


        //For optimization reasons this will be done at the end.
        foreach(Cell cell in grid)
        {
            Destroy(cell.GetComponent<Cell>());
        }
    }
}
