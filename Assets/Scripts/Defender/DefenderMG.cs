public class DefenderMG : DefenderBase
{
    protected override void Firing()
    {
        base.Firing();

        target.GetComponent<Enemy>().Damage(status.atk);
    }
}
