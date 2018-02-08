using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuBackup : MonoBehaviour {
	
	public bool start = false;

    public Camera camera1;
	public Camera camera2;

	void OnMouseDown()
    {
		if( start )
        {
            camera1.enabled = true;
            camera2.enabled = false;
        }
    }
}
