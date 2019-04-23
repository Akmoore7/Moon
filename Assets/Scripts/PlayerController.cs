using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed;
    public Transform gunPivot;
    public Transform gun;
    public Transform corpse;
    public GameObject gunGameObject;

    public Sprite shotgun;
    public Sprite pistol;
    public Animator pistolAnim, shotgunAnim;
    public Animator GunAnimator;
    public SpriteRenderer spriteRenderer;
    public string[] guns = new string[2];
    public int currentGun = 0;

    public float camDistance;
    public Transform cam;

    public float swordDmg, colDmg;

    public Health hp;

    public Transform bullet;
    public Transform shotgunBullet;

    private Rigidbody2D rb2d;
    private DashAbility dashLogic;

    public Animator animator;
    private int time = 0;


    public enum ActState
    {
        Fire, Dash, Run
    }

    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        dashLogic = GetComponent<DashAbility>();
        gunGameObject = gun.gameObject;
        shotgun = Resources.Load<Sprite>("Shotgun1") as Sprite;
        pistol = Resources.Load<Sprite>("Pistol1") as Sprite;
        pistolAnim = Resources.Load<Animator>("PistolAnimator") as Animator;
        shotgunAnim = Resources.Load<Animator>("ShotgunAnimator") as Animator;

        guns[0] = "Pistol1";
        guns[1] = "Shotgun1";
        spriteRenderer = gun.GetComponent<SpriteRenderer>();

    }

    void Update()
    {
        // rotate gun
        if (!GameManager.instance.isStopped)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseDelta = mouseWorld - gunPivot.position;
            float angle = Mathf.Atan2(mouseDelta.y, mouseDelta.x);
            gunPivot.rotation = Quaternion.Euler(0, 0, angle * 180 / Mathf.PI);

            if(angle > 1.5 || angle < -1.5){
                spriteRenderer.flipY = true;
            }else{
                spriteRenderer.flipY = false;
            }

            // translate camera
            Vector3 camTarget = new Vector3(camDistance * Mathf.Cos(angle), camDistance * Mathf.Sin(angle), -10);
            cam.localPosition = Vector3.Lerp(cam.localPosition, camTarget, 0.1f);
            time++;
            if (Input.GetButton("Fire1"))
            {
                switch(guns[currentGun]){
                    case "Pistol1":
                        if(time > 15)
                        {
                            gunGameObject.GetComponent<Animator>().SetTrigger("FirePistol");
                            GetComponent<PlayerSoundController>().FirePistol();
                            shootBullet(bullet, gun.position, gun.rotation);
                            time = 0;
                        }
                        break;
                    case "Shotgun1":
                        if(time > 70)
                        {
                            gunGameObject.GetComponent<Animator>().SetTrigger("FireShotgun");
                            GetComponent<PlayerSoundController>().FireShotgun();
                            Quaternion bullet2Rotation = Quaternion.Euler(gun.rotation.eulerAngles.x, gun.rotation.eulerAngles.y, gun.rotation.eulerAngles.z - 5);
                            Quaternion bullet3Rotation = Quaternion.Euler(gun.rotation.eulerAngles.x, gun.rotation.eulerAngles.y, gun.rotation.eulerAngles.z - 10);
                            Quaternion bullet4Rotation = Quaternion.Euler(gun.rotation.eulerAngles.x, gun.rotation.eulerAngles.y, gun.rotation.eulerAngles.z - 15);
                            Quaternion bullet5Rotation = Quaternion.Euler(gun.rotation.eulerAngles.x, gun.rotation.eulerAngles.y, gun.rotation.eulerAngles.z + 5);
                            Quaternion bullet6Rotation = Quaternion.Euler(gun.rotation.eulerAngles.x, gun.rotation.eulerAngles.y, gun.rotation.eulerAngles.z + 10);
                            Quaternion bullet7Rotation = Quaternion.Euler(gun.rotation.eulerAngles.x, gun.rotation.eulerAngles.y, gun.rotation.eulerAngles.z + 15);

                            shootBullet(shotgunBullet, gun.position, bullet2Rotation);
                            shootBullet(shotgunBullet, gun.position, bullet3Rotation);
                            shootBullet(shotgunBullet, gun.position, bullet4Rotation);
                            shootBullet(shotgunBullet, gun.position, bullet5Rotation);
                            shootBullet(shotgunBullet, gun.position, bullet6Rotation);
                            shootBullet(shotgunBullet, gun.position, bullet7Rotation);
                            shootBullet(shotgunBullet, gun.position, gun.rotation);
                            time = 0;
                        } 
                        break;
                    default:
                        //shootBullet(bullet, gun.position, gun.rotation);
                        break;
                }
            }

            if (Input.GetKeyDown(KeyCode.Q)){

                currentGun = (currentGun + 1) % guns.Length; 
                switch(guns[currentGun]){
                    case "Pistol1":
                        gunGameObject.GetComponent<Animator>().SetTrigger("ToPistol");
                        gunGameObject.GetComponent<SpriteRenderer>().sprite = pistol;
                        break;
                    case "Shotgun1":
                        gunGameObject.GetComponent<Animator>().SetTrigger("ToShotgun");
                        gunGameObject.GetComponent<SpriteRenderer>().sprite = shotgun;
                        break;
                    default: 
                        gunGameObject.GetComponent<SpriteRenderer>().sprite = pistol;
                        break;

                }
            }
        }
        if((hp.RemainingHP / hp.MaxHP) < 0.75) {
            SpriteRenderer playerSprite = GetComponent<SpriteRenderer>();
            float flashSpeed = 4.0f * (1-(hp.RemainingHP / hp.MaxHP));
            float color = (Mathf.Sin(2.0f * Mathf.PI * flashSpeed * Time.time) + 1.0f) / 2.0f;
            playerSprite.color = new Color(1.0f, color, color);
        }
        else {
            SpriteRenderer playerSprite = GetComponent<SpriteRenderer>();
            playerSprite.color = new Color(1.0f, 1.0f, 1.0f);
        }
    }

    void FixedUpdate()
    {
        if (!dashLogic.dashing && !GameManager.instance.isStopped)
        {
            float hSpeed = Input.GetAxis("Horizontal");
            float vSpeed = Input.GetAxis("Vertical");
            Vector2 v = new Vector2(hSpeed, vSpeed);
            rb2d.velocity = v * maxSpeed;
            animator.SetFloat("Direction", hSpeed);
            if (hSpeed == 0f && vSpeed == 0f)
            {
                animator.SetBool("Moving", false);
            }
            else
            {
                animator.SetBool("Moving", true);
            }
            if (hSpeed == 0f)
                animator.SetInteger("X", 0);
            else
                animator.SetInteger("X", 1);
        }
        if (!hp.alive)
        {
            GetComponent<SpriteRenderer>().sprite = corpse.gameObject.GetComponent<SpriteRenderer>().sprite;
            //           Instantiate(corpse, new Vector3(rb2d.gameObject.transform.position.x + 1f, rb2d.gameObject.transform.position.y - 0.7f, rb2d.gameObject.transform.position.z), Quaternion.identity);
            //   Die.exe;
            //Scene thisScene = SceneManager.GetActiveScene();
            //SceneManager.LoadScene(thisScene.name);

        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManager.instance.isStopped)
        {
            Debug.Log("Collision detected");
            if (collision.tag == "Killable" && dashLogic.frame == DashAbility.Frames.Damage)
            {
                if (collision.gameObject.GetComponent<Health>().takeDamage(swordDmg))
                {
                    dashLogic.setKilled();
                    hp.takeHeal(1);
                }
                Debug.Log("Contact");
            }
            else if (collision.tag == "Killable")
            {
                // GetComponent<Health>().takeDamage(colDmg);
                // Take damage if contact in vulnerable 
            }
            else if (collision.tag == "interact")
            {


            }
        }
    }

    void shootBullet(Transform bulletType, Vector3 position, Quaternion rotation){
        var shooting = Instantiate(bulletType, position, rotation);
        shooting.tag = "PlayerAttack";
    }
}