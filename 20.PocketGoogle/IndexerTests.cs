using NUnit.Framework;
using PocketGoogle;
using System.Collections.Generic;
using System.Linq;
namespace PocketGoogleTests
{
    [TestFixture]
    public class IndexerTests
    {
        private Indexer indexer;
        [SetUp]
        public void Setup()
        {
            indexer = new Indexer();
        }

        [Test]
        public void GetPositions_WithRussianText_ReturnsExpectedPositions()
        {
            // Arrange
            string text = "Я, шел, по: мосткам, и вдруг – Там, в глубине потока, Сквозят водяные цветы.";
            indexer.Add(1, text);

            // Define test cases
            var testCases = new Dictionary<string, List<int>>
                {
                    { "Я", new List<int> { 0 } },
                    { "шел", new List<int> { 3 } },
                    { "по", new List<int> { 8 } },
                    { "мосткам", new List<int> { 12 } },
                    { "и", new List<int> { 21 } },
                    { "вдруг", new List<int> { 23 } },
                    { "Там", new List<int> { 31 } },
                    { "в", new List<int> { 36 } },
                    { "глубине", new List<int> { 38 } },
                    { "потока", new List<int> { 46 } },
                    { "Сквозят", new List<int> { 54 } },
                    { "водяные", new List<int> { 62 } },
                    { "цветы", new List<int> { 70 } }
                };

            foreach (var testCase in testCases)
            {
                // Act
                var positions = indexer.GetPositions(1, testCase.Key);

                // Assert
                Assert.AreEqual(testCase.Value, positions, $"Failed for word: {testCase.Key}");
            }
        }

        [Test]
        [TestCase("A", 2, new int[] { 0, 4 })]
        [TestCase("B", 0, new int[] { 2 })]
        [TestCase("C", 0, new int[] { 4 })]
        [TestCase("C", 2, new int[] { 2, 6 })]
        [TestCase("F", 3, new int[] { 0 })]
        public void GetPositions_ForEachLetter_ReturnsExpectedPositions(string word, int documentId, int[] expectedPositions)
        {
            // Arrange
            indexer.Add(0, "A B C");
            indexer.Add(1, "B C");
            indexer.Add(2, "A C A C");
            indexer.Add(3, "F, f ff");

            // Act
            var positions = indexer.GetPositions(documentId, word);

            // Assert
            Assert.AreEqual(expectedPositions, positions);
        }

        [Test]
        public void Add_WhenCalled_AddsWordsWithCorrectPositions()
        {
            // Arrange
            var indexer = new Indexer();
            int documentId = 1;
            string documentText = "Hello world!";

            // Act
            indexer.Add(documentId, documentText);

            // Assert
            var idsForHello = indexer.GetIds("Hello");
            Assert.Contains(documentId, idsForHello);
            var positionsForHello = indexer.GetPositions(documentId, "Hello");
            Assert.AreEqual(0, positionsForHello.First());

            var idsForWorld = indexer.GetIds("world");
            Assert.Contains(documentId, idsForWorld);
            var positionsForWorld = indexer.GetPositions(documentId, "world");
            Assert.AreEqual(6, positionsForWorld.First());
        }

        [Test]
        public void Add_WithMultipleDocuments_HandlesWordsCorrectly()
        {
            // Arrange
            var indexer = new Indexer();
            indexer.Add(1, "Hello world");
            indexer.Add(2, "world peace");

            // Assert
            var ids = indexer.GetIds("world");
            Assert.AreEqual(2, ids.Count);
            Assert.IsTrue(ids.Contains(1) && ids.Contains(2));
        }

        [Test]
        public void Add_WithRepeatingWordsInDocument_TracksAllPositionsCorrectly()
        {
            // Arrange
            var indexer = new Indexer();
            indexer.Add(1, "Hello Hello world");

            // Assert
            var positions = indexer.GetPositions(1, "Hello");
            Assert.AreEqual(2, positions.Count);
            Assert.IsTrue(positions.Contains(0) && positions.Contains(6));
        }

        [Test]
        public void Remove_RemovesSingleDocumentById()
        {
            // Arrange
            indexer.Add(1, "Hello world");
            indexer.Add(2, "Goodbye world");

            // Act
            indexer.Remove(1);

            // Assert
            Assert.IsFalse(indexer.GetIds("Hello").Contains(1), "Document ID 1 should have been removed.");
            Assert.IsTrue(indexer.GetIds("Goodbye").Contains(2), "Document ID 2 should still exist.");
        }

        [Test]
        public void Remove_KeepsWordsFromOtherDocuments()
        {
            // Arrange
            indexer.Add(1, "Shared word");
            indexer.Add(2, "Shared word");

            // Act
            indexer.Remove(1);

            // Assert
            var idsForSharedWord = indexer.GetIds("Shared");
            Assert.IsFalse(idsForSharedWord.Contains(1), "Document ID 1 should have been removed.");
            Assert.IsTrue(idsForSharedWord.Contains(2), "Document ID 2 should still be associated with 'Shared'.");
        }

        [Test]
        public void Remove_DoesNothingWhenIdDoesNotExist()
        {
            // Arrange
            indexer.Add(1, "Existing document");

            // Act
            indexer.Remove(999); // Non-existent ID

            // Assert
            Assert.IsTrue(indexer.GetIds("Existing").Contains(1), "Document ID 1 should still exist.");
        }
    }
}