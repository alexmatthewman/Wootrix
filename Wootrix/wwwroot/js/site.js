

//This function is for re-ordering the Magazines it only needs to be on the Company Admin's Magazine list page (CompanySegments/Index)
$(document).ready(function () {
        $('.OrderSelector').change(function () {            
        var newOrder = $('option:selected', $(this)).text();
        var seg = $('option:selected', $(this)).val();
        var combined = newOrder + "|" + seg;
        window.location.href = '/CompanySegments/ChangeOrder/' + combined;
        $.post("@Url.Action('ChangeOrder')", { id: combined }, function (data) {});
    });
});

//$("#OrderSelector").change(function () {
//    alert("Start");
//    var selectedCountry = $("#Countries").val();
//    var newOrder = $('option:selected', $(this)).text();
//    var id = $('option:selected', $(this)).val();
//    alert("New Order is: " + newOrder + " and segmentID: " + id);
//    $.post('@Url.Action("ChangeOrder", "CompanySegments")', { order: newOrder, segmentID: id }, function (data) {
//    });
//});


//$("#CompanySelector").change(function () {
//    var companyID = $('option:selected', $(this)).val();
//    window.location.href = '/CompanyDepartments/Index/' + companyID;
//});


//$("#CompanyChoose").change(function () {
    
//    var companyID = $('option:selected', $(this)).val();
//    var companyName = $('option:selected', $(this)).text();
//   // alert("CompanyID:" + companyID + "  CompanyName:" + companyName);

//    window.location.href = '/Users/Create/' + companyID; 
//});



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

