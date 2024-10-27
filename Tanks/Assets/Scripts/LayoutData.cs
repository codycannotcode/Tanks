using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LayoutData", menuName = "LayoutData", order = 1)]
public class LayoutData : ScriptableObject
{
    public List<GameObject> size1x1;
    public List<GameObject> size1x2;
    public List<GameObject> size2x1;
    public List<GameObject> size2x2;
}
