<%@ Page Language="C#" MasterPageFile="~/master-templates/Layout.Master" AutoEventWireup="true"
    CodeBehind="RoleList.aspx.cs" Inherits="ProjMngTrack.WebApp.Security.RoleList"
    Theme="Controls" %>

<%@ Register TagPrefix="uc" TagName="Paging" Src="~/UserControls/PagingControl.ascx" %>
<%@ Register TagPrefix="uc" TagName="NoRecords" Src="~/UserControls/NoRecords.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="up" runat="server">
        <ContentTemplate>
            <table width="100%" border="0" cellpadding="0" cellspacing="0" class="DataGridTable">
                <tr>
                    <td class="DataGridTableLeftHeader"></td>
                    <td class="DataGridTableMiddleHeader">&nbsp;<asp:Label ID="lblSearchHeader" runat="server" Text=""></asp:Label></td>
                    <td class="DataGridTableRightHeader"></td>
                </tr>
                <tr>
                    <td colspan="3" class="DataGridTableBody">

                        <table width="100%" align="center" cellpadding="0" cellspacing="0" border="0" class="DataGridStyleTable">
                            <tr>
                                <td width="30%" class="DataGridStyleText" style="height: 27px">
                                    <asp:Label CssClass="label" runat="server">Business Unit</asp:Label>
                                </td>
                                <td width="60%" class="DataGridStyleFormArea" style="height: 27px">
                                    <asp:DropDownList ID="cmbBusinessUnit" runat="server" CssClass="textbox" TabIndex="1" AutoPostBack="true"
                                        Width="180px">
                                    </asp:DropDownList>

                                </td>
                            </tr>
                            <tr>
                                <td width="20%" class="DataGridStyleText">
                                    <asp:Label ID="lblRoleName" runat="server" CssClass="label"></asp:Label></td>
                                <td width="30%" class="DataGridStyleFormArea">
                                    <asp:TextBox ID="txtRoleName" CssClass="textbox" runat="server" TabIndex="2"></asp:TextBox>
                                </td>
                                <td width="35%">&nbsp;</td>
                            </tr>
                        </table>

                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="DataGridTableFooter"></td>
                </tr>
                <tr>
                    <td align="right" colspan="3">
                        <asp:Button ID="btnSearch" runat="server" CssClass="buttonSearch" OnClick="btnSearch_Click" />
                        <asp:Button ID="btnClear" runat="server" CssClass="buttonClear" OnClick="btnClear_Click" />
                        <asp:Button ID="btnAdd" runat="server" CssClass="buttonAdd" OnClick="btnAdd_Click" />
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3" class="DataGridTableBody">
                        <table border="0" align="center" width="100%" cellpadding="2" cellspacing="2">
                            <tr>
                                <td>
                                    <uc:NoRecords ID="ucNoRecords" runat="server" Visible="false" />
                                    <asp:GridView ID="gvRole" runat="server" Width="100%" AutoGenerateColumns="false"
                                        SkinID="gridviewSkin" OnRowCreated="RowCreated" AllowSorting="True" OnSorting="gvRole_RowSorting"
                                        OnRowDataBound="gvRole_RowDataBound" OnRowDeleting="gvRole_RowDeleting">
                                        <Columns>
                                            <asp:TemplateField SortExpression="Role_Name">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="25%" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRoleName" runat="server" Text='<%# DataBinder.Eval( Container.DataItem,"Role_Name" ) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Is_Predefined">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="center" Width="10%" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIsAdmin" runat="server" Text='<%# DataBinder.Eval( Container.DataItem,"Is_Predefined" ) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="5%"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="lblEdit" runat="server" ImageUrl="~/images/edit_icon.png" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"Role_ID","~/Security/Roles.aspx?RoleID={0}" ) %>'></asp:HyperLink>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                                        ImageAlign="Middle" Visible="false" ImageUrl="~/images/delete_icon.png" />
                                                    <asp:Label ID="lblRoleID" runat="server" Visible="false" Text='<%# DataBinder.Eval( Container.DataItem,"Role_ID" ) %>' />
                                                    <asp:Label ID="lblRowVersion" runat="server" Visible="false" Text='<%# Conversion.ByteToString((byte[]) DataBinder.Eval( Container.DataItem,"Row_Version" ) ) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="3">
                                    <uc:Paging ID="ucPaging" runat="server"></uc:Paging>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="DataGridTableFooter"></td>
                </tr>
            </table>
            <asp:HiddenField ID="hdnAcsDesc" runat="server" />
            <asp:HiddenField ID="hdnSortExpression" runat="server" />
            <asp:HiddenField ID="hdnPageCount" runat="server" />
            <asp:HiddenField ID="hdnIndex" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
