using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMovement : MonoBehaviour
{
    [SerializeField] float enemyMoveSpeed = -1f;
    Rigidbody2D myRigidBody;
    BoxCollider2D myCollider;
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        myRigidBody.velocity = new Vector2 (enemyMoveSpeed,0f);

    }

    void Enemyflip()
    {
        
        bool isTouchingWall = myCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        transform.localScale = new Vector2 ((Mathf.Sign(myRigidBody.velocity.x)),1f);
        enemyMoveSpeed = -enemyMoveSpeed;
      
    }
    void OnTriggerExit2D(Collider2D other) 
    {
        
        
        Enemyflip();
    }
}
