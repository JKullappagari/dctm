// JScript File
 function getConfirm()
    {
        if(confirm('Are you sure you want to remove?'))
            return true;
        else
            return false;
    }
    
 function getRejectConfirm()
    {
        if(confirm('Are you sure you want to Reject?'))
            return true;
        else
            return false;
    }
    
function CheckNumber(){
    if (event.keyCode < "48" || event.keyCode > "57"){
	    //alert("Please check. Enter only numbers!");
	    return (false);
    }
	    return (true);
    }

    // onkeypress="return CheckNumber()" 
function Checkdot(){
    if (event.keyCode > "57"){
        //alert("Please check. Enter numbers greater than zero!");                    
        return (false);
    }
         return (true);
    }
    //onblur="return Checkdot()"

function CheckDecNumber(){
    if (event.keyCode < "48" || event.keyCode > "57")
    {
	    //alert("Please check. Enter only numbers!");
	    if (event.keyCode==46)
	    {
	        return (true);
	    }
	    else
	    {
	        return (false);
	    }    
    }
	    return (true);
    }

function checkDecimal(obj)
{
   if (event.keyCode < "48" || event.keyCode > "57")
    {
	    if (event.keyCode!=46)
	    {
	        return (false);
	    }    
    }
    
    var fieldValue;
    var j = 0;
   
    fieldValue = obj.value;
    
    for (i=0;i<fieldValue.length;i++)
       if (fieldValue.charAt(i) == ".")
            j++;
   
    if (j == 1 && event.keyCode == 46)
        return false;
    else
    {
        var index = fieldValue.indexOf(".")
        if (index > -1)
        {
            var str = fieldValue.substr(index+1, fieldValue.length)
            if (str.length > 1)
                return false
        }
    }
    return true;
}


function MM_swapImgRestore() { //v3.0
  var i,x,a=document.MM_sr; for(i=0;a&&i<a.length&&(x=a[i])&&x.oSrc;i++) x.src=x.oSrc;
}

function MM_preloadImages() { //v3.0
  var d=document; if(d.images){ if(!d.MM_p) d.MM_p=new Array();
    var i,j=d.MM_p.length,a=MM_preloadImages.arguments; for(i=0; i<a.length; i++)
    if (a[i].indexOf("#")!=0){ d.MM_p[j]=new Image; d.MM_p[j++].src=a[i];}}
}

function MM_findObj(n, d) { //v4.01
  var p,i,x;  if(!d) d=document; if((p=n.indexOf("?"))>0&&parent.frames.length) {
    d=parent.frames[n.substring(p+1)].document; n=n.substring(0,p);}
  if(!(x=d[n])&&d.all) x=d.all[n]; for (i=0;!x&&i<d.forms.length;i++) x=d.forms[i][n];
  for(i=0;!x&&d.layers&&i<d.layers.length;i++) x=MM_findObj(n,d.layers[i].document);
  if(!x && d.getElementById) x=d.getElementById(n); return x;
}

function MM_swapImage() { //v3.0
  var i,j=0,x,a=MM_swapImage.arguments; document.MM_sr=new Array; for(i=0;i<(a.length-2);i+=3)
   if ((x=MM_findObj(a[i]))!=null){document.MM_sr[j++]=x; if(!x.oSrc) x.oSrc=x.src; x.src=a[i+2];}
}
function window_onload() {
        window.setTimeout( "window_onload()", 1000 );
		t = new Date();
//		if(lblTimer != null){					
//			lblTimer.innerText = t.toDateString() + " " + t.toTimeString();
//			lblTimer.title = t.toString();			
//		}
//		else
//		{   
//			self.status = t.toString();
//		}
}
function OpenPopupPage (pageUrl, controlID, isPostBack)
{
    popUp=window.open(pageUrl+'?controlID='+controlID+'&isPostBack='+ isPostBack,'popupcal', 'width=250,height=300,left=200,top=250'); 
}
		                         
