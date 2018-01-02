using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridElement : MonoBehaviour
{
    public int PosX;
    public int PosY;

    public Position Position { get { return new Position(PosX, PosY); } }
}
