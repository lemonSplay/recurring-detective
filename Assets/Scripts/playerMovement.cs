using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    Vector3 movement = new Vector3(0, 0, 0);
    public Animator playerAnimator;
    float moveHorizontal, moveVertical;

    public float speed = 20f;

    private void FixedUpdate()
    {
        transform.position += movement * speed * Time.fixedDeltaTime;
    }

    private void Update()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        movement = new Vector3(moveHorizontal, moveVertical, 0);

        if (Input.GetKey("a") && transform.rotation.y == 0)
            transform.Rotate(new Vector2(0, -180));
        else if (Input.GetKey("d") && transform.rotation.y == 1)
            transform.Rotate(new Vector2(0, 180));

        playerAnimator.SetFloat("Speed", Mathf.Abs(moveHorizontal) + Mathf.Abs(moveVertical));
    }

}
