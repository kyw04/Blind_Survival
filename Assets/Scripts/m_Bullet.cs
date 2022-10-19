using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class m_Bullet : MonoBehaviourPun
{
    public PhotonView pv;
    //ÃÑ¾Ë ¼Óµµ
    public int BulletSpeed;
    //ÃÑ µ¥¹ÌÁö
    public int Damage = 50;
    public int dir;
    public Rigidbody rb;

    Vector3 curPos;
    Quaternion curRot;
    private float Damping = 10f;

    private void Start()
    {
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * BulletSpeed * dir);
        Destroy(this.gameObject, 5.0f);
    }
    private void Update()
    {
        //transform.Translate(Vector3.forward * BulletSpeed * Time.deltaTime * dir);
    }
    private void OnTriggerEnter(Collider col)
    {

        if(!photonView.IsMine && col.tag =="Player" && col.GetComponent<PhotonView>().IsMine)
        {
            Debug.Log($"{col.gameObject.name}ºÎµ÷Ä§");
            col.GetComponent<CharectorHealth>().OnDamage(Damage);
        }
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
