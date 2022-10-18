using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public enum GunState
{
    //�Ѿ� ���� �غ� ������� ��������
    Ready, Empty, Reloading
}
public class Pistol : MonoBehaviourPunCallbacks
{
    public PhotonView pv;
    public GunState _GunState { get; set; }
    //�Ѿ�
    public GameObject BulletObj;
    //�ѱ�
    public Transform GunFirePos;

    //�� ������
    public int Damage; 

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
    private void Start()
    {
        pv = GetComponentInParent<PhotonView>();
    }
    //WaponManager���� ����
    public void GunFire()
    {
        Debug.Log("Ŭ��");
        Fire();
        pv.RPC("Fire", RpcTarget.Others, null);
    }

    [PunRPC]
    private void Fire()
    {
        //�ѹ߻� ����
        if (_GunState == GunState.Ready)
        {
            StartCoroutine(ShotCoroutine());

        }
    }
    IEnumerator ShotCoroutine()
    {
         yield return new WaitForSeconds(shotDelayTime);

        Debug.Log("�ڷ�ƾ ����");
        //PhotonNetwork.Instantiate("Bullet", GunFirePos.position, GunFirePos.rotation).GetComponent<PhotonView>().RPC("SetSpeed", RpcTarget.All, BulletPower);
        GameObject bullet = Instantiate(BulletObj, GunFirePos.position, GunFirePos.rotation);

        CurrentBullet--;
        if (CurrentBullet <= 0) //�Ѿ��� 0�̳� 
        {
            _GunState = GunState.Empty;
        }
    }

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
