using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float enemyMoveSpeed = -1f;
    Rigidbody2D myRigidBody;
    CapsuleCollider2D myCapsuleCollider;
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        myRigidBody.velocity = new Vector2 (enemyMoveSpeed,0f);

    }

    void Enemyflip()
    {
        
        bool isTouchingWall = myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));

        // transform.localScale = new Vector2 (-(Mathf.Sign(myRigidBody.velocity.x)),1f);
        enemyMoveSpeed = -enemyMoveSpeed;
      
    }
    void OnTriggerExit2D(Collider2D other) 
    {
        
        
        Enemyflip();
    }
}