function SetControlValue(controlID, newDate, isPostBack)
{
    //debugger
    popUp.close();
    document.forms[0].elements[controlID].value=newDate;
    __doPostBack(controlID,'');
}

function SelectAllCheckboxes(spanChk, name){

   
   // Added as ASPX uses SPAN for checkbox
   var oItem = spanChk.children;
   var sItemName = new String();
   var theBox= (spanChk.type=="checkbox") ? 
        spanChk : spanChk.children.item[0];
   xState=theBox.checked;
   elm=theBox.form.elements;

   for(i=0;i<elm.length;i++)
     if(elm[i].type=="checkbox" && 
              elm[i].id!=theBox.id)                
     {
       sItemName = elm[i].name;
       if (sItemName.indexOf(name,0) >= 0)
       {
        if(elm[i].checked!=xState)
            elm[i].checked= xState;
       }
     }
 }


function SelectItemCheckbox(spanChk,name){

   // Added as ASPX uses SPAN for checkbox
   var theBox;
   var IsChecked = false;
   var elm;
   
   var icheck=0;
   var iuncheck=0;
   var icount=0;

   //Commented - 11-Nov-2006
   //alert("fdsfsdf");
   var sItemName = new String();      
   
   elm=spanChk.form.elements;
    
   for(i=0;i<elm.length;i++)
     
     if(elm[i].type=="checkbox")                
     {
       sItemName = elm[i].name;
       if (sItemName.indexOf(name,0) >= 0)
       {
                theBox=elm[i];       
       }
     }
     elm=theBox.form.elements;
    
    for(i=0;i<elm.length;i++)
     
    if(elm[i].type=="checkbox"&& 
              elm[i].id!=theBox.id)                

     {
       sItemName = elm[i].name;
       if (sItemName.indexOf("chkDelete",0) >= 0)
       {
              if(elm[i].checked == true)
                {
                    IsChecked=true;
                    icheck+=1;   
                    icount+=1;
                }
                else
                {
                    IsChecked=false;
                    iuncheck+=1;
                    icount+=1;
                }
           
       }
     }
  
     if (icount==icheck)  
        theBox.checked=true;
    else
    theBox.checked=false;
}
function SelectItemCheckboxInfragistics(spanChk,name){

   // Added as ASPX uses SPAN for checkbox
   var theBox;
   var IsChecked = false;
   var elm;
   
   var icheck=0;
   var iuncheck=0;
   var icount=0;
        
  //  alert("fdsfsdf");
   var sItemName = new String();      
   
   elm=spanChk.form.elements;
    
   for(i=0;i<elm.length;i++)
     
     if(elm[i].type=="checkbox")                
     {
       sItemName = elm[i].name;
       if (sItemName.indexOf(name,0) >= 0)
       {
                theBox=elm[i];       
       }
     }
     elm=theBox.form.elements;
    
    for(i=0;i<elm.length;i++)
     
    if(elm[i].type=="checkbox"&& 
              elm[i].id!=theBox.id)                

     {
       sItemName = elm[i].name;
       if (sItemName.indexOf("chkDelete",0) >= 0)
       {
              if(elm[i].checked == true)
                {
                    IsChecked=true;
                    icheck+=1;   
                    icount+=1;
                }
                else
                {
                    IsChecked=false;
                    iuncheck+=1;
                    icount+=1;
                }
           
       }
     }
  
     if (icount==icheck)  
        theBox.checked=true;
    else
    theBox.checked=false;
}

//15-Nov-2006
//Enable or Disable textbox based on radio button selection
function toggleMultiplier(o, tname, val)
{
    //var tBox = document.getElementsByTagName("txtMultiplier");
    //var tBox = document.getElementById("txtMultiplier");
    //tBox.disabled = !(lTextBox.disabled);
    //tBox.disabled = true;
    
    //if (val == "1")
    //    tBox.enabled = false;
            
    var elm = document.getElementsByTagName("input")
    var strPos
    var strText
    
    for(i=0;i<elm.length;i++)
    {
        if(elm[i].type=="text")                
        {
            strPos = elm[i].name.indexOf(tname);
            strText = elm[i].name.substring(strPos, strPos+tname.length);
            if (strText == tname)
                if (val == "1")
                    elm[i].disabled = false;
                else
                {
                    //elm[i].disabled = true;
                    elm[i].disabled = false;
                    //elm[i].value = 1;
                }
        }
    }
}
    
