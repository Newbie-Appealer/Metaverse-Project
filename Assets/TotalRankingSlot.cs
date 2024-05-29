using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TotalRankingSlot : MonoBehaviour
{
    public TextMeshProUGUI _rank;
    public TextMeshProUGUI _nickName;
    public TextMeshProUGUI _time;

    public void F_UpdateSlot(int v_rank, string v_nickName, int v_time)
    {
        _rank.text = v_rank.ToString();
        _nickName.text = v_nickName.ToString();

        int s, m, h;

        h = v_time / 3600;  // 12050 / 3600 -> 3
        v_time = v_time - (h * 3600); // 3 * 3600 - vtime -> 1250

        m = v_time / 60;   // 1250 / 60 -> 20
        v_time = v_time - (m * 60);   // 20 * 60 - vtime -> 50

        s = v_time;

        string h_string = string.Format("{0:00}", h);
        string m_string = string.Format("{0:00}", m);
        string s_string = string.Format("{0:00}", s);

        _time.text = string.Format("{0}:{1}:{2}",
            h_string,m_string,s_string);
    }
}
