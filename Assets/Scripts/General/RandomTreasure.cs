using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTreasure : MonoBehaviour
{
    public List<string> Categories;

    [Range(0,100)]
    public int Quality; // the percentage to add each bonus to item
}