//Added on 11-Nov-2006
//New function created based on ValidateDeletion to show appropriate message
//Start - ValidateDeletionNew
function ValidateDeletionNew(oButton, oEvent, sMessage)
{
    var theBox;
    var IsChecked = false;
    var elm;

    var icheck=0;
    var iuncheck=0;
    var icount=0;
        
   
    var sItemName = new String();      
    var IsChecked = false;
    var elm = document.getElementsByTagName("input")
  
    
    for(i=0;i<elm.length;i++)
    {
        if(elm[i].type=="checkbox")                
        {
            sItemName = elm[i].name;
            if (sItemName.indexOf("chkAll",0) >= 0)
            {
                theBox=elm[i];       
            }
        }
    }
    
    elm=theBox.form.elements;

    for(i=0;i<elm.length;i++)
    {
        if(elm[i].type=="checkbox" && elm[i].id!=theBox.id)                
        {
            sItemName = elm[i].name;
            if (sItemName.indexOf("chkDelete",0) >= 0)
            {
                if(elm[i].checked == true)
                {
                    IsChecked=true;
                    icheck+=1;   
                    icount+=1;
                }
                else
                {
                    IsChecked=false;
                    iuncheck+=1;
                    icount+=1;
                }

            }
        }
    }
   
    if (icheck==0)
    {
        alert("Please select at least one item to delete");
        oEvent.cancel = true;
        //return false;
    }
    else
    {
        var cnf = window.confirm(sMessage);
        if (cnf)
            oEvent.cancel = false;
        else
            oEvent.cancel = true;
    }
 }
//End - ValidateDeletionNew

 //Start - ValidateDeletionNew
 function ValidateDeletionNew1() {
     var theBox;
     var IsChecked = false;
     var elm;

     var icheck = 0;
     var iuncheck = 0;
     var icount = 0;


     var sItemName = new String();
     var IsChecked = false;
     var elm = document.getElementsByTagName("input")


     for (i = 0; i < elm.length; i++) {
         if (elm[i].type == "checkbox") {
             sItemName = elm[i].name;
             if (sItemName.indexOf("chkAll", 0) >= 0) {
                 theBox = elm[i];
             }
         }
     }

     elm = theBox.form.elements;

     for (i = 0; i < elm.length; i++) {
         if (elm[i].type == "checkbox" && elm[i].id != theBox.id) {
             sItemName = elm[i].name;
             if (sItemName.indexOf("chkDelete", 0) >= 0) {
                 if (elm[i].checked == true) {
                     IsChecked = true;
                     icheck += 1;
                     icount += 1;
                 }
                 else {
                     IsChecked = false;
                     iuncheck += 1;
                     icount += 1;
                 }

             }
         }
     }

     if (icheck == 0) {
         alert("Please select at least one item to delete");
         return false;
     }
     else {
         var cnf = window.confirm("Do you wish to Delete the selected items ?");
         if (cnf)
             return true;
         else
             return false;
     }
 }
 //End - ValidateDeletionNew


 function ValidateDeletion(oButton,oEvent)
 {
    //call    ValidateCheckox(oButton,"chkDelete","Delete");
 

   var theBox;
   var IsChecked = false;
   var elm;
   
   var icheck=0;
   var iuncheck=0;
   var icount=0;
        
   
    var sItemName = new String();      
  var IsChecked = false;
  var elm = document.getElementsByTagName("input")
  
    
   for(i=0;i<elm.length;i++)
     
     if(elm[i].type=="checkbox")                
     {
       sItemName = elm[i].name;
       if (sItemName.indexOf("chkAll",0) >= 0)
       {
                theBox=elm[i];       
       }
     }
     elm=theBox.form.elements;
    

    for(i=0;i<elm.length;i++)
     
    if(elm[i].type=="checkbox"&& 
              elm[i].id!=theBox.id)                

     {
       sItemName = elm[i].name;
       if (sItemName.indexOf("chkDelete",0) >= 0)
       {
              if(elm[i].checked == true)
                {
                    IsChecked=true;
                    icheck+=1;   
                    icount+=1;
                }
                else
                {
                    IsChecked=false;
                    iuncheck+=1;
                    icount+=1;
                }
           
       }
     }
   

        if (icheck==0)
        {
            alert("Please select atleast one item to delete");
            return false;

        }
        else
        {
            var cnf = window.confirm("Do you wish to Delete the selected items ?");
            //alert(cnf);
            if (cnf)            
               oEvent.cancel = false ;
            else
               oEvent.cancel =  true;        
        }        
    
        
    
 }
 
