using System;
using Common;


namespace Client.Controller
{
	public class PageHandlerCharacterPage : PageHandler
	{

		public PageHandlerCharacterPage (View.ViewManager view, Model.ModelManager modelManager) :
		base(view, modelManager)
		{

		}

		override internal void HandleEvent (View.UserEvent userEvent)
		{
			switch (userEvent) {
			case View.UserEvent.CHARACTER_SHOW:
				//nothing needs to happen since the view has been loaded in the contructor with 
				//method SetView()
				break;
			case View.UserEvent.CHARACTER_TRAIN_STRENGTH:
				trainAttribute(Common.AttributeType.STRENGTH);
				break;
			case View.UserEvent.CHARACTER_TRAIN_AGILITY:
				trainAttribute(Common.AttributeType.AGILITY);
				break;
			case View.UserEvent.CHARACTER_TRAIN_ENDURANCE:
				trainAttribute(Common.AttributeType.ENDURANCE);
				break;
			case View.UserEvent.CHARACTER_TRAIN_INTELLIGENCE:
				trainAttribute(Common.AttributeType.INTELLIGENCE);
				break;
			case View.UserEvent.CHARACTER_TRAIN_PERCEPTION:
				trainAttribute(Common.AttributeType.PERCEPTION);
				break;
			case View.UserEvent.CHARACTER_TRAIN_WILLPOWER:
				trainAttribute(Common.AttributeType.WILLPOWER);
				break;

			default:
				Console.WriteLine ("no such event in HandlerCharacterPage");
				break;
			}
		}
		/// <summary>
		/// Sets the coresponding view.
		/// </summary>
		override internal void SetView ()
		{
			page = viewManager.LoadCharacterPage 
				(modelManager.GetCharacterManager().GetMyCharacter());
			UpdatePage();
            View.PageCharacter characterView = (View.PageCharacter)page;
            page.SetStatusMessage("");
            characterView.ClearAllAttributeStatusMessages();
		}

		override public void UpdatePage()
		{
			View.PageCharacter characterView = (View.PageCharacter) page;
			characterView.SetCharacter(modelManager.GetCharacterManager().GetMyCharacter());
		}
		/// <summary>
		/// Sends a message to server req to train an attribute.
		/// </summary>
		/// <param name='type'>
		/// Type.
		/// </param>
		private void trainAttribute (AttributeType type)
		{
			ClientTrainAttributeRequestMsg cta = new ClientTrainAttributeRequestMsg (type);
			try {
				modelManager.GetServerManager ().Send (cta);
			} catch (NWOException nwo_e) {
				handleLostConnection (nwo_e);
      		}
		}
	}
}

