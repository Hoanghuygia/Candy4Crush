using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTile : MonoBehaviour
{
    public int hitPoints;
    private SpriteRenderer sprite;
    public void Start() {
        sprite = GetComponent<SpriteRenderer>();
    }
    public void TakeDamage(int damage) {
        hitPoints -= damage;
        MakeLighter();
    }
    public void Update() { 
        if(hitPoints <= 0) {
            Destroy(this.gameObject);
        }
    }
    public void MakeLighter() {
        //take the current color
        Color color = sprite.color;
        //take the current color alphe value
        float newAlpha = color.a * .5f;
        sprite.color = new Color(color.r, color.g, color.b, newAlpha);
    }
}
