using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    public Vector2 OriginalPosition { get; private set; }

	void Awake()
    {
        OriginalPosition = this.transform.position;
    }

    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    public void Hide()
    {
        this.transform.position = OriginalPosition;
    }

    public void Show()
    {
        this.transform.position = Vector2.zero;
    }
}
