using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAndWoundSound : MonoBehaviour
{
    public AudioClip FirstBlood;
    public AudioClip DoubleKill;
    public AudioClip TripleKill;
    public AudioClip QuadraKill;
    public AudioClip PentaKill;
    private AudioSource audioSource;
    /// <summary>
    /// 一杀计数器
    /// </summary>
    private int FirstKillCount=0;
    private int DoubleKillCount=0;
    private int TripleKillCount=0;
    private int QuadraKillCount=0;
    private int PentaKillCount=0;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
      
        switch (EnemyLogic.instance.DeathTriggerCount)
        {
            case 1:
                audioSource.clip = FirstBlood;
                ++FirstKillCount;
                if (FirstKillCount == 1)
                {
                    Invoke("PlaySound", 0.5f);
                }
                break;
            case 2:
                audioSource.clip = DoubleKill;
                ++DoubleKillCount;
                if (DoubleKillCount == 1)
                {
                    Invoke("PlaySound", 0.5f);
                }
                break;
            case 3:
                audioSource.clip = TripleKill;
                ++TripleKillCount;
                if (TripleKillCount == 1)
                {
                    Invoke("PlaySound", 0.5f);
                }
                break;
            case 4:
                audioSource.clip = QuadraKill;
                ++QuadraKillCount;
                if (QuadraKillCount == 1)
                {
                    Invoke("PlaySound", 0.5f);
                }
                break;
            case 5:
                audioSource.clip = PentaKill;
                ++ PentaKillCount;
                if (PentaKillCount == 1)
                {
                    Invoke("PlaySound", 0.5f);
                }
                break;
        }
    }
    void PlaySound()
    {
        audioSource.Play();
        
    }
}