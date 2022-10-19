using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviourPun
{
    [Header("Weapon Info")]
    public Transform WeaponHold;//���� ���� ��ġ
    public GameObject EquipWeapoin; //��������
    public GameObject StartingWeapon; //ó�� ���� ����


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
            //�ڱ��ڽŸ� ��Ʈ�� ����
            if (Input.GetMouseButtonDown(0))
            {
                Gun.WaponFire();//�� �߻�
            }
            else if (Input.GetKeyDown(KeyCode.R)) 
            {
                Gun.Reloading();
            }
        }
    }
    public void EquipGun(GameObject _gunToEquip)
    {
        //Ȥ�� ���� ��� �ִٸ� ������
        if (EquipWeapoin != null)
        {
            Destroy(EquipWeapoin.gameObject);
        }
        //�� �����ؼ� 
        EquipWeapoin = Instantiate(_gunToEquip, WeaponHold.position, WeaponHold.rotation);
        EquipWeapoin.transform.parent = WeaponHold;

    }

    
}
