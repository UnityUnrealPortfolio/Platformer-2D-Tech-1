using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallChecker : MonoBehaviour
{
    [field: SerializeField] public bool wallToLeft { get; private set; }
    [field: SerializeField] public bool wallToRight { get; private set; }
    [SerializeField] float castDistance;

    public CapsuleCollider2D playerCollider;
    public GroundChecker groundChecker;
    public Rigidbody2D playerRb;

    private void Update()
    {
        if (groundChecker.isGrounded)
        {
            wallToLeft = false;
            wallToRight = false;
            return;
        }

        wallToRight = CheckWall(1);
        wallToLeft = CheckWall(-1);
    }

    private bool CheckWall(int direction)
    {
        var hitInfo = Physics2D.BoxCast(transform.position, playerCollider.size * .75f,
           0f, new Vector2(direction,0f), castDistance, LayerMask.GetMask("Ground"));

        if (hitInfo.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

 
}
