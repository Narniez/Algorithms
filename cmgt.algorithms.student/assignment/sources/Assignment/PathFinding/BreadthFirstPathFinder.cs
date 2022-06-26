using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;

class BreadthFirstPathFinder : PathFinder
{
    List<Node> todoList = new List<Node>();
    List<Node> doneList = new List<Node>();
    List<Node> path = new List<Node>();
    Node currentNode = null;

    public BreadthFirstPathFinder(NodeGraph pGraph) : base(pGraph) { }

    protected override List<Node> generate(Node pFrom, Node pTo)
    {
        todoList.Clear();
        doneList.Clear();
        path.Clear();

      
        todoList.Add(pFrom);

        while (true)
        {
           
            if (todoList.Count() == 0)
            {
                return null;
            }

           
            currentNode = todoList[0];
            todoList.RemoveAt(0);
            doneList.Add(currentNode);
                
            if (currentNode == pTo)
            {
                path.Add(currentNode);
               
                while (currentNode != pFrom)
                {
                    path.Add(currentNode.parentNode);
                    currentNode = currentNode.parentNode;
                }
                
                path.Reverse();             
                return path;
            }
            else
            {
                foreach (Node connection in currentNode.connections)
                {
                    if (todoList.Contains(connection) || doneList.Contains(connection))
                    {
                        continue;
                    }
                    else
                    {                      
                        connection.parentNode = currentNode;
                        todoList.Add(connection);
                    }
                }
            }
        }
    }
}