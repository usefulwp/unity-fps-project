using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Weapon;
namespace Scripts.Weapon
{
    public class Bullet : MonoBehaviour
    {
        public float BulletSpeed;
        public ImpactAudioData ImpactAudioData;
        public GameObject ImpactPrefab;
        public AudioSource audioSource;
        private Vector3 prePosition;
        private Transform bulletTransform;
        private WeaponManager weaponManager;
        // Start is called before the first frame update
        void Start()
        {
            bulletTransform = transform;
            prePosition = bulletTransform.position;
            weaponManager = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponManager>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Enemy"))
            {
                //子弹打到怪物上要进行的处理
                EnemyLogic enemyLogic = other.gameObject.GetComponent<EnemyLogic>();
                enemyLogic.enemyState = EnemyState.Bulleted;
                Animator enemyAnimator = enemyLogic.animator;
                if (enemyLogic.CurrentHp>0)
                {
                    enemyAnimator.SetTrigger("Wound");
                    enemyLogic.CurrentHp -= weaponManager.currentWeapon.Attack;
                    StartCoroutine(ChangeMaterialColor(enemyLogic));
                    enemyLogic.FillImage.fillAmount = enemyLogic.CurrentHp/enemyLogic.TotalHp;
                }
            }
        }
        IEnumerator ChangeMaterialColor(EnemyLogic enemyLogic) //处理怪物受伤变红
        {
            enemyLogic.BodyMaterial.color = Color.red;
            yield return new  WaitForSeconds(0.5f);
            enemyLogic.BodyMaterial.color = enemyLogic.OriginalColor;
        }
        private void Update()
        {
            prePosition = bulletTransform.position;
            bulletTransform.Translate(0, 0, BulletSpeed*Time.deltaTime);
            if (Physics.Raycast(prePosition, (bulletTransform.position-prePosition).normalized, out RaycastHit hitInfo, (bulletTransform.position - prePosition).magnitude))
            {
                var bulletEffect=Instantiate(ImpactPrefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal, Vector3.up));
                Destroy(bulletEffect, 3);

                var ImpactTagsWithAudio =ImpactAudioData.ImpactTagsWithAudios.Find((tmp_ImpactAudio) =>
                               {
                                   return tmp_ImpactAudio.Tag.Equals(hitInfo.collider.tag);
                               }
                               );                 
                if (ImpactTagsWithAudio==null) return;
                int length = ImpactTagsWithAudio.ImpactAudioClips.Count;
                AudioClip ImpactAudioClip = ImpactTagsWithAudio.ImpactAudioClips[Random.Range(0, length)];
                //AudioSource.PlayClipAtPoint(ImpactAudioClip, hitInfo.point);
                audioSource.clip = ImpactAudioClip;
                audioSource.Play();
            }
        }
        
    }  
}
