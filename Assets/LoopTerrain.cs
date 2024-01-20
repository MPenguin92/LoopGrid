using System;
using System.Diagnostics;
using UnityEngine;

public partial class LoopTerrain : MonoBehaviour
{
    private class SingleTerrainData
    {
        public Vector2 offset;
        public Vector2 leftUpCorner { get; private set; }
        public Vector2 rightDownCorner { get; private set; }
        public GameObject terrainGo;
        private readonly float mLength;

        public SingleTerrainData(Vector2 offset,float length,GameObject go)
        {
            this.offset = offset;
            this.mLength = length;
            this.terrainGo = go;
        }

        public void Apply()
        {
            leftUpCorner = offset + new Vector2(-mLength, mLength) * 0.5f;
            rightDownCorner = offset + new Vector2(mLength, -mLength) * 0.5f;
            terrainGo.transform.position = offset;
        }
    }

    private enum TerrainDataIndex
    {
        LeftUp = 0,
        CenterUp = 1,
        RightUp = 2,
        Left = 3,
        Center = 4,
        Right = 5,
        LeftDown = 6,
        CenterDown = 7,
        RightDown = 8
    }

    [SerializeField] private GameObject[] terrainArray;

    private SingleTerrainData[] mTerrainData;

    [SerializeField] private Transform mTargetTrans;

    public float length = 24;
    private const int TERRAIN_MAX_ROW = 3;
    private const int TERRAIN_MAX_COLUMN = 3;
    private static readonly Vector2 Offset = new Vector2(-24, 24);

    private void Start()
    {
        int length = terrainArray.Length;
        if (length != TERRAIN_MAX_ROW * TERRAIN_MAX_COLUMN)
        {
            return;
        }

        mTerrainData = new SingleTerrainData[length];

        for (int rowIdx = 0; rowIdx < TERRAIN_MAX_ROW; rowIdx++)
        {
            for (int columnIdx = 0; columnIdx < TERRAIN_MAX_COLUMN; columnIdx++)
            {
                int idx = rowIdx * TERRAIN_MAX_ROW + columnIdx;
                GameObject terrainGo = terrainArray[idx];
                var newTerrainData =
                    new SingleTerrainData(Offset + new Vector2(this.length * columnIdx, -this.length * rowIdx), length,
                        terrainGo);
                newTerrainData.Apply();
                mTerrainData[idx] = newTerrainData;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (mTerrainData == null)
            return;
        Gizmos.color = Color.yellow;
        for (int i = 0; i < mTerrainData.Length; i++)
        {
            SingleTerrainData terrainData = mTerrainData[i];
            Gizmos.DrawLine(terrainData.offset, terrainData.leftUpCorner);
            Gizmos.DrawLine(terrainData.offset, terrainData.rightDownCorner);
        }
    }


    private void Update()
    {
        Vector2 targetPos = mTargetTrans.position;
        var center = mTerrainData[(int)TerrainDataIndex.Center];
        //出左边
        if (targetPos.x < center.leftUpCorner.x)
        {
            MoveRight2Left(mTerrainData[(int)TerrainDataIndex.RightUp]);
            MoveRight2Left(mTerrainData[(int)TerrainDataIndex.Right]);
            MoveRight2Left(mTerrainData[(int)TerrainDataIndex.RightDown]);
            MoveRight2LeftArray();
            Debug();
        }
        //出右边
        else if (targetPos.x > center.rightDownCorner.x)
        {
            MoveLeft2Right(mTerrainData[(int)TerrainDataIndex.LeftUp]);
            MoveLeft2Right(mTerrainData[(int)TerrainDataIndex.Left]);
            MoveLeft2Right(mTerrainData[(int)TerrainDataIndex.LeftDown]);
            MoveLeft2RightArray();
            Debug();
        }

        //出下边
        if (targetPos.y < center.rightDownCorner.y)
        {
            MoveUp2Down(mTerrainData[(int)TerrainDataIndex.LeftUp]);
            MoveUp2Down(mTerrainData[(int)TerrainDataIndex.CenterUp]);
            MoveUp2Down(mTerrainData[(int)TerrainDataIndex.RightUp]);
            MoveUp2DownArray();
            Debug();
        }

        //出上边
        else if (targetPos.y > center.leftUpCorner.y)
        {
            MoveDown2Up(mTerrainData[(int)TerrainDataIndex.LeftDown]);
            MoveDown2Up(mTerrainData[(int)TerrainDataIndex.CenterDown]);
            MoveDown2Up(mTerrainData[(int)TerrainDataIndex.RightDown]);
            MoveDown2UpArray();
            Debug();
        }
        
    }

    [Conditional("UNITY_EDITOR")]
    void Debug()
    {
        for (int i = 0; i < mTerrainData.Length; i++)
        {
            mTerrainData[i].terrainGo.name = Enum.GetName(typeof(TerrainDataIndex), i) ?? string.Empty;
        }
    }
}