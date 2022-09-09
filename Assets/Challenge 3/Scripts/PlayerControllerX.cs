using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;
    

    public float isLowEnough = 15.0f; 
    public float floatForce;
    public float gravityModifier;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip groundHitSound;


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        // While space is pressed and player is low enough, float up
        if (Input.GetKeyDown(KeyCode.Space) && !gameOver)
        {
            playerRb.AddForce(Vector3.up * floatForce, ForceMode.Impulse);
        }

        if(playerRb.transform.position.y > isLowEnough)
        {
            playerRb.transform.position = new Vector3(transform.position.x, isLowEnough, transform.position.z);
            playerRb.velocity = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground") && !gameOver)
        {
            playerAudio.PlayOneShot(groundHitSound, 1.0f);
            playerRb.AddForce(Vector3.up * 20, ForceMode.Impulse);
        }

        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        } 

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);

        }

    }

}
