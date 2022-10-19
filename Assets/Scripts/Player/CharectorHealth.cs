using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharectorHealth : MonoBehaviourPun
{
    private int StartingHealth; //초기 체력
    public int _currentHealth; //현재 체력
    public bool isDead = false;
    public Slider myHealth;


    public AudioSource _as;
    public AudioClip audioClip;


    private Renderer[] renderers;
    public Photon_Player pp;
    public Pistol playerpistol;
    public BoxCollider playercol;
    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }
    protected  void OnEnable()
    {

        myHealth.maxValue = StartingHealth; //최대 체력
        myHealth.value = _currentHealth;//현재 체력

    }

    public  void RestoreHealth(float _health)
    {
        // 체력 갱신
        myHealth.value = _currentHealth;
    }
    
    public  void OnDamage(int _damage)
    {
        // 갱신된 체력을 체력 슬라이더에 반영
        _currentHealth -= _damage;
        myHealth.value = _currentHealth;

        if(_currentHealth<=0)
        {
            Debug.Log("뒤짐");
            StartCoroutine(Die());
        }
    }

    public IEnumerator Die()
    {
        pp.enabled = false;
        playerpistol.enabled = false;   
        playercol.enabled = false;

        _as.PlayOneShot(audioClip);
        SetPlayerVisible(false);

        yield return new WaitForSeconds(3.0f);

        if (photonView.IsMine)
        {
            Vector3 rand = Random.insideUnitSphere * 50f;

            rand.y = 0f;

            transform.position = rand;
        }
        gameObject.SetActive(false);
        gameObject.SetActive(true);

        SetPlayerVisible(true);
        playerpistol.enabled = true;
        playercol.enabled = true;
        pp.enabled = true;

    }
    void SetPlayerVisible(bool isVisible)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].enabled = isVisible;
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(myHealth.value);
        }
        else
        {
            myHealth.value = (float)stream.ReceiveNext();
        }
    }
}
