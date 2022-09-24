using Moq;
using NumberSort;

namespace SortTest
{
    [TestClass]
    public class SortAlgoTest
    {
        [TestMethod]
        public void SortArray_WithNullArray_ReturnsNull()
        {
            var sortedArray = SortAlgo.SortArray(null, 0, 0);

            Assert.IsNull(sortedArray);
        }

        [TestMethod]
        public void SortArray_WithEmptyArray_ReturnsEmptyArray()
        {
            var sortedArray = SortAlgo.SortArray(new int[0], 0, 0);

            Assert.IsTrue(sortedArray.Length == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void SortArray_WithNegativeLeftIndex_ThrowsException()
        {
            var sortedArray = SortAlgo.SortArray(new int[1], -1, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void SortArray_WithNegativeRightIndex_ThrowsException()
        {
            var sortedArray = SortAlgo.SortArray(new int[1], 0, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void SortArray_WithLeftIndexMoreThanArrayLength_ThrowsException()
        {
            var sortedArray = SortAlgo.SortArray(new int[1], 3, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void SortArray_WithRightIndexMoreThanArrayLength_ThrowsException()
        {
            var sortedArray = SortAlgo.SortArray(new int[1], 0, 3);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void SortArray_WithLeftIndexEqualToArrayLength_ThrowsException()
        {
            var sortedArray = SortAlgo.SortArray(new int[1], 3, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void SortArray_WithRightIndexEqualToArrayLength_ThrowsException()
        {
            var sortedArray = SortAlgo.SortArray(new int[1], 0, 3);
        }

        [TestMethod]
        public void SortArray_WithThreeIndexArray_ThrowsException()
        {
            var array = new int[5] { 19, 73, 45, 2, 24 };

            var sortedArray = SortAlgo.SortArray(array, 0, array.Length - 1);

            Array.Sort(array);

            Assert.IsTrue(Array.Equals(array, sortedArray));
        }
    }
}