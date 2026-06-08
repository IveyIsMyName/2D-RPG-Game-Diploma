using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/Grant skill poing", fileName = "Item effect Data - Grant Skill Point")]
public class ItemEffectGrantSkillPoint : ItemEffectDataSO
{
    [SerializeField] private int pointsToAdd;

	public override void ExecuteEffect()
	{
		UI ui = FindAnyObjectByType<UI>();
		ui.skillTreeUI.AddSkillPoints(pointsToAdd);
	}
}
