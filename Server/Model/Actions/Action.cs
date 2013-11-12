using System;

namespace Server.Model
{
    public abstract class Action
    {
        // Time it takes to perform the action (set by children constructor)
        protected long performingTime;

        // Time when the Action is finished
        protected long timeFinished=0;

        // Used to check if the Action is finished, to know if a result exists
        protected bool finished;

        public Action()
        {
            finished = false;
        }
 
        public void SetTimeFinished(long timeFinished)
        {
            this.timeFinished = timeFinished;
        }

        public long GetTimeFinished()
        {
            return timeFinished;
        }

        public long GetPerformingTime()
        {
            return performingTime;
        }

        public bool GetFinished()
        {
            return finished;
        }
    }
}
