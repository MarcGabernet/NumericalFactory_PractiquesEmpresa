using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class HexagonCreation : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject startingHexagon;

    [Header("Offsets")]
    public float offsetN_z = 7.619304f;
    public float offsetNE_z = 3.809652f;
    public float offsetNE_x = 6;
    public float offsetSE_z = -3.809652f;
    public float offsetSE_x = 6;
    public float offsetS_z = -7.619304f;
    public float offsetSW_z = -3.809652f;
    public float offsetSW_x = -6;
    public float offsetNW_z = 3.809652f;
    public float offsetNW_x = -6;

    public bool rainbow = false;

    private Vector3 offsetN;
    private Vector3 offsetNE;
    private Vector3 offsetSE;
    private Vector3 offsetS;
    private Vector3 offsetSW;
    private Vector3 offsetNW;

    private Vector3[] angle;
    private Vector3 scale;

    private ArrayList alreadyCreated;

    void Start()
    {
        scale = startingHexagon.transform.localScale;
        float scalex = startingHexagon.transform.localScale.x;

        angle = new Vector3[6];

        offsetN = new Vector3(0, 0, offsetN_z);
        angle[0] = offsetN;
        offsetNE = new Vector3(offsetNE_x, 0, offsetNE_z);
        angle[1] = offsetNE;
        offsetSE = new Vector3(offsetSE_x, 0, offsetSE_z);
        angle[2] = offsetSE;
        offsetS = new Vector3(0, 0, offsetS_z);
        angle[3] = offsetS;
        offsetSW = new Vector3(offsetSW_x, 0, offsetSW_z);
        angle[4] = offsetSW;
        offsetNW = new Vector3(offsetNW_x, 0, offsetNW_z);
        angle[5] = offsetNW;

        for(int i = 0; i < 6; i++)
        {
            angle[i] = angle[i] * scalex;
        }

        if (rainbow)
        {
            MeshRenderer ogMesh = startingHexagon.GetComponentInChildren<MeshRenderer>();
            ogMesh.material = NewMaterial(startingHexagon.transform.position);
        }

        alreadyCreated = new ArrayList();
        alreadyCreated.Add(startingHexagon.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject hexagon = other.gameObject.transform.parent.gameObject;

        if (hexagon.CompareTag("Hexagon"))
        {

            Vector3 position = hexagon.transform.parent.position;

            string angleTag;
            
            string originalName = hexagon.transform.parent.name;
            alreadyCreated.Add(originalName);

            string newName = "Tile_";
            string[] nameParts = originalName.Substring(5).Split('_');
            int x = int.Parse(nameParts[0]);
            int z = int.Parse(nameParts[1]);

            bool columnaParella;
            if (x%2 == 0)
            {
                columnaParella = true;
            }
            else
            {
                columnaParella = false;
            }


            for (int i = 0; i < 6; i++)
            {
                angleTag = "Barrier " + (i * 60).ToString();
                if (other.gameObject.CompareTag(angleTag))
                {

                    if (i == 0)
                    {
                        z++;
                    }
                    else if (i == 1)
                    {
                        x++;
                        if (!columnaParella)
                        {
                            z++;
                        }
                    }
                    else if(i == 2)
                    {
                        x++;
                        if (columnaParella)
                        {
                            z--;
                        }
                    }
                    else if(i==3)
                    {
                        z--;
                    }
                    else if(i == 4)
                    {
                        x--;
                        if (columnaParella)
                        {
                            z--;
                        }
                    }
                    else if(i == 5)
                    {
                        x--;
                        if (!columnaParella)
                        {
                            z++;
                        }
                    }
                    newName = newName + x.ToString() + "_" + z.ToString();
                    Debug.Log(newName);
                    other.gameObject.SetActive(false);

                    if (alreadyCreated.Contains(newName))
                    {
                        break;
                    }
                    else
                    {
                        alreadyCreated.Add(newName);
                    }

                    GameObject newTile = Instantiate(tilePrefab);

                    newTile.transform.localScale = scale;
                    newTile.transform.position = position + angle[i];
                    newTile.name = newName;

                    GameObject childObject = newTile.gameObject.transform.GetChild(0).gameObject;
                    Debug.Log(childObject.name);

                    MeshRenderer meshRenderer = childObject.GetComponentInChildren<MeshRenderer>();

                    if (rainbow)
                    {
                        meshRenderer.material = NewMaterial(newTile.transform.position);
                    }
                }
            }
        }
    }

    public Material NewMaterial(Vector3 position)
    {
        Material material = new Material(Shader.Find("Universal Render Pipeline/Lit"));

        float smoothnessValue = 0.7f;
        material.SetFloat("_Smoothness", smoothnessValue);

        float metallicValue = 0.3f; 
        material.SetFloat("_Metallic", metallicValue);

        Color color = new Color((Mathf.Sin(10.1f*position.x)*127f+128f)/ 255f,
                                (Mathf.Sin(10.1f * position.z) * 127f + 128f) /255f,
                                (Mathf.Sin(10.1f * (position.x + position.z)) * 127f + 128f) / 255f, 1f);
        material.SetColor("_BaseColor", color);

        return material;
    }


}
