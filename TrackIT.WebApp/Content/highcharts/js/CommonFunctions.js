function Trim(strValue)
{
	return strValue.replace(/^\s*|\s(?=\s)|\s*$/g, "");
}
function Alert(Title, LocalMessage) {
    var elem = $(this).closest('.item');
    $.confirm({
        'title': Title,
        'message': LocalMessage.replace(/\n/gi, '<br>'),
        'buttons': {
            'Ok': {
                'class': 'blue',
                'action': function () {
                    elem.slideUp();
                }
            }
        }
    });
}