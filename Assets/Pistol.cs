using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunState
{
    //�Ѿ� ���� �غ� ������� ��������
    Ready, Empty, Reloading
}
public class Pistol : MonoBehaviour
{
    public PhotonView _pv;
    public GunState _GunState { get; set; }
    //�Ѿ�
    public GameObject BulletObj;
    //�ѱ�
    public Transform GunFirePos;

    public AudioSource _myGunSound;
    public AudioClip _SoundClip;
    public int Magazine;//źâ ������
    public int HoldingBullets; //���� �Ѿ� ����ִ°�
    public int CurrentBullet; //���� �Ѿ� �ѿ� �������ִ°�
    public int BulletPower; //�Ѿ� �߻� ��                     
    public int ReloadTime; //�������ð�

    public float shotDelayTime; //����ӵ� (�����ð�)


    private void OnEnable()
    {
        //������ ����Ǹ� �ʱ�ȭ��
        shotDelayTime = 0.15f;
        Magazine = 10;
        HoldingBullets = 30;
        CurrentBullet = Magazine;
        _GunState = GunState.Ready;
    }

    private void Update()
    {
        if(_pv.IsMine && Input.GetMouseButtonDown(0))
        {
            _pv.RPC("WaponFire", RpcTarget.All);
        }
        else if(_pv.IsMine && Input.GetKeyDown(KeyCode.R))
        {
            _pv.RPC("Reloading", RpcTarget.All);
        }
    }
    [PunRPC]
    private void WaponFire()
    {
        //�ѹ߻� ����
        if (_GunState == GunState.Ready)
        {
            _myGunSound.PlayOneShot(_SoundClip);

            //PhotonNetwork.Instantiate("Bullet", GunFirePos.position, GunFirePos.rotation).GetComponent<PhotonView>().RPC("SetSpeed", RpcTarget.All, BulletPower);
            Instantiate(BulletObj, GunFirePos.position, GunFirePos.rotation);

            CurrentBullet--;
            if (CurrentBullet <= 0) //�Ѿ��� 0�̳� 
            {
                _GunState = GunState.Empty;
            }
        }
    }
    [PunRPC]
    public void Reloading()
    {
        if (HoldingBullets <= 0 || CurrentBullet >= Magazine)
        {
            return;
        }
        StartCoroutine(Reloaded());
    }

    IEnumerator Reloaded()
    {
        //���� ����
        _GunState = GunState.Reloading;

        //�ð� ���� 
        yield return new WaitForSeconds(ReloadTime);

        //�Ѿ� ����
        CurrentBullet = Magazine;
        //���� �Ѿ� ����
        HoldingBullets = HoldingBullets - Magazine;
        //���� ����
        _GunState = GunState.Ready;
    }

}
