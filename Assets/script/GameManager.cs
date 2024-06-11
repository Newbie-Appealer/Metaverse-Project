using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Transform _players;
    public GameObject _player;
    protected override void InitManager()
    {
    }
    public void F_SetCursor(bool v_mode)
    {
        Cursor.visible = v_mode;

        if (v_mode)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }

}
