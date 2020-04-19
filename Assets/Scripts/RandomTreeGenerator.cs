using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTreeGenerator : MonoBehaviour
{
    public int numberOfObjectsToGenerate;
    //BOX COLLIDER SPACE
    private float minFieldX;
    private float maxFieldX;
    private float minFieldZ;
    private float maxFieldZ;

    public float minXDistanceFromOtherObject;
    public float minZDistanceFromOtherObject;

    public float minSize = 5f;
    public float maxSize = 10f;

    private int maxNumberRetries = 20;

    public List<GameObject> instantiatedObjects;
    private List<Vector3> alreadyInstantiatedPositions;

    [SerializeField]
    private BoxCollider generationField;

    private GameObject[] treePrefabs;

    [SerializeField]
    private string treeFolderName;

    private void Awake()
    {
        this.instantiatedObjects = new List<GameObject>();
        this.alreadyInstantiatedPositions = new List<Vector3>();
        this.minFieldX = generationField.transform.position.x - (generationField.size.x / 2);
        this.maxFieldX = generationField.transform.position.x + (generationField.size.x / 2);
        this.minFieldZ = generationField.transform.position.z - (generationField.size.z / 2);
        this.maxFieldZ = generationField.transform.position.z + (generationField.size.z / 2);

        this.treePrefabs = Resources.LoadAll<GameObject>(this.treeFolderName);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            this.Reroll();
        }
    }

    public void GenerateRandomObjects()
    {
        for (int num = 0; num < this.numberOfObjectsToGenerate; num++)
        {
            float xValue = Random.Range(this.minFieldX, this.maxFieldX);
            float zValue = Random.Range(this.minFieldZ, this.maxFieldZ);

            for (int i = 0; i < this.maxNumberRetries; i++)
            {
                bool collision = false;

                for (int j = 0; j < this.alreadyInstantiatedPositions.Count; j++)
                {
                    if (Mathf.Abs(this.alreadyInstantiatedPositions[j].x - xValue) <= this.minXDistanceFromOtherObject &&
                        Mathf.Abs(this.alreadyInstantiatedPositions[j].z - zValue) <= this.minZDistanceFromOtherObject)
                    {
                        collision = true;
                        break;
                    }
                }

                if (collision == true)
                {
                    xValue = Random.Range(this.minFieldX, this.maxFieldX);
                    zValue = Random.Range(this.minFieldZ, this.maxFieldZ);
                }
                else
                {
                    Vector3 worldSpaceGenerationPosition = new Vector3(xValue, 0.0f, zValue);
                    GameObject objectToGenerate = this.treePrefabs[Random.Range(0, this.treePrefabs.Length)];
                    GameObject objectInstance = Instantiate(objectToGenerate, worldSpaceGenerationPosition, new Quaternion(), this.transform) as GameObject;
                    objectInstance.transform.localScale *= Random.Range(this.minSize, this.maxSize);

                    this.instantiatedObjects.Add(objectInstance);
                    this.alreadyInstantiatedPositions.Add(new Vector3(xValue, 0.0f, zValue));
                    break;
                }
            }
        }
    }

    private void Reroll()
    {
        this.ClearObjects();
        this.GenerateRandomObjects();
    }

    public void ClearObjects()
    {
        foreach (GameObject thisObject in this.instantiatedObjects)
        {
            Destroy(thisObject);
        }

        this.instantiatedObjects = new List<GameObject>();
        this.alreadyInstantiatedPositions = new List<Vector3>();
    }
}