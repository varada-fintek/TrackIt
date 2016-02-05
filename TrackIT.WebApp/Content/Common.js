

//for create new hide and show
function show() {
    document.getElementById("form1").style.display = "block";
    document.getElementById("createnew").style.display = "none";      
}
function hide() {
    document.getElementById("form1").style.display = "none";
    document.getElementById("createnew").style.display = "block";
}
//Jq Grid funtion stare

/////////////Jq Grid funtion start
$(window).triggerHandler('resize.jqGrid');//trigger window resize to make the grid get the correct size


//replace icons with FontAwesome icons like above
function updatePagerIcons(table) {
    var replacement =
    {
        'ui-icon-seek-first': 'ace-icon fa fa-angle-double-left bigger-140',
        'ui-icon-seek-prev': 'ace-icon fa fa-angle-left bigger-140',
        'ui-icon-seek-next': 'ace-icon fa fa-angle-right bigger-140',
        'ui-icon-seek-end': 'ace-icon fa fa-angle-double-right bigger-140'
    };
    $('.ui-pg-table:not(.navtable) > tbody > tr > .ui-pg-button > .ui-icon').each(function () {
        var icon = $(this);
        var $class = $.trim(icon.attr('class').replace('ui-icon', ''));

        if ($class in replacement) icon.attr('class', 'ui-icon ' + replacement[$class]);
    })
}
///////////Jq Grid funtion end


