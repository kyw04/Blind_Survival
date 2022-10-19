using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System; //�̺�Ʈ ���

public class HealthSystem : MonoBehaviourPun
{
    public float StartingHealth = 100f;
    public float _currentHealth { get; protected set; }

    public bool isDead { get; protected set; }

    public event Action OnDeath;

    [PunRPC]//ȣ��Ʈ -> ��� Ŭ���̾�Ʈ �������� ü�°� ������¸� ����ȭ�ϴ� �޼���
    public void ApplyUpdateHealth(float _newHealth,bool _newDead)
    {
        _currentHealth = _newHealth;
        isDead = _newDead;
    }
    protected virtual void OnEnable()
    {
        // ������� ���� ���·� ����
        isDead = false;
        // ü���� ���� ü������ �ʱ�ȭ
        _currentHealth = StartingHealth;
    }
    [PunRPC] //������ó���� ȣ��Ʈ���� ���� �ܵ� ����ǰ� ȣ��Ʈ�� ���� �ٸ� Ŭ���̾�Ʈ���� �ϰ� ����ó��
    public virtual void OnDamage(float _damage)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _currentHealth -= _damage;

            //ȣ��Ʈ���� Ŭ���̾�Ʈ�� ����ȭ
            photonView.RPC("ApplyUpdateHealth", RpcTarget.Others, _currentHealth, isDead);

            //�ٸ�Ŭ���̾�Ʈ�� Ondamag�� �����ϵ�����
            photonView.RPC("OnDamage", RpcTarget.Others, _damage);
        }

        if(_currentHealth<=0 &&!isDead)
        {
            Die();
        }
    }
    //ü�� ȸ�� ���
    [PunRPC]
    public virtual void RestoreHealth(float _health)
    {
        if(isDead)
        {
            return;//�̹� ����ΰ��
        }

        //������ Ŭ���̾�Ʈ(ȣ��Ʈ) ���� ü�� ���� ����
        if (PhotonNetwork.IsMasterClient)
        {
            _currentHealth += _health;

            photonView.RPC("ApplyUpdateHealth", RpcTarget.Others, _currentHealth, isDead);
            //�ٸ� Ŭ�󿡼��� ����
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
