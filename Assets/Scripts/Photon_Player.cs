using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
public class Photon_Player : MonoBehaviourPunCallbacks , IPunObservable
{
    public Slider myHealth;
    public TextMeshProUGUI NickNametext;
    public PhotonView myPhtonView;
    public Animator myAnimator;
    private Camera mainCamera;
    private Rigidbody rb;
    public float speed;

    private void Awake()
    {

        NickNametext.text = myPhtonView.IsMine ? PhotonNetwork.NickName : myPhtonView.Owner.NickName;
        NickNametext.color = myPhtonView.IsMine ? Color.green : Color.red;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        //���ΰ�쿡�� ������ ��Ʈ��
        if (myPhtonView.IsMine)
        {
            mainCamera.transform.position = new Vector3(transform.position.x, mainCamera.transform.position.y, transform.position.z);

            Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            rb.MovePosition(transform.position + direction.normalized * speed * Time.deltaTime);
            myAnimator.SetBool("IsWalk", direction != Vector3.zero);

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;

            if (Physics.Raycast(ray, out raycastHit))
            {
                Vector3 _position = raycastHit.point;
                _position.y = transform.position.y;

                transform.LookAt(_position);
            }
        }
    }

    //���濡�� �����ϴ� ���� ����ȭ �������̽�
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //throw new System.NotImplementedException();
    }
}
