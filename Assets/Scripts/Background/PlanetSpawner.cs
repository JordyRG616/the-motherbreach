using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> planets;
    [SerializeField] private float distanceToSpawn;
    [SerializeField] private Vector2 scaleMinMax;
    [SerializeField] private float initialForce;
    private Camera cam;
    private int lastAngle = 0;
    private Dictionary<GameObject, IEnumerator> activeRoutines =  new Dictionary<GameObject, IEnumerator>();

    public void Initialize()
    {
        cam = GameObject.FindWithTag("SecCamera").GetComponent<Camera>();
    }

    public void SpawnNewPlanet()
    {
        var rdm = Random.Range(0, planets.Count);
        var planet = Instantiate(planets[rdm], GeneratePosition(), GenerateRotation());
        SetScale(planet);

        var routine = MovePlanet(planet);
        activeRoutines.Add(planet, routine);
        StartCoroutine(routine);
    }

    private Quaternion GenerateRotation()
    {
        var rdm = Random.Range(lastAngle, lastAngle + 90);
        lastAngle += rdm;
        var quaternion = Quaternion.Euler(0, 0, rdm);
        return quaternion;
    }

    private Vector3 GeneratePosition()
    {
        var rdm = Random.Range(0, 2 * Mathf.PI);
        var vector = new Vector3(Mathf.Cos(rdm), Mathf.Sin(rdm)) * distanceToSpawn;
        vector += cam.transform.position;
        vector.z = 0;
        return vector;
    }

    private void SetScale(GameObject planet)
    {
        var scale = planet.transform.localScale;
        var rdm = Random.Range(scaleMinMax.x, scaleMinMax.y);
        scale.x += rdm;
        scale.y += rdm;
        planet.transform.localScale = scale;
    }

    private IEnumerator MovePlanet(GameObject planet)
    {
        var body = planet.GetComponent<Rigidbody2D>();

        while(true)
        {
            if(body == null) yield break;
            body.velocity = Vector2.zero;
            var direction = cam.transform.position - planet.transform.position;
            body.AddForce(Vector2.Perpendicular(direction).normalized * initialForce, ForceMode2D.Impulse);

            yield return new WaitForEndOfFrame();
        }
    }
}
