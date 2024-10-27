using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TankData", menuName = "TankData", order = 1)]
public class TankData : ScriptableObject
{
    public GameObject playerTank;
    public List<GameObject> weakTanks;
    public List<GameObject> regularTanks;
    public List<GameObject> strongTanks;
}
