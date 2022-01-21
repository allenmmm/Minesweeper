using FluentAssertions;
using Minesweeper;
using Minesweeper.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace given_a_minesweeper_game.tests
{
    public class when_configuring_mines
    {
        public static IEnumerable<object[]> BoardTestData =>
            new List<object[]>
            {
                new object[] {
                    5,
                    5,
                    new Queue<Coordinate>(new[] {
                        new Coordinate(2,4),
                        new Coordinate(2,4),
                        new Coordinate(1,3)
                    }),
                    new List<Coordinate>() {
                        new Coordinate(2,4),
                        new Coordinate(1,3)
                    },
                    3
                }
            };

        [Theory]
        [MemberData(nameof(BoardTestData))]
        public void then_retrieve_mines(
            int numberOfRowsEXP,
            int numberOfColsEXP,
            Queue<Coordinate> randomMinesEXP,
            List<Coordinate> minesEXP,
            int timesRandomCalledEXP)
        {
            //ARRANGE
             Mock<IRandomise> randomCoordinateGenMOCK
                 = new Mock<IRandomise>();
            int rowsACT = -1;
            int colsACT = -1;
            int totalNumberOfMinesToPrimeACT = -1;
            int totalNumberOfMinesToPrimeEXP = numberOfRowsEXP * numberOfColsEXP;

            randomCoordinateGenMOCK.Setup(fn => fn.GenerateCoordinate(
                It.IsAny<int>(),
                It.IsAny<int>()))
                    .Returns(randomMinesEXP.Dequeue)
                    .Callback<int, int>((rows, cols) =>
                    {
                        rowsACT = rows;
                        colsACT = cols;
                    });

            randomCoordinateGenMOCK.Setup(fn => fn.GenerateInt(
                It.IsAny<int>()))
                    .Returns(minesEXP.Count)
                    .Callback<int>((numberOfMines) =>
                    {
                        totalNumberOfMinesToPrimeACT = numberOfMines;
                    });

            IMineManager sut = new MineManager(
                randomCoordinateGenMOCK.Object);

            //ACT
            var primeMinesACT = sut.Generate(
                numberOfRowsEXP, numberOfColsEXP);

            //ASSERT
            primeMinesACT.Count.Should().Be(minesEXP.Count);
            primeMinesACT.Should().BeEquivalentTo(minesEXP);
            rowsACT.Should().Be(numberOfRowsEXP);
            colsACT.Should().Be(numberOfRowsEXP);
            totalNumberOfMinesToPrimeACT.Should().Be(totalNumberOfMinesToPrimeEXP);
            randomCoordinateGenMOCK.Verify(fn => fn.GenerateCoordinate(
                 It.IsAny<int>(), It.IsAny<int>()),
                 Times.Exactly(timesRandomCalledEXP));
            randomCoordinateGenMOCK.Verify(fn => fn.GenerateInt(
                 It.IsAny<int>()),Times.Once);
        }

        [Fact]
        public void then_generate_random_mine()
        {
            //ARRANGE
            int maxRowEXP = 10;
            int maxColEXP = 23;

            MineRandomiser sut = new MineRandomiser();

            //ACT
            var mineACT = sut.GenerateCoordinate(maxRowEXP, maxColEXP);

            //ASSERT
            mineACT.Col.Should().BeGreaterThanOrEqualTo(0)
                .And.BeLessThan(maxColEXP);
            mineACT.Row.Should().BeGreaterThanOrEqualTo(0)
                .And.BeLessThan(maxRowEXP);
        }

        [Fact]
        public void then_create_mine()
        {
            //ARRANGE
            int rowEXP = 10;
            int colEXP = 23;

            //ACT
            var sut = new Coordinate(rowEXP, colEXP);

            //ASSERT
            sut.Row.Should().Be(rowEXP);
            sut.Col.Should().Be(colEXP);
        }

        [Fact]
        public void then_get_the_random_number_of_mines_to_create()
        {
            //ARRANGE
            int numberOfMinesEXP = 3;
            MineRandomiser sut
                = new MineRandomiser();

            //ACT
            var numberToCreateACT = sut.GenerateInt(numberOfMinesEXP);

            //ASSERT
            numberToCreateACT.Should().BeGreaterThan(0)
                .And.BeLessThanOrEqualTo(numberOfMinesEXP);
        }

        [Theory]
        [InlineData(-1, 10)]
        [InlineData(10, -2)]
        public void then_detect_errors_on_initialiation(int rowEXP, int colEXP)
        {
            //ACT
            Action a = () => new Coordinate(rowEXP, colEXP);
            //ASSERT
            a.Should().Throw<Exception>();
        }
    }
}
