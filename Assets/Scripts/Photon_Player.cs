using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
public class Photon_Player : MonoBehaviourPunCallbacks , IPunObservable
{
    [SerializeField] PhotonView _myPhtonView;
    [SerializeField] TextMeshProUGUI _nickNametext;
    [SerializeField] Animator _myAnimator;
    [SerializeField] Camera _myMainCamera;
    [SerializeField] Rigidbody _myRigidboy;

    public float speed;

    Vector3 curPos;

    //�ʱ�ȭ �޼ҵ�
    public void Init()
    {
        _myPhtonView = GetComponent<PhotonView>();
        _nickNametext.text = _myPhtonView.IsMine ? PhotonNetwork.NickName : _myPhtonView.Owner.NickName;
        _nickNametext.color = _myPhtonView.IsMine ? Color.green : Color.red;

        _myAnimator = GetComponent<Animator>();
        _myRigidboy = GetComponent<Rigidbody>();
        _myMainCamera = Camera.main;
    }


    public void Start()
    {
        Init();
    }
    private void Update()
    {
        //���ΰ�쿡�� ������ ��Ʈ��
        if (_myPhtonView.IsMine)
        {
            Movements();
        }
    }

    //�÷��̾� ȸ��
    public void Movements()
    {
        _myMainCamera.transform.position = new Vector3(transform.position.x, _myMainCamera.transform.position.y, transform.position.z);

        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        _myRigidboy.MovePosition(transform.position + direction.normalized * speed * Time.deltaTime);
        _myAnimator.SetBool("IsWalk", direction != Vector3.zero);

        Ray ray = _myMainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;

        if (Physics.Raycast(ray, out raycastHit))
        {
            Vector3 _position = raycastHit.point;
            _position.y = transform.position.y;

            transform.LookAt(_position);
        }
    }

    //���濡�� �����ϴ� ���� ����ȭ �������̽�
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            curPos = (Vector3)stream.ReceiveNext();

        }
    }
}
