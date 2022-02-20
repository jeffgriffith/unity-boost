using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    Material m;

    AudioSource audioSource;
    public float fuelBurnRate = 0.01f;
    public float fuelLevel = 100f;

    [SerializeReference] float thrustRate = 1000f;
    [SerializeReference] float rotationRate = 20f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        m = GetComponent<Renderer>().material;
        audioSource = GetComponent<AudioSource>();

        Debug.Log("got the rigid body at startup...");
        if (audioSource == null) {
            Debug.Log("COULD NOT GET AUDIO SOURCE!!!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ApplyRotation();
    }

    void ProcessThrust() {

        bool thrusting = Input.GetKey(KeyCode.Space);
        AdjustSound(thrusting);

        if (thrusting) {
            AdjustForFuel();
            if (!OutOfFuel())
                rb.AddRelativeForce(Vector3.up * thrustRate * Time.deltaTime);
        }
    }

    void AdjustSound(bool thrusting) {
        bool playSound = thrusting && !OutOfFuel();

        if (playSound) {
            if (!audioSource.isPlaying) {
                audioSource.Play();
            }
        } else if (audioSource.isPlaying) {
            audioSource.Pause();
        }
    } 

    void AdjustForFuel() {
        fuelLevel = Math.Max(0f, fuelLevel - fuelBurnRate);
        if (OutOfFuel()) {
            m.color = Color.red;
        }
    }

    void ApplyRotation() {
        
        if (Input.GetKey(KeyCode.A)) {
            Rotate(rotationRate);
        } else if (Input.GetKey(KeyCode.D)) {
            Rotate(-rotationRate);
        }
    }

    void Rotate(float rotationRate) {
        rb.freezeRotation = true; // Freeze physics rotation so that we can do it manually.
        transform.Rotate(rotationRate * Vector3.forward * Time.deltaTime);
        rb.freezeRotation = false;
    }

    bool OutOfFuel() {
        return fuelLevel == 0f;
    }
}
