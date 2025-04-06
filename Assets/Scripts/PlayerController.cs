using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
       // Rigidbody of the player.
       private Rigidbody rb; 

       // Variable to keep track of collected "PickUp" objects.
       private int count;

       // Movement along X and Y axes.
       private float movementX;
       private float movementY;

       // Speed at which the player moves.
       public float speed = 0;

       // UI text component to display count of "PickUp" objects collected.
       public TextMeshProUGUI countText;

       // UI object to display winning text.
       public GameObject winTextObject;

       // DOUBLE JUMP STUFF
       private bool isGrounded = true;
       private int jumpCount = 0;
       public float jumpForce = 5f;


       // Start is called before the first frame update.
       void Start()
       {
       // Get and store the Rigidbody component attached to the player.
              rb = GetComponent<Rigidbody>();

       // Initialize count to zero.
              count = 0;

       // Update the count display.
              SetCountText();

       // Initially set the win text to be inactive.
              winTextObject.SetActive(false);
       }
       
       // This function is called when a move input is detected.
       void OnMove(InputValue movementValue)
       {
       // Convert the input value into a Vector2 for movement.
              Vector2 movementVector = movementValue.Get<Vector2>();

       // Store the X and Y components of the movement.
              movementX = movementVector.x; 
              movementY = movementVector.y; 
       }

       // FixedUpdate is called once per fixed frame-rate frame.
       private void FixedUpdate() 
       {
       // Create a 3D movement vector using the X and Y inputs.
              Vector3 movement = new Vector3 (movementX, 0.0f, movementY);

       // Apply force to the Rigidbody to move the player.
              rb.AddForce(movement * speed); 
       }

       private void OnCollisionEnter(Collision collision)
       {
              // DOUBLE JUMP RESET
              if (collision.gameObject.CompareTag("Ground"))
              {
                     isGrounded = true;
                     jumpCount = 0;
              }

              if (collision.gameObject.CompareTag("Enemy"))
              {
                     // Destroy the current object
                     Destroy(gameObject); 
                     // Update the winText to display "You Lose!"
                     winTextObject.gameObject.SetActive(true);
                     winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
              }
       }
       
       void OnTriggerEnter(Collider other) 
       {
       // Check if the object the player collided with has the "PickUp" tag.
       if (other.gameObject.CompareTag("PickUp")) 
              {
       // Deactivate the collided object (making it disappear).
              other.gameObject.SetActive(false);

       // Increment the count of "PickUp" objects collected.
              count = count + 1;

       // Update the count display.
              SetCountText();
              }
       }

       // Function to update the displayed count of "PickUp" objects collected.
       void SetCountText() 
       {
              // Update the count text with the current count.
              countText.text = "Count: " + count.ToString();

              // Check if the count has reached or exceeded the win condition.
              if (count >= 12)
              {
                     // Display the win text.
                     winTextObject.SetActive(true);
                     Destroy(GameObject.FindGameObjectWithTag("Enemy"));
              }
       }

       // DOUBLE JUMP FUNCTION
       void OnJump()
       {
              if (jumpCount < 2)
              {
                     // Reset vertical velocity to prevent stacking jumps
                     rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

                     // Apply upward impulse to jump
                     rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

                     jumpCount++;
                     isGrounded = false;
              }
       }
}