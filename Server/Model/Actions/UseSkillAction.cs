using System;
using Common;

namespace Server.Model
{
    public class UseSkillAction : Action
    {
        public readonly SkillType skillType;
        private int skillGain;
        private string result;

        public UseSkillAction(SkillType type) : base()
        {
            this.skillType = type;
            this.performingTime = 3000; // ms      
        }

        /// <summary>
        /// Get the result from the Skill use
        /// </summary>
        /// <param name="result">
        /// The result gain
        /// </param>
        /// <returns>
        /// True if result is set and can be collected.
        /// False if the result has not been set.
        /// </returns>
        public bool GetSkillUseResult(out string result, out int skillGain)
        {
            if (finished)
            {
                skillGain = this.skillGain;
                result = this.result;
                return true;
            }
            skillGain = 0;
            result = "";
            return false;
        }

        /// <summary>
        /// Set a result, finished are set to 'true'
        /// </summary>
        /// <param name="result">
        /// The result
        /// </param>
        public void SetSkillUseResult(string result, int skillGain)
        {
            this.skillGain = skillGain;
            this.result = result;
            finished = true;
        }
    }
}
