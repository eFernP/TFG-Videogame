using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerExit : MonoBehaviour
{
    // Start is called before the first frame update

    private int doorNumber;
    DoorPuzzleAction action;
    
    TriggerEntry triggerEntry;
    void Start()
    {
        triggerEntry = GameObject.Find("DoorsRoomEntry").GetComponent<TriggerEntry>();
    }

    void OnTriggerEnter()
    {
      triggerEntry.resetRoom(doorNumber, action);
    }

    public void SetDoorNumber(int n)
    {
        this.doorNumber = n;
    }

    public void SetAction(DoorPuzzleAction value)
    {
        this.action = value;
    }
}
