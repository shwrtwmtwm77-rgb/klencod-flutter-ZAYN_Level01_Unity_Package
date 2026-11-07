using UnityEngine;
public class ZaynBoss : MonoBehaviour{
 public int maxHealth=500,currentHealth,damage=25; public float speed=4f,attackRange=3f; Transform player; float nextAttack;
 void Start(){currentHealth=maxHealth;if(ZaynCompleteGame.Instance)player=ZaynCompleteGame.Instance.transform;}
 void Update(){if(!player)return;float d=Vector3.Distance(transform.position,player.position);
  if(d>attackRange){Vector3 t=player.position;t.y=transform.position.y;transform.LookAt(t);transform.position=Vector3.MoveTowards(transform.position,t,speed*Time.deltaTime);}
  else if(Time.time>=nextAttack){ZaynCompleteGame.Instance.TakeDamage(damage);nextAttack=Time.time+1.2f;}}
 public void TakeDamage(int amount){currentHealth-=amount;if(currentHealth<=0)Destroy(gameObject);}
}
