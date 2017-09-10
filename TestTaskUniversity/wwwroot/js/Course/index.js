function initialise() {
    var dialogSelectTeacher, form, dialogRemoveCourse, formRemove;
    var idCourse;
    var btnAdd;

    $(document).on('click', '.edit',
        function () {
            idCourse = this.dataset.id;
            window.location.href = window.location.origin + "/Courses/Edit/" + idCourse;
        });

    $(document).on('click', '.delete',
        function () {
            idCourse = this.dataset.id;
            dialogRemoveCourse.dialog("open");
        });

    dialogRemoveCourse = $("#dialogRemoveCourse").dialog({
        autoOpen: false,
        height: 300,
        width: 350,
        modal: true,
        buttons: [
            {
                text: "Yes",
                click: function() {
                    removeCourse();
                }
            },
            {
                text: "Cancel",
                click: function() {
                    dialogRemoveCourse.dialog("close");
                }
            }
        ],
        close: function() {
            formRemove[0].reset();
        },
        open: function() {
            $("#removeCourse").empty();
            $.ajax({
                url: '/Courses/GetCourse',
                type: 'GET',
                cache: false,
                data: { id: idCourse },
                dataType: 'json',
                success: function(data) {
                    $("#removeCourse").append("<div>Name: " +
                        data.name +
                        "</div><div>Subject: " +
                        data.subject +
                        "</div><div>Hours: " +
                        data.hours +
                        "</div>");
                }
            });
        }
    });

    formRemove = dialogRemoveCourse.find("form").on("submit",
        function (event) {
            event.preventDefault();
            removeCourse();
        });

    function removeCourse() {
        $.ajax({
            url: '/Courses/Delete',
            async: true,
            data: { id: idCourse },
            cache: false,
            type: "POST",
            success: function (data) {
                $('#alrtSucsess').text("Well done! Course was remove").removeClass("hidden");
                $(document).find('.fa-close.' + idCourse).trigger('click');
                setTimeout(function () {
                        $('#alrtSucsess').addClass("hidden");
                    },
                    3000
                );
                console.log("/Courses/Delete- sucses");
            },
            error: function (xhr) {
                $('#alrtDanger').text("Ohs! Smith was wrong!").removeClass("hidden");
                setTimeout(function () {
                        $('#alrtDanger').addClass("hidden");
                    },
                    3000
                );
                console.log("/Courses/Delete- fail");
            },
            complete: function (data) {
                dialogRemoveCourse.dialog("close");
            }
        });
    }

    $(document).on('click', '.btnAddInMyList',
        function () {
            btnAdd = this;
            idCourse = this.dataset.id;
            dialogSelectTeacher.dialog("open");
        });

    dialogSelectTeacher = $("#dialog-form").dialog({
        autoOpen: false,
        height: 400,
        width: 350,
        modal: true,
        buttons: [
            {
                text: "Select",
                click: function () {
                    selectTheacher();
                }
            },
            {
                text: "Cancel",
                click: function () {
                    dialogSelectTeacher.dialog("close");
                }
            }
        ],
        close: function () {
            form[0].reset();
        },
        open: function () {
            $.ajax({
                url: '/Courses/GetTeachersForCourse',
                type: 'GET',
                cache: false,
                data: { id: idCourse },
                dataType: 'json',
                success: function (data) {
                    $("#seelectTheacher").empty();
                    $.each(data,
                        function (index) {
                            $("#seelectTheacher")
                                .append("<div class='radio'><label><input name='rb' type='radio' value=" +
                                data[index].id +
                                "> " +
                                data[index].name +
                                "</label></div>");
                        });
                }
            });
        }
    });

    form = dialogSelectTeacher.find("form").on("submit",
        function (event) {
            event.preventDefault();
            selectTheacher();
        });

    function selectTheacher() {
        var selectedId = $('input[name=rb]:checked', '#seelectTheacher').val();
        $.ajax({
            url: '/Courses/AddInStudentCourse',
            async: true,
            data: { idTheacher: selectedId, idCourse: idCourse },
            cache: false,
            type: "POST",
            success: function (data) {
                $('#alrtSucsess').text("Well done! Course was added in your list").removeClass("hidden");
                setTimeout(function () {
                    $('#alrtSucsess').addClass("hidden");
                },
                    3000
                );

                addTextAndButtonToCardStudent(btnAdd, 'add', idCourse);
                console.log("/Courses/AddInStudentCourse- sucses");
            },
            error: function (xhr) {
                $('#alrtDanger').text("Ohs! Smith was wrong!").removeClass("hidden");
                setTimeout(function () {
                    $('#alrtDanger').addClass("hidden");
                },
                    3000
                );

                console.log("/Courses/AddInStudentCourse- fail");
            },
            complete: function (data) {
                dialogSelectTeacher.dialog("close");
            }
        });
    }

    $(document).on('click', '.removeFromMyList',
        function () {
            var id = this.dataset.id;
            var button = this;
            $.ajax({
                url: '/Courses/RemoveFromStudentList',
                async: true,
                data: { id: id },
                cache: false,
                type: "POST",
                success: function (data) {
                    $('#alrtSucsess').text("Well done! Course was remove from your list").removeClass("hidden");
                    setTimeout(function () {
                        $('#alrtSucsess').addClass("hidden");
                    },
                        3000
                    );
                    addTextAndButtonToCardStudent(button, 'remove', id);

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
                },
                complete: function (data) {
                    dialog.dialog("close");
                }
            });

        });

    function addTextAndButtonToCardStudent(button, fromWhere, id) {
        var firstStringForAddSt =
            '<span class="text-success"><strong>In My List of Courses</strong></span><button class="removeFromMyList btn btn-danger" data-id="';
        var secondStringForAddSt = '"><i><span class="fa fa-trash-o"></span></i></button>';

        var firstStringForRemoveSt = '<button class="btnAddInMyList btn btn-warning" data-id="';
        var secondStringForRemoveSt = '">Add in My</button>';

        var butGroupId = '#buttonGroup_' + id;
        switch (fromWhere) {
            case 'add':
                button.remove();
                $(butGroupId).append(firstStringForAddSt + id + secondStringForAddSt);
                break;
            case 'remove':
                button.remove();
                $(butGroupId).find('.text-success').remove();
                $(butGroupId).append(firstStringForRemoveSt + id + secondStringForRemoveSt);
                break;

        }
    };



    $('#searchString').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/Courses/SearchString',
                type: 'GET',
                cache: false,
                data: request,
                dataType: 'json',
                success: function (data) {
                    response($.map(data,
                        function (item) {
                            return {
                                value: item + ""
                            }
                        }));
                }
            });
        },
        minLength: 1
    });



    $(document).on('click', '.btnWantRead',
        function () {
            var button = this;
            idCourse = this.dataset.id;
            $.ajax({
                url: '/Courses/AddInTeacherCourses',
                async: true,
                data: { idCourse: idCourse },
                cache: false,
                type: "POST",
                success: function (data) {
                    $('#alrtSucsess').text("Well done! Course was added in your list").removeClass("hidden");
                    setTimeout(function () {
                        $('#alrtSucsess').addClass("hidden");
                    },
                        3000
                    );
                    removeButtonFromCardTeacher(button, 'add', idCourse);
                    console.log("/Courses/AddInTeacherCourses- sucses");
                },
                error: function (xhr) {
                    $('#alrtDanger').text("Ohs! Smith was wrong!").removeClass("hidden");
                    setTimeout(function () {
                        $('#alrtDanger').addClass("hidden");
                    },
                        3000
                    );

                    console.log("/Courses/AddInTeacherCourses- fail");
                },
                complete: function (data) {
                    dialog.dialog("close");
                }
            });
        });

    $(document).on('click', '.btnDontWantRead',
        function () {
            var id = this.dataset.id;
            var button = this;
            $.ajax({
                url: '/Courses/RemoveFromTeacherList',
                async: true,
                data: { id: id },
                cache: false,
                type: "POST",
                success: function (data) {
                    $('#alrtSucsess').text("Well done! Course was remove from your list").removeClass("hidden");
                    setTimeout(function () {
                        $('#alrtSucsess').addClass("hidden");
                    },
                        3000
                    );
                    removeButtonFromCardTeacher(button, 'remove', id);

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
                },
                complete: function (data) {
                    dialog.dialog("close");
                }
            });

        });

    function removeButtonFromCardTeacher(button, fromWhere, id) {
        var firstStringForAddTeach = '<button class="edit btn btn-warning" data-id="';
        var secondStringForAddTeach = '">Edit</button><button class="delete btn btn-danger" data-id="';
        var thirdStringForAddTeach = '">Delete</button><button class="btnDontWantRead btn btn-default" data-id="';
        var fourthStringForAddTeach = '">Remove from my list</button>';

        var firstStringForRemoveTeach = ' <button class="btnWantRead btn btn-default" data-id="';
        var secondStringForRemoveTeach = '">I want read this course</button>';
        var butGroupId = '#buttonGroup_' + id;
        switch (fromWhere) {
            case 'add':
                button.remove();
                $(butGroupId).append(firstStringForAddTeach + id + secondStringForAddTeach + id + thirdStringForAddTeach + id + fourthStringForAddTeach);
                break;
            case 'remove':
                button.remove();
                $(butGroupId).find('.edit').remove();
                $(butGroupId).find('.delete').remove();
                $(butGroupId).append(firstStringForRemoveTeach + id + secondStringForRemoveTeach);
                break;


        }
    }
}
$(document).ready(function () {
    initialise();
});
