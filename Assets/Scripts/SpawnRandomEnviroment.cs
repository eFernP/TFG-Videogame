using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class WeightedUnit
{
    public GameObject unit { get; set; }
    public int weight { get; set; }
}

public class SpawnRandomEnviroment : MonoBehaviour
{

    public GameObject EnviromentUnit;
    public GameObject Floor;
    public GameObject Wall;
    public GameObject Door;

    public GameObject WallUI;
    public GameObject MapUnit;
    public GameObject Map;

    private int FLOOR_SIZE = 10;
    private int UI_FLOOR_SIZE = 25;
    private int MAX_DOORS_FOR_ROOM = 3;
    private int MAX_PATH = 3;
    private int MAX_DOORS = 50;
    int MAX_ROOM_SIZE = 5;
    int MIN_ROOM_SIZE = 1;

    private int doorCounter = 0;
    private int pathCounter = 0;
    private int nestingIndex = 1;
    private Color pathColor;

    string getOppositeCardinalPoint(string cardinalPoint)
    {
        if (cardinalPoint == "north") return "south";
        if (cardinalPoint == "south") return "north";
        if (cardinalPoint == "east") return "west";
        return "east";
    }

    string getRandomCardinalPoint()
    {

        string[] values = { "north", "south", "east", "west" };
        return values[Random.Range(0, values.Length)];
    }

    int[] getRandomRoomSize()
    {
        int[] size = { Random.Range(MIN_ROOM_SIZE, MAX_ROOM_SIZE), Random.Range(MIN_ROOM_SIZE, MAX_ROOM_SIZE) };
        return size;
    }

    int getWallRotation(string edgeName)
    {
        if (edgeName == "north" || edgeName == "south")
        {
            return 0;
        }
        return 90;
    }

    float getWallXPosition(string edgeName, int unitSize)
    {
        if (edgeName == "north" || edgeName == "south")
        {
            return (float)unitSize / 2;
        }
        if (edgeName == "west")
        {
            return 0;
        }
        return FLOOR_SIZE;
    }

    float getUIWallXPosition(string edgeName, int unitSize)
    {
        if (edgeName == "north" || edgeName == "south")
        {
            return 0;
        }
        if (edgeName == "east")
        {
            return (float)unitSize / 2;
        }
        return -((float)unitSize / 2);
    }

    float getWallZPosition(string edgeName, int unitSize)
    {
        if (edgeName == "west" || edgeName == "east")
        {
            return (float)unitSize / 2;
        }
        if (edgeName == "north")
        {
            return FLOOR_SIZE;
        }
        return 0;

    }

    float getUIWallZPosition(string edgeName, int unitSize)
    {
        if (edgeName == "east" || edgeName == "west")
        {
            return 0;
        }
        if (edgeName == "north")
        {
            return (float)unitSize / 2;
        }
        return -((float)unitSize / 2);
    }

    //check if there is a door in a edge of a position
    bool checkDoor(List<List<object>> doorsPosition, int x, int z, string edge)
    {
        bool hasDoor = false;
        foreach (List<object> doorPosition in doorsPosition)
        {
            if ((string)doorPosition[0] == edge && (int)doorPosition[1] == x && (int)doorPosition[2] == z)
            {

                hasDoor = true;
                break;
            }
        }

        return hasDoor;
    }

    //find the position that is next to the given position and after the given edge
    int[] findAdjacentPosition(string edgeName, int i, int j)
    {
        int iPosition = i;
        int jPosition = j;

        if (edgeName == "north") jPosition++;
        if (edgeName == "south") jPosition--;
        if (edgeName == "east") iPosition++;
        if (edgeName == "west") iPosition--;

        int[] position = { iPosition, jPosition };
        return position;
    }

    //get the unit that is next to the given position and after the given edge
    GameObject getAdjacentUnit(string edgeName, int x, int z)
    {
        int[] adjacentPosition = findAdjacentPosition(edgeName, x, z);
        GameObject UnitInstance = GameObject.Find("EnviromentUnit_" + adjacentPosition[0] + "_" + adjacentPosition[1]);

        return UnitInstance;
    }

    //get the edge type of the edge of the given unit
    RandomEdgeType getUnitEdgeType(string edgeName, GameObject UnitInstance)
    {
        RandomEnviromentUnit UnitScript = UnitInstance.GetComponent<RandomEnviromentUnit>();
        return UnitScript.Edges[edgeName];
    }

