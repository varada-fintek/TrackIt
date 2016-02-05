<%@ Page Language="C#" MasterPageFile="~/master-templates/Layout.Master" AutoEventWireup="true"
    CodeBehind="UserList.aspx.cs" Inherits="ProjMngTrack.WebApp.Security.UserList" Theme="Controls" %>
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
                            	
                        <table width="100%" cellpadding="0" cellspacing="2" border="0" class="DataGridStyleTable">
                            <tr>
                                <td width="15%" class="DataGridStyleText"><asp:Label ID="lblUserName" runat="server" CssClass="label"></asp:Label></td>
                                <td width="30%" class="DataGridStyleFormArea">
                                    <asp:TextBox ID="txtUserName" runat="server" CssClass="textbox" TabIndex="1" MaxLength="15"></asp:TextBox>
                                </td>
                                <td width="15%" class="DataGridStyleText"><asp:Label ID="lblRoleName" CssClass="label" runat="server"></asp:Label></td>
                                <td width="30%" class="DataGridStyleFormArea">
                                    <asp:DropDownList ID="ddlRoleName" runat="server" CssClass="textbox" TabIndex="2">
                                    </asp:DropDownList>
                                </td>                                
                            </tr>
                            <tr>
                                <td width="15%" class="DataGridStyleText"><asp:Label ID="lblStaffName" runat="server" CssClass="label"></asp:Label></td>
                                <td width="30%" class="DataGridStyleFormArea">
                                    <asp:TextBox ID="txtStaffName" runat="server" CssClass="textbox" TabIndex="3" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    
                    </td>
                </tr>
                <tr><td colspan="3" class="DataGridTableFooter"></td></tr>
                <tr>
                    <td align="right" colspan="3">
                        <asp:Button ID="btnSearch" runat="server" CssClass="buttonSearch" TabIndex="3" OnClick="btnSearch_Click"  />
                        <asp:Button ID="btnClear" runat="server" CssClass="buttonClear" TabIndex="4" OnClick="btnClear_Click" />
                        <asp:Button ID="btnAdd" runat="server" CssClass="buttonAdd" TabIndex="5" OnClick="btnAdd_Click" />
                    </td>
                </tr>
                <tr><td>&nbsp;</td></tr>
                <tr>
                    <td colspan="3" class="DataGridTableBody">
                        <table border="0" align="center" width="100%" cellpadding="2" cellspacing="2">
                            <tr>
                                <td>
                                    <uc:NoRecords ID="ucNoRecords" runat="server" Visible="false" />
                                    <asp:GridView ID="gvUser" runat="server" Width="100%" AutoGenerateColumns="false"
                                        SkinID="gridviewSkin" OnRowCreated="RowCreated" AllowSorting="True" OnSorting="gvUser_RowSorting"
                                        OnRowDataBound="gvUser_RowDataBound" OnRowDeleting="gvUser_RowDeleting">
                                        <Columns>
                                            <asp:TemplateField SortExpression="User_Name">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLoginName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem , "User_Name") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Staff_Name">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="25%" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUserName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem , "Staff_Name") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Position">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPosition" runat="server" Text='<%# DataBinder.Eval(Container.DataItem , "Position") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="5%"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="lblEdit" runat="server" ImageUrl="~/images/edit_icon.png" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"Users_ID","~/Security/Users.aspx?UsersID={0}" ) %>'></asp:HyperLink>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                                        ImageAlign="Middle" ImageUrl="~/images/delete_icon.png" />
                                                    <asp:Label ID="lblUsersID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem , "Users_ID") %>' />
                                                    <asp:Label ID="lblRowVersion" runat="server" Visible="false" Text='<%# Conversion.ByteToString((byte[])DataBinder.Eval(Container.DataItem , "Row_Version")) %>' />
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
                <tr><td colspan="3" class="DataGridTableFooter"></td></tr>  
            </table>
            <asp:HiddenField ID="hdnAcsDesc" runat="server" />
            <asp:HiddenField ID="hdnSortExpression" runat="server" />
            <asp:HiddenField ID="hdnPageCount" runat="server" />
            <asp:HiddenField ID="hdnIndex" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
