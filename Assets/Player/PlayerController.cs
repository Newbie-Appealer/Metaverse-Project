using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public delegate void PlayControllerDelegate();
    public PlayControllerDelegate playerController;

    [Header("=== Movement ===")]
    private Animator _man_Animator;
    private Rigidbody _rb;
    private bool _isGrounded = true;
    private float _moveSpeed = 5f;
    private float _jumpSpeed = 7f;

    [Header("=== Roration ===")]
    private Vector3 _rotationX;
    private float _rotationY;
    private float _mouseSensitivity = 300f;

    [Header("=== Object ===")]
    [SerializeField] private GameObject obj_Cam_First, obj_Cam_Quarter;
    [SerializeField] private GameObject obj_Rotate_Horizontal;
    [SerializeField] private GameObject obj_Body;

    private PhotonView _pv;

    private void Start()
    {
        F_InitController();
    }
    public void F_InitController()
    {
        _pv = GetComponent<PhotonView>();
        _rb = GetComponent<Rigidbody>();
        _man_Animator = GetComponent<Animator>();

        if (_pv.IsMine)
        {
            obj_Cam_First.SetActive(false);
            obj_Cam_Quarter.SetActive(true);
            this.gameObject.name += "(LocalPlayer)";
            RankingManager.Instance.F_AddUser(this.gameObject);
        }
        else
        {
            obj_Cam_First.SetActive(false);
            obj_Cam_Quarter.SetActive(false);
            this.gameObject.name += "(OtherPlayer)";
            RankingManager.Instance.F_AddUser(this.gameObject);
        }

        F_InitDelegate();
    }

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
         _isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.25f);
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

            //´Þ¸®±â
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

            _moveVector = (transform.right * _input_x + transform.forward * _input_z).normalized;
            _rb.MovePosition(transform.position + _moveVector * _moveSpeed * Time.deltaTime);
            _man_Animator.SetBool("Walk", true);
        }
        else
        {
            _man_Animator.SetBool("Run", false);
            _man_Animator.SetBool("Walk", false);
        }
        if (_isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                F_PlayerJump();
        }
    }

    private void F_PlayerJump()
    {
        _rb.AddForce(Vector3.up * _jumpSpeed, ForceMode.Impulse);
        _man_Animator.SetTrigger("Jump");
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
        _rotationY = Mathf.Clamp(_rotationY, 80f, 130f);
        obj_Cam_Quarter.transform.localEulerAngles = new Vector3(_rotationY, 0, 0);
    }
    
    
    private void Update()
    {
        if(_pv.IsMine) 
            playerController();
    }
}