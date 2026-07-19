/*
    Infragistics AddAppointmentDialog Script
    Version 5.3.20053.1096 
    Copyright (c) 2005 - 2006 Infragistics, Inc. All Rights Reserved.
*/			

var  startDateId = "UltraWebTab1:_ctl0:_ctl0:wdcStartTime";
var endDateId =  "UltraWebTab1:_ctl0:_ctl0:wdcEndTime";

if(igdrp_getComboById(startDateId) == null)
{
	startDateId = "UltraWebTab1$xctl0$ctl00$wdcStartTime";
	endDateId = "UltraWebTab1$xctl0$ctl00$wdcEndTime";
}

if(igdrp_getComboById(startDateId) == null)
{
	startDateId = "UltraWebTab1xxctl0xctl00xwdcStartTime";
	endDateId = "UltraWebTab1xxctl0xctl00xwdcEndTime";
}
	/* Stops the WebDateChooser's width from expanding to 100% in Firefox 1.5 */
	if(navigator.userAgent.toLowerCase().indexOf("mozilla")>=0)
	{
		if(navigator.userAgent.toLowerCase().indexOf("firefox")>=0)
		{
			if(navigator.userAgent.indexOf("1.5") >= 0)
			{
				var startDate = igdrp_getComboById(startDateId); 
				var parentstartDateElem = document.getElementById(startDate.Element.id+"_input").parentNode;
				parentstartDateElem.style.width="0px";
				parentstartDateElem = parentstartDateElem.parentNode.parentNode.parentNode;
				parentstartDateElem.style.width="0px";
				var endDate = igdrp_getComboById(endDateId);
				var parentEndDateElem = document.getElementById(endDate.Element.id+"_input").parentNode;
				parentEndDateElem.style.width="0px";
				parentEndDateElem = parentEndDateElem.parentNode.parentNode.parentNode;
				parentEndDateElem.style.width="0px";
			}
		}
	}

var startTimeDropDown = null;
var endTimeDropDown = null;
try
{
	startTimeDropDown = oUltraWebTab1__ctl0__ctl0_ddStartTime;
	endTimeDropDown = oUltraWebTab1__ctl0__ctl0_ddEndTime;
}
catch(e)
{
	startTimeDropDown = oUltraWebTab1__ctl0_ctl00_ddStartTime;
	endTimeDropDown = oUltraWebTab1__ctl0_ctl00_ddEndTime;
}
startTimeDropDown._input.onblur = startTime_onBlur;
startTimeDropDown._input.onfocus = startTime_onFocus;
startTimeDropDown._elem.dropDownEvent = "startTime_DropDown";
startTimeDropDown._elem.itemSelect = "startTime_itemSelect";

endTimeDropDown._input.onblur = endTime_onBlur;
endTimeDropDown._elem.dropDownEvent = "endTime_DropDown";
endTimeDropDown._elem.itemSelect = "endTime_itemSelect";

/* For now hide the recurrence button */
	var recurrenceButton = document.getElementById("UltraWebToolbar2_Item_3");
	if(recurrenceButton != null)
	{
		var node = recurrenceButton;
		while(node.tagName != "TABLE")
			node = node.parentNode;
		for(var i = 0; i < node.childNodes.length; i++)
		{
			if(node.childNodes[i].tagName == "COLGROUP")
			{
				
				if(!ig_shared.IsIE)
				{
					recurrenceButton.style.display = "none";
					recurrenceButton.style.visibility = "hidden";
				}
				else
				{
					node.childNodes[i].childNodes[6].style.display = "none";
					node.childNodes[i].childNodes[6].style.visibility = "hidden";
				}
				break;								   
			}				
		}
	}

/*Removes the spacing between each toolbar item, which closes the gaps between the borders*/
var toolbar = document.getElementById("UltraWebToolbar2");
if(toolbar != null)
{
	for(var i = 0 ; i < toolbar.childNodes.length; i++)
	{
		if(toolbar.childNodes[i].tagName == "TABLE")
		{
			toolbar.childNodes[i].cellSpacing = 0;
			break;
		}
	}
}
	
/* Removes the border around the Tab's content area */
var apptTabContentArea = document.getElementById("UltraWebTab1_cp");
if(apptTabContentArea != null)
	apptTabContentArea.style.border = "none";
	
