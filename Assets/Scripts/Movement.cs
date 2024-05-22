using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // reset state at first frame
        ResetState();
    }

    public void ResetState()
    {
        // set direction to initial direction
        direction = initialDirection;
        // set next direction to (0,0)
        nextDirection = Vector3.zero;
        // set position to starting position
        transform.position = startingPosition;
        // enable script
        enabled = true;
    }

    private void Update()
    {
        // if next direction is not equal (0,0) set direction to queued next direction
        if(nextDirection != Vector2.zero)
        {
            SetDirection(nextDirection);
        }
    }

    private void FixedUpdate()
    {
        // get pacman move vector (multiplied by fixedDeltaTime and Pacman speed)
        Vector2 move = direction * speed * Time.deltaTime;
        // move pacman rigidbody 2D to along calculated vector
        rb.MovePosition(rb.position + move);
    }

    public void SetDirection(Vector2 directionToSet)
    {
        // if direction to set is not occupied set direction to next direction
        // and reset next direction (set to (0,0) )
        if(!Occupied(directionToSet))
        {
            direction = directionToSet;
            nextDirection = Vector2.zero;
        }
        // else queue up direction to set in next direction variable
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
