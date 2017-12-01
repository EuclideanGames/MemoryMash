using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GridDefinition", menuName = "Grid Definition")]
public class GridDefinition : ScriptableObject
{
    public List<Vector2> GridPositions;
    public float SquareScale;
}
