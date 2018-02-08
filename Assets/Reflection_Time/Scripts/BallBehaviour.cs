using UnityEngine;
//**Любой геймобжект может быть шаром
public class BallBehaviour :MonoBehaviour {

    public float speed = 2;
    public bool CanMove { get; set; }
    public static float BallRadius { get; private set; }
    public static bool IsReflected { get; private set; }

    private Collider2D coll;
    private TrailRenderer trailRen;
    private RaycastHit2D hit;

    //Прицеливание игроком (Рисуем шары, вектор направления, разрешаем движение шару по вектору направлению)
    private Vector3 direction;

    void Awake()
    {
        BallRadius = Tools.TrueSize(transform).x;
        trailRen = transform.GetChild(0).GetComponent<TrailRenderer>();
        coll = GetComponent<Collider2D>(); 
    }
    void Start()
    {
        coll.enabled = false;
        direction = new Vector3(speed * Time.deltaTime, 0, 0);
    }

    void Update()
    {
        if(CanMove)
        {
            transform.Translate(direction);
        }
    }

    //При перезагрузке уровня, сбрасываем свойства шара
    public void Reset()
    {
        IsReflected = false;
        coll.enabled = false;
        CanMove = false;
        trailRen.Clear();
        trailRen.enabled = false;

        GameManager.Instance.ResetLevel();
    }
    //При прицеливании рисуем вектор
    public void Throw(Vector3 point)
    {
        trailRen.enabled = true;
        coll.enabled = true;
        Vector2 diff = (point - transform.position).normalized;

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

        CanMove = true;
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        //Если шар уходит за пределы игрового поля - перезагружаем
        if (col.gameObject.layer == LayerMask.NameToLayer(Constants.LayerBounds))
        {
            Reset();
        }

        //Если шар попадает в препятствие
        else
        {
            IsReflected = true;
            ContactPoint2D contact = col.contacts[0];

            Vector3 inDirection = Vector3.Reflect(transform.right, hit.normal);
            Vector2 reflectDir = Vector2.Reflect(inDirection, contact.normal);
            float rot = Mathf.Atan2(reflectDir.y, reflectDir.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, rot);

            if (col.gameObject.layer == LayerMask.NameToLayer(Constants.LayerHited))
            {
                GameManager.Instance.HitResponse(col.collider.gameObject);
            }
        }
    }
}
