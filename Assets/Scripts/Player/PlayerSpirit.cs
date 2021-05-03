using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpirit : MonoBehaviour, IDamageable
{
    public void Die()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
