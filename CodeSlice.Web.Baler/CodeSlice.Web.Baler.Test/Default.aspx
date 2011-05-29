<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CodeSlice.Web.Test.Default" %>
<%@ Import Namespace="CodeSlice.Web.Baler" %>
<%@ Import Namespace="CodeSlice.Web.Baler.Extensions.CoffeeScript" %>
<%@ Import Namespace="CodeSlice.Web.Baler.Extensions.AjaxMinifier" %>

<!DOCTYPE HTML />
<html>
<head>
  <title>Jasmine Test Runner</title>
  <link rel="stylesheet" type="text/css" href="lib/jasmine-1.0.2/jasmine.css"/>
  <script type="text/javascript" src="lib/jasmine-1.0.2/jasmine.js"></script>
  <script type="text/javascript" src="lib/jasmine-1.0.2/jasmine-html.js"></script>

  <!-- include source files here... -->
  <%=Baler.Build("~/src/single.js").AsJs()%>
  <%=Baler.Build("~/src/single.css").AsCss()%>
  <%=Baler.Build("~/src/single.coffee").AsCoffeeScript()%>

  <!-- include spec files here... -->
  <script type="text/javascript" src="spec/baler.spec.js"></script>

</head>
<body>

<script type="text/javascript">
  jasmine.getEnv().addReporter(new jasmine.TrivialReporter());
  jasmine.getEnv().execute();
</script>

<div id="single">Test</div>
</body>
</html>