function BUSiteRight(oButton)
 {

	//Add code to handle your event here.
	
   RightTransfer("lbAvSite","lbAsSite");
   
}

function BUSiteUserRight(oButton)
 {

	//Add code to handle your event here.
	
   RightTransfer("lbAvUser","lbAsUser");
   
}

function SiteLocationRight(oButton)
 {

	//Add code to handle your event here.
	
    RightTransfer("lbAvLoc","lbAsLoc");
   
}


function TradeSubTradeRight(oButton)
 {

	//Add code to handle your event here.
	
   RightTransfer("lbAvSubTrade","lbAsSubTrade");
   
}


function ModuleAccessRightsRight(oButton)
 {

	//Add code to handle your event here.
	
   RightTransfer("lbAvRights","lbAsRights");
   
}

function BUSiteAllRight(oButton)
 {

	//Add code to handle your event here.
	
   AllRightTransfer("lbAvSite","lbAsSite");
   
}

function BUSiteUserAllRight(oButton)
 {

	//Add code to handle your event here.
	
      AllRightTransfer("lbAvUser","lbAsUser");
   
}

function SiteLocationAllRight(oButton)
 {

	//Add code to handle your event here.
	
  AllRightTransfer("lbAvLoc","lbAsLoc");
   
}


function TradeSubTradeAllRight(oButton)
 {

	//Add code to handle your event here.
	
   AllRightTransfer("lbAvSubTrade","lbAsSubTrade");
   
}


function ModuleAccessRightsAllRight(oButton)
 {

	//Add code to handle your event here.
	
   AllRightTransfer("lbAvRights","lbAsRights");
   
}
function BUSiteAllLeft(oButton)
 {

	//Add code to handle your event here.
	
   AllLeftTransfer("lbAvSite","lbAsSite");
   
}

function BUSiteUserAllLeft(oButton)
 {

	//Add code to handle your event here.
	
      AllLeftTransfer("lbAvUser","lbAsUser");
   
}

function SiteLocationAllLeft(oButton)
 {

	//Add code to handle your event here.
	
   AllLeftTransfer("lbAvLoc","lbAsLoc");
   
}


function TradeSubTradeAllLeft(oButton)
 {

	//Add code to handle your event here.
	
   AllLeftTransfer("lbAvSubTrade","lbAsSubTrade");
   
}


function ModuleAccessRightsAllLeft(oButton)
 {

	//Add code to handle your event here.
	
   AllLeftTransfer("lbAvRights","lbAsRights");
   
}

function BUSiteLeft(oButton)
 {

	//Add code to handle your event here.
	
   LeftTransfer("lbAvSite","lbAsSite");
   
}

function BUSiteUserLeft(oButton)
 {

	//Add code to handle your event here.
	
      LeftTransfer("lbAvUser","lbAsUser");
   
}

function SiteLocationLeft(oButton)
 {

	//Add code to handle your event here.
	
   LeftTransfer("lbAvLoc","lbAsLoc");
   
}


function TradeSubTradeLeft(oButton)
 {

	//Add code to handle your event here.
	
   LeftTransfer("lbAvSubTrade","lbAsSubTrade");
   
}


