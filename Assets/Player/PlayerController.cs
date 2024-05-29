using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public delegate void PlayControllerDelegate();
    public PlayControllerDelegate playerController;


    [Header("=== Movement ===")]
    private Animator _man_Animator;
    private Rigidbody _rb;
    private Image _jump_Gauge;
    public bool _isGrounded = true;
    public bool _isCrashed = false;
    public bool _jumpIncrease = true;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpSpeed = 0f;
    
    

    [Header("=== Roration ===")]
    private Vector3 _rotationX;
    private float _rotationY;
    private float _mouseSensitivity = 300f;

    [Header("=== Object ===")]
    [SerializeField] private GameObject obj_Cam_First, obj_Cam_Quarter;
    [SerializeField] private GameObject obj_Rotate_Horizontal;
    [SerializeField] private GameObject obj_Body;


    [Header("=== nickname ===")]
    [SerializeField] private TextMeshProUGUI _nicknameText;
    [SerializeField] private string _nickname;

    private PhotonView _pv;


    [Header("Collisions")]
    [SerializeField] private OnGround _onGround;
    private void Start()
    {
        F_InitController();
    }

    public void F_InitController()
    {
        _onGround._playerController = this;
        _pv = GetComponent<PhotonView>();
        _rb = GetComponent<Rigidbody>();
        _man_Animator = GetComponent<Animator>();
        _jump_Gauge = UIManager.Instance.F_GetJumpGauge();

        PhotonNetwork.LocalPlayer.NickName = AccountManager.Instance.playerID;

        if (_pv.IsMine)
        {
            obj_Cam_First.SetActive(false);
            obj_Cam_Quarter.SetActive(true);
            this.gameObject.name += " - ( LOCAL )";
            this.transform.SetParent(GameManager.Instance._players);
            RankingManager.Instance.F_AddUser(this.gameObject);
        }
        else
        {
            obj_Cam_First.SetActive(false);
            obj_Cam_Quarter.SetActive(false);
            this.gameObject.name += " - ( OTHER )";
            this.transform.SetParent(GameManager.Instance._players);
            RankingManager.Instance.F_AddUser(this.gameObject);
        }
        StartCoroutine(C_SyncDelay());

        F_InitDelegate();
    }
    private void Update()
    {
        if(_pv.IsMine) 
            playerController();
    }
    #region 움직임 함수
    public void F_InitDelegate()
    {
            playerController += F_PlayerHorizonRotate;
            playerController += F_CameraVerticalMove;
            playerController += F_PlayerMove;

    }
    private void F_PlayerMove()
    {
        float _input_x = Input.GetAxis("Horizontal");
        float _input_z = Input.GetAxis("Vertical");
        Vector3 _moveVector;
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            if (_input_x > 0)
            {
                if (_input_z > 0)
                    obj_Body.transform.localEulerAngles = new Vector3(0f, 45f, 0f);
                else if (_input_z < 0)
                    obj_Body.transform.localEulerAngles = new Vector3(0f, 135f, 0f);
                else
                    obj_Body.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
            }
            else if (_input_x < 0)
            {
                if (_input_z > 0)
                    obj_Body.transform.localEulerAngles = new Vector3(0f, -45f, 0f);
                else if (_input_z < 0)
                    obj_Body.transform.localEulerAngles = new Vector3(0f, -135f, 0f);
                else
                    obj_Body.transform.localEulerAngles = new Vector3(0f, 270f, 0f);
            }
            else
            {
                if (_input_z > 0)
                    obj_Body.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                else if (_input_z < 0)
                    obj_Body.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            }

            //달리기
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _man_Animator.SetBool("Walk", false);
                _man_Animator.SetBool("Run", true);
                _moveSpeed = 10f;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                _man_Animator.SetBool("Run", false);
                _man_Animator.SetBool("Walk", true);
                _moveSpeed = 5f;
            }
            if (!_isCrashed)
            {
            _moveVector = (transform.right * _input_x + transform.forward * _input_z).normalized;
            _rb.MovePosition(transform.position + _moveVector * _moveSpeed * Time.deltaTime);
            }
            _man_Animator.SetBool("Walk", true);
        }
        else
        {
            _man_Animator.SetBool("Run", false);
            _man_Animator.SetBool("Walk", false);
        }
        if (_isGrounded)
        {
            F_PlayerJump();
        }
    }

    private void F_PlayerJump()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            F_JumpGaugeCharge();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            _isGrounded = false;
            if (_jumpSpeed < 5f) 
                _rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
            else
                _rb.AddForce(Vector3.up * _jumpSpeed, ForceMode.Impulse);

            _man_Animator.SetTrigger("Jump");
            _jumpSpeed = 0f;
            _jump_Gauge.fillAmount = 0f;
        }
    }

    private void F_JumpGaugeCharge()
    {
        if (_jumpIncrease)
        {
            _jumpSpeed += Time.deltaTime * 12f;
            _jump_Gauge.fillAmount = _jumpSpeed / 12f;
            if (_jumpSpeed >= 12f)
                _jumpIncrease = false;
        }

        else if (!_jumpIncrease)
        {
            _jumpSpeed -= Time.deltaTime * 12f;
            _jump_Gauge.fillAmount = _jumpSpeed / 12f;
            if (_jumpSpeed < 0.01f)
                _jumpIncrease = true;
        }
    }

    private void F_PlayerHorizonRotate()
    {
        float _mouseX = Input.GetAxisRaw("Mouse X");
        _rotationX = new Vector3(0, _mouseX, 0) * _mouseSensitivity * Time.deltaTime;
        _rb.MoveRotation(_rb.rotation * Quaternion.Euler(_rotationX));
    }
    private void F_CameraVerticalMove()
    {
        float _mouseY = Input.GetAxisRaw("Mouse Y");
        _rotationY -= _mouseY * _mouseSensitivity * Time.deltaTime;
        _rotationY = Mathf.Clamp(_rotationY, -150f, -30f);
        obj_Cam_Quarter.transform.parent.transform.localEulerAngles = new Vector3(_rotationY, 0, 0);
    }
    #endregion

    IEnumerator C_SyncDelay()
    {
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < GameManager.Instance._players.childCount; i++)
        {
            Transform player = GameManager.Instance._players.GetChild(i);
            string name = player.GetComponent<PhotonView>().Owner.NickName;
            player.GetComponent<PlayerController>()._nicknameText.text = name;
            player.gameObject.name = name;
        }
    }

    public Rigidbody F_GetRB()
    {
        return _rb;
    }
}