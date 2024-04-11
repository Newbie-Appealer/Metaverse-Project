using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private List<GameObject> _PhotonPrefabs;       // 플레이어 프리팹
    [SerializeField] Transform _spawnPoint;                         // 플레이어 생성 위치

    [Header("infomation")]
    [SerializeField] GameObject _localPlayerObject;                 // 생성된 플레이어 오브젝트

    private void Start()
    {

        DefaultPool pool = PhotonNetwork.PrefabPool as DefaultPool;
        pool.ResourceCache.Clear();

        if(pool != null && _PhotonPrefabs != null)
        {
            foreach(GameObject prefab in _PhotonPrefabs)
            {
                pool.ResourceCache.Add(prefab.name, prefab);
            }
        }

        AccountManager.Instance.onJoinServer += F_InitPhotonServer;
    }

    private void F_InitPhotonServer()
    {
        if(!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("ConnectUsingSettings");
        }

        // 서버 접속 실패했을때 동작 추가해야할듯?
    }

    private void F_CreatePlayer()
    {
        _localPlayerObject = PhotonNetwork.Instantiate(_PhotonPrefabs[0].name, _spawnPoint.position, Quaternion.identity);
        _localPlayerObject.name = AccountManager.Instance.playerID;

        _localPlayerObject.GetComponent<PlayerController>().F_UpdateNickName();
    }

    // 포톤 마스터 서버 콜백
    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        PhotonNetwork.JoinLobby();
    }

    // 로비 콜백
    public override void OnJoinedLobby()
    {
        Debug.Log("Photon : OnJoinedLobby");
        F_JoinRoom();
    }

    // 방 입장
    private void F_JoinRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 16;
        PhotonNetwork.JoinOrCreateRoom("OnlyUP_metaverse", roomOptions, TypedLobby.Default);
    }
    public override void OnJoinedRoom()
    {
        // 방 입장
        Debug.Log("Photon : OnJoinedRoom");
        F_CreatePlayer();

        UIManager.Instance.F_OnLoding(false);       // 캐릭터 생성 완료 -> 로딩 완료
    }


    public override void OnCreatedRoom()
    {
        // 방 만듬
        Debug.Log("Photon : OnCreatedRoom");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        // 방 생성 실패
        Debug.Log("Photon : OnCreateRoomFailed returnCode : " + returnCode + ", message : " + message);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // 방 입장 실패
        Debug.Log("Photon : OnJoinRoomFailed returnCode : " + returnCode + ", message : " + message);
    }
    public override void OnLeftRoom()
    {
        // 퇴장
        Debug.Log("Photon : OnLeftRoom");
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // 유저 입장했을때
        Debug.Log("Photon : OnPlayerEnteredRoom newPlayer : " + newPlayer.UserId);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // 유저 퇴장했을때
        Debug.Log("Photon : OnPlayerLeftRoom otherPlayer : " + otherPlayer.UserId);
    }
}

