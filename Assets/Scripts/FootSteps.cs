using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{

    public enum FloorEnum {Concrete, Grass, Wood};

    public AudioClip[] concreteStepClips;
    public AudioClip[] grassStepClips;
    public AudioClip[] woodStepClips;
    public int terrainType = (int)FloorEnum.Concrete;
    public float timeBetweenSteps = 1f;
    public float timeVariance = 0f;
    public AudioSource source;

    private AudioClip[][] clipsPerTerrain;
    private float elapsedTime = 0f;
    private float originalTimeBetweenSteps;
    private UnityEngine.AI.NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        clipsPerTerrain = new AudioClip[][] {concreteStepClips, grassStepClips, woodStepClips};
        originalTimeBetweenSteps = timeBetweenSteps;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance < 0.01)
        {
            this.enabled = false;
            return;
        }
        elapsedTime += Time.deltaTime;
        if (elapsedTime > timeBetweenSteps)
        {
            source.clip = clipsPerTerrain[terrainType][Random.Range(0, clipsPerTerrain[terrainType].Length)];
            source.Play();
            elapsedTime = 0;
            timeBetweenSteps = Random.Range(originalTimeBetweenSteps - timeVariance, originalTimeBetweenSteps + timeVariance);
        }
    }

}
