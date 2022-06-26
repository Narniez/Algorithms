using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GXPEngine;

class SufficientDungeon : Dungeon
{
    private List<Room> splitRooms;
    private List<Point> crossings = new List<Point>();
    Random widthRandom = new Random();
    Random heightRandom = new Random();
    
    public SufficientDungeon(Size pSize) : base(pSize)
    {

    }

    protected override void generate(int pMinimumRoomSize)
    {
        //Make a list of rooms that can be split into two
        splitRooms = new List<Room>();
        splitRooms.Add(new Room(new Rectangle(0, 0, size.Width, size.Height)));

        //Splitting the rooms and removing them from the splitRoom list  and adding them to the room list if they cannot be split any further 
        while (splitRooms.Count > 0)
        {
            Split(splitRooms[0], pMinimumRoomSize);
        }

        //Adds doors connecting neighbouring rooms
        for (int i = 0; i < rooms.Count - 1; i++)
        {
            for (int j = i + 1; j < rooms.Count; j++)
            {
                AddDoor(rooms[i], rooms[j]);
                
                graphics.Clear(Color.White);
                drawRooms(rooms, Pens.Black);
                drawRoom(rooms[i], Pens.Green);
                drawRoom(rooms[j], Pens.Red);
                drawDoors(doors,Pens.Blue);
                //Console.ReadKey();
            }
        }
    }


    // Splits a room into two by creating two smaller rooms and removing  the first room for which the method is called
    public void Split(Room room, int pMinimumRoomSize)
    {
        if (room.area.Width >= pMinimumRoomSize * 2 && room.area.Width >= room.area.Height)
        {
            // Vertical Split
            int widthRandomLength = widthRandom.Next(pMinimumRoomSize + 1, room.area.Width - pMinimumRoomSize + 1);
            splitRooms.Add(new Room(new Rectangle(room.area.X, room.area.Y, widthRandomLength + 1, room.area.Height)));
            splitRooms.Add(new Room(new Rectangle(room.area.X + widthRandomLength, room.area.Y, room.area.Width - widthRandomLength, room.area.Height)));
            splitRooms.Remove(room);
        }
        else if (room.area.Height >= pMinimumRoomSize * 2 && room.area.Width < room.area.Height)
        {
            // Horizontal Split
            int heightRandomLength = heightRandom.Next(pMinimumRoomSize + 1, room.area.Height - pMinimumRoomSize + 1);
            splitRooms.Add(new Room(new Rectangle(room.area.X, room.area.Y, room.area.Width, heightRandomLength + 1)));
            splitRooms.Add(new Room(new Rectangle(room.area.X, room.area.Y + heightRandomLength, room.area.Width, room.area.Height - heightRandomLength)));
            splitRooms.Remove(room);
        }
        else
        {
            crossings.Add(new Point(room.area.X, room.area.Y));
            crossings.Add(new Point(room.area.X + room.area.Width, room.area.Y + room.area.Height - 1));
            crossings.Add(new Point(room.area.X + room.area.Width - 1, room.area.Y));
            // If it cannot be split anymore, add it to the rooms list and remove it from splitRoom list
            rooms.Add(room);
            splitRooms.Remove(room);
        }
    }

