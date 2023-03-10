using UnityEngine;

public class Nedoplanet : MonoBehaviour
{
    [SerializeField] private ArcShooter shooter;

    # region AnimationTriggers
    public void Boom()
    {
        shooter.Fire();
        Destroy(this.gameObject, 1f);
    }
    #endregion
}
