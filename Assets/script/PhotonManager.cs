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
    [SerializeField] List<GameObject> _globalPlayers;               // 생성된 플레이어들의 오브젝트.
    private void Start()
    {
        _globalPlayers = new List<GameObject>();

        DefaultPool pool = PhotonNetwork.PrefabPool as DefaultPool;
        pool.ResourceCache.Clear();

        if(pool != null && _PhotonPrefabs != null)
        {
            foreach(GameObject prefab in _PhotonPrefabs)
            {
                pool.ResourceCache.Add(prefab.name, prefab);
            }
        }

        F_InitPhotonServer();
    }

    private void F_InitPhotonServer()
    {
        if(!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("ConnectUsingSettings");
        }
    }

    private void F_CreatePlayer()
    {
        _localPlayerObject = PhotonNetwork.Instantiate(_PhotonPrefabs[0].name, _spawnPoint.position, Quaternion.identity);
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
        Debug.Log("Photon : OnJoinedRoom");
        F_CreatePlayer();
    }


    public override void OnCreatedRoom()
    {
        Debug.Log("Photon : OnCreatedRoom");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Photon : OnCreateRoomFailed returnCode : " + returnCode + ", message : " + message);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Photon : OnJoinRoomFailed returnCode : " + returnCode + ", message : " + message);
    }
    public override void OnLeftRoom()
    {
        Debug.Log("Photon : OnLeftRoom");
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Photon : OnPlayerEnteredRoom newPlayer : " + newPlayer.UserId);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Photon : OnPlayerLeftRoom otherPlayer : " + otherPlayer.UserId);
    }
}

