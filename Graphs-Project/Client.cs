using System;
using System.Collections.Generic;

class Client
{
    static void Main(string[] args)
    {
        //Black text on white background
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Clear();

        //Create graph
        Graph<string> graph = new Graph<string>();

        //Build graph
        BuildGraph(graph);

        //Visualise
        PrintAdjacencyMatrix(graph);

        //Traversals
        BreadthFirst(graph, "A");
        DepthFirst(graph, "A");
        Console.WriteLine();

        //Minimum spanning tree
        MST(graph, "A");

        //Opportunity to read output
        Console.Write("\n\tPress any key to exit ...");
        Console.ReadKey();
    } //Main

    private static void BuildGraph(Graph<string> graph)
    {
        //Add edges and vertices
        graph.AddEdge("A", "B", 300, true); graph.AddEdge("B", "C", 100, true); graph.AddEdge("C", "D", 100, true);
        graph.AddEdge("D", "E", 200, true); graph.AddEdge("E", "F", 150, true); graph.AddEdge("D", "F", 300, true);
        graph.AddEdge("F", "G", 80, true); graph.AddEdge("G", "H", 100, true); graph.AddEdge("B", "H", 400, true);
        graph.AddEdge("H", "I", 80, true); graph.AddEdge("I", "J", 100, true); graph.AddEdge("J", "K", 150, true);
        graph.AddEdge("K", "L", 80, true); graph.AddEdge("M", "L", 150, true); graph.AddEdge("J", "M", 100, true);
        graph.AddEdge("I", "N", 200, true); graph.AddEdge("L", "O", 100, true); graph.AddEdge("N", "O", 300, true);
        graph.AddEdge("A", "O", 100, true);
    } //BuildGraph

    private static void PrintAdjacencyMatrix(Graph<string> g)
    {
        Console.Clear();
        Console.WriteLine();

        //Vertices top row
        Console.Write("\tNodes");
        for (int i = 0; i < g.Size; i++)
            Console.Write(g[i].PadLeft(4));
        Console.WriteLine();
        Console.Write("\t======");
        for (int i = 0; i < g.Size; i++)
            Console.Write("====");
        Console.WriteLine();

        //Weights
        for (int i = 0; i < g.Size; i++)
        {
            Console.Write("\t" + g[i].PadLeft(4) + " |");
            for (int j = 0; j < g.Size; j++)
            {
                if (g.M[i, j] == -1 || g.M[i, j] == 0)
                    Console.Write("".PadLeft(4));
                else
                    Console.Write(g.M[i, j].ToString().PadLeft(4));
            } //for j
            Console.WriteLine();
        } //for i

        Console.WriteLine();
        Console.WriteLine();

    } //PrintAdjacencyMatrix

    private static void BreadthFirst(Graph<string> graph, string u)
    {
        Console.Write("\tBreadth first traversal starting from " + u + " : ");
        foreach (string s in graph.ListBreadthFirst(u))
            Console.Write(s + " ");
        Console.WriteLine();

    } //BreadthFirst

    private static void DepthFirst(Graph<string> graph, string u)
    {
        Console.Write("\tDepth first traversal starting from " + u + "   : ");
        foreach (string s in graph.ListDepthFirst(u))
            Console.Write(s + " ");
        Console.WriteLine();

    } //DepthFirst

    private static void MST(Graph<string> graph, string u)
    {
        List<string> lstMST = new List<string>();
        List<Tuple<string, string>> lstEdges = new List<Tuple<string, string>>();
        int totalCost = 0;
        graph.GetMST(u, ref lstMST, ref lstEdges, ref totalCost);

        Console.WriteLine("\tMinimum spanning tree starting with " + u);
        Console.WriteLine("\tNodes in order of addition to tree      : " + string.Join(" ", lstMST) );
        Console.Write("\tEdges                                   : ");
        foreach (Tuple<string, string> t in lstEdges)
            Console.Write(t.Item1 + "-" + t.Item2 + "; ");
        Console.WriteLine();

        Console.WriteLine("\tTotal cost                              : " + totalCost);
    } //MST

} //class Client

