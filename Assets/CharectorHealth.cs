using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharectorHealth : MonoBehaviourPunCallbacks
{
    public PhotonView myPhotonView;
    public Slider myHealth;


    public void Hit(int _damage)
    {
        myHealth.value -= _damage;
        if(myHealth.value <=0)
        {
            Debug.Log("Á×À½");
            myPhotonView.RPC("DestroyRPC", RpcTarget.AllBuffered);
        }
    }
    [PunRPC]
    void DestroyRPC() => Destroy(gameObject);
}
