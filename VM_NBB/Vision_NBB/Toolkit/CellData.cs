using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vision_NBB.Toolkit
{
 

   public class CellData
    {

        LimitedQueue<float[]> A_Cells = new LimitedQueue<float[]>(2);
        LimitedQueue<float[]> B_Cells = new LimitedQueue<float[]>(2);

        LimitedQueue<float> x1_Queue = new LimitedQueue<float>(2);
        LimitedQueue<float> x2_Queue = new LimitedQueue<float>(2);

        public void AddData(int UpGlueResult, int DwGlueResult, ref float[] UpGlueData, ref float[] DownGlueData, ref float X1, ref float X2)
        {

            try
            {
                /////////////////A////////////////////
                ///

                if (UpGlueResult > 0 && UpGlueResult != 2)
                {
                    float[] dataA = new float[UpGlueData.Length];
                    Array.Copy(UpGlueData, dataA, UpGlueData.Length);
                    A_Cells.Enqueue(dataA);
                    x1_Queue.Enqueue(X1);
                }
                else
                {
                    if (A_Cells.Count < 2)
                    {
                        float[] dataA = new float[UpGlueData.Length];
                        Array.Copy(UpGlueData, dataA, UpGlueData.Length);
                        A_Cells.Enqueue(dataA);
                        X1 = 0;
                        x1_Queue.Enqueue(X1);
                    }
                    else
                    {
                        var PreData_A = A_Cells.Peek();
                        var PreX1 = x1_Queue.Peek();
                        A_Cells.Enqueue(PreData_A);
                        x1_Queue.Enqueue(X1);
                        UpGlueData = PreData_A;
                        X1 = PreX1;
                    }

                }

                ///////////////////B/////////////////////////
                if (DwGlueResult > 0)
                {
                    float[] dataB = new float[DownGlueData.Length];
                    Array.Copy(DownGlueData, dataB, DownGlueData.Length);
                    B_Cells.Enqueue(dataB);
                    x2_Queue.Enqueue(X2);
                }
                else
                {
                    if (B_Cells.Count < 2)
                    {
                        float[] dataB = new float[DownGlueData.Length];
                        Array.Copy(DownGlueData, dataB, DownGlueData.Length);
                        B_Cells.Enqueue(dataB);
                        X2 = 0;
                        x2_Queue.Enqueue(X2);
                    }
                    else
                    {


                        var PreData_B = B_Cells.Peek();
                        var PreX2 = x2_Queue.Peek();

                        B_Cells.Enqueue(PreData_B);
                        DownGlueData = PreData_B;
                        X2 = PreX2;


                    }




                }
            }
            catch (Exception ex)
            {


            }


        }




    }

    public class LimitedQueue<T> : Queue<T>
    {
        private int limit;

        public LimitedQueue(int limit)
        {
            this.limit = limit;
        }

        public new void Enqueue(T item)
        {
            if (Count >= limit)
            {
                Dequeue();
            }
            base.Enqueue(item);
        }
    }
}
