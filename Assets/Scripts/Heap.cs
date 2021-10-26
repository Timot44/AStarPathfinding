using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//On crée une class généric de type T afin que cette classe puisse servir dans d'autre cas que pour les nodes
public class Heap<T> where T : IHeapItem<T>
{
    private T[] _items;
    private int _currentItemCount;
    //Permet de savoir combien d'items il y a actuellement dans le heap
    public int Count
    {
        get { return _currentItemCount; }
    }

    public Heap(int maxHeapSize)
    {
        _items = new T[maxHeapSize];
    }
    //Permet d'ajouter des nouveaux "items" dans le heap (binary tree)
    // Le Heap est notre nouvel méthode pour améliorer notre pathfinding
    public void Add(T item)
    {
        item.HeapIndex = _currentItemCount;
        _items[_currentItemCount] = item;
        //Ensuite on va trier cet item
        SortUp(item);
        _currentItemCount++;
    }
    //Permet de retirer le premier item du binary tree (heap)
    public T RemoveFirst()
    {
        T firstItem = _items[0];
        _currentItemCount--;
        _items[0] = _items[_currentItemCount];
        _items[0].HeapIndex = 0;
        SortDown(_items[0]);
        return firstItem;
    }
    //Permet d'update un item (changer sa priorité) dans le cas où l'on trouverai un node qu'on voudrait update avec un fcost plus petit
    //Car on a trouver un autre path vers lui
    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    //Method qui permet de check si le heap contient un item spécifique
    public bool Contains(T item)
    {
        return Equals(_items[item.HeapIndex], item);
    }

    void SortDown(T item)
    {
        while (true)
        {
            //Permet de récupérer l'index du child de gauche de l'item
            int childIndexLeft = (item.HeapIndex * 2) + 1;
            //Permet de récupérer l'index du child de droite de l'item
            int childIndexRight = (item.HeapIndex * 2) + 2;
            int swapIndex = 0;
            //Permet de checker si l'item à un child à gauche
            if (childIndexLeft < _currentItemCount)
            {
                swapIndex = childIndexLeft;
                //Permet de checker si l'item à un child à droite
                if (childIndexRight < _currentItemCount)
                {
                    //Ici on compare qui est le child avec la plus haute priorité (fcost le plu bas)
                    if (_items[childIndexLeft].CompareTo(_items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }
                //On compare si le parent à une plus petite priorité que son child qui a la plus grosse priorité
                if (item.CompareTo(_items[swapIndex]) < 0)
                {
                    Swap(item, _items[swapIndex]);
                }
                //Si le parent a une plus haute priorité que ces 2 children on break
                else
                {
                    return;
                }
            }
            //Si le parent n'a pas de children on break
            else
            {
                return;
            }
        }
    }

    void SortUp(T item)
    {
        //Parent de l'item
        int parentIndex = (item.HeapIndex - 1) / 2;
        while (true)
        {
            T parentItem = _items[parentIndex];
            //Si l'item à une plus grand priorité (donc un fcost plus petit) que son parent alors on va le swap
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }

            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    void Swap(T itemA, T itemB)
    {
        //Method pour swap 2 item dans le binary tree ainsi que leurs heapIndex
        _items[itemA.HeapIndex] = itemB;
        _items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex { get; set; }
}