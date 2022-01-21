using FluentAssertions;
using Minesweeper;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace given_a_minesweeper_game.tests
{
    public class when_processing_move
    {
        public static IEnumerable<object[]> MoveTestData =>
                new List<object[]>
                {
           new object[] {
                10,
                10,

                new List<Coordinate>() {
                    new Coordinate(2, 4),
                    new Coordinate(1, 3)
                },
                new List<Coordinate>() {
                    new Coordinate(1, 1),
                    new Coordinate(1, 3)
                },
                new List<bool>() {
                    false,
                    true
                }
            }
        };

        [Theory]
        [MemberData(nameof(MoveTestData))]
        public void then_check_for_mine(
            int rowsEXP,
            int colsEXP,
            List<Coordinate> minesToGenerateEXP,
            List<Coordinate> pieceMovesEXP,
            List<bool> IsMineStates)
        {
            //ARRANGE
            Mock<MineManager> mineManagerMOCK = 
                new Mock<MineManager>(
                    new Mock<MineRandomiser>().Object);

            mineManagerMOCK.Setup(fn => fn.Generate(
                It.IsAny<int>(),
                It.IsAny<int>())).Returns(minesToGenerateEXP);
            var sut = new Board(rowsEXP, colsEXP,mineManagerMOCK.Object);

            int i = 0;
            var mineCountEXP = minesToGenerateEXP.Count();
            foreach (var cooordinate in pieceMovesEXP)
            {
                //ACT
                var isMineACT= sut.IsMine(cooordinate);
                isMineACT.Should().Be(IsMineStates[i++]);
                if (isMineACT)
                {
                    mineCountEXP--;
                }
            }
            //ASSERT
            sut.Mines.Count().Should().Be(mineCountEXP);
        }

        [Theory]
        [InlineData(10,9,true)]
        [InlineData(10, 8, false)]
        public void then_check_for_is_complete(
            int numberOfBoardRows,
            int coordinateRow,
            bool IsCompleteEXP)
        {
            //ARRANGE
            Mock<MineManager> mineManagerMOCK =
                 new Mock<MineManager>(new Mock<MineRandomiser>().Object);

            mineManagerMOCK.Setup(fn => fn.Generate(
                It.IsAny<int>(),
                It.IsAny<int>())).Returns(new List<Coordinate>{new Coordinate(2, 3)});

            var sut = new Board(numberOfBoardRows, 10, mineManagerMOCK.Object);

            //ACT
            var IsCompleteACT = sut.IsComplete(new Coordinate(coordinateRow, 1));

            //ASSERT
            IsCompleteEXP.Should().Be(IsCompleteACT);
        }
    }
}
