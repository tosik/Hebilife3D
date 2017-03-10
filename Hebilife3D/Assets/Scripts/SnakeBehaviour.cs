using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hebilife;

public class SnakeBehaviour : MonoBehaviour
{
    [TextArea]
    public string Gene;

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

        Gene = Snake.Brain.NeuralNetwork.GeneAsString;
    }

    Color Color
    {
        get
        {
            var gene = Snake.Brain.NeuralNetwork.Gene;
            return new Color(
                (gene & 0xfffff) / (float)(2 << 20),
                ((gene >> 20) & 0xfffff) / (float)(2 << 20),
                ((gene >> 40) & 0xffffff) / (float)(2 << 24)
            );
        }
    }
}
