 /*
  * Infragistics WebSchedule CSOM Script: ig_webweekview.js
  * Version 5.3.20053.1096
  * Copyright(c) 2001-2006 Infragistics, Inc. All Rights Reserved.
  */




// WebWeekView prototype and constructor
function ig_CreateWebWeekView(props)
{
    if(!ig_WebControl.prototype.isPrototypeOf(ig_WebWeekView.prototype))
    {
        ig_WebWeekView.prototype = new ig_WebControl();
        ig_WebWeekView.prototype.constructor = ig_WebWeekView;
        ig_WebWeekView.prototype.base=ig_WebControl.prototype;
        
		ig_WebWeekView.prototype.init = function(props)
		{
			this._isInitializing=true;
			this._initControlProps(props);
			this.base.init.apply(this,[this._clientID]);
			this._element = ig_shared.getElementById(this._clientID);
			this._days = new Array();
			this._selectedAppt = new Object();
			this._initElem(this._element);
			this._initActiveDay();
			this._setActiveDay = function(info, evnt, activeDay, id, smartCallback)
			{
				var me = this._event._object;
				if(!me || !activeDay || me._clientID == id)
					return;
				var day = activeDay.getFullYear() + "," + activeDay.getMonth() + "," + activeDay.getDate();
				var currentActiveDay = info.getActiveDay();
				var currentDay = currentActiveDay.getFullYear() + "," + currentActiveDay.getMonth() + "," + currentActiveDay.getDate(); 
				var pendingActiveDay = info._pendingActiveDay;
				if(pendingActiveDay != null)
					pendingActiveDay = pendingActiveDay.getFullYear() + "," + pendingActiveDay.getMonth() + "," + pendingActiveDay.getDate(); 
				else
					 pendingActiveDay = currentDay;
				if(me._days[day] && me._days[currentDay] && me._days[pendingActiveDay] )
					me._selectDay(day);
				else
				{
					evnt.needPostBack = true;
					if(smartCallback != null)
					{
						var serverContext = {operation:"ActiveDaySync", requestType:"html"};
						var clientContext = {operation:"ActiveDaySync", requestType:"html"};
						smartCallback.registerControl(clientContext, serverContext, null, me._uniqueID, me)
					}
				}
			}
			this._navigate = function(info, evnt, years, months, days, newActiveDay, id, smartCallback)
			{
				var me = this._event._object;
				if(!me || !newActiveDay)
					return;
				var day = newActiveDay.getFullYear() + "," + newActiveDay.getMonth() + "," + newActiveDay.getDate();
				if(me._days[day])
					me._selectDay(day, true);
				else
				{
					if(smartCallback != null)
					{
						var serverContext = {operation:"Navigate", requestType:"html"};
						var clientContext = {operation:"Navigate", Days:days, Months:months, Years:years};
						if(me._clientID == id)
							clientContext.CausedNavigate = true;
						smartCallback.registerControl(clientContext, serverContext, null, me._uniqueID, me)
					}
				}
			}
			this._appointmentModified = function(info, evnt, activity, id, smartCallback)
			{
				var me = this._event._object;
				
				if(smartCallback != null)
				{
					var serverContext = {operation:"ActivityUpdate", requestType:"html"};
					var clientContext = {operation:"ActivityUpdate"};
					smartCallback.registerControl(clientContext, serverContext, null, me._uniqueID, me)
				}
			}
			this.getWebScheduleInfo().addEventListener("InternalSetActiveDay", this._setActiveDay, this, false);
			this.getWebScheduleInfo().addEventListener("InternalNavigate", this._navigate, this, false);
			this.getWebScheduleInfo().addEventListener("InternalActivityUpdate", this._appointmentModified, this, false);
			ig_shared.addEventListener(this._element,"mousedown", ig_handleEvent);
			ig_shared.addEventListener(this._element,"mouseup", ig_handleEvent);
			ig_shared.addEventListener(this._element,"click", ig_handleEvent);
			ig_shared.addEventListener(this._element,"dblclick", ig_handleEvent);
			ig_shared.addEventListener(this._element,"select", ig_cancelEvent);
			ig_shared.addEventListener(this._element,"selectstart", ig_cancelEvent);
			this._isInitializing=false;
			this.fireEvent("initialize");
		}
		
		ig_WebWeekView.prototype._initActiveDay = function()
		{
			var i, day = this._activeDay;
			if(day && (day = this._days[day]) != null)
			{
				var classes = this.getActiveDayHeaderStyle().split(" ");
				var oldClass = day.headElem.className;
				for(i = 0; i < classes.length; i++)
					oldClass = oldClass.replace(classes[i], "");
				day.headOldClass = oldClass;
				classes = this.getActiveDayStyle().split(" ");
				oldClass = day.dayElem.className;
				for(i = 0; i < classes.length; i++)
					oldClass = oldClass.replace(classes[i], "");
				day.dayOldClass = oldClass;
			}
		}
		ig_WebWeekView.prototype._initElem = function(e)
		{
			var attr = e.getAttribute ? e.getAttribute("uie") : null;
			var i = (attr == "Day");
			if(i || attr == "DayHeader")
			{
				if(ig_shared.isEmpty(attr = e.getAttribute("date")))
					return;
				var d = this._days[attr];
				if(!d) d = this._days[attr] = new Object();
				if(i) d.dayElem = e;
				else d.headElem = e;
				if(e.id == this._clientID + "_ActiveDay")
					this._activeDay = attr;
				return;
			}
			var nodes = e.childNodes;
			if(nodes) for(i = 0; i < nodes.length; i++)
				this._initElem(nodes[i]);
		}
		
		ig_WebWeekView.prototype._onClick = function(src, evt)
		{
			if(this.fireEvent(evt.type, evt, src))
				return;
			var info = this.getWebScheduleInfo();
			src = ig_findElemWithAttr(src, "uie");
			var attr = (info && src) ? src.getAttribute("uie") : null;
			var i = (attr == "Next") ? 7 : ((attr == "Prev") ? -7 : 0);
			if(i != 0)
			{
				if(!this.fireEvent((i > 0) ? "navigatenext" : "navigateprevious", evt, i))
					info.navigate(0, 0, i, this._clientID);
				if(!ig_shared.IsIE)ig_cancelEvent(evt);
			}
		}		
		ig_WebWeekView.prototype._onMouseup = function(src, evt)
		{
			this.fireEvent(evt.type, evt, src);
		}
		ig_WebWeekView.prototype._onMousedown = function(src, evt)
		{
			var scheduleInfo = this.getWebScheduleInfo();
			if(src.tagName == "IMG")
				src = src.parentNode;
			if(this.fireEvent(evt.type, evt, src))
				return;
			var uie = src.getAttribute("uie");
			if(uie == "Appt")
			{
				var day = null, e = src;
				while(e)
				{
					if(e.getAttribute && ig_shared.notEmpty(day = e.getAttribute("date")))
						break;
					e = e.parentNode;
				}
				if(!e || !day || !this._selectDay(day))
					return;
				if(this._selectedAppt.elem != null)
					this._selectedAppt.elem.className = this._selectedAppt.oldClass;
				this._selectedAppt.elem = src;
				this._selectedAppt.key = src.getAttribute("ig_key");
				this._selectedAppt.oldClass = src.className;
				src.className -= this.getAllDayEventStyle();
				src.className +=  " " +  this.getSelectedAppointmentStyle();
			}
			this._selectDay(src.getAttribute("date"));
		}
		ig_WebWeekView.prototype._selectDay = function(dayID, setDay)
		{
			var day = this._days[dayID], oldDay = this._activeDay;
			if(!day)
				return false;
			if(this._activeDay == dayID)
				return true;
			if(oldDay) oldDay = this._days[oldDay];
			var scheduleInfo = this.getWebScheduleInfo();
			var date = dayID.split(",");
			if(!setDay)
				setDay = scheduleInfo.setActiveDay(new Date(date[0], date[1], date[2]), false, this._clientID);
			if(!setDay)
				return false;
			if(oldDay != null)
			{
				oldDay.dayElem.className = oldDay.dayOldClass;
				oldDay.headElem.className = oldDay.headOldClass;
				if(this._selectedAppt.elem != null)
				{
					this._selectedAppt.elem.className = this._selectedAppt.oldClass;
					this._selectedAppt.elem = null;
				}
			}
			this._activeDay = dayID;
			day.dayOldClass = day.dayElem.className;
			day.headOldClass = day.headElem.className;
			day.headElem.className +=  " " + this.getActiveDayHeaderStyle();
			day.dayElem.className += " " + this.getActiveDayStyle();
			if(this._selectedAppt.elem != null)
				this._selectedAppt.elem.className = this._selectedAppt.oldClass;
			return true;
		}
		ig_WebWeekView.prototype._onDblclick = function(src, evt)
		{
			if(!src || !src.getAttribute || this.fireEvent(evt.type, evt, src))
				return;
			var scheduleInfo = this.getWebScheduleInfo();
			if(this.getEnableAutoActivityDialog())
			{
				if(src.tagName == "IMG")
					src = src.parentNode;
				var uie = src.getAttribute("uie");
				if(uie == "Appt")
				{
					var prevOccur = src.getAttribute("ig_prevOccur"); 
					var nextOccur = src.getAttribute("ig_nextOccur"); 
					var occur = src.getAttribute("ig_occur");
					var key = src.getAttribute("ig_key");
					scheduleInfo._showAppointmentDialog(key, this._clientID, occur, prevOccur, nextOccur) 
				}
				var date = (uie == "DayHeader" || uie == "Day") ? src.getAttribute("date") : null;
				if(!date)
					return;
				date = date.split(",");
				var dateTime = new Date();
				dateTime.setFullYear(date[0], date[1], date[2]);
				var minutes = dateTime.getMinutes();
				if(minutes < 30)
					dateTime.setMinutes(30);
				else if(minutes > 30)
					dateTime.setHours(dateTime.getHours() + 1,0);
				var appointment = scheduleInfo.getActivities().createActivity();
				appointment.setStartDateTime(dateTime);
				scheduleInfo.showAddAppointmentDialog(appointment, this._clientID);
			}
		}
		ig_WebWeekView.prototype.getSelectedActivity = function()
		{
			if(this._selectedAppt == null || this._selectedAppt.elem == null)
				return null;
			var activities = this.getWebScheduleInfo().getActivities();
			return activities.getItemFromKey(this._selectedAppt.key);
		}
        ig_WebWeekView.prototype.getCaptionHeaderVisible = function() {
           return this._props[2];
        }  
         
        ig_WebWeekView.prototype.getEnableAutoActivityDialog = function() {
           return this._props[3];
        }  

        ig_WebWeekView.prototype.getWebScheduleInfoID = function() {
           return this._props[4];
        }  
		ig_WebWeekView.prototype.getWebScheduleInfo = function()
		{
			return ig_getWebControlById(this._props[4]);
		}
        ig_WebWeekView.prototype.getWeekViewFrameStyle = function() {
           return this._props[5];
        }  
        ig_WebWeekView.prototype.getCaptionHeaderStyle = function() {
           return this._props[6];
        }
        ig_WebWeekView.prototype.getWeekViewDayAreaStyle = function() {
           return this._props[7];
        }
        ig_WebWeekView.prototype.getNavigationAnimation = function() {
           return this._props[8];
        }
        ig_WebWeekView.prototype.getActiveDayHeaderStyle = function() {
           return this._props[9];
        }
        ig_WebWeekView.prototype.getActiveDayStyle = function() {
           return this._props[10];
        } 
        ig_WebWeekView.prototype.getAllDayEventStyle = function() {
           return this._props[11];
        } 
        ig_WebWeekView.prototype.getAppointmentStyle = function() {
           return this._props[12];
        } 
        ig_WebWeekView.prototype.getSelectedAppointmentStyle = function() {
           return this._props[13];           
        }     
        ig_WebWeekView.prototype.getDayHeaderStyle = function() {
           return this._props[14];
        }
        ig_WebWeekView.prototype.getDayStyle = function() {
           return this._props[15];           
        }
        ig_WebWeekView.prototype.getTodayHeaderStyle = function() {
           return this._props[16];
        }
        ig_WebWeekView.prototype.getTodayStyle = function() {
           return this._props[17];           
        }
        ig_WebWeekView.prototype.callbackRender = function(html, context)
        {
			var animate = (context.operation == "Navigate" && context.CausedNavigate && this.getNavigationAnimation() > 0);
			html = html.replace(/\^/g, "\"");
	        var elem = this.getElement();
	        if(animate)
				var cloneElem = elem.cloneNode(true);
	        elem.innerHTML = html;
	        ig_dispose(this._days);
	        this._days = new Array();
	        this._initElem(elem);
	        
	        this._initActiveDay();
	        	
			if(animate)
			{						
				cloneElem.id += "_clone";
				
				if(this.viewport == null)
				{	
					this.viewport = new ig_viewport();
					this.viewport.createViewport(elem, ViewportOrientationEnum.Horizontal);
					this.viewport.transferPositionToDiv(elem,cloneElem);
					this.viewport.animate.increment = 50;
					this.viewport.animate.onEnd = this._afterSlide;
					this.viewport.animate._elemOrigWidth = elem.style.width; 
				}
				this.viewport.div.style.width = this.viewport.div.offsetWidth + "px";
				var direction = AnimationDirectionEnum.Left;
				if(context.Days > 0 || ((context.Years * 12) + context.Months) > 0)
					direction = AnimationDirectionEnum.Right;
				
				this.viewport.animate._newElem = elem;
				elem.style.width = this.viewport.div.style.width;
				this.viewport.animate._oldElem = cloneElem;
				cloneElem.style.width = elem.style.width;
					
				this.viewport.scroll(cloneElem, elem, direction, this.getNavigationAnimation());
				this.viewport.animate._viewDiv = this.viewport.div; 
			}
        }
        
        ig_WebWeekView.prototype._afterSlide = function()
        {
			this._oldElem.parentNode.removeChild(this._oldElem);
			this.getContainer().scrollLeft = 0; 
			this._newElem.style.width = "100%"; 
			this._viewDiv.style.width = this._elemOrigWidth; 
        }
	}
	return new ig_WebWeekView(props);
}

function ig_WebWeekView(props)
{
	if(arguments.length != 0)
		this.init(props);
}

// public: get object from ClientID or UniqueID
function ig_getWebWeekViewById(id)
{
	return ig_getWebControlById(id);
}
