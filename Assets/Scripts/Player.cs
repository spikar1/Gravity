using UnityEngine;
using System;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour {

    Controller2D controller;
    Animator anim;

    public Vector2 velocity;
    public LayerMask traps;

    public float gravity = -.6f;
    public float maxFallSpeed = 10;

    public float maxSpeed = 12;
    public float jumpForce = 16;
    public float acceleration = .4f;
    public float deacceleration = 1f;

    public bool grounded;


    public bool gravityLock = false;

    void Start()
    {
        controller = GetComponent<Controller2D>();
        anim = GetComponent<Animator>();
        
    }

    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space) && !gravityLock)
        {
            gravity *= -1;
        }
        
        if(controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }
        if (controller.collisions.left || controller.collisions.right)
        {
            velocity.x = 0;
        }
        
        grounded = (gravity > 0) ? controller.collisions.above : controller.collisions.below;
        anim.SetBool("Grounded", grounded);
        

        if (Input.GetKeyDown(KeyCode.UpArrow) && grounded)
            velocity.y = jumpForce *.016f * Mathf.Sign(-gravity);

        if (Input.GetKeyUp(KeyCode.UpArrow) && Mathf.Sign(gravity) != Mathf.Sign(velocity.y))
            velocity.y *= .5f;

        if (input.x != 0)
        {
            
            if(Mathf.Sign(input.x) != Mathf.Sign(velocity.x) && velocity.x != 0)
            {
                velocity.x = Mathf.MoveTowards(velocity.x, 0f, deacceleration * Time.deltaTime);
            }
            else
            {

                if (Mathf.Abs(velocity.x) < maxSpeed * .016f)
                    velocity.x += acceleration * Time.deltaTime * input.x;
                else if (Mathf.Sign(input.x) == Mathf.Sign(velocity.x))
                    velocity.x = input.x * .016f * maxSpeed;
            }
            

        }
        else if (Mathf.Abs(velocity.x) > deacceleration * Time.deltaTime)
            velocity.x -= deacceleration*Time.deltaTime * Mathf.Sign(velocity.x);//Mathf.Sign(velocity.x) * deacceleration * Time.deltaTime;
        else
            velocity.x = 0;

        //velocity.x = input.x * Time.deltaTime * speed;
        velocity.y += gravity * .016f;
        if (Math.Abs(velocity.y) > maxFallSpeed * .016f)
            velocity.y = velocity.y.Sign() * maxFallSpeed * .016f;
            

        //print(velocity);
        controller.Move(velocity);
        
        
        anim.SetInteger("InputX", (int)input.x);

        if (input.x != 0)
            transform.localScale = new Vector3(input.x, 1, 1);
        transform.localScale = new Vector3(transform.localScale.x, -Mathf.Sign(gravity), 1);

        if(Input.GetKeyDown(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.UnloadScene(SceneManager.GetActiveScene().buildIndex);
            SceneManager.LoadScene(scene.buildIndex);
        }

        if (Physics2D.OverlapArea(transform.position - controller.collider.bounds.extents*.8f, transform.position + controller.collider.bounds.extents*.8f,traps))
        {
            Scene scene = SceneManager.GetActiveScene();
            //SceneManager.UnloadScene(SceneManager.GetActiveScene().buildIndex); 
            SceneManager.LoadScene(scene.buildIndex);
        }
    }
    

}
