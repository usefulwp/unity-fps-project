using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Weapon
{
    public abstract class Firearms : MonoBehaviour,IWeapon
    {
        public Camera EyeCamera;
        public Camera GunCamera;
        public Transform MuzzlePoint;//枪口点
        public Transform CasingPoint;//弹壳点
        public ParticleSystem MuzzleParticle;
        public ParticleSystem CasingParticle;
        public GameObject BulletPrefab;
        public GameObject BulletImpactPrefab;
        public AudioSource FirearmsShootingAudioSource;
        public AudioSource FirearmsReloadAudioSource;
        public AudioSource ShootingAudioSource;
        public FirearmsAudioData FirearmsAudioData;
        public ImpactAudioData ImpactAudioData;

        [Tooltip("伤害")]
        public float Attack;
        public float FireRate;//射速
        public int AmmoInMag=30;//弹药弹夹
        public int MaxAmmoCarrized =120;//最大携带子弹数

        public float SpeedAngle;//散射角度

        public List<ScopeInfo> ScopeInfos;
        public ScopeInfo BaseIronSight;//机瞄
       
        protected ScopeInfo rigoutScopeInfo;
        protected int CurrentAmmo;
        protected int CurrentMaxAmmoCarrized;
        public Animator GunAnimator;
        protected float lastFireTime;
        protected AnimatorStateInfo GunAnimatorStateInfo;
        protected float EyeOriginalFOV;//视野宽度
        protected float GunOriginalFOV;
        protected bool IsAiming;
        protected bool IsHoldingTrigger;
        private IEnumerator doAimCoroutine;
        protected Vector3 originalEyePosition;
        protected Transform gunCameraTransform;

        public void GetCurrentAmmoAndMaxCarrizedAmmo(out int currentAmmo,out int maxCarrizedAmmo)
        {
            currentAmmo = CurrentAmmo;
            maxCarrizedAmmo = CurrentMaxAmmoCarrized;
        }
        protected virtual void Awake()
        {
            CurrentAmmo = AmmoInMag;
            CurrentMaxAmmoCarrized = MaxAmmoCarrized;
            GunAnimator = GetComponent<Animator>();
            EyeOriginalFOV = EyeCamera.fieldOfView;
            GunOriginalFOV = GunCamera.fieldOfView;
            doAimCoroutine = DoAim();
            rigoutScopeInfo = BaseIronSight;
            gunCameraTransform = GunCamera.transform;
            originalEyePosition = gunCameraTransform.localPosition;

            FirearmsAudioData = Resources.Load<FirearmsAudioData>("Data/AK47 Audio Data");
            ImpactAudioData = Resources.Load<ImpactAudioData>("Data/Impact Audio Data");
        }
        public void doAttack()
        {
            Shooting();
         
        }
        protected abstract void Shooting();
        protected abstract void Reload();//装弹
        internal void Aim(bool isAiming)
        {
            GunAnimator.SetLayerWeight(1, 1);
            IsAiming = isAiming;
            GunAnimator.SetBool("Aim", IsAiming);
            if (doAimCoroutine != null)
            {
                StopCoroutine(doAimCoroutine);
                doAimCoroutine = null;
                doAimCoroutine = DoAim();
                StartCoroutine(doAimCoroutine);
            }
            else
            {
                doAimCoroutine = DoAim();
                StartCoroutine(doAimCoroutine);
            }
        }
        internal void HoldTrigger()
        {
            doAttack();
            IsHoldingTrigger = true;
        }
        internal void ReleaseTrigger()
        {
            IsHoldingTrigger = false;
        }
        internal void ReloadAmmo()
        {
            Reload();
        }
        internal void SetUpCurrentScope(ScopeInfo scopeInfo)
        {
            rigoutScopeInfo = scopeInfo;
        }
        protected bool IsAllowShooting()//ak47的速度 600发/1分钟  10发/秒  1/10=0.1秒/发
        {
            return Time.time - lastFireTime > 1 / FireRate;        
        }
        protected Vector3 CalculateSpeedOffset()
        {
            float SpeedPercent= SpeedAngle / EyeCamera.fieldOfView;//散射百分比
            return SpeedPercent*Random.insideUnitCircle;
        }
        protected IEnumerator CheckReloadAmmoAnimationEnd()
        {
            while (true)
            {
                yield return null;
                GunAnimatorStateInfo = GunAnimator.GetCurrentAnimatorStateInfo(2);
                if (GunAnimatorStateInfo.IsTag("ReloadAmmo"))
                {
                    if (GunAnimatorStateInfo.normalizedTime >= 0.88f)
                    {
                        int needAmmoCount = AmmoInMag - CurrentAmmo;//当前需要的弹药数
                        int remainMaxAmmoCarrized = CurrentMaxAmmoCarrized - needAmmoCount;
                        if (remainMaxAmmoCarrized <= 0)
                        {
                            CurrentAmmo += CurrentMaxAmmoCarrized;
                        }
                        else
                        {
                            CurrentAmmo = AmmoInMag;
                        }
                        CurrentMaxAmmoCarrized = remainMaxAmmoCarrized <= 0 ? 0 : remainMaxAmmoCarrized;
                        GunAnimator.SetLayerWeight(2, 0);
                        yield break;
                    }
                }
            }
        }
        protected IEnumerator DoAim()
        {
            while (true)
            {
                yield return null;

                float tmp_CurrentEyeFOV = 0;
                EyeCamera.fieldOfView = Mathf.SmoothDamp(EyeCamera.fieldOfView, IsAiming ? rigoutScopeInfo.EyeFOV : EyeOriginalFOV, ref tmp_CurrentEyeFOV, Time.deltaTime * 2);

                float tmp_CurrentGunFOV = 0;
                GunCamera.fieldOfView = Mathf.SmoothDamp(GunCamera.fieldOfView, IsAiming ? rigoutScopeInfo.GunFOV : GunOriginalFOV, ref tmp_CurrentGunFOV, Time.deltaTime * 2);

                Vector3 tmp_RefPosition = Vector3.zero;
                gunCameraTransform.localPosition = Vector3.SmoothDamp(gunCameraTransform.localPosition, IsAiming ? rigoutScopeInfo.GunCameraPosition : originalEyePosition, ref tmp_RefPosition, Time.deltaTime * 2);
            }
        }
    }
    [System.Serializable]
    public class ScopeInfo
    {
        public string ScopeName;
        public GameObject ScopeGameObject;
        public float EyeFOV;
        public float GunFOV;
        public Vector3 GunCameraPosition;
    }
}

