//Contains Utility methods for the entire game.

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public static class UtilityMethods
{
    //Utility Methods
    //---------------------------------------------------------------------------

    //Rounds a number to the closest interval of a number. Requires the number to round, and the number interval to round by. 
    //Normal rounding by unity is to integers. This method does not need integers.
    public static float RoundToNumber (float numToRound, float roundBy)
    {
        float result = 0;
        float temp = 0;

        temp = numToRound % roundBy;

        if (temp >= roundBy / 2) result = numToRound + (roundBy - temp); //Round Up
        else result = numToRound - temp; //Round Down

        return result;
    }

    //Same as RoundToNumber Method but includes an offset. For example, Eselmon grid is offset by .32
    //In both dimensions. Rounding with the offset causes problems with the rounding results.
    public static float RoundToNumber(float numToRound, float roundBy, float offset)
    {
        float result = 0;
        float temp = 0;

        numToRound -= offset; //Remove offset from original number

        temp = numToRound % roundBy;

        if (temp >= roundBy / 2) result = numToRound + (roundBy - temp); //Round Up
        else result = numToRound - temp; //Round Down

        result += offset; //Add the offset back to the result

        float temp2 = result * 100;
        int round = Convert.ToInt32(temp2);
        result = (float)round / 100;

        return result;
    }

    //Combat Methods
    //---------------------------------------------------------------------------

    //Returns the positions of each square required to move to a specific square. Requires the start position, the target position, and the space in between the centers of the squares.
    //To call the RoundToNumber with offset, use the staticvariable offset
    public static List<Vector2> PathToPosition(Vector2 startPos, Vector2 targetPos, float blockInterval, LayerMask targetMask)
    {

        List<Vector2> path = new List<Vector2>();
        Vector2 currentPos = startPos;
        path.Add(currentPos);

        float direction = 1; //Will either be 1 or -1
        int timesPassedToBreak = 0;

        while (currentPos != targetPos)
        {
            if (Mathf.Abs(targetPos.x - currentPos.x) >= Mathf.Abs(targetPos.y - currentPos.y) && !FindGroundAdjacent(currentPos, new Vector2((targetPos.x - currentPos.x) / Mathf.Abs(targetPos.x - currentPos.x), 0), targetMask, blockInterval))
            {
                direction = (targetPos.x - currentPos.x) / Mathf.Abs(targetPos.x - currentPos.x);

                currentPos.x += direction * blockInterval;
                path.Add(currentPos);
            }
            else if (!FindGroundAdjacent(currentPos, new Vector2(0, (targetPos.y - currentPos.y) / Mathf.Abs(targetPos.y - currentPos.y)), targetMask, blockInterval))
            {
                direction = (targetPos.y - currentPos.y) / Mathf.Abs(targetPos.y - currentPos.y);

                currentPos.y += direction * blockInterval;
                path.Add(currentPos);
            }
            else
            {
                //Code for when the character is unable to go the two directions it wants to
                timesPassedToBreak++;
                Debug.Log("Caught in an infinite loop here");
                if (timesPassedToBreak >= 6) break;
            }

        }

        return path;
    }

    //Returns true when pos1 is adjacent to pos2. 
    //To be adjacent, pos1 needs to be a blockInterval away horizontally or vertically but not both
    //Does not currently have any miss room. If the two positions given are not actually on the exact transform, then there will be a problem
    //There is a weird problem with y positions where the block interval can be .00000001 off from the actual block interval.
    public static bool isAdjacent(Vector2 pos1, Vector2 pos2, float blockInterval)
    {
        if ((Mathf.Abs(pos1.x - pos2.x) <= blockInterval + .01f && Mathf.Abs(pos1.x - pos2.x) >= blockInterval - .01f) ^ (Mathf.Abs(pos1.y - pos2.y) <= blockInterval + .01f && Mathf.Abs(pos1.y - pos2.y) >= blockInterval - .01f)) return true;
        //if (Mathf.Abs(pos1.x - pos2.x) == blockInterval ^ Mathf.Abs(pos1.y - pos2.y) == blockInterval) return true;

        //Debug.Log("Is not adjacent");

        return false;
    }

    //Returns the Vector2 that is adjacent to the given location. Will return the closest spot that is adjacent.
    public static Vector3 FindAdjacentPosition(Vector2 startPos, Vector2 pos2BAdjacent2,  float blockInterval)
    {
        Vector3 position = Vector2.zero;
        float distanceFrom = Mathf.Infinity; //Totals the x distance and y distance from startPos to determine which is closer.
        int index = -1;
        Vector3[] adjacentPositions = new Vector3[4];

        adjacentPositions[0] = new Vector3(pos2BAdjacent2.x + blockInterval, pos2BAdjacent2.y, 0);
        adjacentPositions[1] = new Vector3(pos2BAdjacent2.x, pos2BAdjacent2.y + blockInterval, 0);
        adjacentPositions[2] = new Vector3(pos2BAdjacent2.x - blockInterval, pos2BAdjacent2.y, 0);
        adjacentPositions[3] = new Vector3(pos2BAdjacent2.x, pos2BAdjacent2.y - blockInterval, 0);

        for (int i = 0; i < adjacentPositions.Length; i++)
        {
            float tempValue = Mathf.Abs(adjacentPositions[i].x - startPos.x) + Mathf.Abs(adjacentPositions[i].y - startPos.y);
            if (tempValue < distanceFrom)
            {
                distanceFrom = tempValue;
                index = i;
            }
        }

        if (index == -1) return Vector3.zero;
        position = adjacentPositions[index];
        return position;
    }

    //Returns true when the next tile in the given direction is a ground tile and a character would not be able to go to it
    //Returns false when the next tile is not a ground piece and the character can go to it. You get the layermask from the combat controller.
    //This method is called by PathToPosition which will also have the layermask.
    public static bool FindGroundAdjacent(Vector2 pos, Vector2 targetDirection, LayerMask targetMask, float blockInterval)
    {
        //This method does not currently work
        if (Physics.Raycast(pos, targetDirection, blockInterval, targetMask)) return true;


        return false;
    }
}
