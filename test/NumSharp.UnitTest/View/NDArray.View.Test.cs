﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumSharp.Backends;

namespace NumSharp.UnitTest.View
{
    [TestClass]
    public class NDArrayViewTest : TestClass
    {
        [TestMethod]
        public void ValueTest()
        {
            var x = np.arange(3);
            var v = x.view();

            v[0] = 1;

            Assert.IsTrue((int)x[0] == (int)v[0]);
        }

        [TestMethod]
        public void GetData_1D_Span()
        {
            // these slicing operations should be executed with Span<T> internally
            var data = np.arange(10);
            Assert.AreEqual(new Shape(10), new Shape(data.shape));
            // return identical view
            var view = data[":"];
            Assert.AreEqual(new Shape(10), new Shape(view.shape));
            AssertAreEqual(data.Data<int>(), view.Data<int>());
            view = data["-77:77"];
            Assert.AreEqual(new Shape(10), new Shape(view.shape));
            AssertAreEqual(data.Data<int>(), view.Data<int>());
            // return reduced view
            view = data["7:"];
            Assert.AreEqual(new Shape(3), new Shape(view.shape));
            AssertAreEqual(new int[] {7, 8, 9}, view.Data<int>());
            view = data[":5"];
            Assert.AreEqual(new Shape(5), new Shape(view.shape));
            AssertAreEqual(new int[] {0, 1, 2, 3, 4}, view.Data<int>());
            view = data["2:3"];
            Assert.AreEqual(new Shape(1), new Shape(view.shape));
            AssertAreEqual(new int[] {2}, view.Data<int>());
        }

        [TestMethod]
        public void GetData_1D_ViewStorage()
        {
            // these slicing operations should be executed with ViewStorage internally
            var data = np.arange(10);
            // return stepped view
            var view = data[ "::2"];
            Assert.AreEqual(new Shape(5), new Shape(view.shape));
            AssertAreEqual(new int[] { 0, 2, 4, 6, 8 }, view.Data<int>());
            view = data[ "::3"];
            Assert.AreEqual(new Shape(4), new Shape(view.shape));
            AssertAreEqual(new int[] { 0, 3, 6, 9 }, view.Data<int>());
            view = data[ "-77:77:77"];
            Assert.AreEqual(new Shape(1), new Shape(view.shape));
            AssertAreEqual(new[] { 0 }, view.Data<int>());
            // negative step!
            view = data[ "::-1"];
            Assert.AreEqual(new Shape(10), new Shape(view.shape));
            AssertAreEqual(data.Data<int>().OfType<int>().Reverse().ToArray(), view.Data<int>());
            view = data[ "::-2"];
            Assert.AreEqual(new Shape(5), new Shape(view.shape));
            AssertAreEqual(new int[] { 9, 7, 5, 3, 1 }, view.Data<int>());
            view = data[ "::-3"];
            Assert.AreEqual(new Shape(4), new Shape(view.shape));
            AssertAreEqual(new int[] { 9, 6, 3, 0 }, view.Data<int>());
            view = data[ "-77:77:-77"];
            Assert.AreEqual(new Shape(1), new Shape(view.shape));
            AssertAreEqual(new[] { 9 }, view.Data<int>());
        }

        [TestMethod]
        public void Indexing_1D_Span()
        {
            var data = np.arange(10);
            // return identical view
            var view = data[ ":"];
            Assert.AreEqual(0, (int)view[0]);
            Assert.AreEqual(5, (int)view[5]);
            Assert.AreEqual(9, (int)view[9]);
            view = data[ "-77:77"];
            Assert.AreEqual(0, (int)view[0]);
            Assert.AreEqual(5, (int)view[5]);
            Assert.AreEqual(9, (int)view[9]);
            // return reduced view
            view = data[ "7:"];
            Assert.AreEqual(7, (int)view[0]);
            Assert.AreEqual(8, (int)view[1]);
            Assert.AreEqual(9, (int)view[2]);
            view = data[ ":5"];
            Assert.AreEqual(0, (int)view[0]);
            Assert.AreEqual(1, (int)view[1]);
            Assert.AreEqual(2, (int)view[2]);
            Assert.AreEqual(3, (int)view[3]);
            Assert.AreEqual(4, (int)view[4]);
            view = data[ "2:3"];
            Assert.AreEqual(2, (int)view[0]);
        }

