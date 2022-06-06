using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpawner : MonoBehaviour
{
    [SerializeField] private GameObject star;
    [SerializeField] private float starDistance;
    [SerializeField] private List<GameObject> planets;
    [SerializeField] private Vector2Int planetCount;
    [SerializeField] private Vector2 initialDistanceFromStar;
    [SerializeField] private Vector2 distanceFromStarIncrement;
    private Queue<GameObject> planetQueue = new Queue<GameObject>();
    private GameObject createdStar;

    void Start()
    {
        GeneratePlanetQueue();
        CreateStarSystem();
    }

    private void GeneratePlanetQueue()
    {
        var _list = new List<GameObject>(planets);

        while (_list.Count > 0)
        {
            var rdm = Random.Range(0, _list.Count);
            planetQueue.Enqueue(_list[rdm]);
            _list.RemoveAt(rdm);
        }
    }

    private void CreateStarSystem()
    {
        var angle = Random.Range(0, 360f);
        createdStar = Instantiate(star, RadialPosition(starDistance), Quaternion.Euler(0, 0, angle));

        var _count = Random.Range(planetCount.x, planetCount.y + 1);
        var distance = Random.Range(initialDistanceFromStar.x, initialDistanceFromStar.y);

        for (int i = 0; i < _count; i++)
        {
            CreatePlanet(distance);
            distance += Random.Range(distanceFromStarIncrement.x, distanceFromStarIncrement.y);
        }

    }

    private Vector2 RadialPosition(float radius)
    {
        var angle = Random.Range(0, 360f) * Mathf.Deg2Rad;
        var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        return direction * radius;
    }

    private void CreatePlanet(float distanceFromStar)
    {
        if(planetQueue.Count == 0) GeneratePlanetQueue();
        var angle = Random.Range(0, 360f);
        Instantiate(planetQueue.Dequeue(), (Vector2)createdStar.transform.position + RadialPosition(distanceFromStar), Quaternion.Euler(0, 0, angle));
    }
}
