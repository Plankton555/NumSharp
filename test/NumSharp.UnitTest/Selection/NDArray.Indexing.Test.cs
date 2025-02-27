﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumSharp;
using NumSharp.Generic;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;

namespace NumSharp.UnitTest.Selection
{
    [TestClass]
    public class IndexingTest : TestClass
    {
        [TestMethod]
        public void IndexAccessorGetter()
        {
            var nd = np.arange(12).reshape(3, 4);

            Assert.IsTrue(nd.Data<int>(1, 1) == 5);
            Assert.IsTrue(nd.Data<int>(2, 0) == 8);
        }

        [TestMethod]
        public void NDArrayAccess()
        {
            var nd = np.arange(4).reshape(2, 2);

            var row1 = (nd[0] as NDArray).MakeGeneric<int>();
            Assert.AreEqual(row1[0], 0);
            Assert.AreEqual(row1[1], 1);
        }

        [TestMethod]
        public void NDArrayAccess3Dim()
        {
            NDArray nd = np.arange(1, 18, 1).reshape(3, 3, 2);
            var row1 = (nd[0] as NDArray).MakeGeneric<int>();
            Assert.AreEqual(row1[0, 0], 1);
            Assert.AreEqual(row1[0, 1], 2);
            Assert.AreEqual(row1[1, 0], 3);
            Assert.AreEqual(row1[1, 1], 4);
            Assert.AreEqual(row1[2, 0], 5);
            Assert.AreEqual(row1[2, 1], 6);
        }

        [TestMethod]
        public void IndexAccessorSetter()
        {
            var nd = np.arange(12).reshape(3, 4);

            Assert.IsTrue(nd.Data<int>(0, 3) == 3);
            Assert.IsTrue(nd.Data<int>(1, 3) == 7);

            // set value
            nd.SetData(10, 0, 0);
            Assert.IsTrue(nd.Data<int>(0, 0) == 10);
            Assert.IsTrue(nd.Data<int>(1, 3) == 7);
        }

        [TestMethod]
        public void BoolArray()
        {
            NDArray A = new double[] {1, 2, 3};

            NDArray booleanArr = new bool[] {false, false, true};

            A[booleanArr.MakeGeneric<bool>()] = 1;

            Assert.IsTrue(System.Linq.Enumerable.SequenceEqual(A.Data<double>(), new double[] {1, 2, 1}));

            A = new double[,] {{1, 2, 3}, {4, 5, 6}};

            booleanArr = new bool[,] {{true, false, true}, {false, true, false}};

            A[booleanArr.MakeGeneric<bool>()] = -2;

            Assert.IsTrue(System.Linq.Enumerable.SequenceEqual(A.Data<double>(), new double[] {-2, 2, -2, 4, -2, 6}));
        }

        [TestMethod]
        public void Compare()
        {
            NDArray A = new double[,] {{1, 2, 3}, {4, 5, 6}};

            var boolArr = A < 3;
            Assert.IsTrue(Enumerable.SequenceEqual(boolArr.Data<bool>(), new[] {true, true, false, false, false, false}));

            A[A < 3] = -2;
            Assert.IsTrue(Enumerable.SequenceEqual(A.Data<double>(), new double[] {-2, -2, 3, 4, 5, 6}));

            var a = A[A == -2 | A > 5];

            Assert.IsTrue(Enumerable.SequenceEqual(a.Data<double>(), new double[] {-2, -2, 6}));
        }

        [TestMethod]
        public void NDArrayByNDArray()
        {
            NDArray x = new double[] {1, 2, 3, 4, 5, 6};

            NDArray index = new int[] {1, 3, 5};

            NDArray selected = x[index];

            double[] a = (System.Array)selected as double[];
            double[] b = {2, 4, 6};

            Assert.IsTrue(Enumerable.SequenceEqual(a, b));
        }

        [TestMethod]
        public void Filter1D()
        {
            var nd = np.array(new int[] {3, 1, 1, 2, 3, 1});
            var filter = np.array(new int[] {0, 2, 5});
            var result = nd[filter];

            Assert.IsTrue(Enumerable.SequenceEqual(new int[] {3, 1, 1}, result.Data<int>()));
        }

