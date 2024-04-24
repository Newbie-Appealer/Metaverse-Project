using Photon.Pun.Demo.PunBasics;
using System;
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
    [SerializeField] Button _exitButton_login;        // 회원가입 버튼
    [SerializeField] Button _exitButton_register;        // 회원가입 버튼

    [Header("=== Player Information ===")]
    [SerializeField] private string _playerID = string.Empty;
    [SerializeField] private string _playerPW = string.Empty;
    [SerializeField] private int _playerUID = -1;

    public string playerID => _playerID;

    private string _accountTable = "account";
    protected override void InitManager() 
    {
        _loginButton.onClick.AddListener(F_Login);
        //_registerButton.onClick.AddListener(F_Register);
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
            UIManager.Instance.F_OnLoading(true);        // 로딩 시작 ( 서버 연결 )
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
        string comfirm = _register_inputField_Comfirm.text;

        F_InitRegisterInputField();             // input Field 초기화

        if (!F_CheckRegister(id, pw, comfirm))  // 만들수있는 아이디인지 확인
            return;

        if(F_RegisterAccount(id,pw))            // 아이디 만들기 시도  ( insert )
        {
            // 1. 회원가입 성공 팝업
        }
    }
    
    /// <summary> 회원가입 Insert 함수 </summary>
    private bool F_RegisterAccount(string v_id, string v_pw)
    {
        string query = string.Format("INSERT INTO {0}(ID,PW) VALUES('{1}','{2}')"
            , _accountTable, v_id, v_pw);
        if(DBConnector.Instance.F_Insert(query))
        {
            Debug.Log("등록 성공");
            return true;
        }
        return false;
    }

    /// <summary> 아이디를 만들수있는지 확인하는 함수</summary>
    private bool F_CheckRegister(string v_id, string v_pw, string v_comfirm)
    {
        if(v_id.Length == 0 || v_id.Length > 15)
        {
            Debug.Log("아이디 비어있음 or 너무 김");
            return false;
        }

        if(v_pw.Length == 0 || v_pw.Length > 20)
        {
            Debug.Log("비밀번호 비어있음 or 너무 김");
            return false;
        }

        if(v_pw != v_comfirm)
        {
            Debug.Log("비밀번호 다름");
            return false;
        }

        if(F_SearchID(v_id))
        {
            Debug.Log("아이디가 이미 존재함");
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

    /// <summary> 아이디가 존재하는지 확인하는 함수</summary>
    private bool F_SearchID(string v_id)
    {
        string query = string.Format("SELECT * FROM {0} WHERE ID = '{1}'",
            _accountTable, v_id);

        DataSet data = DBConnector.Instance.F_Select(query, _accountTable);

        if (data == null)
            return false;

        foreach (DataRow row in data.Tables[0].Rows)
            return true;

        return false;
    }


    // #TODO:회원가입UI
    // 1. 로그인창에서 회원가입 누르면 회원가입 UI ON / 로그인 UI OFF
    // 2. 회원가입창에서 EXIT 누르면 회원가입 UI OFF / 로그인 UI ON
    // 3. 로그인/회원가입 팝업 만들기
}
