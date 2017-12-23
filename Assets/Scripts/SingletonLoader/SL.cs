using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SL = SingletonLoader
public class SL : MonoBehaviour
{
    private static SL _instance = null;

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
    private GameObject _inventoryPrefab = null;
    public static Inventory Inventory;

    [SerializeField]
    private GameObject _dungeonPrefab = null;
    public static Dungeon Dungeon;

    [SerializeField]
    private GameObject _villagePrefab = null;
    public static Village Village;

    [SerializeField]
    private GameObject _windowManagerPrefab = null;
    public static WindowManager WindowManager;

    void Awake()
    {
        singletonSetup();
        createSingletons();
    }

    private void createSingletons()
    {
        CreateSingleton(_dataManagerPrefab,     out DataManager);
        CreateSingleton(_partyPrefab,           out Party);
        CreateSingleton(_inventoryPrefab,       out Inventory);
        CreateSingleton(_dungeonPrefab,         out Dungeon);
        CreateSingleton(_villagePrefab,         out Village);
        CreateSingleton(_windowManagerPrefab,   out WindowManager);
        CreateSingleton(_gameManagerPrefab,     out GameManager);
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
