﻿using System.Collections;
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
    private int _sideBarWordWrapWidth = 25;

    void Start ()
    {
        SetSideBarText("sdfsdf sdfsfdsfsdf <color=green>green</color> sdf <color=blue>sdfsdf</color> dsf sdf dsf df dsf sdfsdf dsfdfd fd df dfdfsdfsfdf dfdfdfdfdf dfdfdf dfd df df dfdfd df dfdfdf dfdfdfd dfdf df d");
    }
	
	void Update ()
    {
        
    }

    public void SetMainCaption(string text)
    {
        _mainCaption.text = text;
    }

    public void SetDescriptionLine(string text)
    {
        _descriptionLine.text = text;
    }

    public void SetSideBarText(string text)
    {
        text = WrapText(text, _sideBarWordWrapWidth);
        _sideBarText.text = text;
        //FitToWidth(_sideBarWidth, _sideBarText);
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
    // Wrap text by line height
    private string WrapText(string input, int lineLength)
    {

        // Split string by char " "         
        string[] words = input.Split(" "[0]);

        // Prepare result
        string result = "";

        // Temp line string
        string line = "";

        // for each all words        
        foreach (string s in words)
        {
            // Append current word into line
            string temp = line + " " + s;

            // If line length is bigger than lineLength
            if (temp.Length > lineLength)
            {

                // Append current line into result
                result += line + "\n";
                // Remain word append into new line
                line = s;
            }
            // Append current word into current line
            else
            {
                line = temp;
            }
        }

        // Append last line into result        
        result += line;

        // Remove first " " char
        return result.Substring(1, result.Length - 1);
    }
    // ########################################################################################
    // great function - need to use it!
    int CalculateLengthOfMessage(string message, TextMesh textMesh)
    {
        int totalLength = 0;

        Font myFont = textMesh.font;  //chatText is my Text component
        CharacterInfo characterInfo = new CharacterInfo();

        char[] arr = message.ToCharArray();

        foreach (char c in arr)
        {
            myFont.GetCharacterInfo(c, out characterInfo, textMesh.fontSize);

            totalLength += characterInfo.advance;
        }

        return totalLength;
    }
    // ########################################################################################
}