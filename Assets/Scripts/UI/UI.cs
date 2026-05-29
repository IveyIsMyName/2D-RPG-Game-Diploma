using UnityEngine;

public class UI : MonoBehaviour
{
    public UISkillToolTip skillToolTip;
    public UISkillTree skillTree;
    private bool skillTreeEnabled;

    private void Awake()
    {
        skillToolTip = GetComponentInChildren<UISkillToolTip>();
        skillTree = GetComponentInChildren<UISkillTree>(true);
    }

    public void ToggleSkillTreeUI()
    {
        skillTreeEnabled = !skillTreeEnabled;
        skillTree.gameObject.SetActive(skillTreeEnabled);
        skillToolTip.ShowToolTip(false, null);
    }
}
