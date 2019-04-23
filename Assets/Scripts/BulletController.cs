using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed;
    public float dmg;
    public float maxDistance;

    private float distance;

    void Start()
    {
        distance = 0;
    }

    void Update()
    {
        if (transform.parent.tag == "PlayerAttack")
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime * 3);
        }else{
            transform.Translate(Vector2.right * speed * Time.deltaTime * 2);

        }
        distance += speed * Time.deltaTime * 2; 
        /*if(distance >= 4 && transform.parent.name == "ShotgunBullet(Clone)"){
            Destroy(transform.parent.gameObject);
        }else if(distance >= 6 && transform.parent.name == "PistolBullet(Clone)"){
            Destroy(transform.parent.gameObject);
        }*/
        if(distance >= maxDistance)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player" && collision.tag != "Ethereal" && transform.parent.tag == "PlayerAttack" && collision.tag!="Untagged") 
        {
            if (collision.tag == "Killable")
            {
                collision.gameObject.GetComponent<Health>().takeDamage(dmg);
                if(transform.parent.name == "PistolBullet(Clone)"){
                    collision.gameObject.GetComponent<EnemyController>().ReceiveStun();
                    collision.gameObject.GetComponent<EnemyController>().KnockBack(transform.right);
                }
            }
            Destroy(transform.parent.gameObject);
        }
        else if (collision.tag != "Killable" && transform.parent.tag == "EnemyAttack")
        {
            if(collision.tag == "Player")
            {
                collision.gameObject.GetComponent<Health>().takeDamage(dmg);
            }
            Destroy(transform.parent.gameObject);
        }
    }
}
