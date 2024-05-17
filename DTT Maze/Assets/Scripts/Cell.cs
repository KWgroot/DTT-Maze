using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cell
{
    public GameObject northWall, eastWall, westWall, southWall;

    public bool visited;
}
