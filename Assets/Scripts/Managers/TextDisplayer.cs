using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextDisplayer : Singleton<TextDisplayer>
{
    [SerializeField]
    private Text _mainCaption = null;
    [SerializeField]
    private Text _descriptionLine = null;
    [SerializeField]
    private Text _sideBarText = null;
    [SerializeField]
    private Text _loggerText = null;

    [SerializeField]
    private int _loggerLinesNumber = 5;
    private List<string> _loggerLines = new List<string>();
    // ====================================================================================================== //
    void Start ()
    {
        displayLoggerLines();
    }
    // ====================================================================================================== //
    void Update ()
    {
        
    }
    // ====================================================================================================== //
    void LateUpdate()
    {
        _sideBarText.text = "";
        showCreatureDetails();
        showPartyMemberDetails();
        showItemDetails();

        _descriptionLine.text = "";
        showButtonDescription();
    }
    // ====================================================================================================== //
    public void AddToLogger(string message)
    {
        while (_loggerLines.Count >= _loggerLinesNumber)
            _loggerLines.RemoveAt(0);
        _loggerLines.Add(message);
        displayLoggerLines();
    }
    // ====================================================================================================== //
    private void displayLoggerLines()
    {
        _loggerText.text = "";
        foreach (string line in _loggerLines)
            _loggerText.text += line + "\n";
    }
    // ====================================================================================================== //
    public void SetMainCaption(string text)
    {
        _mainCaption.text = text;
    }
    // ====================================================================================================== //
    public void SetDescriptionLine(string text)
    {
        _descriptionLine.text = text;
    }
    // ====================================================================================================== //
    public void SetSideBarText(string text)
    {
        //text = WrapText(text, _sideBarWordWrapWidth);
        _sideBarText.text = text;
        //FitToWidth(_sideBarWidth, _sideBarText);
    }
    // ====================================================================================================== //
    private void showCreatureDetails()
    {
        DungeonTile dungeonTile = Utils.GetObjectUnderCursor<DungeonTile>("DungeonTile");
        if (dungeonTile == null)
            return;

        Creature creature = dungeonTile.GetContainedCreature();
        if (creature == null)
            return;

        _sideBarText.text = "<color=red>" + Utils.GetCleanName(creature.name) + "</color>\n";

        foreach (var obj in creature.Stats)
        {
            _sideBarText.text += obj.Key.ToString() + ": " + obj.Value.ToString() + "\n";
        }
    }
    // ====================================================================================================== //
    private void showPartyMemberDetails()
    {
        Portrait portrait = Utils.GetObjectUnderCursor<Portrait>("Portrait");
        if (portrait == null)
            return;

        _sideBarText.text = "<color=green>" + Utils.GetCleanName(portrait.Creature.name) +  "</color>\n";

        foreach (var obj in portrait.Creature.Stats)
        {
            _sideBarText.text += obj.Key.ToString() + ": " + obj.Value.ToString() + "\n";
        }
    }
    // ====================================================================================================== //
    private void showItemDetails()
    {
        Item item = Utils.GetObjectUnderCursor<Item>("Item");
        if (item == null)
            return;

        if (item.State == ItemState.GROUND)
            return;

        _sideBarText.text = "<color=blue>" + Utils.GetCleanName(item.name) + "</color>\n";

        foreach (var obj in item.Stats)
        {
            _sideBarText.text += obj.Key.ToString() + ": " + obj.Value.ToString() + "\n";
        }
    }
    // ====================================================================================================== //
    private void showButtonDescription()
    {
        GameButton gameButton = Utils.GetObjectUnderCursor<GameButton>("GameButton");
        if (gameButton == null)
            return;

        string shortcutKey = gameButton.ShortcutKey == KeyCode.None ? "" : " [ " + gameButton.ShortcutKey.ToString() + " ]";

        _descriptionLine.text = gameButton.Description + shortcutKey;
    }
    // ====================================================================================================== //
}
