$(document).ready(function () {
    var idTeacher;
    $('.deleteTeacher').on('click',
        function () {
            var button = this;
            var courseId = $('#courseId').val();
            idTeacher = this.dataset.id;
            $.ajax({
                url: '/Courses/RemoveTeacherFromCourse',
                async: true,
                data: { idTeacher: idTeacher, courseId: courseId },
                cache: false,
                type: "POST",
                success: function (data) {
                    console.log("/Courses/RemoveTeacherFromCourse- sucses");
                },
                error: function (xhr) {
                   
                },
                complete: function (data) {
                    $(button).closest('tr').remove();
                }
            });
        });

});
