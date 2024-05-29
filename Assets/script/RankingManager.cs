using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class RealTimeRankding
{
    public string name;
    public float distance;

    public RealTimeRankding(string name, float distance)
    {
        this.name = name;
        this.distance = distance;
    }
}

public class RankingManager : Singleton<RankingManager>
{
    private string rankingTable = "ranking";

    public int _localTime;

    public List<GameObject> _globalPlayers;                                 // 생성된 플레이어들의 오브젝트.
    [SerializeField] private List<RealTimeRankding> _realTimeRanking;       // 실시간 랭킹 ( 정렬된 )
    [SerializeField] private List<RankSlot> _rankingSlots;                  // 랭킹 슬롯

    [Header("Total Ranking")]
    [SerializeField] private List<TotalRankingSlot> _totalRankingSlots;           // 전체 랭킹 슬롯

    public PhotonView _pv;
    protected override void InitManager()
    {
        _localTime = 0;

        // 초기화
        _globalPlayers = new List<GameObject>();
        _realTimeRanking = new List<RealTimeRankding>();

        // 실시간 랭킹 코루틴 실행 ( local )
        StartCoroutine(C_RealTimeRankingSort());

        F_UpdateRanking();
    }

    public void F_AddUser(GameObject v_object)
    {
        _globalPlayers.Add(v_object);
    }

    IEnumerator C_RealTimeRankingSort()
    {
        while(true)
        {
            // 1초에 한번
            yield return new WaitForSeconds(1f);
            _localTime++;
            _realTimeRanking.Clear();

            // 유저 데이터 초기화
            for (int i = 0; i < _globalPlayers.Count; i++)
            {
                if (_globalPlayers[i] == null)
                {
                    _globalPlayers.RemoveAt(i);
                    continue;
                }
                RealTimeRankding playerData = new RealTimeRankding(_globalPlayers[i].name, _globalPlayers[i].transform.position.y);
                _realTimeRanking.Add(playerData);
            }

            _realTimeRanking.Sort (delegate (RealTimeRankding a, RealTimeRankding b) { return b.distance.CompareTo(a.distance); });

            F_UpdateRankingUI();
        }
    }

    public void F_UpdateRankingUI()
    {
        for (int i = 0; i < _rankingSlots.Count; i++)
        {
            _rankingSlots[i].gameObject.SetActive(false);

            if (_realTimeRanking.Count <= i)
                continue;

            _rankingSlots[i].gameObject.SetActive(true);
            _rankingSlots[i].F_SetRank(i + 1);
            _rankingSlots[i].F_SetName(_realTimeRanking[i].name);
            _rankingSlots[i].F_SetHeight(_realTimeRanking[i].distance);
        }
    }

    public void F_AddTotalRanking()
    {
        F_UpdateRanking();
        _pv.RPC("F_UpdateRanking", RpcTarget.Others);
    }

    [PunRPC]
    public void F_UpdateRanking()
    {
        string qurey = string.Format("SELECT * FROM {0} ORDER BY Rank ASC",
        rankingTable);

        DataSet data = DBConnector.Instance.F_Select(qurey, rankingTable);

        if (data == null)
            return;

        int dataCount = data.Tables[0].Rows.Count;

        for (int i = 0; i < _totalRankingSlots.Count; i++)
        {
            // RANK
            int rank = i + 1;

            if (dataCount > i)
            {
                DataRow row = data.Tables[0].Rows[i];


                // NAME
                string uid = row["UID"].ToString();
                string nickName = F_GetNickName(uid);

                // TIME
                string time = row["TimeSecond"].ToString();
                int timeSecond = int.Parse(time);

                _totalRankingSlots[i].F_UpdateSlot(rank, nickName, timeSecond);
            }
            else
            {
                _totalRankingSlots[i].F_UpdateSlot(rank, "", 0);
            }
        }
    }

    private string F_GetNickName(string v_uid)
    {
        string qurey_nickName = string.Format("SELECT ID FROM account WHERE UID = '{0}'",
                v_uid);

        DataSet data = DBConnector.Instance.F_Select(qurey_nickName, "account");

        if (data == null)
            return "";
        foreach(DataRow row in data.Tables[0].Rows)
        {
            string ID = row["ID"].ToString();
            return ID;
        }

        return "";
    }
}