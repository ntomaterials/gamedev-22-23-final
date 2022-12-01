using UnityEngine;


public abstract class State : ScriptableObject
{
    // изменяются только во время игры
    public bool isFinished { get; protected set; }
    [HideInInspector] public Enemy owner;

    public virtual void Init(Enemy enemy)
    {
        owner = enemy;
    }

    public abstract void Run();
}
