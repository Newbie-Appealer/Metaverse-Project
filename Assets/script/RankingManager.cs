using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class RankingManager : Singleton<RankingManager>
{
    [SerializeField] private List<GameObject> _globalPlayers;               // 생성된 플레이어들의 오브젝트.
    [SerializeField] private List<Tuple<float,string>> _realTimeRanking;             // 실시간 랭킹 ( 정렬 )

    protected override void InitManager()
    {
        // 초기화
        _globalPlayers = new List<GameObject>();
        _realTimeRanking = new List<Tuple<float, string>>();

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
            yield return new WaitForSeconds(3f);
            _realTimeRanking.Clear();

            // 유저 데이터 초기화
            for (int i = 0; i < _globalPlayers.Count; i++)
            {
                if (_globalPlayers[i] == null)
                {
                    _globalPlayers.RemoveAt(i);
                    continue;
                }
                Tuple<float, string> tmpTuple = new Tuple<float, string>
                    (_globalPlayers[i].transform.position.y, _globalPlayers[i].name);
                _realTimeRanking.Add(tmpTuple);
            }

            _realTimeRanking.Sort
                (delegate (Tuple<float, string> a, Tuple<float, string> b)
                { return a.Item1.CompareTo(b.Item1); });

            foreach(Tuple<float,string> item in _realTimeRanking)
            {
                Debug.Log(item.Item1 + " / " + item.Item2);
            }
        }
    }

    private static bool CompareIntMethod(float a, float b)
    {
        return a < b;
    }
}