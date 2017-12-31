using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonObject : MonoBehaviour
{
    [SerializeField]
    private bool _isBlockPath = false;
    public bool IsBlockPath { get { return _isBlockPath; } }

    [SerializeField]
    private bool _isBlockView = false;
    public bool IsBlockView { get { return _isBlockView; } }
}
