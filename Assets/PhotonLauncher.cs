using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; //server using
using TMPro;
using Photon.Realtime;
using Photon.Pun.Demo.Cockpit;
using System.Linq;
using UnityEngine.AI;
using Unity.VisualScripting.Antlr3.Runtime.Tree;


//serverManager
public class PhotonLauncher : MonoBehaviourPunCallbacks
{
    public static PhotonLauncher Instace;

    
    public TextMeshProUGUI DebugText;
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text RoomNameText;

    [Header("생성된 방")]
    [SerializeField] Transform roomListContent;//방리스트를 생성할 위치
    [SerializeField] GameObject roomListItemPrefab;//방을 만들어서 접속할때 사용할 버튼
    [SerializeField] GameObject startGameButton; //방장 :호스트만 게임을 시작할수 잇게함

    [Header("Player Info")]
    [SerializeField] TextMeshProUGUI PlayerNickName; //플레이어 닉네임
    [SerializeField] Transform playerListContent; //플레이어가 정보 노출 위치
    [SerializeField] GameObject playerLIstItemPrefab;//플레이어 정보를 담은 게임오브젝트


    private void Awake()
    {
        Instace = this;
    }
    private void Start()
    {
        Connect(); //초기에 포톤 온라인 서버에 연결을 합니다. 버전관리 플레이어 관리 등 여기서 진행하며
        //우리가 서버의 버전을 업그레이드할경우 구형버전은 접속할수 없게끔도 만들수있습니다.
    }
    public void Connect()
    {
        //Photon server Connecting
        Debug.Log("서버에 연결 합니다");
        DebugText.text = "Try Connect To Server..";
        PhotonNetwork.ConnectUsingSettings();
    }
    
    //서버에 성공적으로 연결되었을대 호출되는 콜백 메소드 입니다
    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터에 연결합니다. ");
        DebugText.text = "Connect To Master";
        PhotonNetwork.JoinLobby();
        //은 마스터 클라이언트와 일반 클라이언트들이 레벨을 동기화할지 결정한다. true로 설정하면 마스터 클라에서 LoadLevel()로 레벨을 변경하면 모든 클라이언트들이 자동으로 동일한 레벨을 로드.
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    //서버에 참여할 수 있는 권한 공동로비에서 공 성공적으로 서버에 연결후 로비에 연결되게 되면 장면을 이동 하게함
   
    public override void OnJoinedLobby()
    {
        Menumanager.Instace.OpenMenu("LobyTitleMenu"); //로비에 접속이되면 초기 메뉴 방찾기 방생성 버튼을 활성화 합니다.

        Debug.Log("로비에 접속");
        DebugText.text = "Joined Lobby";
      
        PhotonNetwork.NickName = "Player" + Random.Range(0, 1000).ToString("0000"); //자리수 자르고 로비에 접속시 닉네임을 강제 할당함

    }

    //방을 생성합니다.
    public void CreateRoom()
    {
        if(string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        Menumanager.Instace.OpenMenu("LodingMenu");
    }

    //Room Join
    public override void OnJoinedRoom()
    {
        Menumanager.Instace.OpenMenu("RoomMenu");
        RoomNameText.text = PhotonNetwork.CurrentRoom.Name;

        //임시 값을 저장
        Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;

        foreach (Transform _child in playerListContent)
        {
            Destroy(_child.gameObject);
        }

        for (int i = 0; i < players.Count(); i++)
        {
            Debug.Log("플레이어 생성 접속");
            Instantiate(playerLIstItemPrefab, playerListContent).GetComponent<PlayerInfoItem>().SetUp(players[i]);
        }
        //게임 버튼을 활성화하는데 마스터 클라이언트만 활성화되게 만들수 있음 
        //추후 게임 옵션에서 마스터클라이언트가 방 옵션 변경등에 활용가능
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    //만약 방장이 호스트를 떠나면 다른 남은 유저가 호스트를 맡게됨
    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    //방 생성 실패
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creat Failed;" + message;
        Debug.LogError("Room Creation Failed: " + message);
        Menumanager.Instace.OpenMenu("ErrorMenu");
    }
    //방 나가기
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        Menumanager.Instace.OpenMenu("LobyTitleMenu");
    }
    //룸에 접속합니다. 생성되어있는방에 접속합니다.
    public void JoinRoom(RoomInfo _info)
    {
        //로딩메뉴를 띄어 방에 접속하는 시간을 기다리게함
        PhotonNetwork.JoinRoom(_info.Name); //룸 정보를 통해 방에 접속하는 포톤 메소드
        Menumanager.Instace.OpenMenu("LodingMenu");
    }

    public override void OnLeftRoom()
    {
        Menumanager.Instace.OpenMenu("LobyTitleMenu");
    }
    /// <summary>
    /// 포톤에서 우리가 만든 룸정보를 보여주는 메소드 입니다.
    /// </summary>
    /// <param name="roomList"></param>
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(Transform _tr in roomListContent)
        {
            //방이 새로고침될때마다 생성되어있던 방을 삭제합니다.
            Destroy(_tr.gameObject);
        }

        //생성된 룸 정보만큼 카운트를 돌고 룸접속할수있게 생성함
        for(int i = 0; i<roomList.Count;i++)
        {
            if (roomList[i].RemovedFromList)
                continue;
            //객체를 복제함과 동시에 버튼에 룸정보를 셋팅하면서 초기화함
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    /// <summary>
    /// 플레이어가 방에 입장한 경우 호출
    /// </summary>
    /// <param name="newPlayer"></param>
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log($"방입장 {newPlayer.NickName}");
        //방에 입장시 정보를 셋팅합니다.
        Instantiate(playerLIstItemPrefab,playerListContent).GetComponent<PlayerInfoItem>().SetUp(newPlayer);
    }

    public void StartGame()
    {
        //1 은 유니티 빌드셋팅의 목록의 1번 신을 말함
        PhotonNetwork.LoadLevel(1);
    }
}