function ModuleAccessRightsLeft(oButton)
 {

	//Add code to handle your event here.
	
   LeftTransfer("lbAvRights","lbAsRights");
   
}

function RetrieveButton(bName)
{
   var elm;
   var sItemName = new String();      
   var button;
   elm = document.getElementsByTagName("input");
   for(i=0;i<elm.length;i++)
   {
       if (elm[i].type=="button")
       {
            sItemName = elm[i].name;
            if (sItemName.indexOf(bName,0) >= 0) 
            {
                button=elm[i];       
                return button;
            }
        }
   }
}


function RightTransfer(name1,name2)
 {

	//Add code to handle your event here.
	
    var asLst;
   var avLst;
   var elm;
   var selectedOption;
   var icount=0;
   var sItemName = new String();      

   var bRight;
   var bLeft;
   var bAllRight;
   var bAllLeft;
   

  bRight = RetrieveButton("ibRight");
  bLeft = RetrieveButton("ibLeft");
  bAllRight = RetrieveButton("ibAllRight");
  bAllLeft = RetrieveButton("ibAllLeft");
  
   
   
   elm = document.getElementsByTagName("select");
   
   for(i=0;i<elm.length;i++)
   {
       sItemName = elm[i].name;
       if (sItemName.indexOf(name1,0) >= 0)
       {
                avLst=elm[i];       
       }
       if (sItemName.indexOf(name2,0) >= 0)
       {
                asLst=elm[i];       
       } 
     }
    if (avLst.options.length==0)
    {
        alert("Available List is Empty");
        return false;
    }
    icount=0;
    for (i=0;i<avLst.options.length;i++)
    {
        if (avLst.options[i].selected==true)
        {
            icount+=1;
        }
    }
    
    if (icount==0)
    {
        alert("Select atleast one item from the Available List");
        return false;
    }

    for (i=0;i<avLst.options.length;i++)
    {
        if (avLst.options[i].selected==true)
        {
            var newOption = new Option(avLst.options[i].text, avLst.options[i].value,  true, true);
             asLst.options.add(newOption);
        }
    }
    i=avLst.options.length-1;
    while (i>=0)
    {
        if (avLst.options[i].selected==true)
        {
             avLst.options[i] = null;
        }
        i = i-1;
    }

   for (i=0;i<avLst.options.length;i++)
    {
        if (avLst.options[i].selected==true)
        {
            avLst.options[i].selected=false;
        }
    }
   for (i=0;i<asLst.options.length;i++)
    {
        if (asLst.options[i].selected==true)
        {
            asLst.options[i].selected=false;
        }
    }

    
//    if (avLst.options.length > 0)
//    {
//        bRight.enabled = true;
//        bAllRight.enabled = true;
//    }
//    else
//    {
//        bRight.enabled = false;
//        bAllRight.enabled = false;
//    }
//    
//    
//    
//    
//    if (asLst.options.length > 0)
//    {
//        bLeft.enabled = true;
//        bAllLeft.enabled = true;
//    }
//    else
//    {
//        bLeft.enabled = false;
//        bAllLeft.enabled = false;
//    } 


//    
}


function LeftTransfer(name1,name2)
 {

	//Add code to handle your event here.
	
    var asLst;
   var avLst;
   var elm;
   var selectedOption;
   var icount=0;
   
 

   
   var sItemName = new String();      
   elm = document.getElementsByTagName("select");
   for(i=0;i<elm.length;i++)
   {
       sItemName = elm[i].name;
       if (sItemName.indexOf(name1,0) >= 0)
       {
                avLst=elm[i];       
       }
       if (sItemName.indexOf(name2,0) >= 0)
       {
                asLst=elm[i];       
       } 
     }
    if (asLst.options.length==0)
    {
        alert("Assigned List is Empty");
        return false;
    }
    icount=0;
    for (i=0;i<asLst.options.length;i++)
    {
        if (asLst.options[i].selected==true)
        {
            icount+=1;
        }
    }
    
    if (icount==0)
    {
        alert("Select atleast one item from the Assigned List");
        return false;
    }

    for (i=0;i<asLst.options.length;i++)
    {
        if (asLst.options[i].selected==true)
        {
            var newOption = new Option(asLst.options[i].text, asLst.options[i].value,  true, true);
             avLst.options.add(newOption);
        }
    }
   
    i=asLst.options.length-1;
    while (i>=0)
    {
        if (asLst.options[i].selected==true)
        {
             asLst.options[i] = null;
        }
        i = i -1;
    }


   for (i=0;i<avLst.options.length;i++)
    {
        if (avLst.options[i].selected==true)
        {
            avLst.options[i].selected=false;
        }
    }
   for (i=0;i<asLst.options.length;i++)
    {
        if (asLst.options[i].selected==true)
        {
            asLst.options[i].selected=false;
        }
    }

}


