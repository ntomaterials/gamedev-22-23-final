using System;
using UnityEditor;
using UnityEngine;
[RequireComponent (typeof(AudioSource))]
public abstract class Weapon : MonoBehaviour
{
    [Header("Player only")] // в идеале написать кастомный испектор чтоб это всё скрывать если оружие не для игрока предназначено
    //public AnimatorOverrideController leftPlayerAnimation;
    //public AnimatorOverrideController rightPlayerAnimation;

    [Header("Base stats")]
    [SerializeField] protected float reload;
    public int damage;
    
    [Header("Melee")]
    public bool hasBlock;
    public float blockReload=1f;
    public float dashSpeed;
    
    public bool slashActive { get; protected set; }
    protected float reloadTime = 0f;

    protected AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // все методы ниже будут вызываться через анимации, так что может быть такое то что какая то из функций оказывается ненужна
    public virtual void Fire() // для дальнего оружия
    {
        Debug.LogWarning("No Fire()");
    }
    public virtual void SlashStart() // для ближнего
    {
        Debug.LogWarning("No SlashStart()");
    }
    public virtual void SlashStop() // для ближнего
    {
        Debug.LogWarning("No SlashEnd()");
    }

    protected virtual void Update()
    {
        reloadTime -= Time.deltaTime;
    }

    public virtual bool ready
    {
        get
        {
            return (reloadTime <= 0);
        }
    }
}
