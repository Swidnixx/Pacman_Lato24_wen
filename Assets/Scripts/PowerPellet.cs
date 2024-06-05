using UnityEngine;

public class PowerPellet : Pellet
{
    public float duration = 8;

    protected override void Eat()
    {
        base.Eat();
        GameManager.Instance.PowerPelletEaten(this);
    }
}
