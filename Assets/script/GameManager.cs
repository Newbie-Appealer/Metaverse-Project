using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    protected override void InitManager()
    {
        //F_SetCursor(false);
    }

    public void F_SetCursor(bool v_mode)
    {
        Cursor.visible = false;

        if (v_mode)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }
}
