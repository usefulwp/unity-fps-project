using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum EnemyState
{
    /// <summary>
    /// 子弹受伤状态
    /// </summary>
    Bulleted,
    Normal
}
public class EnemyLogic : MonoBehaviour
{
    public static EnemyLogic instance;
    public Animator animator;
    public float CurrentHp;
    public float TotalHp;
    [Tooltip("追击距离")]
    public float PursuitDistance = 10f;
    [Tooltip("攻击距离")]
    public float AttackDistance = 5f;
    [Tooltip("追击速度")]
    public float PursuitSpeed;
    [Tooltip("回巢速度")]
    public float ToOriginalSpeed;
    [Tooltip("攻击间隔时间")]
    public float AttackIntervalTime = 0f;
    public Color OriginalColor { get; set; }
    /// <summary>
    /// 身体上的material
    /// </summary>
    public Material BodyMaterial { get; set; }
    public Image FillImage { get; set; }
    [Tooltip("Attack1攻击力")]
    public int Attack1Value;
    public int Attack2Value;
    public int Attack3Value;

    public EnemyState enemyState=EnemyState.Normal;

    private CharacterController characterController;
    private Transform playerTransform;
    private Vector3 originalPos;
    private Quaternion originalRotation;
    /// <summary>
    /// 初始血量
    /// </summary>
    private float originalHp;
    private float attack1Time;
    private float attack2Time;
    private float attack3Time;
    /// <summary>
    /// 死亡动画时长
    /// </summary>
    private float deathTime;
    private float attackTimer;
    private bool IsFirstAttack=true;
    private string[] attackAnimationStr = new string[3];
    private string currentAttackAnimationStr="Attack1";
    /// <summary>
    /// 死亡触发计数
    /// </summary>
    private static int deathTriggerCount=0;
    public int DeathTriggerCount => deathTriggerCount;
    private PlayerStatus playerStatus;
    /// <summary>
    /// 奖励金币数额
    /// </summary>
    private int rewardNumber;
    /// <summary>
    /// 是否死亡
    /// </summary>
    private bool IsDeath=false;
    private void Awake()
    {
        instance = this;
        BodyMaterial = transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material;
        OriginalColor = BodyMaterial.color;
        originalHp = CurrentHp;
        FillImage = transform.Find("Canvas/HpBackground/HpFillImage").GetComponent<Image>();
        playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();

        InitReWard();
    }
    void InitReWard()//初始化击杀怪物对应的奖励
    {
        switch (name)
        {
            case "BaiXiongJing_04":
                rewardNumber =200;
                break;
            case "YeZhu_02":
                rewardNumber =50;
                break;
            case "MingYao_04":
                rewardNumber =300;
                break;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
  
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        originalPos = transform.position;
        originalRotation = transform.rotation;

        attackAnimationStr[0]="Attack1";
        attackAnimationStr[1]="Attack2";
        attackAnimationStr[2]="Attack3";
        InitAnimationTime();
    }
    void InitAnimationTime()
    {
        AnimationClip[] animationClips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip animationClip in animationClips)
        {
            switch (animationClip.name)
            {
                case "Attack1":
                    attack1Time = animationClip.length;
                    break;
                case "Attack2":
                    attack2Time = animationClip.length;
                    break;
                case "Attack3":
                    attack3Time = animationClip.length;
                    break;
                case "Death":
                    deathTime = animationClip.length;
                    break;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        #region 怪物攻击逻辑及死亡逻辑
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            return;
        }       
        if (CurrentHp <= 0&&IsDeath==false)
        {
            BodyMaterial.color = OriginalColor;
            animator.SetTrigger("Death");
            ++deathTriggerCount;
            Destroy(gameObject, deathTime);
            switch (name)
            {
                case "BaiXiongJing_04":
                    Debug.Log("大白熊被击杀,你获得了"+rewardNumber+"金币");
                    playerStatus.GoldNum += rewardNumber;
                    break;
                case "YeZhu_02":
                    Debug.Log("野猪被击杀,你获得了" + rewardNumber + "金币");
                    playerStatus.GoldNum += rewardNumber;
                    break;
                case "MingYao_04":
                    Debug.Log("女巫被击杀,获得了" + rewardNumber + "金币");
                    playerStatus.GoldNum += rewardNumber;
                    break;
            }
            IsDeath = true;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Death")) return;


        float distance = Vector3.Distance(transform.position, playerTransform.position);
        if (distance > 0 && distance <= AttackDistance)   //攻击范围内 逻辑
        {            
            animator.SetBool("IdleToWalk", false);
           
            if (IsFirstAttack)
            {
                animator.SetTrigger(currentAttackAnimationStr);//currentAttackAnimationStr默认是Attack1
                playerStatus.CurrentHp -= Attack1Value;
                StartCoroutine(DelayPlayerWoundSound());
                IsFirstAttack = false;
                attackTimer -= AttackIntervalTime;
            }
            attackTimer += Time.deltaTime;
            switch (currentAttackAnimationStr)
            {
                case "Attack1":
                    if (attackTimer >= attack1Time)
                    {    
                        attackTimer = 0;
                        attackTimer -= AttackIntervalTime;
                        animator.SetTrigger("Attack1");
                        playerStatus.CurrentHp -= Attack1Value;
                        StartCoroutine(DelayPlayerWoundSound());
                        int index = Random.Range(0, attackAnimationStr.Length);
                        currentAttackAnimationStr = attackAnimationStr[index];
                    }
                    break;
                case "Attack2":
                    if (attackTimer >= attack2Time)
                    {                       
                        attackTimer = 0;
                        attackTimer -= AttackIntervalTime;
                        animator.SetTrigger("Attack2");
                        playerStatus.CurrentHp -= Attack2Value;
                        StartCoroutine(DelayPlayerWoundSound());
                        int index = Random.Range(0, attackAnimationStr.Length);
                        currentAttackAnimationStr = attackAnimationStr[index];
                    }
                    break;
                case "Attack3":
                    if (attackTimer >= attack3Time)
                    {
                        attackTimer = 0;
                        attackTimer -= AttackIntervalTime;
                        animator.SetTrigger("Attack3");
                        playerStatus.CurrentHp -= Attack3Value;
                        StartCoroutine(DelayPlayerWoundSound());
                        int index = Random.Range(0, attackAnimationStr.Length);
                        currentAttackAnimationStr = attackAnimationStr[index];
                    }
                    break;
            }

        }


        if (distance> PursuitDistance) //超出追击范围，回到出生点
        {            
            if (Vector3.Distance(transform.position, originalPos) < 0.01f)
            {
                animator.SetBool("IdleToWalk", false);
                animator.SetBool("WalkToIdle", true);
                transform.rotation = originalRotation;
                CurrentHp = originalHp;
                FillImage.fillAmount = 1f;
            }
            else
            {
                animator.SetBool("WalkToIdle", false);
                animator.SetBool("IdleToWalk", true);
                transform.LookAt(originalPos);
                transform.position = Vector3.MoveTowards(transform.position, originalPos, ToOriginalSpeed * Time.deltaTime);
            }
        }


        if(distance > AttackDistance && distance <= PursuitDistance)//追击
        {
            Vector3 targetPoint = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
            transform.LookAt(targetPoint);
            characterController.Move(transform.forward * PursuitSpeed * Time.deltaTime);
            animator.SetBool("WalkToIdle", false);
            animator.SetBool("IdleToWalk", true);
        }
        #endregion
    }
    IEnumerator DelayPlayerWoundSound()//延时播放受伤音效
    {
        yield return new WaitForSeconds(0.5f);
        playerStatus.WoundAudioSource.Play();
    }
}
