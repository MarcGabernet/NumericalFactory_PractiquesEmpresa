using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CreaBloc : MonoBehaviour
{

    void DuplicaObjecte(int x, int y, int z)
    {
        float scaleX = transform.localScale.x;
        float scaleZ = transform.localScale.z;
        GameObject bloc = Instantiate(this.gameObject, transform.parent);
        bloc.transform.position = transform.position + new Vector3(x * scaleX, 0, z * scaleZ);
        
        string name = this.name;

        string[] splitString = name.Split('_');
        string baseNom = splitString[0];

        int[] coords = new int[3];
        for(int i = 0; i < 3; i++)
        {
            coords[i] = int.Parse(splitString[i+1]);
        }

        coords[0] += x;
        coords[1] += y;
        coords[2] += z;

        bloc.name = baseNom + "_" + coords[0].ToString() + "_" + splitString[1].ToString() + "_" + coords[2].ToString();
    }




#if UNITY_EDITOR

    [MenuItem("Tools/Crea Bloc/X+")]
    public static void CreaXPositiu()
    {
        CreaBloc creaBloc = Selection.activeGameObject.GetComponent<CreaBloc>();
        creaBloc.DuplicaObjecte(1,0,0);
    }
    [MenuItem("Tools/Crea Bloc/X-")]
    public static void CreaXNegatiu()
    {
        CreaBloc creaBloc = Selection.activeGameObject.GetComponent<CreaBloc>();
        creaBloc.DuplicaObjecte(-1, 0, 0);
    }
    [MenuItem("Tools/Crea Bloc/Z+")]
    public static void CreaZPositiu()
    {
        CreaBloc creaBloc = Selection.activeGameObject.GetComponent<CreaBloc>();
        creaBloc.DuplicaObjecte(0, 0, 1);
    }
    [MenuItem("Tools/Crea Bloc/Z-")]
    public static void CreaZNegatiu()
    {
        CreaBloc creaBloc = Selection.activeGameObject.GetComponent<CreaBloc>();
        creaBloc.DuplicaObjecte(0, 0, -1);
    }
#endif
}
