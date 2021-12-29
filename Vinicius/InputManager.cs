using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class InputManager
{

    public static Dictionary<string, KeyCode> keyMapping;
    public static string[] keyMaps = new string[]
    {
        "Forward",
        "Backward",
        "Left",
        "Right",
        "Interact",
        "MagnetUp",
        "MagnetDown",
        "MagnetLeft",
        "MagnetRight",
        "Activate",
        "Swap Character",
        "Carry",
        "Repair/Heal",
        "Pause",
        "Dialogue",
        "Dash",
        "Jump"
    };

    public static KeyCode[] defaults = new KeyCode[]
    {
        KeyCode.W,
        KeyCode.S,
        KeyCode.A,
        KeyCode.D,
        KeyCode.E,
        KeyCode.W,
        KeyCode.S,
        KeyCode.A,
        KeyCode.D,
        KeyCode.F,
        KeyCode.R,
        KeyCode.LeftShift,
        KeyCode.Q,
        KeyCode.Escape,
        KeyCode.Space,
        KeyCode.Space,
        KeyCode.Space
    };

    static InputManager()
    {
        InitializeDictionary();
    }

    private static void InitializeDictionary()
    {
        keyMapping = new Dictionary<string, KeyCode>();
        for (int i = 0; i < keyMaps.Length; ++i)
        {
            keyMapping.Add(keyMaps[i], defaults[i]);
        }
    }

    public static void SetKeyMap(string keyMap, KeyCode key)
    {
        if (!keyMapping.ContainsKey(keyMap))
            throw new ArgumentException("Invalid KeyMap in SetKeyMap: " + keyMap);
        keyMapping[keyMap] = key;
    }

    public static bool GetKeyDown(string keyMap)
    {
        return Input.GetKeyDown(keyMapping[keyMap]);
    }

    public static bool GetKey(string keyMap)
    {
        return Input.GetKey(keyMapping[keyMap]);
    }

    public static bool GetKeyUp(string keyMap)
    {
        return Input.GetKeyUp(keyMapping[keyMap]);
    }
}
