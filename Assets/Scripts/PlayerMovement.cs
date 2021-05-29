using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;
    void Start ()
    {
        m_Animator = GetComponent<Animator> ();
        m_Rigidbody = GetComponent<Rigidbody> ();
        m_AudioSource = GetComponent<AudioSource>();
    }

    // void update ()
    // {
    //     if(!isMuted){
    //         if (Input.GetKeyDown(KeyCode.M))
    //         {
    //             isMuted = !isMuted;
    //             m_AudioSource.volume = 0f;
    //         }
    //     }
    //     else{
    //         if (Input.GetKeyDown(KeyCode.M))
    //         {
    //             isMuted = !isMuted;
    //             m_AudioSource.volume = 1f;
    //         }
    //     }
    // }

    void FixedUpdate ()
    {
        float horizontal = Input.GetAxis ("Horizontal");
        float vertical = Input.GetAxis ("Vertical");
        
        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize ();

        bool hasHorizontalInput = !Mathf.Approximately (horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately (vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;

        if (Input.GetKeyDown(KeyCode.M) & m_AudioSource.isPlaying)
        {
            m_AudioSource.volume = 0;
        }
        else if(Input.GetKeyDown(KeyCode.M) & !m_AudioSource.isPlaying)
        {
            m_AudioSource.volume = 1;
        }

        
        if(isWalking)
        {
            if(!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play ();
            }
        }
        else
        {
            m_AudioSource.Stop ();
        }
        m_Animator.SetBool ("IsWalking", isWalking);

        Vector3 desiredForward = Vector3.RotateTowards (transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation (desiredForward);
    }

    void OnAnimatorMove ()
    {
        m_Rigidbody.MovePosition (m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
        m_Rigidbody.MoveRotation (m_Rotation);
    }
}