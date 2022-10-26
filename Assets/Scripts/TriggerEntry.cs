using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerEntry : MonoBehaviour
{
    Player player;
    public GameObject doorPrefab;
    public GameObject wallPrefab;
    public GameObject uiText;

    private int maxDoorNumber = 8;

    bool areDoorsInverted = false;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        createNewDoors(5, 3);
    }

    // Update is called once per frame
    void Update()
    {

    }


    void createNewDoors(int newDoorsNumber, int usedDoor, bool isInverting = false)
    {
        int doorWidth = 5;
        float firstXPosition = transform.position.x - doorWidth * ((newDoorsNumber - 1) / 2);
        firstXPosition = newDoorsNumber % 2 == 0 ? firstXPosition - 2.5f : firstXPosition;


        //int maxCorrectDoorIndex = action == DoorPuzzleAction.Decrease ? usedDoor-1 : usedDoor;

        for (int i = 0; i < newDoorsNumber; i++)
        {
            GameObject door = Instantiate(doorPrefab, new Vector3(firstXPosition + (doorWidth * i), transform.parent.position.y, transform.parent.position.z + 18), Quaternion.identity);
            TriggerExit doorScript = door.GetComponentInChildren<TriggerExit>();

            int doorNumber = i + 1;
            doorScript.SetDoorNumber(doorNumber);

            DoorPuzzleAction newAction;

            if (doorNumber > usedDoor)
            {
                if (isInverting)
                {
                    newAction = areDoorsInverted ? DoorPuzzleAction.Increase : DoorPuzzleAction.Decrease;
                }
                else
                {
                    newAction = areDoorsInverted ? DoorPuzzleAction.Decrease : DoorPuzzleAction.Increase;
                }
            }
            else if (doorNumber < usedDoor)
            {
                if (isInverting)
                {
                    newAction = areDoorsInverted ? DoorPuzzleAction.Decrease : DoorPuzzleAction.Increase;
                }
                else
                {
                    newAction = areDoorsInverted ? DoorPuzzleAction.Increase : DoorPuzzleAction.Decrease;
                }
            }
            else newAction = DoorPuzzleAction.Invert;


            doorScript.SetAction(newAction);

            if (i == 0) Instantiate(wallPrefab, new Vector3(firstXPosition - 3, 3, -3), Quaternion.identity);
            if (i == newDoorsNumber - 1) Instantiate(wallPrefab, new Vector3(firstXPosition + (doorWidth * i) + 3, 3, -3), Quaternion.identity);

        }

        if (isInverting) areDoorsInverted = !areDoorsInverted;
    }

    public void resetRoom(int doorNumber, DoorPuzzleAction action)
    {
        player.MoveToPosition(transform.position);


        GameObject[] currentDoors = GameObject.FindGameObjectsWithTag("PuzzleDoor");

        foreach (GameObject currentDoor in currentDoors)
            GameObject.Destroy(currentDoor);

        bool isInverting = action == DoorPuzzleAction.Invert;
        int totalDoorNumber = currentDoors.Length - 2;
        int newTotalDoorNumber = totalDoorNumber;
        if (action == DoorPuzzleAction.Increase) newTotalDoorNumber = newTotalDoorNumber + 1;
        else if (action == DoorPuzzleAction.Decrease) newTotalDoorNumber = newTotalDoorNumber - 1;

        if (newTotalDoorNumber == 1)
        {
            uiText.GetComponent<Text>().text = "You Win";
            createNewDoors(newTotalDoorNumber, doorNumber, isInverting);
        }
        else if (newTotalDoorNumber > maxDoorNumber)
        {
            uiText.GetComponent<Text>().text = "You Lost";
        }
        else
        {
            createNewDoors(newTotalDoorNumber, doorNumber, isInverting);
        }



    }
}
