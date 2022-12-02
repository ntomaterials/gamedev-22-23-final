using UnityEngine;

public class Archer : Enemy
{
    [SerializeField] private GameObject projectilePref;
    [SerializeField] private Transform shootPosition;
    [SerializeField] private float reload = 3f;
    
    private float reloadTime = 0f;
    private float hight; // макс возможная разница в y оординатах для попадания в игрока
    private const float repeatRate = 0.2f;
    protected void Start()
    {
        hight = Player.Instance.GetComponent<Collider2D>().bounds.extents.y;
        InvokeRepeating("LookAround", 0, repeatRate); // проверка 5 раз в сек для оптимизации
    }

    // проверка стоит ли стрелять
    private void LookAround()
    {
        reloadTime -= repeatRate;
        if (reloadTime <= 0)
        {
            int side = CheckSides();
            if (side != 0)
            {
                RotateByX(side);
                animator.SetTrigger("shoot");
                reloadTime = reload;
            }
        }
    }

    public void ShootArrow()
    {
        Instantiate(projectilePref, shootPosition.transform.position, transform.rotation);
    }

    /// <summary>
    ///  Возвращает -1, 1 если игрок в зоне 0 если нет
    /// </summary>
    private int CheckSides()
    {
        if (Mathf.Abs(Player.Instance.transform.position.y - transform.position.y) >= hight) return 0;
        float dist = Player.Instance.transform.position.x - transform.position.x;

        if (!Physics2D.Raycast(transform.position, (Vector2.right * dist).normalized, Mathf.Abs((dist)), groundLayerMask))
        {
            if (dist > 0) return 1;
            else return -1;
        }
        else
        {
            return 0;
        }
    }
}
