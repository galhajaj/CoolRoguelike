using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDisplayer : Singleton<TextDisplayer>
{
    [SerializeField]
    private TextMesh _mainCaption = null;
    [SerializeField]
    private TextMesh _descriptionLine = null;
    [SerializeField]
    private TextMesh _sideBarText = null;

    [SerializeField]
    private float _sideBarWidth = 150;

    void Start ()
    {

	}
	
	void Update ()
    {
        _mainCaption.text = WindowManager.Instance.CurrentWindowName;
    }

    public void SetDescriptionLine(string text)
    {
        _descriptionLine.text = text;
    }

    public void SetSideBarText(string text)
    {
        _sideBarText.text = text;
        FitToWidth(_sideBarWidth, _sideBarText);
    }

    // ########################################################################################
    // helper functions for text mesg word wrap
    // ########################################################################################
    public void FitToWidth(float wantedWidth, TextMesh textMesh)
    {

        //if (width <= wantedWidth) return;

        string oldText = textMesh.text;
        textMesh.text = "";

        string[] lines = oldText.Split('\n');

        foreach (string line in lines)
        {
            textMesh.text += wrapLine(line, wantedWidth, textMesh);
            textMesh.text += "\n";
        }
    }
    // ########################################################################################
    private string wrapLine(string s, float w, TextMesh textMesh)
    {
        Dictionary<char, float> dict = new Dictionary<char, float>();

        // need to check if smaller than maximum character length, really...
        if (w == 0 || s.Length <= 0) return s;

        char c;
        char[] charList = s.ToCharArray();

        float charWidth = 0;
        float wordWidth = 0;
        float currentWidth = 0;

        string word = "";
        string newText = "";
        string oldText = textMesh.text;

        for (int i = 0; i < charList.Length; i++)
        {
            c = charList[i];

            if (dict.ContainsKey(c))
            {
                charWidth = (float)dict[c];
            }
            else
            {
                textMesh.text = "" + c;
                charWidth = 5;//GetComponent<Renderer>().bounds.size.x;
                dict.Add(c, charWidth);
                //here check if max char length
            }

            if (c == ' ' || i == charList.Length - 1)
            {
                if (c != ' ')
                {
                    word += c.ToString();
                    wordWidth += charWidth;
                }

                if (currentWidth + wordWidth < w)
                {
                    currentWidth += wordWidth;
                    newText += word;
                }
                else
                {
                    currentWidth = wordWidth;
                    newText += word.Replace(" ", "\n");
                }

                word = "";
                wordWidth = 0;
            }

            word += c.ToString();
            wordWidth += charWidth;
        }

        textMesh.text = oldText;
        return newText;
    }
    // ########################################################################################
    // ########################################################################################
    // ########################################################################################
}
