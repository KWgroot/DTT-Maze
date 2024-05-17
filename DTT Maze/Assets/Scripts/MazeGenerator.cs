using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    ///<summary>
    ///A new approach where walls are placed first so there won't be overlapping walls,
    ///as performance is a huge problem when making 250x250 mazes.
    ///Then by looking at wall positions order the space between them into a collection of cells.
    ///Using those cells run the randomized depth first search algorithm as we can now use recursion.
    ///</summary>

    [SerializeField] private GameObject wallPrefab;
    [SerializeField] [Range(10f, 250f)] private int mazeHeight, mazeWidth;
    [SerializeField] private CellFinder cellFinder;

    private Transform mazeHolder;
    private GameObject wallsParent;
    private Vector3 startPos, currentPos;
    private Camera mainCam;

    const float WALLLENGTH = 1f;

    private void Start()
    {
        //Grab the gameobject on which all the maze parts will be placed
        mazeHolder = transform.GetChild(0);

        //Fix the camera position
        mainCam = Camera.main;
        if (Mathf.Max(mazeHeight, mazeWidth) == mazeHeight)
            mainCam.orthographicSize = mazeHeight / 1.9f;
        else
            mainCam.orthographicSize = mazeWidth / 3.5f;

        CreateWalls();
    }

    public void CreateWalls()
    {
        //First we will need a starting position from where we start placing the walls
        startPos = new Vector3((-mazeWidth / 2f) + WALLLENGTH / 2f, 0f, (-mazeHeight / 2f) + WALLLENGTH / 2f);
        currentPos = startPos;

        //Create walls along the height of the maze
        for (int w = 0; w < mazeHeight; w++)
        {
            for (int h = 0; h <= mazeWidth; h++)
            {
                //The current posistion of where the wall is placed is found by starting at the beginning and simply placing
                //walls of the exact size of 1 until the loop reaches the set max size for the height.
                currentPos = new Vector3(startPos.x + (h * WALLLENGTH) - WALLLENGTH / 2f, 0f, startPos.z + (w * WALLLENGTH) - WALLLENGTH / 2f);
                wallsParent = Instantiate(wallPrefab, currentPos, Quaternion.identity);
                wallsParent.name = "Vertical wall: " + w + ", " + h;
                wallsParent.transform.SetParent(mazeHolder);
            }
        }

        //Now repeat the same for horizontal walls
        for (int w = 0; w <= mazeHeight; w++)
        {
            for (int h = 0; h < mazeWidth; h++)
            {
                currentPos = new Vector3(startPos.x + (h * WALLLENGTH), 0f, startPos.z + (w * WALLLENGTH) - WALLLENGTH);
                //Main difference here is the wall needed to be rotated
                wallsParent = Instantiate(wallPrefab, currentPos, Quaternion.Euler(0, 90, 0));
                wallsParent.name = "Horizontal wall: " + w + ", " + h;
                wallsParent.transform.SetParent(mazeHolder);
            }
        }

        cellFinder.FindCells(wallsParent, mazeHeight * mazeWidth, mazeHeight, mazeWidth);
    }
    
    public void CreateMaze()
    {

    }
}