function AllRightTransfer(name1,name2)
 {

	//Add code to handle your event here.
	
    var asLst;
   var avLst;
   var elm;
   var selectedOption;
   
   var icount=0;
   var sItemName = new String();      
   elm = document.getElementsByTagName("select");
   for(i=0;i<elm.length;i++)
   {
       sItemName = elm[i].name;
       if (sItemName.indexOf(name1,0) >= 0)
       {
                avLst=elm[i];       
       }
       if (sItemName.indexOf(name2,0) >= 0)
       {
                asLst=elm[i];       
       }
     }
    if (avLst.options.length==0)
    {
        alert("Available List is Empty");
        return false;
    }

    for (i=0;i<avLst.options.length;i++)
    {
        var newOption = new Option(avLst.options[i].text, avLst.options[i].value,  true, true);
         asLst.options.add(newOption);
    }
    i=avLst.options.length-1;
    while (i>=0)
    {
        avLst.options[i] = null;
        i = i-1;
    }

   for (i=0;i<asLst.options.length;i++)
    {
        if (asLst.options[i].selected==true)
        {
            asLst.options[i].selected=false;
        }
    }

}

function AllLeftTransfer(name1,name2)
 {

	//Add code to handle your event here.
	
    var asLst;
   var avLst;
   var elm;
   var selectedOption;
   var icount=0;
   


   var sItemName = new String();      
   elm = document.getElementsByTagName("select");
   for(i=0;i<elm.length;i++)
   {
       sItemName = elm[i].name;
       if (sItemName.indexOf(name1,0) >= 0)
       {
                avLst=elm[i];       
       }
       if (sItemName.indexOf(name2,0) >= 0)
       {
                asLst=elm[i];       
       } 
     }
    if (asLst.options.length==0)
    {
        alert("Assigned List is Empty");
        return false;
    }

    for (i=0;i<asLst.options.length;i++)
    {
        var newOption = new Option(asLst.options[i].text, asLst.options[i].value,  true, true);
         avLst.options.add(newOption);
    }
   
    i=asLst.options.length-1;
    while (i>=0)
    {
        asLst.options[i] = null;
        i = i-1;
    }

   for (i=0;i<avLst.options.length;i++)
    {
        if (avLst.options[i].selected==true)
        {
            avLst.options[i].selected=false;
        }
    }
    
    
}

