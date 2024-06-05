using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostChase : GhostBehaviour
{
    private void OnDisable()
    {
        if (Ghost.Scatter) Ghost.Scatter.Enable();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // jeœli zachowanie jest aktywne i zderzyliœmy siê z obiektem posiadaj¹cym skrypt Node
        if (enabled && collision.TryGetComponent(out Node node))
        {
            // domyœlny kierunek to (0,0)
            Vector2 direction = Vector2.zero;
            // min dystans - ustawiamy na najwiêkszy mo¿liwy
            // tak aby pierwszy kierunek by³ od razu dotychczasow¹ najlepsz¹ opcj¹
            float minDistance = float.MaxValue;

            // dla ka¿dego kierunku w obecnym wêŸle
            foreach (Vector2 availableDirection in node.availableDirections)
            {
                // ustalamy now¹ pozycjê jako obecn¹ przesuniêt¹ o sprawdzany kierunek
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y, 0f);
                // obliczamy dystans - u¿ywamy sqrtMagnitude zamiast magnitude dla performance
                float distance = (Ghost.Target.position - newPosition).sqrMagnitude;

                // jeœli obecny dystans jest mniejszy ni¿ dotychczasowy najmniejszy
                // to ten kierunek powinen zbli¿yæ ducha do Pacmana bardziej
                if (distance < minDistance)
                {
                    // ustawiamy kierunek na sprawdzany kierunek
                    direction = availableDirection;
                    // ustawiamy min dystans na obecny dystans
                    minDistance = distance;
                }
            }

            // ustalenie kierunku na najlepszy

        }
    }
}
