using FluentAssertions;
using Minesweeper;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace given_a_minesweeper_game.tests
{
    public class PieceTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 
                new List<bool> { true, true, false },
                2
            };
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class CoordinatesTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                100,
                100,
                new Piece(0,0),
                "U",
                new Piece(1,0)};
            yield return new object[] {
                100,
                100,
                new Piece(0, 0),
                "d",
                new Piece(0,0)};
            yield return new object[] {
                100,
                100,
                new Piece(5, 0),
                "L",
                new Piece(5, 0)};
            yield return new object[] {
                100,
                100,
                new Piece(25,23),
                "L",
                new Piece(25, 22)};
            yield return new object[] {
                100,
                100,
                new Piece(25, 23),
                "R",
                new Piece(25, 24)};
            yield return new object[] {
                500,
                5100,
                new Piece(2, 223),
                "r",
                new Piece(2, 224)};
            yield return new object[] {
                500,
                500,
                new Piece(2, 223),
                "X",
                new Piece(2, 223) };
            yield return new object[] {
                10,
                10,
                new Piece(9,9),
                "u",
                 new Piece(9,9) };
            yield return new object[] {
                10,
                10,
                new Piece(8,9),
                "u",
                 new Piece(9,9) };
            yield return new object[] {
                10,
                10,
                new Piece(9,9),
                "r",
                 new Piece(9,9) };
            yield return new object[] {
                10,
                10,
                new Piece(9,8),
                "r",
                 new Piece(9,9) };
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class when_using_a_piece
    {
        [Fact]
        public void then_ensure_initialised()
        {
            //ARRANGE + ACT
            var piece = new Piece(0,0);

            //ASSERT
            piece.Alive.Should().BeTrue();
            piece.Lives.Should().Be(2);
            piece.Row.Should().Be(0);
            piece.Col.Should().Be(0);
        }

        [Fact]
        public void then_detect_errors_on_initialiation()
        {
            //ARRANGE + ACT
            Action a = () => new Piece(0,0,-1);

            //ASSERT
            a.Should().Throw<Exception>();
        }


        [Theory]
        [ClassData(typeof(CoordinatesTestData))]
        public void then_move_piece(
            int maxRows,
            int maxCols,
            Piece sut,
            string move,
            Coordinate destinationEXP)
        {
            //ARRANGE + ACT 
            sut.Move(move,maxRows,maxCols);

            //ASSERT
            sut.Should().BeEquivalentTo(destinationEXP);
        }

        [Fact]
        public void then_inflict_hit_on_explosion()
        {
            //ARRANGE + ACT
            Piece sut = new Piece(1, 1, 2);
            var livesEXP = sut.Lives;

            //ACT
            sut.Hit();

            //ASSERT
            livesEXP.Should().Be(sut.Lives + 1);

        }

        [Theory]
        [ClassData(typeof(PieceTestData))]
        public void then_check_if_alive(
            List<bool> aliveStatesEXP,
            int numberOfLivesEXP)
        {
           //ARRANGE
            Piece sut = new Piece(1, 1, numberOfLivesEXP);

            //ACT + ASSERT
            foreach(var aliveStateEXP in aliveStatesEXP)
            {
                sut.Hit();
                sut.Alive.Should().Be(aliveStateEXP);
            }
        }
    }
}
