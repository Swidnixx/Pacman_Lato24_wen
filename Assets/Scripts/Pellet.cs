using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Pellet : MonoBehaviour
{
    public int points = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Eat();
        }
    }

    protected virtual void Eat()
    {
        gameObject.SetActive(false);
        GameManager.Instance.PelletEaten(this);
    }
}
