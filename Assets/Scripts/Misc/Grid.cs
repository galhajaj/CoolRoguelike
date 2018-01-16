using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField]
    private int _sizeX = 0;
    public int SizeX { get { return _sizeX; } }
    [SerializeField]
    private int _sizeY = 0;
    public int SizeY { get { return _sizeY; } }
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

    private List<GridElement> _elementsList = new List<GridElement>();
    public List<GridElement> Elements { get { return /*getElementList();*/_elementsList; } }

    void Awake()
    {
        
    }

    void Start ()
    {
        generateGrid();
    }
	
	void Update ()
    {
		
	}

    private void generateGrid()
    {
        // make sure all elements deleted
        deleteAllElements();

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

                _elementsList.Add(elementScript);
                elementScript.Index = _elementsList.Count - 1;
            }
        }
    }

    public void Rebuild(int sizeX, int sizeY = 1)
    {
        _sizeX = sizeX;
        _sizeY = sizeY;
        generateGrid();
    }

    /*private List<GridElement> getElementList()
    {
        List<GridElement> result = new List<GridElement>();

        foreach (var elementList in _elements)
            foreach (var element in elementList)
                result.Add(element);

        return result;
    }*/

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
        _elementsList.Clear();
    }

    // overloading
    public void UpdateElementsVisibility(int activeElements)
    {
        UpdateElementsVisibility(activeElements, activeElements);
    }

    // elements that are active will has white sprite renderer - normal
    // above max will be invisible & uncollidable (if has collider)
    // from active to max they will be black (black sprite renderer)
    public void UpdateElementsVisibility(int activeElements, int maxElements)
    {
        for (int i = 0; i < Elements.Count; ++i)
        {
            SpriteRenderer currentSpriteRenderer = Elements[i].GetComponent<SpriteRenderer>();
            Collider2D currentCollider = Elements[i].GetComponent<Collider2D>();

            // visibility of over the max
            if (i >= maxElements)
            {
                currentSpriteRenderer.enabled = false;
                if (currentCollider != null)
                    currentCollider.enabled = false;
                continue;
            }
            else
            {
                currentSpriteRenderer.enabled = true;
                if (currentCollider != null)
                    currentCollider.enabled = true;
            }

            // black over the number
            if (i >= activeElements)
            {
                currentSpriteRenderer.color = Color.black;
                continue;
            }

            // the rest are white
            currentSpriteRenderer.color = Color.white;
        }
    }
}
