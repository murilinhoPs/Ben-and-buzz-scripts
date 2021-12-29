using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    public GameMode gameMode = GameMode.Gameplay;

    private PlayableDirector activeDirector;

    private PlayState clipState;

    private void Awake()
    {
#if UNITY_EDITOR
        Application.targetFrameRate = 30; //just to keep things "smooth" during presentations
#endif
    }

    private static TimelineManager _instance;
    public static TimelineManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<TimelineManager>();

                if (_instance == null)
                {
                    GameObject container = new GameObject("GameManager");
                    _instance = container.AddComponent<TimelineManager>();
                }
            }

            return _instance;
        }
    }

    private void Update()
    {
        if (gameMode == GameMode.DialogueMoment)
            if (InputManager.GetKeyDown("Dialogue"))
                TimelineManager.Instance.ResumeTimeline();
    }

    public void PauseTimeline(PlayableDirector whichOne)
    {
        activeDirector = whichOne;
        activeDirector.playableGraph.GetRootPlayable(0).SetSpeed(0d);
        gameMode = GameMode.DialogueMoment; //InputManager will be waiting for a spacebar to resume
    }

    public void ResumeTimeline()
    {
        UIManager.Instance.ToggleDialogueContainer(false);
        activeDirector.playableGraph.GetRootPlayable(0).SetSpeed(1d);
        gameMode = GameMode.Gameplay;
    }

    public void SetClipState(PlayState playState)
    {
        clipState = playState;
    }


    public enum GameMode
    {
        Gameplay,
        //Cutscene,
        DialogueMoment, //waiting for input
    }
}
