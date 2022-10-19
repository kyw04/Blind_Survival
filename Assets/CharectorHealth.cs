using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharectorHealth : MonoBehaviour
{
    public PhotonView pv;
    public Slider myHealth;

    private int _myHp = 100; //초기 채력
    private void Awake()
    {
        myHealth.value = _myHp;
    }

    public void Hit(int _damage)
    {
        myHealth.value -= _damage;
        if (myHealth.value <= 0)
        {
            pv.RPC("DestroyRPC", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    void DestroyRPC()
    {
         Destroy(this.gameObject);       
    }

}
