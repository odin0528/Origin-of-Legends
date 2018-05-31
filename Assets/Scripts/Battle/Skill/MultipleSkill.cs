using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public abstract class MultipleSkill: Skill {
	override protected void GetEnemyTarget() {
		if(unit is Monster) {
			target.AddRange(BattleManager.Instance.GetLiveHero());
		} else {
			target.AddRange(GetTargetByMultiple(BattleManager.Instance.mainTarget));
		}
	}

	override protected void GetAllyTarget() {
		if(unit is Monster) {
			target.AddRange(GetTargetByMultiple(unit));
		} else {
			for(int i=0;i < BattleManager.Instance.hero.Length; i++) {
				if(!BattleManager.Instance.hero[i].isDead)
					target.Add(BattleManager.Instance.hero[i]);
			}
		}
	}

	List<Unit> GetTargetByMultiple(Unit startTarget) {
		List<Unit> targetList = new List<Unit>() { startTarget };
		int dist = range.GetLength(0) / 2;
		for(int x = -dist;x <= dist;x++) {
			for(int y = -dist;y <= dist;y++) {
				if(range[dist + y, dist + x] == 0)
					continue;
				int posX = startTarget.x + x;
				int posY = startTarget.y + y;
				if(posX < 0 || posX > 2 || posY < 0 || posY > 2 || BattleManager.Instance.map[posX, posY] == null)
					continue;
				if(!targetList.Contains(BattleManager.Instance.map[posX, posY]))
					targetList.Add(BattleManager.Instance.map[posX, posY]);
			}
		}
		return targetList;
	}
}


