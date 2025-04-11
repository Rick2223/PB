using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footsteps : MonoBehaviour
{
    private CharacterController controller;
    private AudioSource footstepSource;
    private bool isWalking = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Create audio source
        footstepSource = gameObject.AddComponent<AudioSource>();
        Sound footstepSound = System.Array.Find(AudioManager.Instance.effectSound, s => s.name == "WalkingSound");

        if (footstepSound != null)
        {
            footstepSource.clip = footstepSound.clip;
            footstepSource.loop = true;
            footstepSource.volume = AudioManager.Instance.effectSource.volume;
        }
        else
        {
            Debug.LogWarning("Footstep sound not found in AudioManager!");
        }
    }

    void Update()
    {
        // Check input instead of velocity
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        bool isMoving = (horizontal != 0f || vertical != 0f) && controller.isGrounded;

        if (isMoving && !isWalking)
        {
            footstepSource.Play();
            isWalking = true;
        }
        else if (!isMoving && isWalking)
        {
            footstepSource.Stop();
            isWalking = false;
        }

        // Sync volume with AudioManager
        footstepSource.volume = AudioManager.Instance.effectSource.volume;
    }
}
