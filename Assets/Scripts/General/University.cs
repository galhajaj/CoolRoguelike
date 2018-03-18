using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class University : MonoBehaviour
{
    [SerializeField]
    private GenericGrid _grid = null;

    public List<string> Skills = new List<string>();

	void Start ()
    {
        WindowManager.Instance.Event_WindowLoaded += universityWindowLoaded;
    }
	
	void Update ()
    {
        
	}

    private void universityWindowLoaded()
    {
        if (!WindowManager.Instance.IsCurrentWindow(Consts.WindowNames.UNIVERSITY))
            return;

        _grid.Rebuild(Skills.Count);

        for (int i = 0; i < Skills.Count; ++i)
        {
            Sprite skillSprite = Resources.Load<Sprite>("Skills/" + Skills[i]);
            _grid.GetElement(i).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = skillSprite;
        }
    }
}
