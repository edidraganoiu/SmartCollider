using UnityEngine;

namespace SmartCollider.Runtime.Math
{
    public struct EigenResult
    {
        public Vector3 Eigenvalues;
        public Matrix3x3 Eigenvectors;
        public int Iterations;
        public bool Converged;
    }
}
