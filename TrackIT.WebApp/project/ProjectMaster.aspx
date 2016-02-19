<%@ Page Language="C#" MasterPageFile="~/master-templates/Layout.Master" AutoEventWireup="true" CodeBehind="ProjectMaster.aspx.cs" Inherits="TrackIT.WebApp.project.ProjectMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="Infragistics4.WebUI.WebDataInput.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>

<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.ListControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI" TagPrefix="ig" %>

<asp:Content ID="Content2" ContentPlaceHolderID="adminhead" runat="server">
</asp:Content>
<asp:Content ID="project" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
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
    </script>
    <asp:UpdatePanel ID="uplState" runat="server">
        <ContentTemplate>
            <div class="main-container" id="main-container">
                <div class="page-header">

                    <div class="floatright pull_right">
                        <asp:ImageButton ID="btnExportExcel" runat="server" CausesValidation="False" ImageAlign="Middle" ImageUrl="~/images/excel_icon.png"></asp:ImageButton>
                        &nbsp;&nbsp;                       
                        <asp:ImageButton ID="btnExportPDF" runat="server" CausesValidation="False" ImageAlign="Middle" ImageUrl="~/images/pdf_icon.png"></asp:ImageButton>
                        &nbsp;&nbsp;
                             <span class="custom-createnew" style="float: right;" id="createnew" clientidmode="Static" runat="server">
                                 <a href="#" onclick="show(); ShowModalPopup();"></a></span>
                    </div>
                    <ig:WebDocumentExporter ID="WebPDFExporter" runat="server" EnableStylesExport="false" ExportMode="Download" DownloadName="UserMasterPDF" DataExportMode="AllDataInDataSource" Format="PDF" />
                    <ig:WebExcelExporter runat="server" ID="WebExcelExporter" EnableStylesExport="false" ExportMode="Download" DownloadName="UserMasterExcel" DataExportMode="AllDataInDataSource" />

                    <h1>
                        <i class="menu-icon fa fa-lg fa-fw fa-file"></i>
                        <i class="icon-angle-right"></i>
                        <asp:Label ID="lblCreateProjects" runat="server"></asp:Label>
                    </h1>
                </div>
                <div runat="server" id="pnl_projectGrid">
                </div>
            </div>
            <div class="main-content">

                <div class="page-content">

                    <div class="page-content-area">
                        <asp:Button ID="btnShow" runat="server" Text="Show Modal Popup" Visible="false" OnClientClick="return ShowModalPopup()" />
                        <asp:LinkButton ID="lnkDummy" runat="server"></asp:LinkButton>
                        <cc1:ModalPopupExtender ID="mpe_projectPopup" BehaviorID="mpe" runat="server" CancelControlID="lnkDummy"
                            PopupControlID="pnlPopup" TargetControlID="createnew" BackgroundCssClass="modal-backdrop">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="pnlPopup" runat="server" CssClass="modal-dialog" Style="display: none;">
                            <asp:Panel ID="pnlcontent" runat="server" CssClass="modal-content">
                                <asp:Panel ID="pnlheader" runat="server" CssClass="modal-header-green">
                                    <h4 class="modal-title">project</h4>
                                </asp:Panel>
                                <asp:Panel ID="pnlbody" runat="server" CssClass="modal-body text-center">
                                 <asp:ValidationSummary ID="ValProjects" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="vgrpSave" />
                                    <div class="form-horizontal">
                                        
                                        <div class="form-group">
                                            <asp:Label ID="lblclientname" class="control-label col-md-2" runat="server"></asp:Label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="ddlClients" runat="server" TabIndex="1" CssClass="form-control chzn-select" AutoPostBack="false">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-1" style="display: none;">
                                                <asp:RequiredFieldValidator ID="reqvClient" runat="server"
                                                    ControlToValidate="ddlClients" Display="Static" SetFocusOnError="True"
                                                    ValidationGroup="vgrpSave" InitialValue="" ></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlfooter" CssClass="modal-footer" runat="server">

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
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
