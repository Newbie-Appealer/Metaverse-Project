using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    public List<GameObject> _globalPlayers;                                 // 생성된 플레이어들의 오브젝트.
    [SerializeField] private List<RealTimeRankding> _realTimeRanking;       // 실시간 랭킹 ( 정렬된 )
    [SerializeField] private List<RankSlot> _rankingSlots;                  // 랭킹 슬롯

    protected override void InitManager()
    {
        // 초기화
        _globalPlayers = new List<GameObject>();
        _realTimeRanking = new List<RealTimeRankding>();

        // 실시간 랭킹 코루틴 실행 ( local )
        StartCoroutine(C_RealTimeRankingSort());
    }

    public void F_AddUser(GameObject v_object)
    {
        _globalPlayers.Add(v_object);
    }

    IEnumerator C_RealTimeRankingSort()
    {
        while(true)
        {
            // 3초에 한번
            yield return new WaitForSeconds(1f);
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
        for(int i = 0; i < _rankingSlots.Count; i++)
        {
            _rankingSlots[i].gameObject.SetActive(false);

            if (_realTimeRanking.Count <= i)
                continue;

            _rankingSlots[i].gameObject.SetActive(true);
            _rankingSlots[i].F_SetRank(i + 1);
            _rankingSlots[i].F_SetName(_realTimeRanking[i].name);
            _rankingSlots[i].F_SetHeight(_realTimeRanking[i].distance );
        }
    }
}