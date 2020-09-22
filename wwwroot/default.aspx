<%@ Register TagPrefix="swenet" TagName="header" Src="Controls\Header.ascx" %>
<%@ Register TagPrefix="swenet" TagName="sidebar" Src="Controls\Sidebar.ascx" %>
<%@ Page Language="C#" %>
<!doctype html public "-//w3c//dtd html 4.0 transitional//en" >
<HTML>
	<HEAD>
		<title>SWEnet</title>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<link href="style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form runat="server">
			<swenet:header id="Header" runat="server"></swenet:header>
			<table width="750" cellspacing="1" cellpadding="1">
				<tr>
					<td valign="top" width="150">
						<swenet:sidebar id="Sidebar" runat="server"></swenet:sidebar>
					</td>
					<td width="600" style="FLOAT: right; WIDTH: 600px">
						<h1>Welcome
						</h1>
						<p>
							<strong>SWENET</strong> – The Network Community for Software Engineering 
							Education – is a project to produce and organize high-quality materials 
							supporting software engineering education. As a first step in this process, a 
							framework will be developed which serves as a high-level taxonomy for the areas 
							of knowledge comprising software engineering. The framework will not be 
							developed ab initio, but rather will build on the work of the <a href="http://www.abet.org">
								ABET</a> program criteria for software engineering education and the 
							classification proposed by the <a href="http://www.swebok.org">Software Engineering 
								Body of Knowledge</a> (SWEBOK) project sponsored by the <a href="http://www.ieee.org">
								IEEE Computer Society</a>. Four areas of particular importance to 
							undergraduate software engineering – design, quality, requirements, and process 
							– have been singled out as the most appropriate starting points for developing 
							curricular materials.
						</p>
						<h2><a href="feedback.aspx" style="COLOR: orangered">Give us your feedback.</a></h2>
						<p align="center">
							<strong>A Joint Project of:</strong>
						</p>
						<div align="center">
							<table cellspacing="7" width="40%" border="0">
								<tr>
									<td width="15%">
										<p align="center">
											&nbsp;<a href="http://www.rit.edu/" target="_top"><img alt="RIT" src="images/RITLogo1.gif" border="0"></a>
										</p>
									</td>
									<td width="15%">
										<p align="center">
											&nbsp;&nbsp;&nbsp;<a href="http://www.ttu.edu/" target="_top"><img height="100" alt="Texas Tech University" src="images/ttlogo.jpg" border="0"></a>&nbsp;&nbsp;
										</p>
									</td>
									<td width="15%">
										<p align="center">
											&nbsp;<a href="http://www.gatech.edu/" target="_top"><img alt="Georgia Tech" src="images/gt.jpg" border="0"></a>
										</p>
									</td>
								</tr>
								<tr>
									<td width="16%">
										<p align="center">
											&nbsp;<a href="http://www.drexel.edu/" target="_top"><img alt="Drexel University" src="images/DrexLogo.gif" border="0"></a>
										</p>
									</td>
									<td width="19%">
										<p align="center">
											<a href="http://www.embryriddle.edu/" target="_top"><img alt="Embry-Riddle Aeronautical University" src="images/emri.gif" border="0"></a>
										</p>
									</td>
									<td width="16%">
										<p align="center">
											<a href="http://www.msoe.edu/" target="_top"><img height="100" alt="Milwaukee School of Engineering" src="images/MSOELogo.gif" border="0"></a>
										</p>
									</td>
								</tr>
							</table>
						</div>
						<br>
						<div align="center">
							<table width="30%" border="0">
								<tr>
									<td width="50%">
										<p align="center">
											Sponsored In Part by Grant EEC-0080502
										</p>
									</td>
									<td width="50%">
										<a href="http://www.nsf.gov/" target="_top"><img alt="National Science Foundation" src="images/nsf.gif" border="0"></a></td>
								</tr>
							</table>
						</div>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
