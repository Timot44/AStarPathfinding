using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
   public bool walkable;
   public Vector3 worldPosition;
   public int gridX;
   public int gridY;
   public Node parent;
   public int gCost;
   public int hCost;
   public int fCost
   {
      get
      {
         // Ce qui détermine le chemin le plus court du point A au point B
         return gCost + hCost;
      }
     
   }

   private int _heapIndex;

   public Node(bool _walkable, Vector3 _worldPosition, int _gridX, int _gridY)
   {
      walkable = _walkable;
      worldPosition = _worldPosition;
      gridX = _gridX;
      gridY = _gridY;
   }


   //Permet de comparer le fCost de 2 node
   public int CompareTo(Node nodeToCompare)
   {
      int compare = fCost.CompareTo(nodeToCompare.fCost);
      
      //Si les 2 fCost sont égaux alors on utilise le hCost
      if (compare == 0)
      {
         compare = hCost.CompareTo(nodeToCompare.hCost);
      }

      return -compare;
   }

   public int HeapIndex
   {
      get
      {
         return _heapIndex;
      }
      set
      {
         _heapIndex = value;
      }
   }
}
