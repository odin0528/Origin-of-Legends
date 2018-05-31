using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public abstract class ChainSkill: Skill {
	public int chainTimes;
	override protected void GetEnemyTarget() {
		target.AddRange(GetTargetByChain(BattleManager.Instance.mainTarget));
	}

	override protected void GetAllyTarget() {
		if(unit is Monster) {
			target.AddRange(GetTargetByChain(unit));
		}
	}

	List<Unit> GetTargetByChain(Unit startTarget) {
		List<Unit> targetList = new List<Unit>() { startTarget };

		int dist = range.GetLength(0) / 2;
		for(int i = chainTimes;i > 1;i--) {
			Monster lastTarget = (Monster) targetList[targetList.Count - 1];
			List<Monster> nextTarget = new List<Monster>();
			for(int x = -dist;x <= dist;x++) {
				for(int y = -dist;y <= dist;y++) {
					if(range[dist + y, dist + x] == 0)
						continue;
					int posX = lastTarget.x + x;
					int posY = lastTarget.y + y;
					if(posX < 0 || posX > 2 || posY < 0 || posY > 2 || BattleManager.Instance.map[posX, posY] == null)
						continue;
					if(!targetList.Contains(BattleManager.Instance.map[posX, posY]))
						nextTarget.Add(BattleManager.Instance.map[posX, posY]);
				}
			}

			if(nextTarget.Count > 1) {
				targetList.Add(nextTarget[Random.Range(0, nextTarget.Count)]);
			} else if(nextTarget.Count == 1) {
				targetList.Add(nextTarget[0]);
			}
		}
		return targetList;
	}
}


