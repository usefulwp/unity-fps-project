using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootstepLinster : MonoBehaviour
{
    public FootstepsAudioData FootstepsAudioData;
    public AudioSource FootstepAudioSource;
    public LayerMask LayerMask;

    private CharacterController characterController;
    private Transform footstepTransform;
    private float nextPlayTime;
 
 
    private void Start()
    {
        characterController = this.GetComponent<CharacterController>();
        footstepTransform = transform;
        FootstepsAudioData = Resources.Load<FootstepsAudioData>("Data/Footsteps Audio Data");
    }
    private void FixedUpdate()
    {
        if (characterController.isGrounded)
        {
            if (characterController.velocity.normalized.magnitude>=0.1f)
            {
                nextPlayTime += Time.deltaTime;

                bool isHit= Physics.Linecast(footstepTransform.position, footstepTransform.position+Vector3.down *( characterController.height / 2+characterController.skinWidth- characterController.center.y), out RaycastHit hitInfo, LayerMask);
#if UNITY_EDITOR
                Debug.DrawLine(footstepTransform.position, footstepTransform.position+Vector3.down * (characterController.height / 2 + characterController.skinWidth- characterController.center.y), Color.red,0.25f);
#endif
                if (isHit)
                {
                    foreach (var item in FootstepsAudioData.FootstepsAduios)
                    {
                        if (hitInfo.collider.CompareTag(item.Tag))
                        {
                         
                            if (nextPlayTime >= item.Delay)
                            {
                                int audioCount = item.AudioClips.Count;
                                int audioIndex = Random.Range(0, audioCount);
                                AudioClip footstepAudioClip = item.AudioClips[audioIndex];
                                FootstepAudioSource.clip = footstepAudioClip;
                                FootstepAudioSource.Play();
                                nextPlayTime = 0;
                                break;
                            }                          
                        }
                    }
                   
                }
            }
        }
    }
}
