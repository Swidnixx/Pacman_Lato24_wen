using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScatter : GhostBehaviour
{
    private void OnDisable()
    {
        // do usuniêcia póŸniej
        Enable();
        // do w³¹czenia potem
        //if (Ghost.Chase != null) Ghost.Chase.Enable();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // jeœli aktywny (OnTriggerEnter mo¿e aktywowaæ siê nawet jeœli zachowanie nie jest aktywne)
        // i jeœli nie jest przestraszony oraz wszed³ w kollider Node'a
        if (enabled && collision.TryGetComponent(out Node node))
        {
            Debug.Log("Node detected");
            // losujemy index kierunku z listy dostepnych w danym skrzy¿owaniu/nodzie
            int index = Random.Range(0, node.availableDirections.Count);

            // zapobieganie zawracaniu - jeœli wybraliœmy kierunek wstecz/zmieniamy na kolejny
            // tylko jeœli jest inna œcie¿ka (w poziomie nie ma slepych uliczek, nie powinna taka sytuacja zajœæ)
            if (node.availableDirections[index] == -Ghost.Movement.direction &&
                node.availableDirections.Count > 1)
            {
                index++;
                if(index >= node.availableDirections.Count)
                {
                    index = 0;
                }
            }

            // ustalamy kierunek - nastêpny do obrania bo dopiero wchodzimy na wêze³
            Ghost.Movement.SetDirection(node.availableDirections[index]);
        }
    }
}
