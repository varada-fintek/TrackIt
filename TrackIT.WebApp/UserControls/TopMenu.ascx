<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopMenu.ascx.cs" Inherits="TrackIT.WebApp.UserControls.TopMenu" %>
<table border="0" width="100%" cellpadding="0" cellspacing="0" align="center">
    <tr>
        <td class="topMenuBackgroundColor">
            <asp:Menu ID="mnTopMenu" runat="server" Orientation="Horizontal" OnMenuItemClick="mnTopMenu_MenuItemClick">
                <StaticMenuStyle BorderStyle="None" />
                <StaticMenuItemStyle CssClass="topMenu" HorizontalPadding="10px" VerticalPadding="2px"
                    ItemSpacing="9" />
                <StaticHoverStyle CssClass="topMenu" />
                
                <DynamicMenuStyle CssClass="topMenu" />
                <DynamicMenuItemStyle CssClass="topMenu" HorizontalPadding="10px" VerticalPadding="2px"
                    ItemSpacing="9" />
                <DynamicHoverStyle CssClass="topMenu" />
                <Items>
                    <%-- <asp:MenuItem Text="Master" Value="Master"></asp:MenuItem>
                                        <asp:MenuItem ImageUrl="~/images/vert_line_menu.jpg"></asp:MenuItem>
                                        <asp:MenuItem Text="Marketing" Value="Marketing"></asp:MenuItem>--%>
                </Items>
            </asp:Menu>
        </td>
    </tr>
</table>
