using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using TMPro;
using Photon.Pun;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text RoomName;

    //���濡�� �����ϴ� Ŭ����
    public RoomInfo roomInfo; //������� ���� ������ ��� ����

    //������ ���� �� �����ÿ� ������_
    public void SetUp(RoomInfo _roomInfo)
    {
        roomInfo = _roomInfo;
        RoomName.text = _roomInfo.Name; //������ ���̸��� ���ؽ�Ʈ�� �־���
    }

    //��ư�� �����ϴ� �޼ҵ� Ŭ�� -> ���深ó -> JoinRoom -> ����������� �Ѱ� ����
    public void OnClick()
    {
       PhotonLauncher.Instace.JoinRoom(roomInfo); //
    }
}
