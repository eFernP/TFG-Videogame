using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnviromentUnit : MonoBehaviour
{

    
    //deprecated
    private RandomEdgeType north;
    private RandomEdgeType south;
    private RandomEdgeType east;
    private RandomEdgeType west;
    private int[] coordinates;
    // 


    private Dictionary<string, RandomEdgeType> edges = new Dictionary<string, RandomEdgeType>()
    {
        { "north", RandomEdgeType.Empty },
        { "south", RandomEdgeType.Empty },
        { "east", RandomEdgeType.Empty },
        { "west", RandomEdgeType.Empty }
    };


    public Dictionary<string, RandomEdgeType> Edges
    {
        get
        { 
            return edges;
        }
        set
        { 
            edges = value; 
        }
    }
    

    //deprecated
    public void setNorth(RandomEdgeType value)
    {
        this.north = value;
    }

    public void setSouth(RandomEdgeType value)
    {
        this.south = value;
    }

    public void setEast(RandomEdgeType value)
    {
        this.east = value;
    }

    public void setWest(RandomEdgeType value)
    {
        this.west = value;
    }
    //

    public void setEdge(RandomEdgeType value, string edgeName)
    {
        this.edges[edgeName]=value;
    }

    public void setCoordinates(int[] coordinates)
    {
        this.coordinates = coordinates;
    }

    //deprecated
    public Dictionary<string, RandomEdgeType> getFeatures()
    {
        Dictionary<string, RandomEdgeType> features = new Dictionary<string, RandomEdgeType>();
        
        features.Add("north", this.north);
        features.Add("south", this.south);
        features.Add("east", this.east);
        features.Add("west", this.west);

        return features;
    }

    public int[] getCoordinates()
    {
        return this.coordinates;
    }

    public List<string> getEdgesWithWalls()
    {
        List<string> list = new List<string>();

        if(this.edges["north"] == RandomEdgeType.Wall)
        {
            list.Add("north");
        }
        if (this.edges["south"] == RandomEdgeType.Wall)
        {
            list.Add("south");
        }
        if (this.edges["east"] == RandomEdgeType.Wall)
        {
            list.Add("east");
        }
        if (this.edges["west"] == RandomEdgeType.Wall)
        {
            list.Add("west");
        }

        return list;

    }

}
