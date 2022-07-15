using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomRandomizer
{
    private List<Room> allRooms = new List<Room>();
    private List<WarpPoint> availableWarps = new List<WarpPoint>();
    private int maxWarps;
    private bool wantingWarpWillCloseRoom = false;
    private int loopCount = 0;

    public void RandomizeRooms(List<Room> roomsToRandomize, string startingRoom)
    {
        allRooms = roomsToRandomize;
        foreach (Room room in allRooms)
        {
            //this isn't how things will work but I need to test. FIX LATER
            if(room.gameObject.name != startingRoom)
            {
                room.gameObject.SetActive(false);
            }
            foreach (WarpPoint w in room.Warps())
            {
                availableWarps.Add(w);
            }
        }

        int openRoutes = 0;
        List<Room> availableRooms = new List<Room>();
        List<Room> roomQueue = new List<Room>();
        List<WarpPoint> usedWarps = new List<WarpPoint>();
        foreach (Room r in allRooms)
        {
            availableRooms.Add(r);
        }

        //start by choosing the first room for the room queue, and update the openRoutes
        int randIndex = Random.Range(0, availableRooms.Count);
        Room chosenRoom = availableRooms[randIndex];

        foreach (WarpPoint w in chosenRoom.Warps())
        {
            openRoutes++;
        }
        int i = Random.Range(0, chosenRoom.Warps().Count);
        WarpPoint wantingWarp = chosenRoom.Warps()[i];
        roomQueue.Add(chosenRoom);
        if(chosenRoom.Warps().Count == 1)
        {
            wantingWarpWillCloseRoom = true;
        }

        maxWarps = availableWarps.Count;
        
        while (usedWarps.Count < maxWarps && loopCount < 200)
        {
            loopCount++;
            Debug.Log("Max warps: " + maxWarps);
            Debug.Log("Used warps: " + usedWarps.Count);
            Debug.Log("Available room count: " + availableRooms.Count);
            string aRooms = "Rooms are: ";
            foreach(Room x in availableRooms)
            {
                aRooms = aRooms + x.gameObject.name + " ";
            }
            Debug.Log(aRooms);
            //first randomly pick a room from the available rooms
            int index = Random.Range(0, availableRooms.Count);
            Room pickedRoom = availableRooms[index];

            //check to make sure that the room isn't the room we are trying to connect to
            if(pickedRoom.gameObject.name != wantingWarp.ParentRoom().gameObject.name)
            {
                
                List<WarpPoint> possibleWarps = new List<WarpPoint>();
                foreach (WarpPoint warp in pickedRoom.Warps())
                {
                    if(warp.AdjWarp() == null)
                    {
                        possibleWarps.Add(warp);
                    }
                }
                Debug.Log("PossibleWarps count: " + possibleWarps.Count);
                if(possibleWarps.Count == 0)
                {
                    Debug.LogWarning("Room " + pickedRoom.gameObject.name + " has no warps and remained in the available Rooms. Fix this issue.");
                    availableRooms.Remove(pickedRoom);
                }
                else if(possibleWarps.Count == 1)
                {
                    Debug.Log("openRoutes: " + openRoutes);
                    if(openRoutes == 1)
                    {
                        //Either this room cannot be used here, or it could be the last room. Check.
                        //-----THIS WILL NEVER FIRE! The wanting warp room is also on the available rooms count!--------
                        if(availableRooms.Count == 2)
                        {
                            //this is the last available room, along with wanting warps, so connect up the warps and call it a day
                            possibleWarps[0].SetAdjWarp(wantingWarp);
                            wantingWarp.SetAdjWarp(possibleWarps[0]);

                            Debug.Log("Final pairing: " + possibleWarps[0].gameObject.transform.parent.name + " and " + wantingWarp.gameObject.transform.parent.name);
                            Debug.Log("Open Routes " + openRoutes);

                            //after adding the last two warps, this should put the used warps equal to available warps, and end the loop
                            usedWarps.Add(wantingWarp);
                            usedWarps.Add(possibleWarps[0]);
                        }
                        else
                        {
                            //This is where the infinite loop is happening!!!!! However this should now be fixed
                            //do nothing, and the loop will pick a different room
                        }
                    }
                    else if(openRoutes == 2)
                    {
                        if(availableRooms.Count == 2)
                        {
                            //these are the last two rooms, so connect up the warps and call it a day
                            possibleWarps[0].SetAdjWarp(wantingWarp);
                            wantingWarp.SetAdjWarp(possibleWarps[0]);

                            Debug.Log("Final pairing: " + possibleWarps[0].gameObject.transform.parent.name + " and " + wantingWarp.gameObject.transform.parent.name);
                            Debug.Log("Open Routes " + openRoutes);

                            //after adding the last two warps, this should put the used warps equal to available warps, and end the loop
                            usedWarps.Add(wantingWarp);
                            usedWarps.Add(possibleWarps[0]);
                        }
                        else if(availableRooms.Count > 2 && roomQueue.Contains(pickedRoom))
                        {
                            //we cannot connnect to this room, as it will seal a loop and leave rooms out of the map. reroll
                        }
                        else if(availableRooms.Count > 2 && !roomQueue.Contains(pickedRoom))
                        {
                            //This will seal up this route, but there are others, so there is no worries
                            possibleWarps[0].SetAdjWarp(wantingWarp);
                            wantingWarp.SetAdjWarp(possibleWarps[0]);

                            if(roomQueue.Contains(pickedRoom))
                            {
                                //This room was already visited, meaning we sealed 2 routes rather than 1
                                roomQueue.Remove(pickedRoom);
                                openRoutes -= 2;
                            }
                            else
                            {
                                openRoutes--;
                            }

                            if(wantingWarpWillCloseRoom)
                            {
                                //the wanting warp was the last warp to that room, so it is now closed and needs to be removed
                                availableRooms.Remove(wantingWarp.ParentRoom());
                                roomQueue.Remove(wantingWarp.ParentRoom());
                            }

                            availableRooms.Remove(pickedRoom);

                            Debug.Log("Closing loop pairing: " + possibleWarps[0].gameObject.transform.parent.name + " and " + wantingWarp.gameObject.transform.parent.name);
                            Debug.Log("Open Routes " + openRoutes);

                            //after adding the last two warps, this should put the used warps equal to available warps, and end the loop
                            usedWarps.Add(wantingWarp);
                            usedWarps.Add(possibleWarps[0]);

                            //remove the picked room from available rooms, and find a new wanting warp from further down the queue
                            
                            Debug.Log("Finding wanting warp");

                            wantingWarp = FindNewWantingWarp(roomQueue, availableRooms);
                        }
                        else
                        {
                            Debug.LogWarning("I am major borked");
                        }
                    }
                    else
                    {                        
                        //This will seal up this route, but there are others, so there is no worries
                        possibleWarps[0].SetAdjWarp(wantingWarp);
                        wantingWarp.SetAdjWarp(possibleWarps[0]);

                        if(roomQueue.Contains(pickedRoom))
                        {
                            //This room was already visited, meaning we sealed 2 routes rather than 1
                            roomQueue.Remove(pickedRoom);
                            openRoutes -= 2;
                        }
                        else
                        {
                            openRoutes--;
                        }

                        if(wantingWarpWillCloseRoom)
                        {
                            //the wanting warp was the last warp to that room, so it is now closed and needs to be removed
                            availableRooms.Remove(wantingWarp.ParentRoom());
                            roomQueue.Remove(wantingWarp.ParentRoom());
                        }

                        availableRooms.Remove(pickedRoom);

                        Debug.Log("Closing loop pairing: " + possibleWarps[0].gameObject.transform.parent.name + " and " + wantingWarp.gameObject.transform.parent.name);
                        Debug.Log("Open Routes " + openRoutes);

                        //after adding the last two warps, this should put the used warps equal to available warps, and end the loop
                        usedWarps.Add(wantingWarp);
                        usedWarps.Add(possibleWarps[0]);

                        //remove the picked room from available rooms, and find a new wanting warp from further down the queue
                        
                        Debug.Log("Finding wanting warp");

                        wantingWarp = FindNewWantingWarp(roomQueue, availableRooms);
                    }
                }
                else
                {
                    //there are multiple routes to this room. Choose one to connect to wanting warp and remove it from the possible options.
                    int j = Random.Range(0, possibleWarps.Count);
                    
                    possibleWarps[j].SetAdjWarp(wantingWarp);
                    wantingWarp.SetAdjWarp(possibleWarps[j]);


                    Debug.Log("open loop pairing: " + possibleWarps[0].gameObject.transform.parent.name + " and " + wantingWarp.gameObject.transform.parent.name);

                    usedWarps.Add(wantingWarp);
                    usedWarps.Add(possibleWarps[j]);


                    if(!roomQueue.Contains(pickedRoom))
                    {
                        roomQueue.Add(pickedRoom);
                        if(possibleWarps.Count > 2)
                        {
                            openRoutes += possibleWarps.Count - 2;
                        }
                    }
                    else
                    {
                        //this room has already been visited, meaning we just cut the open routes down by 2
                        openRoutes -= 2;
                    }

                    if(wantingWarpWillCloseRoom)
                    {
                        availableRooms.Remove(wantingWarp.ParentRoom());
                        roomQueue.Remove(wantingWarp.ParentRoom());
                    }

                    
                    Debug.Log("Open Routes " + openRoutes);
                    possibleWarps.Remove(possibleWarps[j]);
                    int k = Random.Range(0, possibleWarps.Count);
                    wantingWarp = possibleWarps[k];

                    if(possibleWarps.Count == 1)
                    {
                        //This is the last warp and will close the room
                        wantingWarpWillCloseRoom = true;
                    }
                    else
                    {
                        //This is not the last warp so it will not close the room
                        wantingWarpWillCloseRoom = false;
                    }
                }
            }
            else
            {
                if(availableRooms.Count == 1)
                {
                    //the algorithm was forced into a scenario where we have to connect up the final room to itself
                    //I will want to fix this later, but for now we will just allow it
                    foreach(WarpPoint w in pickedRoom.Warps())
                    {
                        if(w != wantingWarp && w.AdjWarp() == null)
                        {
                            w.SetAdjWarp(wantingWarp);
                            wantingWarp.SetAdjWarp(w);
                            usedWarps.Add(wantingWarp);
                            usedWarps.Add(w);
                        }
                    }
                }
            }
        }
        if(loopCount == 200)
        {
            Debug.LogWarning("Was forced to break out of loop!");
        }
        Debug.Log("All doors should be connected");
        foreach (Room r in allRooms)
        {
            foreach(WarpPoint w in r.Warps())
            {
                if(w.AdjWarp() == null)
                {
                    Debug.LogWarning("Door in room " + r.name + " failed to get a connecting warp");
                }
            }
        }
    }

    private WarpPoint FindNewWantingWarp(List<Room> roomQueue, List<Room> availableRooms)
    {
        Room nextRoom = roomQueue[roomQueue.Count - 1];

        Debug.Log(nextRoom.gameObject.name);

        List<WarpPoint> possibleWarps = new List<WarpPoint>();
        WarpPoint pointToReturn = null;
        foreach (WarpPoint w in nextRoom.Warps())
        {
            if(w.AdjWarp() == null)
            {
                possibleWarps.Add(w);
            }
        }
        if(possibleWarps.Count == 0)
        {
            Debug.Log("Removal attempt incoming. Pre removal: ");
            foreach (Room r in roomQueue)
            {
                Debug.Log(r.gameObject.name);
            }

            roomQueue.Remove(nextRoom);
            availableRooms.Remove(nextRoom);

        
            pointToReturn = FindNewWantingWarp(roomQueue, availableRooms);
            
        }
        else if(possibleWarps.Count == 1)
        {
            pointToReturn = possibleWarps[0];
            wantingWarpWillCloseRoom = true;
        }
        else
        {
            int i = Random.Range(0, possibleWarps.Count);
            pointToReturn = possibleWarps[i];
            wantingWarpWillCloseRoom = false;
        }
        return pointToReturn;
    }
}