        [TestMethod]
        public void Filter2D()
        {
            var nd = np.array(new int[][] {new int[] {3, 1, 1, 2}, new int[] {1, 2, 2, 3}, new int[] {2, 1, 1, 3},});
            var filter = np.array(new int[] {0, 2});
            var result = nd[filter];

            Assert.IsTrue(Enumerable.SequenceEqual(new int[] {3, 1, 1, 2}, (result[0] as NDArray).Data<int>()));
            Assert.IsTrue(Enumerable.SequenceEqual(new int[] {2, 1, 1, 3}, (result[1] as NDArray).Data<int>()));

            var x = nd[1];
            x.ravel();
        }

        [TestMethod]
        public void Slice1()
        {
            var x = np.arange(5);
            var y1 = x["1:3"];
            AssertAreEqual(y1.Data<int>(), new int[] {1, 2});

            var y2 = x["3:"];
            AssertAreEqual(y2.Data<int>(), new int[] {3, 4});
            y2[0] = 8;
            y2[1] = 9;
            Assert.AreEqual((int)y2[0], 8);
        }


        [TestMethod]
        public void Slice2()
        {
            //>>> x = np.arange(5)
            //        >>> x
            //array([0, 1, 2, 3, 4])
            //    >>> y = x[0:5]
            //    >>> y
            //array([0, 1, 2, 3, 4])
            var x = np.arange(5);
            var y1 = x["0:5"];
            AssertAreEqual(y1.Data<int>(), new int[] {0, 1, 2, 3, 4});
            y1 = x["1:4"];
            AssertAreEqual(y1.Data<int>(), new int[] {1, 2, 3});
            //    >>> z = x[:]
            //    >>> z
            //array([0, 1, 2, 3, 4])
            var y2 = x[":"];
            AssertAreEqual(y2.Data<int>(), new int[] {0, 1, 2, 3, 4});

            // out of bounds access is handled gracefully by numpy
            //    >>> y = x[0:77]
            //    >>> y
            //array([0, 1, 2, 3, 4])
            var y3 = x["0:77"];
            AssertAreEqual(y3.Data<int>(), new int[] {0, 1, 2, 3, 4});

            //    >>> y = x[-77:]
            //    >>> y
            //array([0, 1, 2, 3, 4])
            var y4 = x["-77:"];
            AssertAreEqual(y4.Data<int>(), new int[] {0, 1, 2, 3, 4});
            var y = x["-77:77"];
            AssertAreEqual(y.Data<int>(), new int[] {0, 1, 2, 3, 4});
        }

        [TestMethod]
        public void Slice3()
        {
            //>>> x = np.arange(6)
            //>>> x
            //array([0, 1, 2, 3, 4, 5])
            //>>> y = x[1:5]
            //>>> y
            //array([1, 2, 3, 4])
            //>>> z = y[:3]
            //>>> z
            //array([1, 2, 3])
            //>>> z[0] = 99
            //>>> y
            //array([99, 2, 3, 4])
            //>>> x
            //array([0, 99, 2, 3, 4, 5])
            //>>>
            var x = np.arange(6);
            var y = x["1:5"];
            AssertAreEqual(new int[] {1, 2, 3, 4,}, y.Data<int>());
            var z = y[":3"];
            AssertAreEqual(new int[] {1, 2, 3}, z.Data<int>());
            z[0] = 99;
            AssertAreEqual(new int[] {99, 2, 3, 4}, y.Data<int>());
            AssertAreEqual(new int[] {0, 99, 2, 3, 4, 5}, x.Data<int>());
        }

        [TestMethod]
        public void Slice4()
        {
            //>>> x = np.arange(5)
            //>>> x
            //array([0, 1, 2, 3, 4])
            var x = np.arange(5);
            //>>> y = x[2:4]
            //>>> y
            //array([2,3])
            var y = x["2:4"];
            Assert.AreEqual(2, (int)y[0]);
            Assert.AreEqual(3, (int)y[1]);
            y[0] = 77;
            y[1] = 99;
            Assert.AreEqual(77, (int)x[2]);
            Assert.AreEqual(99, (int)x[3]);
        }

        [TestMethod]
        public void Slice2x2Mul()
        {
            //>>> import numpy as np
            //>>> x = np.arange(4).reshape(2, 2)
            //>>> y = x[1:]
            //>>> x
            //array([[0, 1],
            //       [2, 3]])
            //>>> y
            //array([[2, 3]])
            //>>> y*=2
            //>>> y
            //array([[4, 6]])
            var x = np.arange(4).reshape(2, 2);
            var y = x["1:"]; // slice a row as 1D array
            Assert.AreEqual(new Shape(1, 2), new Shape(y.shape));
            AssertAreEqual(y.Data<int>(), new int[] {2, 3});
            y *= 2;
            AssertAreEqual(y.Data<int>(), new int[] {4, 6});
        }

