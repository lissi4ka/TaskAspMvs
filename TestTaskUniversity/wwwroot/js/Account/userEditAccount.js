$(document).ready(function () {
    var dialogRemoveAccount, formRemove;
    $(document).on('click', '.removeFromMyList',
        function () {
            var id = this.dataset.id;
            $.ajax({
                url: '/Courses/RemoveFromStudentList',
                async: true,
                data: { id: id },
                cache: false,
                type: "POST",
                success: function (data) {
                    $('#alrtSucsess').text("Well done! Course was remove from your list").removeClass("hidden");
                    $(document).find('.fa-close.' + id).trigger('click');
                    setTimeout(function () {
                        $('#alrtSucsess').addClass("hidden");
                    },
                        3000
                    );

                    console.log("/Courses/RemoveFromStudentList- sucses");
                },
                error: function (xhr) {
                    $('#alrtDanger').text("Ohs! Smith was wrong!").removeClass("hidden");
                    setTimeout(function () {
                        $('#alrtDanger').addClass("hidden");
                    },
                        3000
                    );
                    console.log("/Courses/RemoveFromStudentList- fail");
                }
            });

        });

    $(document).on('click', '.btnDontWantRead',
        function () {
            var id = this.dataset.id;
            $.ajax({
                url: '/Courses/RemoveFromTeacherList',
                async: true,
                data: { id: id },
                cache: false,
                type: "POST",
                success: function (data) {
                    $('#alrtSucsess').text("Well done! Course was remove from your list").removeClass("hidden");
                    $(document).find('.fa-close.' + id).trigger('click');
                    setTimeout(function () {
                            $('#alrtSucsess').addClass("hidden");
                        },
                        3000
                    );

                    console.log("/Courses/RemoveFromTeacherList- sucses");
                },
                error: function (xhr) {
                    $('#alrtDanger').text("Ohs! Smith was wrong!").removeClass("hidden");
                    setTimeout(function () {
                            $('#alrtDanger').addClass("hidden");
                        },
                        3000
                    );
                    console.log("/Courses/RemoveFromTeacherList- fail");
                }
            });

        });

    $(document).on('click', '.removeMyAccount',
        function() {
            dialogRemoveAccount.dialog("open");
        });

    dialogRemoveAccount = $("#dialogRemoveAccount").dialog({
        autoOpen: false,
        height: 200,
        width: 400,
        modal: true,
        buttons: [
            {
                text: "Yes",
                click: function () {
                    removeAccount();
                }
            },
            {
                text: "Cancel",
                click: function () {
                    dialogRemoveAccount.dialog("close");
                }
            }
        ],
        close: function () {
            formRemove[0].reset();
        }
    });

    formRemove = dialogRemoveAccount.find("form").on("submit",
        function (event) {
            event.preventDefault();
            removeAccount();
        });

    function removeAccount() {
        $.ajax({
            url: '/Account/RemoveUserAccount',
            async: true,
            cache: false,
            type: "POST",
            success: function (data) {
                dialogRemoveAccount.dialog("close");
                console.log("/Account/RemoveUserAccount");
                window.location.origin = "Home/Index";
            },
            error: function (xhr) {
                $('#alrtDanger').text("Ohs! Smith was wrong!").removeClass("hidden");
                setTimeout(function () {
                        $('#alrtDanger').addClass("hidden");
                    },
                    3000
                );
                console.log("Account/RemoveUserAccount");
            },
            complete: function (data) {
                dialogRemoveAccount.dialog("close");
            }
        });
    }
});
