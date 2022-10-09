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
    public TMP_InputField NickNameInput;
    public GameObject TitleUI;

    private void Awake()
    {
        //초기 게임 셋팅시 닉네임 랜덤 생성
        NickNameInput.text = "Player" + Random.Range(0, 1000).ToString("0000");
        //동기화 속도를 올림
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
    }

    //람다식 => 포톤 서버에 연결을 진행함
    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions()
        {
            MaxPlayers = 10
        }, null);
    }
    public override void OnJoinedRoom()
    {
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
    //연결호출 콜백
    public override void OnDisconnected(DisconnectCause cause)
    {
        TitleUI.SetActive(true);
    }

}
