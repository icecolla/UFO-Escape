using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ufo_script : MonoBehaviour
{
    public GameObject rotator_body;
    public GameObject ufo_obj;
    public GameObject crashed_ufo;

    public Rigidbody leftLegRigidbody;
    public Rigidbody rightLegRigidbody;

    public float speed = 10f;
    public float rotationMult = 0.8f;

    public SceneLoader sceneLoader;

    public Camera cameraz;

    private bool crashed = false;
    public bool isFlying = true;

    public Slider rightSlider;
    public Slider leftSlider;

    public Text rightText;
    public Text leftText;

    public Text altMeter;
    public GameObject finishGame;

    public float pushForce = 10f;

    private ControllerColliderHit _contact;

    public AudioSource audios;
    public AudioClip engineStartSFX;
    public AudioClip engineEndSFX;
    public AudioClip fallSFX;
    public AudioClip crashedSFX;
    public AudioClip lostControlSFX;
    public AudioClip finishPlatformSFX;

    public AudioClip[] obstacleBangSFX;


    private void Start()
    {
        rotator_body = GameObject.Find("Dynamic_body");

        audios = GetComponent<AudioSource>();
    }

    private void Update()
    {
        rotator_body.transform.Rotate(0, speed, 0, Space.Self);
    }

    private void FixedUpdate()
    {
        if (isFlying)
        {
            EnginesThrust();
        }
    }

    private void EnginesThrust()
    {
        if (!crashed)
        {
            Vector3 minForce = Vector3.up * speed * rotationMult;
            Vector3 maxForce = Vector3.up * speed;

            Vector3 leftForce = Vector3.zero;
            Vector3 rightForce = Vector3.zero;

            if (Input.GetKey(KeyCode.A))
            {
                leftForce = minForce;
                rightForce = maxForce;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                leftForce = maxForce;
                rightForce = minForce;
            }
            else if (Input.GetKey(KeyCode.Space))
            {
                leftForce = maxForce;
                rightForce = maxForce;
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                ufoEngineStartSFX();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                ufoEngineStartSFX();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                ufoEngineStartSFX();
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                //audios.Stop();
                ufoEngineEndSFX();
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                //audios.Stop();
                ufoEngineEndSFX();
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                //audios.Stop();
                ufoEngineEndSFX();
            }

            leftLegRigidbody.AddRelativeForce(leftForce);
            rightLegRigidbody.AddRelativeForce(rightForce);
            EnginesThrustUI(leftForce, rightForce, maxForce);
        }
    }

    private void EnginesThrustUI(Vector3 leftForce, Vector3 rightForce, Vector3 maxForce)
    {
        leftSlider.value = Mathf.Lerp(0, 1, leftForce.y / maxForce.y);
        rightSlider.value = Mathf.Lerp(0, 1, rightForce.y / maxForce.y);

        rightText.text = rightForce.y + "MWt";
        leftText.text = leftForce.y + "MWt";

        altMeter.text = $"{(int)transform.position.y}";
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle" && !crashed)
        {
            audios.PlayOneShot(bangObstacleSFX());
            crashed = true;
            ufo_obj.SetActive(false);
            Instantiate(crashed_ufo, transform.position, transform.rotation);
            ufoCrashedSFX();
            cameraz.GetComponent<OrbitCamera>().enabled = !enabled;
            StartCoroutine(ReloadScene());
        }

        if (collision.gameObject.tag == "Crates")
        {
            audios.PlayOneShot(bangObstacleSFX());
        }

        if (collision.gameObject.tag == "Finish")
        {
            audios.PlayOneShot(finishPlatformSFX, 1f);
            StartCoroutine(NextScene());
        }
        if (collision.gameObject.tag == "FinishGame")
        {
            audios.PlayOneShot(finishPlatformSFX, 1f);
            finishGame.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Zone"))
        {
            isFlying = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Zone"))
        {
            isFlying = false;
            ufoLostControlSfx();
            altMeter.text = "fail";
            falloutSfx();
        }
    }

    IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(1.5f);
        sceneLoader.LoadScene(0);
    }

    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(1.5f);
        sceneLoader.LoadScene(1);
    }

    /// <summary>
    /// При распознавании столкновения данные этого столкновения сохраняются в методе обратного вызова.
    /// </summary>
    /// <param name="hit"></param>
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _contact = hit;

        // Проверяем, есть ли участвующего в столкновении объекта компонент Rigidbody, обеспечивающий реакцию на приложенную силу.
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body != null && !body.isKinematic)
        {
            // Назначаем физическому телу скорость.
            body.velocity = hit.moveDirection * pushForce;
        }
    }


    public void ufoEngineStartSFX()
    {
        audios.clip = engineStartSFX;
        audios.volume = .3f;
        audios.Play();
    }
    public void ufoEngineEndSFX()
    {
        audios.clip = engineEndSFX;
        audios.volume = .3f;
        audios.Play();
    }

    public void ufoCrashedSFX()
    {
        audios.clip = crashedSFX;
        audios.volume = 1f;
        audios.Play();
    }

    public void ufoLostControlSfx()
    {
        audios.clip = lostControlSFX;
        audios.volume = 1f;
        audios.Play();
    }
    public void falloutSfx()
    {
        //audios.clip = fallSFX;
        //audios.volume = 1f;
        audios.PlayOneShot(fallSFX, 1f);
    }
    public AudioClip bangObstacleSFX()
    {
        return obstacleBangSFX[Random.Range(0, obstacleBangSFX.Length)];
    }
}
