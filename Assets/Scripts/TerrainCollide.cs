using PelagosProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainCollide : MonoBehaviour
{
       //  SOURCE : Loren Logic
       //  http://answers.unity3d.com/questions/282910/tree-collision.html#answer-406989

        /*
        Tree Collision?

        This is a frequently asked question. The most popular answer is some derivative of the reference manual's topic, "Setting Up Tree Collisions." 
        That works just fine, and potentially gives every tree its own collider.

        My cheapo computer has a cheapo video card in it, and I keep it that way so I am forced to optimize my games for those who also have lousy video cards. 
        It occurred to me that my FPS game's flying protagonist can really only hit the nearest tree, so giving every tree a collider means physics processing for every tree but one has no return on investment. 
        Why not move one efficient capsule collider to the nearest tree in case I smack into it? 
        A built-in array and a little math in a short script sped up my FPS dramatically compared to painting trees on my terrain with one probably unused capsule collider each. 
        I got rid of the capsule collider in my prototype tree and supplied the original colliderless tree to the terrain tool for painting onto my terrain. 
        I attached the following script "sctTerrain" to the terrain object named "Terrain." 
        I added a capsule object to the scene having a capsule collider and a kinematic rigidbody (recommended by Unity if I am going to move my collider around a lot). 
        I sized the capsule by hand to appropriately match the prototype tree, the top branches of which I can fly through without dying. 
        The capsule has its mesh renderer turned off so it is invisible but still solid. I named the capsule "Tree" so if I fly into the capsule I can report, "Tree got you." 
        If my script can't find any trees AND cannot find the capsule object, it destroys itself at Start() because it is unneeded. 
        Otherwise it places the capsule to surround the nearest tree trunk and scales it to match the tree's height less the sparse top branches. 
        This math-based technique for colliding with trees is fast, and I recommend it:
        */

        // ****
        //  TO SETUP :
        //  add a capsule object to the scene having a capsule collider and a kinematic rigidbody, named "Tree"
        //  attach the following script to the terrain object named "Terrain"
        //  drag and drop your Player object in the Inspector
        // ****




 public Transform player;
  
 private TreeInstance[] paryTrees;
    public Terrain tx;
 private Vector3 pvecTerrainPosition;
 private Vector3 pvecTerrainSize;
 private GameObject pgobTreeCollide;
 private Vector3 pvecCollideScale;
 private bool pbooCollideWithTrees = false;
  
 void  Start()
    {
        // Get the terrain's position
        pvecTerrainPosition = tx.transform.position;

        // Get the terrain's size from the terrain data
        pvecTerrainSize = tx.terrainData.size;
        // Get the tree instances
        paryTrees = tx.terrainData.treeInstances;

        // Get the invisible capsule having the capsule collider that makes the nearest tree solid
        pgobTreeCollide = GameObject.Find("Tree"); // This is a capsule having a capsule collider, but when the flier hits it we want it to be reported that the flier hit a tree.

        // Are there trees and a tree collider?
        if ((pgobTreeCollide != null) && (paryTrees.Length > 0))
        {
            // Set a flag to make this script useful
            pbooCollideWithTrees = true;
            // Get the original local scale of the capsule. This is manually matched to the scale of the prototype of the tree.
            pvecCollideScale = pgobTreeCollide.transform.localScale;
        }
        // No need to use this script
        else
        {
            Debug.LogWarning("NO CAPSULE NAMED TREE FOUND, OR NO TERRAIN TREES, DESTROYING SCRIPT...");
            Destroy(this);
        }

        // has the player been assigned in the Inspector?
        if (!player)
        {
            Debug.LogWarning("NO PLAYER OBJECT IN THE INSPECTOR, DESTROYING SCRIPT...");
            Destroy(this);
        }
    }

    void Update()
    {
        int L;
        TreeInstance triTree;

        //var vecFlier : Vector3 = sctFly.svecXYZ; // My protagonist's position, passed by a static variable in a script called sctFly.
        Vector3 vecFlier = player.position; // using the player transform, dropped in the inspector

        float fltProximity;
        float fltNearest = 9999.9999f; // Farther, to start, than is possible in my game.
        Vector3 vec3;
        Vector3 vecTree;
        int intNearestPntr = 0;

        // Test the flag
        if (pbooCollideWithTrees == true)
        {
            // Find the nearest tree to the flier
            for (L = 0; L < paryTrees.Length; L++)
            {
                // Get the tree instance
                triTree = paryTrees[L];
                // Get the normalized tree position
                vecTree = triTree.position;
                // Get the world coordinates of the tree position
                vec3 = (Vector3.Scale(pvecTerrainSize, vecTree) + pvecTerrainPosition);
                // Calculate the proximity
                fltProximity = Vector3.Distance(vecFlier, vec3);
                // Nearest so far?
                if (fltProximity < fltNearest)
                {
                    // Remember the nearest
                    fltNearest = fltProximity;
                    // Remember the index
                    intNearestPntr = L;
                }
            }

            int treePrototypeIndex = paryTrees[intNearestPntr].prototypeIndex;
            Debug.Log(tx.terrainData.treePrototypes[treePrototypeIndex].prefab.name);

            /*// Get the closest tree
            triTree = paryTrees[intNearestPntr];
            // Get the normalized tree position of the closest tree
            vecTree = triTree.position;
            // Get the world coordinates of the tree position
            vec3 = (Vector3.Scale(pvecTerrainSize, vecTree) + pvecTerrainPosition);
            // Scale the capsule having the capsule collider that represents a solid tree
            pgobTreeCollide.transform.localScale = (pvecCollideScale * triTree.heightScale);
            // Add some height to position the capsule correctly on the tree
            vec3.y += pgobTreeCollide.transform.localScale.y;
            // Position the capsule having the capsule collider at the nearest tree
            pgobTreeCollide.transform.position = vec3;*/
        }
    
    }
}
