using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Algorithms : MonoBehaviour
{
    private Stack cellStack = new Stack();
    private List<Cell> unvisitedCells = new List<Cell>();
    private List<Cell> walkedCells = new List<Cell>();
    private List<Cell> mazeCells = new List<Cell>();

    private Cell startCell = new Cell(), endCell = new Cell(), currentCell = new Cell(), nextCell = new Cell();

    public GameObject cube;

    /// <summary>
    /// Here the maze algorithm is actually used that carves out a path through the walls
    /// by following it's steps.
    /// </summary>
    /// <param name="currentCell">The current cell from which the maze is made.</param>
    /// <param name="cellGrid">Grid holding all cells in a neatly organized 2d array.</param>
    /// <returns></returns>
    public void RandomDepthFirst(Cell currentCell, Cell[,] cellGrid, int mazeWidth, int mazeHeight, FindOpenCell openCellFinder)
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
            }
            else if (cellStack.Count > 0)
            {
                currentCell = (Cell)cellStack.Pop();
            }

        } while (cellStack.Count > 0);
    }

    public IEnumerator RandomDepthFirstCoroutine(Cell currentCell, Cell[,] cellGrid, int mazeWidth, int mazeHeight, FindOpenCell openCellFinder, float generationSpeed)
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
            else if (cellStack.Count > 0)
            {
                currentCell = (Cell)cellStack.Pop();
            }

        } while (cellStack.Count > 0);
    }

    public void WilsonWalk(Cell[,] cellGrid, int mazeWidth, int mazeHeight, FindOpenCell openCellFinder)
    {
        do
        {
            startCell = cellGrid[Random.Range(0, mazeWidth), Random.Range(0, mazeHeight)];
            endCell = cellGrid[Random.Range(0, mazeWidth), Random.Range(0, mazeHeight)];

            while (startCell.visited || mazeCells.Contains(startCell))
                startCell = cellGrid[Random.Range(0, mazeWidth), Random.Range(0, mazeHeight)];

            while (endCell.visited || mazeCells.Contains(endCell))
                endCell = cellGrid[Random.Range(0, mazeWidth), Random.Range(0, mazeHeight)];

            walkedCells.Add(startCell);
            currentCell = startCell;

            do
            {
                unvisitedCells = openCellFinder.GetRandomCell(currentCell, cellGrid, mazeWidth, mazeHeight);
                nextCell = unvisitedCells[Random.Range(0, unvisitedCells.Count)];

                if (walkedCells.Contains(nextCell))
                {
                    walkedCells = walkedCells.Take(walkedCells.IndexOf(nextCell) + 1).ToList();
                    currentCell = walkedCells.Last();
                }
                else if (mazeCells.Contains(nextCell))
                {
                    ClearWalls(currentCell, nextCell);
                    endCell = nextCell;
                }
                else
                {
                    walkedCells.Add(nextCell);
                    currentCell = nextCell;
                }

            } while (nextCell != endCell);

            foreach (Cell walkedCell in walkedCells)
                walkedCell.Visit();

            for (int i = 0; i < walkedCells.Count - 1; i++)
            {
                ClearWalls(walkedCells[i], walkedCells[i + 1]);
            }

            mazeCells.AddRange(walkedCells);
            walkedCells.Clear();

        } while (mazeCells.Count < mazeWidth * mazeHeight);
    }

    public IEnumerator WilsonWalkCoroutine(Cell[,] cellGrid, int mazeWidth, int mazeHeight, FindOpenCell openCellFinder, float generationSpeed)
    {
        do
        {
            startCell = cellGrid[Random.Range(0, mazeWidth), Random.Range(0, mazeHeight)];
            endCell = cellGrid[Random.Range(0, mazeWidth), Random.Range(0, mazeHeight)];

            while (startCell.visited || mazeCells.Contains(startCell))
                startCell = cellGrid[Random.Range(0, mazeWidth), Random.Range(0, mazeHeight)];

            while (endCell.visited || mazeCells.Contains(endCell))
                endCell = cellGrid[Random.Range(0, mazeWidth), Random.Range(0, mazeHeight)];

            walkedCells.Add(startCell);
            currentCell = startCell;

            do
            {
                unvisitedCells = openCellFinder.GetRandomCell(currentCell, cellGrid, mazeWidth, mazeHeight);
                nextCell = unvisitedCells[Random.Range(0, unvisitedCells.Count)];

                if (walkedCells.Contains(nextCell))
                {
                    walkedCells = walkedCells.Take(walkedCells.IndexOf(nextCell) + 1).ToList();
                    currentCell = walkedCells.Last();
                }
                else if (mazeCells.Contains(nextCell))
                {
                    ClearWalls(currentCell, nextCell);
                    endCell = nextCell;
                }
                else
                {
                    walkedCells.Add(nextCell);
                    currentCell = nextCell;
                }

            } while (nextCell != endCell);

            foreach (Cell walkedCell in walkedCells)
                walkedCell.Visit();

            for (int i = 0; i < walkedCells.Count - 1; i++)
            {
                ClearWalls(walkedCells[i], walkedCells[i + 1]);
                yield return new WaitForSeconds(generationSpeed);
            }

            mazeCells.AddRange(walkedCells);
            walkedCells.Clear();

        } while (mazeCells.Count < mazeWidth * mazeHeight);
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
