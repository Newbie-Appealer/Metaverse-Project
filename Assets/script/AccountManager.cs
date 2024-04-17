using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccountManager : Singleton<AccountManager>
{
    public delegate void LoginDelegate();
    public LoginDelegate onJoinServer;

    [Header("=== Login Field ===")]
    [SerializeField] TMP_InputField _login_inputField_ID;
    [SerializeField] TMP_InputField _login_inputField_PW;

    [Header("=== Register Field ===")]
    [SerializeField] TMP_InputField _register_inputField_ID;
    [SerializeField] TMP_InputField _register_inputField_PW;
    [SerializeField] TMP_InputField _register_inputField_Comfirm;

    [Header("=== Buttons ===")]
    [SerializeField] Button _loginButton;           // 로그인 버튼
    [SerializeField] Button _registerButton;        // 회원가입 버튼

    [Header("=== Player Information ===")]
    [SerializeField] private string _playerID = string.Empty;
    [SerializeField] private string _playerPW = string.Empty;
    [SerializeField] private int _playerUID = -1;

    public string playerID => _playerID;

    private string _accountTable = "account";
    protected override void InitManager() 
    {
        _loginButton.onClick.AddListener(F_Login);
        _registerButton.onClick.AddListener(F_Register);
    }

    #region Login
    private void F_Login()
    {
        string id = _login_inputField_ID.text;
        string pw = _login_inputField_PW.text;
        // 로그인 코드
        if (F_TryLogin(id, pw))
        {
            // 로그인 성공했을때
            UIManager.Instance.F_OnLoding(true);        // 로딩 시작 ( 서버 연결 )
            UIManager.Instance.F_OnLogin(false);        // 로그인 완료 -> 로그인 UI Off
            onJoinServer();                             // 서버 접속 시도
        }
        // 로그인 실패했을때
    }

    private bool F_TryLogin(string v_id, string v_pw)
    {
        F_initLoginInputField();        // 입력 초기화

        string qurey = string.Format("SELECT * FROM {0} WHERE ID = '{1}'"
            ,_accountTable, v_id);
        DataSet data = DBConnector.Instance.F_Select(qurey, _accountTable);

        if (data == null)
            return false;
        foreach (DataRow row in data.Tables[0].Rows)
        {
            string uid = row["UID"].ToString();
            string id = row["ID"].ToString();
            string pw = row["PW"].ToString();

            if (id == v_id && pw == v_pw)
            {
                _playerUID = int.Parse(uid);
                _playerID = id;
                _playerPW = pw;
                return true;
            }
        }
        return false;
    }

    private void F_initLoginInputField()
    {
        _login_inputField_ID.text = "";
        _login_inputField_PW.text = "";
    }
    #endregion

    #region register
    private void F_Register()
    {
        string id = _register_inputField_ID.text;
        string pw = _register_inputField_PW.text;
        string confirm = _register_inputField_Comfirm.text;

        F_InitRegisterInputField();

        if (!F_CheckRegister(id, pw, confirm))
            return;
    }

    private bool F_CheckRegister(string v_id, string v_pw, string v_comfirm)
    {
        if (v_id.Length < 1 || 15 < v_id.Length)
        {
            if (v_id.Length == 0)
                Debug.Log("ID Empty");
            else
                Debug.Log("ID Length Error ( 1 ~ 15 )");
            return false;
        }
        if (v_pw.Length < 4 || 25 < v_pw.Length)
        {
            if (v_pw.Length == 0 || v_pw.Length == 0)
                Debug.Log("PW Empty");
            else
                Debug.Log("PW Length Error ( 4 ~ 25 )");
            return false;
        }
        if (v_pw != v_comfirm)
        {
            Debug.Log("Password Error");
            return false;
        }
        if (F_SearchAccount(v_id))
        {
            Debug.Log("ID already exists");
            return false;
        }

        return true;
    }

    private void F_InitRegisterInputField()
    {
        _register_inputField_ID.text = "";
        _register_inputField_PW.text = "";
        _register_inputField_Comfirm.text = "";
    }
    #endregion

    private bool F_SearchAccount(string v_id)
    {
        string query = string.Format("SELECT * FROM {0} WHERE ID = '{1}'",
    _accountTable, v_id);

        DataSet data = DBConnector.Instance.F_Select(query, _accountTable);

        if (data == null)
            return false;

        // 데이터가 있으면 true 리턴
        foreach(DataRow row in data.Tables[0].Rows) 
        {
            return true;
        }

        return false;
    }
}
