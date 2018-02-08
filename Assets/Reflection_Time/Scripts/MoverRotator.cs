using UnityEngine;
using System.Collections;


/// <summary>
/// In charge to move and rotate gameobject
/// </summary>
public class MoverRotator : MonoBehaviour {

    public bool isRotate;
    public bool isMovable;
    public float rotateSpeed;
    public float moveSpeed;

    public Transform endPos;

    private float startTime;
    private Vector3 startPos;
    void Start ()
    {
        startPos = transform.position;
    }
	
	
	void Update ()
    {
        if(isRotate)
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * 10*rotateSpeed);
        }

        if(isMovable)
        {
            startTime += Time.deltaTime * 2F;
            transform.position = Vector3.Lerp(startPos, endPos.position, (Mathf.Sin(startTime * moveSpeed + Mathf.PI / 2) + 1) / 2);
        }
    }
}
