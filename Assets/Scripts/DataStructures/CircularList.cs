using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularList<T>
{
    public int Count;
    private Node<T>[] nodes;

    public CircularList(T[] items)
    {
        this.Count = items.Length;
        this.nodes = new Node<T>[items.Length];
        //initialize underlying array
        for(int i = 0; i < items.Length; i++)
        {
            this.nodes[i] = new Node<T>(items[i]);
        }
        //initialize Node loop
        this.nodes[0].prev = nodes[items.Length - 1];
        this.nodes[0].next = nodes[1];
        for(int i = 1; i < items.Length - 1; i++)
        {
            this.nodes[i].prev = nodes[i - 1];
            this.nodes[i].next = nodes[i + 1];
        }
        this.nodes[items.Length - 1].prev = nodes[0];
        this.nodes[items.Length - 1].next = nodes[items.Length - 2];
    }

    public void Remove(int index)
    {
        nodes[index].prev.next = nodes[index].next;
        nodes[index].next.prev = nodes[index].prev;
        Count --;
    }

    //Return the value of the node at a given index
    public T ValueAt(int index)
    {
        return nodes[index].value;
    }

    //Return the value of the node just before a given index
    public T ValueBefore(int index)
    {
        return nodes[index].prev.value;
    }

    //Return the value of the node just after a given index
    public T ValueAfter(int index)
    {
        return nodes[index].next.value;
    }

    private class Node<S>
    {
        public S value;
        public Node<S> prev;
        public Node<S> next;

        public Node(S value)
        {
            this.value = value;
        }
    }
}
