/********************************

Najwa¿niejsze punkty zachowania GhostHome to:
1. Po aktywacji duch odbija siê od œcian i zostaje w bazie do jego 
   zakoñczenia niezale¿nie od tego jaki inny stan jest aktywowany.
2. Po zakoñczeniu tego stanu wy³¹czamy poruszanie siê gracza na czas 
   sekwencji wyjœcia z bazy i jest uruchamiana sekwencja wyjœcia 
   sk³adaj¹ca siê z dwóch etapów:
      - przemieszczenia siê do wêz³a Inside bêd¹cego w œrodku bazy
      - przemieszczenia siê do wêz³a Outside bêd¹cego zaraz na zewn¹trz bazy
3. Po zakoñczeniu sekwencji wyjœcia z bazy sterowanie jest oddawane do skryptu 
   Movement z losowo ustawionym kierunkiem prawo/lewo i stanem, i stanem który 
   jest aktualnie aktywny (bêdzie to stan GhostScatter albo GhostChase).

********************************/

using System.Collections;
using UnityEngine;

public class GhostHome : GhostBehaviour
{
    [SerializeField] private Transform inside;
    public Transform Inside => inside;
    [SerializeField] private Transform outside;
    public Transform Outside => outside;

    // uruchamianie sekwencji wyjœcia z bazy w momencie wyjœcia ze stanu GhostHome
    private void OnDisable()
    {
        if(Ghost && Ghost.Movement && gameObject.activeSelf)
            StartCoroutine(ExitTransition());
    }

    private IEnumerator ExitTransition()
    {
        // wy³¹czenie sterowania i fizyki Rigidbody dla obiektu
        // (isKinematic sprawia ¿e rb si³y nie wp³ywaj¹ na obiekt)
        Ghost.Movement.rb.isKinematic = true;
        Ghost.Movement.SetDirection(Vector2.up);
        Ghost.Movement.enabled = false;

        // pozycja startowa
        Vector3 position = transform.position;

        // czas animacji od pozycji startowej w bazie do Inside
        float duration = 0.5f;
        // czas który up³yn¹³
        float elapsed = 0.0f;

        // wykonanie tej funkcji bêdzie roz³o¿one w czasie
        while (elapsed < duration)
        {
            // ustalamy now¹ pozycjê jako po³o¿enie miêdzy pozycj¹ startow¹ a pozycjê Inside
            // w zale¿noœci od czasu który up³yn¹³ elapsed przez d³ugoœæ animacji
            Vector3 newPosition = Vector3.Lerp(position, inside.position, elapsed/duration);
            // przywracamy domyœln¹ pozycjê z (u nas bêdzie ona wynosi³a 0)
            newPosition.z = position.z;
            // przesuwamy ducha na okreœlon¹ pozycjê
            Ghost.transform.position = newPosition;
            // zwiêkszamy czas który up³yn¹³
            elapsed += Time.deltaTime;
            yield return null; // tutaj bêdziemy wznawiaæ od kolejnej klatki
        }

        // zerujemy czas który up³yn¹³
        elapsed = 0f;

        // poni¿sza pêtla jest taka sama jak powy¿sza tylko teraz startem jest pozycja Inide a koñcowa to Outside
        while (elapsed < duration)
        {
            Vector3 newPosition = Vector3.Lerp(inside.position, outside.position, elapsed / duration);
            newPosition.z = position.z;
            Ghost.transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return null; // tutaj bêdziemy wznawiaæ od kolejnej klatki
        }

        // losujemy kierunek, uruchamiamy Movement, ustawiamy mu kierunek i w³¹czamy fizykê w Rigidbody
        // (isKinematic sprawia ¿e rb si³y nie wp³ywaj¹ na obiekt)
        Vector2 randomDirection = new Vector2(Random.value < 0.5f ? -1 : 1, 0f);
        Ghost.Movement.enabled = true;
        Ghost.Movement.SetDirection(randomDirection);
        Ghost.Movement.rb.isKinematic = false;
    }


    // Odbijanie siê od œcian w stanie GhostHome
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (enabled)
        {
            Ghost.Movement.SetDirection(-Ghost.Movement.direction);
        }
    }
}
