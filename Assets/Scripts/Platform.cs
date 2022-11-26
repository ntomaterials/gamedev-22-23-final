using UnityEngine;
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
/// <summary>
/// Этот класс нужен, чтобы противник понимал, где ему можно ходить. Например, если он свалится на другую платформу
/// </summary>
public class Platform : MonoBehaviour
{
    [field: SerializeField] public Transform Left { get; private set; }
    [field: SerializeField] public Transform Right { get; private set; }
}
