using UnityEngine;

namespace SmartCollider.Runtime.Math
{
    public static class ColliderMath
    {
        public static Vector3 ComputeCentroid(Vector3[] pts)
        {
            if (pts == null || pts.Length == 0)
            {
                return Vector3.zero;
            }

            Vector3 sum = Vector3.zero;
            for (int i = 0; i < pts.Length; i++)
            {
                sum += pts[i];
            }

            return sum / pts.Length;
        }

        public static Matrix3x3 ComputeCovariance(Vector3[] pts, Vector3 centroid)
        {
            Matrix3x3 covariance = new Matrix3x3();
            if (pts == null || pts.Length == 0)
            {
                return covariance;
            }

            for (int i = 0; i < pts.Length; i++)
            {
                Vector3 d = pts[i] - centroid;
                covariance.m00 += d.x * d.x;
                covariance.m01 += d.x * d.y;
                covariance.m02 += d.x * d.z;
                covariance.m10 += d.y * d.x;
                covariance.m11 += d.y * d.y;
                covariance.m12 += d.y * d.z;
                covariance.m20 += d.z * d.x;
                covariance.m21 += d.z * d.y;
                covariance.m22 += d.z * d.z;
            }

            float invN = 1f / pts.Length;
            return covariance * invN;
        }

        public static EigenResult JacobiEigenSolve(Matrix3x3 sym, int maxIters = 32, float eps = 1e-10f)
        {
            Matrix3x3 a = sym;
            Matrix3x3 v = Matrix3x3.Identity;

            EigenResult result = new EigenResult
            {
                Eigenvectors = v,
                Converged = false,
                Iterations = 0
            };

            for (int iter = 0; iter < maxIters; iter++)
            {
                int p = 0;
                int q = 1;
                float a01 = Mathf.Abs(a.m01);
                float a02 = Mathf.Abs(a.m02);
                float a12 = Mathf.Abs(a.m12);
                float maxOffDiag = a01;

                if (a02 > maxOffDiag)
                {
                    maxOffDiag = a02;
                    p = 0;
                    q = 2;
                }

                if (a12 > maxOffDiag)
                {
                    maxOffDiag = a12;
                    p = 1;
                    q = 2;
                }

                if (maxOffDiag < eps)
                {
                    result.Converged = true;
                    result.Iterations = iter;
                    break;
                }

                float app = a[p, p];
                float aqq = a[q, q];
                float apq = a[p, q];

                if (Mathf.Abs(apq) < eps)
                {
                    continue;
                }

                float tau = (aqq - app) / (2f * apq);
                float signTau = tau >= 0f ? 1f : -1f;
                float t = signTau / (Mathf.Abs(tau) + Mathf.Sqrt(1f + (tau * tau)));
                float c = 1f / Mathf.Sqrt(1f + (t * t));
                float s = t * c;

                for (int k = 0; k < 3; k++)
                {
                    if (k == p || k == q)
                    {
                        continue;
                    }

                    float akp = a[k, p];
                    float akq = a[k, q];
                    float newKp = (c * akp) - (s * akq);
                    float newKq = (c * akq) + (s * akp);

                    a[k, p] = newKp;
                    a[p, k] = newKp;
                    a[k, q] = newKq;
                    a[q, k] = newKq;
                }

                float newApp = (c * c * app) - (2f * s * c * apq) + (s * s * aqq);
                float newAqq = (s * s * app) + (2f * s * c * apq) + (c * c * aqq);
                a[p, p] = newApp;
                a[q, q] = newAqq;
                a[p, q] = 0f;
                a[q, p] = 0f;

                for (int k = 0; k < 3; k++)
                {
                    float vkp = v[k, p];
                    float vkq = v[k, q];
                    v[k, p] = (c * vkp) - (s * vkq);
                    v[k, q] = (s * vkp) + (c * vkq);
                }

                result.Iterations = iter + 1;
            }

            if (!result.Converged && result.Iterations == 0)
            {
                result.Iterations = maxIters;
            }

            result.Eigenvalues = new Vector3(a.m00, a.m11, a.m22);
            result.Eigenvectors = v;
            SortEigenPairsDescending(ref result);
            return result;
        }

        private static void SortEigenPairsDescending(ref EigenResult result)
        {
            if (result.Eigenvalues.y > result.Eigenvalues.x)
            {
                SwapEigenPair(ref result, 0, 1);
            }

            if (result.Eigenvalues.z > result.Eigenvalues.y)
            {
                SwapEigenPair(ref result, 1, 2);
            }

            if (result.Eigenvalues.y > result.Eigenvalues.x)
            {
                SwapEigenPair(ref result, 0, 1);
            }
        }

        private static void SwapEigenPair(ref EigenResult result, int i, int j)
        {
            float tmp = result.Eigenvalues[i];
            result.Eigenvalues[i] = result.Eigenvalues[j];
            result.Eigenvalues[j] = tmp;

            for (int row = 0; row < 3; row++)
            {
                float colI = result.Eigenvectors[row, i];
                result.Eigenvectors[row, i] = result.Eigenvectors[row, j];
                result.Eigenvectors[row, j] = colI;
            }
        }
    }
}
