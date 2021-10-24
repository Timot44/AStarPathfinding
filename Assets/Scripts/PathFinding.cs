using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    private Grid _grid;
    public Transform seeker, target;
    private void Awake()
    {
        _grid = GetComponent<Grid>();
    }

    private void Update()
    {
        FindPath(seeker.position, target.position);
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        //Point de départ de l'algo (Point A)
        Node startNode = _grid.NodeFromWorldPoint(startPos);
        //Point d'arriver ou celui que l'on veut atteindre de l'algo (Point B)
        Node targetNode = _grid.NodeFromWorldPoint(targetPos);
        
        //On crée notre list de départ de node qui à besoin d'etre évaluer
        List<Node> openSet = new List<Node>();
        // On crée un hashSet de node qui représente ceux déjà évaluer
        HashSet<Node> closedSet = new HashSet<Node>();
        // On ajoute a notre list openSet le startNode (Point de départ)
        openSet.Add(startNode);

        //Tant que notre list de node n'est pas vide on continue la boucle
        while (openSet.Count > 0)
        {
            //Ici on crée un node de base qui est égale à notre startNode de notre list de départ
            Node currentNode = openSet[0];
            //On commence à 1 car on à déja assigner le premier node audessus
            for (int i = 1; i < openSet.Count; i++)
            {
                //Si le fCost de notre node "i" dans notre list de départ est inf à notre currentNode alors currentNode = le node "i"
                //Ensuite si les 2 node (i et current node) sont égaux et si node "i" hCost est inf à currentNode.hcost alors currentNode = node "i" 
                if (openSet[i].fCost < currentNode.fCost ||
                    openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }
            //On remove donc le currentNode de notre list de départ pour l'ajouter à la HashSet "évaluer"
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            
            //Si la currentNode est égale à la targetNode alors on à trouver notre chemin le plus court
            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }
            //Sinon on doit chercher parmis les nodes "voisins" de currentNode
            foreach (Node neighbour in _grid.GetNeighbours(currentNode))
            {
                //Si le node n'est pas walkable ou si il est déjà dans notre list de node "évaluer" on passe au node "voisin" suivant
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                    continue;
                // On calcule si le nouveau mouvement du node "voisin" avec le gcost du currentNode et la dist (h cost) entre celle-ci et le node "voisin"
                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                // Si ce nouveau mouvement est plus petit que le gcost du node voisin ou que ce voisin node n'est pas dans la list des node à évaluer
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    //Alors le node voisin gCost est égale à ce nouveau mouvement
                    neighbour.gCost = newMovementCostToNeighbour;
                    //Et son hCost est recalculer aussi
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    //On assigne le parent du node "voisin" au currentNode
                    neighbour.parent = currentNode;
                    //Si ce node "voisin" n'est pas dans la list des nodes à évaluer alors on l'ajoute à cette list
                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }

    void RetracePath(Node startNode, Node endNode)
    {
        // On crée une list path de node
        List<Node> path = new List<Node>();
        //On crée une node current node qu'on assigne a endNode
        Node currentNode = endNode;
        
        //Tant que currentNode n'est pas égale a startNode (on retrace notre chemin à l'envers)
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        _grid.path = path;
    }
    int GetDistance(Node nodeA, Node nodeB)
    {
        // On calcule la dist entre le node a et node b pour X et Y
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        //Calcul pour savoir selon si la distX ou distY est plus grande le résultat donc la distance du path entre A et B
        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);
        return 14 * distX + 10 * (distY - distX);
    }
}