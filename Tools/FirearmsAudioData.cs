using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Scripts.Weapon
{
    [CreateAssetMenu(menuName ="FPS/Firearms Audio Data")]
    public class FirearmsAudioData:ScriptableObject
    {
        public AudioClip ShootingAudioClip;
        public AudioClip ReloadLeftAudioClip;
        public AudioClip ReloadOutOfAudioClip;

    }
}
