using System;

namespace Client.View
{
	using Gtk;
	using Gdk;

	public class PageCharacter : Page
	{
		private ActionEvent actionEvent;
		private Common.Character character;
	
		private VBox outerVbox;
		private VBox innerVbox; // Contains the containerFrame

		private HBox upperHbox = new HBox(false, 5); // contains character and inventory frames
		private HBox lowerHbox = new HBox(false, 5); // contains skills and attributes frames

		private Frame containerFrame; // Contains all other frames
		private Frame skillsFrame; // Contains skillstable
		private Frame attributesFrame; // contains attributestable
		private Frame inventoryFrame; // contains inventory table
		private Frame characterFrame; // contains character image

		private Table attributesTable;
		private Table skillsTable;
		private Table inventoryTable;
		private Gtk.Image imageCharacter;
		
		private EventBox buttonTrainStrength;
		private EventBox buttonTrainEndurance;
		private EventBox buttonTrainAgility;
		private EventBox buttonTrainIntelligence;
		private EventBox buttonTrainPerception;
		private EventBox buttonTrainWillpower;

		private Label messageLabel;
        private Label[] attributeMessageLabels;

		private Label attrStrengthLabel;
		private Label attrEnduranceLabel;
		private Label attrAgilityLabel;
		private Label attrIntelligenceLabel;
		private Label attrPerceptionLabel;
		private Label attrWillpowerLabel;

		private Label attrStrengthTotalValueLabel;
		private Label attrEnduranceTotalValueLabel;
		private Label attrAgilityTotalValueLabel;
		private Label attrIntelligenceTotalValueLabel;
		private Label attrPerceptionTotalValueLabel;
		private Label attrWillpowerTotalValueLabel;

		private Label attrStrengthDetailedValueLabel;
		private Label attrEnduranceDetailedValueLabel;
		private Label attrAgilityDetailedValueLabel;
		private Label attrIntelligenceDetailedValueLabel;
		private Label attrPerceptionDetailedValueLabel;
		private Label attrWillpowerDetailedValueLabel;

		private Label skillHuntingLabel;
		private Label skillFarmingLabel;
		private Label skillWoodcuttingLabel;
		private Label skillMiningLabel;
		private Label skillMagicLabel;
		private Label skillMeleeLabel;
		private Label skillRangedLabel;
		private Label skillStealthLabel;
		private Label skillDiplomacyLabel;
		private Label skillCraftingLabel;
		private Label skillBuildingLabel;
		private Label skillProphecyLabel;

		private Label skillHuntingTotalValueLabel;
		private Label skillFarmingTotalValueLabel;
		private Label skillWoodcuttingTotalValueLabel;
		private Label skillMiningTotalValueLabel;
		private Label skillMagicTotalValueLabel;
		private Label skillMeleeTotalValueLabel;
		private Label skillRangedTotalValueLabel;
		private Label skillStealthTotalValueLabel;
		private Label skillDiplomacyTotalValueLabel;
		private Label skillCraftingTotalValueLabel;
		private Label skillBuildingTotalValueLabel;
		private Label skillProphecyTotalValueLabel;

		private Label skillHuntingDetailedValueLabel;
		private Label skillFarmingDetailedValueLabel;
		private Label skillWoodcuttingDetailedValueLabel;
		private Label skillMiningDetailedValueLabel;
		private Label skillMagicDetailedValueLabel;
		private Label skillMeleeDetailedValueLabel;
		private Label skillRangedDetailedValueLabel;
		private Label skillStealthDetailedValueLabel;
		private Label skillDiplomacyDetailedValueLabel;
		private Label skillCraftingDetailedValueLabel;
		private Label skillBuildingDetailedValueLabel;
		private Label skillProphecyDetailedValueLabel;

		/// <summary>
		/// Initializes a new instance of the <see cref="Client.View.PageCharacter"/> class.
		/// </summary>
		/// <param name='actionEvent'>
		/// Action event.
		/// </param>
		/// <param name='character'>
		/// Character.
		/// </param>
		public PageCharacter(ActionEvent actionEvent, Common.Character character)
		{
			this.actionEvent = actionEvent;
			this.character = character;

			initialize ();
		}

		/// <summary>
		/// Sets the character.
		/// </summary>
		/// <param name='newCharacter'>
		/// New character.
		/// </param>
		public void SetCharacter(Common.Character newCharacter)
		{
			character = newCharacter;
			UpdateLabelTexts();
		}