        [TestMethod]
        public void Slice2x2Mul_2()
        {
            //>>> import numpy as np
            //>>> x = np.arange(4).reshape(2, 2)
            //>>> y = x[1:]
            //>>> x
            //array([[0, 1],
            //       [2, 3]])
            //>>> y
            //array([[2, 3]])
            //>>> y*=2
            //>>> y
            //array([[4, 6]])
            var x = np.arange(4).reshape(2, 2);
            var y = x["1"]; // slice a row as 1D array
            Assert.AreEqual(new Shape(2), new Shape(y.shape));
            AssertAreEqual(y.Data<int>(), new int[] {2, 3});
            y *= 2;
            AssertAreEqual(y.Data<int>(), new int[] {4, 6});
            //AssertAreEqual(x.Data<int>(), new int[] { 0, 1, 4, 6 });
        }

        [TestMethod]
        public void Slice2x2Mul_3()
        {
            var x = np.arange(4).reshape(2, 2);
            var y = x[":,1"]; // slice a column as 1D array (shape 2)
            Assert.AreEqual(new Shape(2), new Shape(y.shape));
            AssertAreEqual(y.Data<int>(), new int[] {1, 3});
            y *= 2;
            AssertAreEqual(y.Data<int>(), new int[] {2, 6});
        }

        [Ignore("This can never work because C# doesn't allow overloading of the assignment operator")]
        [TestMethod]
        public void Slice2x2Mul_AssignmentChangesOriginal()
        {
            //>>> import numpy as np
            //>>> x = np.arange(4).reshape(2, 2)
            //>>> y = x[1:]
            //>>> x
            //array([[0, 1],
            //       [2, 3]])
            //>>> y
            //array([[2, 3]])
            //>>> y*=2
            //>>> y
            //array([[4, 6]])
            //>>> x
            //array([[0, 1],
            //       [4, 6]])
            var x = np.arange(4).reshape(2, 2);
            var y = x["1"]; // slice a row as 1D array
            Assert.AreEqual(new Shape(2), new Shape(y.shape));
            AssertAreEqual(y.Data<int>(), new int[] {2, 3});
            y *= 2;
            AssertAreEqual(y.Data<int>(), new int[] {4, 6});
            AssertAreEqual(x.Data<int>(), new int[] {0, 1, 4, 6}); // <------- this fails because in C# we can not intercept assignment to a variable
        }

        [TestMethod]
        public void Slice5()
        {
            var x = np.arange(6).reshape(3, 2);
            var y = x[":,0"];
            AssertAreEqual(new int[] {0, 2, 4,}, y.Data<int>());
            var z = x["1,:"];
            AssertAreEqual(new int[] {2, 3}, z.Data<int>());
            z[0] = 99;
            AssertAreEqual(new int[] {99, 3}, z.Data<int>());
            AssertAreEqual(new int[] {0, 99, 4}, y.Data<int>());
            AssertAreEqual(new int[] {0, 1, 99, 3, 4, 5}, x.Data<int>());
        }

        [TestMethod]
        public void Slice_Step()
        {
            //>>> x = np.arange(5)
            //>>> x
            //array([0, 1, 2, 3, 4])
            var x = np.arange(5);
            //>>> y = x[::-1]
            //>>> y
            //array([4, 3, 2, 1, 0])
            var y = x["::-1"];
            AssertAreEqual(y.Data<int>(), new int[] {4, 3, 2, 1, 0});

            //>>> y = x[::2]
            //>>> y
            //array([0, 2, 4])
            y = x["::2"];
            AssertAreEqual(y.Data<int>(), new int[] {0, 2, 4});
        }