/* Stops the Tab's Width from Resizing */
var appTabHeader = document.getElementById("UltraWebTab1td0");
if(appTabHeader != null)
{
	var lastTd = null;
	while(appTabHeader.nextSibling != null)
	{
		appTabHeader = appTabHeader.nextSibling;
		if(appTabHeader.tagName == "TD")
			lastTd = appTabHeader;
	}
	if(lastTd != null)
		lastTd.style.width = "100%";
}

if(this.opener != null) 
{
	var appointmentFieldValues = this.opener.document.fieldValues;
	var appointmentDialog = this.opener.document.__webDialog;
	var scheduleInfo = appointmentDialog._scheduleInfo;
	this.opener.document.__webDialog = null;
	
	var val = appointmentFieldValues.getValue("TimeDisplayFormat");
	if(val == 0)
		MilitaryTime = false;
	else if(val == 1)
		MilitaryTime = true;
		
	var startDate = igdrp_getComboById(startDateId);//UltraWebTab1$xctl0$ctl00$wdcStartTime");
	var date = appointmentFieldValues.getValue("StartDate");
	if(date != null) 
		startDate.setValue(date);
	
	var endDate = igdrp_getComboById(endDateId);
	date = appointmentFieldValues.getValue("EndDate");
	if(date != null) 
		endDate.setValue(date);
	
	val = appointmentFieldValues.getValue("StartTime");
	if(val != null)
		startTimeDropDown.setValue(convertDateTimeToString(val));
	
	val = appointmentFieldValues.getValue("EndTime");
	if(val  != null)
		endTimeDropDown.setValue(convertDateTimeToString(val));
	
	var subject = document.getElementById("tbSubject");
	val = appointmentFieldValues.getValue("Subject");
	if(val && val.length > 0)
		subject.value = val;
	subject.focus();
	
	var loc = document.getElementById("tbLocation");
	val = appointmentFieldValues.getValue("Location");
	if(val && val.length > 0)
		loc.value = val;
	
	var description = document.getElementById("txtMsgBody");
	val = appointmentFieldValues.getValue("Description");
	if(val && val.length > 0)
		description.value = val;
	
	var alldayEvent = document.getElementById("cbAllDayEvent");
	val = appointmentFieldValues.getValue("AllDayEvent");
	alldayEvent.checked = val;
	cbAllDayEvent_Clicked();
		
	val = appointmentFieldValues.getValue("AllowAllDayEvents");
	if(!val)
	{
		alldayEvent.checked = false;
		alldayEvent.style.display = "none"; 
		document.getElementById("AllDayEventLabel").style.display = "none";
		cbAllDayEvent_Clicked();
	}
		
	var enableReminder = document.getElementById("cbReminder");
	val = appointmentFieldValues.getValue("EnableReminder");
	enableReminder.checked = val;
	cbReminder_Clicked();

	var displayInterval = document.getElementById("ddReminder");	
	val = appointmentFieldValues.getValue("ReminderInterval");
	var interval = convertTicksToString(val);
	var index = -1;
	for(var i = 0; i < displayInterval.options.length && index == -1; i++){
		if(interval == displayInterval.options[i].innerHTML)
			index = displayInterval.options[i].index;
	}
	if(index != -1)
		displayInterval.selectedIndex = index;
	else
	{
		var option = document.createElement("OPTION");
		displayInterval.appendChild(option);
		option.innerHTML = interval;
		option.value = "Test";
		displayInterval.value = "Test";
		window.setTimeout("document.getElementById('ddReminder').selectedIndex = document.getElementById('ddReminder').options.length -1;",1);
	}
	var showtimeas = document.getElementById("ddShowTimeAs");
	val = appointmentFieldValues.getValue("ShowTimeAs");
	if(val != null)
		showtimeas.selectedIndex = val;
		
	var importance = igtbar_getToolbarById("UltraWebToolbar2");
	val = appointmentFieldValues.getValue("Importance");
	if(val != null)
	{
		if(val == "0")
			importance.Items[4].Items[1].setSelected(true);
		else if(val == "2")
			importance.Items[4].Items[0].setSelected(true);
	}
	
	var dataKey = appointmentFieldValues.getValue("DataKey");
	if(dataKey == null)
	{	
		var deleteButton = document.getElementById("UltraWebToolbar2_Item_2");
		var node = deleteButton;
		while(node.tagName != "TABLE")
			node = node.parentNode;
		for(var i = 0; i < node.childNodes.length; i++)
		{
			if(node.childNodes[i].tagName == "COLGROUP")
			{
				if(!ig_shared.IsIE)
				{
					deleteButton.style.display = "none";
					deleteButton.style.visibility = "hidden";
				}
				else
				{
					node.childNodes[i].childNodes[4].style.display = "none";
					node.childNodes[i].childNodes[4].style.visibility = "hidden";
				}
				break;								   
			}				
		}
	}
	
	
	
	var calDifference =  igdrp_getComboById(startDateId).getValue().getTime() - igdrp_getComboById(endDateId).getValue().getTime();
}


