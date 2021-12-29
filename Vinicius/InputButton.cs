using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputButton : MonoBehaviour
{
    public Text text;
    bool active = false;
    KeyCode thisKeyCode;
    KeyCode tempKeyCode;
    public string keyMap = "Forward";

    // Start is called before the first frame update
    void Start()
    {
        text.text = InputManager.keyMapping[keyMap].ToString();
    }

    private void OnGUI()
    {
        Event e = Event.current;
        if (e.isKey)
        {
            tempKeyCode = e.keyCode;
            if (active)
              ReMap();
        }
    }

    public void ReMap ()
    {
        if (Input.anyKey && !Input.GetKey(KeyCode.Escape) && !Input.GetKey(KeyCode.Backspace) && Input.inputString != "")
        {
            thisKeyCode = tempKeyCode;
            InputManager.SetKeyMap(keyMap, thisKeyCode);
            text.text = InputManager.keyMapping[keyMap].ToString();
            active = false;
        }
    }

    public void Activate ()
    {
        active = !active;
        if (active)
        {
            text.text = "?";
        }
        if (!active)
        {
            text.text = InputManager.keyMapping[keyMap].ToString();
        }
    }
}