        [TestMethod]
        public void Indexing_1D_ViewStorage()
        {
            var data = np.arange(10);
            // return stepped view
            var view = data[ "::2"];
            Assert.AreEqual(0, (int)view[0]);
            Assert.AreEqual(2, (int)view[1]);
            Assert.AreEqual(4, (int)view[2]);
            Assert.AreEqual(6, (int)view[3]);
            Assert.AreEqual(8, (int)view[4]);
            view = data[ "::3"];
            Assert.AreEqual(0, (int)view[0]);
            Assert.AreEqual(3, (int)view[1]);
            Assert.AreEqual(6, (int)view[2]);
            Assert.AreEqual(9, (int)view[3]);
            view = data[ "-77:77:77"];
            Assert.AreEqual(0, (int)view[0]);
            // negative step!
            view = data[ "::-1"];
            Assert.AreEqual(9, (int)view[0]);
            Assert.AreEqual(4, (int)view[5]);
            Assert.AreEqual(0, (int)view[9]);
            view = data[ "::-2"];
            Assert.AreEqual(9, (int)view[0]);
            Assert.AreEqual(7, (int)view[1]);
            Assert.AreEqual(5, (int)view[2]);
            Assert.AreEqual(3, (int)view[3]);
            Assert.AreEqual(1, (int)view[4]);
            view = data[ "::-3"];
            Assert.AreEqual(9, (int)view[0]);
            Assert.AreEqual(6, (int)view[1]);
            Assert.AreEqual(3, (int)view[2]);
            Assert.AreEqual(0, (int)view[3]);
            view = data[ "-77:77:-77"];
            Assert.AreEqual(9, (int)view[0]);
        }

        [TestMethod]
        public void Shared_Data_1D_Span()
        {
            var data = np.arange(10);
            // return identical view
            var view = data[":"];
            Assert.AreEqual(new Shape(10), new Shape(view.shape));
            AssertAreEqual(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, view.Data<int>());
            data[9] = 99;
            Assert.AreEqual(99, (int)view[9]);
            view[1] = 11;
            Assert.AreEqual(11, (int)data[1]);
            data.ReplaceData(new int[] { 0, -1, -2, -3, -4, -5, -6, -7, -8, -9 });
            Assert.AreEqual(-1, (int)data[1]);
            Assert.AreEqual(-1, (int)view[1]);
        }

        [TestMethod]
        public void NestedView_1D_Span()
        {
            var data = np.arange(10);
            // return identical view
            var identical = data[":"];
            Assert.AreEqual(new Shape(10), new Shape(identical.shape));
            var view1 = identical["1:9"];
            Assert.AreEqual(new Shape(8), new Shape(view1.shape));
            AssertAreEqual(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, }, view1.Data<int>());
            var view2 = view1["4:"];
            Assert.AreEqual(new Shape(4), new Shape(view2.shape));
            AssertAreEqual(new int[] { 5, 6, 7, 8, }, view2.Data<int>());
            var view3 = view2[":2"];
            Assert.AreEqual(new Shape(2), new Shape(view3.shape));
            AssertAreEqual(new int[] { 5, 6 }, view3.Data<int>());
            // all must see the same modifications, no matter if original or any view is modified
            // modify original
            data.ReplaceData(new int[] { 0, -1, -2, -3, -4, -5, -6, -7, -8, -9 });
            AssertAreEqual(new int[] { -1, -2, -3, -4, -5, -6, -7, -8, }, view1.Data<int>());
            AssertAreEqual(new int[] { -5, -6, -7, -8, }, view2.Data<int>());
            AssertAreEqual(new int[] { -5, -6 }, view3.Data<int>());
            // modify views
            view1.SetData(55, 4);
            AssertAreEqual(new int[] { 0, -1, -2, -3, -4, 55, -6, -7, -8, -9 }, data.Data<int>());
            AssertAreEqual(new int[] { -1, -2, -3, -4, 55, -6, -7, -8, }, view1.Data<int>());
            AssertAreEqual(new int[] { 55, -6, -7, -8, }, view2.Data<int>());
            AssertAreEqual(new int[] { 55, -6 }, view3.Data<int>());
            view3.SetData(66, 1);
            AssertAreEqual(new int[] { 0, -1, -2, -3, -4, 55, 66, -7, -8, -9 }, data.Data<int>());
            AssertAreEqual(new int[] { -1, -2, -3, -4, 55, 66, -7, -8, }, view1.Data<int>());
            AssertAreEqual(new int[] { 55, 66, -7, -8, }, view2.Data<int>());
            AssertAreEqual(new int[] { 55, 66, }, view3.Data<int>());
        }

