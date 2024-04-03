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

    [SerializeField] private List<GameObject> _globalPlayers;               // ������ �÷��̾���� ������Ʈ.
    [SerializeField] private List<RealTimeRankding> _realTimeRanking;       // �ǽð� ��ŷ ( ���� )

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
            yield return new WaitForSeconds(3f);
            _realTimeRanking.Clear();

            // ���� ������ �ʱ�ȭ
            for (int i = 0; i < _globalPlayers.Count; i++)
            {
                if (_globalPlayers[i] == null)
                {
                    _globalPlayers.RemoveAt(i);
                    continue;
                }
                RealTimeRankding tmpTuple = new RealTimeRankding(_globalPlayers[i].name, _globalPlayers[i].transform.position.y);
                _realTimeRanking.Add(tmpTuple);
            }

            _realTimeRanking.Sort (delegate (RealTimeRankding a, RealTimeRankding b) { return a.distance.CompareTo(b.distance); });
        }
    }
}