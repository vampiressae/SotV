using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Factions
{
    [CreateAssetMenu(menuName = "Factions Info")]
    public class FactionsInfo : ScriptableObject
    {
        [SerializeField] private LayerMask _actorsLayerMask;
        [SerializeField] private Relationship[] _relationships;

        private Dictionary<FactionInfo, Relationship> _relations;

        public LayerMask ActorsLayerMask => _actorsLayerMask;

        public Dictionary<FactionInfo, Relationship> Relations
        {
            get
            {
                if (_relations == null || _relations.Count == 0)
                {
                    _relations = new();
                    foreach (var relationship in _relationships)
                        _relations.Add(relationship.Faction, relationship);
                }
                return _relations;
            }
        }

        public Reaction GetReaction(FactionInfo what, FactionInfo with) =>
            Relations.TryGetValue(what, out var relationship)
            ? GetRelationshipModule(relationship, with, out var module) 
                ? module.Reaction : relationship.DefaultReaction
            : Reaction.Passive;

        private bool GetRelationshipModule(Relationship relationship, FactionInfo faction, out Relationship.Module result)
        {
            result = relationship.Modules.Where(module=>module.Factions.Contains(faction)).FirstOrDefault();
            return result != null;
        }
    }
}