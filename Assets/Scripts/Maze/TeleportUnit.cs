using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportUnit : MonoBehaviour
{
    public Vector3 destinationCoordinates;
    public Vector3 coordinates;
    public int orientation;
    public int destinationOrientation;

    private int room;
    public bool hasMazeDestination;

    private GameObject SceneManager;
    private Collider collider;

    public int Room
    {
        get
        {
            return room;
        }
        set
        {
            room = value;
        }
    }

    public void setDestination(Vector3 newDestination)
    {
        destinationCoordinates = newDestination;
    }

    public Vector3 getDestination()
    {
        return destinationCoordinates;
    }

    public void setCoordinates(Vector3 newCoordinates)
    {
        coordinates = newCoordinates;
    }

    public Vector3 getCoordinates()
    {
        return coordinates;
    }

    public void setOrientation(int newOrientation)
    {
        orientation = newOrientation;
    }

    public int getOrientation()
    {
        return orientation;
    }

    public void setDestinationOrientation(int newOrientation)
    {
        destinationOrientation = newOrientation;
    }

    public int getDestinationOrientation()
    {
        return destinationOrientation;
    }


    void Start()
    {
        if (hasMazeDestination == null)
        {
            Debug.Log("HAS NULL BOOL");
            hasMazeDestination = false;
        }

        SceneManager = GameObject.FindGameObjectsWithTag("SceneManager")[0];
        collider = this.GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (SceneManager.GetComponent<Timer>().hasEnded())
        {
            collider.isTrigger = false;
        }
        else
        {
            collider.isTrigger = true;
        }
    }
}