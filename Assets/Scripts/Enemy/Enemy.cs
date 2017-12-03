using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform wayLine;

    private Vector3[] wayPoints;

    private int index;
    public float moveSpeed = 10;

    private ParticleSystem particleSys;

    public int giveGold = 50;
    public float HP = 200;

    public event Action<Enemy> OnEnemyDead;
    public event Action<Enemy> OnMoveEnd; 

    private void Awake()
    {
        CalculateWayPoint();

        particleSys = GetComponentInChildren<ParticleSystem>();

        GetComponent<PooledObject>().OnRetrieveFromPool += o => { StartGame(); };
    }

    private void StartGame()
    {
        HP = 200;
        index = 0;
        LookAtFirstWayPoint();
    }

    private void Update()
    {
        if (HP <= 0 || MoveEnd()) return;

        Movement();
    }

    public void LookAtFirstWayPoint()
    {
        if (wayPoints.Length < 0) return;

        LookAt(wayPoints[0]);
    }

    private void CalculateWayPoint()
    { 
        wayPoints = new Vector3[wayLine.childCount];
        for (int i = 0; i < wayLine.childCount; i++)
            wayPoints[i] = wayLine.GetChild(i).position;
    }

    public void Movement()
    {
        transform.position = Vector3.MoveTowards(transform.position, wayPoints[index], moveSpeed * Time.deltaTime);
    
        if (Vector3.Distance(wayPoints[index], transform.position) < 0.1f)
        {
            index += 1;
            if (!MoveEnd())
            {
                LookAt(wayPoints[index]);
            }
            else
            {
                GameController.Instance.AddIntrude();

                EntityManager.Instance.DestroyGO(gameObject);
                if (OnMoveEnd != null)
                    OnMoveEnd(this);
            }
        }
    }

    public bool MoveEnd()
    {
        return index == wayPoints.Length;
    }

    private void LookAt(Vector3 target)
    {
        target.y = transform.position.y;
        transform.LookAt(target);
    }

    public void Damage(int damage)
    {
        HP = HP - damage;
        particleSys.Play();

        if (HP <= 0)
            Dead();
    }

    public void Dead()
    {
        if (name.StartsWith("Soldier"))
        {
            GetComponentInChildren<Animation>().Play("die");
        }

        GameController.Instance.ChangeGold(giveGold);
        GameController.Instance.AddKilled();

        EntityManager.Instance.DestroyGO(gameObject, 3f);

        if (OnEnemyDead != null)
            OnEnemyDead(this);
    }
}
