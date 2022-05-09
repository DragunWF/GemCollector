using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const float baseSpeed = 1.5f;
    const float baseSteerSpeed = 175f;

    const float maxAcceleration = 10f;
    float acceleration = 0f;
    bool carReversing;

    const float maxSteerAcceleration = 50f;
    float steerAcceleration = 0f;
    bool steeringLeft;

    public bool OnRoad { get; private set; }
    public bool OnIntersection { get; private set; }

    public void ChangeRoadStatus(string roadType, bool state)
    {
        switch (roadType)
        {
            case "Road":
                OnRoad = state;
                break;
            case "Intersection":
                OnIntersection = state;
                break;
        }
    }

    void Update()
    {
        CarMovement();
        CarSteering();
    }

    void CarMovement()
    {
        var bonusSpeed = CheckForBonusSpeed();

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
        {
            carReversing = Input.GetKey(KeyCode.DownArrow);

            acceleration += 0.15f;
            if (acceleration >= maxAcceleration)
                acceleration = maxAcceleration;

            var speed = Input.GetAxis("Vertical") * Time.deltaTime * (baseSpeed + acceleration + bonusSpeed);
            transform.Translate(0, speed, 0);
        }
        else
        {
            acceleration -= 0.15f;
            if (acceleration <= 0)
                acceleration = 0;

            var force = carReversing ? -acceleration : acceleration;
            var friction = Time.deltaTime * force;
            transform.Translate(0, friction, 0);
        }
    }

    void CarSteering()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            steeringLeft = Input.GetKey(KeyCode.LeftArrow);

            steerAcceleration += 5f;
            if (steerAcceleration >= maxSteerAcceleration)
                steerAcceleration = maxSteerAcceleration;

            var rotateSpeed = Input.GetAxis("Horizontal") * Time.deltaTime * (baseSteerSpeed + steerAcceleration);
            transform.Rotate(0, 0, rotateSpeed);
        }
        else
        {
            steerAcceleration -= 1f;
            if (steerAcceleration <= 0)
                steerAcceleration = 0;

            var force = steeringLeft ? -steerAcceleration : steerAcceleration;
            var friction = Time.deltaTime * force;
            transform.Rotate(0, 0, friction);
        }
    }

    float CheckForBonusSpeed()
    {
        float bonusSpeed = 0f;

        if (OnRoad)
            bonusSpeed += 1.5f;
        if (OnIntersection)
            bonusSpeed += 1.25f;

        return bonusSpeed;
    }
}
