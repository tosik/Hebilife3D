using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hebilife;

public class SnakeBehaviour : MonoBehaviour
{
    public Snake Snake { get; private set; }
    List<GameObject> _bodies;
    GameObject _bodyObject;

    public void Initialize(Snake snake)
    {
        Snake = snake;
        _bodies = new List<GameObject>();
        _bodyObject = transform.Find("Body").gameObject;
    }

    void Start()
    {
    }

    void Update()
    {
        if (Snake == null)
            return;

        foreach (var body in _bodies)
        {
            Destroy(body);
        }
        _bodies.Clear();

        foreach (var body in Snake.Bodies)
        {
            var obj = Instantiate(_bodyObject, gameObject.transform);
            obj.transform.position = new Vector3(body.X, 0, body.Y);
            obj.SetActive(true);
            var renderer = obj.GetComponent<MeshRenderer>();
            renderer.material.color = Color;

            _bodies.Add(obj);
        }

        if (Snake.Dead)
        {
            Destroy(gameObject);
        }
    }

    Color Color
    {
        get
        {
            return new Color(
                (float)Snake.Brain.NeuralNetwork.GetWeight(0, 0, 0) +
                (float)Snake.Brain.NeuralNetwork.GetWeight(0, 1, 0) +
                (float)Snake.Brain.NeuralNetwork.GetWeight(0, 2, 0) +
                (float)Snake.Brain.NeuralNetwork.GetWeight(0, 0, 1) +
                (float)Snake.Brain.NeuralNetwork.GetWeight(0, 1, 1) +
                (float)Snake.Brain.NeuralNetwork.GetWeight(0, 2, 1) +
                (float)Snake.Brain.NeuralNetwork.GetWeight(0, 0, 2) +
                (float)Snake.Brain.NeuralNetwork.GetWeight(0, 1, 2) +
                (float)Snake.Brain.NeuralNetwork.GetWeight(0, 2, 2) % 1,
                (float)Snake.Brain.NeuralNetwork.GetWeight(1, 0, 0) +
                (float)Snake.Brain.NeuralNetwork.GetWeight(1, 1, 0) +
                (float)Snake.Brain.NeuralNetwork.GetWeight(1, 2, 0) +
                (float)Snake.Brain.NeuralNetwork.GetWeight(1, 0, 1) +
                (float)Snake.Brain.NeuralNetwork.GetWeight(1, 1, 1) +
                (float)Snake.Brain.NeuralNetwork.GetWeight(1, 2, 1) +
                (float)Snake.Brain.NeuralNetwork.GetWeight(1, 0, 2) +
                (float)Snake.Brain.NeuralNetwork.GetWeight(1, 1, 2) +
                (float)Snake.Brain.NeuralNetwork.GetWeight(1, 2, 2) % 1,
                (float)Snake.Brain.NeuralNetwork.GetWeight(2, 0, 0) +
                (float)Snake.Brain.NeuralNetwork.GetWeight(2, 1, 0) +
                (float)Snake.Brain.NeuralNetwork.GetWeight(2, 2, 0) +
                (float)Snake.Brain.NeuralNetwork.GetWeight(2, 0, 1) +
                (float)Snake.Brain.NeuralNetwork.GetWeight(2, 1, 1) +
                (float)Snake.Brain.NeuralNetwork.GetWeight(2, 2, 1) +
                (float)Snake.Brain.NeuralNetwork.GetWeight(2, 0, 2) +
                (float)Snake.Brain.NeuralNetwork.GetWeight(2, 1, 2) +
                (float)Snake.Brain.NeuralNetwork.GetWeight(2, 2, 2) % 1);
        }
    }
}
