using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="FPS/Footstep Audio Data")]
public class FootstepsAudioData : ScriptableObject
{
    public List<FootstepsAudio> FootstepsAduios = new List<FootstepsAudio>();

}
[System.Serializable]
public class FootstepsAudio
{
    public string Tag;
    public List<AudioClip> AudioClips = new List<AudioClip>();
    public float Delay;

}