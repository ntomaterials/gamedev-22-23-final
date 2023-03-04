using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBtwPoints : ToActiveCycle
{
    [SerializeField] private List<Transform> _points;
    [SerializeField] private float _speed;
    private int _currentPointIndex;
private void Start() 
{
    StartCoroutine(Move());
}
private IEnumerator Move()
{
    while (true)
    {
        if(isActive)
        {
        // Перемещение к текущей точке
        Transform currentPoint = _points[_currentPointIndex];
        float distanceToMove = Vector3.Distance(transform.position, currentPoint.position);
        while (distanceToMove > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentPoint.position, _speed * Time.deltaTime);
            distanceToMove = Vector3.Distance(transform.position, currentPoint.position);
            yield return null;
        }

        // Перейти к следующей точке в массиве
        _currentPointIndex = (_currentPointIndex + 1) % _points.Count;
        }
        else yield return null;
    }
}
protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer || collision.gameObject.layer == GlobalConstants.EnemyLayer)
        {
            collision.collider.transform.SetParent(transform);
        }
    }
    protected void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer || collision.gameObject.layer == GlobalConstants.EnemyLayer)
        {
            collision.collider.transform.SetParent(null);
        }
    }
private void OnDrawGizmos()
{
    try
    {
    Gizmos.color=Color.blue;
    for (int i=0; i<_points.Count; i++)
    {
        if(i==_points.Count-1) Gizmos.DrawLine(_points[i].position, _points[0].position);
        else Gizmos.DrawLine(_points[i].position, _points[i+1].position);
    }
    }
    catch {return;}
}
}