using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System; //이벤트 사용

public class HealthSystem : MonoBehaviourPun
{
    public float StartingHealth = 100f;
    public float _currentHealth { get; protected set; }

    public bool isDead { get; protected set; }

    public event Action OnDeath;

    [PunRPC]//호스트 -> 모든 클라이언트 방향으로 체력과 사망상태를 동기화하는 메서드
    public void ApplyUpdateHealth(float _newHealth,bool _newDead)
    {
        _currentHealth = _newHealth;
        isDead = _newDead;
    }
    protected virtual void OnEnable()
    {
        // 사망하지 않은 상태로 시작
        isDead = false;
        // 체력을 시작 체력으로 초기화
        _currentHealth = StartingHealth;
    }
    [PunRPC] //데미지처리를 호스트에서 먼저 단독 실행되고 호스트를 통해 다른 클라이언트에서 일괄 실행처리
    public virtual void OnDamage(float _damage)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _currentHealth -= _damage;

            //호스트에서 클라이언트로 동기화
            photonView.RPC("ApplyUpdateHealth", RpcTarget.Others, _currentHealth, isDead);

            //다른클라이언트도 Ondamag를 실행하도록함
            photonView.RPC("OnDamage", RpcTarget.Others, _damage);
        }

        if(_currentHealth<=0 &&!isDead)
        {
            Die();
        }
    }
    //체력 회복 기능
    [PunRPC]
    public virtual void RestoreHealth(float _health)
    {
        if(isDead)
        {
            return;//이미 사망인경우
        }

        //마스터 클라이언트(호스트) 직접 체력 갱신 가능
        if (PhotonNetwork.IsMasterClient)
        {
            _currentHealth += _health;

            photonView.RPC("ApplyUpdateHealth", RpcTarget.Others, _currentHealth, isDead);
            //다른 클라에서도 실행
            photonView.RPC("RestoreHealth",RpcTarget.Others,_health); ;
        }
    }

    public virtual void Die()
    {
       if(OnDeath !=null)
        {
            OnDeath();
        }
        isDead = true;
    }
}
