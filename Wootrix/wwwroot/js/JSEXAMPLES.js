
//Page Ready Function, listening for events etc.
$(document).ready(function () {



    //On Click Function
    //Look on site for a <li> tag with an id of ViewMobileOption and if the <a> tag inside that is clicked
    $("li#ViewMobileOption a").click(function () {
        //Do Stuff
        alert('here');
        //Or if you don't want a popup and just want the text to show under the firebug console menu:
        console.log('here');
    });


    //Look on site for an input with class of ContactSubmit and come here if clicked
    $("input.ContactSubmit").click(function () {
        //Call the function sendMail
        sendEmail();
    });



    $(".imgState").click(function () {
        //This is an important function for click events as it gets the tag which was clicked!
        var id = ($(this).attr('id'));
    });

    //Get text from an input
    var keyword = '';
    keyword = document.getElementById("searchTxt").value;
    keyword = document.getElementsByClassName("searchField")[0].value;


    //Used this to set an JQuery ui accordion to open according to which state was clicked on
    $("area.imgState").click(function () {
        var stateSelected = '';
        stateSelected = $(this).attr("alt");
        //Now we have the state clicked on, open the accordion
        //attributes aria-selected and aria-expanded must be set to true for the right state, false for the rest
        if (stateSelected == 'VIC') { $(".accordion").accordion("option", "active", 1); }
        if (stateSelected == 'TAS') { $(".accordion").accordion("option", "active", 2); }
        if (stateSelected == 'NSW') { $(".accordion").accordion("option", "active", 3); }
        if (stateSelected == 'ACT') { $(".accordion").accordion("option", "active", 4); }
        if (stateSelected == 'SA') { $(".accordion").accordion("option", "active", 5); }
        if (stateSelected == 'NT') { $(".accordion").accordion("option", "active", 6); }
        if (stateSelected == 'QLD') { $(".accordion").accordion("option", "active", 7); }
        if (stateSelected == 'WA') { $(".accordion").accordion("option", "active", 8); }
    });

    //Catching Keypresses
    $("input#top-nav-search").keypress(function (event) {
        //Event 13 is when the ENTER key has been pressed
        if (event.which == 13) {
            //When event.preventDefault() is called it stops the usual event function from happening. In this case pressing 
            // enter on a webpage would probably force it to refresh. If this was a link you had clicked on it would 
            // stop the link redirecting you.
            event.preventDefault();

            //Call another function but you can do whatever
            productSearch();

            //Returning false here is pretty much the same thing as preventDefault. It also calls e.stopPropagation() too though.
            return false;
        }
        return true;
    });


    //Hide more than x items of type y
    $(".classOfItemToCount:gt(4)").each(function () {
        $(this).hide();
    });



});

//Hide a single element
function hideStuff(id) {
    document.getElementById(id).style.display = 'none';
}

//Show a single hidden element
function showStuff(id) {
    document.getElementById(id).style.display = 'block !important';
}

//Change a CSS Element
function changeCSS(id) {
    document.getElementById(id).css('background', 'none');
    //NB this also works with something like $(this).css('background', 'none');
}

//Stand alone function (called from onclick even set up on page ready)
function sendEmail() {
    //Setup a new variable
    var formAction = '';

    //Build the URI seting for the page with a ton of arguments (can be picked up later)
    formAction += '/post.aspx?postmode=' + 'emailform';
    formAction += '&emailfrom=' + $("input#CVFormField_EmailAddress").val();
    formAction += '&mailto=' + $("input#mailto").val();
    formAction += '&Subject=' + $("input#Subject").val();
    //All these just grab the .val() for the input. That is whatever the value="xxxx" tag is set to in the html. 
    //This can either be predefined and put there in a hidden input or entered by the user
    formAction += '&EmailTemplate=' + $("input#EmailTemplate").val();
    formAction += '&RedirectTemplate=' + $("input#RedirectTemplate").val();
    //The best way to access something on the page is with the function in Jquery document.getElementById() - very fast!
    //So you could to that last one by: 
    formAction += '&RedirectTemplate=' + document.getElementById("RedirectTemplate").val();

}

function CheckCSSForSomeSetting() {
    //So below we find the element with ID tag MobileContainer and see if it has the CSS attribute 'display' and check if it is set to 'none'
    //You could similarly check to see if if the 'color' was  '#FFFFFF' or something else
    var $is_mobile = false;
    if ($('#MobileContainer').css('display') == 'none') {
        $is_mobile = true;
    };

}


//<p>
//  Once there was a <em title="huge, gigantic">large</em> dinosaur...
//</p>
//  The title of the emphasis is:<div></div>

function UsingAttr() {

    //So attr is a jquery function that can be used to grab a particular attribute of a HTML tag. So say instead of the value we need the title or name (or anything else) 
    //attribute of something.
    var title = $("em").attr("title");

    //Now we have extracted the title text we can use it somewhere:
    $("div").text(title);
    //The above would set the text attribute (print out the title text) in it's place.


    //Altering a value!
    //So say we want to alter the text of the em above in one hit?
    $("em").attr("title", "Fantasic coding genius learing JS");
    //The title tag would now be altered to have the above text.
}


