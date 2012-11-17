﻿using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.WebControls;

namespace CCSAdvancedAlerts.Layouts.CCSAdvancedAlerts
{


    public partial class AdvancedAlertSettings : LayoutsPageBase
    {
        private const string alertSettingsListName = "CCSAdvancedAlertsList";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                PopulateSites();
            }
            this.btnsave.Click += new EventHandler(btnsave_Click);
            this.ddlSite.SelectedIndexChanged += new EventHandler(ddlSite_SelectedIndexChanged);
        }

       
        void ddlSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.PopulateLists(this.ddlSite.SelectedValue);
            }
            catch
            {
            }
        }


        private void PopulateSites()
        {
            try
            {
                SPSite site = SPContext.Current.Site;
                if (site != null)
                {
                    SPWebCollection allWebs = site.AllWebs;
                    foreach (SPWeb web in allWebs)
                    {
                        ListItem newWebItem = new ListItem(web.Title, web.ID.ToString());
                        if (!this.ddlSite.Items.Contains(newWebItem))
                        {
                            this.ddlSite.Items.Add(newWebItem);
                        }
 
                    }

                    this.PopulateLists(this.ddlSite.SelectedValue);
                }

            }
            catch 
            {
            }
        }

        private void PopulateLists(string webid)
        {
            try
            {
                SPListCollection allLists = SPContext.Current.Site.AllWebs[new Guid(webid)].Lists;
                if (allLists != null)
                {
                    foreach (SPList  list in allLists)
                    {
                        ListItem newListItem = new ListItem(list.Title, list.ID.ToString());
                        if (!this.ddlList.Items.Contains(newListItem))
                        {
                            this.ddlList.Items.Add(newListItem);
                        }

                    }
                }
            }
            catch
            {
            }
        }


        void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                SPList settingslist = SPContext.Current.Site.RootWeb.Lists.TryGetList(ListAndFieldNames.settingsListName);
                if (settingslist != null)
                {
                    SPListItem listItem = settingslist.AddItem();
                    listItem[ListAndFieldNames.settingsListWebIdFieldName] = ddlSite.SelectedValue;
                    listItem[ListAndFieldNames.settingsListListIdFieldName] = ddlList.SelectedValue;
                    listItem[ListAndFieldNames.settingsListMailBpdyFieldName] = "this is sample message";
                    listItem[ListAndFieldNames.settingsListSubjectFieldName] = "this is sample elert created by CCS";
                    listItem[ListAndFieldNames.settingsListToAddressFieldName] = txtTo.Text;
                    listItem[ListAndFieldNames.settingsListFromAddressFieldName] = txtFrom.Text;

                    listItem.Update();

                }
            }
            catch
            {
            }
        }


    }
}
