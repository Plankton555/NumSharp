﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using NumSharp.Extensions;
using System.Linq;
using FluentAssertions;
using NumSharp;

namespace NumSharp.UnitTest
{
    [TestClass]
    public class NDStorageTest
    {
        //TODO! [TestMethod]
        public void Index()
        {
            var shape0 = new Shape(4, 3);

            int idx0 = shape0.GetIndexInShape(2, 1);

            Assert.IsTrue(idx0 == 4*2+1);
        }

        //TODO! [TestMethod]
        public void CheckIndexing()
        {
            var shape0 = new Shape(4, 3, 2);

            int[] strgDimSize = shape0.Strides;

            int index = shape0.GetIndexInShape(1, 2, 1);

            Assert.IsTrue(Enumerable.SequenceEqual(shape0.GetDimIndexOutShape(index), new int[] {1, 2, 1}));

            var rnd = new Randomizer();
            var randomIndex = new int[] {rnd.Next(0, 3), rnd.Next(0, 2), rnd.Next(0, 1)};

            int index1 = shape0.GetIndexInShape(randomIndex);
            Assert.IsTrue(Enumerable.SequenceEqual(shape0.GetDimIndexOutShape(index1), randomIndex));

            var shape1 = new Shape(2, 3, 4);

            index = shape1.GetIndexInShape(1, 2, 1);
            Assert.IsTrue(Enumerable.SequenceEqual(shape1.GetDimIndexOutShape(index), new int[] {1, 2, 1}));

            randomIndex = new int[] {rnd.Next(0, 1), rnd.Next(0, 2), rnd.Next(0, 3)};
            index = shape1.GetIndexInShape(randomIndex);
            Assert.IsTrue(Enumerable.SequenceEqual(shape1.GetDimIndexOutShape(index), randomIndex));

            randomIndex = new int[] {rnd.Next(1, 10), rnd.Next(1, 10), rnd.Next(1, 10)};

            var shape2 = new Shape(randomIndex);

            randomIndex = new int[] {rnd.Next(0, shape2.Dimensions[0]), rnd.Next(0, shape2.Dimensions[1]), rnd.Next(0, shape2.Dimensions[2])};

            index = shape2.GetIndexInShape(randomIndex);
            Assert.IsTrue(Enumerable.SequenceEqual(shape2.GetDimIndexOutShape(index), randomIndex));
        }

        //TODO! [TestMethod]
        public void CheckColRowSwitch()
        {
            var shape1 = new Shape(5);
            Assert.IsTrue(Enumerable.SequenceEqual(shape1.Strides, new int[] {1}));

            shape1.ChangeTensorLayout();
            Assert.IsTrue(Enumerable.SequenceEqual(shape1.Strides, new int[] {1}));

            var shape2 = new Shape(4, 3);
            Assert.IsTrue(Enumerable.SequenceEqual(shape2.Strides, new int[] {1, 4}));

            shape2.ChangeTensorLayout();
            Assert.IsTrue(Enumerable.SequenceEqual(shape2.Strides, new int[] {3, 1}));

            var shape3 = new Shape(2, 3, 4);
            Assert.IsTrue(Enumerable.SequenceEqual(shape3.Strides, new int[] {1, 2, 6}));

            shape3.ChangeTensorLayout();
            Assert.IsTrue(Enumerable.SequenceEqual(shape3.Strides, new int[] {12, 4, 1}));
        }

        /// <summary>
        ///     Based on issue https://github.com/SciSharp/NumSharp/issues/306
        /// </summary>
        [TestMethod]
        public void EqualityComparer()
        {
            Shape a = null;
            Shape b = null;

            (a == b).Should().BeTrue();
            (a == null).Should().BeTrue();
            (null == b).Should().BeTrue();

            a = 5;
            b = 4;
            (a != b).Should().BeTrue();

            b = 5;
            (a == b).Should().BeTrue();

            a = new Shape(1, 2, 3, 4, 5);
            b = new Shape(1, 2, 3, 4, 5);
            (a == b).Should().BeTrue();
            b = new Shape(1, 2, 3, 4);
            (a != b).Should().BeTrue();
        }
    }
}
