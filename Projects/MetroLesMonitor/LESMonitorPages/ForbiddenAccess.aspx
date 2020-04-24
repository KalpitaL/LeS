<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForbiddenAccess.aspx.cs" Inherits="MetroLesMonitor.LESMonitorPages.ForbiddenAccess" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
      <link href="../assets/css/bootstrap.min.css" rel="stylesheet" />    
      <link href="../assets/css/commonstyle.css" rel="stylesheet" />
      <script src="../assets/Scipts/min/jquery.min.js" type="text/javascript"></script>    
      <script src="../assets/Scripts/min/bootstrap.min.js"></script> 
</head>
<body>
    <form id="form1" runat="server">
    <div class="page-container1">
      <div class="page-content container" style="min-height: 558px;background-color:#fff!important;">
          <div class="page-head">
            <div class="row">
                  <div class="col-md-12 col-sm-12">
                      <div style="text-align:center;margin-top:30px;">
                       <a href="javascripit:void(0)"> <img src="../assets/images/deny.png" width="100" height="100" /></a>
                      </div>
                       <h3 style="text-align:center;color:red">  403 - Forbidden: Access is denied.</h3>
                      <hr />
                      <p class="alert alert-danger align-Center"> 
                          You do not have permission to view this page using the credentials that you supplied.
                      </p>
                      </div>
                </div>
            </div>
        </div>
   
    </div>
    </form>
</body>
</html>
