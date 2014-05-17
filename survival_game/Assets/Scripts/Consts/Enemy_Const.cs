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
	public const float ENEMY_SIDE_SPEED = 2f;
	/// <summary>
	/// エネミー定数
	/// ベースとなるジャンプスピード
	/// </summary>
	public const float ENEMY_JUMP_SPEED = 10f;

	/*------------- Saito Const -----------------*/
	/// <summary>
	/// SAITO定数
	/// HP
	/// </summary>
	public const float SAITO_HP = 1f;
	/// <summary>
	/// SAITO定数
	/// 発見倍率
	/// </summary>
	public const float SAITO_NOTICE_DISTANCE_MAG = 1f;
	/// <summary>
	/// SAITO定数
	/// 速度倍率
	/// </summary>
	public const float SAITO_SPEED_MAG = 1f;
	/// <summary>
	/// SAITO定数
	/// 近づく距離
	/// </summary>
	public const float SAITO_APPROACH_DISTANCE = 5f;
	/// <summary>
	/// SAITO定数
	/// 攻撃する距離
	/// </summary>
	public const float SAITO_ATTACK_DISTANCE = 1.5f;
	/// <summary>
	/// SAITO定数
	/// 攻撃１前硬直時間
	/// </summary>
	public const float SAITO_ATTACK1_BEFORE = 0.1f;
	/// <summary>
	/// SAITO定数
	/// 攻撃１硬直時間
	/// </summary>
	public const float SAITO_ATTACK1_STIFF = 1.5f;
	/// <summary>
	/// SAITO定数
	/// 攻撃１判定時間
	/// </summary>
	public const float SAITO_ATTACK1_DESTROY = 0.2f;

	/*------------- Saito Const -----------------*/

	/*------------- Yukari Const -----------------*/
	/// <summary>
	/// YUKARI定数
	/// HP
	/// </summary>
	public const float YUKARI_HP = 10f;
	/// <summary>
	/// YUKARI定数
	/// 発見倍率
	/// </summary>
	public const float YUKARI_NOTICE_DISTANCE_MAG = 3f;
	/// <summary>
	/// SAITO定数
	/// 速度倍率
	/// </summary>
	public const float YUKARI_SPEED_MAG = 2f;
	/// <summary>
	/// YUKARI定数
	/// 近づく距離
	/// </summary>
	public const float YUKARI_APPROACH_DISTANCE = 5f;
	/// <summary>
	/// YUKARI定数
	/// 攻撃する距離
	/// </summary>
	public const float YUKARI_ATTACK_DISTANCE = 0.5f;
	/// <summary>
	/// YUKARI定数
	/// 攻撃１前硬直時間
	/// </summary>
	public const float YUKARI_ATTACK1_BEFORE = 0.1f;
	/// <summary>
	/// YUKARI定数
	/// 攻撃１硬直時間
	/// </summary>
	public const float YUKARI_ATTACK1_STIFF = 1.5f;
	/// <summary>
	/// YUKARI定数
	/// 攻撃１判定時間
	/// </summary>
	public const float YUKARI_ATTACK1_DESTROY = 0.2f;
	/// <summary>
	/// YUKARI定数
	/// 召喚硬直時間
	/// </summary>
	public const float YUKARI_SUMMON_STIFF = 2.0f;
	
	/*------------- Yukari Const -----------------*/
}
