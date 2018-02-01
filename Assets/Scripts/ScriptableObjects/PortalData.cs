using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    class PortalData
    {
    }
}

[CreateAssetMenu(fileName = "PortalData", menuName = "Data/Portal", order = 4)]
public class PortalData : ScriptableObject
{
    public Sprite Icon;
    public Sprite Image;
}