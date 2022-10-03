using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System;

public class PlayerInfoItem : MonoBehaviourPunCallbacks
{
    [SerializeField] TextMeshProUGUI _playerNickName; 

    //���濡�� �����ϴ� Player Ŭ���� �г��� ���ӹ����� ���̵� �� ������
    Player playerInfo;

    public void SetUp(Player _setPlayer)
    {
        playerInfo = _setPlayer;
        _playerNickName.text = _setPlayer.NickName;
    }

    //�÷��̾� �Ǵ� �÷��̾�� �������� ȣ��Ǵ� �˴ϴ�.
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(playerInfo == otherPlayer)
        {
            Debug.Log($"�÷��̾� ���� {otherPlayer.NickName}");
            Destroy(gameObject);
        }
    }
    //�濡�� ������ ������ ���� ������
    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }

}
