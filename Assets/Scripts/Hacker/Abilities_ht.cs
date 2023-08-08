using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities_ht : MonoBehaviour
{
    //Input Used to Execute Ability - Left Shift
    public KeyCode AbilityInput => input;
    [SerializeField] private KeyCode input;

    //Abilities
    public Roll RollAbility;
    public Teleport TeleportAbility;
    public Invisibility InvisibilityAbility;

    private Ability currentAbility;
    private PlayerEntity playerEntity;

    private void Awake()
    {
        //Cache
        playerEntity = GetComponent<PlayerEntity>();

        //Initialize Abilities
        RollAbility = new Roll(this, playerEntity);
        TeleportAbility = new Teleport(this, playerEntity);
        InvisibilityAbility = new Invisibility(this, playerEntity);

        //Set Default Ability
        ChangeAbility(RollAbility);
    }

    private void Update()
    {
        //Testing Abilities
        if (Input.GetKeyDown(KeyCode.O))
        {
            ChangeAbility(RollAbility);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            ChangeAbility(TeleportAbility);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            ChangeAbility(InvisibilityAbility);
        }
    }

    private void FixedUpdate()
    {
        currentAbility.FixedUpdate();
    }

    public void ExecuteAbility()
    {
        currentAbility.Execute();
    }

    public void ChangeAbility(Ability newAbility)
    {
        currentAbility?.Exit();
        currentAbility = newAbility;
    }
}

public abstract class Ability
{
    protected readonly PlayerEntity PlayerEntity;

    public Ability(Abilities_ht abilities_ht, PlayerEntity playerEntity)
    {
        PlayerEntity = playerEntity;
    }

    public abstract void Execute();
    public virtual void FixedUpdate()
    {

    }
    public virtual void Exit()
    {

    }
}


public class Roll : Ability
{
    public Roll(Abilities_ht abilities_ht, PlayerEntity playerEntity) : base(abilities_ht, playerEntity)
    {

    }

    public override void Execute()
    {
        PlayerEntity.FiniteStateMachine.ChangeState(PlayerEntity.PlayerSlideState);
    }
}

public class Teleport : Ability
{
    public Teleport(Abilities_ht abilities_ht, PlayerEntity playerEntity) : base(abilities_ht, playerEntity)
    {

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        PlayerEntity.AbilityTimer += Time.deltaTime;
    }

    public override void Execute()
    {
        if (PlayerEntity.AbilityTimer < PlayerEntity.AbilityCooldown) return;

        //check for nearest collision
        float dist = float.MaxValue;
        RaycastHit2D hit = Physics2D.Raycast(PlayerEntity.TeleportRaycastPositions[0].position,
                new Vector2(Mathf.Sign(PlayerEntity.transform.localScale.x), 0), PlayerEntity.TeleportDistance);
        if (hit.collider)
        {
            dist = hit.distance;
            if (hit.collider.gameObject.CompareTag("Enemy")) {
                dist = hit.collider.transform.position.x + Mathf.Sign(hit.collider.transform.localScale.x) * -1;
            }
        }
        for (int i=1; i< PlayerEntity.TeleportRaycastPositions.Length; i++)
        {
            hit = Physics2D.Raycast(PlayerEntity.TeleportRaycastPositions[i].position, 
                new Vector2(Mathf.Sign(PlayerEntity.transform.localScale.x), 0), PlayerEntity.TeleportDistance);
            float holdDist = float.MaxValue;
            if (hit.collider)
            {
                holdDist = hit.distance;
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    holdDist = hit.collider.transform.position.x + Mathf.Sign(hit.collider.transform.localScale.x) * -1;
                }
            }
            dist = Mathf.Min(dist, holdDist);
        }
        PlayerEntity.transform.position += Vector3.right * Mathf.Sign(PlayerEntity.transform.localScale.x) * 
            Mathf.Clamp(dist, 0, PlayerEntity.TeleportDistance);
        PlayerEntity.gameObject.layer = LayerMask.NameToLayer("Invincible");
        PlayerEntity.StartCoroutine(ReturnToPlayerLayer());
        PlayerEntity.AbilityTimer = 0;
    }
    private IEnumerator ReturnToPlayerLayer()
    {
        yield return new WaitForSecondsRealtime(0.15f);
        PlayerEntity.gameObject.layer = LayerMask.NameToLayer("Player");
    }
}

public class Invisibility : Ability
{
    public Invisibility(Abilities_ht abilities_ht, PlayerEntity playerEntity) : base(abilities_ht, playerEntity)
    {

    }

    public override void Execute()
    {
        throw new System.NotImplementedException();
    }
}