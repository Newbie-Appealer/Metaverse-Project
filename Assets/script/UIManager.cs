using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("=== FPS ===")]
    private float _deltaTime = 0f;
    private Rect _rect;
    private GUIStyle _style;

    [Header("=== Loding ===")]
    [SerializeField] private GameObject _loding_panel;

    [Header("=== Login ===")]
    [SerializeField] private GameObject _login_Panel;

    [Header("=== Player ===")]
    [SerializeField] private Image _player_JumpGauge;
    [SerializeField] private TextMeshProUGUI _playerNickName;
    protected override void InitManager()
    {
        _style = new GUIStyle();
        _style.fontSize = 32;
        _style.normal.textColor = Color.white;

        _rect = new Rect(20, 20, Screen.width, Screen.height);
    }
    private void Update()
    {
        //FPS 실시간 갱신
        _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
    }

    private void OnGUI()
    {
        float _fps = 1.0f / _deltaTime;
        string _text = string.Format("{0:0.} FPS", _fps);

        GUI.Label(_rect, _text, _style);
    }

    //로그인 UI On/Off
    public void F_OnLogin(bool v_state)
    {
        _login_Panel.SetActive(v_state);
    }

    public Image F_GetJumpGauge()
    {
        return _player_JumpGauge;
    }

    // 로딩 UI On/Off
    public void F_OnLoding(bool v_state)
    {
        _loding_panel.SetActive(v_state);

        if(v_state)
        {
            // 로딩 화면이 켜질때 동작 ( 로딩 중 )
        }
        else
        {
            // 로딩 화면이 꺼질때 동작 ( 로딩 완료 후 )
        }
    }
}
