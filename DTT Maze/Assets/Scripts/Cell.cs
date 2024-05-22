using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cell
{
    public GameObject northWall, eastWall, westWall, southWall;

    [SerializeField] public bool visited;
    [SerializeField] public Vector3 position;

    public void Visit()
    {
        visited = true;
    }

    public void SetPosition()
    {
        position = (northWall.transform.position + eastWall.transform.position +
            westWall.transform.position + southWall.transform.position) / 4;

    }

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
