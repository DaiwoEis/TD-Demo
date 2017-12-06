using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform wayLine;

    private Vector3[] _wayPoints;

    private int _index;

    public float MoveSpeed = 10;

    private ParticleSystem particleSys;

    public int GiveGold = 50;

    public float HP = 200;

    public string ConfigDataName;

    private EnemyConfigData _configData;

    public event Action<Enemy> OnEnemyDead;
    public event Action<Enemy> OnMoveEnd; 

    private void Awake()
    {
        _configData = ResourceManager.Load<ScriptableObject>(ConfigDataName) as EnemyConfigData;

        CalculateWayPoint();

        particleSys = GetComponentInChildren<ParticleSystem>();

        GetComponent<PooledObject>().OnRetrieveFromPool += o => { ResetData(); };
    }

    private void ResetData()
    {
        _index = 0;
        SetUpConfigData();
        LookAtFirstWayPoint();
    }

    private void Update()
    {
        if (HP <= 0 || MoveEnd()) return;

        Movement();
    }

    private void SetUpConfigData()
    {
        HP = _configData.BaseHP;
        MoveSpeed = _configData.BaseMoveSpeed;
        GiveGold = _configData.GiveGlod;
    }

    public void LookAtFirstWayPoint()
    {
        if (_wayPoints.Length < 0) return;

        LookAt(_wayPoints[0]);
    }

    private void CalculateWayPoint()
    { 
        _wayPoints = new Vector3[wayLine.childCount];
        for (int i = 0; i < wayLine.childCount; i++)
            _wayPoints[i] = wayLine.GetChild(i).position;
    }

    public void Movement()
    {
        transform.position = Vector3.MoveTowards(transform.position, _wayPoints[_index], MoveSpeed * Time.deltaTime);
    
        if (Vector3.Distance(_wayPoints[_index], transform.position) < 0.1f)
        {
            _index += 1;
            if (!MoveEnd())
            {
                LookAt(_wayPoints[_index]);
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
        return _index == _wayPoints.Length;
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

        GameController.Instance.ChangeGold(GiveGold);
        GameController.Instance.AddKilled();

        EntityManager.Instance.DestroyGO(gameObject, 3f);

        if (OnEnemyDead != null)
            OnEnemyDead(this);
    }
}
