using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; //server using
using TMPro;


//serverManager
public class PhotonLauncher : MonoBehaviourPunCallbacks
{


    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text RoomNameText;
    private void Start()
    {
        //Photon server Connecting
        Debug.Log("서버에 연결 합니.");
        PhotonNetwork.ConnectUsingSettings();
    }
    private void Update()
    {
        
    }
    //Method called after connecting to photon server
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connect To Master ");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    { 
        Debug.Log("Joined Loddy");
        Menumanager.Instace.OpenMenu("LobyTitleMenu");
    }
    /// <summary>
    /// I create my server in Photon Network.
    /// </summary>
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
        
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creat Failed;" + message;
        Menumanager.Instace.OpenMenu("ErrorMenu");
    }
    //방 나가
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        Menumanager.Instace.OpenMenu("LobyTitleMenu");
    }

}
