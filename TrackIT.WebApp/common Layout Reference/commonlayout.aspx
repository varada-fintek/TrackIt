<%@ Page Language="C#" MasterPageFile="~/master-templates/Layout.Master" AutoEventWireup="true" CodeBehind="commonlayout.aspx.cs" Inherits="TrackIT.WebApp.common_Layout.commonlayout" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc" TagName="Paging" Src="~/UserControls/PagingControl.ascx" %>
<%@ Register TagPrefix="uc" TagName="NoRecords" Src="~/UserControls/NoRecords.ascx" %>
<%--<%@ Register TagPrefix="ucProp" TagName="ucProperty" Src="~/UserControls/ucControls.ascx" %>--%>


<%@ Register Assembly="Infragistics4.WebUI.WebDataInput.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>

<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.ListControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI" TagPrefix="ig" %>




<asp:Content ID="Content2" ContentPlaceHolderID="adminhead" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Script for Popup Show /hide
        function ShowModalPopup() {
            $find("mpe").show();
            return false;
        }
        function HideModalPopup() {
            $find("mpe").hide();
            document.getElementById("createnew").style.display = "block";
            return false;
        }

        // edit value of a single Row in the grid
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
            var pop_open = '0';
            document.getElementById("hdnpop").value = pop_open;
        }

    </script>


    <asp:UpdatePanel ID="uplState" runat="server">

        <ContentTemplate>

            <div class="main-container" id="main-container">
                <%-- Landing Page title begins --%>
                <div class="page-header">
                    <div class="floatright pull_right">
                        <%-- Export Section begin--%>
                        <asp:ImageButton ID="btnExportExcel" runat="server" CausesValidation="False" ImageAlign="Middle" ImageUrl="~/images/excel_icon.png" ></asp:ImageButton>
                        &nbsp;&nbsp;                       
                        <asp:ImageButton ID="btnExportPDF" runat="server" CausesValidation="False" ImageAlign="Middle" ImageUrl="~/images/pdf_icon.png" ></asp:ImageButton>
                        &nbsp;&nbsp;
                        <%-- !popup add page button--%>
                             <span class="custom-createnew" style="float: right;" id="createnew" clientidmode="Static" runat="server">
                                 <a href="#" onclick="show();"></a></span>
                        
                    </div>

                    <ig:WebDocumentExporter ID="WebPDFExporter" runat="server" ExportMode="Download" DownloadName="UserMasterPDF" DataExportMode="AllDataInDataSource" EnableStylesExport="false" Format="PDF" />
                    <ig:WebExcelExporter runat="server" ID="WebExcelExporter" ExportMode="Download" DownloadName="UserMasterExcel" DataExportMode="AllDataInDataSource" EnableStylesExport="false" />
                     <%-- Export Section end--%>

                     
                    <h1>
                        <i class="menu-icon fa fa-lg fa-fw fa-user"></i>
                        <i class="icon-angle-right"></i>
                        <asp:Label ID="lblCreateUser" runat="server"></asp:Label>
                    </h1>

                </div>
                   <%-- Landing Page title ends --%>

                <%-- Landing Page Grid begins --%>
                <div runat="server" id="pnl_Grid">

                </div>
                <%-- Landing Page Grid End --%>


                <script type="text/javascript">
                    try { ace.settings.check('main-container', 'fixed') } catch (e) { }
                </script>
                <style type="text/css">
                    .Datealign {
                        text-align: center !important;
                    }
                </style>


                <%-- Landing Page Main contenet begins--%>

                <div class="main-content">

                    <div class="page-content">

                        <div class="page-content-area">
                            <%-- Popup begins --%>
                            <asp:Button ID="btnShow" runat="server" Text="Show Modal Popup" Visible="false" OnClientClick="return ShowModalPopup()" />
                            <asp:LinkButton ID="lnkDummy" runat="server"></asp:LinkButton>
                            <cc1:ModalPopupExtender ID="mpe_Popup" BehaviorID="mpe" runat="server" CancelControlID="lnkDummy"
                                PopupControlID="pnlPopup" TargetControlID="createnew" BackgroundCssClass="modal-backdrop">
                            </cc1:ModalPopupExtender>

                            <asp:Panel ID="pnlPopup" runat="server" CssClass="modal-dialog" Style="display: none;">

                                <asp:Panel ID="pnlcontent" runat="server" CssClass="modal-content">
                                    <%-- ! popup header --%>
                                    <asp:Panel ID="pnlheader" runat="server" CssClass="modal-header-green">
                                        <h4 class="modal-title">Title</h4>
                                    </asp:Panel>

                                    <%-- ! popup body --%>
                                    <asp:Panel ID="pnlbody" runat="server" CssClass="modal-body text-center">
                                        <asp:ValidationSummary ID="valSumUser" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="vgrpSave" />
                                        <%-- controls to place in the popup --%>

                                        <div class="form-horizontal">

                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <div class="col-md-3">
                                                       </div>
                                                    <div class="col-md-1" style="display: none;">
                                                        
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>

                                    <%-- popup footer --%>

                                    <asp:Panel ID="pnlfooter" runat="server" CssClass="modal-footer">
                                        <div class="form-group">
                                            <div class="col-md-1 floatright">
                                                <asp:Button ID="btnClear" Text="Cancel" runat="server" CssClass=" btn btn-orange" TabIndex="8" CausesValidation="false" />
                                            </div>
                                            <div class="col-md-1 floatright">
                                                <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="btn btn-blue" TabIndex="7" OnClientClick="removequery();" ValidationGroup="vgrpSave"/>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </asp:Panel>

                            </asp:Panel>

                            <%-- Popup Ends --%>
                          
                            
                       
                        </div>
                        <div class="col-sm-13" id="listform">
                            <%-- Panel Body in Landing Page begins --%>
                            <div class="panel panel-white">
                                <div class="panel-heading">
                                    <h3 class="panel-title">
                                       
                                </div>

                                <div class="panel-body">
                                    <div class="form-horizontal">
                                   </div>
                                </div>
                            </div>
                             <%-- Panel Body in Landing Page ends --%>
                        </div>
                          <%-- Landing Page Controls begins --%>
                        <div class="row">
                            <div class="col-sm-6 col-lg-3">
                            <div class="widget-box" ">
                                <div class="widget-body">
                                    <div class="widget-main">
                                    </div>
                                </div>
                            </div>
                            </div>
                            <div class="col-sm-6 col-lg-3">
                            <div class="widget-box" ">
                                <div class="widget-body">
                                    <div class="widget-main">
                                    </div>
                                </div>
                            </div>
                            </div>
                            <div class="col-sm-6 col-lg-3">
                            <div class="widget-box" ">
                                <div class="widget-body">
                                    <div class="widget-main">
                                    </div>
                                </div>
                            </div>
                            </div>
                            <div class="col-sm-6 col-lg-3">
                            <div class="widget-box" ">
                                <div class="widget-body">
                                    <div class="widget-main">
                                    </div>
                                </div>
                            </div>
                            </div>
                        </div>
                  <%-- Landing Page Controls begins --%>
                    </div>

                    <a href="#" id="btn-scroll-up" class="btn-scroll-up btn btn-sm btn-inverse">
                        <i class="ace-icon fa fa-angle-double-up icon-only bigger-110"></i>
                    </a>
                </div>

                <%-- Landing Page Main contenet begins--%>

                <%-- Hidden fields begins--%>
                <asp:HiddenField ID="hdnpop" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnID" runat="server" ClientIDMode="Static" />
                <%-- Hidden fields end--%>

        </ContentTemplate>
        <Triggers>
            <%-- !post back Controls --%>
            <asp:PostBackTrigger ControlID="btnExportExcel" />
            <asp:PostBackTrigger ControlID="btnExportPDF" />
        </Triggers>
    </asp:UpdatePanel>

    <script type="text/javascript">
        // script to set Style for Multiselect Dropdown
        $(document).ready(function () {
            $(".e1").select2();
            $(".e1").css("border", "none");
        });
    </script>
</asp:Content>