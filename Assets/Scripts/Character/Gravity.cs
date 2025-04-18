using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    [SerializeField]
    private float _gravityForce;

    private void FixedUpdate()
    {
        AdaptationGravity();
    }
 
    public void AdaptationGravity()
    {
        transform.position += Vector3.down * _gravityForce * Time.deltaTime;
    }
}
