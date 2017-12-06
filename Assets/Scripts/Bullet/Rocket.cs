using UnityEngine;
using System.Linq;
 
public class Rocket : MonoBehaviour
{
    public float moveSpeed = 10;

    [HideInInspector]
    public Transform attackTarget;

    public float explodeRange = 10;

    public int atk;

    public GameObject explodeFx;

    private void Awake()
    {
        GetComponent<PooledObject>().OnReturnToPool += o => { attackTarget = null; };
    }

    private void Update()
    {
        if (attackTarget == null)
            EntityManager.Instance.DestroyGO(gameObject);

        Movement();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == GameConfig.Tag_Enemy)
            Explode();
    }

    public void Movement()
    {           
        LookAtTarget(attackTarget.position);
        transform.position = Vector3.MoveTowards(transform.position, attackTarget.position + new Vector3(0, 1.5f, 0), moveSpeed * Time.deltaTime);
    }

    private void LookAtTarget(Vector3 target)
    {
        transform.LookAt(target);
    }
     
    private void Explode()
    {        
        var fxGo = EntityManager.Instance.CreateGO(explodeFx, transform.position, Quaternion.identity);             
        Enemy[] enemyArray = EntityManager.Instance.GetGameObjectWithTag("Enemy").Select(e => e.GetComponent<Enemy>())
            .Where(e => e.HP > 0 && Vector3.Distance(e.transform.position, transform.position) < explodeRange)
            .ToArray();

        foreach (Enemy item in enemyArray)
            item.Damage(atk);

        EntityManager.Instance.DestroyGO(gameObject);
        EntityManager.Instance.DestroyGO(fxGo, 1f);
    }

    public GameObject OwnerPool { get; set; }

    public void OnRetrieveFromPool()
    {
        
    }

    public void OnReturnToPool()
    {
        attackTarget = null;
    }
}
