using System;

namespace Client.Controller
{
	public abstract class PageHandler : Handler
	{
		protected View.Page page;

		public PageHandler (View.ViewManager view, Model.ModelManager model) :
		base(view, model)
		{
			SetView();
		}
		/// <summary>
		/// Gets the page.
		/// </summary>
		/// <returns>
		/// The page.
		/// </returns>
		public View.Page GetPage ()
		{
			return page;
		}
		/// <summary>
		/// Updates the page.
		/// </summary>
		public abstract void UpdatePage();
		internal abstract void SetView();
	}
}

