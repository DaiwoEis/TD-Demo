using UnityEngine;
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;

public abstract class DefenderBase : SerializedMonoBehaviour
{ 
    protected Animation anim;

    protected Transform destination;
      
    protected AudioSource audioSource;

    protected int _currLevel;

    protected int _currAtk = 50;

    protected int _currAtkDistance = 20;

    protected float _currAtkIntervial = 0.5f;

    [ShowInInspector] public ResourceLoader<DefenderConfigData> ConfigLoader;

    protected DefenderConfigData _configData;

    protected Enemy target;

    protected void Awake()
    {
        _configData = ConfigLoader.Load();
        _configData.Init();

        _currLevel = 1;
        _currAtk = _configData.Atk;
        _currAtkDistance = _configData.AtkDistance;
        _currAtkIntervial = _configData.AtkIntervial;

        anim = GetComponentInChildren<Animation>();
        destination = GameObject.FindGameObjectWithTag(GameConfig.Tag_Destination).transform;
        audioSource = GetComponent<AudioSource>();        
    }

    protected void Start()
    {           
        StartCoroutine(_ScanEnemy());
    }

    private IEnumerator _ScanEnemy()
    {
        var intervial = new WaitForSeconds(_configData.ScanIntervial);
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
        anim.Play(_configData.AnimName.Attack);
        this.Invoke(anim.GetClip(_configData.AnimName.Attack).length, () => { anim.Play(_configData.AnimName.Idle); });
        this.Invoke(_currAtkIntervial, () => { StartCoroutine(_ScanEnemy()); });
        this.Invoke(_configData.FireTime, Firing);
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
                .Where(e => e.HP > 0 && Vector3.Distance(e.transform.position, transform.position) < _currAtkDistance);

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
        return _currLevel != _configData.MaxLevel;
    }

    public int UpgradePrice()
    {
        return _configData.BuyPrices[_currLevel + 1];
    }

    public void Upgrade()
    {
        _currLevel++;
        _currAtk += _configData.UpgradeAtkChanged;
        _currAtkIntervial /= _configData.UpgradeAtkIntervialChanged;
        _currAtkDistance += _configData.UpgradeAtkDistanceChanged;
    }

    public int SalePrice()
    {
        int price = 0;

        for (int i = 1; i <= _currLevel; i++)
            price += BuyPrice(i);
        price /= 2;
        return price;
    }

    public void Sale()
    {        
        Destroy(gameObject);
    }

    public int Level { get { return _currLevel; } }

    public int BuyPrice(int level)
    {
        return _configData.BuyPrices[_currLevel];
    }
}
