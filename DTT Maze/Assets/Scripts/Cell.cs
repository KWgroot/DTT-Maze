using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    //The first chosen algorithm to generate a maze is a recursive depth-first search found here: https://en.wikipedia.org/wiki/Maze_generation_algorithm
    //*edit* The algorithm will be Randomized depth-first iterative implementation so a stack will be used to 
    //As they call each 'square' in the maze a cell they will be named the same here.
    //The reason this algorithm is chosen first is because it's both simple and decently fast.

    ///<summary>
    ///This class holds the information pertaining a singular cell.
    ///Including the West, East, North and South wall.
    ///It also holds the information whether this cell is already 'visited' or included in the current maze.
    ///Finally this class houses functions to remove it's own walls and switch itself between visited or not.
    ///</summary>

    [SerializeField]
    private GameObject westWall, eastWall, northWall, southWall;

    public bool Visited { get; private set; }

    public void Visit()
    {
        Visited = true;
    }

    public void ClearWall(GameObject wall)
    {
        wall.SetActive(false);
    }
}
