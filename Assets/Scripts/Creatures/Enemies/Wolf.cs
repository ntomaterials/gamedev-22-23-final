using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Wolf : Squirrel
{
    [SerializeField] protected float _distCanSeeMeet;
    [SerializeField] protected State runToMeetState;
    [SerializeField] protected State calmState;
    private List<Product> _meetList;
    public Meet target;
    public bool runToMeet;
    protected virtual void Start()
    {
        Player.Instance.MeetInventory.onMeetDroped+=AddMeet;
        _meetList=new List<Product>();
    }
    protected override void ChooseNewState()
    {
        CheckMeet();
        if (target!=null)// && currentState!=calmState)
        {
            SetState(runToMeetState);
            animator.SetFloat("speed", speed);
            runToMeet=true;
            _attaking=false;
        }
        //Vector2 dir = Player.Instance.transform.position - transform.position;
        if (!CanSeePlayer() && !runToMeet)
        {
            SetState(startState);
            _attaking = false;
        }
        else if (!runToMeet && isGrounded && reloadTime <= 0f)
        {
            Attack();
        }      
    }
    /*protected override void FixedUpdate()
    {
        base.FixedUpdate();
        CheckMeet();
    }*/
    protected void AddMeet(Product meet)
    {
        _meetList.Add(meet);
        //foreach(Meet item in _meetList) if(item==null) _meetList.Remove(item);
        //CheckMeet();
        //UpdateList();
    }
    protected void CheckMeet()
    {
        float minDist=999;
        foreach(Meet meet in _meetList)
        {
            if(meet!=null) 
            {
                float dist=CheckDistance(meet.transform.position);
                if(dist!=0 && dist<minDist)
                {
                    minDist=dist;
                    target=meet;
                }
            }
        }
        //if(target!=null) SetState(runToMeetState);
        //else target=null;
    }
    protected float CheckDistance(Vector3 pos)
    {
        Vector2 dir = pos - transform.position;
        float dist = dir.magnitude;
        //if (dist > int.MaxValue) return 0;
        if (Physics2D.Raycast(transform.position, dir, dir.magnitude, groundLayerMask)) return 0;
        else return dist;
    }
    public void TakeMeet()
    {
        runToMeet=false;
        UpdateList();
        animator.SetTrigger("calm");
        SetState(calmState);
        Destroy(target.gameObject);
    }
    public void UpdateList()
    {
        foreach(Meet item in _meetList) if(item==null) _meetList.Remove(item);
    }
    public bool IsTargetNullOrMissing()
    {
        return target == null || !GameObject.ReferenceEquals(target, null);
    }
    private void OnDrawGizmos()
    {
        try
        {
        Gizmos.color=Color.blue;
        Gizmos.DrawLine(transform.position, transform.position+new Vector3(_distCanSeeMeet, 0,0));    
        }
        catch{return;}
    }
}
