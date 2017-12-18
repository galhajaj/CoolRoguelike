using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SL = SingletonLoader
public class SL : MonoBehaviour
{
    private static SL _instance = null;

    [SerializeField]
    private GameObject _dataManagerPrefab = null;
    private static GameObject _dataManagerInstance = null;
    public static GameObject DataManager { get { return _dataManagerInstance; } }

    [SerializeField]
    private GameObject _partyPrefab = null;
    private static GameObject _partyInstance = null;
    public static GameObject Party { get { return _partyInstance; } }

    [SerializeField]
    private GameObject _inventoryPrefab = null;
    private static GameObject _inventoryInstance = null;
    public static GameObject Inventory { get { return _inventoryInstance; } }

    [SerializeField]
    private GameObject _dungeonPrefab = null;
    private static GameObject _dungeonInstance = null;
    public static GameObject Dungeon { get { return _dungeonInstance; } }

    void Awake()
    {
        singletonSetup();
        createSingletons();
    }

    private void createSingletons()
    {
        CreateSingleton(_dataManagerInstance,   _dataManagerPrefab);
        CreateSingleton(_partyInstance,         _partyPrefab);
        CreateSingleton(_inventoryInstance,     _inventoryPrefab);
        CreateSingleton(_dungeonInstance,       _dungeonPrefab);
    }

    private void CreateSingleton(GameObject instance, GameObject prefab)
    {
        instance = Instantiate(prefab);
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
