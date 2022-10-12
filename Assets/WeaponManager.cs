using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject EqupitWeapon; //���� ������ ����
    public Transform WeaponHoldPos; //���� ������� ��ġ
    public GameObject startingWeapon; //ó�� ���۹���


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
        //������
        if (Input.GetKeyDown(KeyCode.R))
        {
            EqupitWeapon.GetComponent<Pistol>().Reloading();
        }
    }
}
