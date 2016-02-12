
<%@ Page Language="C#" MasterPageFile="~/master-templates/Layout.Master" AutoEventWireup="true" CodeBehind="UserMaster.aspx.cs" Inherits="TrackIT.WebApp.Setup.UserMaster" %>

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
 <%--   <asp:scriptmanager id="ScriptManager1" runat="server">
</asp:scriptmanager>--%>
    <script type="text/javascript">

        function ShowModalPopup() {
            $find("mpe").show();
            $find("ctl00_ContentPlaceHolder1_pnlPopup").show();
            return false;
        }
        function HideModalPopup() {
            $find("mpe").hide();
            document.getElementById("createnew").style.display = "block";
            return false;
        }
        function editRow(obj) {
            //Unit Testing- Security_ASPX_001
            var grid = $find("lwdg_FundMasterGrid");
            var row = $(obj).parents("tr[type='row']").get(0);
            var rowid = row.cells[1].innerHTML;
            var pop_open = '1';
            document.getElementById("hdnUserID").value = rowid;
            document.getElementById("hdnpop").value = pop_open;
            //$find("mpe").show();
            return true;
        }
        function removequery() {
            //alert();
            var pop_open = '0';
            document.getElementById("hdnpop").value = pop_open;
        }

    </script>
    <asp:UpdatePanel ID="uplState" runat="server">

        <ContentTemplate>
            
            <div class="main-container" id="main-container">
                <div class="page-header">

                    <div class="floatright pull_right">
                        <asp:ImageButton ID="btnExportExcel" runat="server" CausesValidation="False" ImageAlign="Middle" ImageUrl="~/images/excel_icon.png" OnClick="btnExportExcel_Click"></asp:ImageButton>
                        &nbsp;&nbsp;                       
                        <asp:ImageButton ID="btnExportPDF" runat="server" CausesValidation="False" ImageAlign="Middle" ImageUrl="~/images/pdf_icon.png" OnClick="btnExportPDF_Click"></asp:ImageButton>
                        &nbsp;&nbsp;
                             <span class="custom-createnew" style="float: right;" id="createnew" clientidmode="Static" runat="server">
                                 <a href="#" onclick="ShowModalPopup();"></a></span>
                    </div>

                    <ig:WebDocumentExporter ID="WebPDFExporter" runat="server" EnableStylesExport="false"  ExportMode="Download" DownloadName="UserMasterPDF" DataExportMode="AllDataInDataSource"  Format="PDF" />
                    <ig:WebExcelExporter runat="server" ID="WebExcelExporter" EnableStylesExport="false"  ExportMode="Download" DownloadName="UserMasterExcel" DataExportMode="AllDataInDataSource"/>

                    <h1>
                        <i class="menu-icon fa fa-lg fa-fw fa-user"></i>
                        <i class="icon-angle-right"></i>
                        <asp:Label ID="lblCreateUser" runat="server"></asp:Label>
                    </h1>

                </div>
                <%-- Landing Page Grid begins --%>
                <div runat="server" id="pnl_UserGrid"></div>
                <%-- Landing Page Grid End --%>
                <script type="text/javascript">
                    try { ace.settings.check('main-container', 'fixed') } catch (e) { }
                </script>
                <style type="text/css">
                    

                    .Datealign {
                        text-align: center !important;
                    }
                </style>



                <div class="main-content">

                    <div class="page-content">

                        <div class="page-content-area">
                            
                            <asp:Button ID="btnShow" runat="server" Text="Show Modal Popup" Visible="false" OnClientClick="return ShowModalPopup()" />
                    <asp:LinkButton ID="lnkDummy" runat="server"></asp:LinkButton>
                   
                    <cc1:ModalPopupExtender ID="mpe_UserPopup" BehaviorID="mpe" runat="server" CancelControlID="lnkDummy"
                        PopupControlID="pnlPopup" TargetControlID="createnew" BackgroundCssClass="modal-backdrop">
                    </cc1:ModalPopupExtender>
                             
                            
                            <asp:Panel ID="pnlPopup" runat="server" CssClass="modal-dialog" Style="display: none;">
                                <asp:Panel ID="pnlcontent" runat="server" CssClass="modal-content">
                                    <asp:Panel ID="pnlheader" runat="server" CssClass="modal-header-green">
                                        <h4 class="modal-title">User</h4>
                                    </asp:Panel>


                                    <asp:Panel ID="pnlbody" runat="server" CssClass="modal-body text-center">
                                        <asp:ValidationSummary ID="valSumUser" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="vgrpSave" />
                                        <div class="form-horizontal">

                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <asp:Label ID="lblUserID" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-3">
                                      <asp:TextBox ID="txtUserID" CssClass="form-control" runat="server" TabIndex="1" MaxLength="100" ToolTip="Maximum Character 10"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1" style="display: none;">
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4"
                                                            runat="server" Enabled="True" TargetControlID="txtUserID"
                                                            ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="reqvtxtUserId" runat="server"
                                                            ControlToValidate="txtUserID" Display="Static" SetFocusOnError="True"
                                                            ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>

                                                        <asp:RequiredFieldValidator ID="reqvuserIdUNQ" runat="server"
                                                            ControlToValidate="txtUserID" Display="Static" SetFocusOnError="True"
                                                            ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>

                                                    </div>
                                                </div>


                                                <div class="form-group">
                                                    <asp:Label ID="lblusername" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="txtuserfirstname" CssClass="form-control" runat="server" TabIndex="1" MaxLength="100"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1" style="display: none;">
                                                        <cc1:FilteredTextBoxExtender ID="ccfirstname_FilteredTextBoxExtender"
                                                            runat="server" Enabled="True" TargetControlID="txtuserfirstname"
                                                            ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="reqvUsername" runat="server"
                                                            ControlToValidate="txtuserfirstname" Display="Static" SetFocusOnError="True"
                                                            ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <asp:Label ID="lblmiddlename" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="txtusersmidlename" CssClass="form-control" runat="server" TabIndex="2" MaxLength="100"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <cc1:FilteredTextBoxExtender ID="ccmiddlename_FilteredTextBoxExtender"
                                                            runat="server" Enabled="True" TargetControlID="txtusersmidlename"
                                                            ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                        </cc1:FilteredTextBoxExtender>

                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <asp:Label ID="lbllastname" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="txtuserlastname" CssClass="form-control" runat="server" TabIndex="3" MaxLength="100"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1" style="display: none;">
                                                        <cc1:FilteredTextBoxExtender ID="cclastname_FilteredTextBoxExtender"
                                                            runat="server" Enabled="True" TargetControlID="txtuserlastname"
                                                            ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="reqvtxtlastname" runat="server"
                                                            ControlToValidate="txtuserlastname" Display="Dynamic" SetFocusOnError="True"
                                                            ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <asp:Label ID="lblphone" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="txtphone" CssClass="form-control" TextMode="Phone" runat="server" TabIndex="4" MaxLength="50"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1" style="display: none;">
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                                            runat="server" Enabled="True" TargetControlID="txtphone"
                                                            ValidChars="1234567890">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="reqvphone" runat="server"
                                                            ControlToValidate="txtphone" Display="Static" SetFocusOnError="True"
                                                            ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>

                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <asp:Label ID="lblRole" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-6">
                                                        <asp:DropDownList ID="ddlRole" runat="server" TabIndex="2" CssClass="form-control chzn-select" AutoPostBack="false">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-1" style="display: none;">
                                                        <asp:RequiredFieldValidator ID="reqvRole" runat="server"
                                                            ControlToValidate="ddlRole" Display="Static" SetFocusOnError="True"
                                                            ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-6">

                                                <div class="form-group" >
                                                    <asp:Label ID="lblPassword" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-5">
                                                        <asp:TextBox ID="txtPassword" runat="server" TabIndex="5" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-3" style="display: none">
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                                                            runat="server" Enabled="True" TargetControlID="txtPassword"
                                                            ValidChars="abcdefghijklmnopqrstuvwxyz 1234567890 _@.">
                                                        </cc1:FilteredTextBoxExtender>

                                                        <asp:RequiredFieldValidator ID="reqvPassword" runat="server"
                                                            ControlToValidate="txtPassword"  SetFocusOnError="True"
                                                            ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>

                                                <div class="form-group" >
                                                    <asp:Label ID="lblConfirmPassword" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-5">
                                                        <asp:TextBox ID="txtConfrimPass" runat="server" TabIndex="6" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-3" style="display: none">
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3"
                                                            runat="server" Enabled="True" TargetControlID="txtConfrimPass"
                                                            ValidChars="abcdefghijklmnopqrstuvwxyz 1234567890 _@.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:CompareValidator ID="CmprPassword" runat="server" ControlToValidate="txtConfrimPass" ValidationGroup="vgrpSave" Operator="Equal" ControlToCompare="txtPassword"></asp:CompareValidator>
                                                        <asp:RequiredFieldValidator ID="reqvConfirmPassword" runat="server"
                                                            ControlToValidate="txtConfrimPass" Display="Static" SetFocusOnError="True"
                                                            ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="form-group" >
                                                    <asp:Label ID="lblmailID" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-5">
                                                        <asp:TextBox ID="txtmailID" runat="server" CssClass="form-control" TabIndex="7" MaxLength="50"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-3" style="display: none">
                                                        <cc1:FilteredTextBoxExtender ID="txtmailID_FilteredTextBoxExtender"
                                                            runat="server" Enabled="True" TargetControlID="txtmailID"
                                                            ValidChars="abcdefghijklmnopqrstuvwxyz 1234567890 _@.">
                                                        </cc1:FilteredTextBoxExtender>
                                                       <%-- <asp:RegularExpressionValidator ID="txtmailID_RegularExpressionValidator" runat="server" ErrorMessage="Not a valid email format" ForeColor="#FF3300" SetFocusOnError="True" ControlToValidate="txtmailID" ValidationGroup="vgrpSave" ValidationExpression="^(?('')(''.+?''@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$"></asp:RegularExpressionValidator>--%>
                                                        <asp:RequiredFieldValidator ID="reqvEmailId" runat="server"
                                                            ControlToValidate="txtmailID" Display="Static" SetFocusOnError="True"
                                                            ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="form-group" style="padding-bottom: 10px;">
                                                    <asp:Label ID="lblusertitle" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-6">
                                                        <asp:DropDownList ID="ddlUserTitle" runat="server" TabIndex="2" CssClass="form-control chzn-select" AutoPostBack="false">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-1" style="display: none;">
                                                        <asp:RequiredFieldValidator ID="reqvUserTitle" runat="server"
                                                            ControlToValidate="ddlUserTitle" Display="Static" SetFocusOnError="True"
                                                            ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="form-group" style="padding-bottom: 10px;">
                                                    <asp:Label ID="lblusertimezoneid" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-6">
                                                        <asp:DropDownList ID="ddlUserTimeZone" runat="server" TabIndex="2" CssClass="form-control chzn-select" AutoPostBack="false">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-1" style="display: none;">
                                                        <asp:RequiredFieldValidator ID="reqvTimeZoneID" runat="server"
                                                            ControlToValidate="ddlUserTimeZone" Display="Static" SetFocusOnError="True"
                                                            ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="form-group" style="padding-bottom: 10px;">
                                                    <asp:Label ID="lblLocation" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-6">
                                                        <asp:DropDownList ID="ddlUserLocation" runat="server" TabIndex="2" CssClass="form-control chzn-select" AutoPostBack="false">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-1" style="display: none;">
                                                        <asp:RequiredFieldValidator ID="reqvtxtLocation" runat="server"
                                                            ControlToValidate="ddlUserLocation" Display="Static" SetFocusOnError="True"
                                                            ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <asp:Label ID="lblInactive" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-1">
                                                        <span class="input-icon">
                                                            <asp:CheckBox ID="chkinactive" class="checkbox" runat="server" TabIndex="11" />
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>

                                    <asp:Panel ID="pnlfooter" runat="server" CssClass="modal-footer">
                                        <div class="form-group">
                                            <div class="col-md-1 floatright">
                                                <asp:Button ID="btnClear" Text="Cancel" runat="server" CssClass=" btn btn-orange" TabIndex="8" OnClick="btnClear_Click" CausesValidation="false" />
                                            </div>
                                            <div class="col-md-1 floatright">
                                                <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="btn btn-blue" TabIndex="7" OnClientClick="removequery();" ValidationGroup="vgrpSave" OnClick="btnSave_Click" />
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </asp:Panel>

                            </asp:Panel>
                           
                        </div>
                    </div>

                    <a href="#" id="btn-scroll-up" class="btn-scroll-up btn btn-sm btn-inverse">
                        <i class="ace-icon fa fa-angle-double-up icon-only bigger-110"></i>
                    </a>
                </div>
                <asp:HiddenField ID="hdnpop" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnUserID" runat="server" ClientIDMode="Static" />
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
