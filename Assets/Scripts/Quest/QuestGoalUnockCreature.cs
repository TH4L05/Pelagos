

using PelagosProject.Interactables;
using PelagosProject.UI;

namespace PelagosProject.Quests
{  
    public class QuestGoalUnockCreature : QuestGoal
    {
        public override void Setup()
        {
            ScannableLibary.CreatureUnlocked += UpdateAmount;
        }

        public override void Destroy()
        {
            ScannableLibary.CreatureUnlocked += UpdateAmount;
        }

        public void UpdateAmount(ScannableData data)
        {
            base.UpdateAmount();
        }
    }
}
