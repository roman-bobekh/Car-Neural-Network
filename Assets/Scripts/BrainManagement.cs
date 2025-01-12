using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainManagement : MonoBehaviour
{
    NeuroEvolution evolution;
    int population = 50;
    int generation = 0;

    CarMovement[] cars;

    public GameObject carObject = null;
    public GameObject trophy = null;
    GameObject bestCar;

    float startTime;

    // Start is called before the first frame update
    void Start()
    {
        // create neuro evolution with properties
        evolution = new NeuroEvolution(8, 4, 2, 4, population, 1, 0.4, 0.4, "tanh");
        cars = new CarMovement[population];

        for (int i = 0; i < population; i++)
        {
            GameObject car = Instantiate(carObject, transform);
            cars[i] = car.GetComponent<CarMovement>();
        }

        ResetCars();

        Time.timeScale = 1;
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        float max = -1;

        foreach (CarMovement car in cars)
        {
            // Make the car move
            car.Control();

            // Save score if score is better than current score
            car.UpdateScore();
            if (car.score > max)
            {
                max = car.score;
                bestCar = car.gameObject;
            }
        }

        if (population > 0)
        {
            trophy.transform.SetParent(bestCar.transform);
            trophy.transform.localPosition = Vector3.zero;
        }

        if (Time.time - startTime > 10)
        {
            Evolve();
            ResetCars();
        }
    }

    void Evolve()
    {
        double[] scores = new double[population];
        float max = -1;
        for (int i = 0; i < population; i++)
        {
            scores[i] = cars[i].score;
            if (cars[i].score > max)
            {
                max = cars[i].score;
            }
        }
        print("Score: " + max);
        print("Generation: " + generation);

        evolution.Evolve(scores);
        generation++;
    }

    void ResetCars()
    {
        for (int i = 0; i < population; i++)
        {
            cars[i].ResetCar();
            cars[i].SetBrain(evolution.brains[i]);
        }

        startTime = Time.time;
    }
}
