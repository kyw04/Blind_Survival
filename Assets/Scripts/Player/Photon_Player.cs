using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;
/// <summary>
/// 움직임 컨트롤
/// </summary>
public class Photon_Player : MonoBehaviourPunCallbacks , IPunObservable
{
    [Header("Server")]
    [SerializeField] PhotonView _myPhtonView;

    [Header("Player INFO")]
    [SerializeField] TextMeshProUGUI _nickNametext;

    [Header("Player INIT")]
    [SerializeField] Animator _myAnimator;
    [SerializeField] Camera _myMainCamera;
    [SerializeField] Rigidbody _myRigidboy;
    public float Movespeed = 10f;


    //보정값
    Vector3 curPos;
    Quaternion curRot;
    private float Damping = 10f;
    public void Start()
    {
        Init();
    }
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



    private void FixedUpdate()
    {
        //나인경우에만 움직임 컨트롤
        if (_myPhtonView.IsMine)
        {
            Move();
            Turn();
        }
        else
        {
            //상대기준
            transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * Damping);
            transform.rotation = Quaternion.Slerp(transform.rotation, curRot, Time.deltaTime * Damping);
            _myRigidboy.isKinematic = true;
        }
    }
    private void Move()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

       
        _myRigidboy.MovePosition(transform.position + direction.normalized * Movespeed * Time.deltaTime);


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
        }
        else
        {
            curPos = (Vector3)stream.ReceiveNext();
            curRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
