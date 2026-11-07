using UnityEngine;
public class ZaynEnemy : MonoBehaviour{
 public int maxHealth=100,currentHealth,damage=10,rewardCoins=10,rewardXP=20;
 public float speed=3f,detectionRange=15f,attackRange=2f; Transform player; float nextAttack;
 void Start(){currentHealth=maxHealth;if(ZaynCompleteGame.Instance)player=ZaynCompleteGame.Instance.transform;}
 void Update(){if(!player)return;float d=Vector3.Distance(transform.position,player.position);if(d>detectionRange)return;
  if(d>attackRange){Vector3 t=player.position;t.y=transform.position.y;transform.LookAt(t);transform.position=Vector3.MoveTowards(transform.position,t,speed*Time.deltaTime);}
  else if(Time.time>=nextAttack){ZaynCompleteGame.Instance.TakeDamage(damage);nextAttack=Time.time+1.5f;}}
 public void TakeDamage(int amount){currentHealth-=amount;if(currentHealth<=0){if(ZaynCompleteGame.Instance)ZaynCompleteGame.Instance.EnemyDefeated(rewardCoins,rewardXP);Destroy(gameObject);}}
}
