using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviourPun
{
    [Header("Weapon Info")]
    public Transform WeaponHold;//무기 장착 위치
    public GameObject EquipWeapoin; //무기장착
    public GameObject StartingWeapon; //처음 시작 무기


    private void Start()
    {
        if (StartingWeapon != null) //
            EquipGun(StartingWeapon.gameObject);
    }
    private void Update()
    {
        if (photonView.IsMine)
        {
            var Gun = EquipWeapoin.GetComponent<Pistol>();
            //자기자신만 컨트롤 가능
            if (Input.GetMouseButtonDown(0))
            {
                Gun.WaponFire();//총 발사
            }
            else if (Input.GetKeyDown(KeyCode.R)) 
            {
                Gun.Reloading();
            }
        }
    }
    public void EquipGun(GameObject _gunToEquip)
    {
        //혹시 총을 쥐고 있다면 삭제함
        if (EquipWeapoin != null)
        {
            Destroy(EquipWeapoin.gameObject);
        }
        //총 생성해서 
        EquipWeapoin = Instantiate(_gunToEquip, WeaponHold.position, WeaponHold.rotation);
        EquipWeapoin.transform.parent = WeaponHold;

    }

    
}
