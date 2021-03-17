using System;
using System.Collections.Generic;
using System.Text;

namespace ParProg
{
    class VecNd {
        private int N;
        public int Dimension => N;

        private double[] mtx;
        public double[] Matrix => mtx;

        public VecNd(int N) {
            this.N = N;
            this.mtx = new double[N];
        }

        public VecNd(double[] mtx) {
            this.N = mtx.Length;
            this.mtx = new double[N];
            Array.Copy(mtx, this.mtx, N);
        }

        public void SetElement(int i, double element) { 
            if (i >= N) throw new Exception("i should be lower than N!");
            this.mtx[i] = element;
        }

        public string ToString() {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < N; i++)
                result.Append(this.mtx[i] + "\t");
            return result.ToString();
        }

        public VecNd Copy() {
            VecNd result = new VecNd(N);
            Array.Copy(this.mtx, result.mtx, N);
            return result;
        }

        public VecNd RandomInt(int from, int to) {
            Random random = new Random();
            VecNd result = new VecNd(N);
            for (int i = 0; i < N; i++) result.mtx[i] = random.Next(from, to);
            return result;
        }
    }
}
