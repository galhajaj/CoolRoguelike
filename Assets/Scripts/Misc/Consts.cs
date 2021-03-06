﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Consts
{
    public static readonly int MAX_ACTION_UNITS = 20;
    public static readonly int MAX_POCKETS_NUMBER = 10;
    public static readonly int LEVELS_PER_SKILL_TITLE = 10; // Beginner, Intermediate, Expert, Master, Grandmaster

    public static readonly string CURRENT_PLAYER = "Player1"; // temp...hard coded in the meantime. get it from text somehow
    public static readonly string SAVE_FILES_PATH = @"c:/temp/Saves"; // change location and name to be for the current player
    public static readonly string DUNGEON_FILES_PATH = @"c:/temp/Dungeons"; // change location and name to be for the current player

    public static readonly string FIRST_DEFAULT_DUNGEON_NAME = "a";

    // windows names
    public struct WindowNames
    {
        public static readonly string MAIN_MENU         = "MainMenu";
        public static readonly string VILLAGE           = "Village";
        public static readonly string DUNGEON           = "Dungeon";
        public static readonly string INVENTORY         = "Inventory";
        public static readonly string BAG               = "Bag";
        public static readonly string LIBRARY           = "Library";
        public static readonly string OPEN_BOOK         = "OpenBook";
        public static readonly string CHARACTER_SHEET   = "CharacterSheet";
        public static readonly string UNIVERSITY        = "University";
    }
}
