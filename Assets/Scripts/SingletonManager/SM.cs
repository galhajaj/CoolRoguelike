using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SM = Singleton Manager
public class SM : MonoBehaviour
{
    private static SM _instance = null;

    // ================================================= //
    // instantiated
    // ================================================= //
    [SerializeField]
    private GameObject _gameManagerPrefab = null;
    public static GameManager GameManager;

    [SerializeField]
    private GameObject _dataManagerPrefab = null;
    public static DataManager DataManager;

    [SerializeField]
    private GameObject _partyPrefab = null;
    public static Party Party;

    [SerializeField]
    private GameObject _windowManagerPrefab = null;
    public static WindowManager WindowManager;
    // ================================================= //
    // managers that exist, like windows
    // ================================================= //
    public static Dungeon Dungeon;
    public static Inventory Inventory;

    void Awake()
    {
        singletonSetup();
        createSingletons();
    }

    private void createSingletons()
    {
        AddSingleton("Dungeon",     out Dungeon);
        AddSingleton("Inventory",   out Inventory);
        CreateSingleton(_dataManagerPrefab,     out DataManager);
        CreateSingleton(_partyPrefab,           out Party);
        CreateSingleton(_windowManagerPrefab,   out WindowManager);
        CreateSingleton(_gameManagerPrefab,     out GameManager);
    }

    private void AddSingleton<T>(string name, out T script)
    {
        GameObject obj = GameObject.Find(name);
        script = obj.GetComponent<T>();
    }

    private void CreateSingleton<T>(GameObject prefab, out T script)
    {
        GameObject instance = Instantiate(prefab);
        script = instance.GetComponent<T>();
        instance.transform.parent = this.transform;
    }

    private void singletonSetup()
    {
        if (_instance == null)
            _instance = this;

        else if (_instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(this);
    }
}
