using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Control cursor physics 
/// </summary>
public class Cursor : MonoBehaviour {

    public GameObject cursor;
    private List<Transform> dots = new List<Transform>();
    private Transform ballTrans,pointParent;
    private bool isPressed, isBallThrown, isMouseOnBall;
    BallBehaviour ballBehaviour;
    void Awake()
    {
        foreach (Transform child in cursor.transform)
        {
            if (child.name == "Ball")
            {
                ballTrans = child;
            }
            else if(child.name == "PointParent")
            {
                pointParent = child;
                foreach (Transform subChild in child)
                {
                    dots.Add(subChild);
                }
            }
        }
        ballBehaviour = ballTrans.GetComponent<BallBehaviour>();
    }
    void OnEnable()
    {
        InputManager.OnPress += Press;
        InputManager.OnUnPress += UnPress;
    }
    void OnDisable()
    {
        InputManager.OnPress -= Press;
        InputManager.OnUnPress -= UnPress;
    }

    void Start ()
    {
        cursor.SetActive(false);
    }	
	void Update ()
    {
        #region IsPressed
        if (isPressed)
        {
            //Debug.Log("<color=blue>IsPressed</color>");
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 vel = GetForceFrom(ballTrans.transform.position, mousePos);

            SetTrajectoryPoints(ballTrans.position, vel);
            if(MenuManager.Instance.IsMenuShown)
            {
                if (Vector2.Distance(mousePos, ballTrans.position) > BallBehaviour.BallRadius )
                {
                    MenuManager.Instance.MenuHide();
                }
            }
            
        }
        #endregion

        #region !IsPressed
        if (!isPressed && ballTrans.gameObject.activeInHierarchy)
        {
            if (!BallBehaviour.IsReflected)
            {
                foreach (Transform dot in dots)
                {
                    if (dot.gameObject.activeInHierarchy)
                    {
                        float dist = Vector2.Distance(ballTrans.position, dot.position);
                        if (dist < BallBehaviour.BallRadius / 2)  dot.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                foreach (Transform dot in dots)
                    dot.gameObject.SetActive(false);
            }
        }
        #endregion
    }

    /// <summary>
    /// Call when player press on screen
    /// </summary>
    /// <param name="posPress">Position of pressed point</param>
    void Press(Vector3 posPress)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //if press point is located above top menu edge 
        if (mousePos.y > MenuManager.Instance.PosMenuTopEdge.y)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.layer != LayerMask.NameToLayer(Constants.LayerHited))
                {
                    isPressed = true;
                    cursor.SetActive(true);
                    pointParent.gameObject.SetActive(true);
                    ballTrans.position = new Vector3(posPress.x, posPress.y, ballTrans.position.z);

                    isBallThrown = false;
                    ballBehaviour.Reset();

                    Vector3 vel = GetForceFrom(ballTrans.transform.position, mousePos);
                    SetTrajectoryPoints(ballTrans.position, vel);
                    foreach (Transform dot in dots)
                        dot.gameObject.SetActive(true);
                }
            }
        }
    }

    /// <summary>
    /// Call when player unpress mouse or finger
    /// </summary>
    void UnPress()
    {
        isPressed = false;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //if mouse pos is very close to, ball is not going to thrown and Menu panel appears
        if (Vector2.Distance(mousePos,ballTrans.position)< BallBehaviour.BallRadius/2)
        {
            UnityEngine.Debug.Log("<color=red>The same POS</color> ");
            GameManager.Instance.ResetLevel();

            cursor.SetActive(false);
            if(MenuManager.Instance.IsMenuShown)
            {
                if (mousePos.y > MenuManager.Instance.PosMenuTopEdge.y)
                    MenuManager.Instance.MenuHide();                
            }

            else
                MenuManager.Instance.MenuShow();             
        }

        //Call BallBehaviour script to thrown a ball
        else
        {
            if (!isBallThrown)
            {
                isBallThrown = true;
                ballBehaviour.Throw(mousePos);
            }
        }        
    }

    private Vector2 GetForceFrom(Vector3 fromPos, Vector3 toPos)
    {
        return (toPos - fromPos);
    }

    /// <summary>
    /// Following method displays projectile trajectory path..
    /// </summary>
    /// <param name="posStart">start position of object(ball)</param>
    /// <param name="pVelocity">initial velocity of object(ball)</param>
    void SetTrajectoryPoints(Vector3 posStart, Vector2 pVelocity)
    {
        float velocity = Mathf.Sqrt((pVelocity.x * pVelocity.x) + (pVelocity.y * pVelocity.y));
        float angle = Mathf.Rad2Deg * (Mathf.Atan2(pVelocity.y, pVelocity.x));
        float fTime = 0;

        fTime += 0.1f;
        foreach (Transform dot in dots)
        {
            float dx = velocity * fTime * Mathf.Cos(angle * Mathf.Deg2Rad);
            float dy = velocity * fTime * Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector3 pos = new Vector3(posStart.x + dx, posStart.y + dy, 2);
            dot.position = pos;
            dot.eulerAngles = new Vector3(0, 0, Mathf.Atan2(pVelocity.y, pVelocity.x));
            fTime += 0.1f;
        }
    }
}
