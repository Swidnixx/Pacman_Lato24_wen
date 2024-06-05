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
        // je�li zachowanie jest aktywne i zderzyli�my si� z obiektem posiadaj�cym skrypt Node
        if (!eaten && enabled && collision.TryGetComponent(out Node node))
        {
            // domy�lny kierunek to (0,0)
            Vector2 direction = Vector2.zero;
            // min dystans - ustawiamy na najwi�kszy mo�liwy
            // tak aby pierwszy kierunek by� od razu dotychczasow� najlepsz� opcj�
            float maxDistance = float.MinValue;

            // dla ka�dego kierunku w obecnym w�le
            foreach (Vector2 availableDirection in node.availableDirections)
            {
                // ustalamy now� pozycj� jako obecn� przesuni�t� o sprawdzany kierunek
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y, 0f);
                // obliczamy dystans - u�ywamy sqrtMagnitude zamiast magnitude dla performance
                float distance = (Ghost.Target.position - newPosition).sqrMagnitude;

                // je�li obecny dystans jest mniejszy ni� dotychczasowy najmniejszy
                // to ten kierunek powinen zbli�y� ducha do Pacmana bardziej
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
