using System;
using Leap.Unity.Interaction;
using TMPro;
using UnityEngine;

namespace _TheThing.SCRIPTS
{
    public class SpawnThought : MonoBehaviour
    {
        public GameObject anchorObject;

        public GameObject thoughtBubblePrefab;

        public void SpawnTheThought(String textToSpawn)
        {
            var temp = anchorObject.GetComponent<Anchor>();
            TextMeshPro mesh = thoughtBubblePrefab.GetComponentInChildren<TextMeshPro>();
            thoughtBubblePrefab.transform.localPosition = gameObject.transform.position;
            mesh.text = textToSpawn;
            thoughtBubblePrefab.gameObject.GetComponent<AnchorableBehaviour>().anchor = temp;
            
            Instantiate(thoughtBubblePrefab);
        }
    }
}