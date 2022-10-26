using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnFloor : MonoBehaviour
{

    public GameObject RandomEnviromentUnit;
    public GameObject Floor;
    public GameObject BrokenFloor1;
    public GameObject BrokenFloor2A;
    public GameObject BrokenFloor2B;
    public GameObject BrokenFloor3;


    private RandomEdgeType[] BF1DefaultConfig = {RandomEdgeType.Wall, RandomEdgeType.Wall, RandomEdgeType.Empty, RandomEdgeType.Wall};
    private RandomEdgeType[] BF2ADefaultConfig = {RandomEdgeType.Wall, RandomEdgeType.Empty, RandomEdgeType.Empty, RandomEdgeType.Wall};
    private RandomEdgeType[] BF2BDefaultConfig = {RandomEdgeType.Wall, RandomEdgeType.Empty, RandomEdgeType.Wall, RandomEdgeType.Empty};
    private RandomEdgeType[] BF3DefaultConfig = {RandomEdgeType.Empty, RandomEdgeType.Empty, RandomEdgeType.Empty, RandomEdgeType.Wall};

    private int[] matrixDimensions = {5, 10};

    bool isOutOfGrid(int position, int maxValue){

        return position >= maxValue || position < 0 ;
    }

    string getOppositeCardinalPoint(string cardinalPoint){
        if(cardinalPoint == "north") return "south";
        if(cardinalPoint == "south") return "north";
        if(cardinalPoint == "east") return "west";
        return "east";
    }

    RandomEdgeType getRandomEdge(){
  
        RandomEdgeType[] values = (RandomEdgeType[]) System.Enum.GetValues(typeof(RandomEdgeType));
        return values[Random.Range(0, values.Length)];
    }


    bool isWestAndEastEmpty(RandomEdgeType[] config){
        return config[0]==RandomEdgeType.Empty && config[2]==RandomEdgeType.Empty;
    }

    bool isNorthAndSouthEmpty(RandomEdgeType[] config){
        return config[1]==RandomEdgeType.Empty && config[3]==RandomEdgeType.Empty;
    }


    GameObject getFloorType (RandomEdgeType[] config){

        int emptyCount = 0;


        foreach(RandomEdgeType edgeValue in config)
            if(edgeValue == RandomEdgeType.Empty)
                emptyCount++;

        if(emptyCount == 0) return Floor;
        else if(emptyCount == 1) return BrokenFloor1;
        else if(emptyCount == 2){
            if(isWestAndEastEmpty(config) || isNorthAndSouthEmpty(config))
                return BrokenFloor2B;
            else
                return BrokenFloor2A;
        }
        else if(emptyCount == 3) return BrokenFloor3;
        else return null;
    }


    RandomEdgeType[] transformDictionaryToArray(Dictionary<string, RandomEdgeType> features){
        RandomEdgeType[] arr = new RandomEdgeType[4];

        foreach(string key in features.Keys){
            int position;

            RandomEdgeType value = features[key];
            if(value == RandomEdgeType.Door) value = RandomEdgeType.Wall;


            if(key=="north") position = 0;
            else if(key == "east") position = 1;
            else if(key == "south") position = 2;
            else position = 3;

             arr.SetValue( value, position);
        }
        return arr;
    }

    int findFirstWallIndex(RandomEdgeType[] config){
        int index = 0;
        for (int i = 0; i < config.Length; i++){
            if(config[i] == RandomEdgeType.Wall && config[(i+1)%4] == RandomEdgeType.Wall){
                index = i;
                break;
            }
        }

        return index;
    }

    int getYRotation(RandomEdgeType[] currentConfig, GameObject chosenFloor){
        currentConfig.SequenceEqual(currentConfig);

        if(chosenFloor.name == "BrokenFloor1"){
            if(currentConfig.SequenceEqual(BF1DefaultConfig)) return 0;
            else{
                int emptyIndex = System.Array.IndexOf(currentConfig, RandomEdgeType.Empty);
                int defaultEmptyIndex =  System.Array.IndexOf(BF1DefaultConfig, RandomEdgeType.Empty);
                return (emptyIndex-defaultEmptyIndex) * 90;
            }
        }

        if(chosenFloor.name == "BrokenFloor3"){
            if(currentConfig.SequenceEqual(BF3DefaultConfig)) return 0;
            else{
                int wallIndex = System.Array.IndexOf(currentConfig, RandomEdgeType.Wall);
                int defaultWallIndex =  System.Array.IndexOf(BF3DefaultConfig, RandomEdgeType.Wall);
                return (wallIndex-defaultWallIndex) * 90;
            }
        }

        if(chosenFloor.name == "BrokenFloor2A"){
            if(currentConfig.SequenceEqual(BF2ADefaultConfig)) return 0;
            else{
                int firstWallIndex = findFirstWallIndex(currentConfig);
                int firstDefaultWallIndex =  3;
                return (firstWallIndex-firstDefaultWallIndex) * 90;

            }
        }

        if(chosenFloor.name == "BrokenFloor2B"){
            if(currentConfig.SequenceEqual(BF2BDefaultConfig)) return 0;
            else{
                return 90;
            }
        }

        return 0;
    }

    int[] findAdjacentPosition(string edgeLocation, int i, int j){
        int iPosition = i;
        int jPosition = j;

        if(edgeLocation == "north") jPosition++;
        if(edgeLocation == "south") jPosition--;
        if(edgeLocation == "east") iPosition++;
        if(edgeLocation == "west") iPosition--;

        int[] position = {iPosition, jPosition};
        return position;
    }



    RandomEdgeType setEdge(string edgeLocation, int i, int j){
   
        int[] adjacentPosition = findAdjacentPosition(edgeLocation, i, j);
        GameObject AdjacentGround = GameObject.Find("GroundUnit_"+ adjacentPosition[0] + "_" + adjacentPosition[1]);
        //get All adjacent grounds
        // check edges to check walls example -> if edgeLocation = north, check north edges for left and right grounds
        // and...?? (decide rules to set walls)

        if(!AdjacentGround) {
            if(isOutOfGrid(adjacentPosition[0], matrixDimensions[0]) || isOutOfGrid(adjacentPosition[1], matrixDimensions[1])){
                return RandomEdgeType.Empty;
            }else{
                return getRandomEdge();
            }
        }else{
            Dictionary<string, RandomEdgeType> AdjacentGroundFeatures= AdjacentGround.GetComponent<RandomEnviromentUnit>().getFeatures();
            return AdjacentGroundFeatures[getOppositeCardinalPoint(edgeLocation)];
        }
    }

    void instantiateWalls(){

        // Case Broken Wall:
        // Edge NORTH o SOUTH -> Si el mateix edge de l'esquerra està Empty, no es toca, si es el de la dreta, 180º
        // Edge EAST o WEST -> -> Si el mateix edge de davant està Empty, 90, i si es el del darrere, -90

        // Positions

        // East -> eix X -> floor pos - 5(size del floor)
        // North-> eix Z -> floor pos + 5(size del floor)
        // West-> eix X -> floor pos + 5(size del floor)
        // South-> eix Z -> floor pos - 5(size del floor)



        //foreach edge
        //check if exists instance
        //if value is wall, findAdjacentValues
            //if one value is empty, instantiate broken wall
            //getBrokenWallRotation
        //getWallPosition
        //instantiate

    }

   
    void Start()
    {
        GameObject[] floors = {Floor, BrokenFloor1, BrokenFloor2A, BrokenFloor2B, BrokenFloor3};
        for(int i = 0; i < matrixDimensions[0]; i++)
        {

            for(int j = 0; j <  matrixDimensions[1]; j++)
            {
                GameObject GroundUnit = Instantiate(RandomEnviromentUnit, new Vector3(5 + 10 * i, 0, 5 + 10 * j), Quaternion.identity);

                RandomEnviromentUnit GroundUnitScript = GroundUnit.GetComponent<RandomEnviromentUnit>();

                GroundUnit.name= "GroundUnit_"+ i + "_" + j;
                GroundUnitScript.setNorth(setEdge("north", i, j));
                GroundUnitScript.setSouth(setEdge("south", i, j));
                GroundUnitScript.setEast(setEdge("east", i, j));
                GroundUnitScript.setWest(setEdge("west", i, j));

                Dictionary<string, RandomEdgeType> features = GroundUnitScript.getFeatures();
                RandomEdgeType[] configArray = transformDictionaryToArray(features);


                GameObject floor = getFloorType(configArray);

                if(floor){
                    int yRotation = getYRotation(configArray, floor);
                    GameObject UnitFloor = Instantiate(floor, new Vector3(5 + 10 * i, 0, 5 + 10 * j), Quaternion.Euler (-90, yRotation, 0), GroundUnit.transform);
                }

            }

        }
    }


}



/*

Guardar como graph, donde los nodos son las GroundUnit, y los edges tienen como valor RandomEdgeType

Estructura AdjacentList, cada nodo tiene guardados los nodos con los que está relacionado 



*/