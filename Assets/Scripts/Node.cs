using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
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

   public Node(bool _walkable, Vector3 _worldPosition, int _gridX, int _gridY)
   {
      walkable = _walkable;
      worldPosition = _worldPosition;
      gridX = _gridX;
      gridY = _gridY;
   }

   
}
