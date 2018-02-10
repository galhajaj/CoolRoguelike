﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Consts
{
    public static readonly int DUNGEON_WIDTH = 40;
    public static readonly int DUNGEON_HEIGHT = 23;

    public static readonly int MAX_ACTION_UNITS = 20;
    public static readonly int MAX_BELT_SLOTS = 6;

    public static readonly string CURRENT_PLAYER = "Player1"; // temp...hard coded in the meantime. get it from text somehow
    public static readonly string SAVE_FILES_PATH = @"c:/temp/Saves"; // change location and name to be for the current player
    public static readonly string DUNGEON_FILES_PATH = @"c:/temp/Dungeons"; // change location and name to be for the current player
}
