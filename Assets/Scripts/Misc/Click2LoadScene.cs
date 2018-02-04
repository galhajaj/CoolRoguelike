using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Click2LoadScene : MonoBehaviour
{
    [SerializeField]
    private string _sceneName = "";

    void OnMouseDown()
    {
        SceneManager.LoadScene(_sceneName);
    }
}