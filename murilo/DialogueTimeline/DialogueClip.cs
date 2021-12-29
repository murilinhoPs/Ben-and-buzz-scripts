using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class DialogueClip : PlayableAsset, ITimelineClipAsset
{
    public ExposedReference<AnimationManager> benAnim;
    public DialogueBehaviour template = new DialogueBehaviour();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        // DialogueBehaviour playableBehaviour = new DialogueBehaviour();

        // playableBehaviour.benAnim = animationManager.Resolve(graph.GetResolver());

        var playable = ScriptPlayable<DialogueBehaviour>.Create(graph, template);

        playable.GetBehaviour().benAnim = benAnim.Resolve(graph.GetResolver());

        return playable;
    }
}
