using System;
using System.Collections.Generic;
using System.Linq;

//See McMillan p 286
public class Graph<T> where T:IComparable
{
    #region Embedded class for vertices

    private class Node
    {
        public T Value { get; private set; }
        public int keyValue; //For MST
        public Node(T u)
        {
            this.Value = u;
        }
    } //Embedded class

    #endregion Embedded class

    //Private data members and public properties
    private Node[] nodes;
    public int[,] M { get; private set; }
    public int Size { get { return nodes.Length; } }

    //Constructor
    public Graph()
    {
        nodes = new Node[0];
        M = new int[0, 0];
    } //Constructor

    //Indexer (Added from example)
    public T this[int i] { get { return nodes[i].Value; } }

    public void AddNode(T u)
    {
        //1.1 Resize nodes array
        int size = nodes.Length + 1;
        System.Array.Resize(ref nodes, size);

        //1.2 Assign new node to the last spot in the array
        nodes[size - 1] = new Node(u);

        //2. Resize adjacency matrix
        ResizeAdjMatrix(size);
    } //AddNode

    //Private helper for AddNode
    private void ResizeAdjMatrix(int newSize)
    {
        //2.1 Create a temporary array of the new size
        int[,] temp = new int[M.GetLength(0), M.GetLength(1)];

        //2.2 Copy the elements from M to temp
        for (int i = 0; i < M.GetLength(0); i++)
            for (int j = 0; j < M.GetLength(1); j++)
                temp[i, j] = M[i, j];

        //2.3 Create a new adjacency matrix, M
        M = new int[newSize, newSize];

        //2.4 Copy the elements from temp to M
        for (int i = 0; i < temp.GetLength(0); i++)
            for (int j = 0; j < temp.GetLength(1); j++)
                M[i, j] = temp[i, j];
    } //ResizeAdjMatrix

    public void AddEdge(T u, T v, int weight, bool isReverse)
    {
        //Add vertice if they were not added already
        if (!nodes.Select(U => U.Value).Contains(u))
            AddNode(u);
        if (!nodes.Select(V => V.Value).Contains(v))
            AddNode(v);

        //Get indexes of u and v in the matrix
        int u_ = Array.IndexOf(nodes.Select(U => U.Value).ToArray(), u);
        int v_ = Array.IndexOf(nodes.Select(V => V.Value).ToArray(), v);

        //Enter weight into matrix
        if (u_ >= 0 && v_ >= 0)
            M[u_, v_] = weight;

        //Run this method again for the reversed edge if specified
        if (isReverse)
            AddEdge(v, u, weight, false);
    } //AddEdge

    public void RemoveEdge(T u, T v, bool isReverse)
    {
        //Get indexes of u and v in the matrix
        int u_ = Array.IndexOf(nodes.Select(U => U.Value).ToArray(), u);
        int v_ = Array.IndexOf(nodes.Select(V => V.Value).ToArray(), v);

        //Enter 0 weight into matrix
        if (u_ >= 0 && v_ >= 0)
            M[u_, v_] = 0;

        if (isReverse)
            RemoveEdge(v, u, false);
    } //RemoveEdge

    #region Breadth first traversal

    public List<T> ListBreadthFirst(T u)
    {
        List<T> lstVisited = new List<T>();
        lstVisited.Add(u);
        Queue<T> Q = new Queue<T>();
        Q.Enqueue(u);

        while (Q.Count > 0)
        {
            u = Q.Dequeue();
            //Get the index of the current node
            int u_ = Array.IndexOf(nodes.Select(U => U.Value).ToArray(), u);

            //Step through the cells of the adjacency matric in one dimension
            for (int v_ = 0; v_ < Size; v_++)
            {
                //If the weight in a cell is > 0 (there is an edge) and the node at that index has not yet been visited, do so now
                if (M[u_, v_] > 0
                    && !lstVisited.Contains(nodes[v_].Value))
                {
                    lstVisited.Add(nodes[v_].Value);
                    Q.Enqueue(nodes[v_].Value);
                }
            } //for all successors with a weight > 0
        } //while Q.Count > 0
        return lstVisited;
    } //ListBreadthFirst

    #endregion Breadth first

    #region Depth first 

    public List<T> ListDepthFirst(T u)
    {
        List<Node> lstNodes = new List<Node>();
        ListDepthFirst(lstNodes, GetNode(u));
        return lstNodes.Select(nde => nde.Value).ToList();
    } //ListDepthFirst

