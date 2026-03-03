using NUnit.Framework;
using UnityEngine;
using SmartCollider.Runtime.Math;

namespace SmartCollider.Tests
{
    public class MathModuleTests
    {
        [Test]
        public void ComputeCentroid_ReturnsExpectedAverage()
        {
            Vector3[] pts =
            {
                new Vector3(1f, 2f, 3f),
                new Vector3(3f, 4f, 5f),
                new Vector3(5f, 6f, 7f)
            };

            Vector3 centroid = ColliderMath.ComputeCentroid(pts);
            Assert.That(centroid.x, Is.EqualTo(3f).Within(1e-6f));
            Assert.That(centroid.y, Is.EqualTo(4f).Within(1e-6f));
            Assert.That(centroid.z, Is.EqualTo(5f).Within(1e-6f));
        }

        [Test]
        public void ComputeCovariance_IsSymmetricAndReasonable()
        {
            Vector3[] pts =
            {
                new Vector3(-1f, 0f, 0f),
                new Vector3(1f, 0f, 0f),
                new Vector3(3f, 0f, 0f)
            };

            Vector3 centroid = ColliderMath.ComputeCentroid(pts);
            Matrix3x3 cov = ColliderMath.ComputeCovariance(pts, centroid);

            Assert.That(cov.m01, Is.EqualTo(cov.m10).Within(1e-6f));
            Assert.That(cov.m02, Is.EqualTo(cov.m20).Within(1e-6f));
            Assert.That(cov.m12, Is.EqualTo(cov.m21).Within(1e-6f));
            Assert.That(cov.m11, Is.EqualTo(0f).Within(1e-6f));
            Assert.That(cov.m22, Is.EqualTo(0f).Within(1e-6f));
            Assert.That(cov.m00, Is.GreaterThan(0f));
        }

        [Test]
        public void JacobiEigenSolve_ProducesOrthogonalEigenvectors()
        {
            Matrix3x3 sym = new Matrix3x3
            {
                m00 = 3.5f, m01 = 0.4f, m02 = -0.8f,
                m10 = 0.4f, m11 = 2.2f, m12 = 0.6f,
                m20 = -0.8f, m21 = 0.6f, m22 = 1.1f
            };

            EigenResult eigen = ColliderMath.JacobiEigenSolve(sym, 64, 1e-8f);
            Matrix3x3 identityApprox = eigen.Eigenvectors.Transpose() * eigen.Eigenvectors;

            Assert.That(identityApprox.m00, Is.EqualTo(1f).Within(1e-3f));
            Assert.That(identityApprox.m11, Is.EqualTo(1f).Within(1e-3f));
            Assert.That(identityApprox.m22, Is.EqualTo(1f).Within(1e-3f));
            Assert.That(identityApprox.m01, Is.EqualTo(0f).Within(1e-3f));
            Assert.That(identityApprox.m02, Is.EqualTo(0f).Within(1e-3f));
            Assert.That(identityApprox.m10, Is.EqualTo(0f).Within(1e-3f));
            Assert.That(identityApprox.m12, Is.EqualTo(0f).Within(1e-3f));
            Assert.That(identityApprox.m20, Is.EqualTo(0f).Within(1e-3f));
            Assert.That(identityApprox.m21, Is.EqualTo(0f).Within(1e-3f));
        }

        [Test]
        public void JacobiEigenSolve_IsStableForRotatedSymmetricMatrix()
        {
            Matrix3x3 d = new Matrix3x3
            {
                m00 = 6f, m11 = 3f, m22 = 1f
            };

            Matrix3x3 r = RotationMatrix(Quaternion.Euler(20f, -35f, 12f));
            Matrix3x3 sym = r * d * r.Transpose();

            EigenResult eigen = ColliderMath.JacobiEigenSolve(sym, 64, 1e-8f);
            Matrix3x3 lambda = new Matrix3x3
            {
                m00 = eigen.Eigenvalues.x,
                m11 = eigen.Eigenvalues.y,
                m22 = eigen.Eigenvalues.z
            };

            Matrix3x3 reconstructed = eigen.Eigenvectors * lambda * eigen.Eigenvectors.Transpose();

            Assert.That(AbsMax(sym - reconstructed), Is.LessThan(2e-3f));
        }

        private static Matrix3x3 RotationMatrix(Quaternion q)
        {
            Vector3 x = q * Vector3.right;
            Vector3 y = q * Vector3.up;
            Vector3 z = q * Vector3.forward;

            return new Matrix3x3
            {
                m00 = x.x, m01 = y.x, m02 = z.x,
                m10 = x.y, m11 = y.y, m12 = z.y,
                m20 = x.z, m21 = y.z, m22 = z.z
            };
        }

        private static float AbsMax(Matrix3x3 m)
        {
            float max = Mathf.Abs(m.m00);
            max = Mathf.Max(max, Mathf.Abs(m.m01));
            max = Mathf.Max(max, Mathf.Abs(m.m02));
            max = Mathf.Max(max, Mathf.Abs(m.m10));
            max = Mathf.Max(max, Mathf.Abs(m.m11));
            max = Mathf.Max(max, Mathf.Abs(m.m12));
            max = Mathf.Max(max, Mathf.Abs(m.m20));
            max = Mathf.Max(max, Mathf.Abs(m.m21));
            max = Mathf.Max(max, Mathf.Abs(m.m22));
            return max;
        }
    }
}