		public Widget GetContainer ()
		{
			return outerVbox;
		}

		/// <summary>
		///  Determines whether this screen desires the window to be resizable or not. 
		/// </summary>
		/// <returns>
		/// <c>true</c> if this instance is window resizable; otherwise, <c>false</c>.
		/// </returns>
		public bool GetWindowRequestResizable ()
		{
			return true;
		}

		public void SetStatusMessage (string message)
		{
			messageLabel.Text = message;
		}

        /// <summary>
        /// Sets status message for an attribute
        /// </summary>
        /// <param name="attrType"></param>
        /// <param name="message"></param>
        public void SetAttributeStatusMessage(Common.AttributeType attrType, string message)
        {
            attributeMessageLabels[(int)attrType].Text = message;
        }

        /// <summary>
        /// Clears all attribute status messages
        /// </summary>
        public void ClearAllAttributeStatusMessages()
        {
            foreach(Label l in attributeMessageLabels)
                l.Text = "";
        }

	
		private void initialize()
		{
			innerVbox = new VBox(false, 5);
			outerVbox = new VBox(false, 0);

			upperHbox = new HBox(false, 5);
			lowerHbox = new HBox(false, 5);

			containerFrame = new Frame("Character view");
			skillsFrame = new Frame("Skills");
			attributesFrame = new Frame("Attributes");
			inventoryFrame = new Frame("Inventory");
			characterFrame = new Frame(character.Name);

			messageLabel = WidgetFactory.CreateLabel ("", false);

			// Initialize tables
			attributesTable = new Table((uint)Enum.GetNames(typeof(Common.AttributeType)).Length , 5, true);
			skillsTable = new Table((uint)Enum.GetNames(typeof(Common.SkillType)).Length, 3, true);
			inventoryTable = new Table(1, 1, true);

			attributesTable.RowSpacing = 10;
			attributesTable.ColumnSpacing = 10;
			attributesTable.BorderWidth = 10;
			skillsTable.RowSpacing = 10;
			skillsTable.ColumnSpacing = 10;
			skillsTable.BorderWidth = 10;

			initStatsTables();

			// Character pic
			imageCharacter = WidgetFactory.CreateImage ("Graphics/avatar.png", 100, 150);

			// boxes and frames 
			containerFrame.LabelXalign = (float)0.5;
			containerFrame.LabelWidget = WidgetFactory.CreateLabelTitle("Character view", true, 180, 30);
			containerFrame.LabelWidget.ModifyFg(Gtk.StateType.Normal, new Gdk.Color(255,255,255));
			containerFrame.BorderWidth = 20;

			characterFrame.Add (imageCharacter);
			characterFrame.LabelXalign = (float)0.5;
			characterFrame.LabelWidget.ModifyFg(Gtk.StateType.Normal, new Gdk.Color(255,255,255));
			characterFrame.BorderWidth = 2;

			inventoryFrame.Add (inventoryTable);
			inventoryFrame.LabelXalign = (float)0.5;
			inventoryFrame.LabelWidget.ModifyFg(Gtk.StateType.Normal, new Gdk.Color(255,255,255));
			inventoryFrame.BorderWidth = 2;

			attributesFrame.Add (attributesTable);
			attributesFrame.LabelXalign = (float)0.5;
			attributesFrame.LabelWidget.ModifyFg(Gtk.StateType.Normal, new Gdk.Color(255,255,255));
			attributesFrame.BorderWidth = 2;

			skillsFrame.Add (skillsTable);
			skillsFrame.LabelXalign = (float)0.5;
			skillsFrame.LabelWidget.ModifyFg(Gtk.StateType.Normal, new Gdk.Color(255,255,255));
			skillsFrame.BorderWidth = 2;

			// Pack stuff into boxes
			upperHbox.PackStart(characterFrame, false, true, 0);
			upperHbox.PackStart(inventoryFrame, true, true, 0);

			lowerHbox.PackStart (attributesFrame, true, true, 0);
			lowerHbox.PackStart (skillsFrame, true, true, 0);

			innerVbox.PackStart(upperHbox, true, true, 0);
			innerVbox.PackStart(lowerHbox, false, false, 0);

			containerFrame.Add(innerVbox);

			outerVbox.PackStart (containerFrame, false, false, 0);
			outerVbox.PackEnd(messageLabel, false, false, 5);
			outerVbox.PackEnd(WidgetFactory.CreateButtonText("Main screen", true, this.actionEvent, UserEvent.MAIN_SHOW), false, false, 0);
		}

