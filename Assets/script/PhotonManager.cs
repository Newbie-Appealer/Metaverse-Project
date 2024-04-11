using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private List<GameObject> _PhotonPrefabs;       // �÷��̾� ������
    [SerializeField] Transform _spawnPoint;                         // �÷��̾� ���� ��ġ

    [Header("infomation")]
    [SerializeField] GameObject _localPlayerObject;                 // ������ �÷��̾� ������Ʈ
    [SerializeField] Dictionary<int, GameObject> _playerObjects;
    private void Start()
    {
        _playerObjects = new Dictionary<int, GameObject>();

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
        // ���� ���� ���������� ���� �߰��ؾ��ҵ�?
    }

    private void F_CreatePlayer()
    {
        _localPlayerObject = PhotonNetwork.Instantiate(_PhotonPrefabs[0].name, _spawnPoint.position, Quaternion.identity);
        PhotonNetwork.LocalPlayer.NickName = AccountManager.Instance.playerID;
    }

    // ���� ������ ���� �ݹ�
    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        PhotonNetwork.JoinLobby();
    }

    // �κ� �ݹ�
    public override void OnJoinedLobby()
    {
        Debug.Log("Photon : OnJoinedLobby");
        F_JoinRoom();
    }

    // �� ����
    private void F_JoinRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 16;
        PhotonNetwork.JoinOrCreateRoom("OnlyUP_metaverse", roomOptions, TypedLobby.Default);
    }
    public override void OnJoinedRoom()
    {
        // �� ����
        Debug.Log("Photon : OnJoinedRoom");
        F_CreatePlayer();

        UIManager.Instance.F_OnLoding(false);       // ĳ���� ���� �Ϸ� -> �ε� �Ϸ�
    }


    public override void OnCreatedRoom()
    {
        // �� ����
        Debug.Log("Photon : OnCreatedRoom");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        // �� ���� ����
        Debug.Log("Photon : OnCreateRoomFailed returnCode : " + returnCode + ", message : " + message);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // �� ���� ����
        Debug.Log("Photon : OnJoinRoomFailed returnCode : " + returnCode + ", message : " + message);
    }
    public override void OnLeftRoom()
    {
        // ����
        Debug.Log("Photon : OnLeftRoom");
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // ���� ����������
        Debug.Log("Photon : OnPlayerEnteredRoom newPlayer : " + newPlayer.UserId);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // ���� ����������
        Debug.Log("Photon : OnPlayerLeftRoom otherPlayer : " + otherPlayer.UserId);
    }
}

