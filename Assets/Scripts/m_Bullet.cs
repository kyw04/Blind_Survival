using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class m_Bullet : MonoBehaviourPunCallbacks
{
    //ÃÑ¾Ë ¼Óµµ

    //ÃÑ µ¥¹ÌÁö
    public int Damage = 10;

    private void Start()
    {
 
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 1000f);
        Destroy(this.gameObject, 3.0f);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<CharectorHealth>().Hit(Damage);
        }
        
        //ºÎµ÷Ä¡¸é »èÁ¦ÇÔ
        Destroy(this.gameObject);
    }
    /*
    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * bulletPower);
        distroyTime += Time.deltaTime;
        if (distroyTime >= 5f)
        {
            myPhotonView.RPC("DestroyRPC", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    void SetSpeed(int _BulletSpeed)
    {
        bulletPower = _BulletSpeed;
    }
    [PunRPC]
    void DestroyRPC() => Destroy(gameObject);
    
    private void OnTriggerEnter(Collider other)
    {
        if(!myPhotonView.IsMine && other.tag =="Player" && other.GetComponent<PhotonView>().IsMine)
        {
            other.GetComponent<CharectorHealth>().Hit(10);
            myPhotonView.RPC("DestroyRPC", RpcTarget.AllBuffered);
        }
    }
    */
}
