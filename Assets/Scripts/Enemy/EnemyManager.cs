using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class EnemyManager
{
    private static int killedEnemies = 0;

    public static int KilledEnemies
    {
        get { return killedEnemies; }
    }

    public static void EnemyKilled()
    {
        killedEnemies++;
    }
}
