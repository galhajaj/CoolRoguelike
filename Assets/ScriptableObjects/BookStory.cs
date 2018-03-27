using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Data", menuName = "BookStory", order = 1)]
public class BookStory : ScriptableObject
{
    [Multiline]
    [TextArea]
    public List<string> Story;
}
