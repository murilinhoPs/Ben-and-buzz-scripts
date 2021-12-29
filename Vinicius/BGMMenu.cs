using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class BGMMenu : MonoBehaviour
{

    private static GameObject instance;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
            instance = gameObject;
        else
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<AudioSource>().volume = OptionsManager.musicVolume;
        if (SceneManager.GetActiveScene().name == "Menu" || SceneManager.GetActiveScene().name == "Opções")
            return;
        Destroy(gameObject);
    }
}
