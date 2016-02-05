<%@ Page Language="C#" MasterPageFile="~/master-templates/Layout.Master" AutoEventWireup="true"
    CodeBehind="Users.aspx.cs" Inherits="ProjMngTrack.WebApp.Security.Users" Theme="Controls" %>

<%@ Register TagPrefix="uc" TagName="NoRecords" Src="~/UserControls/NoRecords.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">

        function MinimumOneCheckboxSelected(objView, objAdd, objEdit, objDelete) 
        {
            var objChkView = document.getElementById(objView);           
            var objChkAdd = document.getElementById(objAdd);
            var objChkEdit = document.getElementById(objEdit);
            var objChkDelete = document.getElementById(objDelete);

            if(objChkAdd != null)
            {
                if (objChkAdd.checked)
                {
                    if(objChkView != null)
                    {
                        objChkView.checked = true;
                    }                    
                }
            }
            
            if(objChkEdit != null)
            {
                if (objChkEdit.checked)
                {
                    if(objChkView != null)
                    {
                        objChkView.checked = true;
                    }                    
                }
            }
            
            if(objChkDelete != null)
            {
                if (objChkDelete.checked)
                {
                    if(objChkView != null)
                    {
                        objChkView.checked = true;
                    }                    
                }
            }
        }

        function UnSelectCheckboxes(objView, objAdd, objEdit, objDelete) 
        {
            var objChkView = document.getElementById(objView);            
            var objChkAdd = document.getElementById(objAdd);
            var objChkEdit = document.getElementById(objEdit);
            var objChkDelete = document.getElementById(objDelete);

            if (objChkView != null) 
            {            
                if (!objChkView.checked) 
                {
                    if(objChkAdd != null)
                    {
                        objChkAdd.checked = false;
                    }
                    
                    if(objChkEdit != null)
                    {
                        objChkEdit.checked = false;
                    }
                    
                    if(objChkDelete != null)
                    {
                        objChkDelete.checked = false;
                    }
                }
            }
        }

        function fnViewSelectAll( chkViewSelectAll , chkAddSelectAll ,chkEditSelectAll ,chkDeleteSelectAll ) 
        {
            var objchkViewSelectAll = document.getElementById(chkViewSelectAll)            
            var objchkAddSelectAll = document.getElementById(chkAddSelectAll)
            var objchkEditSelectAll = document.getElementById(chkEditSelectAll)
            var objchkDeleteSelectAll = document.getElementById(chkDeleteSelectAll)

            var arr = document.getElementsByTagName("INPUT");

            if (objchkViewSelectAll.checked) 
            {         
                for (var i = 0; i < arr.length; i++) 
                {
                    if (arr[i].id.indexOf("chkView") >= 0) 
                    {
                        arr[i].checked = true;
                    }
                }             
            }
            else 
            {
                objchkAddSelectAll.checked = false;
                objchkEditSelectAll.checked = false;
                objchkDeleteSelectAll.checked = false;
            
                for (var i = 0; i < arr.length; i++) 
                {
                    if ((arr[i].id.indexOf("chkView") >= 0) || (arr[i].id.indexOf("chkEdit") >= 0) || (arr[i].id.indexOf("chkAdd") >= 0) || (arr[i].id.indexOf("chkDelete") >= 0))
                    {
                        arr[i].checked = false;
                    }
                }              
            }
        }

        function fnAddSelectAll( chkAddSelectAll , chkViewSelectAll ) 
        {
            var objchkAddSelectAll = document.getElementById(chkAddSelectAll)
            var objchkViewSelectAll = document.getElementById(chkViewSelectAll)

            var arr = document.getElementsByTagName("INPUT");

            if (objchkAddSelectAll.checked) 
            {
                objchkViewSelectAll.checked = true;
                
                for (var i = 0; i < arr.length; i++) 
                {
                    if ((arr[i].id.indexOf("chkView") >= 0 ) || arr[i].id.indexOf("chkAdd") >= 0) 
                    {
                        arr[i].checked = true;
                    }
                }
            }
            else 
            {               
                for (var i = 0; i < arr.length; i++) 
                {
                    if (arr[i].id.indexOf("chkAdd") >= 0) {
                        arr[i].checked = false;
                    }
                }
            }
        }
        
        function fnEditSelectAll(chkEditSelectAll , chkViewSelectAll) 
        {
            var objchkEditSelectAll = document.getElementById(chkEditSelectAll)
            var objchkViewSelectAll = document.getElementById(chkViewSelectAll)

            var arr = document.getElementsByTagName("INPUT");

            if (objchkEditSelectAll.checked) 
            {
                objchkViewSelectAll.checked = true;
                
                for (var i = 0; i < arr.length; i++) 
                {
                    if ((arr[i].id.indexOf("chkView") >= 0 ) || arr[i].id.indexOf("chkEdit") >= 0) 
                    {
                        arr[i].checked = true;
                    }
                }
            }
            else 
            {                
                for (var i = 0; i < arr.length; i++) 
                {
                    if (arr[i].id.indexOf("chkEdit") >= 0) 
                    {
                        arr[i].checked = false;
                    }
                }
            }
        }

        function fnDeleteSelectAll( chkDeleteSelectAll , chkViewSelectAll ) 
        {
            var objchkDeleteSelectAll = document.getElementById(chkDeleteSelectAll)
            var objchkViewSelectAll = document.getElementById(chkViewSelectAll)

            var arr = document.getElementsByTagName("INPUT");

            if (objchkDeleteSelectAll.checked) 
            {
                objchkViewSelectAll.checked = true;
                
                for (var i = 0; i < arr.length; i++) 
                {
                    if ((arr[i].id.indexOf("chkView") >= 0 ) || arr[i].id.indexOf("chkDelete") >= 0) 
                    {
                        arr[i].checked = true;
                    }
                }
            }
            else 
            {               
                for (var i = 0; i < arr.length; i++) 
                {
                    if (arr[i].id.indexOf("chkDelete") >= 0) 
                    {
                        arr[i].checked = false;
                    }
                }
            }
        }       
                
    </script>

    <asp:UpdatePanel ID="up" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="UserAccessvalid" runat="server" ShowMessageBox="True" ShowSummary="False" DisplayMode="List" />
            <table width="100%" border="0" cellpadding="0" cellspacing="0" class="DataGridTable">
            	<tr>
                	<td class="DataGridTableLeftHeader"></td>
                    <td class="DataGridTableMiddleHeader">&nbsp;<asp:Label ID="lblUserHeader" runat="server"></asp:Label></td>
                    <td class="DataGridTableRightHeader"></td>
                </tr>
                <tr>
                	<td colspan="3" class="DataGridTableBody">
                                
                        <table width="100%" cellpadding="0" cellspacing="2" border="0" class="DataGridStyleTable">
                            <tr>
                                <td width="15%" class="DataGridStyleText"><asp:Label ID="lblUserName" CssClass="label" runat="server"></asp:Label></td>
                                <td width="30%" class="DataGridStyleFormArea">
                                    <asp:TextBox ID="txtUserName" runat="server" CssClass="textbox" MaxLength="15"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ControlToValidate="txtUserName"
                                        Text="*" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                </td>
                                <td width="15%" class="DataGridStyleText">
                                    <asp:Label ID="lblPassword" runat="server" CssClass="label"></asp:Label>
                                </td>
                                <td width="30%" runat="server" id="tdPassword" class="DataGridStyleFormArea">
                                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="textbox" MaxLength="30"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                                        Text="*" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td width="15%" class="DataGridStyleText">
                                    <asp:Label ID="lblRoleName" runat="server" CssClass="label"></asp:Label>
                                </td>
                                <td width="30%" class="DataGridStyleFormArea">
                                    <asp:DropDownList ID="ddlRoleName" runat="server" CssClass="textbox" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlRoleName_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvRoleName" runat="server" ControlToValidate="ddlRoleName"
                                        Text="*" SetFocusOnError="True" InitialValue=""></asp:RequiredFieldValidator>
                                </td>
                                <td width="15%" class="DataGridStyleText">
                                    <asp:Label ID="lblModuleName" runat="server" CssClass="label"></asp:Label>
                                </td>
                                <td width="30%" class="DataGridStyleFormArea">
                                    <asp:DropDownList ID="ddlModuleName" runat="server" CssClass="textbox" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlModuleName_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvModuleName" runat="server" ControlToValidate="ddlModuleName"
                                        Text="*" SetFocusOnError="True" InitialValue=""></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                                
                    </td>
                </tr>
                <tr><td colspan="3" class="DataGridTableFooter">&nbsp;</td></tr>
                <tr id="trHeadingRow" runat="server" visible ="false">
                    <td colspan="3" class="DataGridTableBody">
                        <table border="0" align="center" width="100%" cellpadding="2" cellspacing="2">
                            <tr>
                                <td>
                                    <uc:NoRecords ID="ucNoRecords" runat="server" Visible="false" />
                                    <asp:GridView ID="gvUserAccess" runat="server" Width="100%" AutoGenerateColumns="false"
                                        SkinID="gridviewSkin" OnRowCreated="RowCreated" AllowSorting="True" OnRowDataBound="gvUserAccess_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblScreenName" runat="server" Text='<%# DataBinder.Eval( Container.DataItem , "Screen_Name") %>' />
                                                    <asp:Label ID="lblScreenID" runat="server" Visible="false" Text='<%# Eval("Screen_ID") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCategory" runat="server" Text='<%# DataBinder.Eval( Container.DataItem , "Category") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                <HeaderTemplate>
                                                    <asp:CheckBox runat="server" ID="htView" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkView" runat="server" Checked='<%#Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"View")) == true ? true : false %>'
                                                        Visible='<%#Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"View_Screen")) == true ? true : false %>' />
                                                    <asp:Label ID="lblView_Screen" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem , "View_Screen") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                <HeaderTemplate>
                                                    <asp:CheckBox runat="server" ID="htAdd" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkAdd" runat="server" Checked='<%#Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Add")) == true ? true : false %>'
                                                        Visible='<%#Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Add_Screen")) == true ? true : false %>' />
                                                    <asp:Label ID="lblAdd_Screen" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem , "Add_Screen") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                <HeaderTemplate>
                                                    <asp:CheckBox runat="server" ID="htEdit" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkEdit" runat="server" Checked='<%#Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Edit")) == true ? true : false %>'
                                                        Visible='<%#Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Edit_Screen")) == true ? true : false %>' />
                                                    <asp:Label ID="lblEdit_Screen" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem , "Edit_Screen") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                <HeaderTemplate>
                                                    <asp:CheckBox runat="server" ID="htDelete" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkDelete" runat="server" Checked='<%#Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Delete")) == true ? true : false %>'
                                                        Visible='<%#Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Delete_Screen")) == true ? true : false %>' />
                                                    <asp:Label ID="lblDelete_Screen" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem , "Delete_Screen") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="trBottomRow" runat="server" visible ="false"><td colspan="3" class="DataGridTableFooter"></td></tr>
                <tr>
                    <td align="right" colspan="3">
                        <asp:Button ID="btnSave" runat="server" AccessKey="S" CssClass="buttonSave" TabIndex="3"
                            OnClick="btnSave_Click" />
                        <asp:Button ID="btnClear" runat="server" AccessKey="R" CssClass="buttonClear" CausesValidation="false"
                            TabIndex="4" OnClick="btnClear_Click" />
                        <asp:Button ID="btnCancel" runat="server" AccessKey="C" CssClass="buttonBack" CausesValidation="false"
                            TabIndex="5" OnClick="btnCancel_Click" />
                    </td>
                </tr>
            </table>                        
            <asp:HiddenField ID="hdnUserID" runat="server" />
            <asp:HiddenField ID="hdnRowVersion" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
