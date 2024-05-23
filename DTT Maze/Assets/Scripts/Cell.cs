using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cell
{
    public GameObject northWall, eastWall, westWall, southWall;

    [SerializeField] public bool visited;
    [SerializeField] public Vector3 position;
    [SerializeField] public Vector2 gridPos;

    public void Visit()
    {
        visited = true;
    }

    public void SetPosition()
    {
        position = (northWall.transform.position + eastWall.transform.position +
            westWall.transform.position + southWall.transform.position) / 4;
    }

    public void SetGridPosition(int x, int z)
    {
        gridPos = new Vector2(x, z);
    }

    public void ClearWall(int wall)
    {
        switch (wall)
        {
            case 1:
                Debug.Log("North");
                northWall.SetActive(false);
                break;

            case 2:
                Debug.Log("East");
                eastWall.SetActive(false);
                break;

            case 3:
                Debug.Log("West");
                westWall.SetActive(false);
                break;

            case 4:
                Debug.Log("South");
                southWall.SetActive(false);
                break;

            default:
                break;
        }
    }
}
