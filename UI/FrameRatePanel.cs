using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FrameRatePanel : MonoBehaviour
{
    /// <summary>
    /// 上次更新帧率的时间
    /// </summary>
    private float m_lastUpdateShowTime=0f;
    /// <summary>
    /// 更新显示帧率的时间间隔
    /// </summary>
    private readonly float m_updateTime=0.2f;
    /// <summary>
    /// 帧数
    /// </summary>
    private int m_frames = 0;
    /// <summary>
    /// 帧间间隔
    /// </summary>
    private float m_frameDeltaTime = 0f;
    private float m_FPS = 0;
    private Text fpsText;
    private Text frameIntervalText;
    private void Awake()
    {
        fpsText = transform.Find("FPSTitle/Text").GetComponent<Text>();
        frameIntervalText = transform.Find("Inter-frame interval title/Text").GetComponent<Text>();
    }
    // Start is called before the first frame update
    void Start()
    {
        m_lastUpdateShowTime = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update()
    {
        ++m_frames;
        float timespan = Time.realtimeSinceStartup - m_lastUpdateShowTime;
        if (timespan>m_updateTime)
        {
            m_FPS = m_frames / timespan;
            m_frameDeltaTime = timespan*1000 / m_frames;
            m_frames = 0;
            m_lastUpdateShowTime = Time.realtimeSinceStartup;
        }
        fpsText.text = m_FPS.ToString("f0");
        frameIntervalText.text=m_frameDeltaTime.ToString("f0")+"ms";
    }
}