    //get the bottom left corner of a room
    int[] getBottomLeftCorner(int[] roomSize, int[] prevRoomDoorPosition, string doorEdge)
    {
        int xPosition;
        int zPosition;

        if (doorEdge == "west")
        {
            xPosition = prevRoomDoorPosition[0] + 1;
            zPosition = Random.Range(prevRoomDoorPosition[1] - roomSize[1] + 1, prevRoomDoorPosition[1]);
        }
        else if (doorEdge == "east")
        {
            xPosition = prevRoomDoorPosition[0] - roomSize[0];
            zPosition = Random.Range(prevRoomDoorPosition[1] - roomSize[1] + 1, prevRoomDoorPosition[1]);
        }
        else if (doorEdge == "north")
        {
            xPosition = Random.Range(prevRoomDoorPosition[0] - roomSize[0] + 1, prevRoomDoorPosition[0]);
            zPosition = prevRoomDoorPosition[1] - roomSize[1];
        }
        else
        {
            xPosition = Random.Range(prevRoomDoorPosition[0] - roomSize[0] + 1, prevRoomDoorPosition[0]);
            zPosition = prevRoomDoorPosition[1] + 1;
        }

        int[] position = { xPosition, zPosition };
        return position;

    }

    //get possible doors locations for a room
    List<List<object>> getDoorsLocation(int[] initialPosition, int[] roomSize)
    {

        List<List<object>> availableDoorsLocation = new List<List<object>>();
        List<List<object>> selectedDoorsLocation = new List<List<object>>();

        //get a number of new doors
        int doorsNumber = Random.Range(2, MAX_DOORS_FOR_ROOM);

        //get all the available positions on the north and south walls and save them in the list. The available positions will be positions whose adjacent position is free.
        for (int i = initialPosition[0]; i < initialPosition[0] + roomSize[0]; i++)
        {

            if (getAdjacentUnit("south", i, initialPosition[1]) == null)
            {
                availableDoorsLocation.Add(new List<object> { (object)"south", (object)i, (object)initialPosition[1] });
            }
            if (getAdjacentUnit("north", i, initialPosition[1] + roomSize[1] - 1) == null)
            {
                availableDoorsLocation.Add(new List<object> { (object)"north", (object)i, (object)(initialPosition[1] + roomSize[1] - 1) });
            }
        }

        //get all the available positions on the north and south walls and save them in the list. The available positions will be positions whose adjacent position is free.
        for (int j = initialPosition[1]; j < initialPosition[1] + roomSize[1]; j++)
        {

            if (getAdjacentUnit("west", initialPosition[0], j) == null)
            {
                availableDoorsLocation.Add(new List<object> { (object)"west", (object)initialPosition[0], (object)j });
            }
            if (getAdjacentUnit("east", initialPosition[0] + roomSize[0] - 1, j) == null)
            {
                availableDoorsLocation.Add(new List<object> { (object)"east", (object)(initialPosition[0] + roomSize[0] - 1), (object)j });
            }
        }

        //for every door that has to be created, get a random position from the available positions list.
        for (int i = 0; i < doorsNumber; i++)
        {
            if (availableDoorsLocation.Count >= 1)
            {
                int index = Random.Range(0, availableDoorsLocation.Count - 1);
                selectedDoorsLocation.Add(availableDoorsLocation[index]);

                //remove the selected position from the available positions list, so the same position cannot be selected twice
                availableDoorsLocation.RemoveAt(index);
            }
            else
            {
                break;
            }
        }

        return selectedDoorsLocation;

    }



    bool createWallOrDoor(GameObject UnitInstance, string edgeName, int x, int z, List<List<object>> doorsPosition, Color roomColor)
    {
        RandomEnviromentUnit UnitScript = UnitInstance.GetComponent<RandomEnviromentUnit>();
        GameObject AdjacentUnit = getAdjacentUnit(edgeName, x, z);

        //Check if exists an adjacent unit
        if (AdjacentUnit == null)
        {

            //Check if the position has a door
            bool isDoor = checkDoor(doorsPosition, x, z, edgeName);

            if (isDoor)
            {
                doorCounter = doorCounter + 1;
            }
            UnitScript.setEdge(isDoor ? RandomEdgeType.Door : RandomEdgeType.Wall, edgeName);

            //Instantiate the unit wall or door
            float WallXPosition = getWallXPosition(edgeName, FLOOR_SIZE);
            float WallZPosition = getWallZPosition(edgeName, FLOOR_SIZE);
            int WallRotation = getWallRotation(edgeName);

            GameObject UnitWall = Instantiate(isDoor ? Door : Wall, new Vector3(WallXPosition + FLOOR_SIZE * x, -2, WallZPosition + FLOOR_SIZE * z), Quaternion.Euler(90, WallRotation, 0), UnitInstance.transform);
            var unitRenderer = UnitWall.GetComponent<Renderer>();
            unitRenderer.material.SetColor("_BaseColor", roomColor);




            if (isDoor)
            {
                UnitWall.name = "Door_" + edgeName;
                UnitWall.tag = "Door";
                return true;

            }
            else
            {

                UnitWall.name = "Wall_" + edgeName;
                UnitWall.tag = "Wall";
            }

        }
        else
        {
            //If there is an adjacent unit, set the edge according to its edge type
            RandomEdgeType adjacentUnitEdgeType = getUnitEdgeType(getOppositeCardinalPoint(edgeName), AdjacentUnit);
            UnitScript.setEdge(adjacentUnitEdgeType, edgeName);
        }

        return false;


    }

