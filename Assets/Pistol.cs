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
    public GunState _GunState { get; set; }
    //총알
    public m_Bullet BulletObj;
    //총구
    public Transform GunFirePos;

    //총 데미지
    public int Damage; 

    public int Magazine;//탄창 사이즈
    public int HoldingBullets; //예비 총알 들고있는거
    public int CurrentBullet; //현재 총알 총에 장전되있는거
    public int BulletPower; //총알 발사 힘                     
    public int ReloadTime; //재장전시간

    public float shotDelayTime; //연사속도 (지연시간)
    public float msBetweebshots; //지연시간

    private void OnEnable()
    {
        //게임이 실행되면 초기화시
        Magazine = 10;
        HoldingBullets = 30;
        CurrentBullet = Magazine;
        _GunState = GunState.Ready;
    }
    private void Update()
    {
        //총발사 구현
        if (Input.GetButtonDown("Fire1")&& _GunState == GunState.Ready)
        {
            //플레이시간 
            if(Time.time> shotDelayTime)
            {                                 
                shotDelayTime = Time.time + msBetweebshots / 1000f;
                Debug.Log("총알 발사");

                //총알 생성 
                m_Bullet _Bullet = Instantiate(BulletObj, GunFirePos.position, GunFirePos.rotation);
                _Bullet.SetSpeed(BulletPower);

                //5초뒤에 생성한 총알 삭제
                Destroy(_Bullet.gameObject, 5f);

                CurrentBullet--;
                if(CurrentBullet <=0) //총알이 0이냐 
                {
                    _GunState = GunState.Empty;
                }
            }
        }

        //재장전
        if(Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("재장전");
            //남은 총알이 0인경우  그리고 장전되있는 총알이 탄창 사이즈랑 같거나 더 많은경우 재장전 안되게함
            if(HoldingBullets <=0 || CurrentBullet>= Magazine)
            {
                return;
            }
            StartCoroutine(Reloaded());
        }
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
