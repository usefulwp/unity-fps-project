using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="FPS/Music Data")]
public class MusicData : ScriptableObject
{
    public List<Music> Musics;
}
[System.Serializable]
public class Music
{
    public int Index;
    public string Name;
    public AudioClip AudioClip;
}

