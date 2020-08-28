using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public string Name;
    public int Score;
}

public class PlayerComparer : IComparer<PlayerData>
{
    public int Compare(PlayerData x, PlayerData y)
    {
        if (x.Score > y.Score)
            return -1;
        else if (x.Score < y.Score)
            return 1;
        else
            return 0;
    }
}
