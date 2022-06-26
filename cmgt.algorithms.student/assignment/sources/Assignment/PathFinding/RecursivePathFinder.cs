using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
class RecursivePathFinder : PathFinder
{
   
    public RecursivePathFinder(NodeGraph pGraph) : base(pGraph) { }

    protected override List<Node> generate(Node pFrom, Node pTo)
    {
        return findPath(new List<Node>(), new List<Node>(), pFrom, pTo);
    }

    protected List<Node> findPath(List<Node> visitedNotes, List<Node> path, Node pFrom, Node pEnd)
    {
        //The fromNode will always be the first One visited 
        visitedNotes.Add(pFrom);

        //If the end node is found then return the path 
        if (pFrom == pEnd)
        {
            //If this is not yet the sortest path add the current visited path to the shortest path
            //If the current visited path is shorter than the current path replace it
            if (path.Count == 0 || visitedNotes.Count() < path.Count())
            {
                path = new List<Node>(visitedNotes);
            }
            
            return path;
        }

        //Go through all the connections of the node to find a path
        foreach(Node connections in pFrom.connections)
        {
            if (!visitedNotes.Contains(connections))
            {
                path = findPath(new List<Node>(visitedNotes), path, connections, pEnd); 
            }         
        }
        return path;
    }
}


