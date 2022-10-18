using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
//퀵 조인
public class ServerManager : MonoBehaviourPunCallbacks
{
    //로딩시 텍스트 출력
    [SerializeField] TextMeshProUGUI _lodingText;
    [SerializeField] GameObject _JoinMenu;
    [SerializeField] readonly string _gameVersion = "1.0";

    public TMP_InputField NickNameInput;
    
    public GameObject TitleUI;

    //게임 실행시 바로 접속하게함
    private void Awake()
    {
        Init();

    }


    private void Init()
    {
        //서버에 연결중일때는 꺼둠
        _JoinMenu.SetActive(false);
        //초기 게임 셋팅시 닉네임 랜덤 생성
        NickNameInput.text = "Player" + Random.Range(0, 1000).ToString("0000");
        //동기화 속도를 올림
        PhotonNetwork.SendRate = 60;

        PhotonNetwork.SerializationRate = 30;

        PhotonNetwork.GameVersion = this._gameVersion;

        //프로그램을 키면 바로 서버 마스터에 연결함
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("마스터에 연결중 입니다.");
        //마스터에 접속때까지 
        _lodingText.text = "Connect To Server..."; 
    }


    //ConnectUsingSettings -> 서버에 접속되면 콜백되는 함수
    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터에 연결하였습니다");
        // 마스터에 연결하면 UI를 이름을 입력할수 잇게 만듬.
        _lodingText.text = "JoinedLobby";

        //서버와 연결이끝나면 메뉴를 띄움
        _JoinMenu.SetActive(true);

        //로비에 접속하면 강제로 룸에 접속시킴
        PhotonNetwork.JoinOrCreateRoom("Room1", new RoomOptions()
        {
            MaxPlayers = 10
        }, null);

        
    }
    public override void OnJoinedRoom()
    {
        //방에 접속되면 텍스트 변경
        _lodingText.text = "JoinedRoom";
    }

    public void JoinButtonClick()
    {
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        //조인되면서 룸을 만듬

        Spwan();
        //로비 타이틀 끄기
        TitleUI.SetActive(false);

    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && PhotonNetwork.IsConnected)
        {
            //연결해제
            PhotonNetwork.Disconnect();
        }

    }
    //소환
    public void Spwan()
    {
        //리소스 폴더안에 있는 이름으로 정의해야함
        PhotonNetwork.Instantiate("Player",Vector3.zero,Quaternion.identity);
    }

    //연결호출 콜백
    public override void OnDisconnected(DisconnectCause cause)
    {
        TitleUI.SetActive(true);
    }

}
