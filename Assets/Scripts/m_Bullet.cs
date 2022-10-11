using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class m_Bullet : MonoBehaviour
{
    //ÃÑ¾Ë ¼Óµµ
    public float bulletPower { get; private set; }

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * bulletPower);
    }
    public void SetSpeed(float _BulletSpeed)
    {
        bulletPower = _BulletSpeed;
    }
}
