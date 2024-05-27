using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// The main generator class that will hold the chosen algorithm but 
/// outsources the finding of cells to other classes.
/// </summary>
public class MazeGenerator : MonoBehaviour
{
    [SerializeField] Algorithms algorithms;

    [SerializeField] private GameObject wallPrefab;
    [SerializeField] [Range(10f, 250f)] private int mazeHeight, mazeWidth;
    [Range(0f, 0.25f)] public float generationSpeed = 0f;
    [SerializeField] private CellFinder cellFinder;
    [SerializeField] private FindOpenCell openCellFinder;
    [SerializeField] private Slider heightSlider, widthSlider, speedSlider, algorithmSlider;
    [SerializeField] private Button generateButton, reloadButton;
    [SerializeField] private TextMeshProUGUI heightText, widthText, speedText, algorithmText;
    
    [SerializeField] private Gradient gradient = new Gradient();
    [SerializeField] private Material wallMat;

    private Transform mazeHolder;
    private List<GameObject> wallsToBatch = new List<GameObject>();
    private GameObject wallsParent;
    private Vector3 startPos, currentPos;
    private Camera mainCam;

    private int chosenAlgorithm = 1;

    const float WALLLENGTH = 1f;

    private void Start()
    {
        //Grab the gameobject on which all the maze parts will be placed
        mazeHolder = transform.GetChild(0);

        reloadButton.interactable = false;
    }

    public void SetAlgorithm()
    {
        chosenAlgorithm = (int)algorithmSlider.value;
        switch (chosenAlgorithm)
        {
            case 1:
                algorithmText.text = "Depth-First";
                break;

            case 2:
                algorithmText.text = "Wilsons";
                    break;
        }
    }

    public void SetMazeHeight()
    {
        mazeHeight = (int)heightSlider.value;
        heightText.text = mazeHeight.ToString();
    }

    public void SetMazeWidth()
    {
        mazeWidth = (int)widthSlider.value;
        widthText.text = mazeWidth.ToString();
    }

    public void SetSpeed()
    {
        generationSpeed = speedSlider.value;
        speedText.text = (0.25f - generationSpeed).ToString("0.00");
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Simply 'creates' walls by placing the wall prefab along the set sizes of the maze.
    /// </summary>
    public void CreateWalls()
    {
        heightSlider.interactable = false;
        widthSlider.interactable = false;
        generateButton.interactable = false;
        speedSlider.interactable = false;
        algorithmSlider.interactable = false;
        reloadButton.interactable = true;

        //Fix the camera position
        mainCam = Camera.main;
        mainCam.orthographicSize = Mathf.Max((mazeHeight / 1.9f), (mazeWidth / 3.5f));

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
                wallsToBatch.Add(wallsParent);
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
                wallsToBatch.Add(wallsParent);
                wallsParent.name = "Horizontal wall: " + w + ", " + h;
                wallsParent.transform.SetParent(mazeHolder);
            }
        }

        StaticBatchingUtility.Combine(wallsToBatch.ToArray(), mazeHolder.gameObject);

        wallMat.DOGradientColor(gradient, 20f).SetLoops(-1, LoopType.Restart);

        cellFinder.FindCells(mazeHolder, mazeHeight * mazeWidth, mazeHeight, mazeWidth);
    }
    
    
    public void ApplyAlgorithm(Cell currentCell, Cell[,] cellGrid)
    {
        switch (chosenAlgorithm)
        {
            case 1:
                if (generationSpeed > 0)
                    StartCoroutine(algorithms.RandomDepthFirstCoroutine(currentCell, cellGrid, mazeWidth, mazeHeight, openCellFinder, generationSpeed));
                else
                    algorithms.RandomDepthFirst(currentCell, cellGrid, mazeWidth, mazeHeight, openCellFinder);
                break;

            case 2:
                if (generationSpeed > 0)
                    StartCoroutine(algorithms.WilsonWalkCoroutine(cellGrid, mazeWidth, mazeHeight, openCellFinder, generationSpeed));
                else
                    algorithms.WilsonWalk(cellGrid, mazeWidth, mazeHeight, openCellFinder);
                break;
        }
    }
}
