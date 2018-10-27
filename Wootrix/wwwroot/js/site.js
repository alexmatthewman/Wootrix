



/* Slide Menu functions START */

////Hide/Show tabs under the slide menu
//function slideMenuClick(clicked_id) {
//    var content = "content" + clicked_id;
//    $('#UserView section').css('display', 'none');
//    $(content).css('display', 'block');
//    document.getElementById(content).style.display = "block";
//}

$("#CompanySelector").change(function () {
    var companyID = $('option:selected', $(this)).val();
    window.location.href = '/CompanyDepartments/Index/' + companyID;
});


$("#CompanyChoose").change(function () {
    
    var companyID = $('option:selected', $(this)).val();
    var companyName = $('option:selected', $(this)).text();
   // alert("CompanyID:" + companyID + "  CompanyName:" + companyName);

    window.location.href = '/Users/Create/' + companyID;
    //@Session["CompanyID"] = companyName;
   // @Session["CompanyName"] = companyName;
    //sessionStorage.setItem("CompanyID", companyID);
    
    //var cseshID5 = '@Session["CompanyName"]';
    //alert(cseshID5:" + cseshID5);

////    $('#test1').text = sessionStorage.getItem("CompanyID");
//    //$('#test2').text = sessionStorage.getItem("CompanyName");
//    $('#test1').text("@HttpContext.Session.GetInt32('CompanyID')");
//    $('#test3').text('@Context.Session.GetInt32("CompanyID")');
//    $('#test4').text('@Context.Session.GetString("CompanyName")');
});

//var hidWidth;
//var scrollBarWidths = 40;

//var widthOfList = function () {
//    var itemsWidth = 0;
//    $('.list li').each(function () {
//        var itemWidth = $(this).outerWidth();
//        itemsWidth += itemWidth;
//    });
//    return itemsWidth;
//};

//var widthOfHidden = function () {
//    return (($('.wrapper').outerWidth()) - widthOfList() - getLeftPosi()) - scrollBarWidths;
//};

//var getLeftPosi = function () {
//    return $('.list').position().left;
//};

//var reAdjust = function () {
//    if (($('.wrapper').outerWidth()) < widthOfList()) {
//        $('.scroller-right').show();
//    }
//    else {
//        $('.scroller-right').hide();
//    }

//    if (getLeftPosi() < 0) {
//        $('.scroller-left').show();
//    }
//    else {
//        $('.item').animate({ left: "-=" + getLeftPosi() + "px" }, 'slow');
//        $('.scroller-left').hide();
//    }
//}

//reAdjust();

//$(window).on('resize', function (e) {
//    reAdjust();
//});


//$('.scroller-right').click(function () {

//    $('.scroller-left').fadeIn('slow');
//    $('.scroller-right').fadeOut('slow');

//    $('.list').animate({ left: "+=" + widthOfHidden() + "px" }, 'slow', function () {

//    });
//});

//$('.scroller-left').click(function () {

//    $('.scroller-right').fadeIn('slow');
//    $('.scroller-left').fadeOut('slow');

//    $('.list').animate({ left: "-=" + getLeftPosi() + "px" }, 'slow', function () {

//    });
//});    

/* Slide Menu functions END */

