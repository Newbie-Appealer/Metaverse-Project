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

    [Header("=== Loading ===")]
    [SerializeField] private GameObject _loading_panel;
    [SerializeField] private TextMeshProUGUI _loading_Text;

    [Header("=== Register ===")]
    [SerializeField] private GameObject _register_panel;

    [Header("=== Login ===")]
    [SerializeField] private GameObject _login_Panel;

    [Header("=== Popup ===")]
    [SerializeField] private GameObject _popup;
    [SerializeField] private TextMeshProUGUI _popupTEXT;
    [SerializeField] private Button _popupButton;

    [Header("=== Player ===")]
    [SerializeField] private Image _player_JumpGauge;

    protected override void InitManager()
    {
        _style = new GUIStyle();
        _style.fontSize = 32;
        _style.normal.textColor = Color.white;

        _rect = new Rect(20, 20, Screen.width, Screen.height);

        _popupButton.onClick.AddListener(() => F_OnPopup(false));
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
        _login_Panel.transform.parent.gameObject.SetActive(v_state);
    }
    
    //회원가입 UI On/Off
    public void F_OnRegister(bool v_state)
    {
        _register_panel.SetActive(v_state);
        _login_Panel.SetActive(!v_state);
    }

    public void F_OnPopup(bool v_state, string v_text = "")
    {
        _popup.gameObject.SetActive(v_state);
        _popupTEXT.text = v_text;
    }

    public Image F_GetJumpGauge()
    {
        return _player_JumpGauge;
    }

    // 로딩 UI On/Off
    public void F_OnLoading(bool v_state)
    {
        _loading_panel.SetActive(v_state);

        if(v_state)
        {
            // 로딩 화면이 켜질때 동작 ( 로딩 중 )
            StartCoroutine(C_LoadingText());
        }
        else
        {
            // 로딩 화면이 꺼질때 동작 ( 로딩 완료 후 )
            StopCoroutine(C_LoadingText());
        }
    }

    private IEnumerator C_LoadingText()
    {
         _loading_Text.text = "Loading";
        while(_loading_panel.activeSelf)
        {
            _loading_Text.text += " .";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
