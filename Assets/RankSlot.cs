using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _rankText;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _heightText;

    public void F_SetRank(int v_rank)
    {
        _rankText.text = v_rank.ToString();
    }

    public void F_SetName(string v_name)
    {
        _nameText.text = v_name;
    }

    public void F_SetHeight(float v_height)
    {
        _heightText.text = (int)v_height + " M";
    }
}
