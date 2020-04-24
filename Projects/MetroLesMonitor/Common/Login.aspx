<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MetroLesMonitor.Common.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head><meta charset="utf-8" /><title>
LesConnect Exchange
</title><meta http-equiv="X-UA-Compatible" content="IE=edge"/><meta content="width=device-width, initial-scale=1" name="viewport" /><meta name="author" />
<link href="../assets/css/login.min.css" rel="stylesheet" />    
<link rel="shortcut icon" href="../assets/images/favicon.ico" />  
</head>

<body translate="no" class="login" style="overflow:hidden;">
<div class="dvlogin">
<div id="logcontent" style="position: relative; width:360px;margin-left:35%;">
<div class="logo fadeInDown animated"><a href="javascripit:void(0)"> <img src="../assets/images/LeS_Logo.png" class="logo-width"/></a>
<h3 class="form-title fadeIn animated">LeSConnect Exchange</h3>
</div>
<div class="alert alert-danger" id="validate">
<button class="close" data-close="alert" style="margin-top:5px;"></button>
<span> Enter valid username and password. </span>
</div>
<div class="form-group txtcontent">
<input class="form-control fadeIn animated" autocomplete="on" placeholder="Username" id="input-Username" />
<label class="label_place display-hide" id="lblUsername">Username</label>
</div>
<div class="form-group txtcontent">
<input type="password" class="form-control fadeIn animated" autocomplete="off" placeholder="Password" id="input-Password" />
<label class="label_place display-hide" id="lblPassword">Password</label>
</div>
<div class="form-actions">
<button type="submit" class="btnlogin fadeIn animated" id="btnLogin">Login</button>
</div>
</div>
</div>
<script src="../assets/Scripts/min/jquery.min.js"></script>
    <script src="../assets/Scripts/min/bootstrap.min.js"></script>
    <script src="../assets/Scripts/min/toastr.min.js" type="text/javascript" async></script> 
    <script src="../assets/Scripts/min/metronic.min.js"></script>
    <script src="../assets/Scripts/smCommon.js" type="text/javascript"></script> 
    <script src="../assets/Scripts/ob/login.js"></script>
<script>jQuery(document).ready(function(){Metronic.init();Login.init();});</script>
</body>
</html>
