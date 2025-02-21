namespace Actor
{
    [System.Serializable]
    public class ActorInfoWithChance : ChancesWithIntRange<ActorInfo>
    {
        public override string ToString() => $"{(Item ? Item.name : "EMPTY")} - {Chance:f1}";
    }
}