        [TestMethod]
        public void NestedView_1D_ViewStorage()
        {
            var data = np.arange(10);
            // return identical view
            var identical = data[ ":"];
            Assert.AreEqual(new Shape(10), new Shape(identical.shape));
            var view1 = identical[ "1:9"];
            Assert.AreEqual(new Shape(8), new Shape(view1.shape));
            AssertAreEqual(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, }, view1.Data<int>());
            var view2 = view1["::-2"];
            Assert.AreEqual(new Shape(4), new Shape(view2.shape));
            AssertAreEqual(new int[] { 8, 6, 4, 2, }, view2.Data<int>());
            var view3 = view2["::-3"];
            Assert.AreEqual(new Shape(2), new Shape(view3.shape));
            AssertAreEqual(new int[] { 2, 8 }, view3.Data<int>());
            // all must see the same modifications, no matter if original or any view is modified
            // modify original
            data.ReplaceData(new int[] { 0, -1, -2, -3, -4, -5, -6, -7, -8, -9 });
            AssertAreEqual(new int[] { -1, -2, -3, -4, -5, -6, -7, -8, }, view1.Data<int>());
            AssertAreEqual(new int[] { -8, -6, -4, -2, }, view2.Data<int>());
            AssertAreEqual(new int[] { -2, -8 }, view3.Data<int>());
            // modify views
            view1.SetData(88, 7);
            AssertAreEqual(new int[] { 0, -1, -2, -3, -4, -5, -6, -7, 88, -9 }, data.Data<int>());
            AssertAreEqual(new int[] { -1, -2, -3, -4, -5, -6, -7, 88, }, view1.Data<int>());
            AssertAreEqual(new int[] { 88, -6, -4, -2, }, view2.Data<int>());
            AssertAreEqual(new int[] { -2, 88 }, view3.Data<int>());
            view3.SetData(22, 0);
            AssertAreEqual(new int[] { 0, -1, 22, -3, -4, -5, -6, -7, 88, -9 }, data.Data<int>());
            AssertAreEqual(new int[] { -1, 22, -3, -4, -5, -6, -7, 88, }, view1.Data<int>());
            AssertAreEqual(new int[] { 88, -6, -4, 22, }, view2.Data<int>());
            AssertAreEqual(new int[] { 22, 88 }, view3.Data<int>());
        }

        [TestMethod]
        public void GetData_2D_Span()
        {
            //>>> x = np.arange(9).reshape(3, 3)
            //>>> x
            //array([[0, 1, 2],
            //       [3, 4, 5],
            //       [6, 7, 8]])
            //>>> x[:]
            //array([[0, 1, 2],
            //       [3, 4, 5],
            //       [6, 7, 8]])
            //>>> x[1:]
            //array([[3, 4, 5],
            //       [6, 7, 8]])
            //>>> x[1:,:]
            //array([[3, 4, 5],
            //       [6, 7, 8]])
            //>>> x[:, 1:]
            //array([[1, 2],
            //       [4, 5],
            //       [7, 8]])
            //>>> x[1:2, 0:1]
            //array([[3]])
            var data = np.arange(9).reshape(3, 3);
            Assert.AreEqual(new Shape(3, 3), new Shape(data.shape));
            // return identical view
            var view = data[ ":"];
            Assert.AreEqual(new Shape(3, 3), new Shape(view.shape));
            AssertAreEqual(data.Data<int>(), view.Data<int>());
            // return reduced view
            view = data[ "1:"];
            Assert.AreEqual(new Shape(2, 3), new Shape(view.shape));
            AssertAreEqual(new int[] { 3, 4, 5, 6, 7, 8 }, view.Data<int>());
            view = data[ "1:,:"];
            Assert.AreEqual(new Shape(2, 3), new Shape(view.shape));
            AssertAreEqual(new int[] { 3, 4, 5, 6, 7, 8 }, view.Data<int>());
        }

        [TestMethod]
        public void GetData_2D_ViewStorage()
        {
            var data = np.arange(9).reshape(3, 3);
            Assert.AreEqual(new Shape(3, 3), new Shape(data.shape));
            var view = data[ ":,1:"];
            Assert.AreEqual(new Shape(3, 2), new Shape(view.shape));
            AssertAreEqual(new int[] { 1, 2, 4, 5, 7, 8 }, view.Data<int>());
            view = data[ "1:2, 0:1"];
            Assert.AreEqual(new Shape(1, 1), new Shape(view.shape));
            AssertAreEqual(new int[] { 3 }, view.Data<int>());
            // return stepped view
            //>>> x
            //array([[0, 1, 2],
            //       [3, 4, 5],
            //       [6, 7, 8]])
            //>>> x[::2]
            //array([[0, 1, 2],
            //       [6, 7, 8]])
            //>>> x[::3]
            //array([[0, 1, 2]])
            //>>> x[::- 1]
            //array([[6, 7, 8],
            //       [3, 4, 5],
            //       [0, 1, 2]])
            //>>> x[::- 2]
            //array([[6, 7, 8],
            //       [0, 1, 2]])
            //>>> x[::- 3]
            //array([[6, 7, 8]])
            view = data[ "::2"];
            Assert.AreEqual(new Shape(2, 3), new Shape(view.shape));
            AssertAreEqual(new int[] { 0, 1, 2, 6, 7, 8 }, view.Data<int>());
            view = data[ "::3"];
            Assert.AreEqual(new Shape(1, 3), new Shape(view.shape));
            AssertAreEqual(new int[] { 0, 1, 2 }, view.Data<int>());
            view = data[ "::-1"];
            Assert.AreEqual(new Shape(3, 3), new Shape(view.shape));
            AssertAreEqual(new int[] { 6, 7, 8, 3, 4, 5, 0, 1, 2, }, view.Data<int>());
            view = data[ "::-2"];
            Assert.AreEqual(new Shape(2, 3), new Shape(view.shape));
            AssertAreEqual(new int[] { 6, 7, 8, 0, 1, 2, }, view.Data<int>());
            view = data[ "::-3"];
            Assert.AreEqual(new Shape(1, 3), new Shape(view.shape));
            AssertAreEqual(new int[] { 6, 7, 8, }, view.Data<int>());
            // N-Dim Stepping
            //>>> x[::2,::2]
            //array([[0, 2],
            //       [6, 8]])
            //>>> x[::- 1,::- 2]
            //array([[8, 6],
            //       [5, 3],
            //       [2, 0]])
            view = data["::2, ::2"];
            Assert.AreEqual(new Shape(2, 2), new Shape(view.shape));
            AssertAreEqual(new int[] { 0, 2, 6, 8 }, view.Data<int>());
            view = data[ "::-1, ::-2"];
            Assert.AreEqual(new Shape(3, 2), new Shape(view.shape));
            AssertAreEqual(new int[] { 8, 6, 5, 3, 2, 0 }, view.Data<int>());
        }

        [TestMethod]
        public void NestedView_2D()
        {
            var data = np.array(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            data=data.reshape(2, 10);
            //>>> x = np.array([0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9])
            //>>> x = x.reshape(2, 10)
            //>>> x
            //array([[0, 1, 2, 3, 4, 5, 6, 7, 8, 9],
            //       [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]])
            // return identical view
            var identical = data[ ":"];
            Assert.AreEqual(new Shape(2, 10), new Shape(identical.shape));
            //>>> x[:, 1:9]
            //array([[1, 2, 3, 4, 5, 6, 7, 8],
            //       [1, 2, 3, 4, 5, 6, 7, 8]])
            var view1 = identical[":,1:9"];
            Assert.AreEqual(new Shape(2, 8), new Shape(view1.shape));
            AssertAreEqual(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8 }, view1.Data<int>());
            //>>> x[:, 1:9][:,::- 2]
            //array([[8, 6, 4, 2],
            //       [8, 6, 4, 2]])
            var view2 = view1[":,::-2"];
            Assert.AreEqual(new Shape(2, 4), new Shape(view2.shape));
            AssertAreEqual(new int[] { 8, 6, 4, 2, 8, 6, 4, 2 }, view2.Data<int>());
            //>>> x[:, 1:9][:,::- 2][:,::- 3]
            //array([[2, 8],
            //       [2, 8]])
            var view3 = view2[":,::-3"];
            Assert.AreEqual(new Shape(2, 2), new Shape(view3.shape));
            AssertAreEqual(new int[] { 2, 8, 2, 8 }, view3.Data<int>());
            // all must see the same modifications, no matter if original or any view is modified
            // modify original
            data.ReplaceData(new int[] { 0, -1, -2, -3, -4, -5, -6, -7, -8, -9, 0, -1, -2, -3, -4, -5, -6, -7, -8, -9 });
            AssertAreEqual(new int[] { -1, -2, -3, -4, -5, -6, -7, -8, -1, -2, -3, -4, -5, -6, -7, -8 }, view1.Data<int>());
            AssertAreEqual(new int[] { -8, -6, -4, -2, -8, -6, -4, -2 }, view2.Data<int>());
            AssertAreEqual(new int[] { -2, -8, -2, -8 }, view3.Data<int>());
            // modify views
            view1.SetData(88, 0, 7);
            view1.SetData(888, 1, 7);
            AssertAreEqual(new int[] { 0, -1, -2, -3, -4, -5, -6, -7, 88, -9, 0, -1, -2, -3, -4, -5, -6, -7, 888, -9 },
                data.Data<int>());
            AssertAreEqual(new int[] { -1, -2, -3, -4, -5, -6, -7, 88, -1, -2, -3, -4, -5, -6, -7, 888 },
                view1.Data<int>());
            AssertAreEqual(new int[] { 88, -6, -4, -2, 888, -6, -4, -2 }, view2.Data<int>());
            AssertAreEqual(new int[] { -2, 88, -2, 888 }, view3.Data<int>());
            view3.SetData(22, 0, 0);
            view3.SetData(222, 1, 0);
            AssertAreEqual(new int[] { 0, -1, 22, -3, -4, -5, -6, -7, 88, -9, 0, -1, 222, -3, -4, -5, -6, -7, 888, -9 },
                data.Data<int>());
            AssertAreEqual(new int[] { -1, 22, -3, -4, -5, -6, -7, 88, -1, 222, -3, -4, -5, -6, -7, 888 },
                view1.Data<int>());
            AssertAreEqual(new int[] { 88, -6, -4, 22, 888, -6, -4, 222 }, view2.Data<int>());
            AssertAreEqual(new int[] { 22, 88, 222, 888 }, view3.Data<int>());
        }

        [TestMethod]
        public void Reduce_1D_to_Scalar()
        {
            var data = np.arange(10);
            Assert.AreEqual(new Shape(10), new Shape(data.shape));
            // return scalar
            var view = data[ "7"];
            Assert.AreEqual(new Shape(), new Shape(view.shape));
            AssertAreEqual(new int[] { 7 }, view.Data<int>());
        }

        [TestMethod]
        public void Reduce_2D_to_1D_and_0D_Span()
        {
            //>>> x = np.arange(9).reshape(3, 3)
            //>>> x
            //array([[0, 1, 2],
            //       [3, 4, 5],
            //       [6, 7, 8]])
            //>>> x[1]
            //array([3, 4, 5])
            //>>> x[2, 2]
            //8
            var data = np.arange(9).reshape(3, 3);
            Assert.AreEqual(new Shape(3, 3), new Shape(data.shape));
            // return identical view
            var view = data["1"];
            Assert.AreEqual(new Shape(3), new Shape(view.shape));
            AssertAreEqual(new int[] { 3, 4, 5 }, view.Data<int>());
            // return reduced view
            view = data["2,2"];
            Assert.AreEqual(new Shape(), new Shape(view.shape));
            AssertAreEqual(new int[] { 8 }, view.Data<int>());
            // recursive dimensionality reduction
            view = data["2"]["2"];
            Assert.AreEqual(new Shape(), new Shape(view.shape));
            AssertAreEqual(new int[] { 8 }, view.Data<int>());
        }

        [TestMethod]
        public void Reduce_2D_to_1D_and_0D_ViewStorage()
        {
            //>>> x = np.arange(9).reshape(3, 3)
            //>>> x
            //array([[0, 1, 2],
            //       [3, 4, 5],
            //       [6, 7, 8]])
            //>>> x[1]
            //array([3, 4, 5])
            //>>> x[:,1]
            //array([1, 4, 7])
            //>>> x[2, 2]
            //8
            var data = np.arange(9).reshape(3,3);
            Assert.AreEqual(new Shape(3, 3), new Shape(data.shape));
            // return identical view
            var view = data[ "1"];
            Assert.AreEqual(new Shape(3), new Shape(view.shape));
            AssertAreEqual(new int[] { 3, 4, 5 }, view.Data<int>());
            // return reduced view
            view = data[ ":,1"];
            Assert.AreEqual(new Shape(3), new Shape(view.shape));
            AssertAreEqual(new int[] { 1, 4, 7 }, view.Data<int>());
            view = data[ "2,2"];
            Assert.AreEqual(new Shape(), new Shape(view.shape));
            AssertAreEqual(new int[] { 8 }, view.Data<int>());
            // recursive dimensionality reduction
            view = data[ "2"]["2"];
            Assert.AreEqual(new Shape(), new Shape(view.shape));
            AssertAreEqual(new int[] { 8 }, view.Data<int>());
        }

        [TestMethod]
        public void NestedDimensionalityReduction_Span()
        {
            var data = np.arange(9).reshape(3, 3);
            Assert.AreEqual(new Shape(3, 3), new Shape(data.shape));
            var view = data["2"];
            Assert.AreEqual(new Shape(3), new Shape(view.shape));
            AssertAreEqual(new int[] { 6, 7, 8 }, view.Data<int>());
            var view1 = view["2"];
            Assert.AreEqual(new Shape(), new Shape(view1.shape));
            AssertAreEqual(new int[] { 8 }, view1.Data<int>());
            var view2 = view[":2:1"];
            Assert.AreEqual(new Shape(2), new Shape(view2.shape));
            AssertAreEqual(new int[] { 6,7 }, view2.Data<int>());
        }

        [TestMethod]
        public void NestedDimensionalityReduction_ViewStorage()
        {
            var data = np.arange(9).reshape(3, 3);
            Assert.AreEqual(new Shape(3, 3), new Shape(data.shape));
            var view = data[ "2"];
            Assert.AreEqual(new Shape(3), new Shape(view.shape));
            AssertAreEqual(new int[] { 6, 7, 8 }, view.Data<int>());
            var view1 = view["2"];
            Assert.AreEqual(new Shape(), new Shape(view1.shape));
            AssertAreEqual(new int[] { 8 }, view1.Data<int>());
            var view2 = view[":2:-1"];
            Assert.AreEqual(new Shape(2), new Shape(view2.shape));
            AssertAreEqual(new int[] { 7, 6 }, view2.Data<int>());
        }

    }
}