    GameObject createUnitInstanceAndFloor(int x, int z, Color roomColor)
    {
        //Check if a unit with the given position exists
        GameObject UnitInstance = GameObject.Find("EnviromentUnit_" + x + "_" + z);



        //Return null because the position is occupied and the unit cannot be created
        if (UnitInstance != null)
        {
            return null;
        }

        //Instantiate the unit
        UnitInstance = Instantiate(EnviromentUnit, new Vector3(FLOOR_SIZE / 2 + FLOOR_SIZE * x, 0, FLOOR_SIZE / 2 + FLOOR_SIZE * z), Quaternion.identity, this.transform);
        RandomEnviromentUnit UnitInstanceScript = UnitInstance.GetComponent<RandomEnviromentUnit>();
        UnitInstance.name = "EnviromentUnit_" + x + "_" + z;

        UnitInstanceScript.setCoordinates(new int[] { x, z });

        //Instantiate the floor
        GameObject UnitFloor = Instantiate(Floor, new Vector3(FLOOR_SIZE / 2 + FLOOR_SIZE * x, 0, FLOOR_SIZE / 2 + FLOOR_SIZE * z), Quaternion.Euler(-90, 0, 0), UnitInstance.transform);
        var unitRenderer = UnitFloor.GetComponent<Renderer>();
        unitRenderer.material.SetColor("_BaseColor", roomColor);

        return UnitInstance;
    }



    void generateRooms(string prevRoomDoorEdge = null, int[] prevRoomDoorPosition = null)
    {

        //the bottom left corner position that will have the first room
        int[] bottomLeftCorner = { 0, 0 };
        List<List<object>> createdDoorsLocation = new List<List<object>>();


        int[] roomSize = getRandomRoomSize();

        //to see every room in a different color
        //Color roomColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f); 
        //Color roomColor = new Color(0.5f, 0.6f, 0.7f, 0.8f);
        Color roomColor = pathColor;
        // if there is prevRoomEdge, is not the first room so the bottomLeftCorner must be calculate
        if (prevRoomDoorEdge != null)
        {
            //calculate the bottom left corner for this room. Get the opposite edge because the prevRoomDoorEdge is from the point of view of the previous room
            string firstDoorEdge = getOppositeCardinalPoint(prevRoomDoorEdge);
            bottomLeftCorner = getBottomLeftCorner(roomSize, prevRoomDoorPosition, firstDoorEdge);
        }


        List<List<object>> suggestedDoorsLocation = getDoorsLocation(bottomLeftCorner, roomSize);

        //iterate all the grid occupied by the current room, starting for the unit located on the bottom left corner 
        for (int i = bottomLeftCorner[0]; i < roomSize[0] + bottomLeftCorner[0]; i++)
        {
            for (int j = bottomLeftCorner[1]; j < roomSize[1] + bottomLeftCorner[1]; j++)
            {

                GameObject UnitInstance = createUnitInstanceAndFloor(i, j, roomColor);

                //If the unit instance has not been created, there is already a unit in this grid position and createUnitInstanceAndFloor will return null
                if (UnitInstance != null)
                {

                    bool isDoor = false;


                    //if the position is a position with wall, create a wall or door, and if a door is created, save the location in the created doors list
                    if (i == bottomLeftCorner[0])
                    {
                        isDoor = createWallOrDoor(UnitInstance, "west", i, j, suggestedDoorsLocation, roomColor);
                        if (isDoor == true)
                        {
                            createdDoorsLocation.Add(new List<object> { "west", (object)i, (object)j });
                        }
                    }
                    if (i == roomSize[0] + bottomLeftCorner[0] - 1)
                    {
                        isDoor = createWallOrDoor(UnitInstance, "east", i, j, suggestedDoorsLocation, roomColor);
                        if (isDoor == true)
                        {
                            createdDoorsLocation.Add(new List<object> { "east", (object)i, (object)j });
                        }
                    }
                    if (j == roomSize[1] + bottomLeftCorner[1] - 1)
                    {
                        isDoor = createWallOrDoor(UnitInstance, "north", i, j, suggestedDoorsLocation, roomColor);
                        if (isDoor == true)
                        {
                            createdDoorsLocation.Add(new List<object> { "north", (object)i, (object)j });
                        }
                    }
                    if (j == bottomLeftCorner[1])
                    {
                        isDoor = createWallOrDoor(UnitInstance, "south", i, j, suggestedDoorsLocation, roomColor);
                        if (isDoor == true)
                        {
                            createdDoorsLocation.Add(new List<object> { "south", (object)i, (object)j });
                        }
                    }
                }
            }
        }

        //For each new door created, if the door counter has not reach yet the max number of doors
        foreach (List<object> doorLocation in createdDoorsLocation)
        {

            if (pathCounter < MAX_PATH && doorCounter <= MAX_DOORS)
            {
                pathCounter++;
                int[] doorPosition = { (int)doorLocation[1], (int)doorLocation[2] };
                generateRooms((string)doorLocation[0], doorPosition);

            }
            else
            {
                pathCounter = 0;
                nestingIndex++;
                pathColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

                if (nestingIndex > 1)
                {
                    nestingIndex--;
                    break;

                }
            }
        }

    }



