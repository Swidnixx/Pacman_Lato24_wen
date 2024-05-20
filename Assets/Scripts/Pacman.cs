using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pacman : MonoBehaviour
{
    

    private void RotatePacman(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }
}
