using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using TMPro;
using Photon.Pun;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text RoomName;

    //포톤에서 제공하는 클래스
    public RoomInfo roomInfo; //룸생성시 룸의 정보를 담고 있음

    //룸정보 셋팅 룸 생성시에 룸정보_
    public void SetUp(RoomInfo _roomInfo)
    {
        roomInfo = _roomInfo;
        RoomName.text = _roomInfo.Name; //생성된 룸이름을 룸텍스트에 넣어줌
    }

    //버튼에 연결하는 메소드 클릭 -> 포톤런처 -> JoinRoom -> 포톤방정보를 넘겨 접속
    public void OnClick()
    {
       PhotonLauncher.Instace.JoinRoom(roomInfo); //
    }
}
