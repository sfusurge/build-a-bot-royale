using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PartHealth : MonoBehaviour
{
    public float maxHealth = 20;
    private Color initialColor;
    public float health;
    public Vector2Int relPos { get; private set; } = Vector2Int.zero;
    private string type,direction;
    [SerializeField] private GameObject DestroyedPartPrefab = default;

    private bool dead;

    private Rigidbody rb;

    private AllRobotStats allRobotStats;
    
    private PartHandler handler;
    private bool subtractedStrength = false;
    // Start is called before the first frame update
    void Start()
    {
        handler = transform.parent.gameObject.GetComponent<PartHandler>();
        health = maxHealth;
        initialColor = GetComponent<Renderer>().material.color;
        rb = transform.parent.gameObject.GetComponent<Rigidbody>();
        allRobotStats = FindObjectOfType<AllRobotStats>();
        dead = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool SubtractHealth(float damage)
    {
        bool killed = false;
        health -= damage;
        if (health <= 0)
        {
            if (gameObject.CompareTag("Center"))
            {
                if(!dead && transform.parent.gameObject != null){
                    handler.EmitEmptyParts();
                    allRobotStats.AddToList(transform.parent.gameObject);
                    Destroy(transform.parent.gameObject);
                    dead = true;
                    killed = true;
                }
            }
            else
            {
                if(!subtractedStrength){
                    subtractDirectionStrength();
                    subtractedStrength = true;
                    Destroy(gameObject);
                    rb.mass--;
                }
                handler.partDestroyed(relPos.x, relPos.y);
            }
        }
        UpdateColor();
        return killed;
    }

    private void UpdateColor(){
        GetComponent<Renderer>().material.color = Color.Lerp(initialColor, Color.red, 1-(health/maxHealth));
    }
    public void setRelPos(int x, int z, string type, string direction)
    {
        relPos = new Vector2Int(x, z);
        this.type = type;
        this.direction = direction;
    }

    public void subtractDirectionStrength(){
        if(type == "block"){
            if(relPos.x < 0){
                transform.parent.gameObject.GetComponent<PartHandler>().changeDirectionStrength("west",-1);
            }
            if(relPos.x > 0){
                transform.parent.gameObject.GetComponent<PartHandler>().changeDirectionStrength("east",-1);
            }
            if(relPos.y > 0){
                transform.parent.gameObject.GetComponent<PartHandler>().changeDirectionStrength("north",-1);
            }
            if(relPos.y < 0){
                transform.parent.gameObject.GetComponent<PartHandler>().changeDirectionStrength("south",-1);
            }
        }else{
            transform.parent.gameObject.GetComponent<PartHandler>().changeDirectionStrength(direction,-3);
        }
    }

    public string GetPartType(){
        return type;
    }
    public string GetPartDirection(){
        return direction;
    }
    public Vector2Int GetRelPos(){
        return relPos;
    }
    
    public float GetHealth(){
        return health/maxHealth;
    }

    public float ReturnHealth(){
        return Math.Max(health,0);
    }

    private void OnDestroy()
    {
        // dont spawn stuff when quitting the app. It will be left behind in the editor
        if (GameStateManager.appIsQuitting == false)
        {
            Instantiate(DestroyedPartPrefab, transform.position, UnityEngine.Random.rotation);
        }
    }
}
