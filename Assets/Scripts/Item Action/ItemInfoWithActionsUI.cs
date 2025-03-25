using System.Collections;
using UnityEngine;

namespace Items
{
    public class ItemInfoWithActionsUI : MonoBehaviour
    {
        [SerializeField] private ItemInfoWithActionUI _prefab;

        private IHasAction _infoWithAction;
        private ItemInfoWithActionUI[] _uis;

        private IEnumerator Start()
        {
            yield return null;
            transform.SetParent(transform.parent.parent);
        }

        public void Init(IHasAction info, ItemUI ui)
        {
            _infoWithAction = info;
            transform.Clear(t => !t.name.StartsWith("_"));

            var actor = FightController.Player.Info;
            var skill = _infoWithAction.UseSkill;
            var raws = _infoWithAction.Actions;
            _uis = new ItemInfoWithActionUI[raws.Length];
            for (int i = 0; i < raws.Length; i++)
            {
                if (actor && skill)
                {
                    var expertise = (int)actor.GetActionAmount(skill);
                    if ((int)raws[i].Expertise > expertise) continue;
                }

                _uis[i] = Instantiate(_prefab, transform);
                _uis[i].Init(raws[i], ui);
            }
            name = ui.RawSlotUI.name + " : Actions";
        }

        public void Uninit()
        {
            for (int i = 0; i < _uis.Length; i++)
                _uis[i].Uninit();
        }
    }
}
