using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    #region Fields
    [SerializeField] float moveSpeed;
    bool isMovingRight = true; 
    #endregion

    #region Monobehaviour Callbacks
    private void Update()
    {
        Vector2 moveDirection = SetDirection();
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("waypoint"))
        {
            FlipCharacter();
        }
    }
    #endregion

    #region Movement Utility
    private Vector2 SetDirection()
    {
        Vector2 moveDirection = Vector2.zero;
        if (isMovingRight)
        {
            moveDirection = new Vector2(1, 0);
        }
        else
        {
            moveDirection = new Vector2(-1, 0);
        }

        return moveDirection;
    }
    void FlipCharacter()
    {
        if (isMovingRight)
        {
            isMovingRight = false;
            transform.localScale = new Vector3(-1, 1f, 1f);
        }
        else
        {
            isMovingRight = true;
            transform.localScale = new Vector3(1, 1f, 1f);

        }
    } 
    #endregion
}
