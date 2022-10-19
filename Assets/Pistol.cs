using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunState
{
    //총알 상태 준비 비어있음 재장전중
    Ready, Empty, Reloading
}
public class Pistol : MonoBehaviourPun , IPunObservable
{
    public PhotonView _pv;
    public GunState _GunState { get; set; }
    //총알
    public GameObject BulletObj;
    //총구
    public Transform GunFirePos;

    public AudioSource _myGunSound;
    public AudioClip _SoundClip;
    public AudioClip _reloading;
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
        HoldingBullets = 9999999;
        CurrentBullet = Magazine;
        _GunState = GunState.Ready;
    }
    private void Update()
    {
        if (photonView.IsMine)
        {
            //자기자신만 컨트롤 가능
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
        //로컬 (자기자신)오브젝트라면 쓰기 부분이 실행되고 아니면 아래 동기화 else에서 처리
        if(stream.IsWriting)
        {
            stream.SendNext(HoldingBullets); //남은 전체총알
            stream.SendNext(CurrentBullet); //총에 장전되어잇는거
            stream.SendNext(_GunState); //총 상태
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
        //총발사 구현
        if (_GunState == GunState.Ready)
        {
            _myGunSound.PlayOneShot(_SoundClip);
    
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
        _myGunSound.PlayOneShot(_reloading);
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
