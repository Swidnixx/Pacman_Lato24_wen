using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Animator Animator { get; private set; }
    public Movement Movement { get; private set; }
    public GhostHome Home { get; private set; }
    public GhostScatter Scatter { get; private set; }
    public GhostChase Chase { get; private set; }
    public GhostFrightened Frightened { get; private set; }

    [SerializeField] GhostBehaviour initialBehaviour;

    public Transform Target { get; private set; }

    [SerializeField] int points = 200;
    public int Points => points;

    private void Awake()
    {
        Movement = GetComponent<Movement>();
        Home = GetComponent<GhostHome>();
        Scatter = GetComponent<GhostScatter>();
        Chase = GetComponent<GhostChase>();
        Frightened = GetComponent<GhostFrightened>();
    }

    private void Start()
    {
        Target = GameObject.FindGameObjectWithTag("Player").transform;
        ResetState();
    }

    public void ResetState()
    {
        gameObject.SetActive(true);
        Movement.ResetState();

        Frightened.Disable();
        Chase.Disable();
        Scatter.Enable();
        
        if (Home == initialBehaviour)
        {
            Home.Enable();
        }

        if (initialBehaviour != null)
        {
            initialBehaviour.Enable();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player") && !Frightened.enabled)
        {
            GameManager.Instance.PacmanEaten();
        }
    }
}
