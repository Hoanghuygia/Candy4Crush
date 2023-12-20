using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTile : MonoBehaviour
{
    public int hitPoints;
    public void TakeDamage(int damage) {
        hitPoints -= damage;
    }
    public void Update() { 
        if(hitPoints <= 0) {
            Destroy(this.gameObject);
        }
    }
}
