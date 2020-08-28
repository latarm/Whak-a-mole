using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int xIndex;
    public int yIndex;

    public void Init(int x, int y)
    {
        xIndex = x;
        yIndex = y;
    }
}
