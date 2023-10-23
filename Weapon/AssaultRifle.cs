using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Scripts.Weapon
{
    public class AssaultRifle : Firearms
    {
        private IEnumerator reloadAmmoCheckerCoroutine;
       
        private MouseLook mouseLook;
        protected override void Awake()
        {
            base.Awake();
            reloadAmmoCheckerCoroutine = CheckReloadAmmoAnimationEnd();
            
            mouseLook = FindObjectOfType<MouseLook>();
        }

        protected override void Reload()
        {
            if (CurrentMaxAmmoCarrized == 0) return;
            GunAnimator.SetLayerWeight(2, 1);
            string motionName = CurrentAmmo > 0 ? "ReloadLeft" : "ReloadOutOf";//ReloadLeft 弹夹还有弹药  ReloadOutOf 弹夹没有弹药
            GunAnimator.SetTrigger(motionName);
            FirearmsReloadAudioSource.clip = CurrentAmmo > 0 ? FirearmsAudioData.ReloadLeftAudioClip:FirearmsAudioData.ReloadOutOfAudioClip;
            FirearmsReloadAudioSource.Play();
           
            if (reloadAmmoCheckerCoroutine == null)
            {
                reloadAmmoCheckerCoroutine = CheckReloadAmmoAnimationEnd();
                StartCoroutine(reloadAmmoCheckerCoroutine);
            }
            else
            {
                StopCoroutine(reloadAmmoCheckerCoroutine);
                reloadAmmoCheckerCoroutine = null;
                reloadAmmoCheckerCoroutine = CheckReloadAmmoAnimationEnd();
                StartCoroutine(reloadAmmoCheckerCoroutine);
            }
        }

        protected override void Shooting()
        {
            if (CurrentAmmo <= 0) return;
            if (!IsAllowShooting()) return;
            MuzzleParticle.Play();//火花
            FirearmsShootingAudioSource.clip = FirearmsAudioData.ShootingAudioClip;
            FirearmsShootingAudioSource.Play();
            CurrentAmmo -= 1;
            GunAnimator.Play("Fire", IsAiming?1:0, 0);
            CreateBullet();           
            CasingParticle.Play();//掉弹壳
            mouseLook.FiringForTest();//
            lastFireTime = Time.time;
        }
 
      
        protected void CreateBullet()
        {
            GameObject bullet = Instantiate(BulletPrefab, MuzzlePoint.position, MuzzlePoint.rotation);
            bullet.transform.eulerAngles += CalculateSpeedOffset();
            var bulletScript=bullet.AddComponent<Bullet>();
            bulletScript.ImpactPrefab = BulletImpactPrefab;
            bulletScript.ImpactAudioData = ImpactAudioData;
            bulletScript.audioSource = ShootingAudioSource;
            bulletScript.BulletSpeed = 80f;
            Destroy(bullet, 1f);
        }
      
       

    }
}

