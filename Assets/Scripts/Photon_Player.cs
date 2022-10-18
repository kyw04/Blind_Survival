using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;

public class Photon_Player : MonoBehaviourPunCallbacks , IPunObservable
{
    [SerializeField] PhotonView _myPhtonView;
    [SerializeField] TextMeshProUGUI _nickNametext;
    [SerializeField] Animator _myAnimator;
    [SerializeField] Camera _myMainCamera;
    [SerializeField] Rigidbody _myRigidboy;

    public CharectorHealth _myCharectorHealth;
    public float speed;

    Vector3 curPos;
    Quaternion curRot;
    public float Damping = 10f;
    //초기화 메소드
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
        //나인경우에만 움직임 컨트롤
        if (_myPhtonView.IsMine)
        {
            Move();
            Turn();
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * Damping);
            transform.rotation = Quaternion.Slerp(transform.rotation, curRot, Time.deltaTime * Damping);
        }
    }
    private void Move()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

       
        _myRigidboy.MovePosition(transform.position + direction.normalized * speed * Time.deltaTime);


        _myAnimator.SetBool("IsWalk", direction != Vector3.zero);
    }
    //플레이어 회전
    public void Turn()
    {
        _myMainCamera.transform.position = new Vector3(transform.position.x, _myMainCamera.transform.position.y, transform.position.z);

        Ray ray = _myMainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;

        if (Physics.Raycast(ray, out raycastHit))
        {
            Vector3 _position = raycastHit.point;
            _position.y = transform.position.y;

            transform.LookAt(_position);
        }
    }

    //포톤에서 제공하는 변수 동기화 인터페이스
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
           // stream.SendNext(_myCharectorHealth.myHealth.value);
        }
        else
        {
            curPos = (Vector3)stream.ReceiveNext();
            curRot = (Quaternion)stream.ReceiveNext();
           // _myCharectorHealth.myHealth.value = (float)stream.ReceiveNext();
        }
    }
}
