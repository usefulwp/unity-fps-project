using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Scripts.Weapon
{
    public class WeaponManager : MonoBehaviour
    {
        public Firearms MainWeapon;      
        public Firearms SecondaryWeapon;
        public Text AmmoCountLabel;
        public GameObject CrosshairUI;
        public Transform WorldCameraTransform;
        public float RayCastMaxDistance = 2f;
        public LayerMask LayerMask;
        public List<Firearms> Arms = new List<Firearms>();
        [Tooltip("当前的持有的枪支")]
        public Firearms currentWeapon;
        [Space(10)]
        [SerializeField] private FPCharacterControllerMovement fpCharacterControllerMovement;
        private IEnumerator waitingForHolsterEndCoroutine;
        private bool isHideGun;//是否收枪
        public bool isFiring;

        private void UpdateAmmoInfo(int ammoCount,int carrizedMaxAmmo)
        {
            AmmoCountLabel.text = ammoCount+"/"+carrizedMaxAmmo;
        }
        private void Start()
        {
            if (MainWeapon != null)
            {
                currentWeapon = MainWeapon;
                fpCharacterControllerMovement = FindObjectOfType<FPCharacterControllerMovement>();
                fpCharacterControllerMovement.SetupAnimator(currentWeapon.GunAnimator);
            }
         

        }
        private void Update()
        {
            CheckItem();
            if (!currentWeapon) return;
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.currentSelectedGameObject == null)
                {
                    isFiring = true;
                    currentWeapon.HoldTrigger();
                }
      
            }
            if (Input.GetMouseButtonUp(0))
            {
                isFiring = false;
                currentWeapon.ReleaseTrigger();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                currentWeapon.ReloadAmmo();
               
            }
            if (Input.GetMouseButtonDown(1))
            {
                CrosshairUI.SetActive(false);
                //瞄准
                currentWeapon.Aim(true);
            }
            if (Input.GetMouseButtonUp(1))
            {
                CrosshairUI.SetActive(true);
                //退出瞄准
                currentWeapon.Aim(false);
                currentWeapon.GunAnimator.SetLayerWeight(1, 0);
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                if (!isHideGun)
                {
                    currentWeapon.GunAnimator.SetBool("IsHolster", true);
                    currentWeapon.GunAnimator.SetTrigger("Holster");
                    isHideGun = true;
                }
                else
                {
                    currentWeapon.GunAnimator.SetBool("IsHolster", false);
                    isHideGun = false;
                }

            }
            SwapWeapon();
            currentWeapon.GetCurrentAmmoAndMaxCarrizedAmmo(out int currentAmmo, out int currentCarrizedAmmo);
            UpdateAmmoInfo(currentAmmo, currentCarrizedAmmo);
        }
        void SwapWeapon()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                currentWeapon.gameObject.SetActive(false);
                currentWeapon = MainWeapon;
                currentWeapon.gameObject.SetActive(true);

                fpCharacterControllerMovement.SetupAnimator(currentWeapon.GunAnimator);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                currentWeapon.gameObject.SetActive(false);
                currentWeapon = SecondaryWeapon;
                currentWeapon.gameObject.SetActive(true);

                fpCharacterControllerMovement.SetupAnimator(currentWeapon.GunAnimator);
            }
        }

        private void SetupCurrentWeapon(Firearms targetWeapon)
        {
            if (currentWeapon != null)
                currentWeapon.gameObject.SetActive(false);
            currentWeapon = targetWeapon;
            currentWeapon.gameObject.SetActive(true);


        }
        private void CheckItem()
        {
           
            if (Physics.Raycast(WorldCameraTransform.position, WorldCameraTransform.forward, out RaycastHit hitInfo, RayCastMaxDistance, LayerMask))
            {               
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (hitInfo.collider.TryGetComponent(out BaseItem baseItem))
                    {
                        PickUpWeapon(baseItem);
                     
                        PickUpScope(baseItem);
                    }
                }
            }
        }
        private void PickUpWeapon(BaseItem baseItem)
        {
            if (baseItem is FirearmsItem tmp_firearmsItem)
            {
                foreach (Firearms firearms in Arms)
                {
                    if (tmp_firearmsItem.ArmsName.CompareTo(firearms.name) == 0)
                    {
                        switch (tmp_firearmsItem.CurrentFirearmsType)
                        {
                            case FirearmsItem.FirearmsType.AssaultRifle:
                            case FirearmsItem.FirearmsType.Submachinegun:
                            case FirearmsItem.FirearmsType.SniperRifle:
                                MainWeapon = firearms;
                                break;
                            case FirearmsItem.FirearmsType.Handgun:
                                SecondaryWeapon = firearms;
                                break;
                        }
                        SetupCurrentWeapon(firearms);
                        fpCharacterControllerMovement.SetupAnimator(currentWeapon.GunAnimator);
                    }
                }
            }
        }
        private void PickUpScope(BaseItem baseItem)
        {
            if (baseItem is AttachmentItem attachmentItem)
            {
             
                    switch (attachmentItem.CurrentAttachmentType)
                    {
                        case AttachmentItem.AttachmentType.Scope:
                        foreach (ScopeInfo scopeInfo in currentWeapon.ScopeInfos)
                        {
                            if (scopeInfo.ScopeName.CompareTo(attachmentItem.ItemName) == 0)
                            {
                                scopeInfo.ScopeGameObject.SetActive(true);
                                currentWeapon.BaseIronSight.ScopeGameObject.SetActive(false);
                                currentWeapon.SetUpCurrentScope(scopeInfo);
                            }
                            else
                            {
                                scopeInfo.ScopeGameObject.SetActive(false);
                            }
                        }
                            break;
                        case AttachmentItem.AttachmentType.other:
                            break;
                        default:
                            break;
                    }                    
                
            }
        }
    }
}
