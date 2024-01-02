using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stacks {
    private GameObject[][,] stack;
    private int front;
    private int max;
    public Stacks(int size) {
        stack = new GameObject[size][,];
        front = -1;
        max = size;
    }
    public bool Empty() {
        return front == -1;
    }
    public bool Full() {
        return front == max - 1;
    }

    public void push(GameObject[,] item) {
            stack[++front] = item; 
    }

    public GameObject[,] pop() {
        return stack[front--];       //I think that we do not need to check it empty or full because 
        
    }

    public GameObject[,] peek() {
            return stack[front];
    }
}
