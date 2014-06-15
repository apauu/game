using UnityEngine;
using System.Collections;

public class Yoda : Saito {

	/// <summary>
	/// スキル１　近接攻撃
	/// </summary>
	protected void skill1Exe ()
	{
		base.Attack(base.attack1Prefab);
	}

	/// <summar
	/// <summary>
	/// キャラクター固有のステータスを初期化する
	/// </summary>
//	protected override void setCharacteristic() {
//		this.approachDistance = Enemy_Const.LULU_APPROACH_DISTANCE;
//		this.attackDistance = Enemy_Const.LULU_ATTACK_DISTANCE;
//		this.noticeDistanceXMag = Enemy_Const.LULU_NOTICE_DISTANCE_MAG;
//		this.noticeDistanceYMag = Enemy_Const.LULU_NOTICE_DISTANCE_MAG;
//		this.enemySideSpeedMag = Enemy_Const.LULU_SPEED_MAG;;
//		this.hitPoint = Enemy_Const.LULU_HP;
//	}
}
