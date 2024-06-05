using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    public float speed = 8.0f;
    public Vector2 initialDirection;
    public LayerMask obstacleLayer;

    public Rigidbody2D rb { get; private set; }

    public Vector2 direction { get; private set; }

    public Vector2 nextDirection { get; private set; }

    public Vector3 startingPosition { get; private set; }

    

    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
    }

    private void Start()
    {
        ResetState();
    }

    public void ResetState()
    {
        direction = initialDirection;
        nextDirection = Vector2.zero;
        transform.position = startingPosition;
        enabled = true;
    }

    private void Update()
    {
        if (nextDirection != Vector2.zero)
        {
            SetDirection(nextDirection);
        }
    }

    private void FixedUpdate()
    {
        Vector2 move = direction * speed * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + move);
    }

    public void SetDirection(Vector2 directionToSet)
    {
        if (!Occupied(directionToSet))
        {
            direction = directionToSet;
            nextDirection = Vector2.zero;
        }
        else
        {
            nextDirection = directionToSet;
        }
    }

    public bool Occupied(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.75f, 0.0f, direction, 1.5f, obstacleLayer);

        return hit.collider != null;
    }
}
