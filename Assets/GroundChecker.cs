using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    BoxCollider2D playerFeetCollider;

    public bool isGrounded {  get; private set; }

    private void Awake()
    {
        playerFeetCollider = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        if (playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}
