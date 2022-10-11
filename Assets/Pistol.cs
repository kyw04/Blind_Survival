using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunState
{
    //�Ѿ� ���� �غ� ������� ��������
    Ready, Empty, Reloading
}
public class Pistol : MonoBehaviour
{
    public GunState _GunState { get; set; }
    //�Ѿ�
    public m_Bullet BulletObj;
    //�ѱ�
    public Transform GunFirePos;

    //�� ������
    public int Damage; 

    public int Magazine;//źâ ������
    public int HoldingBullets; //���� �Ѿ� ����ִ°�
    public int CurrentBullet; //���� �Ѿ� �ѿ� �������ִ°�
    public int BulletPower; //�Ѿ� �߻� ��                     
    public int ReloadTime; //�������ð�

    public float shotDelayTime; //����ӵ� (�����ð�)
    public float msBetweebshots; //�����ð�

    private void OnEnable()
    {
        //������ ����Ǹ� �ʱ�ȭ��
        Magazine = 10;
        HoldingBullets = 30;
        CurrentBullet = Magazine;
        _GunState = GunState.Ready;
    }
    private void Update()
    {
        //�ѹ߻� ����
        if (Input.GetButtonDown("Fire1")&& _GunState == GunState.Ready)
        {
            //�÷��̽ð� 
            if(Time.time> shotDelayTime)
            {                                 
                shotDelayTime = Time.time + msBetweebshots / 1000f;
                Debug.Log("�Ѿ� �߻�");

                //�Ѿ� ���� 
                m_Bullet _Bullet = Instantiate(BulletObj, GunFirePos.position, GunFirePos.rotation);
                _Bullet.SetSpeed(BulletPower);

                //5�ʵڿ� ������ �Ѿ� ����
                Destroy(_Bullet.gameObject, 5f);

                CurrentBullet--;
                if(CurrentBullet <=0) //�Ѿ��� 0�̳� 
                {
                    _GunState = GunState.Empty;
                }
            }
        }

        //������
        if(Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("������");
            //���� �Ѿ��� 0�ΰ��  �׸��� �������ִ� �Ѿ��� źâ ������� ���ų� �� ������� ������ �ȵǰ���
            if(HoldingBullets <=0 || CurrentBullet>= Magazine)
            {
                return;
            }
            StartCoroutine(Reloaded());
        }
    }
    IEnumerator Reloaded()
    {
        //���� ����
        _GunState = GunState.Reloading;

        //�ð� ���� 
        yield return new WaitForSeconds(ReloadTime);

        //�Ѿ� ����
        CurrentBullet = Magazine;
        //���� �Ѿ� ����
        HoldingBullets = HoldingBullets - Magazine;
        //���� ����
        _GunState = GunState.Ready;


    }

}
