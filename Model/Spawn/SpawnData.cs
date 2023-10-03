using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mercenary_data_editor
{
  [Serializable]
  public class SpawnData
  {
    [Serializable]
    public class Enemy
    {
      [Serializable]
      public class Status
      {
        public float hp;
        public float damage;
        public float moveSpeed;
        public float dropCoin;
      }

      [Serializable]
      public class Detail
      {
        public enum DetailStatus
        {
          Range, BulletSpeed, AttackSpeed,
          IncreaseMoveSpeedPerSecond, MaxMoveSpeed
        }

        public DetailStatus status;
        public float value;
      }

      public string name;
      public Status defaultStatus;
      public Status increaseStatus;

      public Detail[] detailStatus;
    }

    [Serializable]
    public class Spawns
    {
      [Serializable]
      public class Spawn
      {
        public string name;
        public int count;
        public int simultaneousSpawnCount;
        public float delay;
        public float range;
      }

      public Spawn[] spawns;
    }

    public Enemy[] enemyData;

    public Spawns[] spawns;

    public float[] waveTime;
  }
}
