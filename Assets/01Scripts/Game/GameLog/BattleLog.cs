using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattleLog : MonoBehaviour
{
    public Image attackerIcon, defenderIcon;
    public Image EffectIcon;
    public Button attackerInfoButton, defenderInfoButton;

    Card attacker, defender;
    // Start is called before the first frame update
    public void Setup(Card _attacker, Card _defender)
    {
        this.attacker = _attacker;
        this.defender = _defender;

        attackerIcon.sprite = attacker.sprite;
        defenderIcon.sprite = defender.sprite;

        attackerInfoButton.onClick.AddListener(() =>
        {
            attackerInfo();
        });

        defenderInfoButton.onClick.AddListener(() =>
        {
            defenderInfo();
        });
    }

    public void SetupOutpost(Card _attacker)
    {
        this.attacker = _attacker;

        attackerIcon.sprite = attacker.sprite;
    }

    void attackerInfo()
    {
        EnlargeCardManager.instance.Setup(attacker, true);
    }

    void defenderInfo()
    {
        EnlargeCardManager.instance.Setup(defender, true);
    }
}
