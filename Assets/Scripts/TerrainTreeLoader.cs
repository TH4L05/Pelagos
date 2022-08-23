/// <author>Thoams Krahl</author>

using System.Collections.Generic;
using UnityEngine;

namespace PelagosProject
{
    public class TerrainTreeLoader : MonoBehaviour
    {
        #region Fields

        [SerializeField] private List<Terrain> terrainList = new List<Terrain>();
        [SerializeField] private GameObject terrainRootObject;

        #endregion

        #region UnityFunctions

        private void Awake()
        {
            if (terrainList.Count == 0) return;
            foreach (var terrain in terrainList)
            {
                InstantiateTrees(terrain);
            }
        }

        #endregion

        private void InstantiateTrees(Terrain terrain)
        {
            //terrain.drawTreesAndFoliage = false;
            terrain.treeDistance = 0f;

            TreeInstance[] treeInstances = terrain.terrainData.treeInstances;
            Vector3 terrainSize = terrain.terrainData.size;
            Vector3 terrainposition = terrain.transform.position;

            for (int i = 0; i < treeInstances.Length; i++)
            {
                TreeInstance treeInstance = treeInstances[i];
                Vector3 treePosition = treeInstance.position;
                Vector3 treeWorldPostion = Vector3.Scale(terrainSize, treePosition) + terrainposition;

                float height = treeInstance.heightScale;
                float width = treeInstance.widthScale;
                float rotation = treeInstance.rotation;
                int prototypeIndex = treeInstance.prototypeIndex;
                GameObject terrainTreePrefab = terrain.terrainData.treePrototypes[prototypeIndex].prefab;

                GameObject terrainTree = Instantiate(terrainTreePrefab, treeWorldPostion, Quaternion.Euler(0f, terrainTreePrefab.transform.rotation.y + rotation, 0f));
                terrainTree.transform.localScale = new Vector3(terrainTree.transform.localScale.x * width, terrainTree.transform.localScale.y * height, terrainTree.transform.localScale.z * width);

                if (terrainRootObject == null) continue;
                terrainTree.transform.parent = terrainRootObject.transform;
            }
        }
    }
}

