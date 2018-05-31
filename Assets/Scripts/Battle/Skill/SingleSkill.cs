using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class SingleSkill: Skill {
	override protected void GetEnemyTarget() {
		if(unit is Monster) {
			target.Add(BattleManager.Instance.GetRandomHero());
		} else {
			target.Add(BattleManager.Instance.mainTarget);
		}
	}

	override protected void GetAllyTarget() {
		if(unit is Monster) {
			target.AddRange(BattleManager.Instance.GetRandomEnemy(1, unit));
		} else {
			if(unit.sn == 0 && !BattleManager.Instance.hero[1].isDead)
				target.Add(BattleManager.Instance.hero[1]);
			else if(!BattleManager.Instance.hero[0].isDead)
				target.Add(BattleManager.Instance.hero[0]);
		}
	}
}


