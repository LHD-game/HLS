using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WindowStack : MonoBehaviour
{

    const int MAX_SIZE = 5;
    private string[] stack = new string[MAX_SIZE];
    private int top = -1;


    private bool isEmpty()
    {
        return top == -1 ? true : false;
    }
    private bool isFull()
    {
        return (top + 1 == MAX_SIZE) ? true : false;
    }

    public void push(string data)
    {
        Debug.Log($"push = {data}");
        if (!isFull())
            stack[++top] = data;
    }

    public string pop()
    {
        if (!isEmpty())
        {
            Debug.Log($"pop = {stack[top]}");
            return stack[top--];
        }
        return "Quit";
    }
    public void reset()
    {
        top = -1;
    }

}
