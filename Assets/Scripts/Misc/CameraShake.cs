using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour 
{
    private float _time2Shake = 0.0F;
    private Vector3 _initCameraPosition;

    // ================================================================================== //
	void Start () 
    {
        _initCameraPosition = Camera.main.transform.position;
	}
    // ================================================================================== //
	void Update () 
    {
        if (_time2Shake > 0.0F)
        {
            _time2Shake -= Time.deltaTime;
            float randX = Random.insideUnitCircle.x * 0.1F;
            float randY = Random.insideUnitCircle.y * 0.1F;
            float toX = Camera.main.transform.position.x + randX;
            float toY = Camera.main.transform.position.y + randY;
            float toZ = Camera.main.transform.position.z;
            Camera.main.transform.position = new Vector3(toX, toY, toZ);
        }
        else
        {
            Camera.main.transform.position = _initCameraPosition;
        }
	}
    // ================================================================================== //
    public void Shake(float timeInSec)
    {
        _time2Shake = timeInSec;
    }
    // ================================================================================== //
}
