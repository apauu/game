using System.Collections;

/// <summary>
/// Damage_ info.
/// ダメージ発生時に受け渡す情報を持つクラス
/// </summary>
public class Damage_Info
{
		/// <summary>
		/// ダメージ値
		/// </summary>
		private int dmgPoint;
		/// <summary>
		/// 攻撃者の情報
		/// </summary>
		private string enemyName;
		/// <summary>
		/// 発生する硬直時間
		/// </summary>
		private float stiffTime;
		/// <summary>
		/// 発生する無敵時間
		/// </summary>
		private float mutekiTime;

		/// <summary>
		/// Initializes a new instance of the <see cref="Damage_Info"/> class.
		/// </summary>
		/// <param name="dmgPoint">ダメージ値/param>
		/// <param name="enemyName">攻撃者の情報</param>
		public Damage_Info (int dmgPoint, string enemyName)
		{
				this.dmgPoint = dmgPoint;
				this.enemyName = enemyName;

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Damage_Info"/> class.
		/// </summary>
		/// <param name="dmgPoint">ダメージ値/param>
		/// <param name="enemyName">攻撃者の情報</param>
		/// <param name="stiffTime">発生する硬直時間</param>
		public Damage_Info (int dmgPoint, string enemyName, float stiffTime)
		{
				this.dmgPoint = dmgPoint;
				this.enemyName = enemyName;
				this.stiffTime = stiffTime;
		
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Damage_Info"/> class.
		/// </summary>
		/// <param name="dmgPoint">ダメージ値/param>
		/// <param name="enemyName">攻撃者の情報</param>
		/// <param name="stiffTime">発生する硬直時間</param>
		/// <param name="mutekiTime">発生する無敵時間</param>
		public Damage_Info (int dmgPoint, string enemyName, float stiffTime, float mutekiTime)
		{
				this.dmgPoint = dmgPoint;
				this.enemyName = enemyName;
				this.stiffTime = stiffTime;
				this.mutekiTime = mutekiTime;
		
		}

		public int getDmgPoint ()
		{
				return dmgPoint;
		}

		public void setDmgPoint (int dmpPoint)
		{
				this.dmgPoint = dmgPoint;
		}
	
		public string getEnemyName ()
		{
				return enemyName;
		}

		public void setEnemyName (string enemyName)
		{
				this.enemyName = enemyName;
		}
	
		public float getStiffTime ()
		{
				return stiffTime;
		}

		public void setStiffTime (float stiffTime)
		{
				this.stiffTime = stiffTime;
		}
	
		public float getMutekiTimet ()
		{
				return mutekiTime;
		}

		public void setMutekiTime (float mutekiTime)
		{
				this.mutekiTime = mutekiTime;
		}


}
