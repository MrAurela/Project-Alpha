using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpirit : MonoBehaviour, IDamageable
{
    public void Die()
    {
        SceneManager.LoadScene("Lose");
    }
}
