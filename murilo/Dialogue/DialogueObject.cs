using UnityEngine;

[System.Serializable]
public struct DialogueObject
{
    public string dialogueName;

    [TextArea(3, 10)]
    public string[] sentences;
}