		public void UpdateLabelTexts()
		{
			attrStrengthTotalValueLabel.Text = "" + character.GetAttributeTotalValue(Common.AttributeType.STRENGTH);
			attrEnduranceTotalValueLabel.Text = "" + character.GetAttributeTotalValue(Common.AttributeType.ENDURANCE);
			attrAgilityTotalValueLabel.Text = "" + character.GetAttributeTotalValue(Common.AttributeType.AGILITY);
			attrIntelligenceTotalValueLabel.Text = "" + character.GetAttributeTotalValue(Common.AttributeType.INTELLIGENCE);
			attrPerceptionTotalValueLabel.Text = "" + character.GetAttributeTotalValue(Common.AttributeType.PERCEPTION);
			attrWillpowerTotalValueLabel.Text = "" + character.GetAttributeTotalValue(Common.AttributeType.WILLPOWER);
			
			attrStrengthDetailedValueLabel.Text =
				"(" + character.AttributeSet.Attributes[(int)Common.AttributeType.STRENGTH] + " + " + character.GetAttributeBonusValue(Common.AttributeType.STRENGTH) + ")";
			
			attrEnduranceDetailedValueLabel.Text = 
				"(" + character.AttributeSet.Attributes[(int)Common.AttributeType.ENDURANCE] + " + " +  character.GetAttributeBonusValue(Common.AttributeType.ENDURANCE) + ")";
			
			attrAgilityDetailedValueLabel.Text = 
				"(" + character.AttributeSet.Attributes[(int)Common.AttributeType.AGILITY] + " + " + character.GetAttributeBonusValue(Common.AttributeType.AGILITY) + ")";
			
			attrIntelligenceDetailedValueLabel.Text = 
				"(" + character.AttributeSet.Attributes[(int)Common.AttributeType.INTELLIGENCE] + " + " + character.GetAttributeBonusValue(Common.AttributeType.INTELLIGENCE) + ")";
			
			attrPerceptionDetailedValueLabel.Text = 
				"(" + character.AttributeSet.Attributes[(int)Common.AttributeType.PERCEPTION] + " + " + character.GetAttributeBonusValue(Common.AttributeType.PERCEPTION) + ")";
			
			attrWillpowerDetailedValueLabel.Text = 
				"(" + character.AttributeSet.Attributes[(int)Common.AttributeType.WILLPOWER] + " + " + character.GetAttributeBonusValue(Common.AttributeType.WILLPOWER) + ")";
			
			skillHuntingTotalValueLabel.Text = "" + character.GetSkillTotalValue(Common.SkillType.HUNTING);
			skillFarmingTotalValueLabel.Text = "" + character.GetSkillTotalValue(Common.SkillType.FARMING);
			skillWoodcuttingTotalValueLabel.Text = "" + character.GetSkillTotalValue(Common.SkillType.WOODCUTTING);
			skillMiningTotalValueLabel.Text = "" + character.GetSkillTotalValue(Common.SkillType.MINING);
			skillMagicTotalValueLabel.Text = "" + character.GetSkillTotalValue(Common.SkillType.MAGIC);
			skillMeleeTotalValueLabel.Text = "" + character.GetSkillTotalValue(Common.SkillType.MELEE);
			skillRangedTotalValueLabel.Text = "" + character.GetSkillTotalValue(Common.SkillType.RANGED);
			skillStealthTotalValueLabel.Text = "" + character.GetSkillTotalValue(Common.SkillType.STEALTH);
			skillDiplomacyTotalValueLabel.Text = "" + character.GetSkillTotalValue(Common.SkillType.DIPLOMACY);
			skillCraftingTotalValueLabel.Text = "" + character.GetSkillTotalValue(Common.SkillType.CRAFTING);
			skillBuildingTotalValueLabel.Text = "" + character.GetSkillTotalValue(Common.SkillType.BUILDING);
			skillProphecyTotalValueLabel.Text = "" + character.GetSkillTotalValue(Common.SkillType.PROPHECY);
			
			skillHuntingDetailedValueLabel.Text = 
				"(" + character.SkillSet.Skills[(int)Common.SkillType.HUNTING] + " + " + character.GetSkillBonusValue(Common.SkillType.HUNTING) + ")";
			
			skillFarmingDetailedValueLabel.Text = 
				"(" + character.SkillSet.Skills[(int)Common.SkillType.FARMING] + " + " + character.GetSkillBonusValue(Common.SkillType.FARMING) + ")";
			
			skillWoodcuttingDetailedValueLabel.Text = 
				"(" + character.SkillSet.Skills[(int)Common.SkillType.WOODCUTTING] + " + " + character.GetSkillBonusValue(Common.SkillType.WOODCUTTING) + ")";
			
			skillMiningDetailedValueLabel.Text = 
				"(" + character.SkillSet.Skills[(int)Common.SkillType.MINING] + " + " + character.GetSkillBonusValue(Common.SkillType.MINING) + ")";
			
			skillMagicDetailedValueLabel.Text = 
				"(" + character.SkillSet.Skills[(int)Common.SkillType.MAGIC] + " + " + character.GetSkillBonusValue(Common.SkillType.MAGIC) + ")";
			
			skillMeleeDetailedValueLabel.Text =
				"(" + character.SkillSet.Skills[(int)Common.SkillType.MELEE] + " + " + character.GetSkillBonusValue(Common.SkillType.MELEE) + ")";
			
			skillRangedDetailedValueLabel.Text = 
				"(" + character.SkillSet.Skills[(int)Common.SkillType.RANGED] + " + " + character.GetSkillBonusValue(Common.SkillType.RANGED) + ")";
			
			skillStealthDetailedValueLabel.Text =
				"(" + character.SkillSet.Skills[(int)Common.SkillType.STEALTH] + " + " + character.GetSkillBonusValue(Common.SkillType.STEALTH) + ")";
			
			skillDiplomacyDetailedValueLabel.Text = 
				"(" + character.SkillSet.Skills[(int)Common.SkillType.DIPLOMACY] + " + " + character.GetSkillBonusValue(Common.SkillType.DIPLOMACY) + ")";
			
			skillCraftingDetailedValueLabel.Text = 
				"(" + character.SkillSet.Skills[(int)Common.SkillType.CRAFTING] + " + " + character.GetSkillBonusValue(Common.SkillType.CRAFTING) + ")";
			
			skillBuildingDetailedValueLabel.Text =
				"(" + character.SkillSet.Skills[(int)Common.SkillType.BUILDING] + " + " + character.GetSkillBonusValue(Common.SkillType.BUILDING) + ")";
			
			skillProphecyDetailedValueLabel.Text =
				"(" + character.SkillSet.Skills[(int)Common.SkillType.PROPHECY] + " + " + character.GetSkillBonusValue(Common.SkillType.PROPHECY) + ")";
			
		}

