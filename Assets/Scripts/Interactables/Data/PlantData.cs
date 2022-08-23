/// <author>Thoams Krahl</author>

using UnityEngine;

namespace PelagosProject.Interactables
{
    [CreateAssetMenu(fileName = "newPlantData", menuName = "PelagosProject/Data/Scannables/PlantData")]
    public class PlantData : ScannableData
    {
        public enum PlantSpecies
        {
            Invalid = -1,
            GreenAlgae,
            BrownAlgae,
            RedAlgae,
            Seagras,
            Kelp,
        }

        [SerializeField] private PlantSpecies species = PlantSpecies.Invalid;
        [SerializeField] private Habitat habitat1 = Habitat.None;
        [SerializeField] private Habitat habitat2 = Habitat.None;
        [SerializeField, Range(0.1f, 9999f)] private float size = 1f;

        public PlantSpecies Species => species;
        public Habitat Habitat1 => habitat1;
        public Habitat Habitat2 => habitat2;
        public float Size => size;

    }
}
