using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField]
    private int _sizeX = 0;
    [SerializeField]
    private int _sizeY = 0;
    [SerializeField]
    private int _offsetX = 0;
    [SerializeField]
    private int _offsetY = 0;
    [SerializeField]
    private int _spacing = 0;
    [SerializeField]
    private bool _invertYAxis = false;

    [SerializeField]
    public Transform _elementPrefab = null;

    private List<List<GridElement>> _elements = new List<List<GridElement>>();

    public List<GridElement> Elements { get { return getElementList(); } }

    void Awake()
    {
        
    }

    void Start ()
    {
        GenerateTiles(_sizeX, _sizeY);
    }
	
	void Update ()
    {
		
	}

    public void GenerateTiles(int sizeX, int sizeY = 1)
    {
        // make sure all elements deleted
        deleteAllElements();

        _sizeX = sizeX;
        _sizeY = sizeY;

        float boardOriginX = this.transform.position.x;
        float boardOriginY = this.transform.position.y;
        float elementWidth = _elementPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        float elementHeight = _elementPrefab.GetComponent<SpriteRenderer>().bounds.size.y;
        int inv = _invertYAxis ? -1 : 1; // inverted y axis

        for (int x = 0; x < _sizeX; x++)
        {
            _elements.Add(new List<GridElement>());

            for (int y = 0; y < _sizeY; y++)
            {
                Vector3 tilePosition = new Vector3(
                    boardOriginX + _offsetX + (elementWidth / 2) + (x * elementWidth) + (_spacing * x),
                    boardOriginY + _offsetY - (elementHeight / 2) - inv * (y * elementHeight) + (_spacing * y),
                    0);
                Transform element = Instantiate(_elementPrefab, tilePosition, Quaternion.identity);
                element.name = "Element" + x + "_" + y;
                element.parent = this.transform;

                GridElement elementScript = element.GetComponent<GridElement>();
                elementScript.PosX = x;
                elementScript.PosY = y;

                _elements[x].Add(elementScript);
            }
        }
    }

    private List<GridElement> getElementList()
    {
        List<GridElement> result = new List<GridElement>();

        foreach (var elementList in _elements)
            foreach (var element in elementList)
                result.Add(element);

        return result;
    }

    public GridElement GetElement(int x, int y = 0)
    {
        return _elements[x][y];
    }

    public GridElement GetElement(Position pos)
    {
        return GetElement(pos.X, pos.Y);
    }

    private void deleteAllElements()
    {
        foreach (List<GridElement> list in _elements)
        {
            foreach (GridElement element in list)
                Destroy(element.gameObject);
            list.Clear();
        }
        _elements.Clear();
    }
}
