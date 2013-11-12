using System;
using Common;

namespace Server.Model
{
    public class TrainAttributeAction : Action
    {
        public readonly AttributeType attributeType;
        private int attributeGainResult = 0;

        public TrainAttributeAction(AttributeType type): base()
        {
            this.attributeType = type;
            this.performingTime = 3000; // ms      
        }

        /// <summary>
        /// Get the result from the Attribute train
        /// </summary>
        /// <param name="result">
        /// The result gain
        /// </param>
        /// <returns>
        /// True if result is set and can be collected.
        /// False if the result has not been set.
        /// </returns>
        public bool GetAttributeGainResult(out int result)
        {
            if (finished)
            {
                result = attributeGainResult;
                return true;
            }
            result = 0;
            return false;
        }

        /// <summary>
        /// Set a result, finished are set to 'true'
        /// </summary>
        /// <param name="result">
        /// The result
        /// </param>
        public void SetAttributeGainResult(int result)
        {
            attributeGainResult = result;
            finished = true;
        }
    }
}
