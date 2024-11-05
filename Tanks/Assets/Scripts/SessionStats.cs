using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SessionStats
{
    public static int Level;
    public static Dictionary<GameObject, int> DestroyCount;
    public static int Lives;

    //The following variables are used when a level is replayed, and certain data needs to be remembered
    public static bool Replay;
    public static Dictionary<GameObject, Vector3> OriginalPositions;
    public static Vector3 PlayerPosition;
    public static GameObject Layout;

    static SessionStats() {
        Reset();
    }

    public static void Reset() {
        Level = 30;
        DestroyCount = new Dictionary<GameObject, int>();
        Lives = 3;

        Replay = false;
        OriginalPositions = null;
        Layout = null;
    }
}
