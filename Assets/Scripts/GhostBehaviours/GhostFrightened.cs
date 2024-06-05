using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFrightened : GhostBehaviour
{
    [SerializeField] private SpriteRenderer normalBody;
    [SerializeField] private SpriteRenderer flashingBody;
    [SerializeField] private SpriteRenderer blueBody;

    [SerializeField] private SpriteRenderer eyesRenderer;

    private bool eaten;

    private void ShowGhostBody(SpriteRenderer bodyToShow)
    {
        SpriteRenderer[] bodies = { normalBody, flashingBody, blueBody };

        foreach (SpriteRenderer body in bodies)
        {
            body.enabled = (body == bodyToShow);
        }
    }

    public override void Enable(float duration)
    {
        base.Enable(duration);

        ShowGhostBody(blueBody);
        eyesRenderer.enabled = false;
        eaten = false;

        CancelInvoke(nameof(Flash));
        Invoke(nameof(Flash), duration / 2f);
    }

    void Flash()
    {
        ShowGhostBody(flashingBody);
        eyesRenderer.enabled = false;
    }

    public override void Disable()
    {
        base.Disable();

        ShowGhostBody(normalBody);
        eyesRenderer.enabled = true;
        eaten = false;
    }

    private void GhostEaten()
    {
        eaten = true;
        Vector3 position = Ghost.Home.Inside.position;
        position.z = Ghost.transform.position.z;
        Ghost.transform.position = position;
        Ghost.Home.Enable();

        GameManager.Instance.GhostEaten(Ghost);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (enabled)
            {
                GhostEaten();
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // jeœli zachowanie jest aktywne i zderzyliœmy siê z obiektem posiadaj¹cym skrypt Node
        if (!eaten && enabled && collision.TryGetComponent(out Node node))
        {
            // domyœlny kierunek to (0,0)
            Vector2 direction = Vector2.zero;
            // min dystans - ustawiamy na najwiêkszy mo¿liwy
            // tak aby pierwszy kierunek by³ od razu dotychczasow¹ najlepsz¹ opcj¹
            float maxDistance = float.MinValue;

            // dla ka¿dego kierunku w obecnym wêŸle
            foreach (Vector2 availableDirection in node.availableDirections)
            {
                // ustalamy now¹ pozycjê jako obecn¹ przesuniêt¹ o sprawdzany kierunek
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y, 0f);
                // obliczamy dystans - u¿ywamy sqrtMagnitude zamiast magnitude dla performance
                float distance = (Ghost.Target.position - newPosition).sqrMagnitude;

                // jeœli obecny dystans jest mniejszy ni¿ dotychczasowy najmniejszy
                // to ten kierunek powinen zbli¿yæ ducha do Pacmana bardziej
                if (distance > maxDistance)
                {
                    // ustawiamy kierunek na sprawdzany kierunek
                    direction = availableDirection;
                    // ustawiamy min dystans na obecny dystans
                    maxDistance = distance;
                }
            }

            // ustalenie kierunku na najlepszy
            Ghost.Movement.SetDirection(direction);
        }
    }
}
