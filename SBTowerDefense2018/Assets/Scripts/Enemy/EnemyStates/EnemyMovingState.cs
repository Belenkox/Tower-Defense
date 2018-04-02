﻿using UnityEngine;

public class EnemyMovingState : IEnemyState
{
    private Path path;                      //Path to walk along
    private int targetWaypoint;
    private HexTile currentlyOn;            //Tile enemy is currently standing on
    private float moveProgress;             //0.0 -- 1.0

    public EnemyMovingState(Path path)
    {
        this.path = path;
        this.targetWaypoint = 1;
        this.currentlyOn = path[0];
        this.moveProgress = 0;
    }

    public void UpdateState(Enemy enemy)
    {
        //Update position
        moveProgress += enemy.Speed * Time.deltaTime;
        enemy.transform.position = Vector3.Lerp(path[targetWaypoint - 1].worldPos, path[targetWaypoint].worldPos, moveProgress);

        //Calculate when this enemy enters / leaves current tile
        if (moveProgress >= 0.5f && path[targetWaypoint] != currentlyOn)
        {
            currentlyOn.OnEnemyExit.Invoke(enemy);         //Exit last visited tile
            currentlyOn = path[targetWaypoint];
            currentlyOn.OnEnemyEnter.Invoke(enemy);        //Enter new tile

            //Finally attack the base / other tower
            if (currentlyOn == path.Destination)
                enemy.Attack(currentlyOn);
        }

        //Change to the next waypoint / destination
        if (moveProgress >= 1f)
        {
            moveProgress = 0f;
            targetWaypoint++;

            //TODO: this may be useful later
            //This doesn't get called, unless enemy is not supposed to be attacking at all.
            if (targetWaypoint > path.Waypoints.Count - 1)
                enemy.Idle();               //For now, just chill in that spot
        }
    }
}
