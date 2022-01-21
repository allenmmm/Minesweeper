using FluentAssertions;
using Minesweeper;
using Minesweeper.Interfaces;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace given_a_minesweeper_game.tests
{
    public class when_playing_the_game
    {
        private readonly Mock<IConsole> _ConsoleMOCK;
        private readonly Mock<IRandomise> _RandomGeneratorMOCK;

        public when_playing_the_game()
        {
            _ConsoleMOCK = new Mock<IConsole>();
            _RandomGeneratorMOCK = new Mock<IRandomise>();
        }

        public static IEnumerable<object[]> GameTestData =>
           new List<object[]>
           {
                /*
                 *      Game Config
                 *      S = start, F = finish
                 *      
                        |X|X| | |F|
                        | | | |X| |
                        |S| | | | |
                */
                new object[] {                
                    new Queue<string>( new[]{"U","R","R","D","R","R","U","U"}),//key strokes
                    3,//rows
                    5,//cols
                    2,//lives
                    new Queue<Coordinate>(new[] {//random coordinates
                        new Coordinate(2,0),
                        new Coordinate(2,1),
                        new Coordinate(1,3)
                    }),
                    true// succeeded
                },
                /*
                 *      Game Config
                 *      S = start, F = finish
                 *      
                 *      | | | | | |
                 *      | |X|X| | |
                 *      |X| | | |X|  
                 *      | | |X| | |
                 *      |X| | | | |
                 *      |S| | | |X|
                 */
                new object[] {                      //X     //X             //X //X     //X         //X
                    new Queue<string>( new[]{"R","U","L","U","U","R","D","U","U","R","D","D","R","R","U"}),//key strokes
                    6,//rows
                    5,//cols
                    5,//lives
                    new Queue<Coordinate>(new[] {//random coordinates
                        new Coordinate(1,0),
                        new Coordinate(3,0),
                        new Coordinate(4,1),
                        new Coordinate(4,2),
                        new Coordinate(2,2),
                        new Coordinate(3,4),
                        new Coordinate(0,4)
                    }),
                    false// succeeded
                }
           };

        [Theory]
        [MemberData(nameof(GameTestData))]
        public void then_play(
            Queue<string> directions,
            int rows,
            int cols,
            int lives,
            Queue<Coordinate> mines,
            bool succeededEXP)
        {
            //ARRANGE
            var numMinesToCreate = mines.Count;
            _ConsoleMOCK.Setup(fn => fn.ReadLine()).Returns(directions.Dequeue);
            _RandomGeneratorMOCK.Setup(fn => fn.GenerateInt(It.IsAny<int>()))
                .Returns(numMinesToCreate);
            _RandomGeneratorMOCK.Setup(fn => fn.GenerateCoordinate(
                It.IsAny<int>(), It.IsAny<int>()))
                .Returns(mines.Dequeue);

            var board = new Board(rows, cols,
                    new MineManager(
                        _RandomGeneratorMOCK.Object));

            GameController sut = new GameController(
                _ConsoleMOCK.Object,
                board,
                new Piece(0, 0, lives));

            //ACT
            bool succeededACT = sut.Play();

            //ASSERT
            succeededACT.Should().Be(succeededEXP);
        }

        [Fact]
        public void then_retrieve_board_status()
        {
            //ARRANGE
            Mock<MineManager> mineManagerMock =
                new Mock<MineManager>(_RandomGeneratorMOCK.Object);

            var listEXP = new List<Coordinate>() {
                    new Coordinate(2,2),
                    new Coordinate(3,2),
                    new Coordinate(3,6),
                    new Coordinate(5,6),
                };

            mineManagerMock.Setup(fn => fn.Generate(
                It.IsAny<int>(),
                It.IsAny<int>()))
                    .Returns(listEXP);

            var sut = new Board(
                10, 10, mineManagerMock.Object);

            //ACT + ASSERT
            sut.ToString().Should().Be(
                $"Board Status - {listEXP.Count}/{listEXP.Count} mines remaining");
        }

        [Fact]
        public void then_retrieve_piece_status()
        {
            //ARRANGE
            int livesEXP = 3;
            var sut = new Piece(0, 0, livesEXP);

            //ACT + ASSERT
            sut.ToString().Should().Be(
                $"Piece Status - {livesEXP} lives remaining");
        }
    }
}
