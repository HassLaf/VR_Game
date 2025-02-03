using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class CharacterControllerScript : MonoBehaviour
{
    public TextMeshProUGUI toothCounterText; // Reference to the UI text
    public TextMeshProUGUI timerText;
    private int toothCount = 0; // Variable to store collected teeth
    public float speed = 5f;
    public float gravity = 9.81f;
    public float jumpHeight = 2f;
    private float timer = 300f; // Initialiser à 5 minutes (300 secondes)
    private bool timerActive = true; // Booléen pour vérifier si le timer est actif


    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    
    void Start()
    {
         controller = GetComponent<CharacterController>();
         UpdateToothCounter();
    
        if (controller == null)
        {
            Debug.LogError("CharacterController component is missing on " + gameObject.name);
            enabled = false; // Disable script if the component is missing
        }
    }
    
    void OnTriggerEnter(Collider other)
{
    if (other.gameObject.CompareTag("tooth"))
    {
        // Collect the teeth
        Debug.Log("Tooth Collected!");
        toothCount++; // Increase count
        UpdateToothCounter(); // Update UI
        Destroy(other.gameObject); // Remove the collected tooth
        
    }
}
    void Update()
    {
        // Gérer le timer
        if (timerActive)
        {
            timer -= Time.deltaTime; // Décrémente le timer
            if (timer <= 0f)
            {
                timer = 0f; // Evite que le timer devienne négatif
                timerActive = false; // Arrêter le timer une fois qu'il atteint zéro
                Debug.Log("Timer finished!");
                // Tu peux ajouter un événement ici, comme arrêter le jeu ou afficher un message
            }
            UpdateTimerDisplay(); // Mettre à jour l'affichage du timer
        }
        // Check if the character is on the ground
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small value to keep grounded
        }


        // Get input for movement
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * speed * Time.deltaTime);


        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
        }


        // Apply gravity
        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            // Calculer les minutes et secondes
            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);
            // Affichage du timer sur l'UI au format "MM:SS"
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            Debug.LogError("timerText is NULL! Assign it in the Inspector.");
        }
    }
    
    void UpdateToothCounter(){
    if (toothCounterText != null)
    {
        toothCounterText.text = "Teeth: " + toothCount;
        Debug.Log("UI Updated: Teeth Count = " + toothCount);
    }
    else
    {
        Debug.LogError("toothCounterText is NULL! Assign it in the Inspector.");
    }
}
}
