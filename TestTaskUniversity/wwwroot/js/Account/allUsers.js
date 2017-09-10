$(document).ready(function () {
    var idUser, userRole,dialogRemoveUser, formRemove;
    $(document).on('click', '.editUser',
        function () {
            var id= this.dataset.id;
            window.location.href = window.location.origin + "/Account/UserEditAccount/" + id;
        });

    $(document).on('click', '.removeUser',
        function () {
            idUser = this.dataset.id;
            userRole = this.dataset.role;

            dialogRemoveUser.dialog("open");
        });

    dialogRemoveUser = $("#dialogRemoveUser").dialog({
        autoOpen: false,
        height: 300,
        width: 350,
        modal: true,
        buttons: [
            {
                text: "Yes",
                click: function () {
                    removeUser();
                }
            },
            {
                text: "Cancel",
                click: function () {
                    dialogRemoveUser.dialog("close");
                }
            }
        ],
        close: function () {
            formRemove[0].reset();
        },
        open: function () {
            $("#removeUser").empty();
            $.ajax({
                url: '/Account/GetUser',
                type: 'GET',
                cache: false,
                data: { id: idUser },
                dataType: 'json',
                success: function (data) {
                    $("#removeUser").append("<div>Name: " +
                        data.name +
                        "</div><div>Role: " +
                        userRole +
                        "</div><div>Age: " +
                        data.age +
                        "</div><div>Email: " +
                        data.email +
                        "</div><div>Login: " +
                        data.login +
                        "</div>");
                }
            });
        }
    });

    formRemove = dialogRemoveUser.find("form").on("submit",
        function (event) {
            event.preventDefault();
            removeUser();
        });

    function removeUser() {
        $.ajax({
            url: '/Account/RemoveUserAccount',
            async: true,
            data: { id: idUser },
            cache: false,
            type: "POST",
            success: function (data) {
                $('#alrtSucsess').text("Well done! Course was remove").removeClass("hidden");
                $(document).find('.col-md-12.col-sm-12.col-xs-12.profile_details.' + idUser).remove();
                
                console.log("/Account/RemoveUserAccount- sucses");
            },
            error: function (xhr) {
                console.log("/Account/RemoveUserAccount- fail");
            },
            complete: function (data) {
                dialogRemoveUser.dialog("close");
            }
        });
    }
});