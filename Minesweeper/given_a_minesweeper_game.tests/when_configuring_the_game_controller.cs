using FluentAssertions;
using Minesweeper;
using Minesweeper.Interfaces;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace given_a_minesweeper_game.tests
{
    public class when_configuring_the_game_controller
    {
        private readonly Mock<IRandomise> _RandomGeneratorMOCK;
        private readonly Mock<IConsole> _ConsoleMOCK;
        private readonly Mock<MineManager> _MineManagerMOCK;
        private readonly Mock<Board> _BoardMOCK;
        public when_configuring_the_game_controller()
        {
            _RandomGeneratorMOCK =
                new Mock<IRandomise>();
            _ConsoleMOCK = new Mock<IConsole>();
            _MineManagerMOCK =
                new Mock<MineManager>(_RandomGeneratorMOCK.Object);

            _MineManagerMOCK.Setup(fn => fn.Generate(
                It.IsAny<int>(),
                It.IsAny<int>()))
                .Returns(new List<Coordinate>
                {
                    new Coordinate( It.IsAny<int>(), 
                    It.IsAny<int>())
                }); 

            _BoardMOCK = new Mock<Board>(8, 8, _MineManagerMOCK.Object);
        }

        [Fact]
        public void then_initialise_game_controller()
        {
            //ARRANGE
            var sut = new GameController(_ConsoleMOCK.Object,
                 _BoardMOCK.Object,
                  It.IsAny<Piece>());

            //ASSERT
            sut.Should().NotBeNull();
            _ConsoleMOCK.Verify(fn =>
                fn.WriteLine(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Fact]
        public void then_prompt_for_play_again()
        {
            //ARRANGE
            var playAgainsEXP = true;

            Mock<GameController> gameControllerMOCK =
                new Mock<GameController>(
                _ConsoleMOCK.Object,
                _BoardMOCK.Object,
                It.IsAny<Piece>());

            gameControllerMOCK.Setup(fn => fn.DisplayGameSummary(
                It.IsAny<bool>()));

            gameControllerMOCK.Setup(fn =>
                fn.PlayAgain()).Returns(playAgainsEXP);

            var sut = new MineSweeperGame(gameControllerMOCK.Object);

            //ACT
            var playAgainStatusACT = sut.Run();

            //ASSERT
            playAgainStatusACT.Should().BeTrue();
            gameControllerMOCK.Verify(fn =>
                fn.PlayAgain(), Times.Once);
            gameControllerMOCK.Verify(fn =>
                fn.Play(), Times.Once);
            gameControllerMOCK.Verify(fn =>
                fn.DisplayGameSummary(It.IsAny<bool>()), Times.Once);
        }

        public static IEnumerable<object[]> PlayAgainResponseData =>
         new List<object[]>
         {
            new object[] {"y",true },
            new object[] {"Y",true },
            new object[] {"N",false },
            new object[] {"N",false }
         };

        [Theory]
        [MemberData(nameof(PlayAgainResponseData))]
        public void get_play_again_status(
            string playAgainResponseEXP,
            bool playAgainStatusEXP)
        {
            //ARRANGE
            _ConsoleMOCK.Setup(fn => fn.ReadLine()).Returns(playAgainResponseEXP);
            GameController sut =
                  new GameController(
                  _ConsoleMOCK.Object,
                  _BoardMOCK.Object,
                  It.IsAny<Piece>());

            //ACT
            var playAgainStatusACT = sut.PlayAgain();

            //ASSERT
            playAgainStatusACT.Should().Be(playAgainStatusEXP);
            _ConsoleMOCK.Verify(fn => fn.ReadLine(), Times.Once);
        }

        [Fact]
        public void then_print_game_summary_status()
        {
            //ARRANGE
            Mock<Piece> pieceMOCK = new Mock<Piece>(3,3,2);
            _BoardMOCK.Setup(fn => fn.ToString()).Returns("board summary");

            GameController sut =
                  new GameController(
                  _ConsoleMOCK.Object,
                  _BoardMOCK.Object,
                  pieceMOCK.Object);

            //ACT
            sut.DisplayGameSummary(It.IsAny<bool>());

            //ASSERT
            _ConsoleMOCK.Verify(fn => fn.WriteLine(It.IsAny<string>()), Times.AtLeastOnce);
            pieceMOCK.Verify(fn => fn.ToString(), Times.Once);
            _BoardMOCK.Verify(fn => fn.ToString(), Times.Once);
        }

        public static IEnumerable<object[]> PlayData => new List<object[]>
        {
            new object[] {

                new Queue<bool>( new[]{true,true,false}),//IsMine
                new Queue<bool>( new[]{true,true,false,false}),//IsAlive
            }
        };

        [Theory]
        [MemberData(nameof(PlayData))]
        public void then_provide_play_capability(
           Queue<bool> isMineEXP,
            Queue<bool> isAliveEXP)
        {
            //ARRANGE
            string responseEXP = "U";
            int rowEXP = 1;
            int colEXP = 1;
            int rowACT = -1;
            int colACT= -1;
            string responseACT = "";
     
            int movesEXP = isMineEXP.Count;
            int numberOfHits = isMineEXP.Where(c => c == true).Count();

            Mock<Piece> pieceMOCK = new Mock<Piece>(rowEXP, colEXP, It.IsAny<int>());
            _ConsoleMOCK.Setup(fn => fn.ReadLine()).Returns(responseEXP);

            pieceMOCK.Setup(fn => fn.Move(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>()))
                .Callback<string, int, int>(
                    (keyboard, rows, cols) =>
                {
                    responseACT = keyboard;
                    rowACT = rows;
                    colACT = cols;
                });

            GameController sut =
                  new GameController(
                  _ConsoleMOCK.Object,
                  _BoardMOCK.Object,
                  pieceMOCK.Object);

            Mock<Coordinate> C = new Mock<Coordinate>(5,5);
            Coordinate coordinateACT = null;
 
            pieceMOCK.Setup(fn => fn.Alive).Returns(isAliveEXP.Dequeue);
            _BoardMOCK.Setup(fn => fn.IsMine(It.IsAny<Coordinate>()))
                .Returns(isMineEXP.Dequeue)
                .Callback<Coordinate>((c) => { coordinateACT = c; });

            //ACT
            sut.Play();

            //ASSERT
            rowACT.Should().Be( _BoardMOCK.Object.Rows);
            colACT.Should().Be(_BoardMOCK.Object.Cols);
            responseACT.Should().Be(responseEXP);
            coordinateACT.Row.Should().Be(rowEXP);
            coordinateACT.Col.Should().Be(colEXP);
           
            _ConsoleMOCK.Verify(fn => 
                fn.WriteLine(It.IsAny<string>()), Times.AtLeastOnce);
            pieceMOCK.Verify(fn =>
                fn.Move(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()),Times.Exactly(movesEXP));

            _BoardMOCK.Verify(fn => fn.IsMine(It.IsAny<Piece>()), Times.Exactly(movesEXP));
            pieceMOCK.Verify(fn => fn.Hit(), Times.Exactly(numberOfHits));
        }
    }
}
