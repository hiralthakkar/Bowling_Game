using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Models
{
    public enum ScoreType
    {
        Strike,
        Spare,
        OpenFrame
    }

    public class Frame
    {
        public Frame(Frame prevFrame)
        {
            if (prevFrame != null)
            { 
                prevFrame.NextFrame = this;
                this.FrameNumber = prevFrame.FrameNumber + 1;
            }
            else
                this.FrameNumber = 1;

            FrameRolls = new List<Roll>();
        }

        //to be used if in future a UI is to be added to show the frames
        public int FrameNumber { get; set; }

        //this will hold the current frame score
        public int FrameScore { get; set; } = 0;

        //this will used to hold the rolls for the current frame, it could be 1 for a strike, 2 for a spare/open frame, 3 for the 10th frame if it's a strike or spare
        //hence keeping it as a list so it can grow as needed
        public List<Roll> FrameRolls { get; set; }

        //this will be used primarily to easily access the next frame and any rolls in it rather than relying on the indexes
        public Frame NextFrame { get; set; }

        //this is to tell if the frame is strike/spare or openframe (used while calculating the total score as well as can be used in future to display on UI)
        public ScoreType ScoreType { get; set; }

        /// <summary>
        /// This method to add the roll to the current frame
        /// </summary>
        /// <param name="roll"></param>
        public void AddRoll(Roll roll)
        {
            FrameRolls.Add(roll);
        }
    }
}