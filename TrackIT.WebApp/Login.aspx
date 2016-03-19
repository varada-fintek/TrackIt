<%@ Page Language="C#" MasterPageFile="~/master-templates/LoginMaster.Master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="TrackIT.WebApp.Login" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="up" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="Loginvalid" runat="server" ShowMessageBox="True" ShowSummary="False" DisplayMode="List" />

            <!--begin logo-->
            <div class="login_logo">
                <img src="../images/logo_fin.png" alt="Fintek Solutions">
            </div>
            <!--end logo-->

            <!--begin container-->
            <div class="login_container">
                <div class="login_top_header">
                    <span class="login_top_header">LOGIN NOW</span>
                </div>
                <div class="login_body">
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="login_text_box" placeholder="User Name" autocomplete="off" />
                    <cc1:FilteredTextBoxExtender ID="txtUsername_FilteredTextBoxExtender"
                                    runat="server" Enabled="True" TargetControlID="txtUsername"
                                    ValidChars="abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890@._/-">
                    </cc1:FilteredTextBoxExtender>

                    <asp:TextBox ID="txtPassword" runat="server" CssClass="login_text_box" placeholder="Password" TextMode="Password" />

                    <a href="#" class="forgot_password" data-toggle="modal" data-target="#reassign">Forgot Username/Password</a>
                                        
                    <asp:Button ID="btnlogin" CssClass="login_btn" Text="LOGIN" runat="server" OnClick="btnuserlogin_Click" />

                    <%--<asp:LinkButton ID="btnuserlogin" AlternateText="Login" CssClass="login_btn" runat="server" OnClick="btnLogin_Click">Login</asp:LinkButton>--%>
                    
                    <div style="clear:both; height:40px;"></div>
                </div>
            </div>
            <!--end container-->
            
            <!--begin footer-->
            <div class="login_footer">
                &copy 2016, copyrights <a href="http://stradegi.com/" target="_blank">stradegi</a>
            </div>
            <!--end footer-->           

            <div  style="margin-top:20px;">
                
                <asp:Panel ID="loginpanel" runat="server">
                            <div class="login-input-area">
                           

                                <div class="modal fade" id="reassign" role="dialog">
                                    <div class="modal-dialogsmall">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <h4>
                                                    <asp:Label class="modal-title" ID="lblForgetPasswordCaption" runat="server"></asp:Label></h4>
                                            </div>
                                            <div class="modal-body-small text-center">
                                                <asp:ValidationSummary ID="varForgetPasswordSummary" runat="server" ShowMessageBox="false" ShowSummary="true" DisplayMode="List" ValidationGroup="vgrpSavePWD" />

                                                <div class="form-group">
                                                    <br />
                                                    <br />
                                                    <asp:Label ID="lblUserNameMail" class="control-label col-md-2" CssClass="form-control" runat="server"></asp:Label>
                                                    <br />
                                                    <br />
                                                    <div class="col-md-5">
                                                        <asp:TextBox ID="txtForgetUserName" CssClass="form-control" runat="server" TabIndex="10"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <asp:RequiredFieldValidator ID="rfvUserName" runat="server" CssClass="validator_msg"
                                                            ControlToValidate="txtForgetUserName" Display="Static" SetFocusOnError="True" Text="*"
                                                            ValidationGroup="vgrpSavePWD" InitialValue=""></asp:RequiredFieldValidator>
                                                        <asp:RequiredFieldValidator ID="reqInvalidUserName" runat="server" CssClass="validator_msg"
                                                            ControlToValidate="txtForgetUserName" Display="Static" SetFocusOnError="True" Text="Invalid User Name"
                                                            ValidationGroup="vgrpSavePWD" InitialValue="" Visible="false"></asp:RequiredFieldValidator>
                                                        <cc1:FilteredTextBoxExtender ID="txtForgetUserName_FilteredTextBoxExtender"
                                                            runat="server" Enabled="True" TargetControlID="txtForgetUserName"
                                                            ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                        </cc1:FilteredTextBoxExtender>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="modal-footer">
                                                <button type="button" class="btn btn-success" data-dismiss="modal">Close</button>
                                                <asp:Button ID="btnSavePW" Text="Send" runat="server" class="btn btn-success" TabIndex="28" ValidationGroup="vgrpSavePWD" OnClick="btnSavePW_Click" />
                                                <asp:Button ID="btnCancelPW" Visible="false" Text="Cancel" runat="server" class="btn btn-success" TabIndex="7" OnClick="btnCancelPW_Click" />

                                            </div>
                                        </div>
                                    </div>
                                </div>


                            </div>
                            </asp:Panel>

                <script type="text/javascript">
                    function sentmail() {
                        alert("Auto Generation Password Sent to Your Mail....Use the password while login again");
                        location.reload();
                    }
                    function styleload() {
                        alert("Invalid User Name / Email");
                    }
                    function EnterError() {
                        alert("Please Enter valid User Name / Email");
                    }


                </script>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