        [TestMethod]
        public void Slice_Step1()
        {
            //>>> x = np.arange(6)
            //>>> x
            //array([0, 1, 2, 3, 4, 5])
            //>>> y = x[::- 1]
            //>>> y
            //array([5, 4, 3, 2, 1, 0])
            //>>> y[0] = 99
            //>>> x
            //array([0, 1, 2, 3, 4, 99])
            //>>> y
            //array([99, 4, 3, 2, 1, 0])
            //>>> y = x[::-1]
            //>>> y
            //array([5, 4, 3, 2, 1, 0])
            var x = np.arange(6);
            var y = x["::-1"];
            y[0] = 99;
            AssertAreEqual(new int[] {0, 1, 2, 3, 4, 99}, x.Data<int>());
            AssertAreEqual(new int[] {99, 4, 3, 2, 1, 0}, y.Data<int>());
            //>>> z = y[::2]
            //>>> z
            //array([99, 3, 1])
            //>>> z[1] = 111
            //>>> x
            //array([0, 1, 2, 111, 4, 99])
            //>>> y
            //array([99, 4, 111, 2, 1, 0])
            var z = y["::2"];
            AssertAreEqual(new int[] {99, 3, 1}, z.Data<int>());
            z[1] = 111;
            AssertAreEqual(new int[] {99, 111, 1}, (int[])z);
            AssertAreEqual(new int[] {0, 1, 2, 111, 4, 99}, x.Data<int>());
            AssertAreEqual(new int[] {99, 4, 111, 2, 1, 0}, y.Data<int>());
        }

        [TestMethod]
        public void Slice_Step2()
        {
            //>>> x = np.arange(5)
            //>>> x
            //array([0, 1, 2, 3, 4])
            var x = np.arange(5);
            //>>> y = x[::2]
            //>>> y
            //array([0, 2, 4])
            var y = x["::2"];
            Assert.AreEqual(0, (int)y[0]);
            Assert.AreEqual(2, (int)y[1]);
            Assert.AreEqual(4, (int)y[2]);
        }

        [TestMethod]
        public void Slice_Step3()
        {
            var x = np.arange(5);
            Assert.AreEqual("array([0, 1, 2, 3, 4])", x.ToString());
            var y = x["::2"];
            Assert.AreEqual("array([0, 2, 4])", y.ToString());
        }

        [TestMethod]
        public void Slice_Step_With_Offset()
        {
            //>>> x = np.arange(9).astype(np.uint8)
            //>>> x
            //array([0, 1, 2, 3, 4, 5, 6, 7, 8])
            var x = np.arange(9).astype(np.uint8);

            //>>> y = x[::3]
            //>>> y
            //array([0, 3, 6], dtype=uint8)
            var y0 = x["::3"];
            AssertAreEqual(new byte[] {0, 3, 6}, y0.Data<byte>());

            //>>> y = x[1::3]
            //>>> y
            //array([1, 4, 7], dtype=uint8)
            var y1 = x["1::3"];
            AssertAreEqual(new byte[] {1, 4, 7}, y1.Data<byte>());

            //>>> y = x[2::3]
            //>>> y
            //array([2, 5, 8], dtype=uint8)
            var y2 = x["2::3"];
            AssertAreEqual(new byte[] {2, 5, 8}, y2.Data<byte>());

            //>>> y = x[3::3]
            //>>> y
            //array([3, 6], dtype=uint8)
            var y3 = x["3::3"];
            AssertAreEqual(new byte[] {3, 6}, y3.Data<byte>());
        }


        [TestMethod]
        public void Slice3x2x2()
        {
            //>>> x = np.arange(12).reshape(3, 2, 2)
            //>>> x
            //array([[[0, 1],
            //        [ 2,  3]],
            //
            //       [[ 4,  5],
            //        [ 6,  7]],
            //
            //       [[ 8,  9],
            //        [10, 11]]])
            //>>> y1 = x[1:]
            //>>> y1
            //array([[[ 4,  5],
            //        [ 6,  7]],
            //
            //       [[ 8,  9],
            //        [10, 11]]])

            var x = np.arange(12).reshape(3, 2, 2);
            var y1 = x["1:"];
            Assert.IsTrue(Enumerable.SequenceEqual(y1.shape, new int[] {2, 2, 2}));
            Assert.IsTrue(Enumerable.SequenceEqual(y1.Data<int>(), new int[] {4, 5, 6, 7, 8, 9, 10, 11}));
            Assert.IsTrue(Enumerable.SequenceEqual(y1[0, 1].Data<int>(), new int[] {6, 7}));

            var y1_0 = y1[0];
            Assert.IsTrue(Enumerable.SequenceEqual(y1_0.shape, new int[] {2, 2}));
            Assert.IsTrue(Enumerable.SequenceEqual(y1_0.Data<int>(), new int[] {4, 5, 6, 7}));

            // change view
            y1[0, 1] = new int[] {100, 101};
            Assert.IsTrue(Enumerable.SequenceEqual(x.Data<int>(), new int[] {0, 1, 2, 3, 4, 5, 100, 101, 8, 9, 10, 11}));
            Assert.IsTrue(Enumerable.SequenceEqual(y1.Data<int>(), new int[] {4, 5, 100, 101, 8, 9, 10, 11}));

            var y2 = x["2:"];
            Assert.IsTrue(Enumerable.SequenceEqual(y2.shape, new int[] {1, 2, 2}));
            Assert.IsTrue(Enumerable.SequenceEqual(y2.Data<int>(), new int[] {8, 9, 10, 11}));
        }