    private void ListDepthFirst(List<Node> lstNodes, Node V)
    {
        if (!lstNodes.Contains(V))
        {
            lstNodes.Add(V);
            foreach (Node successor in Neighbours(V))
                ListDepthFirst(lstNodes, successor);
        }
    } //ListDepthFirst

    #endregion Depth first

    #region Helper methods

    //Returns a Node object from a value
    private Node GetNode(T value)
    {
        foreach (Node node in nodes)
            if (node.Value.Equals(value))
                return node;
        return null;
    } //GetNode

    //Returns the index of a node in the nodes array based on its value
    private int Idx(T u)
    {
        return Array.IndexOf(nodes.Select(U => U.Value).ToArray(), u);
    } //GetIndex

    //Returns the index of a node in the nodes array based on the Node object
    private int Idx(Node U)
    {
        return Array.IndexOf(nodes.ToArray(), U);
    } //GetIndex

    //Returns a List<Node> of neighbours of a Node object
    private List<Node> Neighbours(Node U)
    {
        List<Node> lstNeighbours = new List<Node>();
        int u_ = Idx(U);
        for (int v_ = 0; v_ < Size; v_++)
            if (M[u_, v_] > 0)
                lstNeighbours.Add(nodes[v_]);
        return lstNeighbours;
    } //Neighbours

    //Returns a List<T> of values of nodes that are neighbouring to a node with value u
    private List<T> Neighbours(T u)
    {
        List<T> lstNeighbours = new List<T>();
        int u_ = Idx(u);
        if (u_ == -1) return null;
        for (int v_ = 0; v_ < Size; v_++)
            if (M[u_, v_] > 0)
                lstNeighbours.Add(nodes[v_].Value);
        return lstNeighbours;
    } //Neighbours

    private int GetCost(List<T> lstNodes)
    {
        int cost = 0;

        for (int i = 1; i < lstNodes.Count; i++)
        {
            int u_ = Idx(lstNodes[i - 1]),
                v_ = Idx(lstNodes[i]);
            cost += M[u_, v_];
        }

        return cost;
    } //GetCost

    #endregion Helper methods

  

    #region Minimum spanning tree

    //Adapted from example
    public void GetMST(T u, ref List<T> lstMST, ref List<Tuple<T, T>> lstEdges, ref int totalCost) 
    {
        //The steps below refer to Prim's algoritm (https://www.geeksforgeeks.org/prims-minimum-spanning-tree-mst-greedy-algo-5/)

        //Step 1. Initialise
        lstMST = new List<T>();
        lstEdges = new List<Tuple<T, T>>();
        totalCost = 0;

        //Step 2. Assign initial key values
        foreach (Node node in nodes)
            node.keyValue = int.MaxValue;
        nodes[Idx(u)].keyValue = 0;

        //Step 3
        while (lstMST.Count < nodes.Length)
        {
            //3a. Pick vertex u with minimum key value
            int minKey = int.MaxValue;
            int u_ = -1;
            for (int i = 0; i < nodes.Length; i++)
            {
                if (!lstMST.Contains(nodes[i].Value) && nodes[i].keyValue < minKey)
                {
                    minKey = nodes[i].keyValue;
                    u_ = i;
                }
            } //for

            //3b. Include u to lstMST
            lstMST.Add(nodes[u_].Value);

            //Include edge (Added from example)
            //- Find nearest node in the existing graph to the newly added node
            int minCost = int.MaxValue;
            u = nodes[u_].Value;
            T v = default;
            for (int v_ = 0; v_ < nodes.Length; v_++) //For all nodes
            {
                if (lstMST.Contains(nodes[v_].Value) //Already in MST
                    && M[u_, v_] > 0                 //Edge exists beween u and v
                    && M[u_, v_] < minCost           //Edge is nearest to graph
                    ) 
                {
                    minCost = M[u_, v_];
                    v = nodes[v_].Value; //Hold nearest node to existing graph
                }
            }
            if (v != null) //Add edge from nearest node if found
            {
                lstEdges.Add(new Tuple<T, T>(v, u));
                totalCost += M[u_, Idx(v)];
            }

            //3c. Update key values of adjacent vertices to u
            for (int v_ = 0; v_ < nodes.Length; v_++) //For all nodes
                if (!lstMST.Contains(nodes[v_].Value))     //If the node is not yet in the MST
                    if (M[u_, v_] > 0)        //If an edge exists between u and v
                        if (M[u_, v_] < nodes[v_].keyValue) //If the edge weight is less than the current key value of v
                            nodes[v_].keyValue = M[u_, v_]; //Assign new key value to v
        } //while

        //return 
        //return lstMST;
    } //MST

    #endregion Minimum spanning tree

} //class Graph

