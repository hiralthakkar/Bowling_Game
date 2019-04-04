using System;
using Bowling;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bowling_TestProject
{
    [TestClass]
    public class BowlingGame_Test
    {
        Game game;

        #region Initialize and Cleanup
        [TestInitialize]
        public void Initialize()
        {
            game = new Game();
        }

        [TestCleanup]
        public void CleanUp()
        {
            game.Dispose();
            game = null;
        }
        #endregion

        #region Test methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Roll_throws_exception_when_trying_to_roll_more_than_allowed()
        {
            RollMany(20, 1);
            game.Roll(2);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Score_without_rolling_all_rolls_throws_exception()
        {
            RollMany(10, 0);
            game.Score();
        }

        [TestMethod]
        public void Score_for_all_gutter_rolls_results_in_gutter_game()
        {
            RollMany(20, 0);
            Assert.AreEqual(0, game.Score());
        }

        [TestMethod]
        public void Score_for_all_strikes_results_in_perfect_game()
        {
            RollMany(21, 10);
            Assert.AreEqual(300, game.Score());
        }

        [TestMethod]
        public void Score_for_all_ones_results_in_total_number_of_rolls()
        {
            RollMany(20, 1);
            Assert.AreEqual(20, game.Score());
        }

        [TestMethod]
        public void Score_for_one_spare_with_other_gutters_matches_expected_score()
        {
            RollOneSpare();
            game.Roll(6);
            RollMany(17, 0);
            Assert.AreEqual(22, game.Score());
        }

        [TestMethod]
        public void Score_for_one_strike_with_other_gutters_matches_expected_score()
        {
            RollMany(1, 10);
            game.Roll(6);
            game.Roll(2);
            RollMany(16, 0);
            Assert.AreEqual(26, game.Score());
        }

        [TestMethod]
        public void Score_for_scenario_given_in_problem_statement()
        {
            game.Roll(1);
            game.Roll(4);

            game.Roll(4);
            game.Roll(5);

            game.Roll(6);
            game.Roll(4);

            game.Roll(5);
            game.Roll(5);

            game.Roll(10);

            game.Roll(0);
            game.Roll(1);

            game.Roll(7);
            game.Roll(3);

            game.Roll(6);
            game.Roll(4);

            game.Roll(10);

            game.Roll(2);
            game.Roll(8);
            game.Roll(6);

            Assert.AreEqual(133, game.Score());
        }

        #endregion

        #region Helper Methods
        private void RollMany(int numberOfRolls, int pins)
        {
            for (int index = 0; index < numberOfRolls; index++)
                game.Roll(pins);
        }

        private void RollOneSpare()
        {
            game.Roll(5);
            game.Roll(5);
        }
        #endregion
    }
}