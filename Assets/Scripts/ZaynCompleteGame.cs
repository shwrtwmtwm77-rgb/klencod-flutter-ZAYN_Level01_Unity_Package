using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class ZaynCompleteGame : MonoBehaviour
{
    public static ZaynCompleteGame Instance;
    public CharacterController controller;
    public Transform playerCamera, attackPoint, firePoint;
    public float moveSpeed=6f, gravity=-20f, jumpHeight=2f, rotationSpeed=12f;
    public float dashSpeed=18f, dashDuration=.25f, attackRange=2f;
    public int maxHealth=100,currentHealth,maxEnergy=100,currentEnergy=100;
    public int meleeDamage=25,energyDamage=35,ultimateDamage=100;
    public float attackCooldown=.5f,energyCooldown=1f;
    public LayerMask enemyLayer;
    public GameObject energyPrefab;
    public int enemiesRequired=5,enemiesDefeated,coins,experience,level=1;
    public bool missionCompleted,shieldActive;
    public GameObject enemyPrefab,bossPrefab;
    public Transform[] enemySpawnPoints;
    public Transform bossSpawnPoint;
    public Slider healthBar,energyBar;
    public Text healthText,energyText,coinsText,missionText;

    Vector2 moveInput; Vector3 velocity;
    bool attackPressed,energyPressed,jumpPressed,dashPressed,isDashing,isDead;
    float dashTimer,nextAttack,nextEnergy;

    void Awake(){Instance=this;}
    void Start(){
        if(!controller) controller=GetComponent<CharacterController>();
        currentHealth=maxHealth; currentEnergy=maxEnergy; SpawnEnemies(); UpdateUI();
    }
    void Update(){if(isDead)return; Move(); Actions(); RegenerateEnergy(); UpdateUI();}
    void Move(){
        Vector3 d=new Vector3(moveInput.x,0,moveInput.y);
#if UNITY_EDITOR
        d+=new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));
        if(Input.GetButtonDown("Jump"))jumpPressed=true;
        if(Input.GetMouseButtonDown(0))attackPressed=true;
        if(Input.GetMouseButtonDown(1))energyPressed=true;
#endif
        if(isDashing){controller.Move(transform.forward*dashSpeed*Time.deltaTime);dashTimer-=Time.deltaTime;if(dashTimer<=0)isDashing=false;}
        else if(d.magnitude>.1f){
            if(playerCamera){d=playerCamera.TransformDirection(d);d.y=0;}
            d.Normalize(); controller.Move(d*moveSpeed*Time.deltaTime);
            transform.rotation=Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(d),rotationSpeed*Time.deltaTime);
        }
        if(controller.isGrounded&&velocity.y<0)velocity.y=-2;
        if(jumpPressed&&controller.isGrounded){velocity.y=Mathf.Sqrt(jumpHeight*-2*gravity);jumpPressed=false;}
        velocity.y+=gravity*Time.deltaTime; controller.Move(velocity*Time.deltaTime);
    }
    void Actions(){if(attackPressed){Melee();attackPressed=false;}if(energyPressed){Energy();energyPressed=false;}if(dashPressed){Dash();dashPressed=false;}}
    public void SetMove(Vector2 v){moveInput=v;} public void AttackButton(){attackPressed=true;}
    public void EnergyButton(){energyPressed=true;} public void JumpButton(){jumpPressed=true;} public void DashButton(){dashPressed=true;}
    public void ShieldButton(){ActivateShield();} public void UltimateButton(){UltimateAttack();}
    void Melee(){
        if(Time.time<nextAttack||!attackPoint)return;nextAttack=Time.time+attackCooldown;
        foreach(var h in Physics.OverlapSphere(attackPoint.position,attackRange,enemyLayer)){
            var e=h.GetComponentInParent<ZaynEnemy>();if(e)e.TakeDamage(meleeDamage);
            var b=h.GetComponentInParent<ZaynBoss>();if(b)b.TakeDamage(meleeDamage);
        }
    }
    void Energy(){
        if(Time.time<nextEnergy||currentEnergy<20||!energyPrefab||!firePoint)return;
        currentEnergy-=20;nextEnergy=Time.time+energyCooldown;
        var p=Instantiate(energyPrefab,firePoint.position,firePoint.rotation);
        var s=p.GetComponent<ZaynProjectile>();if(!s)s=p.AddComponent<ZaynProjectile>();s.damage=energyDamage;
    }
    void Dash(){if(isDashing)return;isDashing=true;dashTimer=dashDuration;}
    public void ActivateShield(){shieldActive=true;Invoke(nameof(DisableShield),3f);} void DisableShield(){shieldActive=false;}
    public void UltimateAttack(){
        if(currentEnergy<50)return;currentEnergy-=50;
        foreach(var h in Physics.OverlapSphere(transform.position,8f,enemyLayer)){
            var e=h.GetComponentInParent<ZaynEnemy>();if(e)e.TakeDamage(ultimateDamage);
            var b=h.GetComponentInParent<ZaynBoss>();if(b)b.TakeDamage(ultimateDamage);
        }
    }
    public void TakeDamage(int d){if(isDead)return;if(shieldActive)d=Mathf.Max(1,d/4);currentHealth=Mathf.Max(0,currentHealth-d);if(currentHealth<=0)isDead=true;}
    void RegenerateEnergy(){currentEnergy=Mathf.Min(maxEnergy,currentEnergy+Mathf.CeilToInt(10f*Time.deltaTime));}
    public void EnemyDefeated(int c,int xp){enemiesDefeated++;coins+=c;experience+=xp;if(!missionCompleted&&enemiesDefeated>=enemiesRequired){missionCompleted=true;if(bossPrefab&&bossSpawnPoint)Instantiate(bossPrefab,bossSpawnPoint.position,bossSpawnPoint.rotation);}}
    void SpawnEnemies(){if(!enemyPrefab||enemySpawnPoints==null||enemySpawnPoints.Length==0)return;for(int i=0;i<enemiesRequired;i++){var p=enemySpawnPoints[i%enemySpawnPoints.Length];Instantiate(enemyPrefab,p.position,p.rotation);}}
    void UpdateUI(){
        if(healthBar){healthBar.maxValue=maxHealth;healthBar.value=currentHealth;}
        if(energyBar){energyBar.maxValue=maxEnergy;energyBar.value=currentEnergy;}
        if(healthText)healthText.text=currentHealth+"/"+maxHealth;
        if(energyText)energyText.text=currentEnergy+"/"+maxEnergy;
        if(coinsText)coinsText.text="Coins: "+coins;
        if(missionText)missionText.text=missionCompleted?"Defeat the Rift Guardian":"Defeat enemies "+enemiesDefeated+"/"+enemiesRequired;
    }
}
