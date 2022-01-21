using FluentAssertions;
using Minesweeper;
using Minesweeper.Interfaces;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace given_a_minesweeper_game.tests
{
    public class when_configuring_the_board
    {
        public class BoardTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { -1, 10, new Mock<IMineManager>() };
                yield return new object[] { 1, 0, new Mock<IMineManager>() };
                yield return new object[] { 0, 1, new Mock<IMineManager>() };
                yield return new object[] { 1, -2, new Mock<IMineManager>() };
                yield return new object[] { 1, -2, null };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Fact]
        public void then_initialise_board()
        {
            //ARRANGE
            int rowsEXP = 4;
            int colsEXP = 5;
            var minesEXP = new List<Coordinate>() {
                   new Coordinate(2,4),
                   new Coordinate(1,3)
            };
            Mock<IMineManager> gameConfigMOCK = new Mock<IMineManager>();
            gameConfigMOCK.Setup(fn => 
                fn.Generate(It.IsAny<int>(),It.IsAny<int>()))
                    .Returns(minesEXP);

            //ACT
            var board = new Board(
                rowsEXP, colsEXP,
                gameConfigMOCK.Object);

            //ASSERT
            board.Rows.Should().Be(rowsEXP);
            board.Cols.Should().Be(colsEXP);
            board.Mines.Should().BeEquivalentTo(minesEXP);
            board.NumberOfMines.Should().Be(minesEXP.Count());
            gameConfigMOCK.Verify(fn => 
                fn.Generate(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
        }

        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[] { -1, 10, new Mock<IMineManager>()},
                new object[] { 1, 0, new Mock<IMineManager>() },
                new object[] { 0, 1, new Mock<IMineManager>() },
                new object[] { 1, -2, new Mock<IMineManager>() },
                new object[] { 1, 1, null }
            };

        [Theory]
        [MemberData(nameof(Data))]
        public void then_detect_errors_on_initialisation(
            int rows,
            int cols,
            Mock<IMineManager> mock)
        {
            //ARRANGE + ACT
            Action a = () => new Board(
                rows,
                cols,
                mock?.Object);

            //ASSERT
            a.Should().Throw<Exception>();
        }

    }
}
