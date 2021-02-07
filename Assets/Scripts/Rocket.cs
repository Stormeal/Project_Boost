using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;
    private Game game;
    bool toggleCollision;

    [SerializeField] Image imgLife1, imgLife2, imgLife3;
    [SerializeField] static int health = 3;
    [SerializeField] float rcsThrust = 1000f;
    [SerializeField] float mainThrust = 10f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip levelCompleteSound;
    [SerializeField] AudioClip deathSound;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;


    enum State { Alive, Dyin, Transcending };
    State state = State.Alive;


    // Start is called before the first frame update
    void Start()
    {
        game = FindObjectOfType<Game>();
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        UpdateHealth();
    }

    // Update is called once per frame
    void Update()
    {
        // Todo Stop sound on death
        if (state == State.Alive)
        {
            ProcessInput();
        }
        else
        {
            return;
        }
    }

    private void ProcessInput()
    {
        RespondToThrustInput();
        RespondToRotateInput();
        RespondToCKey();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) { return; } // ignore collisions when dead

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                if (!toggleCollision)
                {
                    StartDeathSequence();
                }
                break;
        }
    }

    private void UpdateHealth()
    {
        switch (health)
        {
            case 1:
                imgLife1.enabled = true;
                imgLife2.enabled = false;
                imgLife3.enabled = false;
                break;
            case 2:
                imgLife1.enabled = true;
                imgLife2.enabled = true;
                imgLife3.enabled = false;
                break;
            case 3:
                imgLife1.enabled = true;
                imgLife2.enabled = true;
                imgLife3.enabled = true;
                break;
            default:
                break;
        }

    }


    private void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(levelCompleteSound);
        successParticles.Play();
        Invoke("LoadNextLevel", 2f);
    }
    private void StartDeathSequence()
    {
        state = State.Dyin;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        deathParticles.Play();
        health--;
        UpdateHealth();
        if (health == 0)
        {
            Invoke("LoadFirstLevel", 2f);
            health = 3;

        }
        else
        {
            Invoke("ReloadCurrentLevel", 2f);
        }
    }

    private void LoadNextLevel()
    {
        game.LoadNextLevel();
    }

    private void LoadFirstLevel()
    {
        game.LoadFirstLevel();
    }

    private void ReloadCurrentLevel()
    {
        game.ReloadCurrentLevel();
    }


    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space)) // can thrust while rotating 
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying) // so it doesn't layer
        {
            audioSource.PlayOneShot(mainEngine);
            mainEngineParticles.Play();
        }
    }

    private void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true; // take manual control of rotation

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        rigidBody.freezeRotation = false; // resume physics control of rotation
    }

    private void RespondToCKey()
    {
        if (Input.GetKeyDown(KeyCode.C) && Debug.isDebugBuild)
        {
            if (toggleCollision != true)
            {
                toggleCollision = true;

            }
            else
            {
                toggleCollision = false;
            }
        }
    }

}