        [TestMethod]
        public void AssignGeneric1DSlice1()
        {
            //>>> x = np.arange(5)
            //>>> y1 = np.arange(5, 8)
            //>>> y2 = np.arange(10, 13)
            //>>> x
            //array([0, 1, 2, 3, 4])
            //>>>
            //>>> xS1 = x[1:4]
            //>>> xS1[0] = y1[0]
            //>>> xS1[1] = y1[1]
            //>>> xS1[2] = y1[2]
            //>>>
            //>>> xS1
            //array([5, 6, 7])
            //>>> x
            //array([0, 5, 6, 7, 4])
            //>>>
            //>>> xS2 = x[1:-1]
            //>>> xS2[:] = y2
            //>>>
            //>>> xS2
            //array([10, 11, 12])
            //>>> x
            //array([0, 10, 11, 12, 4])
            //>>>

            var x = np.arange(5).MakeGeneric<int>();
            var y1 = np.arange(5, 8).MakeGeneric<int>();
            var y2 = np.arange(10, 13).MakeGeneric<int>();

            AssertAreEqual(new int[] {0, 1, 2, 3, 4}, x.Data<int>());

            var xS1 = x["1:4"];
            xS1[0] = y1[0];
            xS1[1] = y1[1];
            xS1[2] = y1[2];

            AssertAreEqual(new int[] {5, 6, 7}, xS1.Data<int>());
            AssertAreEqual(new int[] {0, 5, 6, 7, 4}, x.Data<int>());

            var xS2 = x[new Slice(1, -1)];
            xS2[":"] = y2;

            AssertAreEqual(new int[] {10, 11, 12}, xS2.Data<int>());
            AssertAreEqual(new int[] {0, 10, 11, 12, 4}, x.Data<int>());
        }

        [TestMethod]
        public void AssignGeneric1DSliceWithStepAndOffset1()
        {
            //>>> x = np.arange(9).astype(np.uint16)
            //>>> x
            //array([0, 1, 2, 3, 4, 5, 6, 7, 8], dtype = uint16)
            var x = np.arange(9).astype(np.uint16).MakeGeneric<ushort>();

            //>>> yS1 = np.arange(10, 13).astype(np.uint16)
            //>>> yS1
            //array([10, 11, 12], dtype = uint16)
            var yS0 = np.array<ushort>(new ushort[] {10, 11, 12}).MakeGeneric<ushort>();

            //>>> y0 = x[::3]
            //>>> y0
            //array([0, 3, 6], dtype = uint16)
            var y0 = x["::3"];
            AssertAreEqual(new ushort[] {0, 3, 6}, y0.Data<ushort>());

            //>>> x[::3] = yS0
            //>>> y0
            //array([10, 11, 12], dtype = uint16)
            x["::3"] = yS0;
            AssertAreEqual(new ushort[] {10, 11, 12}, y0.Data<ushort>());
            //>>> x
            //array([10, 1, 2, 11, 4, 5, 12, 7, 8], dtype = uint16)
            AssertAreEqual(new ushort[] {10, 1, 2, 11, 4, 5, 12, 7, 8}, x.Data<ushort>());

            //>>> x[1::3] = yS
            //>>> x
            //array([10, 10, 2, 11, 11, 5, 12, 12, 8], dtype = uint16)
            x["1::3"] = yS0;
            AssertAreEqual(new ushort[] {10, 10, 2, 11, 11, 5, 12, 12, 8}, x.Data<ushort>());
        }

