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

    private Rigidbody rb;
    SocketConnectionHandler socketIO;
    private bool subtractedStrength = false;
    // Start is called before the first frame update
    void Start()
    {
        socketIO = FindObjectOfType<SocketConnectionHandler>();
        health = maxHealth;
        initialColor = GetComponent<Renderer>().material.color;
        rb = transform.parent.gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SubtractHealth(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            if (gameObject.CompareTag("Center"))
            {
                Destroy(transform.parent.gameObject);
                socketIO.EmitEmptyParts(transform.parent.gameObject.name);
            }
            else
            {
                transform.parent.gameObject.GetComponent<PartHandler>().partDestroyed(relPos.x, relPos.y);
                if(!subtractedStrength){
                    subtractDirectionStrength();
                    subtractedStrength = true;
                    Destroy(gameObject);
                    GameStateManager.Instance.killRobot(gameObject);
                    socketIO.EmitCurrentParts(transform.parent.gameObject);
                    rb.mass--;
                }
            }
        }
        UpdateColor();
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
        if(type != "spike"){
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

    private void OnDestroy()
    {
        // dont spawn stuff when quitting the app. It will be left behind in the editor
        if (GameStateManager.appIsQuitting == false)
        {
            Instantiate(DestroyedPartPrefab, transform.position, UnityEngine.Random.rotation);
        }
    }
}
