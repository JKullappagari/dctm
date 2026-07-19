// JScript File
var request;
var response;
var cOption;
var ctDropdown;
var ctTrade;
//var status = document.getElementById("lblStatus");

function UploadSiteSpecificRFIDCard(ddlSite, hdnRFID)
{
    var Site = document.getElementById(ddlSite.name);
    var RFIDCardNumber = document.getElementById(hdnRFID.name).value;
    ctl00_Master_ContentPlaceHolder_lblMessage.innerText = "";
    
    if (Site.options[Site.selectedIndex].value != "0" && RFIDCardNumber != "")
    {	  
        return SendUploadRequest(Site.options[Site.selectedIndex].value, RFIDCardNumber);		
    }		
}

function SendUploadRequest(SiteID, RFIDCardNumber)
{
    InitializeRequest();	
	var url = "AJAXServer.aspx?ST=" + SiteID + "&CN=" + RFIDCardNumber;
	request.onreadystatechange = ProcessUploadRequest;	
	request.open("GET", url, true);	
	request.send(null);
}


function ProcessUploadRequest()
{
	if (request.readyState == 4) // If the readyState is in the "Ready" state
	{
		if (request.status == 200) // If the returned status code was 200. Everything was OK.
		{
	        UpdateStatus(request.responseText); // Call the populateList fucntion
		}
	}
	return true; 
}

function UpdateStatus(response)
{
	var xmlDoc = new ActiveXObject("Microsoft.XMLDOM"); // Create the XMLDOM object
	xmlDoc.async = false;
	xmlDoc.loadXML(response); // Load the responseText into the XMLDOM document

	var opt;
	var StatusElement = xmlDoc.getElementsByTagName("UploadData"); // Create the StatusElement
	var AttributeElement = StatusElement[0].getElementsByTagName("AttributeData"); // Create the AttributeElement

	if (StatusElement.length > 0) // If there are one or more Status nodes
	{
        var Message = AttributeElement[0].getAttribute("Value");  
        var Count = AttributeElement[1].getAttribute("Value");  
        var CurrentCount = AttributeElement[2].getAttribute("Value"); 
        
        ctl00_Master_ContentPlaceHolder_lblMessage.innerText = Message;
	    ctl00_Master_ContentPlaceHolder_lblCount.innerText = Count;
	    ctl00_Master_ContentPlaceHolder_lblCurrentCount.innerText = CurrentCount;
	}
}

