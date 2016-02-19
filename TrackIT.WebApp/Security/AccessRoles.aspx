<%@ Page Language="C#" MasterPageFile="~/master-templates/Layout.Master" AutoEventWireup="true" CodeBehind="AccessRoles.aspx.cs"
    Inherits="TrackIT.WebApp.Security.AccessRoles" Theme="Controls" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>




<%@ Register Assembly="Infragistics4.WebUI.WebDataInput.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>

<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.ListControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI" TagPrefix="ig" %>





<asp:Content ID="Content2" ContentPlaceHolderID="adminhead" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">

        function ShowModalPopup() {
            $find("mpe").show();
            return false;
        }
        function HideModalPopup() {
            $find("mpe").hide();
            document.getElementById("createnew").style.display = "block";
            return false;
        }
        function editRow(obj) {
            //Unit Testing- Security_ASPX_001
            // var grid = $find("lwdg_FundMasterGrid");
            var row = $(obj).parents("tr[type='row']").get(0);
            var rowid = row.cells[1].innerHTML;
            var pop_open = '1';
            document.getElementById("hdnRoleID").value = rowid;
            document.getElementById("hdnpop").value = pop_open;
            //$find("mpe").show();
            return true;
        }
        function removequery() {
            //alert();
            var pop_open = '0';
            document.getElementById("hdnpop").value = pop_open;
        }

        function MinimumOneCheckboxSelected(objView, objAdd, objEdit, objDelete) {
            var objChkView = document.getElementById(objView);
            var objChkAdd = document.getElementById(objAdd);
            var objChkEdit = document.getElementById(objEdit);
            var objChkDelete = document.getElementById(objDelete);

            if (objChkAdd != null) {
                if (objChkAdd.checked) {
                    if (objChkView != null) {
                        objChkView.checked = true;
                    }
                }
            }

            if (objChkEdit != null) {
                if (objChkEdit.checked) {
                    if (objChkView != null) {
                        objChkView.checked = true;
                    }
                }
            }

            if (objChkDelete != null) {
                if (objChkDelete.checked) {
                    if (objChkView != null) {
                        objChkView.checked = true;
                    }
                }
            }
        }

        function UnSelectCheckboxes(objView, objAdd, objEdit, objDelete) {
            var objChkView = document.getElementById(objView);
            var objChkAdd = document.getElementById(objAdd);
            var objChkEdit = document.getElementById(objEdit);
            var objChkDelete = document.getElementById(objDelete);

            if (objChkView != null) {
                if (!objChkView.checked) {
                    if (objChkAdd != null) {
                        objChkAdd.checked = false;
                    }

                    if (objChkEdit != null) {
                        objChkEdit.checked = false;
                    }

                    if (objChkDelete != null) {
                        objChkDelete.checked = false;
                    }
                }
            }
        }

        function fnViewSelectAll(chkViewSelectAll, chkAddSelectAll, chkEditSelectAll, chkDeleteSelectAll) {
            var objchkViewSelectAll = document.getElementById(chkViewSelectAll)
            var objchkAddSelectAll = document.getElementById(chkAddSelectAll)
            var objchkEditSelectAll = document.getElementById(chkEditSelectAll)
            var objchkDeleteSelectAll = document.getElementById(chkDeleteSelectAll)

            var arr = document.getElementsByTagName("INPUT");

            if (objchkViewSelectAll.checked) {
                for (var i = 0; i < arr.length; i++) {
                    if (arr[i].id.indexOf("chkView") >= 0) {
                        arr[i].checked = true;
                    }
                }
            }
            else {
                objchkAddSelectAll.checked = false;
                objchkEditSelectAll.checked = false;
                //objchkDeleteSelectAll.checked = false;

                for (var i = 0; i < arr.length; i++) {
                    if ((arr[i].id.indexOf("chkView") >= 0) || (arr[i].id.indexOf("chkEdit") >= 0) || (arr[i].id.indexOf("chkAdd") >= 0) || (arr[i].id.indexOf("chkDelete") >= 0)) {
                        arr[i].checked = false;
                    }
                }
            }
        }

        function fnAddSelectAll(chkAddSelectAll, chkViewSelectAll) {
            var objchkAddSelectAll = document.getElementById(chkAddSelectAll)
            var objchkViewSelectAll = document.getElementById(chkViewSelectAll)

            var arr = document.getElementsByTagName("INPUT");

            if (objchkAddSelectAll.checked) {
                objchkViewSelectAll.checked = true;

                for (var i = 0; i < arr.length; i++) {
                    if ((arr[i].id.indexOf("chkView") >= 0) || arr[i].id.indexOf("chkAdd") >= 0) {
                        arr[i].checked = true;
                    }
                }
            }
            else {
                for (var i = 0; i < arr.length; i++) {
                    if (arr[i].id.indexOf("chkAdd") >= 0) {
                        arr[i].checked = false;
                    }
                }
            }
        }

        function fnEditSelectAll(chkEditSelectAll, chkViewSelectAll) {
            var objchkEditSelectAll = document.getElementById(chkEditSelectAll)
            var objchkViewSelectAll = document.getElementById(chkViewSelectAll)

            var arr = document.getElementsByTagName("INPUT");

            if (objchkEditSelectAll.checked) {
                objchkViewSelectAll.checked = true;

                for (var i = 0; i < arr.length; i++) {
                    if ((arr[i].id.indexOf("chkView") >= 0) || arr[i].id.indexOf("chkEdit") >= 0) {
                        arr[i].checked = true;
                    }
                }
            }
            else {
                for (var i = 0; i < arr.length; i++) {
                    if (arr[i].id.indexOf("chkEdit") >= 0) {
                        arr[i].checked = false;
                    }
                }
            }
        }

        function fnDeleteSelectAll(chkDeleteSelectAll, chkViewSelectAll) {
            var objchkDeleteSelectAll = document.getElementById(chkDeleteSelectAll)
            var objchkViewSelectAll = document.getElementById(chkViewSelectAll)

            var arr = document.getElementsByTagName("INPUT");

            if (objchkDeleteSelectAll.checked) {
                objchkViewSelectAll.checked = true;

                for (var i = 0; i < arr.length; i++) {
                    if ((arr[i].id.indexOf("chkView") >= 0) || arr[i].id.indexOf("chkDelete") >= 0) {
                        arr[i].checked = true;
                    }
                }
            }
            else {
                for (var i = 0; i < arr.length; i++) {
                    if (arr[i].id.indexOf("chkDelete") >= 0) {
                        arr[i].checked = false;
                    }
                }
            }
        }

    </script>
       <style>

        .modal-body{
                max-height: 560px !important;
                top:0;
        }
    </style>
    <asp:UpdatePanel ID="uplState" runat="server">
        <ContentTemplate>
            <div class="main-container" id="main-container">
                <div class="page-header">
                    <div class="floatright pull_right">
                        <asp:ImageButton ID="btnExportExcel" runat="server" CausesValidation="False" ImageAlign="Middle" ImageUrl="~/images/excel_icon.png" OnClick="btnExportExcel_Click"></asp:ImageButton>
                        &nbsp;&nbsp;
                        
                              <asp:ImageButton ID="btnExportPDF" runat="server" CausesValidation="False" ImageAlign="Middle" ImageUrl="~/images/pdf_icon.png" OnClick="btnExportPDF_Click"></asp:ImageButton>&nbsp;&nbsp;

                             <span class="custom-createnew" style="float: right;" id="createnew" clientidmode="Static" runat="server">
                                 <a href="#" onclick="show();"></a></span>

                    </div>
                    <ig:WebDocumentExporter ID="WebPDFExporter" runat="server" ExportMode="Download" DownloadName="RoleMasterPDF" DataExportMode="AllDataInDataSource" Format="PDF" />
                    <ig:WebExcelExporter runat="server" ID="WebExcelExporter" ExportMode="Download" DownloadName="RoleMasterExcel" DataExportMode="AllDataInDataSource" />

                    <h1>
                        <i class="fa fa-gears"></i>
                        <asp:Label ID="lblCreateRoles" runat="server"></asp:Label>

                    </h1>
                </div>
                <div runat="server" id="pnl_RoleGrid"></div>

                <script type="text/javascript">
                    try { ace.settings.check('main-container', 'fixed') } catch (e) { }
                </script>
                <div class="main-content">
                    <asp:Button ID="btnShow" runat="server" Text="Show Modal Popup" Visible="false" OnClientClick="return ShowModalPopup()" />
                    <asp:LinkButton ID="lnkDummy" runat="server"></asp:LinkButton>
                    <cc1:ModalPopupExtender ID="ModalPopupExtender1" BehaviorID="mpe" runat="server" CancelControlID="lnkDummy"
                        PopupControlID="pnlPopup" TargetControlID="createnew" BackgroundCssClass="modal-backdrop">
                    </cc1:ModalPopupExtender>

                    <asp:Panel ID="pnlPopup" runat="server" CssClass="modal-dialog" Style="display: none;">
                        <asp:Panel ID="pnlcontent" runat="server" CssClass="modal-content">
                            <asp:Panel ID="pnlheader" runat="server" CssClass="modal-header-green">
                                <h4 class="modal-title">Roles</h4>
                            </asp:Panel>


                            <asp:Panel ID="pnlbody" runat="server" CssClass="modal-body text-center">
                                <div class="form-horizontal">

                                    <asp:ValidationSummary ID="valSumRoles" runat="server" ShowMessageBox="true" ShowSummary="false"  ValidationGroup="vgrpSave" />
                                    <div class="form-group">
                                        <asp:Label ID="lblRoleCode" class="control-label col-md-2" runat="server"></asp:Label>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtRoleCode" CssClass="form-control" runat="server" TabIndex="3" MaxLength="10"></asp:TextBox>
                                        </div>
                                        <div class="col-md-1">
                                            <cc1:FilteredTextBoxExtender ID="txtRoleCode_FilteredTextBoxExtender"
                                                runat="server" Enabled="True" TargetControlID="txtRoleCode"
                                                ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                            </cc1:FilteredTextBoxExtender>
                                            <asp:RequiredFieldValidator ID="reqvtxtRoleCode" runat="server" CssClass="validator_msg"
                                                ControlToValidate="txtRoleCode" Display="Dynamic" SetFocusOnError="True" Text="*"
                                                ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label ID="lblRoleName" class="control-label col-md-2" runat="server"></asp:Label>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtRoleName" CssClass="form-control" runat="server" TabIndex="4" MaxLength="50"></asp:TextBox>
                                        </div>
                                        <div class="col-md-1">
                                            <cc1:FilteredTextBoxExtender ID="txtRoleNameFilteredTextBoxExtender"
                                                runat="server" Enabled="True" TargetControlID="txtRoleName"
                                                ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                            </cc1:FilteredTextBoxExtender>
                                            <asp:RequiredFieldValidator ID="reqvtxtRoleName" runat="server" CssClass="validator_msg"
                                                ControlToValidate="txtRoleName" Display="Dynamic" SetFocusOnError="True" Text="*"
                                                ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <asp:Label ID="lblInactive" class="control-label col-md-2" Text="Inactive" runat="server"></asp:Label>
                                        <div class="col-md-3">
                                            <span class="input-icon">
                                                <asp:CheckBox ID="chkinactive" class="checkbox" runat="server" TabIndex="5" />
                                            </span>
                                        </div>
                                    </div>

                                    <%--        edit grid/create new grid        --%>
                       <%--             style="border: 1px solid #ede3e3; padding: 10px; margin-left: 50px; margin-bottom: 10px;"--%>
                                    <div class="col-sm-10">
                                        <asp:GridView ID="gvRoleAccess" runat="server" Width="100%" AutoGenerateColumns="false" TabIndex="6"
                                            CssClass="table table-bordered"  AllowSorting="True" OnRowDataBound="gvRoleAccess_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblScreenName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem , "Screen_Name") %>' />
                                                        <asp:Label ID="lblScreenID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem , "Screen_ID") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container.DataItem , "Category") %>' />
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
                                                <asp:TemplateField Visible="false">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                    <HeaderTemplate>
                                                        <asp:CheckBox runat="server" ID="htDelete" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkDelete" runat="server" Checked='<%#Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Delete")) == true ? true : false %>'
                                                            Visible='<%#Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Delete_Screen")) == true ? true : false %>' />
                                                        <asp:Label ID="lblDelete_Screen" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem , "Delete_Screen") %>' />
                                                        <asp:Label ID="lblSAMenu" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem , "SA_Menu") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </asp:Panel>

                            <asp:Panel ID="pnlfooter" runat="server" CssClass="modal-footer">
                                <div class="form-group">
                                    <div class="col-md-1 floatright">
                                        <asp:Button ID="btnClear" Text="Cancel" runat="server" CssClass="btn btn-orange" TabIndex="8" OnClick="btnClear_Click" CausesValidation="false" />
                                    </div>
                                    <div class="col-md-1 floatright">
                                        <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="btn btn-blue" TabIndex="7" ValidationGroup="grpSave" OnClientClick="removequery();" OnClick="btnSave_Click" />
                                    </div>
                                </div>

                            </asp:Panel>
                        </asp:Panel>
                    </asp:Panel>

                    <%--                        </div>
                      
                    </div>--%>
                </div>

                <a href="#" id="btn-scroll-up" class="btn-scroll-up btn btn-sm btn-inverse">
                    <i class="ace-icon fa fa-angle-double-up icon-only bigger-110"></i>
                </a>
            </div>
            <asp:HiddenField ID="hdnAcsDesc" runat="server" />
            <asp:HiddenField ID="hdnSortExpression" runat="server" />
            <asp:HiddenField ID="hdnPageCount" runat="server" />
            <asp:HiddenField ID="hdnIndex" runat="server" />
            <asp:HiddenField ID="hdnRowVersion" runat="server" />
            <asp:HiddenField ID="hdnRoleID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnpop" runat="server" ClientIDMode="Static" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportExcel" />
            <asp:PostBackTrigger ControlID="btnExportPDF" />
        </Triggers>
    </asp:UpdatePanel>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".e1").select2();
            $(".e1").css("border", "none");
        });
    </script>
</asp:Content>
