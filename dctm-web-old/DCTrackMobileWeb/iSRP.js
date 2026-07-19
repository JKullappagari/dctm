/*
"AJAXEngine and utilities"
Generic objects and functions that encapsulates xmlHttp request


*/

var ajaxMessageID = 'ajaxmsgid';

// get XMLHTTP object
function getXMLHTTP()
{
	var A = null;
	try{
		A = new ActiveXObject("Msxml2.XMLHTTP");
	}
	catch(e){
		try{
			A = new ActiveXObject("Microsoft.XMLHTTP");
		} catch(oc){
			A = null;
		}
	}
	if(!A && typeof XMLHttpRequest != "undefined") {
		A = new XMLHttpRequest();
	}
	return A;
}

// build URL
function buildURL(baseURL, paramsName, paramsValue)
{
	var url = '';
	if (typeof baseURL != 'undefined' && baseURL != null)
		url = baseURL;
		
	if (url.length>0 && url.indexOf('?')==-1)
		url += '?';
	else if (url.lastIndexOf('&') != url.length-1)
		url += '&';
		
	for(var i=0; i<paramsName.length; i++)
	{
		if (i>0 && url.lastIndexOf('&') != url.length-1)
			url += '&';
			
		var val=paramsValue[i];
		val=escape(val);
		val=val.replace(/\+/g,'%2B');
		
		url += paramsName[i] + '=' + val;
	}
	return url;
}

function AjaxEngine(sourceURL)
{
	var self = this;
	var xmlHttp;
	
	// public
	this.sourceURL = sourceURL;
	this.searching = false;

	this.DoRemoteQuery = function(paramsName, paramsValue, responseHandler, isAsync, requestMode, callbackarg) {
		if (!self.searching) {
			showBusy('Loading...',isAsync);	
			self.searching = true;
			if (xmlHttp && xmlHttp.readyState != 0)
				xmlHttp.abort()
				
			xmlHttp = getXMLHTTP();
			if (xmlHttp) {
				if (typeof requestMode == 'undefined' || requestMode == null)
					requestMode = 'GET';
				
				var url;
				var content = null;
				if (requestMode == 'GET') {
					url = buildURL(self.sourceURL, paramsName, paramsValue);
					xmlHttp.open(requestMode, url, isAsync);
				}
				else {
					url = self.sourceURL;
					xmlHttp.open(requestMode, url, isAsync);
					xmlHttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
					content = buildURL(null, paramsName, paramsValue);
				}
				
				var callbackarg = (typeof(callbackarg)=='undefined') ? null : callbackarg;

				xmlHttp.onreadystatechange = function() {
					if (xmlHttp.readyState==4) {
						hideBusy();					
						self.searching = false;
						if (xmlHttp.status==200 && xmlHttp.responseText!=null) {
							if (typeof(responseHandler)=='function')
								responseHandler(unescape(xmlHttp.responseText),callbackarg);							
						}
					}				
				};
				
				xmlHttp.send(content);
			}
		}
	}
}


function getXmlFromControls(mapper) {
	if (typeof(mapper)=='undefined' || mapper == null)
		return;
		
	var xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
	xmlRoot = xmlDoc.createElement("ROOT");
	xmlDoc.appendChild(xmlRoot);
	for(var i=0; i<mapper.length; i++) {
		controlID = mapper[i][0];
		elementName = mapper[i][1];
		controlValue = '';
		if (ctl = document.getElementById(controlID)) {
			// get control value
			var tagName = ctl.tagName;
			if (tagName == 'INPUT')
				controlValue = ctl.value;
			else if (tagName == 'SELECT')
				controlValue = ctl.options[ctl.selectedIndex].value;
			
			xmlElement = xmlDoc.createElement(elementName);
			xmlElement.text = controlValue;
			xmlRoot.appendChild(xmlElement);
		}
	}
	return xmlDoc.xml;
}

function getQueryStringFromControls(mapper) {
	if (typeof(mapper)=='undefined' || mapper == null)
		return;
	var qs = '';
	for(var i=0; i<mapper.length; i++) {
		controlID = mapper[i][0];
		elementName = mapper[i][1];
		controlValue = '';
		if (ctl = document.getElementById(controlID)) {
			var tagName = ctl.tagName;
			if (tagName == 'INPUT')
				controlValue = ctl.value;
			else if (tagName == 'SELECT')
				controlValue = ctl.options[ctl.selectedIndex].value;
			
			qs+=elementName+'='+escape(controlValue)+'&';
		}
	}
	if (qs.lastIndexOf('&')==qs.length-1)
		qs = qs.substr(0,qs.length-1);
		
	return qs;
}

function setControlsFromXml(xml, mapper) {
	if (typeof(xml)=='undefined' || xml == null ||
		typeof(mapper)=='undefined' || mapper == null)
		return;
		
	var xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
	xmlDoc.loadXML(xml);
	for(var i=0; i<mapper.length; i++) {
		var controlID = mapper[i][0];
		var elementName = mapper[i][1];
		var xpathQuery = '/ROOT/' + elementName;
		var controlValue = xmlDoc.selectSingleNode(xpathQuery).text;
		
		if (ctl = document.getElementById(controlID)) {
			tagName = ctl.tagName;
			if (tagName == 'INPUT')
				ctl.value = controlValue;
			else if (tagName == 'SELECT') {
				selectList(ctl, controlValue);
			}
		}
		
	}	
}


// unescape Html
function unescapeHtml(s) {
	//s = unescape(s);
	//s = s.replace(/%20/gi, " ");
	return s;
}
function showBusy(message) {
	oDiv = getAjaxMessageBox(message);
	oDiv.style.visibility = 'visible';
}

function hideBusy() {
	if (oDiv = document.getElementById(ajaxMessageID)) {
		oDiv.style.visibility='hidden';
	}
}

function getAjaxMessageBox(message) {
	if (oDiv = document.getElementById(ajaxMessageID)) {
		document.body.removeChild(oDiv);
	}

	var oDiv = document.createElement('div');
	oDiv.id = ajaxMessageID;
	oDiv.className = "AjaxMessage";
	oDiv.style.visibility = 'hidden';
	oDiv.innerText = message;
	document.body.appendChild(oDiv);

	x = document.body.clientWidth - oDiv.offsetWidth;
	y = document.body.scrollTop;
	oDiv.style.left = x + "px";
	oDiv.style.top = y + "px";
	return oDiv;
}


