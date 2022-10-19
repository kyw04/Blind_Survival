using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunState
{
    //�Ѿ� ���� �غ� ������� ��������
    Ready, Empty, Reloading
}
public class Pistol : MonoBehaviourPun , IPunObservable
{
    public PhotonView _pv;
    public GunState _GunState { get; set; }
    //�Ѿ�
    public GameObject BulletObj;
    //�ѱ�
    public Transform GunFirePos;

    public AudioSource _myGunSound;
    public AudioClip _SoundClip;
    public AudioClip _reloading;
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
        HoldingBullets = 9999999;
        CurrentBullet = Magazine;
        _GunState = GunState.Ready;
    }
    private void Update()
    {
        if (photonView.IsMine)
        {
            //�ڱ��ڽŸ� ��Ʈ�� ����
            if (Input.GetMouseButtonDown(0))
            {
                WaponFire();

                _pv.RPC("WaponFire", RpcTarget.Others, null);
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                Reloading();
                _pv.RPC("Reloading", RpcTarget.Others, null);
            }
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //���� (�ڱ��ڽ�)������Ʈ��� ���� �κ��� ����ǰ� �ƴϸ� �Ʒ� ����ȭ else���� ó��
        if(stream.IsWriting)
        {
            stream.SendNext(HoldingBullets); //���� ��ü�Ѿ�
            stream.SendNext(CurrentBullet); //�ѿ� �����Ǿ��մ°�
            stream.SendNext(_GunState); //�� ����
        }
        else
        {
            HoldingBullets = (int)stream.ReceiveNext();
            CurrentBullet = (int)stream.ReceiveNext();
            _GunState = (GunState)stream.ReceiveNext();
        }
    }


    [PunRPC]
    public void WaponFire()
    {
        //�ѹ߻� ����
        if (_GunState == GunState.Ready)
        {
            _myGunSound.PlayOneShot(_SoundClip);
    
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
        _myGunSound.PlayOneShot(_reloading);
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
