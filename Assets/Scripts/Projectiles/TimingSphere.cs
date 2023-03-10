using UI;
using UnityEngine;

public class TimingSphere : Creature, IReloadable
{
    [SerializeField] private float time = 10f;
    [SerializeField] private GameObject deathTrail;
    

    protected override void FixedUpdate()
    {
        time -= Time.fixedDeltaTime;
        if (time <= 0)
        {
            if (deathTrail != null)
            {
                GameObject flash = Instantiate(deathTrail, transform.position, deathTrail.transform.rotation);
            }
            Die();
        }
    }

    public float cooldown
    {
        get
        {
            return time;
        }
    }
}
