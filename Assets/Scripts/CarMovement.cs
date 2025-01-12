using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    new Rigidbody2D rigidbody;
    
    NeuralNetwork brain;

    int laps;
    int linesCrossed;
    int totalLines = 9;

    public float score;

    // Reset the car's position and score
    public void ResetCar()
    {
        score = 0;
        ResetPosition();
    }

    // Reset the car's position
    void ResetPosition()
    {
        transform.position = new Vector3(-3, 2.3f);
        transform.rotation = Quaternion.Euler(0, 0, -24);

        laps = 0;
        linesCrossed = 0;
    }

    // Set the neural network of the car
    public void SetBrain(NeuralNetwork brain)
    {
        this.brain = brain;
    }

    // calculate and update score
    public void UpdateScore()
    {
        float newScore = laps + (float)linesCrossed / totalLines;
        if (newScore > score)
        {
            score = newScore;
        }
    }

    public void Control()
    {
        if (rigidbody == null)
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        int layerMask = LayerMask.GetMask("Map");
        Vector2[] dirs = {new Vector2(-1, -1), new Vector2(0, -1), new Vector2(1, -1),
                          new Vector2(-1, 0), new Vector2(0, 0), new Vector2(1, 0),
                          new Vector2(-1, 1), new Vector2(0, 1), new Vector2(1, 1)};

        double[] input = new double[8];
        for (int i = 0; i < 8; i++)
        {
            input[i] = Physics2D.Raycast(transform.position, dirs[i], float.MaxValue, layerMask).distance;
        }

        double[] output = brain.Guess(input);
        Turn((float) output[0]);
        Move((float) output[1]);
    }

    // turn the car by rotation
    void Turn(float rotationChange)
    {
        rigidbody.rotation += -rotationChange * 120 * Time.deltaTime;
    }

    // move the car by movement
    void Move(float movement)
    {
        rigidbody.velocity += RotateVector(Vector2.right, rigidbody.rotation) * movement * 60 * Time.deltaTime;
    }

    // rotate 2d vector by angle in degrees
    Vector2 RotateVector(Vector2 vector, float angle)
    {
        float direction = Mathf.Atan2(vector.y, vector.x) + Mathf.Deg2Rad * angle;
        return new Vector2(Mathf.Cos(direction) * vector.magnitude, Mathf.Sin(direction) * vector.magnitude);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Finish" && linesCrossed == totalLines)
        {
            laps++;
            linesCrossed = 0;
        }

        if (collider.tag == "Line")
        {
            Line lineObject = collider.gameObject.GetComponent<Line>();
            if (lineObject.number <= linesCrossed)
            {
                linesCrossed = lineObject.number + 1;
            }
        }
        
        if (collider.tag == "Map")
        {
            ResetPosition();
        }
    }
}
