using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingManager : Singleton<RankingManager>
{
    [SerializeField] private List<GameObject> _globalPlayers;               // 생성된 플레이어들의 오브젝트.

    protected override void InitManager()
    {
        _globalPlayers = new List<GameObject>();    
    }

    public void F_AddUser(GameObject v_object)
    {
        _globalPlayers.Add(v_object);
    }
    public void F_DelUser()
    {

    }
}
