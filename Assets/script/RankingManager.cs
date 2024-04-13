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

    public List<GameObject> _globalPlayers;                                 // ������ �÷��̾���� ������Ʈ.
    [SerializeField] private List<RealTimeRankding> _realTimeRanking;       // �ǽð� ��ŷ ( ���ĵ� )
    [SerializeField] private List<RankSlot> _rankingSlots;                  // ��ŷ ����

    protected override void InitManager()
    {
        // �ʱ�ȭ
        _globalPlayers = new List<GameObject>();
        _realTimeRanking = new List<RealTimeRankding>();

        // �ǽð� ��ŷ �ڷ�ƾ ���� ( local )
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
            // 3�ʿ� �ѹ�
            yield return new WaitForSeconds(1f);
            _realTimeRanking.Clear();

            // ���� ������ �ʱ�ȭ
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