    List<GameObject> getExteriorAssets(string tag)
    {
        List<GameObject> exteriorAssets = new List<GameObject>();
        GameObject[] assets = GameObject.FindGameObjectsWithTag(tag);

        for (int i = 0; i < assets.Length; i++)
        {
            string edgeName = assets[i].name.Split("_")[1];
            string x = assets[i].transform.parent.name.Split("_")[1];
            string z = assets[i].transform.parent.name.Split("_")[2];

            if (!getAdjacentUnit(edgeName, int.Parse(x), int.Parse(z)))
            {
                exteriorAssets.Add(assets[i]);
            }
        }

        return exteriorAssets;
    }

    GameObject ReplaceDoorForWall(GameObject door, string edgeName)
    {
        door.transform.parent.gameObject.GetComponent<RandomEnviromentUnit>().setEdge(RandomEdgeType.Wall, edgeName);
        GameObject wall = Instantiate(Wall, door.transform.position, door.transform.rotation, door.transform.parent.transform);
        Destroy(door);

        door.name = "Wall_" + edgeName;
        return wall;
    }


    void removeExteriorDoors()
    {
        List<GameObject> doors = getExteriorAssets("Door");

        foreach (GameObject door in doors)
        {
            string edgeName = door.name.Split("_")[1];
            ReplaceDoorForWall(door, edgeName);
        }
    }


    string getFirstFreeEdge(GameObject unit)
    {
        int[] coordinates = unit.GetComponent<RandomEnviromentUnit>().getCoordinates();
        string[] edges = { "north", "east", "south", "west" };

        string freeEdge = "";

        foreach (string edge in edges)
        {
            if (!getAdjacentUnit(edge, coordinates[0], coordinates[1]))
            {
                freeEdge = edge;
                break;
            }
        }

        return freeEdge;
    }

    List<GameObject> getExteriorUnits()
    {
        List<GameObject> exteriorUnits = new List<GameObject>();
        GameObject[] units = GameObject.FindGameObjectsWithTag("RandomUnit");

        for (int i = 0; i < units.Length; i++)
        {
            if (getFirstFreeEdge(units[i]) != "")
            {
                exteriorUnits.Add(units[i]);
            }

        }

        return exteriorUnits;
    }

    GameObject ReplaceWallForDoor(GameObject wall, string edgeName)
    {
        RandomEnviromentUnit ParentScript = wall.transform.parent.gameObject.GetComponent<RandomEnviromentUnit>();
        int[] coordinates = ParentScript.getCoordinates();
        int[] adjacentPosition = findAdjacentPosition(edgeName, coordinates[0], coordinates[1]);
        ParentScript.setEdge(RandomEdgeType.Door, edgeName);

        GameObject door = Instantiate(Door, wall.transform.position, wall.transform.rotation, wall.transform.parent.transform);

        //TODO: Change for special floor with teleport
        Instantiate(Floor, new Vector3(FLOOR_SIZE / 2 + FLOOR_SIZE * adjacentPosition[0], 0, FLOOR_SIZE / 2 + FLOOR_SIZE * adjacentPosition[1]), Quaternion.Euler(-90, 0, 0));

        Destroy(wall);

        door.name = "Door_" + edgeName;

        return door;
    }



