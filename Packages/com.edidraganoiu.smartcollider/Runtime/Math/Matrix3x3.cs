using UnityEngine;

namespace SmartCollider.Runtime.Math
{
    /// <summary>
    /// Lightweight 3x3 matrix for SmartCollider runtime math and PCA-related operations.
    /// </summary>
    public struct Matrix3x3
    {
        public float m00, m01, m02;
        public float m10, m11, m12;
        public float m20, m21, m22;

        public static Matrix3x3 Identity
        {
            get
            {
                return new Matrix3x3
                {
                    m00 = 1f,
                    m11 = 1f,
                    m22 = 1f
                };
            }
        }

        public float this[int row, int col]
        {
            get
            {
                if (row == 0)
                {
                    if (col == 0) return m00;
                    if (col == 1) return m01;
                    if (col == 2) return m02;
                }
                else if (row == 1)
                {
                    if (col == 0) return m10;
                    if (col == 1) return m11;
                    if (col == 2) return m12;
                }
                else if (row == 2)
                {
                    if (col == 0) return m20;
                    if (col == 1) return m21;
                    if (col == 2) return m22;
                }

                throw new System.IndexOutOfRangeException("Matrix3x3 index out of range.");
            }
            set
            {
                if (row == 0)
                {
                    if (col == 0) { m00 = value; return; }
                    if (col == 1) { m01 = value; return; }
                    if (col == 2) { m02 = value; return; }
                }
                else if (row == 1)
                {
                    if (col == 0) { m10 = value; return; }
                    if (col == 1) { m11 = value; return; }
                    if (col == 2) { m12 = value; return; }
                }
                else if (row == 2)
                {
                    if (col == 0) { m20 = value; return; }
                    if (col == 1) { m21 = value; return; }
                    if (col == 2) { m22 = value; return; }
                }

                throw new System.IndexOutOfRangeException("Matrix3x3 index out of range.");
            }
        }

        public Matrix3x3 Transpose()
        {
            return new Matrix3x3
            {
                m00 = m00,
                m01 = m10,
                m02 = m20,
                m10 = m01,
                m11 = m11,
                m12 = m21,
                m20 = m02,
                m21 = m12,
                m22 = m22
            };
        }

        public static Vector3 operator *(Matrix3x3 m, Vector3 v)
        {
            return new Vector3(
                (m.m00 * v.x) + (m.m01 * v.y) + (m.m02 * v.z),
                (m.m10 * v.x) + (m.m11 * v.y) + (m.m12 * v.z),
                (m.m20 * v.x) + (m.m21 * v.y) + (m.m22 * v.z)
            );
        }

        public static Matrix3x3 operator +(Matrix3x3 a, Matrix3x3 b)
        {
            return new Matrix3x3
            {
                m00 = a.m00 + b.m00,
                m01 = a.m01 + b.m01,
                m02 = a.m02 + b.m02,
                m10 = a.m10 + b.m10,
                m11 = a.m11 + b.m11,
                m12 = a.m12 + b.m12,
                m20 = a.m20 + b.m20,
                m21 = a.m21 + b.m21,
                m22 = a.m22 + b.m22
            };
        }

        public static Matrix3x3 operator -(Matrix3x3 a, Matrix3x3 b)
        {
            return new Matrix3x3
            {
                m00 = a.m00 - b.m00,
                m01 = a.m01 - b.m01,
                m02 = a.m02 - b.m02,
                m10 = a.m10 - b.m10,
                m11 = a.m11 - b.m11,
                m12 = a.m12 - b.m12,
                m20 = a.m20 - b.m20,
                m21 = a.m21 - b.m21,
                m22 = a.m22 - b.m22
            };
        }

        public static Matrix3x3 operator *(Matrix3x3 m, float s)
        {
            return new Matrix3x3
            {
                m00 = m.m00 * s,
                m01 = m.m01 * s,
                m02 = m.m02 * s,
                m10 = m.m10 * s,
                m11 = m.m11 * s,
                m12 = m.m12 * s,
                m20 = m.m20 * s,
                m21 = m.m21 * s,
                m22 = m.m22 * s
            };
        }

        public static Matrix3x3 operator *(float s, Matrix3x3 m)
        {
            return m * s;
        }

        public static Matrix3x3 operator *(Matrix3x3 a, Matrix3x3 b)
        {
            return new Matrix3x3
            {
                m00 = (a.m00 * b.m00) + (a.m01 * b.m10) + (a.m02 * b.m20),
                m01 = (a.m00 * b.m01) + (a.m01 * b.m11) + (a.m02 * b.m21),
                m02 = (a.m00 * b.m02) + (a.m01 * b.m12) + (a.m02 * b.m22),
                m10 = (a.m10 * b.m00) + (a.m11 * b.m10) + (a.m12 * b.m20),
                m11 = (a.m10 * b.m01) + (a.m11 * b.m11) + (a.m12 * b.m21),
                m12 = (a.m10 * b.m02) + (a.m11 * b.m12) + (a.m12 * b.m22),
                m20 = (a.m20 * b.m00) + (a.m21 * b.m10) + (a.m22 * b.m20),
                m21 = (a.m20 * b.m01) + (a.m21 * b.m11) + (a.m22 * b.m21),
                m22 = (a.m20 * b.m02) + (a.m21 * b.m12) + (a.m22 * b.m22)
            };
        }
    }
}
