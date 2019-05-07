#region Copyright
// ******************************************************************************************
//
// 							SimplePath, Copyright © 2011, Alex Kring
//
// ******************************************************************************************
#endregion

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Coolape
{
    public class GridBase
    {
        #region Constants
        protected static readonly Vector3 kXAxis;       // points in the directon of the positive X axis
        protected static readonly Vector3 kZAxis;       // points in the direction of the positive Y axis
        private static readonly float kDepth;           // used for intersection tests done in 3D.
        #endregion

        #region Fields
        protected int m_numberOfRows;
        protected int m_numberOfColumns;
        protected float m_cellSize;
        private Vector3 m_origin;
        #endregion

        #region Properties
        public float Width
        {
            get { return (m_numberOfColumns * m_cellSize); }
        }

        public float Height
        {
            get { return (m_numberOfRows * m_cellSize); }
        }

        public Vector3 Origin
        {
            get { return m_origin; }
        }

        public int NumberOfCells
        {
            get { return m_numberOfRows * m_numberOfColumns; }
        }

        public float Left
        {
            get { return Origin.x; }
        }

        public float Right
        {
            get { return Origin.x + Width; }
        }

        public float Top
        {
            get { return Origin.z + Height; }
        }

        public float Bottom
        {
            get { return Origin.z; }
        }

        public float CellSize
        {
            get { return m_cellSize; }
        }
        #endregion

        static GridBase()
        {
            kXAxis = new Vector3(1.0f, 0.0f, 0.0f);
            kZAxis = new Vector3(0.0f, 0.0f, 1.0f);
            kDepth = 1.0f;
        }

        // Use this for initialization
        public void init(Vector3 origin, int numRows, int numCols, float cellSize)
        {
            m_origin = origin;
            m_numberOfRows = numRows;
            m_numberOfColumns = numCols;
            m_cellSize = cellSize;
        }

        // Update is called once per frame
        //		public virtual void Update () 
        //		{
        //	
        //		}
        //		
        //		public virtual void OnDrawGizmos()
        //		{
        //	
        //		}

        public static void DebugDraw(Vector3 origin, int numRows, int numCols, float cellSize, Color color)
        {
            float width = (numCols * cellSize);
            float height = (numRows * cellSize);

            // Draw the horizontal grid lines
            for (int i = 0; i < numRows + 1; i++)
            {
                Vector3 startPos = origin + i * cellSize * kZAxis;
                Vector3 endPos = startPos + width * kXAxis;
                Debug.DrawLine(startPos, endPos, color);
            }

            // Draw the vertial grid lines
            for (int i = 0; i < numCols + 1; i++)
            {
                Vector3 startPos = origin + i * cellSize * kXAxis;
                Vector3 endPos = startPos + height * kZAxis;
                Debug.DrawLine(startPos, endPos, color);
            }
        }

        // pos is in world space coordinates. The returned position is also in world space coordinates.
        public Vector3 GetNearestCellCenter(Vector3 pos)
        {
            int index = GetCellIndex(pos);
            Vector3 cellPos = GetCellPosition(index);
            cellPos.x += (m_cellSize / 2.0f);
            cellPos.z += (m_cellSize / 2.0f);
            return cellPos;
        }

        // returns a position in world space coordinates.
        public Vector3 GetCellCenter(int index)
        {
            Vector3 cellPosition = GetCellPosition(index);
            cellPosition.x += (m_cellSize / 2.0f);
            cellPosition.z += (m_cellSize / 2.0f);
            return cellPosition;
        }
        public Vector3 GetCellCenter(int col, int row)
        {
            return GetCellCenter(GetCellIndex(col, row));
        }
        /// <summary>
        /// Returns the lower left position of the grid cell at the passed tile index. The origin of the grid is at the lower left,
        /// so it uses a cartesian coordinate system.
        /// </summary>
        /// <param name="index">index to the grid cell to consider</param>
        /// <returns>Lower left position of the grid cell (origin position of the grid cell), in world space coordinates</returns>
        public Vector3 GetCellPosition(int index)
        {
            int row = GetRow(index);
            int col = GetColumn(index);
            float x = col * m_cellSize;
            float z = row * m_cellSize;
            Vector3 cellPosition = Origin + new Vector3(x, 0.0f, z);
            return cellPosition;
        }

        // pass in world space coords. Get the tile index at the passed position
        public int GetCellIndex(Vector3 pos)
        {
            if (!IsInBounds(pos))
            {
                return -1;
            }

            pos -= Origin;

            return GetCellIndex((int)(pos.x / m_cellSize), (int)(pos.z / m_cellSize));
        }

        public int GetCellIndex(int col, int row)
        {
            return (row * m_numberOfColumns + col);
        }

        // pass in world space coords. Get the tile index at the passed position, clamped to be within the grid.
        public int GetCellIndexClamped(Vector3 pos)
        {
            pos -= Origin;

            int col = (int)(pos.x / m_cellSize);
            int row = (int)(pos.z / m_cellSize);

            //make sure the position is in range.
            col = (int)Mathf.Clamp(col, 0, m_numberOfColumns - 1);
            row = (int)Mathf.Clamp(row, 0, m_numberOfRows - 1);

            return (row * m_numberOfColumns + col);
        }

        public Bounds GetCellBounds(int index)
        {
            Vector3 cellCenterPos = GetCellPosition(index);
            cellCenterPos.x += (m_cellSize / 2.0f);
            cellCenterPos.z += (m_cellSize / 2.0f);
            Bounds cellBounds = new Bounds(cellCenterPos, new Vector3(m_cellSize, kDepth, m_cellSize));
            return cellBounds;
        }

        public Bounds GetGridBounds()
        {
            Vector3 gridCenter = Origin + (Width / 2.0f) * kXAxis + (Height / 2.0f) * kZAxis;
            Bounds gridBounds = new Bounds(gridCenter, new Vector3(Width, kDepth, Height));
            return gridBounds;
        }

        public int GetRow(int index)
        {
            int row = index / m_numberOfRows;
            return row;
        }

        public int GetColumn(int index)
        {
            int col = index % m_numberOfColumns;
            return col;
        }

        public bool IsInBounds(int col, int row)
        {
            if (col < 0 || col >= m_numberOfColumns)
            {
                return false;
            }
            else if (row < 0 || row >= m_numberOfRows)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool IsInBounds(int index)
        {
            return (index >= 0 && index < NumberOfCells);
        }

        // pass in world space coords
        public bool IsInBounds(Vector3 pos)
        {
            return (pos.x >= Left &&
                     pos.x <= Right &&
                     pos.z <= Top &&
                     pos.z >= Bottom);
        }


        public int LeftIndex(int index)
        {
            int col = GetColumn(index);
            int row = GetRow(index);
            col = col - 1;
            if (IsInBounds(col, row))
            {
                return GetCellIndex(col, row);
            }
            else
            {
                return -1;
            }
        }

        public int RightIndex(int index)
        {
            int col = GetColumn(index);
            int row = GetRow(index);
            col = col + 1;
            if (IsInBounds(col, row))
            {
                return GetCellIndex(col, row);
            }
            else
            {
                return -1;
            }
        }
        public int UpIndex(int index)
        {
            int col = GetColumn(index);
            int row = GetRow(index);
            row = row + 1;
            if (IsInBounds(col, row))
            {
                return GetCellIndex(col, row);
            }
            else
            {
                return -1;
            }
        }

        public int DownIndex(int index)
        {
            int col = GetColumn(index);
            int row = GetRow(index);
            row = row - 1;
            if (IsInBounds(col, row))
            {
                return GetCellIndex(col, row);
            }
            else
            {
                return -1;
            }
        }

        public int LeftUpIndex(int index)
        {
            int col = GetColumn(index);
            int row = GetRow(index);
            col = col - 1;
            row = row + 1;
            if (IsInBounds(col, row))
            {
                return GetCellIndex(col, row);
            }
            else
            {
                return -1;
            }
        }

        public int RightUpIndex(int index)
        {
            int col = GetColumn(index);
            int row = GetRow(index);
            col = col + 1;
            row = row + 1;
            if (IsInBounds(col, row))
            {
                return GetCellIndex(col, row);
            }
            else
            {
                return -1;
            }
        }

        public int LeftDownIndex(int index)
        {
            int col = GetColumn(index);
            int row = GetRow(index);
            col = col - 1;
            row = row - 1;
            if (IsInBounds(col, row))
            {
                return GetCellIndex(col, row);
            }
            else
            {
                return -1;
            }
        }

        public int RightDownIndex(int index)
        {
            int col = GetColumn(index);
            int row = GetRow(index);
            col = col + 1;
            row = row - 1;
            if (IsInBounds(col, row))
            {
                return GetCellIndex(col, row);
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Gets the own grids.根据中心点及占用格子size,取得占用格子index数,注意只有在长宽一样的情况时
        /// </summary>
        /// <returns>
        /// The own grids.
        /// </returns>
        /// <param name='center'>
        /// Center. 中心点index
        /// </param>
        /// <param name='size'>
        /// Size. Size * Size的范围
        /// </param>
        public List<int> getCells(int center, int size)
        {
            List<int> ret = new List<int>();
            if (center < 0)
            {
                return ret;
            }

            int half = size / 2;
            int numRows = m_numberOfRows;
            if (size % 2 == 0)
            {
                for (int row = 0; row <= half; row++)
                {
                    for (int i = 1; i <= half; i++)
                    {
                        int tpindex = center - row * numRows - i;
                        if (tpindex / numRows != (center - row * numRows) / numRows)
                        {
                            tpindex = -1;
                        }
                        ret.Add(tpindex);
                    }
                    for (int i = 0; i < half; i++)
                    {
                        int tpindex = center - row * numRows + i;
                        if (tpindex / numRows != (center - row * numRows) / numRows)
                        {
                            tpindex = -1;
                        }
                        ret.Add(tpindex);
                    }
                }
                for (int row = 1; row <= half - 1; row++)
                {
                    for (int i = 1; i <= half; i++)
                    {
                        int tpindex = center + row * numRows - i;
                        if (tpindex / numRows != (center + row * numRows) / numRows)
                        {
                            tpindex = -1;
                        }
                        ret.Add(tpindex);
                    }
                    for (int i = 0; i < half; i++)
                    {
                        int tpindex = center + row * numRows + i;
                        if (tpindex / numRows != (center + row * numRows) / numRows)
                        {
                            tpindex = -1;
                        }
                        ret.Add(tpindex);
                    }
                }

            }
            else
            {
                for (int row = 0; row <= half; row++)
                {
                    for (int i = 0; i <= half; i++)
                    {
                        int tpindex = center - row * numRows - i;
                        if (tpindex / numRows != (center - row * numRows) / numRows)
                        {
                            tpindex = -1;
                        }
                        ret.Add(tpindex);
                    }

                    for (int i = 1; i <= half; i++)
                    {
                        int tpindex = center - row * numRows + i;
                        if (tpindex / numRows != (center - row * numRows) / numRows)
                        {
                            tpindex = -1;
                        }
                        ret.Add(tpindex);
                    }
                }

                for (int row = 1; row <= half; row++)
                {
                    for (int i = 0; i <= half; i++)
                    {
                        int tpindex = center + row * numRows - i;
                        if (tpindex / numRows != (center + row * numRows) / numRows)
                        {
                            tpindex = -1;
                        }
                        ret.Add(tpindex);
                    }

                    for (int i = 1; i <= half; i++)
                    {
                        int tpindex = center + row * numRows + i;
                        if (tpindex / numRows != (center + row * numRows) / numRows)
                        {
                            tpindex = -1;
                        }
                        ret.Add(tpindex);
                    }
                }
            }
            return ret;
        }
    }
}
