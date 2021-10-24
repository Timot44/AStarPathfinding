using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private Node[,] _grid;

    public Vector2 gridWorldSize;

    public float nodeRadius;

    public LayerMask unWalkableMask;

    private float _nodeDiameter;

    private int _gridSizeX, _gridSizeY;

    public List<Node> path;
    // Start is called before the first frame update
    void Start()
    {
        _nodeDiameter = nodeRadius * 2;
        //Permet de savoir combien de node on va avoir dans notre grid
        _gridSizeX = Mathf.RoundToInt(gridWorldSize.x / _nodeDiameter);
        _gridSizeY = Mathf.RoundToInt(gridWorldSize.y / _nodeDiameter);
        CreateGrid();
    }

   
    private void OnDrawGizmos()
    {
        //Dessine un cube avec la taille de la gridWorldSize en paramètre
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        if (_grid != null)
        {
            //Pour chaque node dans le tableau 2D de _grid si le node est considérer comme "walkable" la couleur est blanche sinon elle est rouge
            foreach (Node n in _grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                //Si notre path n'est pas null et qu'il contient les node dans notre list grid alors la couleur du chemin l plus court est cyan
                if (path !=null)
                {
                    if (path.Contains(n))
                    {
                        Gizmos.color = Color.cyan;
                    }
                }
                
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (_nodeDiameter-.1f));
            }
        }
    }



    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt(_gridSizeX * percentX);
        int y = Mathf.RoundToInt(_gridSizeY  * percentY);
        x = Mathf.Clamp(x, 0, _gridSizeX - 1);
        y = Mathf.Clamp(y, 0, _gridSizeY - 1);
        return _grid[x, y];
    }

    public List<Node> GetNeighbours(Node node)
    {
        //On crée une list de "voisins" node vide
        List<Node> neighbours = new List<Node>();

        //On loop dans un block de 3/3 (x= -1,0,1 / y = -1,0,1)
        for (int x = -1; x <= 1; x++)
        {
            
            for (int y = -1; y <= 1; y++)
            {
                // On skip l'itération 0 car c'est le node de base
                if (x == 0 && y == 0)
                    continue;
                // On cree 2 int pour la position des node "voisins"
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;
                // On check si notre node est dans notre grid et 
                if (checkX >= 0 && checkX < _gridSizeX && checkY >= 0 && checkY < _gridSizeY)
                {
                    //Si tel est le cas on ajoute ce node dans notre list de node "voisins"
                    neighbours.Add(_grid[checkX, checkY]);
                }
                
            }
        }
        // On return notre list de node "voisins"
        return neighbours;
    }
    void CreateGrid()
    {
        //On populate notre tableau 2D avec notre gridSize
        _grid = new Node[_gridSizeX, _gridSizeY];
        //On part du point en bas à gauche
        Vector3 worldBottomLeft =
            transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        for (int x = 0; x < _gridSizeX; x++)
        {
            for (int y = 0; y < _gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * _nodeDiameter + nodeRadius) +
                                     Vector3.forward * (y * _nodeDiameter + nodeRadius);
                //Si dans un certain nodeRadius on touche un objet qui n'est pas dans le mask unwalkable alors le bool est true sinon le node est dit "unwalkable"
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unWalkableMask));
                //On ajoute dans notre tableau de node les points avec le constructor
                _grid[x, y] = new Node(walkable, worldPoint, x, y); 
            }
        }
    }
}