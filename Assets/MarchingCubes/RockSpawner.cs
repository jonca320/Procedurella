using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    [Header("Instantiation Settings")]
    [SerializeField]
    private float distanceBetweenRocks = 50f;
    [SerializeField]
    [Range(0, 1)]
    private float distanceNoise = 0.5f;

    [Header("Rock Settings")]
    [SerializeField]
    private GameObject RockPrefab;

    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private int averageRockHeight = 20;
    [SerializeField]
    private int averageRockWidth = 20;
    [SerializeField]
    private float averageRockHoleIntensity = 0.3f;

    private int height;
    private int width;
    private int falloff;
    private float heightThreshold;
    private float holeIntensity;
    private int noiseScale;

    private List<GameObject> allRocks;

    [Header("Material Settings")]
    [SerializeField]
    private Color topColor;
    [SerializeField]
    private Color midColor;
    [SerializeField]
    private Color botColor;

    private Material mat;
    private void Start()
    {
        allRocks = new List<GameObject>();
    }

    public List<GameObject> SpawnRocks(Vector3 pos)
    {
        //RemoveAllRocks();
        MakeMaterial();
        List<GameObject> rocks = new List<GameObject>();

        for (float x = pos.x; x < pos.x + 2; x++)
        {
            for (float z = pos.z; z < pos.z + 2; z++)
            {
                MakeRandomRockValues();

                float xOffset = x * distanceBetweenRocks + Random.Range(-1.0f, 1.0f) * distanceBetweenRocks * distanceNoise;
                float zOffset = z * distanceBetweenRocks + Random.Range(-1.0f, 1.0f) * distanceBetweenRocks * distanceNoise;


                xOffset = x + distanceBetweenRocks* Random.Range(-1.0f, 1.0f);
                zOffset = z + distanceBetweenRocks* Random.Range(-1.0f, 1.0f);


                rocks.Add(SpawnRock(new Vector3(xOffset, 0, zOffset)));
            }
        }
        return rocks;
    }

    void MakeMaterial()
    {
        Material baseMat = RockPrefab.GetComponent<MeshRenderer>().sharedMaterial;
        mat = new Material(baseMat);
        mat.SetColor("_TopColor", topColor);
        mat.SetColor("_MidColor", midColor);
        mat.SetColor("_BottomColor", botColor);
    }
    void MakeRandomRockValues()
    {
        height = averageRockHeight + Mathf.RoundToInt((((float)averageRockHeight / 2f) * (Random.Range(-1.0f, 1.0f))));
        width = averageRockWidth + Mathf.RoundToInt((((float)averageRockWidth / 2f) * (Random.Range(-1.0f, 1.0f))));
        falloff = Random.Range(2, 5);
        heightThreshold = 0.005f + (((4.0f - falloff) * 5f) / 250);
        holeIntensity = averageRockHoleIntensity + ((((float)averageRockHoleIntensity / 1.8f) * (Random.Range(-1.0f, 1.0f))));
        noiseScale = Random.Range(4, 12);
    }

    public GameObject SpawnRock(Vector3 position)
    {
        MakeRandomRockValues();
        MakeMaterial();
        GameObject rock = Instantiate(RockPrefab, position, new Quaternion(0, 0, 0, 0));
        allRocks.Add(rock);
        MarchingCubes mc = rock.GetComponent<MarchingCubes>();
        rock.GetComponent<MeshRenderer>().material = mat;
        mc.AssignValues(width, height, falloff, holeIntensity, heightThreshold, noiseScale);
        mc.Generate();
        return rock;
    }

    public void UpdateLOD( List<GameObject> rocks)
    {
        foreach (GameObject g in rocks)
        {
            if (Vector2.Distance(new Vector2(playerTransform.position.x, playerTransform.position.z) , new Vector2(g.transform.position.x, g.transform.position.z)) < 200*3)
            {
                g.GetComponent<MarchingCubes>().SetMesh(2);
            }
            else if (Vector2.Distance(new Vector2(playerTransform.position.x, playerTransform.position.z), new Vector2(g.transform.position.x, g.transform.position.z)) < 400*3)
            {
                g.GetComponent<MarchingCubes>().SetMesh(1);

            }
            else
            {
                g.GetComponent<MarchingCubes>().SetMesh(0);


            }

        }

    }
    public void SetLOD(GameObject rock, float LODLevel)
    {



        rock.GetComponent<MarchingCubes>().width = Mathf.RoundToInt(10 * LODLevel);
        rock.GetComponent<MarchingCubes>().height = Mathf.RoundToInt(10 * LODLevel);
        rock.transform.localScale = Vector3.one*10 * (1/ LODLevel);
        rock.GetComponent<MarchingCubes>().Generate();
      
    }

    //public void DecreaseLOD()
    //{
    //    foreach (GameObject g in allRocks)
    //    {
    //        if (g.transform.localScale.x >= 4.0f) return;
    //        g.GetComponent<MarchingCubes>().width = Mathf.RoundToInt(g.GetComponent<MarchingCubes>().width * 0.5f);
    //        g.GetComponent<MarchingCubes>().height = Mathf.RoundToInt(g.GetComponent<MarchingCubes>().height * 0.5f);
    //        g.transform.localScale = g.transform.localScale * 2;
    //        g.GetComponent<MarchingCubes>().Generate();
    //    }
    //}
    public void RemoveAllRocks()
    {
        foreach (GameObject g in allRocks)
        {
            Destroy(g);
        }
    }
}
