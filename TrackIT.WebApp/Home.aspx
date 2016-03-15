<%@ Page Language="C#" MasterPageFile="~/master-templates/Layout.Master" UICulture="en-US" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="TrackIT.WebApp.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <style type="text/css">
       /**
 * Confidential: Copyright of Fintek solutions
 *
 */ 
@font-face{font-family:common_font;src:url(../fonts/Montserrat-Light.otf)}
body,.col-md-4{font-family:common_font}
.form-signin{width:100%;padding:15px;margin:40px auto 0;position:relative}
.profile-img{margin:0 auto;display:block}
.form-signin input{margin-bottom:-1px;border-bottom-left-radius:0;border-bottom-right-radius:0;padding-left:47px}
.form-signin .form-control{position:relative;font-size:16px;height:50px;box-sizing:border-box}
.form-signin .form-control:focus{z-index:2}
.forgot-pass{margin:15px auto 0;min-width:154px;width:150px;text-decoration:underline;cursor:pointer}
.input-addon{height:50px;width:50px;z-index:100;display:inline-block;position:absolute}
.user-name .input-addon{background:url("../img/icons/user_login.png") no-repeat center}
.password .input-addon{background:url("../img/icons/password.png") no-repeat center}
.validate{position:absolute;width:20px;right:0;top:0;margin:15px 0 0 0;height:50px}
.password .validate{top:50px}
.validate-icon{display:none;height:100%;width:100%}
.correct{background:url("../img/icons/right.png") no-repeat center}
.wrong{background:url("../img/icons/wrong.png") no-repeat center}
.sign-in{margin-top:20px;font-weight:800;background-color:#4c91dc;border:none;border-radius:2px}
.form-signin .form-control,.sign-in{width:97%}
.login-img{background:url("../img/images/login_image.jpg") no-repeat center;-webkit-background-size:100% 100%;-moz-background-size:100% 100%;-o-background-size:100% 100%;background-size:100% 100%;padding-bottom:50%;background-size:cover}
#errorMsg{text-align:center;margin-top:15px;color:#f00;font-weight:bold}
.forgot-row{margin:5px 0}
.forgot-content{margin-left:5px}
.forgot-label{width:30%;display:inline-block;vertical-align:top}
.forgot-cont{width:65%;display:inline-block}
.send-mail{padding:4px 12px;vertical-align:top}
.col-md-4{padding:9% 2%}
@media screen and (max-width: 769px){
	.col-md-8.column.clear-padding{position:absolute}
	.col-md-4{padding:40% 2% !important}	
	}
}
   </style>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
           <!DOCTYPE html>
<html>
    
<head>
<meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1">
<meta name="viewport" content="width=device-width, initial-scale=1">
<link rel="shortcut icon" type="image/png" href="../img/icons/favicon.ico"/>

<link href="../css/login.css" rel="stylesheet">

</head>
<body>
<div class="container" style="width:100%">
	<div class="row clearfix" style="display:none;">
		<div class="col-md-8 column clear-padding">
			<img src="../img/images/login_image.jpg" class="col-md-12 clear-padding" style="height:100vh;width:100%"/>
		</div>
		<div class="col-md-4 column">
			<div class="account-wall">
                <img class="profile-img" src="../img/images/avatar_login.png">
                <form class="form-signin" action="dashboard_display.html">
					<div class="user-name">
						<span class="input-addon"></span>
						<input type="text" maxlength="50" class="form-control">
						<div class="validate">
							<span class="validate-icon correct"></span>
							<span class="validate-icon wrong"></span>
						</div>
					</div>
					<div class="password">
						<span class="input-addon"></span>
						<input type="password"  maxlength="10"  class="form-control">
						<div class="validate">
							<span class="validate-icon correct"></span>
							<span class="validate-icon wrong"></span>
						</div>
					</div>
	               <button class="btn btn-lg btn-primary btn-block sign-in">LOGIN</button>
	               <div class="forgot-pass">Forgot your password?</div>
	               <div id="errorMsg"></div>
                </form>
            </div>
		</div>
	</div>	
</div>
<div style="overflow:auto;min-width:500px;min-height:150px;display:none;"" class="template_popup">
	<div id="popup_title">Forgot Password</div>
	<form>
		<div class="forgot-content">
			<div class="forgot-row">
			<div class="forgot-label">User ID</div>
			<div class="forgot-cont">
				<input type="text" maxlength="50" class="form-control">
			</div>
		</div>
		<div class="forgot-row">
			<div class="forgot-label">Email ID</div>
			<div class="forgot-cont">
				<input type="text" maxlength="50" class="form-control">
			</div>
		</div>
		</div>
		<div></div>
		
		<div style="margin:15px auto;width:225px">
			<button class="btn btn-primary send-mail" type="submit">SEND MAIL</button>
			<span class="orange-btn pop-btn">CANCEL</span>
		</div>
	</form>
</div>
<div id="trans_overlay"></div>
</body>
</html>

            </ContentTemplate>
         </asp:UpdatePanel>
</asp:Content>
