<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeftMenu.ascx.cs" Inherits="TrackIT.WebApp.UserControls.LeftMenu" %>
<%--<table width="200px" height="20px" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td width="25px" class="leftMenuBorderBottom">&nbsp;<img src="../images/bullet.png" alt="" /></td>
        <td class="leftMenuBorderBottom"><a href="../Marketing/EnquiryList.aspx" class="leftMenu">Enquiry</a></td>
    </tr>
    <tr>
        <td width="25px" class="leftMenuBorderBottom">&nbsp;<img src="../images/bullet.png" alt="" /></td>
        <td class="leftMenuBorderBottom"><a href="../Marketing/FollowupList.aspx" class="leftMenu">Follow Up</a></td>
    </tr> 
</table>--%>
<table width="200px" height="20px" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td>
            <asp:Menu ID="mnLeftMenu" runat="server" Orientation="Vertical" Width="200px" OnMenuItemClick="mnLeftMenu_MenuItemClick">
                <StaticMenuStyle BorderStyle="None" />
                <StaticMenuItemStyle CssClass="leftMenu" HorizontalPadding="3px" VerticalPadding="3px"
                    ItemSpacing="0" />
                <StaticHoverStyle CssClass="leftMenu" />
                <StaticSelectedStyle CssClass="leftMenu" />
                <DynamicMenuStyle CssClass="leftMenu" />
                <DynamicMenuItemStyle CssClass="leftMenu" HorizontalPadding="3px" VerticalPadding="3px"
                    ItemSpacing="0" />
                <DynamicHoverStyle CssClass="leftMenu" />
            </asp:Menu>
        </td>
    </tr>
</table>
