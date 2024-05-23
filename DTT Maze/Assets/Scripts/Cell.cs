using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This houses all the information on a individual cell.
/// </summary>
[System.Serializable]
public class Cell
{
    public GameObject northWall, eastWall, westWall, southWall;

    [SerializeField] public bool visited;
    [SerializeField] public Vector3 position;
    [SerializeField] public Vector2 gridPos;

    /// <summary>
    /// Marks the cells as visited.
    /// </summary>
    public void Visit()
    {
        visited = true;
    }

    /// <summary>
    /// Set the position of the cell in the grid so it can be called for each cell.
    /// </summary>
    /// <param name="x">X coordinate</param>
    /// <param name="z">Z coordinate</param>
    public void SetGridPosition(int x, int z)
    {
        gridPos = new Vector2(x, z);
    }

    /// <summary>
    /// Clears the wall that is between currentcell and nextcell.
    /// </summary>
    /// <param name="wall">Int to check in the switch case</param>
    public void ClearWall(int wall)
    {
        switch (wall)
        {
            case 1:
                northWall.SetActive(false);
                break;

            case 2:
                eastWall.SetActive(false);
                break;

            case 3:
                westWall.SetActive(false);
                break;

            case 4:
                southWall.SetActive(false);
                break;

            default:
                break;
        }
    }
}
