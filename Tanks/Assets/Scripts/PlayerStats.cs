using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats
{
    public static int level;
    public static Dictionary<GameObject, int> destroyCount;
    public static int lives;

    static PlayerStats() {
        Reset();
    }

    public static void Reset() {
        level = 1;
        destroyCount = new Dictionary<GameObject, int>();
        lives = 3;
    }
}
