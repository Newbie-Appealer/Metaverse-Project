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

    [Header("=== Login ===")]
    [SerializeField] private GameObject _login_Panel;

    [Header("=== Player ===")]
    [SerializeField] private Image _player_JumpGauge;
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

    //로그인 버튼 
    public void F_LoginBtn()
    {
        _login_Panel.SetActive(false);
    }

    public Image F_GetJumpGauge()
    {
        return _player_JumpGauge;
    }

}
