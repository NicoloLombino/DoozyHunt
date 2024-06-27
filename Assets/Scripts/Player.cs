using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private CharacterController cc;
    private AudioSource audioSource;

    [SerializeField]
    private EventsManager eventsManager;
    [SerializeField]
    private GameManager gameManager;

    [Header("Stats")]
    [SerializeField]
    private float speed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float currentHealth;
    [SerializeField]
    private float maxStamina;
    [SerializeField]
    private float currentStamina;

    [Header("Hands")]
    [SerializeField]
    private Animator animatorHandRight;
    [SerializeField]
    private Animator animatorHandRightMelee;
    [SerializeField]
    private Animator animatorHandRightGun;
    [SerializeField]
    private Animator animatorHandLeft;
    [SerializeField]
    private Transform leftHandThrowTransform;
    [SerializeField]
    private GameObject leftHand;

    [Header("Weapons")]
    [SerializeField]
    private GameObject baseRightWeapon;
    [SerializeField]
    private GameObject gunRightHand;
    [SerializeField]
    private WeaponBase[] leftHandWeapons;
    private int leftWeaponSelected;

    [Header("Others")]
    [SerializeField]
    private Transform endThrowPosition;

    [Header("UI components")]
    [SerializeField]
    private Image leftWeaponImage;
    [SerializeField]
    private Image InteractionUI;
    [SerializeField]
    private Slider healthSlider;
    [SerializeField]
    private Slider staminaSlider;
    [SerializeField]
    private GameObject gameOverUI;

    private bool isLeftHandWeaponUsed;
    public bool hasGunUnlocked;
    internal bool canShot;

    private bool canRun = true;
    private bool isRecoveringStamina;

    [Header("Audio components")]
    [SerializeField]
    private AudioClip easterEggMusicClip;
    [SerializeField]
    private AudioClip vodkaClip;
    [SerializeField]
    private AudioClip TakeGunClip;
    [SerializeField]
    private AudioClip OpenDoorClip;

    // UglyFastFix
    private float leftWeaponTimerFix;
    [SerializeField]
    private GameObject baseRightWeapon_AnimationTransform;
    [SerializeField]
    private GameObject gunRightHand_AnimationTransform;
    private Vector3 RightHandMeleeStartRotation;
    private Vector3 RightHandGunStartRotation;
    [SerializeField]
    private GameObject baseRightWeapon_AnimationTransform2;
    [SerializeField]
    private GameObject gunRightHand_AnimationTransform2;
    private Vector3 RightHandMeleeStartRotation2;
    private Vector3 RightHandGunStartRotation2;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        eventsManager.weaponUsedEvent.AddListener(() => HandleWeaponUsedEvent());

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //EasyUglyFix
        RightHandMeleeStartRotation = baseRightWeapon_AnimationTransform.transform.localEulerAngles;
        RightHandGunStartRotation = gunRightHand_AnimationTransform.transform.localEulerAngles;
        RightHandMeleeStartRotation2 = baseRightWeapon_AnimationTransform2.transform.localEulerAngles;
        RightHandGunStartRotation2 = gunRightHand_AnimationTransform2.transform.localEulerAngles;
    }
    void Update()
    {
        transform.position = new Vector3(transform.position.x, 1.1f, transform.position.z);
        ReadAttackInput();
        ReadScrollWeaponsInput();
        ReadMovementInput();
        DoRaycastAndSetUIColor();
        ReadChangeRightHandWeapon();
        EasyUglyFix();
    }

    private void ReadMovementInput()
    {
        if(Input.GetKey(KeyCode.LeftShift) && canRun && !isRecoveringStamina)
        {
            speed = runSpeed;
            currentStamina -= Time.deltaTime;
            staminaSlider.value = currentStamina / maxStamina;
            currentStamina = Mathf.Max(currentStamina, 0);

            if(currentStamina <= 0 && !isRecoveringStamina)
            {
                Debug.Log("END RUN");
                canRun = false;            
            }
        }
        else
        {
            speed = walkSpeed;   
            staminaSlider.value = currentStamina / maxStamina;
            currentStamina = Mathf.Min(maxStamina, currentStamina);

            if(canRun)
            {
                currentStamina += Time.deltaTime;
            }
        }

        if(currentStamina <= 0 && !isRecoveringStamina && Input.GetKeyUp(KeyCode.LeftShift))
        {
            Debug.Log("UP");
            isRecoveringStamina = true;
            StartCoroutine(RecoverStaminaTimer());
        }


        Vector3 movement = (transform.forward * Input.GetAxis("Vertical") +
            transform.right * Input.GetAxis("Horizontal")).normalized;
        cc.Move(movement * speed);
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, mouseX);
    }

    private void ReadAttackInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            MeleeAttack();
        }

        else if(Input.GetMouseButtonDown(1) && !isLeftHandWeaponUsed)
        {
            ThrowAttack();
        }
    }

    private void MeleeAttack()
    {
        if(InteractionUI.color == Color.green)
        {
            return;
        }

        animatorHandRight.SetTrigger("Attack");
    }

    private void ReadScrollWeaponsInput()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            ScrollUp();
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            ScrollDown();
        }
    }

    private void ScrollUp()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel") * 10;
        if (leftWeaponSelected < leftHandWeapons.Length - 1)
        {
            leftWeaponSelected += (int)scroll;
            leftWeaponImage.sprite = leftHandWeapons[leftWeaponSelected].sprite;
        }
    }

    private void ScrollDown()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel") * 10;
        if(leftWeaponSelected > 0)
        {
            leftWeaponSelected += (int)scroll;
            leftWeaponImage.sprite = leftHandWeapons[leftWeaponSelected].sprite;
        }
    }

    private void ReadChangeRightHandWeapon()
    {
        if(Input.GetKeyDown(KeyCode.F) && hasGunUnlocked)
        {
            gunRightHand.SetActive(!gunRightHand.activeInHierarchy);
            baseRightWeapon.SetActive(!baseRightWeapon.activeInHierarchy);

            animatorHandRight = baseRightWeapon.activeInHierarchy ?
                animatorHandRightMelee : animatorHandRightGun;

            //EasyUglyFix
            baseRightWeapon_AnimationTransform.transform.localEulerAngles = RightHandMeleeStartRotation;
            gunRightHand_AnimationTransform.transform.localEulerAngles = RightHandGunStartRotation;
            baseRightWeapon_AnimationTransform2.transform.localEulerAngles = RightHandMeleeStartRotation2;
            gunRightHand_AnimationTransform2.transform.localEulerAngles = RightHandGunStartRotation2;
            baseRightWeapon.GetComponent<MeleeWeapon>().EndAttack();
        }
    }

    private void ThrowAttack()
    {
        if(leftHandWeapons[leftWeaponSelected].gameObject.TryGetComponent<ThrowingWeapon>(out ThrowingWeapon throwingWeapon))
        {
            animatorHandLeft.SetTrigger("Throw");

            GameObject obj = Instantiate(leftHandWeapons[leftWeaponSelected].gameObject,
                leftHandThrowTransform.position,
                leftHandThrowTransform.rotation);

            if(obj.GetComponent<ThrowingWeapon>().eventsManager == null)
            {
                obj.GetComponent<ThrowingWeapon>().eventsManager = this.eventsManager;
            }
            obj.transform.parent = leftHandThrowTransform;
            obj.GetComponent<ThrowingWeapon>().DoThrow(obj, endThrowPosition);

            //StartCoroutine(ObjectThrowing(obj));
            //throwingWeapon.DoThrow(obj, endThrowPosition);
        }
        else
        {
            leftHand.SetActive(false);
            leftHandWeapons[leftWeaponSelected].gameObject.SetActive(true);
        }
        isLeftHandWeaponUsed = true;
    }

    private void DoRaycastAndSetUIColor()
    {
        if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 5, LayerMask.GetMask("Door")))
        {
            InteractionUI.color = Color.green;
            if (Input.GetMouseButton(0))
            {
                if(gameManager.money >= hit.collider.gameObject.GetComponent<Door>().pointsForOpen)
                {
                    gameManager.money -= hit.collider.gameObject.GetComponent<Door>().pointsForOpen;
                    Destroy(hit.collider.gameObject);
                    gameManager.UpdateMoneyOnUI();
                    audioSource.PlayOneShot(OpenDoorClip);
                }
                else
                {
                    // sound
                }
            }

        }
        else if (Physics.Raycast(transform.position, transform.forward, 3, LayerMask.GetMask("Enemy")))
        {
            InteractionUI.color = Color.red;
        }
        else
        {
            InteractionUI.color = Color.white;
        }

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit2, 5, LayerMask.GetMask("EasterEggMusic")))
        {
            if (Input.GetMouseButton(0))
            {
                gameManager.easterEggMusicCounter++;
                hit2.collider.enabled = false;
                audioSource.PlayOneShot(easterEggMusicClip);
                if (gameManager.easterEggMusicCounter >= 3)
                {
                    gameManager.StartMusicEasterEgg();
                }
            }
        }
    }

    private void HandleWeaponUsedEvent()
    {
        isLeftHandWeaponUsed = false;
        if(!leftHand.activeInHierarchy)
        {
            leftHand.SetActive(true);
        }
    }

    public void ReceiveDamage(int damage)
    {
        currentHealth -= damage;
        healthSlider.value = currentHealth / maxHealth;
        if(currentHealth <= 0)
        {
            gameOverUI.SetActive(true);
            this.enabled = false;
            CheckScore();
            StartCoroutine(GoToMenuScene());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("UnlockGun"))
        {
            hasGunUnlocked = true;
            Destroy(other.gameObject);
            audioSource.PlayOneShot(TakeGunClip);
        }

        if (other.CompareTag("Vodka"))
        {
            currentHealth += 30;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
            healthSlider.value = currentHealth / maxHealth;
            Destroy(other.gameObject);
            audioSource.PlayOneShot(vodkaClip);
        }
    }

    private IEnumerator RecoverStaminaTimer()
    {
        yield return new WaitForSecondsRealtime(2);
        canRun = true;
        isRecoveringStamina = false;
    }

    private IEnumerator GoToMenuScene()
    {
        yield return new WaitForSecondsRealtime(5);
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    private void CheckScore()
    {
        if(gameManager.score > PlayerPrefs.GetInt("MaxScore"))
        {
            PlayerPrefs.SetInt("MaxScore", gameManager.score);
        }
    }

    private void EasyUglyFix()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            speed = 0;
        }

        if(isLeftHandWeaponUsed && leftWeaponTimerFix < 10)
        {
            leftWeaponTimerFix += Time.deltaTime;
            if(leftWeaponTimerFix >= 10)
            {
                leftWeaponTimerFix = 0;
                isLeftHandWeaponUsed = false;
            }
        }
    }
}
