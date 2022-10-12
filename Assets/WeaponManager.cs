using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject EqupitWeapon; //내가 장착한 무기
    public Transform WeaponHoldPos; //총이 쥐어지는 위치
    public GameObject startingWeapon; //처음 시작무기


    private void OnEnable()
    {
        if (startingWeapon != null)
            EqupitGun(startingWeapon.gameObject);
    }

    private void EqupitGun(GameObject _gunToEquip)
    {
        if (EqupitWeapon != null)
        {
            Destroy(EqupitWeapon.gameObject);
        }
        EqupitWeapon = Instantiate(_gunToEquip, WeaponHoldPos.position, WeaponHoldPos.rotation);
        EqupitWeapon.transform.parent = WeaponHoldPos;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0)&& EqupitWeapon !=null)
        {
            EqupitWeapon.GetComponent<Pistol>().Fire();
        }
        //재장전
        if (Input.GetKeyDown(KeyCode.R))
        {
            EqupitWeapon.GetComponent<Pistol>().Reloading();
        }
    }
}