//************TextBox - MultiLine - Maximum Length Validation - Start
// Keep user from entering more than maxLength characters
function doKeypress(control, maxLength){
    value = control.value;
     if(maxLength && value.length > maxLength-1){
          event.returnValue = false;
          maxLength = parseInt(maxLength);
     }
}
// Cancel default behavior
function doBeforePaste(control, maxLength){
     if(maxLength)
     {
          event.returnValue = false;
     }
}
// Cancel default behavior and create a new paste routine
function doPaste(control, maxLength){
    value = control.value;
     if(maxLength){
          event.returnValue = false;
          maxLength = parseInt(maxLength);
          var oTR = control.document.selection.createRange();
          var iInsertLength = maxLength - value.length + oTR.text.length;
          var sData = window.clipboardData.getData("Text").substr(0,iInsertLength);
          oTR.text = sData;
     }
}
//************TextBox - MultiLine - Maximum Length Validation - End


 function OpenPopup(pageUrl, controlID,  isPostBack)
 {	
 	var winSettings = "left=100px,top=100px,width=850px,height=370px,toolbar=0,resizable=1,menubar=0,status=0,scrollbars=1"	;
	var winUrl = pageUrl+'?controlID='+controlID+'&isPostBack='+ isPostBack
	popUp = window.open(winUrl,'PopupWindow', winSettings);
 }

 function PostBackData(controlID)
 {
    popUp.close();
    __doPostBack(controlID,'');
 }
 function Browseris () {
	var agt = navigator.userAgent.toLowerCase();
        this.osver = 1.0;
        if (agt)
        {
            var stOSVer = agt.substring(agt.indexOf("windows ") + 11);
	    this.osver = parseFloat(stOSVer);
        }
	this.major = parseInt(navigator.appVersion);
	this.nav = ((agt.indexOf('mozilla')!=-1)&&((agt.indexOf('spoofer')==-1) && (agt.indexOf('compatible')==-1)));
 	this.nav2 = (this.nav && (this.major == 2));
	this.nav3 = (this.nav && (this.major == 3));
	this.nav4 = (this.nav && (this.major == 4));
	this.nav6 = this.nav && (this.major == 5);
	this.nav6up = this.nav && (this.major >= 5);
	this.nav7up = false;
	if (this.nav6up)
	{
		var navIdx = agt.indexOf("netscape/");
		if (navIdx >=0 )
			this.nav7up = parseInt(agt.substring(navIdx+9)) >= 7;
	}
	this.ie = (agt.indexOf("msie")!=-1);
	this.aol = this.ie && agt.indexOf(" aol ")!=-1;
	if (this.ie)
		{
		var stIEVer = agt.substring(agt.indexOf("msie ") + 5);
		this.iever = parseInt(stIEVer);
		this.verIEFull = parseFloat(stIEVer);
		}
	else
		this.iever = 0;
	this.ie3 = ( this.ie && (this.major == 2));
	this.ie4 = ( this.ie && (this.major == 4));
	this.ie4up = this.ie && (this.major >=4);
	this.ie5up = this.ie && (this.iever >= 5);
	this.ie55up = this.ie && (this.verIEFull >= 5.5);
	this.ie6up = this.ie && (this.iever >= 6);
    this.win16 = ((agt.indexOf("win16")!=-1)
               || (agt.indexOf("16bit")!=-1) || (agt.indexOf("windows 3.1")!=-1)
               || (agt.indexOf("windows 16-bit")!=-1) );
    this.win31 = (agt.indexOf("windows 3.1")!=-1) || (agt.indexOf("win16")!=-1) ||
                 (agt.indexOf("windows 16-bit")!=-1);
    this.win98 = ((agt.indexOf("win98")!=-1)||(agt.indexOf("windows 98")!=-1));
    this.win95 = ((agt.indexOf("win95")!=-1)||(agt.indexOf("windows 95")!=-1));
    this.winnt = ((agt.indexOf("winnt")!=-1)||(agt.indexOf("windows nt")!=-1));
    this.win32 = this.win95 || this.winnt || this.win98 || 
                 ((this.major >= 4) && (navigator.platform == "Win32")) ||
                 (agt.indexOf("win32")!=-1) || (agt.indexOf("32bit")!=-1);
    this.os2   = (agt.indexOf("os/2")!=-1) 
                 || (navigator.appVersion.indexOf("OS/2")!=-1)  
                 || (agt.indexOf("ibm-webexplorer")!=-1);
    this.mac    = (agt.indexOf("mac")!=-1);
    this.mac68k = this.mac && ((agt.indexOf("68k")!=-1) || 
                               (agt.indexOf("68000")!=-1));
    this.macppc = this.mac && ((agt.indexOf("ppc")!=-1) || 
                               (agt.indexOf("powerpc")!=-1));
    this.w3c = this.nav6up;
}
var browseris = new Browseris();

