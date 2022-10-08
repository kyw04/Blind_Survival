using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
//�� ����
public class ServerManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField NickNameInput;
    public GameObject TitleUI;

    private void Awake()
    {
        //�ʱ� ���� ���ý� �г��� ���� ����
        NickNameInput.text = "Player" + Random.Range(0, 1000).ToString("0000");
        //����ȭ �ӵ��� �ø�
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
    }

    //���ٽ� => ���� ������ ������ ������
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
        //�κ� Ÿ��Ʋ ����
        TitleUI.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && PhotonNetwork.IsConnected)
        {
            //��������
            PhotonNetwork.Disconnect();
        }

    }
    //����ȣ�� �ݹ�
    public override void OnDisconnected(DisconnectCause cause)
    {
        TitleUI.SetActive(true);
    }

}
