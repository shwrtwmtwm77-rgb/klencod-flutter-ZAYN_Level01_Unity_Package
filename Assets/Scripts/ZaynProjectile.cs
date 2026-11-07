using UnityEngine;
public class ZaynProjectile : MonoBehaviour{
 public float speed=20f; public int damage=35;
 void Start(){Destroy(gameObject,4f);}
 void Update(){transform.position+=transform.forward*speed*Time.deltaTime;}
 void OnTriggerEnter(Collider other){var e=other.GetComponentInParent<ZaynEnemy>();if(e)e.TakeDamage(damage);var b=other.GetComponentInParent<ZaynBoss>();if(b)b.TakeDamage(damage);if(e||b)Destroy(gameObject);}
}
