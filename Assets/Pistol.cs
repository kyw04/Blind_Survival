using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunState
{
    //총알 상태 준비 비어있음 재장전중
    Ready, Empty, Reloading
}
public class Pistol : MonoBehaviour
{
    public PhotonView _pv;
    public GunState _GunState { get; set; }
    //총알
    public GameObject BulletObj;
    //총구
    public Transform GunFirePos;

    public AudioSource _myGunSound;
    public AudioClip _SoundClip;
    public int Magazine;//탄창 사이즈
    public int HoldingBullets; //예비 총알 들고있는거
    public int CurrentBullet; //현재 총알 총에 장전되있는거
    public int BulletPower; //총알 발사 힘                     
    public int ReloadTime; //재장전시간

    public float shotDelayTime; //연사속도 (지연시간)


    private void OnEnable()
    {
        //게임이 실행되면 초기화시
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
        //총발사 구현
        if (_GunState == GunState.Ready)
        {
            _myGunSound.PlayOneShot(_SoundClip);

            //PhotonNetwork.Instantiate("Bullet", GunFirePos.position, GunFirePos.rotation).GetComponent<PhotonView>().RPC("SetSpeed", RpcTarget.All, BulletPower);
            Instantiate(BulletObj, GunFirePos.position, GunFirePos.rotation);

            CurrentBullet--;
            if (CurrentBullet <= 0) //총알이 0이냐 
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
        //상태 변경
        _GunState = GunState.Reloading;

        //시간 지연 
        yield return new WaitForSeconds(ReloadTime);

        //총알 충전
        CurrentBullet = Magazine;
        //남은 총알 감소
        HoldingBullets = HoldingBullets - Magazine;
        //상태 변경
        _GunState = GunState.Ready;
    }

}
