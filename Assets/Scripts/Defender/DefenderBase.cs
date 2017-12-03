using UnityEngine;
using System.Collections;
using System.Linq;

public abstract class DefenderBase : MonoBehaviour
{ 
    protected Animation anim;

    protected Transform destination;
      
    protected AudioSource audioSource;

    protected DefenderStatus status;

    protected Enemy target;

    protected void Start()
    {
        anim = GetComponentInChildren<Animation>();
        destination = GameObject.FindGameObjectWithTag(GameConfig.Tag_Destination).transform; 
        audioSource = GetComponent<AudioSource>();
        status = GetComponent<DefenderStatus>();

        StartCoroutine(_ScanEnemy());
    }

    private IEnumerator _ScanEnemy()
    {
        var intervial = new WaitForSeconds(status.scanIntervial);
        while (true)
        {
            target = SelectEnemy();
            if (target != null)
            {
                Attack();                
                yield break;                
            }            
            yield return intervial;
        }
        
    }

    private void Attack()
    {        
        LookAtTarget(target.transform.position);
        anim.Play(status.animName.Attack);
        this.Invoke(anim.GetClip(status.animName.Attack).length, () => { anim.Play(status.animName.Idle); });
        this.Invoke(status.attackIntervial, () => { StartCoroutine(_ScanEnemy()); });
        this.Invoke(status.fireTime, Firing);
    }

    protected virtual void Firing()
    {
        audioSource.Play();
    }
     
    protected Enemy SelectEnemy()
    {
        if (EntityManager.Instance.GetGameObjectWithTag(GameConfig.Tag_Enemy).Count > 0)
        {             
            var enemiesInAtkRange = EntityManager.Instance.GetGameObjectWithTag(GameConfig.Tag_Enemy)
                .Select(g => g.GetComponent<Enemy>())
                .Where(e => e.HP > 0 && Vector3.Distance(e.transform.position, transform.position) <
                            status.attackDistance);

            Enemy enemy = enemiesInAtkRange.OrderBy(e => Vector3.Distance(e.transform.position, destination.position))
                .FirstOrDefault();

            if (enemy != null)
                return enemy;
        }
        return null;
    }

    protected void LookAtTarget(Vector3 targetPoint)
    {
        targetPoint.y = transform.position.y;
        transform.LookAt(targetPoint);
    }

    public bool CanUpgrade()
    {
        return !status.IsMaxLevel;
    }

    public int UpgradePrice()
    {
        return status.UpgradePrice;
    }

    public void Upgrade()
    {
        status.Upgrade();
    }

    public int SalePrice()
    {
        return status.SalePrice();
    }

    public void Sale()
    {        
        Destroy(gameObject);
    }

    public int Level { get { return status.level; } }

    public int BuyPrice(int level)
    {
        return status.buyPrice[level];
    }
}
