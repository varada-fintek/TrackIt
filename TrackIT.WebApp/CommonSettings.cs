using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Infragistics.Web.UI.GridControls;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TrackIT.WebApp
{
    public class CommonSettings
    {
        public static void ApplyGridSettings(WebDataGrid lwdg_CommonWebDataGrid)
        {
            //lwdg_CommonWebDataGrid.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            lwdg_CommonWebDataGrid.Behaviors.CreateBehavior<Sorting>();
            lwdg_CommonWebDataGrid.Behaviors.CreateBehavior<Paging>();
            lwdg_CommonWebDataGrid.Behaviors.CreateBehavior<Filtering>();

            lwdg_CommonWebDataGrid.Behaviors.Sorting.Enabled = true;
            SortingColumnSetting scs = new SortingColumnSetting(lwdg_CommonWebDataGrid);
            scs.ColumnKey = "Action";
            scs.Sortable = false;
            lwdg_CommonWebDataGrid.Behaviors.Sorting.ColumnSettings.Add(scs);

            lwdg_CommonWebDataGrid.Behaviors.Paging.Enabled = true;
            lwdg_CommonWebDataGrid.Behaviors.Paging.PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            lwdg_CommonWebDataGrid.Behaviors.Paging.QuickPages = Convert.ToInt32(ConfigurationManager.AppSettings["QuickPages"]);
            switch (Convert.ToString(ConfigurationManager.AppSettings["PagerAppearance"]))
            {
                case "Both":
                    lwdg_CommonWebDataGrid.Behaviors.Paging.PagerAppearance = PagerAppearance.Both;
                    break;
                case "Top":
                    lwdg_CommonWebDataGrid.Behaviors.Paging.PagerAppearance = PagerAppearance.Top;
                    break;
                case "Bottom":
                    lwdg_CommonWebDataGrid.Behaviors.Paging.PagerAppearance = PagerAppearance.Bottom;
                    break;
                default:
                    lwdg_CommonWebDataGrid.Behaviors.Paging.PagerAppearance = PagerAppearance.Bottom;
                    break;
            }

            //lwdg_CommonWebDataGrid.Behaviors.Paging.PagerCssClass = "lightblue";
            lwdg_CommonWebDataGrid.Behaviors.CreateBehavior<Infragistics.Web.UI.GridControls.ColumnResizing>();
            lwdg_CommonWebDataGrid.Behaviors.Paging.PagerMode = Infragistics.Web.UI.GridControls.PagerMode.NumericFirstLast;
            lwdg_CommonWebDataGrid.Behaviors.Filtering.Enabled = true;
            lwdg_CommonWebDataGrid.Width = Unit.Percentage(100);


            //lwdg_UserMasterGrid.Behaviors.CreateBehavior<ColumnMoving>();
            //lwdg_UserMasterGrid.Behaviors.ColumnMoving.Enabled = true;
            //lwdg_UserMasterGrid.Behaviors.CreateBehavior<ColumnFixing>();
            //lwdg_UserMasterGrid.Behaviors.ColumnFixing.Enabled = true;

        }

    }

    public class CustomItemTemplateEdit : ITemplate
    {
        #region ITemplate Members

        public void InstantiateIn(Control container)
        {

            //ImageButton edit = new ImageButton();
            LinkButton edit = new LinkButton();
            edit.CssClass = "LinkButton icon-pencil";
            //edit.Text = "Edit";// +container.ID;
            //edit.ImageUrl = "";
            edit.ID = container.ID;
            edit.Width = 20;
            edit.OnClientClick = "return edit(this)";
            //lwdg_grid.Behaviors.Sorting.ColumnSettings.RemoveAt(1);

            container.Controls.Add(edit);
        }
        #endregion
    }
    public class CustomItemTemplateEditDyn : ITemplate
    {
        #region ITemplate Members

        public void InstantiateIn(Control container)
        {

            //ImageButton edit = new ImageButton();
            LinkButton edit = new LinkButton();
            edit.CssClass = "LinkButton icon-pencil";
            //edit.Text = "Edit";// +container.ID;
            //edit.ImageUrl = "";
            edit.ID = container.ID;
            edit.Width = 20;
            edit.OnClientClick = "return editRows(this)";
            //lwdg_grid.Behaviors.Sorting.ColumnSettings.RemoveAt(1);

            container.Controls.Add(edit);
        }

        #endregion
    }

    public class CustomItemTemplate : ITemplate
    {
        #region ITemplate Members

        public void InstantiateIn(Control container)
        {
            //ImageButton edit = new ImageButton();
            LinkButton edit = new LinkButton();
            edit.CssClass = "LinkButton icon-pencil";
            //edit.Text = "Edit";// +container.ID;
            //edit.ImageUrl = "";
            edit.ID = container.ID;
            edit.Width = 20;
            edit.OnClientClick = "return editRow(this)";
            //lwdg_grid.Behaviors.Sorting.ColumnSettings.RemoveAt(1);

            container.Controls.Add(edit);
        }

        #endregion
    }
}
public class CustomItemTemplateView : ITemplate
{
    #region ITemplate Members

    public void InstantiateIn(Control container)
    {

        //ImageButton edit = new ImageButton();
        LinkButton edit = new LinkButton();
        edit.CssClass = "menu-icon fa fa-lg fa-fw fa-pencil";
        //edit.CssClass = "LinkButton eye-icon";

        //edit.Text = "View";// +container.ID;
        //edit.ImageUrl = "";
        edit.ID = container.ID;
        edit.OnClientClick = "return editRow(this)";
        container.Controls.Add(edit);

    }

    #endregion
}
public class CustomItemTemplateModel : ITemplate
{
    #region ITemplate Members

    public void InstantiateIn(Control container)
    {

        //ImageButton edit = new ImageButton();
        LinkButton edit = new LinkButton();
        edit.CssClass = "icon-circle-arrow-down";
        edit.ID = container.ID;
        container.Controls.Add(edit);
    }

    #endregion
}
public class CustomItemTemplateDelete : ITemplate
{
    #region ITemplate Members

    public void InstantiateIn(Control container)
    {

        LinkButton edit = new LinkButton();
        edit.CssClass = "LinkButton icon-trash";

        edit.ID = container.ID;
        edit.OnClientClick = "return DeleteRow(this)";
        container.Controls.Add(edit);

    }

    #endregion
}
public class CustomItemTemplateEmpty : ITemplate
{
    #region ITemplate Members

    public void InstantiateIn(Control container)
    {
        Label emptylabel = new Label();
        emptylabel.Text = "";
        emptylabel.ID = container.ID;
        container.Controls.Add(emptylabel);
    }

    #endregion
}