// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    //debugger;

    var placeHolderElement = $('#modal-placeholder');

    $('button[data-toggle="ajax-modal"]').click(function (event) {

        var url = $(this).data('url');
        $.get(url).done(function (data) {
            placeHolderElement.html(data);
            placeHolderElement.find('.modal').modal('show');

        });
    });

    placeHolderElement.on('click', '[data-save="modal"]', function (event) {
        debugger;
        event.preventDefault();

        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');

        //to serialize if we have object in modal body
        //var dataToSend = form.serialize();

        var finalData = [];
        //get particular value from each table row and construct a object
        $('table > tbody  > tr').each(function (index, tr) {
            //console.log(index);
            let i = index + 1;
            var arrayElementList = $('#row' + i + ' input:not([type="checkbox"])' ).serializeArray();
            //console.log(arrayElementList);

            //dynamically create object
            var obj = {};
            arrayElementList.forEach(function (value) {
                obj[value["name"].split('.')[1]] = value["value"];
            });

            //Add checkbox property in the row
            $('form #row' + i + ' input:checkbox').each(function () {
                obj[this.name.split('.')[1]] = this.checked;
            });

            finalData.push(obj);


        });

        var dataToSend = finalData;
        //console.log(finalData);

        //placeHolderElement.find('.modal').modal('hide');

        //$.post(actionUrl, dataToSend).done(function (data) {
        //    placeHolderElement.find('.modal').modal('hide');
        //}).fail(function (error) {
        //    console.log(error);
        //    placeHolderElement.find('.modal').modal('hide');
        //});
        $.ajax({
            url: actionUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(dataToSend),
            success: function (data) {
                //replace with new body
                var newBody = $('.modal-body', data);
                placeHolderElement.find('.modal-body').replaceWith(newBody);

                // find IsValid input field and check it's value
                // if it's valid then hide modal window
                var isValid = newBody.find('[name="IsValid"]').val() == 'True';
                if (isValid) {
                    placeHolderElement.find('.modal').modal('hide');
                }
            },
            error: function (err) {
                console.log(err);
                placeHolderElement.find('.modal').modal('hide');
            }
        });
    });

    function objectifyForm(formArray) {
        //serialize data function
        var returnList = [];
        for (var i = 0; i < formArray.length; i++) {
            var returnArray = {};
            returnArray[formArray[i]['name']] = formArray[i]['value'];
            returnList.push(returnArray);
        }
        return returnList;
    }

});
