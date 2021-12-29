using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class DialogueBehaviour : PlayableBehaviour
{
    public string characterName;
    [TextArea(3, 10)]
    public string dialogueLine;
    public bool hasToPause = true;

    public AnimationManager benAnim;
    public string animToPlay;

    private bool clipPlayed = false;
    private bool pauseScheduled = false;
    private PlayableDirector director;

    public override void OnPlayableCreate(Playable playable)
    {
        director = (playable.GetGraph().GetResolver() as PlayableDirector);
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        TimelineManager.Instance.SetClipState(playable.GetPlayState());

        if (!clipPlayed
            && info.weight > 0f)
        {
            UIManager.Instance.SetDialogueCutscene(characterName, dialogueLine);

            if (animToPlay != "")
            {
                Debug.Log(animToPlay);
                var animSentence = animToPlay.Split(',');

                string animSentenceName = animSentence[0];
                string animSentenceValue = animSentence[1];

                if (animSentenceName != "comunicadorIdle") benAnim.SetBool("comunicadorIdle", false);

                if (animSentenceValue.ToLower() == "trigger")
                    benAnim.Trigger(animSentenceName);
                else
                    benAnim.SetBool(animSentenceName, Convert.ToBoolean(animSentenceValue));
            }

            if (Application.isPlaying)
            {
                if (hasToPause)
                {
                    pauseScheduled = true;
                }
            }

            clipPlayed = true;
        }
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (pauseScheduled)
        {
            pauseScheduled = false;

            TimelineManager.Instance.PauseTimeline(director);
            TimelineManager.Instance.SetClipState(playable.GetPlayState());

            UIManager.Instance.ToggleDialogueContainer(true);
        }
        else
            UIManager.Instance.ToggleDialogueContainer(false);

        clipPlayed = false;
    }
}