        [TestMethod]
        public void AssignGeneric2DSlice1()
        {
            //>>> x = np.arange(9).reshape(3, 3)
            //>>> y1 = np.arange(6, 9)
            //>>> y2 = np.arange(12, 15)
            //>>>
            //>>> x
            //array([[0, 1, 2],
            //       [3, 4, 5],
            //       [6, 7, 8]])
            //>>>
            //>>> xS1 = x[1]
            //>>> xS1[0] = y1[0]
            //>>> xS1[1] = y1[1]
            //>>> xS1[2] = y1[2]
            //>>>
            //>>> xS1
            //array([6, 7, 8])
            //>>> x
            //array([[0, 1, 2],
            //       [6, 7, 8],
            //       [6, 7, 8]])
            //>>>
            //>>> xS2 = x[1:-1]
            //>>> xS2[:] = y2
            //>>>
            //>>> xS2
            //array([[12, 13, 14]])
            //>>> x
            //array([[ 0,  1,  2],
            //       [12, 13, 14],
            //       [ 6,  7,  8]])

            var x = np.arange(9).reshape(3, 3).MakeGeneric<int>();
            var y1 = np.arange(6, 9).MakeGeneric<int>();
            var y2 = np.arange(12, 15).MakeGeneric<int>();

            AssertAreEqual(new int[] {0, 1, 2, 3, 4, 5, 6, 7, 8}, x.Data<int>());

            var xS1 = x["1"];
            xS1[0] = y1[0];
            xS1[1] = y1[1];
            xS1[2] = y1[2];

            AssertAreEqual(new int[] {6, 7, 8}, xS1.Data<int>());
            AssertAreEqual(new int[] {0, 1, 2, 6, 7, 8, 6, 7, 8}, x.Data<int>());

            var xS2 = x[new Slice(1, -1)];
            xS2[":"] = y2;

            AssertAreEqual(new int[] {12, 13, 14}, xS2.Data<int>());
            AssertAreEqual(new int[] {0, 1, 2, 12, 13, 14, 6, 7, 8}, x.Data<int>());
        }


