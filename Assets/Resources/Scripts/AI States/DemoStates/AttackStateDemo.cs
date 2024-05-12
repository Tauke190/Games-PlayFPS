using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackStateDemo: IEnemyStateDemo
{
    enemyAIDemo myEnemy;
    float actualTimeBetweenShots = 0;


    // When we call the constructor, we save
    // a reference to our enemy's AI
    public AttackStateDemo(enemyAIDemo enemy)
    {
        myEnemy = enemy;
    }

    // Here goes all the functionality that we want
    // what the enemy does when he is in this
    // state.
    public void UpdateState()
    {
        myEnemy.myLight.color = Color.red;
        actualTimeBetweenShots += Time.deltaTime;
    }

    //If the Player has hit us we do nothing
    public void Impact() { }


    //We are in this state so we never call it
    public void GoToAttackState() { }

    public void GoToPatrolState() { }

    public void GoToAlertState()
    {
        myEnemy.currentState = myEnemy.alertState;
    }

    //The player is already in our trigger
    public void OnTriggerEnter(Collider col) { }

    //We rotate the enemy to look at the Player while attacking him/her
    public void OnTriggerStay(Collider col)
    {
        //We always look at the player
        Vector3 lookDirection = col.transform.position - myEnemy.transform.position;

        //We rotate around the Y axis
        myEnemy.transform.rotation = Quaternion.FromToRotation(Vector3.forward, new Vector3(lookDirection.x, 0, lookDirection.z));

        //Turn to shoot
        if (actualTimeBetweenShots > myEnemy.timeBetweenShots)
        {
            actualTimeBetweenShots = 0;
            col.gameObject.GetComponent<Hero>().Hit(myEnemy.damageForce);
            myEnemy.audioSource.PlayOneShot(myEnemy.laser_gun);
        }
    }

    public void OnTriggerExit(Collider col)
    {
        GoToAlertState();
    }

}