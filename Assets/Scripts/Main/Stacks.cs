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
        return tempArray;       //I think that we do not need to check it empty or full because
    }

    public string[,] peek() {
        return stack[front];
    }
    public void DeleteRear() {
        if (front == max - 1) {          //Only do this function when the stack is full
            for (int i = 0; i < stack.Length - 1; i++) {
                stack[i] = stack[i + 1];
            }
            front--;
        }
    }
}
