using Photon.Pun;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class TotalRanking : MonoBehaviourPun
{
    private string rankingTable = "ranking";

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerController>()._pv.IsMine)
            {
                UIManager.Instance.F_OnClear(true);

                int uid = AccountManager.Instance.playerUID;
                int time = RankingManager.Instance._localTime;
                photonView.RPC("F_AddRanking", RpcTarget.All, uid, time);
            }
        }
    }

    private bool F_SearchID(int v_uid)
    {
        string query_select = string.Format("SELECT * FROM {0} WHERE UID = '{1}'",
            rankingTable, v_uid);

        DataSet data = DBConnector.Instance.F_Select(query_select, rankingTable);

        if (data == null)
            return false;

        foreach(DataRow row in data.Tables[0].Rows)
            return true;

        return false;
    }

    [PunRPC]
    private void F_AddRanking(int uid, int time)
    {
        string query_insert = string.Format("INSERT INTO {0}(UID,TimeSecond) VALUES('{1}','{2}')",
            rankingTable, uid, time);

        if (!F_SearchID(uid))
        {
            if (DBConnector.Instance.F_Insert(query_insert))
            {
                RankingManager.Instance.F_AddTotalRanking();
            }
        }
    }
}
