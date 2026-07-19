//Get XMLHTTP for IE or Mozilla
function getXmlHTTP(){
	if(typeof XMLHttpRequest!="undefined")
	{
		return new XMLHttpRequest();
		alert('inside scripppppptttt');
	}
	else if(typeof ActiveXObject != "undefined")
	{
		try{
			var xmlhttp= new ActiveXObject("Microsoft.XMLHTTP");
			return xmlhttp;
		}
		catch(e)
		{
			return null;
		}
	}
	return null;
}

//Flag we can set after we modify the Grid prototype, so that we only do it once.
var _updatedRecordChange=false;
//Change the _recordChange prototype to point to our new function, which can then call
//the original _recordChange, only if the type is not "ChangedCells".  This way, we don't
//keep these changes in memory - we don't need them.
function UpdateGridRecordChange(){
	if(_updatedRecordChange)return;
	_updatedRecordChange=true;
	//Keep track of the old function, we're just changing its name.
	igtbl_Grid.prototype._recordChangeEX=igtbl_Grid.prototype._recordChange;
	//Now we can poing _recordChange to a new function
	igtbl_Grid.prototype._recordChange=function(type, obj, value){
		//We want to record all changes except for ChangedCells updates, there is no need to track them.
		if(type!="ChangedCells" &&type!="AddedRows" && type!="DeletedRows"){
			this._recordChangeEX(type,obj,value);
		}
	}
}