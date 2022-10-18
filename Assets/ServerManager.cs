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
    //�ε��� �ؽ�Ʈ ���
    [SerializeField] TextMeshProUGUI _lodingText;
    [SerializeField] GameObject _JoinMenu;
    [SerializeField] readonly string _gameVersion = "1.0";

    public TMP_InputField NickNameInput;
    
    public GameObject TitleUI;

    //���� ����� �ٷ� �����ϰ���
    private void Awake()
    {
        Init();

    }


    private void Init()
    {
        //������ �������϶��� ����
        _JoinMenu.SetActive(false);
        //�ʱ� ���� ���ý� �г��� ���� ����
        NickNameInput.text = "Player" + Random.Range(0, 1000).ToString("0000");
        //����ȭ �ӵ��� �ø�
        PhotonNetwork.SendRate = 60;

        PhotonNetwork.SerializationRate = 30;

        PhotonNetwork.GameVersion = this._gameVersion;

        //���α׷��� Ű�� �ٷ� ���� �����Ϳ� ������
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("�����Ϳ� ������ �Դϴ�.");
        //�����Ϳ� ���Ӷ����� 
        _lodingText.text = "Connect To Server..."; 
    }


    //ConnectUsingSettings -> ������ ���ӵǸ� �ݹ�Ǵ� �Լ�
    public override void OnConnectedToMaster()
    {
        Debug.Log("�����Ϳ� �����Ͽ����ϴ�");
        // �����Ϳ� �����ϸ� UI�� �̸��� �Է��Ҽ� �հ� ����.
        _lodingText.text = "JoinedLobby";

        //������ �����̳����� �޴��� ���
        _JoinMenu.SetActive(true);

        //�κ� �����ϸ� ������ �뿡 ���ӽ�Ŵ
        PhotonNetwork.JoinOrCreateRoom("Room1", new RoomOptions()
        {
            MaxPlayers = 10
        }, null);

        
    }
    public override void OnJoinedRoom()
    {
        //�濡 ���ӵǸ� �ؽ�Ʈ ����
        _lodingText.text = "JoinedRoom";
    }

    public void JoinButtonClick()
    {
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        //���εǸ鼭 ���� ����

        Spwan();
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
    //��ȯ
    public void Spwan()
    {
        //���ҽ� �����ȿ� �ִ� �̸����� �����ؾ���
        PhotonNetwork.Instantiate("Player",Vector3.zero,Quaternion.identity);
    }

    //����ȣ�� �ݹ�
    public override void OnDisconnected(DisconnectCause cause)
    {
        TitleUI.SetActive(true);
    }

}
