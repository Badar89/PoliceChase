using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftAtRuntime : MonoBehaviour
{
    [SerializeField] private DayNight dayNight;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (dayNight)
            {
                dayNight.isNight = !dayNight.isNight;
                dayNight.ChangeMaterial();

            }
        }

    }

}