		private void initStatsTables()
		{
			// Attributes table col 1 - Attribute names
			// Initialize labels
			attrStrengthLabel = WidgetFactory.CreateLabel("Strength: ", false);
			attrEnduranceLabel = WidgetFactory.CreateLabel("Endurance: ", false);
			attrAgilityLabel = WidgetFactory.CreateLabel("Agility: ", false);
			attrIntelligenceLabel = WidgetFactory.CreateLabel("Intelligence: ", false);
			attrPerceptionLabel = WidgetFactory.CreateLabel("Perception: ", false);
			attrWillpowerLabel = WidgetFactory.CreateLabel("Willpower: ", false);
			
			// Attach labels
			attributesTable.Attach (attrStrengthLabel, 0, 1, 0, 1);
			attributesTable.Attach (attrEnduranceLabel, 0, 1, 1, 2);
			attributesTable.Attach (attrAgilityLabel, 0, 1, 2, 3);
			attributesTable.Attach (attrIntelligenceLabel, 0, 1, 3, 4);
			attributesTable.Attach (attrPerceptionLabel, 0, 1, 4, 5);
			attributesTable.Attach (attrWillpowerLabel, 0, 1, 5, 6);
			
			// Attributes table col 2 - Total values
			// Initialize labels
			attrStrengthTotalValueLabel = WidgetFactory.CreateLabel ("" + character.GetAttributeTotalValue(Common.AttributeType.STRENGTH), false);
			attrEnduranceTotalValueLabel = WidgetFactory.CreateLabel ("" + character.GetAttributeTotalValue(Common.AttributeType.ENDURANCE), false);
			attrAgilityTotalValueLabel = WidgetFactory.CreateLabel ("" + character.GetAttributeTotalValue(Common.AttributeType.AGILITY), false);
			attrIntelligenceTotalValueLabel = WidgetFactory.CreateLabel ("" + character.GetAttributeTotalValue(Common.AttributeType.INTELLIGENCE), false);
			attrPerceptionTotalValueLabel = WidgetFactory.CreateLabel ("" + character.GetAttributeTotalValue(Common.AttributeType.PERCEPTION), false);
			attrWillpowerTotalValueLabel = WidgetFactory.CreateLabel ("" + character.GetAttributeTotalValue(Common.AttributeType.WILLPOWER), false);
			
			// Attach labels
			attributesTable.Attach (attrStrengthTotalValueLabel, 1, 2, 0, 1);
			attributesTable.Attach (attrEnduranceTotalValueLabel, 1, 2, 1, 2);
			attributesTable.Attach (attrAgilityTotalValueLabel, 1, 2, 2, 3);
			attributesTable.Attach (attrIntelligenceTotalValueLabel, 1, 2, 3, 4);
			attributesTable.Attach (attrPerceptionTotalValueLabel, 1, 2, 4, 5);
			attributesTable.Attach (attrWillpowerTotalValueLabel, 1, 2, 5, 6);
			
			// Attributes table col 3 - Detailed value
			// Initialize labels
			attrStrengthDetailedValueLabel = WidgetFactory.CreateLabel (
				"(" + character.AttributeSet.Attributes[(int)Common.AttributeType.STRENGTH] + " + " + character.GetAttributeBonusValue(Common.AttributeType.STRENGTH) + ")",false);
			attrStrengthDetailedValueLabel.ModifyFg(StateType.Normal, new Gdk.Color(211,211,211) );
			
			attrEnduranceDetailedValueLabel = WidgetFactory.CreateLabel (
				"(" + character.AttributeSet.Attributes[(int)Common.AttributeType.ENDURANCE] + " + " +  character.GetAttributeBonusValue(Common.AttributeType.ENDURANCE) + ")", false);
			attrEnduranceDetailedValueLabel.ModifyFg(StateType.Normal, new Gdk.Color(211,211,211) );
			
			attrAgilityDetailedValueLabel = WidgetFactory.CreateLabel (
				"(" + character.AttributeSet.Attributes[(int)Common.AttributeType.AGILITY] + " + " + character.GetAttributeBonusValue(Common.AttributeType.AGILITY) + ")", false);
			attrAgilityDetailedValueLabel.ModifyFg(StateType.Normal, new Gdk.Color(211,211,211) );
			
			attrIntelligenceDetailedValueLabel = WidgetFactory.CreateLabel (
				"(" + character.AttributeSet.Attributes[(int)Common.AttributeType.INTELLIGENCE] + " + " + character.GetAttributeBonusValue(Common.AttributeType.INTELLIGENCE) + ")", false);
			attrIntelligenceDetailedValueLabel.ModifyFg(StateType.Normal, new Gdk.Color(211,211,211) );
			
			attrPerceptionDetailedValueLabel = WidgetFactory.CreateLabel (
				"(" + character.AttributeSet.Attributes[(int)Common.AttributeType.PERCEPTION] + " + " + character.GetAttributeBonusValue(Common.AttributeType.PERCEPTION) + ")", false);
			attrPerceptionDetailedValueLabel.ModifyFg(StateType.Normal, new Gdk.Color(211,211,211) );
			
			attrWillpowerDetailedValueLabel = WidgetFactory.CreateLabel (
				"(" + character.AttributeSet.Attributes[(int)Common.AttributeType.WILLPOWER] + " + " + character.GetAttributeBonusValue(Common.AttributeType.WILLPOWER) + ")", false);
			attrWillpowerDetailedValueLabel.ModifyFg(StateType.Normal, new Gdk.Color(211,211,211) );
			
			
			// Attach labels
			attributesTable.Attach (attrStrengthDetailedValueLabel, 2, 3, 0, 1);
			attributesTable.Attach (attrEnduranceDetailedValueLabel, 2, 3, 1, 2);
			attributesTable.Attach (attrAgilityDetailedValueLabel, 2, 3, 2, 3);
			attributesTable.Attach (attrIntelligenceDetailedValueLabel, 2, 3, 3, 4);
			attributesTable.Attach (attrPerceptionDetailedValueLabel, 2, 3, 4, 5);
			attributesTable.Attach (attrWillpowerDetailedValueLabel, 2, 3, 5, 6);
			
			// Attributes col 4 - Train buttons
			buttonTrainStrength = WidgetFactory.CreateButtonText ("Lift weights", 80, 20, true, actionEvent, UserEvent.CHARACTER_TRAIN_STRENGTH);
			buttonTrainEndurance = WidgetFactory.CreateButtonText ("Go for a run", 80, 20, true, actionEvent, UserEvent.CHARACTER_TRAIN_ENDURANCE);
			buttonTrainAgility = WidgetFactory.CreateButtonText ("Stretch", 80, 20, true, actionEvent, UserEvent.CHARACTER_TRAIN_AGILITY);
			buttonTrainIntelligence = WidgetFactory.CreateButtonText ("Read book", 80, 20, true, actionEvent, UserEvent.CHARACTER_TRAIN_INTELLIGENCE);
			buttonTrainPerception = WidgetFactory.CreateButtonText ("Meditate", 80, 20, true, actionEvent, UserEvent.CHARACTER_TRAIN_PERCEPTION);
			buttonTrainWillpower = WidgetFactory.CreateButtonText ("Rest", 80, 20, true, actionEvent, UserEvent.CHARACTER_TRAIN_WILLPOWER);
			
			attributesTable.Attach(buttonTrainStrength, 3, 4, 0, 1);
			attributesTable.Attach(buttonTrainEndurance, 3, 4, 1, 2);
			attributesTable.Attach(buttonTrainAgility, 3, 4, 2, 3);
			attributesTable.Attach(buttonTrainIntelligence, 3, 4, 3, 4);
			attributesTable.Attach(buttonTrainPerception, 3, 4, 4, 5);
			attributesTable.Attach(buttonTrainWillpower, 3, 4, 5, 6);
			
			// Attribute table col 5 - status
			attributeMessageLabels = new Label[(uint)Enum.GetNames(typeof(Common.AttributeType)).Length];
			
			for (uint i = 0;i<attributeMessageLabels.Length; i++)
			{
				Label l = WidgetFactory.CreateLabel("", true);
				attributeMessageLabels[i] = l;
				attributesTable.Attach(l, 4, 5, i, i + 1);
			}
			
			
			// Skills table col 1 - Skill names
			// Initialize labels
			skillHuntingLabel = WidgetFactory.CreateLabel("Hunting: ", false);
			skillFarmingLabel = WidgetFactory.CreateLabel("Farming: ", false);
			skillWoodcuttingLabel = WidgetFactory.CreateLabel("Woodcutting: ", false);
			skillMiningLabel = WidgetFactory.CreateLabel("Mining: ", false);
			skillMagicLabel = WidgetFactory.CreateLabel("Magic: ", false);
			skillMeleeLabel = WidgetFactory.CreateLabel("Melee: ", false);
			skillRangedLabel = WidgetFactory.CreateLabel("Ranged: ", false);
			skillStealthLabel = WidgetFactory.CreateLabel("Stealth: ", false);
			skillDiplomacyLabel = WidgetFactory.CreateLabel("Diplomacy: ", false);
			skillCraftingLabel = WidgetFactory.CreateLabel("Crafting: ", false);
			skillBuildingLabel = WidgetFactory.CreateLabel("Building: ", false);
			skillProphecyLabel = WidgetFactory.CreateLabel("Prophecy: ", false);
			
			
			// Attatch labels
			skillsTable.Attach (skillHuntingLabel, 0, 1, 0, 1);
			skillsTable.Attach (skillFarmingLabel, 0, 1, 1, 2);
			skillsTable.Attach (skillWoodcuttingLabel, 0, 1, 2, 3);
			skillsTable.Attach (skillMiningLabel, 0, 1, 3, 4);
			skillsTable.Attach (skillMagicLabel, 0, 1, 4, 5);
			skillsTable.Attach (skillMeleeLabel, 0, 1, 5, 6);
			skillsTable.Attach (skillRangedLabel, 0, 1, 6, 7);
			skillsTable.Attach (skillStealthLabel, 0, 1, 7, 8);
			skillsTable.Attach (skillDiplomacyLabel, 0, 1, 8, 9);
			skillsTable.Attach(skillCraftingLabel, 0, 1, 9, 10);
			skillsTable.Attach (skillBuildingLabel, 0, 1, 10, 11);
			skillsTable.Attach (skillProphecyLabel, 0, 1, 11, 12);
			
			// Skills table col 2 - Total values
			// Initialize labels
			skillHuntingTotalValueLabel = WidgetFactory.CreateLabel("" + character.GetSkillTotalValue(Common.SkillType.HUNTING), false);
			skillFarmingTotalValueLabel = WidgetFactory.CreateLabel("" + character.GetSkillTotalValue(Common.SkillType.FARMING), false);
			skillWoodcuttingTotalValueLabel = WidgetFactory.CreateLabel("" + character.GetSkillTotalValue(Common.SkillType.WOODCUTTING), false);
			skillMiningTotalValueLabel = WidgetFactory.CreateLabel("" + character.GetSkillTotalValue(Common.SkillType.MINING), false);
			skillMagicTotalValueLabel = WidgetFactory.CreateLabel("" + character.GetSkillTotalValue(Common.SkillType.MAGIC), false);
			skillMeleeTotalValueLabel = WidgetFactory.CreateLabel("" + character.GetSkillTotalValue(Common.SkillType.MELEE), false);
			skillRangedTotalValueLabel = WidgetFactory.CreateLabel("" + character.GetSkillTotalValue(Common.SkillType.RANGED), false);
			skillStealthTotalValueLabel = WidgetFactory.CreateLabel("" + character.GetSkillTotalValue(Common.SkillType.STEALTH), false);
			skillDiplomacyTotalValueLabel = WidgetFactory.CreateLabel("" + character.GetSkillTotalValue(Common.SkillType.DIPLOMACY), false);
			skillCraftingTotalValueLabel = WidgetFactory.CreateLabel("" + character.GetSkillTotalValue(Common.SkillType.CRAFTING), false);
			skillBuildingTotalValueLabel = WidgetFactory.CreateLabel("" + character.GetSkillTotalValue(Common.SkillType.BUILDING), false);
			skillProphecyTotalValueLabel = WidgetFactory.CreateLabel("" + character.GetSkillTotalValue(Common.SkillType.PROPHECY), false);
			
			// Attatch labels
			skillsTable.Attach (skillHuntingTotalValueLabel, 1, 2, 0, 1);
			skillsTable.Attach (skillFarmingTotalValueLabel, 1, 2, 1, 2);
			skillsTable.Attach (skillWoodcuttingTotalValueLabel, 1, 2, 2, 3);
			skillsTable.Attach (skillMiningTotalValueLabel, 1, 2, 3, 4);
			skillsTable.Attach (skillMagicTotalValueLabel, 1, 2, 4, 5);
			skillsTable.Attach (skillMeleeTotalValueLabel, 1, 2, 5, 6);
			skillsTable.Attach (skillRangedTotalValueLabel, 1, 2, 6, 7);
			skillsTable.Attach (skillStealthTotalValueLabel, 1, 2, 7, 8);
			skillsTable.Attach (skillDiplomacyTotalValueLabel, 1, 2, 8, 9);
			skillsTable.Attach (skillCraftingTotalValueLabel, 1, 2, 9, 10);
			skillsTable.Attach (skillBuildingTotalValueLabel, 1, 2, 10, 11);
			skillsTable.Attach (skillProphecyTotalValueLabel, 1, 2, 11, 12);
			
			// Skills table col 3 - Detailed value
			// Initialize labels
			skillHuntingDetailedValueLabel = WidgetFactory.CreateLabel (
				"(" + character.SkillSet.Skills[(int)Common.SkillType.HUNTING] + " + " + character.GetSkillBonusValue(Common.SkillType.HUNTING) + ")", false);
			skillHuntingDetailedValueLabel.ModifyFg(StateType.Normal, new Gdk.Color(211,211,211) );
			
			skillFarmingDetailedValueLabel = WidgetFactory.CreateLabel (
				"(" + character.SkillSet.Skills[(int)Common.SkillType.FARMING] + " + " + character.GetSkillBonusValue(Common.SkillType.FARMING) + ")", false);
			skillFarmingDetailedValueLabel.ModifyFg(StateType.Normal, new Gdk.Color(211,211,211) );
			
			skillWoodcuttingDetailedValueLabel= WidgetFactory.CreateLabel (
				"(" + character.SkillSet.Skills[(int)Common.SkillType.WOODCUTTING] + " + " + character.GetSkillBonusValue(Common.SkillType.WOODCUTTING) + ")" , false);
			skillWoodcuttingDetailedValueLabel.ModifyFg(StateType.Normal, new Gdk.Color(211,211,211) );
			
			skillMiningDetailedValueLabel= WidgetFactory.CreateLabel (
				"(" + character.SkillSet.Skills[(int)Common.SkillType.MINING] + " + " + character.GetSkillBonusValue(Common.SkillType.MINING) + ")" , false);
			skillMiningDetailedValueLabel.ModifyFg( StateType.Normal, new Gdk.Color(211,211,211) );
			
			skillMagicDetailedValueLabel= WidgetFactory.CreateLabel (
				"(" + character.SkillSet.Skills[(int)Common.SkillType.MAGIC] + " + " + character.GetSkillBonusValue(Common.SkillType.MAGIC) + ")", false);
			skillMagicDetailedValueLabel.ModifyFg( StateType.Normal, new Gdk.Color(211,211,211) );
			
			skillMeleeDetailedValueLabel= WidgetFactory.CreateLabel (
				"(" + character.SkillSet.Skills[(int)Common.SkillType.MELEE] + " + " + character.GetSkillBonusValue(Common.SkillType.MELEE) + ")", false);
			skillMeleeDetailedValueLabel.ModifyFg( StateType.Normal, new Gdk.Color(211,211,211) );
			
			skillRangedDetailedValueLabel= WidgetFactory.CreateLabel (
				"(" + character.SkillSet.Skills[(int)Common.SkillType.RANGED] + " + " + character.GetSkillBonusValue(Common.SkillType.RANGED) + ")", false);
			skillRangedDetailedValueLabel.ModifyFg( StateType.Normal, new Gdk.Color(211,211,211) );
			
			skillStealthDetailedValueLabel= WidgetFactory.CreateLabel (
				"(" + character.SkillSet.Skills[(int)Common.SkillType.STEALTH] + " + " + character.GetSkillBonusValue(Common.SkillType.STEALTH) + ")", false);
			skillStealthDetailedValueLabel.ModifyFg( StateType.Normal, new Gdk.Color(211,211,211) );
			
			skillDiplomacyDetailedValueLabel= WidgetFactory.CreateLabel (
				"(" + character.SkillSet.Skills[(int)Common.SkillType.DIPLOMACY] + " + " + character.GetSkillBonusValue(Common.SkillType.DIPLOMACY) + ")", false);
			skillDiplomacyDetailedValueLabel.ModifyFg( StateType.Normal, new Gdk.Color(211,211,211) );
			
			skillCraftingDetailedValueLabel= WidgetFactory.CreateLabel (
				"(" + character.SkillSet.Skills[(int)Common.SkillType.CRAFTING] + " + " + character.GetSkillBonusValue(Common.SkillType.CRAFTING) + ")", false);
			skillCraftingDetailedValueLabel.ModifyFg( StateType.Normal, new Gdk.Color(211,211,211) );
			
			skillBuildingDetailedValueLabel= WidgetFactory.CreateLabel (
				"(" + character.SkillSet.Skills[(int)Common.SkillType.BUILDING] + " + " + character.GetSkillBonusValue(Common.SkillType.BUILDING) + ")", false);
			skillBuildingDetailedValueLabel.ModifyFg( StateType.Normal, new Gdk.Color(211,211,211) );
			
			skillProphecyDetailedValueLabel= WidgetFactory.CreateLabel (
				"(" + character.SkillSet.Skills[(int)Common.SkillType.PROPHECY] + " + " + character.GetSkillBonusValue(Common.SkillType.PROPHECY) + ")", false);
			skillProphecyDetailedValueLabel.ModifyFg( StateType.Normal, new Gdk.Color(211,211,211) );
			
			// Attatch labels
			skillsTable.Attach (skillHuntingDetailedValueLabel, 2, 3, 0, 1);
			skillsTable.Attach (skillFarmingDetailedValueLabel, 2, 3, 1, 2);
			skillsTable.Attach (skillWoodcuttingDetailedValueLabel, 2, 3, 2, 3);
			skillsTable.Attach (skillMiningDetailedValueLabel, 2, 3, 3, 4);
			skillsTable.Attach (skillMagicDetailedValueLabel, 2, 3, 4, 5);
			skillsTable.Attach (skillMeleeDetailedValueLabel, 2, 3, 5, 6);
			skillsTable.Attach (skillRangedDetailedValueLabel, 2, 3, 6, 7);
			skillsTable.Attach (skillStealthDetailedValueLabel, 2, 3, 7, 8);
			skillsTable.Attach (skillDiplomacyDetailedValueLabel, 2, 3, 8, 9);
			skillsTable.Attach (skillCraftingDetailedValueLabel, 2, 3, 9, 10);
			skillsTable.Attach (skillBuildingDetailedValueLabel, 2, 3, 10, 11);
			skillsTable.Attach (skillProphecyDetailedValueLabel, 2, 3, 11, 12);
		}
	}
}