    //Adds a door on the coordinates where two rooms intersect 
    public void AddDoor(Room roomA, Room roomB)
    {    
        Point rightBottomA = new Point(roomA.area.Right, roomA.area.Y + roomA.area.Height);
        Point rightBottomB = new Point(roomB.area.Right, roomB.area.Y + roomB.area.Height);

        if (roomA.area.IntersectsWith(roomB.area))
        {          
            ////If B is below A and also If B is below A but also on the right
            ////Console.WriteLine("rightBottomA.Y = " + rightBottomA.Y + "roomB.Y = " + roomB.area.Y);
            Console.WriteLine("rightBottomA.X = " + rightBottomA.X + " roomB.area.X = " + roomB.area.X);
            if (roomA.area.X <= roomB.area.X && roomB.area.Y > roomA.area.Y && rightBottomA.X <= rightBottomB.X  && rightBottomA.X -1 != roomB.area.X + 1 && roomA.doorBottom == false && roomB.doorTop == false && rightBottomA.Y <= roomB.area.Y + 1)
            {
                doors.Add(new Door(new Point(Utils.Random(roomB.area.X + 1, rightBottomA.X - 1), roomB.area.Y), roomA, roomB));
                roomA.doorBottom = true;
                roomB.doorTop = true;
            }


            //Console.WriteLine("rightBottomB.Y = " + rightBottomB.Y + " roomA.Y = " + roomA.area.Y);
            //If B is above A on the Left 
             if (roomA.area.X > roomB.area.X && rightBottomA.Y >= rightBottomB.Y && roomA.area.Y + 1 != rightBottomB.Y && roomA.doorLeft == false && roomB.doorRight == false)
            {
                doors.Add(new Door(new Point(roomA.area.Left, Utils.Random(roomA.area.Y + 1, rightBottomB.Y - 1)), roomA, roomB));
                roomA.doorLeft = true;
                roomB.doorRight = true;
            }

            //Console.WriteLine("rightBottomA.X = " + rightBottomA.X + " roomB.area.X = " + roomB.area.X);
            // Console.WriteLine("rightBottomA.Y = " + rightBottomA.Y + " rightBottomB.Y = " + rightBottomB.Y);

            //If B is next to A on the right 
             if (rightBottomA.X == roomB.area.X + 1 && rightBottomA.Y == rightBottomB.Y && roomA.area.Y == roomB.area.Y && roomA.doorRight == false && roomB.doorLeft == false)
            {
                doors.Add(new Door(new Point(roomB.area.Left, Utils.Random(roomA.area.Y + 1, roomB.area.Bottom - 1)), roomA, roomB));
                roomA.doorRight = true;
                roomB.doorLeft = true;
            }

            ////If B is next to A on the left
             if (rightBottomB.X == roomA.area.X + 1 && rightBottomA.Y == rightBottomB.Y && roomA.area.Y == roomB.area.Y && roomA.doorLeft == false && roomB.doorRight == false)
            {
                doors.Add(new Door(new Point(roomB.area.Right - 1, Utils.Random(roomA.area.Y + 1, roomB.area.Bottom - 1)), roomA, roomB));
                roomA.doorLeft = true;
                roomB.doorRight = true;
            }

            ////If B is Above A on the Right
            ////Console.WriteLine("rightBottomB.Y = " + rightBottomB.Y + "roomA.area.Y  = " + roomA.area.Y);
            //Console.WriteLine("roomB.area.Bottom = " + roomB.area.Bottom + "rightBottomA.Y = " + rightBottomA.Y);
             if (roomB.area.X > roomA.area.X && rightBottomB.Y - 1 > roomA.area.Y + 1 && rightBottomB.X != roomA.area.X && rightBottomA.Y >= rightBottomB.Y && roomB.area.Bottom != rightBottomA.Y && roomA.doorRight == false && roomB.doorLeft == false)
            {
                doors.Add(new Door(new Point(roomA.area.Right - 1, Utils.Random(roomA.area.Y + 1, rightBottomB.Y - 1)), roomA, roomB));
                roomA.doorRight = true;
                roomB.doorLeft = true;
            }

            ////If B is Above A
            ////Console.WriteLine("RoomA.area.X = " + roomA.area.X + " rightBottomB.X = " + rightBottomB.X);
             if (roomA.area.Y > roomB.area.Y && roomA.area.Y + 1 >= rightBottomB.Y && roomA.area.X != rightBottomB.X && rightBottomB.X >= rightBottomA.X && roomB.area.X <= roomA.area.X && roomB.doorBottom == false && roomA.doorTop == false)
            {
                doors.Add(new Door(new Point(Utils.Random(roomB.area.X + 1, roomA.area.Right - 1), roomA.area.Y), roomA, roomB));
                roomA.doorTop = true;
                roomB.doorBottom = true;
            }
        }
    }
}


