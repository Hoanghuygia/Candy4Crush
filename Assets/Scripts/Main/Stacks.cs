using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stacks {
    public string[][,] stack;
    public int front;
    private int max;
    public Stacks(int size) {
        stack = new string[size][,];
        front = -1;
        max = size;
    }
    public bool Empty() {
        return front == -1;
    }
    public bool Full() {
        return front == max - 1;
    }

    public void push(string[,] item) {
        front++;
        stack[front] = item;
    }

    public string[,] pop() {
        string[,] tempArray = stack[front];
        front--;
        return tempArray;       
    }

    public string[,] peek() {
        return stack[front];
    }
    public void DeleteRear() {
        if (front == max - 1) {          
            for (int i = 0; i < stack.Length - 1; i++) {
                stack[i] = stack[i + 1];
            }
            front--;
        }
    }
}
