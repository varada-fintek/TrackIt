<%@ Page Language="C#" MasterPageFile="~/master-templates/Layout.Master" AutoEventWireup="true" CodeBehind="ClientMaster.aspx.cs" Inherits="TrackIT.WebApp.client.ClientMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="Infragistics4.WebUI.WebDataInput.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>

<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.ListControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI" TagPrefix="ig" %>

<asp:Content ID="Content2" ContentPlaceHolderID="adminhead" runat="server">
</asp:Content>

<asp:Content ID="client" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
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
                                 <a href="#" onclick="ShowModalPopup();"></a></span>
                    </div>
                    <ig:WebDocumentExporter ID="WebPDFExporter" runat="server" EnableStylesExport="false" ExportMode="Download" DownloadName="UserMasterPDF" DataExportMode="AllDataInDataSource" Format="PDF" />
                    <ig:WebExcelExporter runat="server" ID="WebExcelExporter" EnableStylesExport="false" ExportMode="Download" DownloadName="UserMasterExcel" DataExportMode="AllDataInDataSource" />

                    <h1>
                        <i class="menu-icon fa fa-lg fa-fw fa-user"></i>
                        <i class="icon-angle-right"></i>
                        <asp:Label ID="lblCreateClient" runat="server"></asp:Label>
                    </h1>
                </div>
                <div id="pnl_clientGrid" runat="server"></div>
            </div>

            <div class="main-content">
                <div class="page-content">
                    <div class="page-content-area">
                        <asp:Button ID="btnShow" runat="server" Text="Show Modal Popup" Visible="false" OnClientClick="return ShowModalPopup()" />
                        <asp:LinkButton ID="lnkDummy" runat="server"></asp:LinkButton>
                        <cc1:ModalPopupExtender ID="mpe_ClientsPopup" BehaviorID="mpe" runat="server" CancelControlID="lnkDummy"
                            PopupControlID="pnlPopup" TargetControlID="createnew" BackgroundCssClass="modal-backdrop">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="pnlPopup" runat="server" CssClass="modal-dialog" Style="display: none;">
                           
                             <asp:Panel ID="pnlcontent" runat="server" CssClass="modal-content">
                               
                                  <asp:Panel ID="pnlheader" runat="server" CssClass="modal-header-green">
                                    <h4 class="modal-title">Clients</h4>
                                </asp:Panel>

                                <asp:Panel ID="pnlbody" runat="server" CssClass="modal-body text-center">                                    
                                   <asp:ValidationSummary ID="valSumUser" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="vgrpSave" />
                                      <div class="form-horizontal">
                                          <div class="col-md-6" >
                                              <div class="form-group">
                                                   <asp:Label ID="lblclientname" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="txtclintName" CssClass="form-control" runat="server" TabIndex="1" MaxLength="20" ToolTip="Maximum Character 20"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1" style="display: none;">FOR VALIDATIONS                                                       
                                                    </div>
                                              </div>

                                               <div class="form-group">
                                                   <asp:Label ID="lblclientcode" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-3">
                                                    <asp:TextBox ID="txtclientCode" CssClass="form-control" runat="server" TabIndex="1" MaxLength="20" ToolTip="Maximum Character 20"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1" style="display: none;">FOR VALIDATIONS                                                       
                                                    </div>
                                              </div>

                                               <div class="form-group">
                                                   <asp:Label ID="lblclientaddress1" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-3">
                                                    <asp:TextBox ID="txtclientaddress1" CssClass="form-control" runat="server" TabIndex="1" MaxLength="50" ToolTip="Maximum Character 50"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1" style="display: none;">FOR VALIDATIONS                                                       
                                                    </div>
                                              </div>

                                               <div class="form-group">
                                                   <asp:Label ID="lblclientaddress2" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-3">
                                                    <asp:TextBox ID="txtclientaddress2" CssClass="form-control" runat="server" TabIndex="1" MaxLength="50" ToolTip="Maximum Character 50"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1" style="display: none;">FOR VALIDATIONS                                                       
                                                    </div>
                                              </div>
                                               <div class="form-group">
                                                   <asp:Label ID="lblclientcity" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-3">
                                                    <asp:TextBox ID="txtclientcity" CssClass="form-control" runat="server" TabIndex="1" MaxLength="20" ToolTip="Maximum Character 20"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1" style="display: none;">FOR VALIDATIONS                                                       
                                                    </div>
                                              </div>
                                               <div class="form-group">
                                                   <asp:Label ID="lblclientstate" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-3">
                                                    <asp:TextBox ID="txtclientstate" CssClass="form-control" runat="server" TabIndex="1" MaxLength="20" ToolTip="Maximum Character 20"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1" style="display: none;">FOR VALIDATIONS                                                       
                                                    </div>
                                              </div>

                                               <div class="form-group">
                                                   <asp:Label ID="lblclientzip" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-3">
                                                    <asp:TextBox ID="txtclientzip" CssClass="form-control" runat="server" TabIndex="1" MaxLength="50" ToolTip="Maximum Character 50"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1" style="display: none;">FOR VALIDATIONS                                                       
                                                    </div>
                                              </div>
                                          </div>
                                           <div class="form-group">
                                                   <asp:Label ID="lblcountry" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-3">
                                                    <asp:DropDownList ID="ddlRole" runat="server" TabIndex="2" CssClass="form-control chzn-select" AutoPostBack="false">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-1" style="display: none;">FOR VALIDATIONS                                                       
                                                    </div>
                                              </div>


                                          <div class="col-md-6" >

                                              <div class="form-group">
                                                   <asp:Label ID="Label1" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="TextBox1" CssClass="form-control" runat="server" TabIndex="1" MaxLength="100" ToolTip="Maximum Character 10"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1" style="display: none;">FOR VALIDATIONS                                                       
                                                    </div>
                                              </div>



                                          </div>

            </div>
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
                            
                            </div></div></div>



                                 <asp:Panel ID="pnlfooter" runat="server" CssClass="modal-footer">
                                        <div class="form-group">
                                            <div class="col-md-1 floatright">
                                                <asp:Button ID="btnClear" Text="Cancel" runat="server" CssClass=" btn btn-orange" TabIndex="8" CausesValidation="false" />
                                            </div>
                                            <div class="col-md-1 floatright">
                                                <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="btn btn-blue" TabIndex="7" OnClientClick="removequery();" ValidationGroup="vgrpSave" />
                                            </div>
                                        </div>
                                    </asp:Panel>
                            </asp:Panel>
                        </asp:Panel>
                    </div>
                </div>
            </div>
             <asp:HiddenField ID="hdnpop" runat="server" ClientIDMode="Static" />
           <asp:HiddenField ID="hdnUserID" runat="server" ClientIDMode="Static" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
