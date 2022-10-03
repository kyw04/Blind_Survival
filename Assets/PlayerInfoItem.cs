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

    //포톤에서 제공하는 Player 클래스 닉네임 접속방정보 아이디 등 제공함
    Player playerInfo;

    public void SetUp(Player _setPlayer)
    {
        playerInfo = _setPlayer;
        _playerNickName.text = _setPlayer.NickName;
    }

    //플레이어 또는 플레이어외 나갔을때 호출되는 됩니다.
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(playerInfo == otherPlayer)
        {
            Debug.Log($"플레이어 나감 {otherPlayer.NickName}");
            Destroy(gameObject);
        }
    }
    //방에서 완전히 나가면 방을 삭제함
    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }

}
