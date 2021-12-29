using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutscenesCameras : MonoBehaviour
{
    [SerializeField] private GameObject[] cinemaCameras;

    [HideInInspector] public bool canMoveCamera = true;

    private GameObject currentCamera;

    private static CutscenesCameras _instance;
    public static CutscenesCameras Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<CutscenesCameras>();

                if (_instance == null)
                {
                    GameObject container = new GameObject("CutscenesCameras");
                    _instance = container.AddComponent<CutscenesCameras>();
                }
            }

            return _instance;
        }
    }

    public IEnumerator activeOne(string camName, float time, string nextScene = "")
    {
        foreach (var camera in cinemaCameras)
        {
            if (camera.name == camName)
            {
                currentCamera = camera;

                currentCamera.SetActive(true);
                break;
            }
        }
        canMoveCamera = false;

        yield return new WaitForSeconds(time);

        if (nextScene != "")
        {
            SceneManager.LoadScene(nextScene);

            yield return null;
        }
        else if (nextScene == "")
        {
            if (FindObjectOfType<DialogueManager>().sentences.ToArray().Length == 0)
                yield return null;
            else yield return new WaitUntil(() => InputManager.GetKeyDown("Dialogue"));
        }

        currentCamera.SetActive(false);

        canMoveCamera = true;
    }
}