function LoadSite(ddlBu,ddlSite,ddlProject,ddlTrade)
{
	var Bu = document.getElementById(ddlBu);
	//ctDropdown = document.getElementById(ddlSite);
	var ddlProject = document.getElementById(ddlProject);
	
	var rfvsite = document.getElementById("ctl00_Master_ContentPlaceHolder_rfvSite");
	//window.alert('rfvsite ' + rfvsite);
	if(rfvsite !=  null)
	    rfvsite.visible = true;
	var rfvproject = document.getElementById("ctl00_Master_ContentPlaceHolder_rfvProject");
	//window.alert('rfvproject ' + rfvproject);
	if(rfvproject != null)
	    rfvproject.visible=true;
	
	clearSelect(ddlProject);
	 ddlTrade = document.getElementById(ddlTrade);
	    clearSelect(ddlTrade);
	if(Bu.options[Bu.selectedIndex].value != '0')
	//Check if the selectedItem as not "--Select--"
	{	  
	    //LoadTrade(ddlTrade,Bu.options[Bu.selectedIndex].value);
	     ctDropdown = document.getElementById(ddlSite);
	     clearSelect(ctDropdown);
		 SendRequest(Bu.options[Bu.selectedIndex].value,'site');
		//return SendRequest(Bu.options[Bu.selectedIndex].value,'trade');
	}
}
function LoadTrade(ddlProject,hdnProject,ddlTrade,ddlBu)
{
     var ddlProject = document.getElementById(ddlProject)
     var hdnProject = document.getElementById(hdnProject);
	     //alert(ddlSubcode.options[ddlSubcode.selectedIndex].text);
     hdnProject.value = ddlProject.options[ddlProject.selectedIndex].value;
     ctDropdown = document.getElementById(ddlTrade);
     clearSelect(ctDropdown);
    var Bu = document.getElementById(ddlBu);
    if(Bu.options[Bu.selectedIndex].value != '0')
    //Check if the selectedItem as not "--Select--"
    {	  
        return SendRequest(Bu.options[Bu.selectedIndex].value,'trade');		
    }
}
function LoadProject(ddlBu,ddlSite,ddlProject,hdnSiteID)
{
    var Bu = document.getElementById(ddlBu);
	var Site = document.getElementById(ddlSite);
	ctDropdown = document.getElementById(ddlProject)
	clearSelect(ctDropdown);
	//var ddlSubcode = document.getElementById(ddlSubcode);
	//clearSelect(ddlSubcode);
	if(Bu.options[Bu.selectedIndex].value != '0'  && Site.options[Site.selectedIndex].value != '0')	
	{
	     var hdnSite = document.getElementById(hdnSiteID);
	     //alert(Site.options[Site.selectedIndex].value);
	     hdnSite.value = Site.options[Site.selectedIndex].value;
		return SendRequestProject(Bu.options[Bu.selectedIndex].value,Site.options[Site.selectedIndex].value,'project');
	}	
}
function LoadProjectSubCode(ddlBu,ddlSite,ddlProject,ddlSubcode,hdnProjectID)
{
    var Bu = document.getElementById(ddlBu);
	var Site = document.getElementById(ddlSite);
	var Project = document.getElementById(ddlProject);
	ctDropdown = document.getElementById(ddlSubcode)
	clearSelect(ctDropdown)	
	if(Bu.options[Bu.selectedIndex].value != '0'  && Site.options[Site.selectedIndex].value != '0')	
	{
	    var hdnProject = document.getElementById(hdnProjectID);
	     //alert(Project.options[Project.selectedIndex].value);
	     hdnProject.value = Project.options[Project.selectedIndex].value;
		return SendRequestProjectSubCode(Bu.options[Bu.selectedIndex].value,Site.options[Site.selectedIndex].value,Project.options[Project.selectedIndex].value,'subcode');
	}	
}
function populateTerritory()
{
	var Region = document.getElementById("ddlRegion");
	ctDropdown = document.getElementById("ddTerritory")
	var ctDropdown1 = document.getElementById("ddEmployee")
	clearSelect(ctDropdown)
	clearSelect(ctDropdown1)
	if(Region.options[Region.selectedIndex].value != '-1')
	//Check if the selectedItem as not "--Select--"
	{
		return SendRequest(Region.options[Region.selectedIndex].value,'region');
	}
}

function populateEmployee()
{
   	var Territory = document.getElementById("ddTerritory");
	ctDropdown = document.getElementById("ddEmployee")
	clearSelect(ctDropdown)
	if(Territory.options[Territory.selectedIndex].value != '-1')
	//Check if the selectedItem as not "--Select--"
	{
		return SendRequest(Territory.options[Territory.selectedIndex].value,'employee');
	}
}

function GetMSXmlHttp()
{
    var xmlhttp=null;
    
    var clsids = ["Msxml2.XMLHTTP.6.0","Msxml2.XMLHTTP.5.0",
                 "Msxml2.XMLHTTP.4.0","Msxml2.XMLHTTP.3.0", 
                 "Msxml2.XMLHTTP.2.6","Microsoft.XMLHTTP.1.0", 
                 "Microsoft.XMLHTTP.1","Microsoft.XMLHTTP"];
    for(var i=0; i<clsids.length && xmlhttp == null; i++) 
    {
        xmlhttp = CreateXmlHttp(clsids[i]);
    }
    
    if (!xmlhttp && typeof XMLHttpRequest!='undefined') {
	    try {
		    xmlhttp = new XMLHttpRequest();
	    } catch (e) {
    		xmlhttp=false;
	    }
    }
    
    if (!xmlhttp && window.createRequest) {
	    try {
		    xmlhttp = window.createRequest();
	    } catch (e) {
		    xmlhttp=false;
	    }
    }   

    return xmlhttp;
}