//The below function is very useful as you call it on the page below passing custompage it would return WhereToBuyAU
//www.mackboots.com.au/custompage.aspx?custompage=WhereToBuyAU
function getURLParameter(name) {
    return decodeURI(
        (RegExp(name + '=' + '(.+?)(&|$)').exec(location.search) || [, null])[1]
    );
}

//Calling a method in another JS file we use a $.class.method(args) eg
function callAnotherMethod() {

    //The file is called jquery.url.js and has a param method insdie the url class :
    //jQuery.url = function()
    //{
    //    var param = function (item) {
    //    }
    //}

    if (typeof $.url.param("pageSizeProduct") != "undefined") {
        //Do something
        alert('here');
    }
}

//If you reference your css or js files like the below (with the V=130307) then the proxy and or the browser 
//will be forced to re-download the file when the page is refreshed. This is great for proxys that cache the files.
<link href="/documents/css/main.css?V=130307" rel="stylesheet" type="text/css">
<link href="/documents/css/pages.css?V=130307C" rel="stylesheet" type="text/css">
<link href="/documents/css/carousel.css?V=130307B" rel="stylesheet" type="text/css">


//The input below contains some JS to make IE display the placeholder 'Postcode'
<input class="location-search-postcode" placeholder="Postcode" accesskey="p" type="text" name="email" value="Postcode" 
onfocus="if (this.value == 'Postcode') { this.value = ''; }"
onblur="if (this.value == '') { this.value = 'Postcode'; }" 
onclick="if (this.value == 'Postcode') { this.value = ''; }" 
 />



 //Below is the browser toolbar dropdown with a redirect/login
 //Basically two buttons, send a browser url redirect based on inputs or saved 
javascript:(function(e,a,g,h,f,c,b,d){if(!(f=e.jQuery)||g>f.fn.jquery||h(f)){c=a.createElement("script");c.type="text/javascript";c.src="http://ajax.googleapis.com/ajax/libs/jquery/"+g+"/jquery.min.js";c.onload=c.onreadystatechange=function(){if(!b&&(!(d=this.readyState)||d=="loaded"||d=="complete")){h((f=e.jQuery).noConflict(1),b=1);f(c).remove()}};a.documentElement.childNodes[0].appendChild(c)}})(window,document,"1.8.2",function($,L){if($('#BookmarkletLoginContainer').length == 0){$('body').prepend('<div style="z-index:2000;height:30px;width:100%;background:#EEEEEE;display:none;color:#000000;font-size:14px;padding:5px 0 0 20px;font-weight:bold;font-family:Arial;position:absolute;top:0;left:0;" id="BookmarkletLoginContainer">Email Address <input type="text" id="BookmarkletLoginEmail" />&nbsp;&nbsp;&nbsp;&nbsp;Password: <input type="password" id="BookmarkletLoginPassword" />&nbsp;&nbsp;&nbsp;&nbsp;<input type="button" id="BookmarkletLoginButton" value="Login" />&nbsp;&nbsp;&nbsp;&nbsp;<input type="button" id="BookmarkletCVGlobalLoginButton" value="Login with Global Password" />&nbsp;&nbsp;&nbsp;&nbsp;<input type="button" id="BookmarkletCVSupportLoginButton" value="CVSupport Login" /></div>');}$('#BookmarkletLoginContainer').animate({height:'toggle'},100,function(){});$('#BookmarkletLoginButton').unbind('click');$('#BookmarkletCVGlobalLoginButton').unbind('click');$('#BookmarkletCVSupportLoginButton').unbind('click');$('#BookmarkletLoginButton').click(function(){var newURL = window.location.protocol + '//' + window.location.host + '/post.aspx?postmode=login&postuserid=' + ($('#BookmarkletLoginEmail').val() == '' ? 'cvsupport' : $('#BookmarkletLoginEmail').val()) + '&postpassword=' + ($('#BookmarkletLoginPassword').val() == '' ? 'CommVisi1' : $('#BookmarkletLoginPassword').val());var redirectURL = '';location.href = newURL});$('#BookmarkletCVGlobalLoginButton').click(function(){var newURL = window.location.protocol + '//' + window.location.host + '/post.aspx?postmode=login&postuserid=' + ($('#BookmarkletLoginEmail').val() == '' ? 'cvsupport' : $('#BookmarkletLoginEmail').val()) + '&postpassword=V1s1on2013';var redirectURL = '';location.href = newURL});$('#BookmarkletCVSupportLoginButton').click(function(){var newURL = window.location.protocol + '//' + window.location.host + '/post.aspx?postmode=login&postuserid=cvsupport&postpassword=V1s1on2013';var redirectURL = '';location.href = newURL})});