        [TestMethod]
        public void SliceNotation()
        {
            // items start through stop-1
            Assert.AreEqual("1:3", new Slice("1:3").ToString());
            Assert.AreEqual("-5:-8", new Slice("-5:-8").ToString());
            Assert.AreEqual("3:4", new Slice(3, 4).ToString());
            Assert.AreEqual("7:8:9", new Slice(7, 8, 9).ToString());
            Assert.AreEqual("7:8:9", new Slice("7:8:9").ToString());

            // items start through the rest of the array
            Assert.AreEqual("1:", new Slice("1:").ToString());
            Assert.AreEqual("1:", new Slice(1).ToString());
            Assert.AreEqual("1:", new Slice(1, null).ToString());
            Assert.AreEqual("1:", new Slice(1, null, 1).ToString());
            Assert.AreEqual("7::9", new Slice(7, null, 9).ToString());
            Assert.AreEqual("7::9", new Slice("7::9").ToString());

            // items from the beginning through stop-1
            Assert.AreEqual(":2", new Slice(":2").ToString());
            Assert.AreEqual(":2", new Slice(null, 2).ToString());
            Assert.AreEqual(":2", new Slice(stop: 2).ToString());
            Assert.AreEqual(":7:9", new Slice(null, 7, 9).ToString());
            Assert.AreEqual(":7:9", new Slice(":7:9").ToString());

            // slice a view of the whole array or matrix
            Assert.AreEqual(":", new Slice(":").ToString());
            Assert.AreEqual(":", new Slice().ToString());
            Assert.AreEqual(":", new Slice(null, null).ToString());
            Assert.AreEqual(":", new Slice(null, null, 1).ToString());

            // step
            Assert.AreEqual("::-1", new Slice("::- 1").ToString());
            Assert.AreEqual("::2", new Slice(step: 2).ToString());
            Assert.AreEqual("::2", new Slice(null, null, 2).ToString());

            // pick exactly one item and reduce the dimension
            Assert.AreEqual("17", new Slice("17").ToString());
            // pick exactly one item but keep the dimension
            Assert.AreEqual("17:18", new Slice("17:18").ToString());


            // equality
            Assert.AreEqual(new Slice("::- 1"), new Slice("::- 1"));
            Assert.AreEqual(new Slice(":"), new Slice(":"));
            Assert.AreEqual(new Slice(":7:9"), new Slice(":7:9"));
            Assert.AreEqual(new Slice(":2"), new Slice(":2"));
            Assert.AreEqual(new Slice("7::9"), new Slice("7::9"));
            Assert.AreEqual(new Slice("7:8:9"), new Slice("7:8:9"));
            Assert.AreEqual(new Slice("17"), new Slice("17"));
            Assert.AreEqual(new Slice("-5:-8"), new Slice("-5:-8"));
            Assert.AreEqual(new Slice("-  5:- 8"), new Slice("-5:-8"));
            Assert.AreEqual(new Slice("+  5:+ 8"), new Slice("+5:8"));
            Assert.AreEqual(new Slice("        5:    8"), new Slice("+5:8"));
            Assert.AreEqual(new Slice("\r\n\t:\t  "), new Slice(":"));
            Assert.AreEqual(new Slice(":\t:\t    \t1"), new Slice(":"));
            Assert.AreEqual(new Slice("  : \t:\t    \t2  "), new Slice("::2"));

            // inequality
            Assert.AreNotEqual(new Slice(":"), new Slice("1:"));
            Assert.AreNotEqual(new Slice(":1"), new Slice("1:"));
            Assert.AreNotEqual(new Slice(":8:9"), new Slice(":7:9"));
            Assert.AreNotEqual(new Slice(":7:8"), new Slice(":7:9"));
            Assert.AreNotEqual(new Slice(":-2"), new Slice(":2"));
            Assert.AreNotEqual(new Slice("7::9"), new Slice("7::19"));
            Assert.AreNotEqual(new Slice("17::9"), new Slice("7::9"));
            Assert.AreNotEqual(new Slice("7:1:9"), new Slice("7::9"));
            Assert.AreNotEqual(new Slice("7:8:9"), new Slice("7:18:9"));
            Assert.AreNotEqual(new Slice("-5:-8"), new Slice("-5:-8:2"));

            // Create functions
            Assert.AreEqual(Slice.All(), new Slice(":"));
            Assert.AreEqual(Slice.Index(17), new Slice("17:18"));

            // invalid values
            Assert.ThrowsException<ArgumentException>(() => new Slice(""));
            Assert.ThrowsException<ArgumentException>(() => new Slice(":::"));
            Assert.ThrowsException<ArgumentException>(() => new Slice("x"));
            Assert.ThrowsException<ArgumentException>(() => new Slice("0.5:"));
            Assert.ThrowsException<ArgumentException>(() => new Slice("0.00008"));
            Assert.ThrowsException<ArgumentException>(() => new Slice("x:y:z"));
            Assert.ThrowsException<ArgumentException>(() => new Slice("209572048752047520934750283947529083475"));
            Assert.ThrowsException<ArgumentException>(() => new Slice("209572048752047520934750283947529083475:"));
            Assert.ThrowsException<ArgumentException>(() => new Slice(":209572048752047520934750283947529083475:2"));
            Assert.ThrowsException<ArgumentException>(() => new Slice("::209572048752047520934750283947529083475"));
        }

        [TestMethod]
        public void N_DimensionalSliceNotation()
        {
            var s = "1:3,-5:-8,7:8:9,1:,999,:,:1,7::9,:7:9,::-1,-5:-8,5:8";
            Assert.AreEqual(s, Slice.FormatSlices(Slice.ParseSlices(s)));
        }

        [TestMethod]
        public void Transpose10x10()
        {
            new Action(() =>
            {
                var array = np.arange(100).reshape(3, 3, 3);
                for (var i = 0; i < array.shape[0]; i++)
                {
                    for (var j = 0; j < array.shape[1]; j++)
                    {
                        Console.WriteLine(array[i, j].ToString());
                    }
                }
            }).Should().NotThrow("It has to run completely.");
        }

        /// <summary>
        /// Based on issue https://github.com/SciSharp/NumSharp/issues/293
        /// </summary>
        [TestMethod]
        public void CastingWhenSettingDifferentType()
        {
            NDArray output = np.zeros(5);
            double newValDouble = 2;
            int newValInt = 4;
            output[3] = newValDouble; // This works fine
            new Action(()=>output[4] = newValInt).Should().NotThrow<NullReferenceException>(); // throws System.NullReferenceException
            output.Array.GetValue(4).Should().Be(newValInt);
        }
    }
}
