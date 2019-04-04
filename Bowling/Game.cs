using Bowling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling
{
    public class Game : IDisposable
    {
        #region private members declaration
        const int TotalPins = 10;
        const int TotalFrames = 10;
        List<Frame> _frameList;
        #endregion

        #region Properties
        List<Frame> Frames
        {
            get
            {
                if (_frameList == null)
                    _frameList = new List<Frame>();

                return _frameList;
            }
        }

        Frame CurrentFrame
        {
            get
            {
                if (Frames.Count > 0)
                    return Frames[Frames.Count - 1];
                else
                    return null;
            }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// This method rolls the next ball and updates the score and score type (open frame, strike, spare) for the current frame
        /// </summary>
        /// <param name="pins"></param>
        public void Roll(int pins)
        {
            try
            {
                //check if any frames are added already or this is the first roll
                if (CurrentFrame == null)
                {
                    AddNewFrame(pins, null);
                }
                else
                {
                    //check if the current frame does have any more rolls else add a new frame and the new roll to it
                    if (CanAddRoll)
                    {
                        CurrentFrame.AddRoll(CreateRoll(pins));
                        UpdateFrameScoreType(CurrentFrame);

                    }
                    else
                    {
                        AddNewFrame(pins, CurrentFrame);
                    }
                }
            }
            catch (ArgumentOutOfRangeException arg)
            {
                throw arg;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while trying to execute Roll.");
            }
        }

        /// <summary>
        /// This method calculates and updates the score for each frame and also a running total to be displayed as final score
        /// </summary>
        /// <returns></returns>
        public int Score()
        {
            int totalScore = 0;

            try
            {
                if (Frames.Count >= TotalFrames)
                {
                    //explicitly looping for 10 frames as the 11th frame has been added only for scoring purpose
                    for (int index = 0; index < TotalFrames; index++)
                    {
                        switch (Frames[index].ScoreType)
                        {
                            case ScoreType.Spare:
                                Frames[index].FrameScore += GetSpareBonus(Frames[index]);

                                break;

                            case ScoreType.Strike:
                                Frames[index].FrameScore += GetStrikeBonus(Frames[index]);
                                break;
                        }

                        totalScore += Frames[index].FrameScore;
                    }
                }
                else
                    throw new InvalidOperationException("Cannot calculate final score as the game is incomplete");
            }
            catch (InvalidOperationException ioEx)
            {
                throw ioEx;
            }
            catch
            {
                throw new Exception("An error occurred while trying to calculate final score");
            }

            return totalScore;
        }

        #endregion

        #region internal methods for Roll 

        /// <summary>
        /// This is a common method to add a new frame, associate the next frame of the previous frame to the new frame being added as well as adds the first
        /// roll to the frame and updates the score type
        /// </summary>
        /// <param name="pins"></param>
        /// <param name="prevFrame"></param>
        private void AddNewFrame(int pins, Frame prevFrame)
        {
            try
            {
                Roll roll = CreateRoll(pins);

                Frame frame = CreateFrame(prevFrame);
                frame.AddRoll(roll);
                UpdateFrameScoreType(frame);
                Frames.Add(frame);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// This method updates the score type as well as score for the current frame
        /// </summary>
        /// <param name="frame"></param>
        private void UpdateFrameScoreType(Frame frame)
        {
            //it is in an unlikely scenario to have no rolls on a frame
            if (frame.FrameRolls.Count == 0)
                frame.ScoreType = ScoreType.OpenFrame;
            else
            {
                //calculate the total pins knocked on the frame
                int totalPinsKnocked = 0;
                for (int index = 0; index < frame.FrameRolls.Count; index++)
                    totalPinsKnocked += frame.FrameRolls[index].PinsKnocked;

                //if it's equal to total number of pins available then it could be a strike or spare
                if (totalPinsKnocked == TotalPins)
                {
                    //all pins knocked in one roll, so it's a strike
                    if (frame.FrameRolls.Count == 1)
                        frame.ScoreType = ScoreType.Strike;
                    else
                        frame.ScoreType = ScoreType.Spare;
                }
                else
                    frame.ScoreType = ScoreType.OpenFrame;

                //update the frame score with total pins knocked
                frame.FrameScore = totalPinsKnocked;
            }
        }

        /// <summary>
        /// this will return if a new frame can be added or not
        /// </summary>
        private bool CanAddFrame
        {
            get
            {
                if (Frames.Count == TotalFrames)
                {
                    //if the last frame is spare or strike, allow for an extra frame to be added. This is just to keep the counts
                    if (Frames[Frames.Count - 1].ScoreType == ScoreType.Spare || Frames[Frames.Count - 1].ScoreType == ScoreType.Strike)
                        return true;
                    else
                        return false;
                }
                else
                    return true;
            }
        }

        /// <summary>
        /// this method adds a new frame to the list
        /// </summary>
        /// <param name="frameNumber"></param>
        /// <param name="prevFrame"></param>
        /// <returns></returns>
        private Frame CreateFrame(Frame prevFrame)
        {
            Frame newFrame = null;

            if (CanAddFrame)
                newFrame = new Frame(prevFrame);
            else
                throw new ArgumentOutOfRangeException("No more rolls are allowed");

            return newFrame;
        }

        /// <summary>
        /// this will return true or false depending upon if a new roll can be added to the current frame
        /// </summary>
        private bool CanAddRoll
        {
            get
            {
                //no frames yet, return true
                if (CurrentFrame == null)
                    return true;
                else
                {
                    switch (CurrentFrame.ScoreType)
                    {
                        //if it's an open frame and only one ball has been rolled, allow for another roll
                        case ScoreType.OpenFrame:
                            if (CurrentFrame.FrameRolls.Count == 1)
                                return true;
                            else
                                return false;


                        //no more rolls allowed for a spare or strike
                        case ScoreType.Spare:
                        case ScoreType.Strike:
                            return false;

                        default:
                            return false;
                    }
                }
            }
        }

        /// <summary>
        /// this method creates and returns a new roll
        /// </summary>
        /// <param name="pinsKnocked"></param>
        /// <returns></returns>
        private Roll CreateRoll(int pinsKnocked)
        {
            return new Roll(pinsKnocked);
        }
        #endregion

        #region internal methods for Score
        /// <summary>
        /// This method calculates and returns the bonus for a spare
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        private int GetSpareBonus(Frame frame)
        {
            if (frame.NextFrame != null)
                //for a spare, add a roll score from the next roll
                return frame.NextFrame.FrameRolls[0].PinsKnocked;
            else
                return 0;
        }

        /// <summary>
        /// This method calculates and returns the bonus for a strike
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        private int GetStrikeBonus(Frame frame)
        {
            int strikeBonus = 0;
            bool addOneMoreFrame = false;
            if (frame.NextFrame != null)
            {
                //if the next frame is strike too, add score from one more frame or roll to the current frame's score
                if (frame.NextFrame.ScoreType == ScoreType.Strike)
                    addOneMoreFrame = true;

                //add frame scroe from next strike to current frame score
                if (frame.NextFrame.ScoreType == ScoreType.Strike) //Frames[index].NextFrame.FrameNumber != TotalFrames || 
                    strikeBonus += frame.NextFrame.FrameScore;
                else
                    //else add score from next two rolls to current frame score
                    strikeBonus += frame.NextFrame.FrameRolls[0].PinsKnocked + frame.NextFrame.FrameRolls[1].PinsKnocked;
            }

            //check for one more frame score in this case as it was a strike
            if (addOneMoreFrame)
            {
                if (frame.NextFrame.NextFrame != null)
                {
                    //if the frame is strike, add frame score
                    if (frame.NextFrame.NextFrame.ScoreType == ScoreType.Strike)
                        strikeBonus += frame.NextFrame.NextFrame.FrameScore;
                    else
                        //or add the score from first roll of the frame
                        strikeBonus += frame.NextFrame.NextFrame.FrameRolls[0].PinsKnocked;
                }
            }

            return strikeBonus;
        }
        #endregion

        public void Dispose()
        {
            _frameList = null;
        }
    }
}