function OKClicked(oToolbar, oButton, oEvent) 
{
	if(oButton.Key != "High" && oButton.Key != "Low")
	{
		if(oButton.Index == 0 || oButton.Index == 2)	// Save and Close and Delete
		{
			var alldayEvent = document.getElementById("cbAllDayEvent");
			if(alldayEvent.checked)
			{
				var time = new Date();
				time.setHours(0,0,0,0);
				startTimeDropDown.setValue(convertDateTimeToString(time));
				time.setHours(23,59,0,0);
				endTimeDropDown.setValue(convertDateTimeToString(time));
			}
			
			var startDateObj = igdrp_getComboById(startDateId);
			var startDateValue = startDateObj.getValue();
			var startTime = convertStringToDateTime(startTimeDropDown.getValue());
			
			var endDateObj = igdrp_getComboById(endDateId);
			var endDateValue = endDateObj.getValue();
			var endTime = convertStringToDateTime(endTimeDropDown.getValue());
			
			var startDateTime = new Date();
			startDateTime.setTime(startDateValue.getTime());
			startDateTime.setHours(startTime.getHours(), startTime.getMinutes());
			
			
			var endDateTime = new Date();
			endDateTime.setTime(endDateValue.getTime());
			endDateTime.setHours(endTime.getHours(), endTime.getMinutes());
			
			var duration = (endDateTime.getTime() - startDateTime.getTime())/60000;
			var intduration = parseInt(duration.toString());
			if((duration - intduration) > 0)
				duration += 1;
			
			var subject = document.getElementById("tbSubject");
			var loc = document.getElementById("tbLocation");
			var description = document.getElementById("txtMsgBody");			
			var enableReminder = document.getElementById("cbReminder");
			
			var displayInterval = document.getElementById("ddReminder");	
			var interval = convertStringToTicks(displayInterval.options[displayInterval.selectedIndex].innerHTML.split(" "));
			
			var showtimeas = document.getElementById("ddShowTimeAs");									
			
			var importance = igtbar_getToolbarById("UltraWebToolbar2");
			var hightItem = null;
			var lowItem = null;
			
			try
			{
				highItem = importance.Items[5].Items[0];
				lowItem = importance.Items[5].Items[1];
			}
			catch(e)
			{
				highItem = importance.Items[4].Items[0];
				lowItem = importance.Items[4].Items[1];
			}
			var importanceValue = "1";
			if(highItem.getSelected())
				importanceValue = "2";
			else if(lowItem.getSelected())
				importanceValue = "0";
			
		var activityEditProps = {StartDate:		 {	Element: startDateObj.Element, 
													Object : startDateObj,
													Value  : startDateValue},
									StartTime:		 {	Element: startTimeDropDown._elem,
													Value  : startTime},
									EndDate:		 {	Element: endDateObj.Element, 
													Object : endDateObj,
													Value  : endDateValue},
									EndTime:		 {	Element: endTimeDropDown._elem,
													Value  : endTime},
									AllDayEvent:	 {	Element: alldayEvent, 
													Value  : alldayEvent.checked},
									Subject:		 {	Element: subject, 
													Value  : subject.value},
									Location:		 {	Element: loc, 
													Value  : loc.value}, 
									Description:	 {	Element: description, 
													Value  : description.value},
									EnableReminder: {	Element: enableReminder, 
													Value  : enableReminder.checked},
									DisplayInterval:{	Element: displayInterval, 
													Value  : interval}, 
									ShowTimeAs:	 {	Element: showtimeas, 
													Value  : showtimeas.options[showtimeas.selectedIndex].innerHTML},
									Importance:	 {	ToolBar: importance, 
													HighItem: highItem,
													LowItem: lowItem, 
													Value  : importanceValue},
									Duration:		 {  Value  : duration},
									document:		 document,
									window:		 window
								}	
								
			if(!scheduleInfo._onActivityDialogEdit(activityEditProps))		
			{
				if(oButton.Index == 0 && dataKey != null)
					appointmentFieldValues["Operation"] = "Update";
				else if(oButton.Index == 0 && dataKey == null)
					appointmentFieldValues["Operation"] = "Add";
				else if(oButton.Index == 2)	
					appointmentFieldValues["Operation"] = "Delete";
				
				appointmentFieldValues["AllDayEvent"] = alldayEvent.checked;
				appointmentFieldValues["StartDateTime"] = startDateTime;	
				appointmentFieldValues["Duration"] = duration;					
				appointmentFieldValues["Subject"] = subject.value;
				appointmentFieldValues["Location"] = loc.value;
				appointmentFieldValues["Description"] = description.value;
				appointmentFieldValues["EnableReminder"] = enableReminder.checked;
				appointmentFieldValues["ReminderInterval"] = interval;
				appointmentFieldValues["ShowTimeAs"] =  showtimeas.selectedIndex;
				appointmentFieldValues["Importance"] = importanceValue; 
				
				appointmentDialog._dialogClosed(true);
				appointmentDialog.closeDialog();
			}
		}
		else if(oButton.Index == 1) // Print
		{
			var ActiveResourceName = scheduleInfo.getActiveResourceName();

			var frame = document.getElementById("printFrame");
			frame.style.position = 'absolute';
			frame.style.zIndex = -1;
			frame.style.height = 1;
			frame.style.width = 1;
			frame.style.visibility = "visible";

			var frameDocument = frame.contentWindow.document;
			
			frameDocument.getElementById("ResourceLabel").innerHTML = ActiveResourceName;
			frameDocument.getElementById("SubjectLabel").innerHTML =  document.getElementById("SubjectLabel").innerHTML;
			frameDocument.getElementById("tbSubject").innerHTML = document.getElementById("tbSubject").value;
			frameDocument.getElementById("LocationLabel").innerHTML =  document.getElementById("LocationLabel").innerHTML;
			frameDocument.getElementById("tbLocation").innerHTML = document.getElementById("tbLocation").value;
			frameDocument.getElementById("StartTimeLabel").innerHTML = document.getElementById("StartTimeLabel").innerHTML;
			frameDocument.getElementById("EndTimeLabel").innerHTML = document.getElementById("EndTimeLabel").innerHTML;
			frameDocument.getElementById("cbAllDayEvent").checked = document.getElementById("cbAllDayEvent").checked;
			if(frameDocument.getElementById("cbAllDayEvent").checked)
			{
				frameDocument.getElementById("ddStartTime").style.display = "none";
				frameDocument.getElementById("ddEndTime").style.display = "none";
			}
			else
			{
				frameDocument.getElementById("ddStartTime").innerHTML = startTimeDropDown.getValue();
				frameDocument.getElementById("ddEndTime").innerHTML = endTimeDropDown.getValue();
			}
			frameDocument.getElementById("AllDayEventLabel").innerHTML = document.getElementById("AllDayEventLabel").innerHTML;
			frameDocument.getElementById("cbReminder").checked = document.getElementById("cbReminder").checked;
			frameDocument.getElementById("cbReminderLabel").innerHTML = document.getElementById("ReminderLabel").innerHTML;
			var ddShowTimeAs = document.getElementById("ddShowTimeAs");
			frameDocument.getElementById("ddShowTimeAs").innerHTML = ddShowTimeAs.options[ddShowTimeAs.selectedIndex].innerHTML;
			var ddReminder = document.getElementById("ddReminder");
			frameDocument.getElementById("ddReminder").innerHTML = ddReminder.options[ddReminder.selectedIndex].innerHTML;
			frameDocument.getElementById("txtMsgBody").value = document.getElementById("txtMsgBody").value;
			frameDocument.getElementById("wdcStartTime").innerHTML = igdrp_getComboById(startDateId).getText();
			frameDocument.getElementById("wdcEndTime").innerHTML = igdrp_getComboById(endDateId).getText();
		
			frame.contentWindow.document.parentWindow.focus();
			frame.contentWindow.document.parentWindow.print();
			
			frame.style.visibility = "hidden";
		}
				
	}
	if(oButton.Index == 3) // Recurrence
	{
		var recurrenceObj = new Object();
		var startDateObj = igdrp_getComboById(startDateId);
		var endDateObj = igdrp_getComboById(endDateId);
		
		recurrenceObj.startTime = startTimeDropDown.getValue();
		recurrenceObj.endTime = endTimeDropDown.getValue();
		recurrenceObj.startDate = startDateObj.getValue();
		recurrenceObj.endDate = endDateObj.getValue();
		recurrenceObj.appointmentDialog = document;
		
		var recurrenceDialog = null;
		if(ig_shared.IsIE) // IE is Modal
		{
			recurrenceDialog = showModalDialog("Recurrence.aspx", recurrenceObj, "dialogHeight:'390px'; dialogWidth: 500px; edge: Sunken; center: Yes; help: No; resizable: Yes; status: No;");
			updateRecurrence();
		}
		else// Firefox is Modeless
		{
			recurrenceDialog = window.open("Recurrence.aspx", null, 'modal=yes,resizable=yes,scrollbars=auto,dependent=yes,toolbar=no,status=no,location=no,menubar=no,height=390px, width=500px'); 
			recurrenceDialog.onunload = recurrenceDialogClosing;
			var modalDiv = document.createElement("div");
			modalDiv.style.position = "absolute";
			modalDiv.style.height = "100%";
			modalDiv.style.width = "100%";
			modalDiv.style.background = "lime"
			modalDiv.id = "modalDiv";
			modalDiv.zIndex = 10000;
			modalDiv.innerHTML = "&nbsp;"
			document.body.appendChild(modalDiv);
			modalDiv.focus = true;
		}
	}
}
document.recurrenceApplied = false;

