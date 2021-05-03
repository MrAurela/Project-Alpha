using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] GameObject playerHealthObject;

    // Update is called once per frame
    void Update()
    {
        GetComponent<Slider>().value = playerHealthObject.GetComponent<Damageable>().GetHealthPercentage();
    }
}