    void createSpecialDoor(GameObject unit)
    {
        string edge = getFirstFreeEdge(unit);
        Debug.Log("free edge" + edge);
        foreach (Transform child in unit.transform)
        {
            if (child.gameObject.name == "Wall_" + edge)
            {
                ReplaceWallForDoor(child.gameObject, edge);
            }
        }
    }


    void createSpecialRoomDoors()
    {

        List<GameObject> units = getExteriorUnits();
        Debug.Log(units.Count);

        //the weight is the sum of the coordinates
        List<WeightedUnit> weightedUnits = units.ConvertAll<WeightedUnit>((unit) =>
        new WeightedUnit { unit = unit, weight = unit.GetComponent<RandomEnviromentUnit>().getCoordinates()[0] + unit.GetComponent<RandomEnviromentUnit>().getCoordinates()[1] }
        );

        GameObject entranceUnit = weightedUnits.Aggregate((i1, i2) => i1.weight > i2.weight ? i1 : i2).unit;
        GameObject archiveUnit = weightedUnits.Aggregate((i1, i2) => i1.weight < i2.weight ? i1 : i2).unit;

        createSpecialDoor(entranceUnit);
        createSpecialDoor(archiveUnit);
    }


    float[][] getSeparatedCoordinates(GameObject[] units)
    {
        float[] xValues = new float[units.Length];
        float[] yValues = new float[units.Length];

        for (int i = 0; i < units.Length; i++)
        {
            int[] coordinates = units[i].GetComponent<RandomEnviromentUnit>().getCoordinates();
            xValues[i] = (float)coordinates[0];
            yValues[i] = (float)coordinates[1];
        }

        return new float[][] { xValues, yValues };
    }


    float[] getCenterPoint(GameObject[] units)
    {
        float[][] coordinatesValues = getSeparatedCoordinates(units);
        float[] xValues = coordinatesValues[0];
        float[] yValues = coordinatesValues[1];
        return new float[] { (xValues.Max() + xValues.Min()) / 2, (yValues.Max() + yValues.Min()) / 2 };

    }

    GameObject createMapUnit(GameObject UnitInstance, float[] centerPoint)
    {
        RandomEnviromentUnit UnitInstanceScript = UnitInstance.GetComponent<RandomEnviromentUnit>();
        int[] coordinates = UnitInstanceScript.getCoordinates();

        float x = coordinates[0] * UI_FLOOR_SIZE - centerPoint[0] * UI_FLOOR_SIZE;
        float y = coordinates[1] * UI_FLOOR_SIZE - centerPoint[1] * UI_FLOOR_SIZE;

        GameObject MapUnitInstance = Instantiate(MapUnit, Map.transform);
        RectTransform unitTransform = MapUnitInstance.GetComponent<RectTransform>();
        unitTransform.anchoredPosition = new Vector2(x, y);
        //GameObject MapUnitInstance = Instantiate(MapUnit, unitInstance.transform.position, Quaternion.identity, Map.transform);
        return MapUnitInstance;
    }

    void instantiateMapWall(GameObject mapUnit, string edgeName)
    {
        GameObject Wall = Instantiate(WallUI, mapUnit.transform);
        RectTransform unitTransform = Wall.GetComponent<RectTransform>();
        unitTransform.anchoredPosition = new Vector2(getUIWallXPosition(edgeName, UI_FLOOR_SIZE), getUIWallZPosition(edgeName, UI_FLOOR_SIZE));
        unitTransform.rotation = Quaternion.Euler(0, 0, 90f + getWallRotation(edgeName));
        Wall.name = edgeName;
    }


    void generateMap()
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag("RandomUnit");
        float[] centerPoint = getCenterPoint(units);

        for (int i = 0; i < units.Length; i++)
        {

            GameObject MapUnitInstance = createMapUnit(units[i], centerPoint);
            RandomEnviromentUnit UnitScript = units[i].GetComponent<RandomEnviromentUnit>();
            List<string> walls = UnitScript.getEdgesWithWalls();

            foreach (string wall in walls)
            {
                instantiateMapWall(MapUnitInstance, wall);
            }
        }

    }


    void Start()
    {
        generateRooms();
        removeExteriorDoors();
        createSpecialRoomDoors();
        generateMap();

    }

}

