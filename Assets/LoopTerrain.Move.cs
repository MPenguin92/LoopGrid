// LoopTerrain.Move.cs
// Created by Cui Lingzhi
// on 2024 - 01 - 20

using UnityEngine;

public partial class LoopTerrain
{
    private SingleTerrainData mTemp;

    private void MoveRight2LeftArray()
    {
        for (int rowIdx = 0; rowIdx < TERRAIN_MAX_ROW; rowIdx++)
        {
            //最右先存起来
            mTemp = mTerrainData[rowIdx * TERRAIN_MAX_ROW + TERRAIN_MAX_COLUMN - 1];
            for (int columnIdx = TERRAIN_MAX_COLUMN - 2; columnIdx >= 0; columnIdx--)
            {
                //一起右移
                mTerrainData[rowIdx * TERRAIN_MAX_ROW + columnIdx + 1] = mTerrainData[rowIdx * TERRAIN_MAX_ROW + columnIdx];
            }

            mTerrainData[rowIdx * TERRAIN_MAX_ROW] = mTemp;
        }
    }

    private void MoveLeft2RightArray()
    {
        for (int rowIdx = 0; rowIdx < TERRAIN_MAX_ROW; rowIdx++)
        {
            mTemp = mTerrainData[rowIdx * TERRAIN_MAX_ROW];
            for (int columnIdx = 1; columnIdx < TERRAIN_MAX_COLUMN; columnIdx++)
            {
                mTerrainData[rowIdx * TERRAIN_MAX_ROW + columnIdx - 1] = mTerrainData[rowIdx * TERRAIN_MAX_ROW + columnIdx];
            }

            mTerrainData[rowIdx * TERRAIN_MAX_ROW + TERRAIN_MAX_COLUMN - 1] = mTemp;
        }
    }

    private void MoveUp2DownArray()
    {
        for (int columnIdx = 0; columnIdx < TERRAIN_MAX_COLUMN; columnIdx++)
        {
            //最上面存起来
            mTemp = mTerrainData[columnIdx];
            for (int rowIdx = 1; rowIdx < TERRAIN_MAX_ROW; rowIdx++)
            {
                //一起上移
                mTerrainData[(rowIdx - 1) * TERRAIN_MAX_ROW + columnIdx] = mTerrainData[rowIdx * TERRAIN_MAX_ROW + columnIdx];
            }

            mTerrainData[(TERRAIN_MAX_ROW - 1) * TERRAIN_MAX_ROW + columnIdx] = mTemp;
        }
    }

    private void MoveDown2UpArray()
    {
        for (int columnIdx = 0; columnIdx < TERRAIN_MAX_COLUMN; columnIdx++)
        {
            mTemp = mTerrainData[(TERRAIN_MAX_ROW - 1) * TERRAIN_MAX_ROW + columnIdx];
            for (int rowIdx = TERRAIN_MAX_ROW - 2; rowIdx >= 0; rowIdx--)
            {
                mTerrainData[(rowIdx + 1) * TERRAIN_MAX_ROW + columnIdx] = mTerrainData[rowIdx * TERRAIN_MAX_ROW + columnIdx];
            }

            mTerrainData[columnIdx] = mTemp;
        }
    }

    private void MoveRight2Left(SingleTerrainData data)
    {
        data.offset -= new Vector2(length, 0) * 3;
        data.Apply();
    }

    private void MoveLeft2Right(SingleTerrainData data)
    {
        data.offset += new Vector2(length, 0) * 3;
        data.Apply();
    }

    private void MoveUp2Down(SingleTerrainData data)
    {
        data.offset -= new Vector2(0, length) * 3;
        data.Apply();
    }

    private void MoveDown2Up(SingleTerrainData data)
    {
        data.offset += new Vector2(0, length) * 3;
        data.Apply();
    }
}