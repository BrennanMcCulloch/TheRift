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
    public UnityEngine.AI.NavMeshAgent agent;

    private AudioClip[][] clipsPerTerrain;
    private List<AudioClip>[] possibleClips;
    private float elapsedTime = 0f;
    private float originalTimeBetweenSteps;
    private int newTerrainType = (int)FloorEnum.Concrete;

    // Start is called before the first frame update
    void Start()
    {
        clipsPerTerrain = new AudioClip[][] {concreteStepClips, grassStepClips, woodStepClips};
        possibleClips = new List<AudioClip>[] {new List<AudioClip>(concreteStepClips),
                                               new List<AudioClip>(grassStepClips),
                                               new List<AudioClip>(woodStepClips)
                                              };
        originalTimeBetweenSteps = timeBetweenSteps;
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
            if (possibleClips[terrainType].Count == 0)
            {
                possibleClips[terrainType] = new List<AudioClip>(clipsPerTerrain[terrainType]);
            }
            int clipIndex = Random.Range(0, possibleClips[terrainType].Count);
            source.clip = possibleClips[terrainType][clipIndex];
            possibleClips[terrainType].RemoveAt(clipIndex);
            source.Play();
            elapsedTime = 0;
            timeBetweenSteps = Random.Range(originalTimeBetweenSteps - timeVariance, originalTimeBetweenSteps + timeVariance);
        }
    }

    // Queue the new terrain type for when we exit our current one
    void OnTriggerEnter(Collider other)
    {
        string[] commaSplitStrings = other.gameObject.tag.Split(',');
        if (commaSplitStrings[0] == "Floor")
        {
            newTerrainType = (int)FloorEnum.Parse(typeof(FloorEnum), commaSplitStrings[1]);
        }
    }

    // Change our terrain type only after leaving our old one
    void OnTriggerExit(Collider other)
    {
        string[] commaSplitStrings = other.gameObject.tag.Split(',');
        if (commaSplitStrings[0] == "Floor")
        {
            if (terrainType == (int)FloorEnum.Parse(typeof(FloorEnum), commaSplitStrings[1]))
            {
                terrainType = newTerrainType;
            }
        }
    }

}