function updateRecurrence()
{
	if(document.recurrenceApplied)
	{
		var recurrenceTable = document.getElementById("recurrenceTable");
		var durationTable = document.getElementById("durationTable");
		durationTable.style.display = "none";
		recurrenceTable.style.display = "";
	}	
}
function recurrenceDialogClosing(evnt)
{
	/* unload fires twice, the first time it fires the url is "about:blank" ignore that one. */
	if(evnt.target.URL != "about:blank") 
	{
		updateRecurrence();
	}
}

function window_onunload() 
{
	if(this.opener != null)
		appointmentDialog.closeDialog();		
}

function convertTicksToString(ticks){
	var seconds = ticks / 10000000;
	var minutes = seconds / 60;
	var hours = minutes / 60; 
	var days = hours / 24;
	var weeks = days / 7;
	var returnString; 
	
	if(weeks == 1)
		returnString = "1 week";
	else if(weeks > 1)
		returnString = weeks + " weeks";
	else if(days == 1)
		returnString = "1 day";
	else if (days > 1)
		returnString = days + " days";		
	else if(hours == 1)
		returnString = "1 hour";
	else if (hours > 1)
		returnString = hours + " hours";
	else if(minutes == 1)
		returnString = "1 minute";
	else if (minutes > 1 || minutes == 0)
		returnString = minutes + " minutes";
		
	if(returnString == "12 hours")
		returnString = "0.5 days";
					
	return returnString;
}
function convertStringToTicks(string){
	var interval = string[0];
	var units = string[1];
	var ticks = 0; 
	if(units.indexOf("day",0) != -1)
		ticks = interval * 24 * 60 * 60 * 10000000;
	else if(units.indexOf("hour",0) != -1)
		ticks = interval * 60 * 60 * 10000000;
	else if(units.indexOf("minute",0) != -1)
		ticks = interval * 60 * 10000000;
	return ticks;	
}

function cbAllDayEvent_Clicked()
{
	var cb = document.getElementById("cbAllDayEvent");
	var td1 = document.getElementById("startTime");
	var td2 = document.getElementById("endTime");

	if(cb.checked)
	{
		startTimeDropDown._elem.style.visibility = "hidden"; 
		startTimeDropDown._elem.style.display = "none";
		endTimeDropDown._elem.style.visibility = "hidden"; 
		endTimeDropDown._elem.style.display = "none";
		td1.style.display = "none";
		td1.style.visibility = "hidden";
		td2.style.display = "none";
		td2.style.visibility = "hidden";
	}
	else
	{
		startTimeDropDown._elem.style.visibility = ""; 
		startTimeDropDown._elem.style.display = "";
		endTimeDropDown._elem.style.visibility = ""; 
		endTimeDropDown._elem.style.display = "";
		td1.style.display = "";
		td1.style.visibility = "";
		td2.style.display = "";
		td2.style.visibility = "";
	}
}



function cbReminder_Clicked()
{
	var reminder = document.getElementById("cbReminder");
	var ddreminder = document.getElementById("ddReminder");
	ddreminder.disabled = !reminder.checked;
}


