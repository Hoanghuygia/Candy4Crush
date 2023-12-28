using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTile : MonoBehaviour
{
    public int hitPoints;
    private SpriteRenderer sprite;
    private GoalManager goalManager;
    public void Start() {
        sprite = GetComponent<SpriteRenderer>();
        goalManager = FindObjectOfType<GoalManager>();
        
    }
    
    public void Update() { 
        if(hitPoints <= 0) {
            if(goalManager != null) {
                goalManager.CompareGoal(this.gameObject.tag.ToString());
                goalManager.UpdateGoal();
            }
            Destroy(this.gameObject);
        }
    }
    public void TakeDamage(int damage) {
        //Debug.Log("Hit points: " + hitPoints);
        hitPoints -= damage;
        //Debug.Log("HP: " + hitPoints);
        MakeLighter();
    }
    public void MakeLighter() {
        //take the current color
        Color color = sprite.color;
        //take the current color alphe value
        float newAlpha = color.a * .5f;
        sprite.color = new Color(color.r, color.g, color.b, newAlpha);
    }
}
