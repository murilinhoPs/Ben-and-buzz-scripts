using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadScene(string cena)
    {
        SceneManager.LoadScene(cena);

        // if (cena == "J2 - Cenário")
        // {
        //     SwapPlayerCharacter.Instance.isHuman = true;

        //     print("Humano");
        // }


    }


    public void enable(GameObject obj)
    {
        obj.SetActive(true);
    }

    public void disable(GameObject obj)
    {
        obj.SetActive(false);
    }
    
    public void respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
