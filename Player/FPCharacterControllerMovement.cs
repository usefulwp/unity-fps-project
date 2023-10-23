using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPCharacterControllerMovement : MonoBehaviour
{
    private CharacterController characterController;
    private Transform characterTransform;
    private Vector3 movementDirection;
    private bool isCroushed;//判断蹲下
    private float originalHeight;
    private Animator characterAnimator;
    private float velocity;//动画状态机中的参数
    private bool isThirdView;
    public float SprintingSpeed;//奔跑速度
    public float WalkSpeed;
    public float SprintingSpeedWhenCroush;//下蹲时候的奔跑速度
    public float WalkSpeedWhenCroush;
    public float Gravity=9.8f;
    public float JumpHeight;
    public float CrouchHeight=1.25f;//下蹲
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        characterTransform = transform;
        originalHeight = characterController.height;
       // characterAnimator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float tmp_CurrentSpeed = WalkSpeed;
        if (characterController.isGrounded)
        {
            var tmp_Horizontal = Input.GetAxis("Horizontal");
            var tmp_Vertical = Input.GetAxis("Vertical");
            movementDirection =
                characterTransform.TransformDirection(new Vector3(tmp_Horizontal, 0, tmp_Vertical));
            // characterController.SimpleMove(tmp_MovementDirection* MovementSpeed*Time.deltaTime);
            if (Input.GetButtonDown("Jump"))
            {
                movementDirection.y = JumpHeight; 
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                var tmp_CurrentHeight = isCroushed ?originalHeight : CrouchHeight;
                StartCoroutine(DoCrouch(tmp_CurrentHeight));
                isCroushed = !isCroushed;
            }
            if (Input.GetKeyDown(KeyCode.F))//第三人称
            {
                if (!isThirdView)
                {
                    GameObject.Find("the third person of Camera").GetComponent<Camera>().enabled = true;
                    isThirdView = true;
                }
                else
                {
                    GameObject.Find("the third person of Camera").GetComponent<Camera>().enabled = false;
                    isThirdView = false;
                }
            }
            if (isCroushed)
            {
                tmp_CurrentSpeed = Input.GetKey(KeyCode.LeftShift) ? SprintingSpeedWhenCroush : WalkSpeedWhenCroush;
            }
            else
            {
                tmp_CurrentSpeed = Input.GetKey(KeyCode.LeftShift) ? SprintingSpeed : WalkSpeed;
            }
            var tmp_velocity = characterController.velocity;
            tmp_velocity.y = 0;
            velocity = tmp_velocity.magnitude;
            if (characterAnimator != null) characterAnimator.SetFloat("Velocity", velocity,0.25f,Time.deltaTime);
        }
        movementDirection.y -= Gravity*Time.deltaTime;
        characterController.Move(movementDirection * tmp_CurrentSpeed * Time.deltaTime);
    }

    IEnumerator  DoCrouch(float traget)
    {
        float tmp_CurrentHeight = 0;
        while (Mathf.Abs(characterController.height- traget) >0.1f)
        {
            yield return null;
            characterController.height =
                Mathf.SmoothDamp(characterController.height, traget, ref tmp_CurrentHeight, Time.deltaTime * 5);
        }
    }
    internal void SetupAnimator(Animator animator)
    {
        characterAnimator = animator;
    }
}
