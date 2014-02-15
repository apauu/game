using UnityEngine;
using System.Collections;

public class Enemy_Const{
	/// <summary>
	/// エネミー定数
	/// 初期位置からのプレイヤーとの距離　これ以上離れると元の位置に戻る
	/// </summary>
	public const float IMMOBILE_DISTANCE = 60f;
	/// <summary>
	/// エネミー定数
	/// プレイヤーに気づくx距離
	/// </summary>
	public const float NOTICE_DISTANCE_X = 10f;
	/// <summary>
	/// エネミー定数
	/// プレイヤーに気づくy距離
	/// </summary>
	public const float NOTICE_DISTANCE_Y = 10f;
	/// <summary>
	/// エネミー定数
	/// ベースとなる歩行スピード
	/// </summary>
	public const float ENEMY_SIDE_SPEED = 10f;
}
