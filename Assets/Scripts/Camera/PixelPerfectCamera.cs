using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelPerfectCamera : MonoBehaviour
{

    void Awake()
    {
        this.transform.position = new Vector3(this.transform.position.x - 0.1F, this.transform.position.y - 0.1F, this.transform.position.z);
    }

	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}
}
