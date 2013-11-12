using System;
using System.Collections.Generic;
using Common;
using MongoDB.Bson;

namespace Server.Model
{
	/// <summary>
	/// Server character encapsulation.
	/// </summary>
	public class ServerCharacter
	{
		public string Name {
			get {
				return character.Name;
			}
			set {
				character.Name = value;
			}
		}

		public AttributeSet AttributeSet {
			get {
				return character.AttributeSet;
			}
			set {
				character.AttributeSet = value;
			}
		}

		public SkillSet SkillSet {
			get {
				return character.SkillSet;
			}
			set {
				character.SkillSet = value;
			}
		}

		public Position Position;
        
        /// <summary>
        /// Attributes
        /// </summary>
		private Character character;
		private string accountId;
        private List<Action> waitingActions;
        private Action performingAction = null;
        private int globalActionCooldown = 0;

		/// <summary>
		/// Constructor used by mongodb deserializer.
		/// </summary>
		public ServerCharacter ()
		{
			character = new Character ("");
			Position = new Position (0, 0);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Server.ServerCharacter"/> class.
		/// </summary>
		/// <param name='name'>
		/// Name.
		/// </param>
		public ServerCharacter (string name, Position position)
		{
			character = new Character(name);
			Position = position;
		}

		/// <summary>
		/// Gets the account identifier.
		/// </summary>
		/// <returns>
		/// The account identifier.
		/// </returns>
		public string GetAccountId ()
		{
			return accountId;
		}

        /// <summary>
        /// Getn the Character object
        /// </summary>
        /// <returns></returns>
		public Character GetCharacter ()
		{
			return character;
		}

        /// <summary>
        /// Update actions.
        /// This method is called by CharacterHandler
        /// </summary>
        /// <param name="currentTime"></param>
        public void UpdateAction(long currentTime, out Action newPerformingAction, out Action newFinishedAction)
        {
            // Reduce global action cooldown
            globalActionCooldown--;
            if (globalActionCooldown < 0)
                globalActionCooldown = 0;

            ensureInitiation();
            newPerformingAction = null;
            newFinishedAction = null;

            // If currently performing action exists
            if(performingAction != null)
            {
                // If performing action has finnished its time
                if (currentTime >= performingAction.GetTimeFinished())
                {
                    CalculateResult(performingAction);
                    newFinishedAction = performingAction;
                    performingAction = null;
                }
            }

            // If no currently performing action exists
            if (performingAction == null)
            {
                if (globalActionCooldown != 0)
                    return;

                // If there is a queued action, set it as performing action
                if (waitingActions.Count > 0)
                {
                    // Get the action from the waiting queue
                    Action nextAction = waitingActions[0];
                    waitingActions.Remove(nextAction);

                    // Calculate when the action will be finished
                    nextAction.SetTimeFinished(currentTime + nextAction.GetPerformingTime());

                    // Set the next action as performing action
                    performingAction = nextAction;
                    newPerformingAction = performingAction;

                    // Set global action cooldown
                    globalActionCooldown = 100;
                 }
            }
        }

        /// <summary>
        /// Calculate result and put it in action
        /// </summary>
        /// <param name="action"></param>
        private void CalculateResult(Action action)
        {
            if (action is TrainAttributeAction)
            {
                TrainAttributeAction taa = (TrainAttributeAction)action;
                // Attributegain prob = 1 / AttributeValue
                Random rand = new Random();
                int probability;
                int attributegain = 0;

                probability = AttributeSet.Attributes[(int)taa.attributeType];
                if (rand.Next(probability) == 0)
                {
                    attributegain = 1;
                }
                EnhanceAttribute(taa.attributeType, attributegain);

                taa.SetAttributeGainResult(attributegain);
            }
            else if(action is UseSkillAction)
            {
                UseSkillAction usa = (UseSkillAction)action;
                // Skillgain prob = 1 / SkillValue
                Random rand = new Random();
                int probabilityGain;
                int skillgain = 0;
                string result = null;

                // Calculate skillgain
                probabilityGain = SkillSet.Skills[(int)usa.skillType];
                if (rand.Next(probabilityGain) == 0)
                {
                    skillgain = 1;
                }
                EnhanceSkill(usa.skillType, skillgain);

                // Calculate result
                switch(usa.skillType)
                {
                    case (SkillType.HUNTING): 
                    {
                        int huntResult = rand.Next(30); 
                        int huntQuantity = 1+rand.Next(5);

                        switch (huntResult)
                        {
                            case 0:
                                result = "You got nothing, noob.";
                                break;

                            case 1:
                                result = "You got " + huntQuantity + " old Man.";
                                break;

                            case 2: 
                                result = "You got " + huntQuantity + " fresh Fish.";
                                break;

                            case 3:
                                result = "You got " + huntQuantity + " fat Rat.";
                                break;

                            case 4:
                                result = "You got " + huntQuantity + " red nosed Rain deer.";
                                break;

                            case 5:
                                result = "You got " + huntQuantity + " brain dead Owl.";
                                break;

                            case 6:
                                result = "You got " + huntQuantity + " sugar sweet Honey bee.";
                                break;

                            case 7:
                                result = "You got " + huntQuantity + " itchy Hedgehog.";
                                break;

                            case 8:
                                result = "You got " + huntQuantity + " agile Cow.";
                                break;

                            case 9:
                                result = "You got " + huntQuantity + " hairy Beaver.";
                                break;

                            case 10:
                                result = "You got " + huntQuantity + " sweaty Hippopotamus.";
                                break;

                            case 11:
                                result = "You got " + huntQuantity + " angry Bird.";
                                break;

                            case 12:
                                result = "You got " + huntQuantity + " Yellow-bellied sapsucker.";
                                break;

                            case 13:
                                result = "You got " + huntQuantity + " crazy Frog.";
                                break;

                            case 14:
                                result = "You got " + huntQuantity + " stupid looking Guinea pig.";
                                break;

                            case 15:
                                result = "You got " + huntQuantity + " imaginary Unicorn.";
                                break;

                            case 16:
                                result = "You got " + huntQuantity + " mad Monky.";
                                break;

                            case 17:
                                result = "You got " + huntQuantity + " stelthy Elephant.";
                                break;

                            case 18:
                                result = "You got " + huntQuantity + " head hidden Ostrish.";
                                break;

                            case 19:
                                result = "You got " + huntQuantity + " extinct Dinosaur.";
                                break;

                            case 20:
                                result = "You got " + huntQuantity + " tricky Anteater.";
                                break;

                            case 21:
                                result = "You got " + huntQuantity + " bullying Bull.";
                                break;

                            case 22:
                                result = "You got " + huntQuantity + " dumb Duck.";
                                break;

                            case 23:
                                result = "You got " + huntQuantity + " butthurt Butterfly.";
                                break;

                            case 24:
                                result = "You got " + huntQuantity + " licking Lizard.";
                                break;

                            case 25:
                                result = "You got " + huntQuantity + " drunk Dromedary.";
                                break;

                            case 26:
                                result = "You got " + huntQuantity + " eldery Elk.";
                                break;

                            case 27:
                                result = "You got " + huntQuantity + " super intelligent Donkey.";
                                break;

                            case 28:
                                result = "You got " + huntQuantity + " swift sloths.";
                                break;

                            case 29:
                                result = "You got " + huntQuantity + " clumsy Cheetah.";
                                break;
                        }
                        break;
                    }
                    default:
                        throw new NWOException("Unhandeled skill type");
                }
                if (skillgain > 0)
                    result += " Your skill increased by " + skillgain + ".";

                usa.SetSkillUseResult(result, skillgain);
            }
            else
            {
                throw new NWOException("Unhadneled Action when Calculating action result!");
            }
        }

        /// <summary>
        /// Try to enqueue an action
        /// </summary>
        /// <param name="action">
        /// The action to enqueue
        /// </param>
        /// <returns>
        /// True if successfull
        /// </returns>
        public void EnqueueAction(Action action)
        {
            waitingActions.Add(action);
        }

        /// <summary>
        /// Get the current performing action
        /// </summary>
        /// <returns>
        /// The Action if an action are being performed,
        /// else return null
        /// </returns>
        public Action GetPerformingAction()
        {
            return performingAction;
        }

        /// <summary>
        /// Get reference to list watingActions
        /// </summary>
        /// <returns></returns>
        public List<Action> GetListWatingAction()
        {
            return waitingActions;
        }

		/// <summary>
		/// Enhances the attribute.
		/// </summary>
		/// <returns>
		/// <c>true</c>, if attribute was enhanced, <c>false</c> otherwise.
		/// </returns>
		/// <param name='attrType'>
		/// Attr type.
		/// </param>
		/// <param name='value'>
		/// Value.
		/// </param>
		public bool EnhanceAttribute (AttributeType attrType, int value)
		{
			int newValue = AttributeSet.Attributes[(int)attrType] + value;

			if (newValue < 0 || newValue > 20)
				return false;

			AttributeSet.Attributes[(int)attrType] = newValue;
			return true;
		}

		/// <summary>
		/// Enhances the skill.
		/// </summary>
		/// <returns>
		/// <c>true</c>, if skill was enhanced, <c>false</c> otherwise.
		/// </returns>
		/// <param name='skillType'>
		/// Skill type.
		/// </param>
		/// <param name='value'>
		/// Value.
		/// </param>
		public bool EnhanceSkill (SkillType skillType, int value)
		{
			int newValue = SkillSet.Skills[(int)skillType] + value;

			if (newValue < 0 || newValue > 100) 
				return false;

			SkillSet.Skills[(int)skillType] = newValue;
			return true;
		}

		/// <summary>
		/// Sets the account identifier.
		/// </summary>
		/// <param name='accountId'>
		/// Account identifier.
		/// </param>
		public void SetAccountId (string accountId)
		{
			this.accountId = accountId;
		}

        private void ensureInitiation()
        {
            if(waitingActions == null)
                waitingActions = new List<Action>();
        }
	}
}

