/// <author>Thoams Krahl</author>

using UnityEngine;

namespace PelagosProject.Interactables
{
    public enum Habitat
    {
        Invalid = -1,
        None,
        CoralReef,
        KelpForest,
        DeepSea,
        Floor,

    }

    [CreateAssetMenu(fileName = "newCreatureData", menuName = "PelagosProject/Data/Scannables/CreatureData")]
    public class CreatureData : ScannableData
    {
        public enum CreatureSpecies
        {
            Invalid = -1,
            Jellyfish,
            Coral,
            Anemine,
            Sandworm,
            Siboglinid,
            Shell,
            Nautilus,
            Squid,
            Crab,
            Starfish,
            Urchins,
            Holothurian,
            Fish,
            SeaSnake,
            MarineMammal,
            Mystical
        }

        [SerializeField] private CreatureSpecies species = CreatureSpecies.Invalid;
        [SerializeField] private Habitat habitat1 = Habitat.None;
        [SerializeField] private Habitat habitat2 = Habitat.None;
        [SerializeField, Range(1.0f, 9999f)] private float size = 100f;
        [SerializeField, Range(1.0f, 9999999f)] private float weight = 1000f;

        public CreatureSpecies Species => species;
        public Habitat Habitat1 => habitat1;
        public Habitat Habitat2 => habitat2;
        public float Size => size;
        public float Weight => weight;
    }
}

