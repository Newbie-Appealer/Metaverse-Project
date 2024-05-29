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
    [SerializeField] private TMP_InputField _login_inputField_ID;
    [SerializeField] private TMP_InputField _login_inputField_PW;

    [Header("=== Register Field ===")]
    [SerializeField] private TMP_InputField _register_inputField_ID;
    [SerializeField] private TMP_InputField _register_inputField_PW;
    [SerializeField] private TMP_InputField _register_inputField_Comfirm;

    [Header("=== Buttons ===")]
    [SerializeField] private Button _loginButton;           
    [SerializeField] private Button _onRegisterButton;         
    [SerializeField] private Button _exitButton_login;       
    [SerializeField] private Button _registerButton;        
    [SerializeField] private Button _exitButton_register;       

    [Header("=== Player Information ===")]
    [SerializeField] private string _playerID = string.Empty;
    [SerializeField] private string _playerPW = string.Empty;
    [SerializeField] private int _playerUID = -1;

    public string playerID => _playerID;
    public int playerUID => _playerUID;

    private string _accountTable = "account";
    protected override void InitManager() 
    {
        // �α��� UI ��ư
        _loginButton.onClick.AddListener(F_Login);
        _onRegisterButton.onClick.AddListener(() => F_OnRegister(true));
        _exitButton_login.onClick.AddListener(() => Application.Quit());

        // ȸ������ UI ��ư
        _registerButton.onClick.AddListener(F_Register);
        _exitButton_register.onClick.AddListener(() => F_OnRegister(false));
    }

    #region Cavans
    private void F_OnRegister(bool v_bValue)
    {
        F_initLoginInputField();
        F_InitRegisterInputField();
        UIManager.Instance.F_OnRegister(v_bValue);
    }
    #endregion

    #region Login
    private void F_Login()
    {
        string id = _login_inputField_ID.text;
        string pw = _login_inputField_PW.text;
        // �α��� �ڵ�
        if (F_TryLogin(id, pw))
        {
            // �α��� ����������
            UIManager.Instance.F_OnLoading(true);        // �ε� ���� ( ���� ���� )
            UIManager.Instance.F_OnLogin(false);        // �α��� �Ϸ� -> �α��� UI Off
            onJoinServer();                             // ���� ���� �õ�
        }
        // �α��� ����������
    }

    private bool F_TryLogin(string v_id, string v_pw)
    {
        F_initLoginInputField();        // �Է� �ʱ�ȭ

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
        UIManager.Instance.F_OnPopup(true, "Login Failed");
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

        F_InitRegisterInputField();             // input Field �ʱ�ȭ

        if (!F_CheckRegister(id, pw, comfirm))  // ������ִ� ���̵����� Ȯ��
            return;

        if(F_RegisterAccount(id,pw))            // ���̵� ����� �õ�  ( insert )
        {
            UIManager.Instance.F_OnRegister(false); // ȸ������ UI OFF
            UIManager.Instance.F_OnPopup(true, "Register Successfully");
        }
    }

    /// <summary> ȸ������ Insert �Լ� </summary>
    private bool F_RegisterAccount(string v_id, string v_pw)
    {
        string query = string.Format("INSERT INTO {0}(ID,PW) VALUES('{1}','{2}')"
            , _accountTable, v_id, v_pw);
        if(DBConnector.Instance.F_Insert(query))
        {
            Debug.Log("��� ����");
            return true;
        }
        return false;
    }

    /// <summary> ���̵� ������ִ��� Ȯ���ϴ� �Լ�</summary>
    private bool F_CheckRegister(string v_id, string v_pw, string v_comfirm)
    {
        if(v_id.Length == 0 || v_id.Length > 15)
        {
            UIManager.Instance.F_OnPopup(true, "Fail");
            return false;
        }

        if(v_pw.Length == 0 || v_pw.Length > 20)
        {
            UIManager.Instance.F_OnPopup(true, "Fail");
            return false;
        }

        if(v_pw != v_comfirm)
        {
            UIManager.Instance.F_OnPopup(true, "Fail");
            return false;
        }

        if(F_SearchID(v_id))
        {
            UIManager.Instance.F_OnPopup(true, "Fail");
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

    /// <summary> ���̵� �����ϴ��� Ȯ���ϴ� �Լ�</summary>
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
}
