using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The main generator class that will hold the chosen algorithm but 
/// outsources the finding of cells to other classes.
/// </summary>
public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] [Range(10f, 250f)] private int mazeHeight, mazeWidth;
    [SerializeField] [Range(0f, 3f)] private float generationSpeed = 0f;
    [SerializeField] private CellFinder cellFinder;
    [SerializeField] private FindOpenCell openCellFinder;

    private Stack cellStack = new Stack();
    private List<Cell> unvisitedCells = new List<Cell>();

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
        mainCam.orthographicSize = Mathf.Max((mazeHeight / 1.9f), (mazeWidth / 3.8f));

        CreateWalls();
    }

    /// <summary>
    /// Simply 'creates' walls by placing the wall prefab along the set sizes of the maze.
    /// </summary>
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

        cellFinder.FindCells(mazeHolder, mazeHeight * mazeWidth, mazeHeight, mazeWidth);
    }
    
    /// <summary>
    /// Here the maze algorithm is actually used that carves out a path through the walls
    /// by following it's steps.
    /// </summary>
    /// <param name="currentCell">The current cell from which the maze is made.</param>
    /// <param name="cellGrid">Grid holding all cells in a neatly organized 2d array.</param>
    /// <returns></returns>
    public IEnumerator ApplyAlgorithm(Cell currentCell, Cell[,] cellGrid)
    {
        currentCell.Visit(); // First cell must be visited
        Cell nextCell = new Cell();
        do
        {
            // Grab all the unvisited nearby cells
            unvisitedCells = openCellFinder.GetUnvisitedCell(currentCell, cellGrid, mazeWidth, mazeHeight);

            if (unvisitedCells.Count > 0) // If we can still find unvisited cells nearby, go for one of those
            {
                nextCell = unvisitedCells[Random.Range(0, unvisitedCells.Count)];
            }
            else // If we cant, start going through the stack of previous cells we visited.
            {
                nextCell = null;
            }            

            if (nextCell != null) // We have found a next unvisited cell and will now visit it
            {                
                nextCell.Visit();
                cellStack.Push(currentCell);
                ClearWalls(currentCell, nextCell);
                currentCell = nextCell;
                yield return new WaitForSeconds(generationSpeed);
            }
            else
            {
                currentCell = (Cell)cellStack.Pop();
            }
        } while (cellStack.Count > 0);
    }

    /// <summary>
    /// Simply clears the wall between two cells by checking their grid position.
    /// </summary>
    /// <param name="currentCell">Cell we are moving away from.</param>
    /// <param name="nextCell">Cell we are going to.</param>
    private void ClearWalls(Cell currentCell, Cell nextCell)
    {
        if (currentCell == null)
            return;

        if (currentCell.gridPos.x < nextCell.gridPos.x)
            currentCell.ClearWall(2);

        if (currentCell.gridPos.x > nextCell.gridPos.x)
            currentCell.ClearWall(3);

        if (currentCell.gridPos.y < nextCell.gridPos.y)
            currentCell.ClearWall(1);

        if (currentCell.gridPos.y > nextCell.gridPos.y)
            currentCell.ClearWall(4);
    }
}