function CreateXmlHttp(clsid) {
    var xmlHttp = null;
    try {
        xmlHttp = new ActiveXObject(clsid);
        lastclsid = clsid;
        return xmlHttp;
    }
    catch(e) {}
}

function InitializeRequest()
{
	request = GetMSXmlHttp();
}

function SendRequest(ID, option)
{
    //Set the status to "Loading....."
	//status.innerText = "Loading " + option + " .....";
	
	cOption = option;
	
	//Call InitializeRequest to set request object
	InitializeRequest();
	
	//Create the url to send the request to
	var url = "AJAXServer.aspx?option=" + option + "&ID=" + ID;
	
	//Delegate ProcessRequest to onreadystatechange property so it gets called for every change in readyState value
	request.onreadystatechange = ProcessRequest;
	
	//Open a GET request to the URL
	request.open("GET", url, true);
	
	//Send the request with a null body.
	request.send(null);
}
function SendRequestProject(BUID,SiteID, option)
{
    InitializeRequest();	
	var url = "AJAXServer.aspx?option=" + option + "&BUID=" + BUID + "&SiteID=" + SiteID;
	request.onreadystatechange = ProcessRequest;	
	request.open("GET", url, true);	
	request.send(null);
}
function SendRequestProjectSubCode(BUID,SiteID,ProjectID,option)
{
    InitializeRequest();	
	var url = "AJAXServer.aspx?option=" + option + "&BUID=" + BUID + "&SiteID=" + SiteID+ "&ProjectID=" + ProjectID;
	request.onreadystatechange = ProcessRequest;	
	request.open("GET", url, true);	
	request.send(null);
}

function ProcessRequest()
{
	if(request.readyState == 4)//If the readyState is in the "Ready" state
	{
		if(request.status == 200)//If the returned status code was 200. Everything was OK.
		{
			if(request.responseText != "")//If responseText is not blank
			{
			    //alert(request.responseText);
				populateList(request.responseText);//Call the populateList fucntion
				//status.innerText = "Territories Loaded";//Set the status to "Territories Loaded"
			}
			else
			{
				//status.innerText = "None Found";//Set the status to "None Found"
				clearSelect(ctDropdown);//Call clearSelect function
			}
		}
	}
	return true;//return
}

function populateList(response)
{
	var xmlDoc=new ActiveXObject("Microsoft.XMLDOM");//Create the XMLDOM object
	xmlDoc.async = false;
	xmlDoc.loadXML(response);//Load the responseText into the XMLDOM document

	var opt;
	var TerritoriesElem = xmlDoc.getElementsByTagName("xmlData");//Create the EmployeeTerritories element
	//alert(TerritoriesElem[0]);
	var TerritoryElem = TerritoriesElem[0].getElementsByTagName("tableData");//Create the TERRITORIES element
	

	clearSelect(ctDropdown);//Clear the dropdown before filling it with new values
	//alert(ctDropdown);
	if(TerritoriesElem.length > 0)//If there are one or more TERRITORIES nodes
	{
	    for (var i = 0; i < TerritoryElem.length; i++)//Loop through the XML TERRITORIES nodes
	    {
		    var textNode = document.createTextNode(TerritoryElem[i].getAttribute("DisplayText"));//Create a TextNode
		    appendToSelect(ctDropdown, TerritoryElem[i].getAttribute("DisplayValue"), textNode);//Call appendToSelect to append the text elements to the ctDropdown dropdown
	    }
	}
}

function appendToSelect(select, value, content)
{
	var opt;
	opt = document.createElement("option");//Create an Element of type option
	opt.value = value;//Set the option's value
	//alert(content);
	opt.appendChild(content);//Attach the text content to the option
	select.appendChild(opt);//Append the option to the referenced [ctDropdown] select box
}

function clearSelect(select)
{
	select.options.length = 1;//Set the select box's length to 1 so only "--Select--" is availale in the selection on calling this function.
							  //You may want to write your own clearSelect logic
}