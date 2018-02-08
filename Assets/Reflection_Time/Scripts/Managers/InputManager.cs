using UnityEngine;
using System.Collections;

/// <summary>
/// Class in charge of the input on different platforms.
/// </summary>
public class InputManager : MonoBehaviour {

    public delegate void InputAction(Vector3 pos);
    public static event InputAction OnPress;

    public delegate void InputAction2();
    public static event InputAction2 OnUnPress;
  	void Update ()
    {
        //Press
        if (Input.GetMouseButtonDown(0))
        {
            if (OnPress != null)
                OnPress(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        //UnPress
        else if (Input.GetMouseButtonUp(0))
        {
            if (OnUnPress != null)
                OnUnPress();
        }

        //Escaping
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.Escape();
        }
        
    }
}
