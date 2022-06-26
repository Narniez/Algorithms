using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;
using GXPEngine;

internal class SufficientDungeonNodeGraph : SampleDungeonNodeGraph
{

    public SufficientDungeonNodeGraph(Dungeon pDungeon) : base(pDungeon)
    {

    }


    protected override void generate()
    {
        // Adding nodes to all the rooms 
        foreach (Room room in _dungeon.rooms)
        {
            Node node = new Node(getRoomCenter(room),room,null);
            room.node = node;
            nodes.Add(node);
        }

        //Adding nodes to all the doors and connecting them to the adjacent room nodes
        foreach (Door door in _dungeon.doors)
        {
            Node node = new Node(getDoorCenter(door), null, door);
            door.node = node;
            nodes.Add(node);
            AddConnection(node, door.roomA.node);
            AddConnection(node, door.roomB.node);
        }
    }
}

