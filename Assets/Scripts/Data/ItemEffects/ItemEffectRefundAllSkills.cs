using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/Reset all skilss", fileName = "Reset all skills")]
public class ItemEffectRefundAllSkills : ItemEffectDataSO
{
	public override void ExecuteEffect()
	{
		UI ui = FindAnyObjectByType<UI>();
		ui.skillTreeUI.RefundAllSkills();
	}
}
