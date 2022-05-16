using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JailShipManager : MonoBehaviour
{
    [SerializeField] private List<Transform> slots;
    [SerializeField] private List<GameObject> arsenal;
    [SerializeField] private float distanceCap;
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float rotationSpeedDeviation;
    private Vector3 distance;
    private List<GameObject> activeArsenal = new List<GameObject>();
    private int indexToUnlock;
    private Transform ship;

    private float distanceMagnitude
    {
        get
        {
            if(distance.magnitude <= 0) return .1f;
            else if(distance.magnitude >= distanceCap) return distanceCap;
            else return distance.magnitude;

        }
    }

    void Start()
    {
        foreach(Transform slot in slots)
        {
            var rdm = Random.Range(0, arsenal.Count);
            var weapon = Instantiate(arsenal[rdm], slot.position, Quaternion.identity, slot.transform);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localEulerAngles = Vector3.zero;
            weapon.GetComponent<ActionEffect>().Initiate();
            weapon.GetComponent<EnemyHealthController>().OnDeath += RemoveWeapon;
            activeArsenal.Add(weapon);
        }

        WaveManager.Main.OnWaveEnd += DestroyJail;

        ship = ShipManager.Main.transform;

        indexToUnlock = GameManager.Main.GetPilotIndexToUnlock();
    }

    private void RemoveWeapon(object sender, EnemyEventArgs e)
    {
        activeArsenal.Remove(e.healthController.gameObject);
        e.healthController.OnDeath -= RemoveWeapon;
        if(activeArsenal.Count == 0) DestroyJail();
    }

    private void DestroyJail()
    {
        GameManager.Main.UnlockPilot(indexToUnlock);
        Destroy(this.gameObject);
    }

    private void DestroyJail(object sender, System.EventArgs e)
    {
        WaveManager.Main.OnWaveEnd -= DestroyJail;
        Destroy(this.gameObject);
    }

    void FixedUpdate()
    {
        // MoveAway();
        Rotate();
    }

    private void MoveAway()
    {
        distance = transform.position - ship.position;

        transform.position += distance.normalized * speed / distanceMagnitude;
    }

    private void Rotate()
    {
        var rotation = transform.eulerAngles.z;
        var rdm = Random.Range(-rotationSpeedDeviation, rotationSpeedDeviation);
        rotation += (rotationSpeed + rdm) * Time.fixedDeltaTime;
        transform.eulerAngles = new Vector3(0, 0, rotation);
    }
}
