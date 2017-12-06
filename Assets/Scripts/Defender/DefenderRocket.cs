using UnityEngine;
using System.Collections;

public class DefenderRocket : DefenderBase
{       
    public Transform shootPos;
      
    protected override void Firing()
    {
        base.Firing();
        StartCoroutine(Shooting());
    }

    private IEnumerator Shooting()
    {
        yield return new WaitForSeconds(0.3f);

        var rocketGO = EntityManager.Instance.CreateGO(_configData.As<DefenderRocketConfigData>().RocketPrefab, shootPos.position, Quaternion.identity);
        Rocket rocket = rocketGO.GetComponent<Rocket>();
        rocket.attackTarget = target.transform;
        rocket.atk = _currAtk;
    }
}
